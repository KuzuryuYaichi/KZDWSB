#pragma once


#ifdef TOEC_DLLAPI
#define TOEC_DLLAPI extern "C" __declspec(dllimport)
#else
#define TOEC_DLLAPI extern "C" __declspec(dllexport) 
#endif
#pragma pack(1)
typedef  struct __ais_struct
{
	long lUserID;                      //30bit 用户ID MMSI号码
	char cpNation[20];                    //国籍	   
	long lIMO;                         //30bit IMO编号 1-999999999;0 = 不可用 = 默认
	char cpCallLetters[7];               //42bit 呼号 7 * 6 比特 ASCII 码,@@@@@@@ = 不可用 = 默认 
	char cpName[20];                      //120bit 船名 最长 20字符的 6比特 ASCII码,如表 44的规定                                           
	unsigned char bShipAndFreightType;           //8bit 船舶和货物类型 

	long  lETA;                      //20bit 估计到达时间  估计到达时间;MMDDHHMM UTC 
	//比特 19-16: 月; 1-12;0 = 不可用 = 默认 
	//比特 15-11: 天; 1-31;0 = 不可用 = 默认 
	//比特 10-6:  时; 0-23;24 = 不可用 = 默认 
	//比特 5-0:   分; 0-59;60 = 不可用 = 默认 

	float      fShipWater;                 //8bit静态吃水深度,1/10m为单位,255表示大于25.5m
	char     cpDestination[20];             //120bit 目的地 	 
	unsigned char bnavigationState;               //4bit导航状态
	//0 = 机航中,1 = 锚泊,2 = 未操作,3 = 有限操作性,4 = 受船舶吃水限制,
	//5 = 系泊,6 = 搁浅,7 = 从事捕捞,8 = 帆航中,9 = ??
	//????????,???? DG?HS? MP,??? IMO? C
	//????????(HSC)???,10 = ??????????,
	//???? DG?HS? MP,??? IMO? A????????
	//(WIG)???; 11-14 = ?????,15 = ??? = ??? 
	int iSOG;                                 //10bit地面航速
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