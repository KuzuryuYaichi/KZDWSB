// TestDLL.cpp : 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#define TOEC_DLLAPI 
#include "../AISDLL/TOEC_AIS.h"
#include <windows.h>
#include <iostream>
//静态载入： 直接调用即可，不用函数指针。
//start:
//comment(lib, "../Debug/TOEC_AIS.lib)
//end


using namespace  std;
//定义函数指针
typedef int (*func)(unsigned char *, int, AIS_STRUCT * t);

int _tmain(int argc, _TCHAR* argv[])
{
	//测试数据
	unsigned char * buffer = new   unsigned char[25];
	buffer[0] = 0x7e;
	buffer[1] = 0x20;
	buffer[2] = 0x00;
	buffer[3] = 0xb0;
	buffer[4] = 0x09;
	buffer[5] = 0x1c;
	buffer[6] = 0x00;
	buffer[7] = 0x02;
	buffer[8] = 0x58;
	buffer[9] = 0x98;
	buffer[10] = 0x25;
	buffer[11] = 0x1d;
	buffer[12] = 0x68;
	buffer[13] = 0x69;
	buffer[14] = 0xc1;
	buffer[15] = 0x03;
	buffer[16] = 0x00;
	buffer[17] = 0x00;
	buffer[18] = 0x00;
	buffer[19] = 0x00;
	buffer[20] = 0x00;
	buffer[21] = 0x00;
	buffer[22] = 0x8e;
	buffer[23] = 0xd9;
	buffer[24] = 0x7e;
	
	//加载动态库;
	HMODULE hDll = LoadLibrary(TEXT("AISDLL.dll"));
	if(hDll == NULL)
	{
		std::cout << "Cannot Find DLL!";
		return 0;
	}
	func Function = (func)GetProcAddress(hDll, "PraseAISData");

	//解析AIS数据;
	AIS_STRUCT data;
	int rc = Function(buffer, 25, &data);

	//解析结果打印;
	switch(rc)
	{
	case -1:
		cout<<"解析失败！"<<endl << "错误码:" << rc << endl <<"失败原因："<<"数据头错误！"<<endl;
		break;
	case -2:
		cout<<"解析失败！"<<endl << "错误码:" << rc << endl <<"失败原因："<<"CRC校验失败！"<<endl;
		break;
	case 0:
		cout<<"解析成功！"<<endl << "经度 ：" << data.dLongitude << endl <<"纬度 ：" << data.dLatitude<<endl;
		break;
	}

	
	FreeLibrary(hDll);
	char a;
	cin>>a;
	return 0;

}

