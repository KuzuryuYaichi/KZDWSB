#pragma once


#ifdef TOEC_DLLAPI
#define TOEC_DLLAPI extern "C" __declspec(dllimport)
#else
#define TOEC_DLLAPI extern "C" __declspec(dllexport) 
#endif
#pragma pack(1)
typedef  struct __ais_struct
{
	long lUserID;                      //30bit �û�ID MMSI����
	char cpNation[20];                    //����	   
	long lIMO;                         //30bit IMO��� 1-999999999;0 = ������ = Ĭ��
	char cpCallLetters[7];               //42bit ���� 7 * 6 ���� ASCII ��,@@@@@@@ = ������ = Ĭ�� 
	char cpName[20];                      //120bit ���� � 20�ַ��� 6���� ASCII��,��� 44�Ĺ涨                                           
	unsigned char bShipAndFreightType;           //8bit �����ͻ������� 

	long  lETA;                      //20bit ���Ƶ���ʱ��  ���Ƶ���ʱ��;MMDDHHMM UTC 
	//���� 19-16: ��; 1-12;0 = ������ = Ĭ�� 
	//���� 15-11: ��; 1-31;0 = ������ = Ĭ�� 
	//���� 10-6:  ʱ; 0-23;24 = ������ = Ĭ�� 
	//���� 5-0:   ��; 0-59;60 = ������ = Ĭ�� 

	float      fShipWater;                 //8bit��̬��ˮ���,1/10mΪ��λ,255��ʾ����25.5m
	char     cpDestination[20];             //120bit Ŀ�ĵ� 	 
	unsigned char bnavigationState;               //4bit����״̬
	//0 = ������,1 = ê��,2 = δ����,3 = ���޲�����,4 = �ܴ�����ˮ����,
	//5 = ϵ��,6 = ��ǳ,7 = ���²���,8 = ������,9 = ??
	//????????,???? DG?HS? MP,??? IMO? C
	//????????(HSC)???,10 = ??????????,
	//???? DG?HS? MP,??? IMO? A????????
	//(WIG)???; 11-14 = ?????,15 = ??? = ??? 
	int iSOG;                                 //10bit���溽��
	//????,??? 1/10 ?(0-102.2?) 
	//1 023 = ???,1 022 = 102.2???? 


	double dLongitude;                  //28bit??   ? 1/10 000 min??????(?80?? = ?(???2??
	//?),? = ?(??? 2???)?181?6791AC0h) = ??? = ???)
	double dLatitude;                   //27bit?? ? 1/10 000 min??????(?0?? = ?(??? 2??
	//?),? = ?(??? 2???)? 91? (3412140h) =??? = ???)
	double dCOG ;                       //12bit???? ????,? 1/10???(0-3599)?3600 (E10h) =                                              //??? = ????3 601-4 095 ???? 
	int iVerityHeading;                 //9bit ???? ?(0-359)(511 ????? = ???)

	unsigned char  ucbType;              //AIS???
	int ichangdu;              //???
	int ikuandu;               //???
}AIS_STRUCT;
#pragma pack()


TOEC_DLLAPI int PraseAISData(unsigned char* buffer, int bufferLen, AIS_STRUCT * outData);
TOEC_DLLAPI void check_AIS_massage(int  type, unsigned char *packet, int pacekt_len,unsigned char data[]);