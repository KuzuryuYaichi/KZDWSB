#include "pch.h"

#include "dllexport.h"
#include "stream_dma.hpp"
#include "streaming_dma.h"
#include <array>
#include <numeric>
#include <thread>
#include <vector>

#define AIS_IDENTIFY "rev_00"
#define ADS_B_IDENTIFY "rev_01"

enum CARD_TYPE
{
	CARD_AIS_ACARS = 0,
	CARD_ADS_B
};

#define AIS_MASK (1 << CARD_AIS_ACARS)
#define ADS_B_MASK (1 << CARD_ADS_B)

alignas(128) std::array<uint32_t, array_size> write_data;
alignas(128) std::array<uint32_t, array_size> read_data = { { 0 } };

xdma_device* pdev[2] = { nullptr };

PDATA_CALLBACK Callback2A = nullptr, CallbackIFF = nullptr;

int OpenDevice()
{
    std::iota(std::begin(write_data), std::end(write_data), 0);

    const auto device_paths = get_device_paths(GUID_DEVINTERFACE_XDMA);
    if (device_paths.empty()) {
		return -1;
    }

	SetIsRunning(true);
	int res = 0;
	for (int i = 0; i < device_paths.size(); i++)
	{
		pdev[i] = new xdma_device(device_paths[i]);
		ReadThread(*pdev[i], Callback2A, CallbackIFF);
		if (device_paths[i].find(AIS_IDENTIFY) != std::string::npos)
			res |= AIS_MASK;
		else if (device_paths[i].find(ADS_B_IDENTIFY) != std::string::npos)
			res |= ADS_B_MASK;
	}
	return res;
}

int CloseDevice()
{
	SetIsRunning(false);
	for (int i =0; i < 2; i++)
	{
		if(pdev[i] != nullptr)
			delete pdev[i];
	}
    return 0;
}

int WriteData_AIS_Interrupt(unsigned char* data, int len)
{
	unsigned char* data2 = new unsigned char[20];
	for (int i = 0; i < 5; i++)
	{
		for (int j = 0; j < 4; j++)
		{
			data2[i * 4 + j] = data[4 * i + 4 - 1 - j];
		}
	}
	unsigned int* Data = (unsigned int*)data2;
	for (int i = 0; i < 5; ++i)
	{
		WriteUserRegister(0x14, Data[i]);
	}
	delete[] data2;
    return 0;
}

int WriteData_ADS_Interrupt(unsigned char* data, int len)
{	
	unsigned char* data2 = new unsigned char[20];
	for (int i=0;i< 5;i++ )
	{
		for (int j = 0; j < 4; j++)
		{
			data2[i*4 +j ] = data[4 * i + 4 -1- j];
		}	
	}
	unsigned int* Data = (unsigned int*)data2;
	for (int i = 0; i < 5; i++)
	{
		WriteUserRegister(0x14, Data[i]);
	}
	delete[] data2;
    return 0;
}

void RegisterCallBackAIS(PDATA_CALLBACK pfunc, unsigned char* pref)
{
    Callback2A = pfunc;
}

void RegisterCallBackADS(PDATA_CALLBACK pfunc, unsigned char* pref)
{
    CallbackIFF = pfunc;
}

int StopCallbackFunc()
{
    Callback2A = nullptr;
    CallbackIFF = nullptr;
	return 0;
}

void WriteUserRegister(long addr, unsigned int value)
{
	pdev[0]->write_user_register(addr, value);
}

unsigned int ReadUserRegister(long addr)
{
	return pdev[0]->read_user_register(addr);
}
