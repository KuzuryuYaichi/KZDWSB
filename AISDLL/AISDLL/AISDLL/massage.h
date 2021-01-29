#ifndef _MASSAGE_H_
#define _MASSAGE_H_

#include "stdio.h"
//#include "TOEC_AIS.h"
//typedef  unsigned char unsigned char;
//typedef  unsigned short unsigned short;
#define EXPORT extern "C" __declspec(dllexport)
	

#pragma pack(1)

#define DECODE_MSG 1;

struct info_1_2_3
{
	unsigned char ID_info_code;
	unsigned char sendtimes;
	unsigned int User_id_code;
	unsigned char shipping_condition;
	signed char turning_speeed;
	short int speed_earth;
	unsigned char location_precision;
	int longitude;
	int latitude;
	short int course_earth;
	short int course_real;
	unsigned char mark_time;
	unsigned char region_keep;
	unsigned char standby;
	unsigned char mark_rami;
	int comm_condition;
};


struct info_4_11
	{
	unsigned char ID_info_code;
	unsigned char sendtimes;
	unsigned int User_id_code;
	short int UTC_year;
	unsigned char UTC_month;
	unsigned char UTC_date;
	unsigned char UTC_hour;
	unsigned char UTC_minter;
	unsigned char UTC_seconds;
	unsigned char location_precision; 
	int longitude; 
	int latitude;  
	unsigned char location_type;
	short int standby;
	unsigned char mark_rami;
	int comm_condition;     
	};



struct info_5
	{
	unsigned char ID_info_code;
	unsigned char sendtimes;
	unsigned int User_id_code;
	unsigned char version_AIS;
	unsigned int code_IMO;
	unsigned char calling[7];
	unsigned char ID[20];
	unsigned char ship_type;
	unsigned int scale;
	unsigned char location_type;
	unsigned int time_arrive;
	unsigned char draft_static;
	unsigned char destination[20];
	unsigned char date_terminal;
	unsigned char standby;
	};


struct info_6
	{
	unsigned char ID_info_code;
	unsigned char sendtimes;
	unsigned int ID_sender_MMSI;
	unsigned char serial_code;
	unsigned int ID_destination_MMSI;
	unsigned char mark_resend;
	unsigned char standby;
	unsigned char date_binary[156];
	};


struct info_7_13
	{
	unsigned char ID_info_code;
	unsigned char sendtimes;
	unsigned int ID_sender_MMSI;
	unsigned char standby;
	unsigned int ID_destination_MMSI_first;
	unsigned char serial_code_first;
	unsigned int ID_destination_MMSI_second;
	unsigned char serial_code_second;
	unsigned int ID_destination_MMSI_third;
	unsigned char serial_code_third;
	unsigned int ID_destination_MMSI_fourth;
	unsigned char serial_code_fourth;
	};



struct info_8
	{
	unsigned char ID_info_code;
	unsigned char sendtimes;
	unsigned int ID_sender_MMSI;
	unsigned char standby;
	unsigned char date_binary[121];
	};



struct info_9
	{
	unsigned char ID_info_code;
	unsigned char sendtimes;
	unsigned int ID_sender_MMSI;
	unsigned short int altitute;
	unsigned short int speed_earth;
	unsigned char location_precision;
	int longitude;
	int latitude;
	unsigned short int course_earth;
	unsigned char mark_time;
	unsigned char region_keep;
	unsigned char date_terminal;
	unsigned char standby;
	unsigned char mark_rami;
	unsigned int comm_condition;
	};



struct info_10
	{
	unsigned char ID_info_code;
	unsigned char sendtimes;
	unsigned int ID_sender_MMSI;
	unsigned char standby_1;
	unsigned int ID_destination_MMSI;
	unsigned char standby_2;
	};



struct info_12
	{
	unsigned char ID_info_code;
	unsigned char sendtimes;
	unsigned int ID_sender_MMSI;
	unsigned char serial_code;
	unsigned int ID_destination_MMSI;
	unsigned char mark_resend;
	unsigned char standby;
	unsigned char information_safety[156];
	};



struct info_14
	{
	unsigned char ID_info_code;
	unsigned char sendtimes;
	unsigned int ID_sender_MMSI;
	unsigned char standby;
	unsigned char information_safety[161];
	};



