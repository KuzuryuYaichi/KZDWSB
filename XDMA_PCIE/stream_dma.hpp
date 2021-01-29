#pragma once

#include <string>
#include <thread>
#include <array>
#include <iostream>
#include <system_error>
#include <Windows.h>
#include <SetupAPI.h>

constexpr size_t array_size = 2048 / 4;

// ============ windows device handle ===========
struct device_file {
    HANDLE h;
    device_file();
    device_file(const std::string& path, DWORD accessFlags);
    ~device_file();
};

device_file::device_file()
{

}

device_file::device_file(const std::string& path, DWORD accessFlags) {
    h = CreateFile(path.c_str(), accessFlags, 0, NULL, OPEN_EXISTING,
        FILE_ATTRIBUTE_NORMAL, NULL);
    if (h == INVALID_HANDLE_VALUE) {
        throw std::runtime_error("CreateFile control failed: " + std::to_string(GetLastError()));
    }
}

device_file::~device_file() {
    CloseHandle(h);
}

// ============ XDMA device ==============
class xdma_device {
public:
    xdma_device();
    xdma_device(const std::string& device_path);
    // transfer data from Host PC to FPGA Card using SGDMA engine
    size_t write_to_engine(void* buffer, size_t size);
    // transfer data from FPGA Card to Host PC using SGDMA engine
    size_t read_from_engine(void* buffer, size_t size);
	void write_user_register(long addr, uint32_t value);
	uint32_t read_user_register(long addr);
private:
    device_file control;
    device_file h2c0;
    device_file c2h0;
    device_file user;
    uint32_t read_control_register(long addr);
	std::string device_path;
};

inline static uint32_t bit(uint32_t n) {
    return (1 << n);
}

inline static bool is_bit_set(uint32_t x, uint32_t n) {
    return (x & bit(n)) == bit(n);
}

xdma_device::xdma_device()
{

}

xdma_device::xdma_device(const std::string& device_path) :
    control(device_path + "\\control", GENERIC_READ | GENERIC_WRITE),
    user(device_path + "\\user", GENERIC_READ | GENERIC_WRITE),
    h2c0(device_path + "\\h2c_0", GENERIC_WRITE),
    c2h0(device_path + "\\c2h_0", GENERIC_READ),
	device_path(device_path){

    if (!is_bit_set(read_control_register(0x0), 15) || !is_bit_set(read_control_register(0x1000), 15)) {
        throw std::runtime_error("XDMA engines h2c_0 and/or c2h_0 are not streaming engines!");
    }
}

uint32_t xdma_device::read_control_register(long addr) {
    uint32_t value = 0;
    size_t num_bytes_read;
    if (INVALID_SET_FILE_POINTER == SetFilePointer(control.h, addr, NULL, FILE_BEGIN)) {
        throw std::runtime_error("SetFilePointer failed: " + std::to_string(GetLastError()));
    }
    if (!ReadFile(control.h, (LPVOID)&value, 4, (LPDWORD)&num_bytes_read, NULL)) {
        throw std::runtime_error("ReadFile failed:" + std::to_string(GetLastError()));
    }
    return value;
}

uint32_t xdma_device::read_user_register(long addr) {
    uint32_t value = 0;
    size_t num_bytes_read;
    if (INVALID_SET_FILE_POINTER == SetFilePointer(user.h, addr, NULL, FILE_BEGIN)) {
        throw std::runtime_error("SetFilePointer failed: " + std::to_string(GetLastError()));
    }
    if (!ReadFile(user.h, (LPVOID)&value, 4, (LPDWORD)&num_bytes_read, NULL)) {
        throw std::runtime_error("ReadFile failed:" + std::to_string(GetLastError()));
    }
    return value;
}

void xdma_device::write_user_register(long addr, uint32_t value) {
    size_t num_bytes_read;
    if (INVALID_SET_FILE_POINTER == SetFilePointer(user.h, addr, NULL, FILE_BEGIN)) {
        throw std::runtime_error("SetFilePointer failed: " + std::to_string(GetLastError()));
    }
    if (!WriteFile(user.h, (LPVOID)&value, 4, (LPDWORD)&num_bytes_read, NULL)) {
        throw std::runtime_error("ReadFile failed:" + std::to_string(GetLastError()));
    }
}

