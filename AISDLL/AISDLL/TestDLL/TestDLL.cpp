// TestDLL.cpp : �������̨Ӧ�ó������ڵ㡣
//

#include "stdafx.h"
#define TOEC_DLLAPI 
#include "../AISDLL/TOEC_AIS.h"
#include <windows.h>
#include <iostream>
//��̬���룺 ֱ�ӵ��ü��ɣ����ú���ָ�롣
//start:
//comment(lib, "../Debug/TOEC_AIS.lib)
//end


using namespace  std;
//���庯��ָ��
typedef int (*func)(unsigned char *, int, AIS_STRUCT * t);

int _tmain(int argc, _TCHAR* argv[])
{
	//��������
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
	
	//���ض�̬��;
	HMODULE hDll = LoadLibrary(TEXT("AISDLL.dll"));
	if(hDll == NULL)
	{
		std::cout << "Cannot Find DLL!";
		return 0;
	}
	func Function = (func)GetProcAddress(hDll, "PraseAISData");

	//����AIS����;
	AIS_STRUCT data;
	int rc = Function(buffer, 25, &data);

	//���������ӡ;
	switch(rc)
	{
	case -1:
		cout<<"����ʧ�ܣ�"<<endl << "������:" << rc << endl <<"ʧ��ԭ��"<<"����ͷ����"<<endl;
		break;
	case -2:
		cout<<"����ʧ�ܣ�"<<endl << "������:" << rc << endl <<"ʧ��ԭ��"<<"CRCУ��ʧ�ܣ�"<<endl;
		break;
	case 0:
		cout<<"�����ɹ���"<<endl << "���� ��" << data.dLongitude << endl <<"γ�� ��" << data.dLatitude<<endl;
		break;
	}

	
	FreeLibrary(hDll);
	char a;
	cin>>a;
	return 0;

}