struct info_15
	{
	unsigned char ID_info_code;
	unsigned char sendtimes;
	unsigned int ID_sender_MMSI;
	unsigned char standby_1;
	unsigned int ID_destination_MMSI_first;
	unsigned char ID_information_code_1_1;
	short int time_offset_1_1;
	unsigned char standby_2;
	unsigned char ID_information_code_1_2;
	short int time_offset_1_2;	
	unsigned char standby_3;	
	unsigned int ID_destination_MMSI_second;
	unsigned char ID_information_code_2_1;	
	short int time_offset_2_1;
	unsigned char standby_4;
	};


struct info_16
	{
	unsigned char ID_info_code;
	unsigned char sendtimes;
	unsigned int ID_sender_MMSI;
	unsigned char standby_1;
	unsigned int ID_destination_MMSI_A;
	short int time_offset_A;
	short int incremental_A;
	unsigned int ID_destination_MMSI_B;
	short int time_offset_B;
	short int incremental_B;
	unsigned char standby_2;
	};


struct info_17
	{
	unsigned char ID_info_code;
	unsigned char sendtimes;
	unsigned int ID_sender_MMSI;
	unsigned char standby_1;
	int longitude;
	int latitude;
	unsigned char standby_2;
	unsigned char date[92];
	};


struct info_18
	{
	unsigned char ID_info_code;
	unsigned char sendtimes;
	unsigned int ID_sender_MMSI;
	unsigned char region_keep_1;
	unsigned short speed_earth;
	unsigned char location_precision;
	int longitude;
	int latitude;
	unsigned short course_earth;
	unsigned short course_real;
	unsigned char mark_time;
	unsigned char region_keep_2;
	unsigned char standby;
	unsigned char mark_rami;
	unsigned char mark_comm_condition;
	unsigned int comm_condition;
	};


struct info_19
	{
	unsigned char ID_info_code;
	unsigned char sendtimes;
	unsigned int ID_sender_MMSI;
	unsigned char region_keep_1;
	unsigned short speed_earth;
	unsigned char location_precision;
	int longitude;
	int latitude;
	unsigned short course_earth_COG;
	unsigned short course_real;
	unsigned char mark_time;
	unsigned char region_keep_2;
	unsigned char name[20];
	unsigned char ship_type;
	unsigned int scale;
	unsigned char location_type;
	unsigned char mark_rami;
	unsigned char date_terminal_condition;
	unsigned char standby;
	};


struct info_20
	{
	unsigned char ID_info_code;
	unsigned char sendtimes;
	unsigned int ID_sender_MMSI;
	unsigned char standby_1;
	short int time_offset_num_first;
	unsigned char time_num_first;
	unsigned char timeout_first;
	short int incremental_first;
	short int time_offset_num_second;
	unsigned char time_num_second;
	unsigned char timeout_second;
	short int incremental_second;                                                                           
	short int time_offset_num_third;
	unsigned char time_num_third;
	unsigned char timeout_third;
	short int incremental_third;
	short int time_offset_num_fourth;
	unsigned char time_num_fourth;
	unsigned char timeout_fourth;
	short int incremental_fourth;
	unsigned char standby_2;
	};


struct info_21
	{
	unsigned char ID_info_code;
	unsigned char sendtimes;
	unsigned int ID_sender_MMSI;
	unsigned char navigation_type;
	unsigned char navigation_name[20];
	unsigned char location_precision;
	int longitude;
	int latitude;
	unsigned int scale;
	unsigned char location_type;
	unsigned char mark_time;
	unsigned char location_offset;
	unsigned char region_keep;
	unsigned char mark_rami;
	unsigned char standby;
	};


struct info_22
	{
	unsigned char ID_info_code;
	unsigned char sendtimes;
	unsigned int ID_sender_MMSI;
	unsigned char standby_1;
	short int channel_A;
	short int channel_B;
	unsigned char send_or_receive_mode;
	unsigned char power;
	int longitude_first;
	int latitude_first;
	int longitude_second;
	int latitude_second;
	unsigned char mark_broadcast_information;
	unsigned char bandwidth_A;
	unsigned char bandwidth_B;
	unsigned char excessive_area;
	unsigned int standby_2;
	};

struct info_23
{
	unsigned char ID_info_code;//6
	unsigned char sendtimes;//2
	unsigned int ID_sender_MMSI;//30
	unsigned char standby_1;//2备用
	int longitude_first;//18
	int latitude_first;//17
	int longitude_second;//18
	int latitude_second;//17