size_t xdma_device::write_to_engine(void* buffer, size_t size) {
    unsigned long num_bytes_written;
    if (!WriteFile(h2c0.h, buffer, (DWORD)size, &num_bytes_written, NULL)) {
        throw std::runtime_error("Failed to write to stream! " + std::to_string(GetLastError()));
    }
    return num_bytes_written;
}

size_t xdma_device::read_from_engine(void* buffer, size_t size) {
    unsigned long num_bytes_read;
    if (!ReadFile(c2h0.h, buffer, (DWORD)size, &num_bytes_read, NULL)) {
        throw std::runtime_error("Failed to read from stream! " + std::to_string(GetLastError()));
    }
    return num_bytes_read;
}

bool isRunning = true;

void SetIsRunning(bool state)
{
	isRunning = state;
}

void read(xdma_device& dev, void* lpBuffer2A, void* lpBufferIFF, const size_t size, const size_t block_size, PDATA_CALLBACK& CallBack2A, PDATA_CALLBACK& CallBackIFF) {

	char *buffer2A = (char*)lpBuffer2A;
	char *bufferIFF = (char*)lpBufferIFF;

	size_t base_offset_2A = 0, base_offset_IFF = 0;
    while (isRunning)
    {
		unsigned char buffer[2048];
		size_t bytes_remaining = size;
        try {
			static unsigned int last = 0, now = 0;
            while (bytes_remaining > 0) {
                const size_t offset =  size - bytes_remaining;
                const size_t bytes_to_read = bytes_remaining < block_size ? bytes_remaining : block_size;
                bytes_remaining -= dev.read_from_engine((char*)buffer + offset, bytes_to_read);
            }
			if (buffer[0] != 0x01 || buffer[1] != 0x23 || buffer[2] != 0x45 || buffer[3] != 0x67)
			{
				std::cout << "Error Head" << std::endl;
				continue;
			}
 			/*now = (buffer[8] << 24) | (buffer[9] << 16) | (buffer[10] << 8) | buffer[11];
			if(now != last + 1)
				std::cout << "Error Order Now: " << now << " Last: " << last << std::endl;
			last = now;*/

			if (buffer[127] == 0x01 || buffer[127] == 0x02)
			{
				memcpy(buffer2A + base_offset_2A, buffer, 2048);
				base_offset_2A += 2048;
			}
			else if (buffer[127] == 0x03)
			{
				memcpy(bufferIFF + base_offset_IFF, buffer, 2048);
				base_offset_IFF += 2048;
			}

			if (base_offset_2A > 2048 - 1)
			{
				base_offset_2A = 0;
				if (CallBack2A != nullptr)
					CallBack2A(buffer2A, 1);
			}
			else if (base_offset_IFF > 2048 - 1)
			{
				base_offset_IFF = 0;
				if (CallBackIFF != nullptr)
					CallBackIFF(bufferIFF, 1);
			}
        }
        catch (const std::exception& e) {
			std::cout << e.what() << std::endl;
        }
    }
}

struct paramStruct
{
	xdma_device *pDev;
	void *buffer2A;
	void *bufferIFF;
	int length;
	int block_length;
	PDATA_CALLBACK *CallBack2A;
	PDATA_CALLBACK *CallBackIFF;
};

DWORD WINAPI ThreadProc(LPVOID lpParam)
{
	struct paramStruct *param = (struct paramStruct *)lpParam;
	read(*param->pDev, param->buffer2A, param->bufferIFF, param->length, param->block_length, *param->CallBack2A, *param->CallBackIFF);
	return 0;
}

struct paramStruct Info;
char buffer2A[2048 * 2048], bufferIFF[2048 * 2048];

void ReadThread(xdma_device& dev, PDATA_CALLBACK& CallBack2A, PDATA_CALLBACK& CallBackIFF)
{
	Info.pDev = &dev;
	Info.block_length = 2048;
	Info.length = 2048;
	Info.buffer2A = (void*)buffer2A;
	Info.bufferIFF = (void*)bufferIFF;
	Info.CallBack2A = &CallBack2A;
	Info.CallBackIFF = &CallBackIFF;
    try {
		DWORD dwThreadID;
		CreateThread(NULL, 0, ThreadProc, &Info, 0, &dwThreadID);
    }
    catch (const std::exception& e) {

    }
}