	unsigned char station_type;//4
	unsigned char ship_type;//8
	unsigned int standby_2;//22备用
	unsigned char send_or_receive_mode;//2
	unsigned char report_time;//4
	unsigned char silence_time;//4
	unsigned char standby_3;//6备用
};

//静态数据报告	
struct info_24_A
{
	unsigned char ID_info_code;//6
	unsigned char sendtimes;//2
	unsigned int ID_sender_MMSI;//30
	unsigned char selection_number;//2
	unsigned char name[20];//120
};

struct info_24_B
{
	unsigned char ID_info_code;//6
	unsigned char sendtimes;//2
	unsigned int ID_sender_MMSI;//30
	unsigned char selection_number;//2
	unsigned char ship_type;//8
	unsigned char seller_ID[7];//42
	unsigned char calling[7];//42
	unsigned int scale;
	unsigned char standby;
};

//消息25：单时隙二进制消息	最大168
struct info_25
{
	unsigned char ID_info_code;//6
	unsigned char sendtimes;//2
	unsigned int ID_sender_MMSI;//30
	unsigned char destination_instruction_sign;//1
	unsigned char binary_data_sign;//1
	unsigned int ID_destination_MMSI;//0或者30
	unsigned char date_binary[16];//98/128
};


//消息26：带有通信状态的多时隙的二进制消息
struct info_26
{
	unsigned char ID_info_code;//6
	unsigned char sendtimes;//2
	unsigned int ID_sender_MMSI;//30
	unsigned char destination_instruction_sign;//1
	unsigned char binary_data_sign;//1
	unsigned int ID_destination_MMSI;//0或者30
	unsigned char date_binary[14];//78/108
	unsigned char time_internal_2_add[28];//224     允许32bit的比特填充   或者u32 time_internal_2_add[7];
	unsigned char time_internal_3_add[28];//224     允许32bit的比特填充
	unsigned char time_internal_4_add[28];//224     允许32bit的比特填充
	unsigned char time_internal_5_add[28];//224     允许32bit的比特填充
	unsigned char communication_state_choose;//1
	unsigned int comm_condition;//19
};


struct recive_setting
{
	unsigned char ID_info_code;
	float AIS_receive_channal_1_frequence;
	float AIS_receive_channal_2_frequence;
	float ACARS_receive_channal_1_frequence;
	float ACARS_receive_channal_2_frequence;
	float ACARS_receive_channal_3_frequence;
	float ACARS_receive_channal_4_frequence;
	int attenuator_AIS1;
	int attenuator_AIS2;
	int attenuator_ACARS1;
	int attenuator_ACARS2;
	int attenuator_ACARS3;
	int attenuator_ACARS4;
	int standby;
};


struct work_mode
{
	unsigned char ID_info_code;
	float send_channal_frequence;
	unsigned char send_power;
	unsigned char device_type;
};

struct condition_report
{
	unsigned char ID_info_code;
	int ID_sender_MMSI;
	int code_IMO;
	unsigned char calling[8];
	unsigned char ID[20];
	unsigned char ship_type;
	int scale;
	unsigned char location_type;
	unsigned char time_arrive[3];
	unsigned char draft_static;
	unsigned char destination[20];
	unsigned char date_terminal;	
};
// //AIS消息输出结构体
//typedef  struct __ais_struct
//{
//	  long lUserID;                      //30bit 用户ID MMSI号码
//    char cpNation[20];                    //国籍	   
//	  long lIMO;                         //30bit IMO编号 1-999999999;0 = 不可用 = 默认
//	  char cpCallLetters[7];               //42bit 呼号 7 * 6 比特 ASCII 码,@@@@@@@ = 不可用 = 默认 
//	  char cpName[20];                      //120bit 船名 最长 20字符的 6比特 ASCII码,如表 44的规定                                           
//	  unsigned char bShipAndFreightType;           //8bit 船舶和货物类型 
//	  
//	  long  lETA;                      //20bit 估计到达时间  估计到达时间;MMDDHHMM UTC 
//	  //比特 19-16: 月; 1-12;0 = 不可用 = 默认 
//	  //比特 15-11: 天; 1-31;0 = 不可用 = 默认 
//	  //比特 10-6:  时; 0-23;24 = 不可用 = 默认 
//	  //比特 5-0:   分; 0-59;60 = 不可用 = 默认 
//	  
//	  float      fShipWater;                 //8bit静态吃水深度,1/10m为单位,255表示大于25.5m
//	  char     cpDestination[20];             //120bit 目的地 	 
//	  unsigned char bnavigationState;               //4bit导航状态
//	  //0 = 机航中,1 = 锚泊,2 = 未操作,3 = 有限操作性,4 = 受船舶吃水限制,
//	  //5 = 系泊,6 = 搁浅,7 = 从事捕捞,8 = 帆航中,9 = ??
//	  //????????,???? DG?HS? MP,??? IMO? C
//	  //????????(HSC)???,10 = ??????????,
//	  //???? DG?HS? MP,??? IMO? A????????
//	  //(WIG)???; 11-14 = ?????,15 = ??? = ??? 
//	  
//	  
//	  int iSOG;                                 //10bit地面航速
//	  //????,??? 1/10 ?(0-102.2?) 
//	  //1 023 = ???,1 022 = 102.2???? 
//	  
//	  
//	  double dLongitude;                  //28bit??   ? 1/10 000 min??????(?80?? = ?(???2??
//	                                      //?),? = ?(??? 2???)?181?6791AC0h) = ??? = ???)
//	  double dLatitude;                   //27bit?? ? 1/10 000 min??????(?0?? = ?(??? 2??
//	                                      //?),? = ?(??? 2???)? 91? (3412140h) =??? = ???)
//	  double dCOG ;                       //12bit???? ????,? 1/10???(0-3599)?3600 (E10h) =                                              //??? = ????3 601-4 095 ???? 
//	  int iVerityHeading;                 //9bit ???? ?(0-359)(511 ????? = ???)
//	  
//	  unsigned char  ucbType;              //AIS???
//	  int ichangdu;              //???
//	  int ikuandu;               //???
//}AIS_STRUCT;
	



#pragma pack()

void test_init(void);
void ADCandDDS_code_init(float dds, signed char adc1,signed char adc2,unsigned char channal);
void add_package(void * pbuff,int datelength); 
void send_date_to_pci(void);
 unsigned short BitReverse_16bit(unsigned short x);
unsigned char get_BYTE_data(unsigned char bufffer[],unsigned char start,unsigned char len);
short get_u16_data(unsigned char buffer[],unsigned char start,unsigned char len);
int get_u32_data(unsigned char buffer[],unsigned char start,unsigned char len);
void get_sting_data(unsigned char buffer[], char data[],unsigned char start,unsigned char len);
void initialization_condation(void);
void initialization_ais_struct(void);
void check_HDLC(unsigned char date[],unsigned char length);
void demode_ship_location(unsigned char *packet);
void demode_UTC_time(unsigned char *packet);
void demode_static_data(unsigned char *packet);
void demode_addressing(unsigned char *packet);
void demode_sefaty_comfirm(unsigned char *packet);
void demode_broadcast_binary(unsigned char *packet);
void demode_searchrescue_info(unsigned char *packet);
void demode_UTCtime_calling(unsigned char *packet);
void demode_addressing_safety(unsigned char *packet);
void demode_safety(unsigned char *packet);
void demode_calling(unsigned char *packet);
void demode_allocation_order(unsigned char *packet);
void demode_GNSS_calling(unsigned char *packet);
void demode_location_B(unsigned char *packet);
void demode_expand_locationB(unsigned char *packet);
void demode_datalink(unsigned char *packet);
void demode_navaids_report(unsigned char *packet);
void demode_channel_manage(unsigned char *packet);
void check_adc_code(unsigned char comd[]);
int handle_message(unsigned char type, unsigned char packet_len, unsigned char *packet,unsigned char *outData);
signed char tarnsform_6to_8(unsigned char dat);
signed char tarnsform_8to_6(unsigned char dat);
EXPORT int BitReverseData(unsigned char data[],int length);
//extern "C"_declspec() 
EXPORT int phase_ais_packet(unsigned char *packet, int packet_len,unsigned char data[]);
EXPORT int  phase_acars_packet(unsigned char *packet,  int packet_len,unsigned char data[]);
EXPORT void  check_AIS_massage(unsigned char *packet,  int packet_len,unsigned char data[]);
EXPORT int phase_ads_packet(unsigned char* packet,unsigned char len,unsigned char data[]);
EXPORT float GetPower(unsigned char flag, unsigned char  num[5]);
EXPORT int phase_mode_s_packet(unsigned char* packet, unsigned char len, unsigned char data[]);
int crc_24_calc(unsigned char * data,unsigned char len);

#endif /* MASSAGE.h */
