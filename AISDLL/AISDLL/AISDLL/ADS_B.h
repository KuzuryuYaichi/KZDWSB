#ifndef __ADS_B_H
#define __ADS_B_H
#include "stdio.h"
#include "massage.h"


#pragma pack(1)
#define EXPORT extern "C" __declspec(dllexport)

typedef struct ads_b{
	unsigned char DF;
	unsigned char CA_CF;
	unsigned char AA[3];
	unsigned char ME[7];
	unsigned char PI[3];
}ADS_B;
//空中位置消息
typedef struct air_location{
	unsigned char type;
	unsigned char monitor_condition;
	unsigned char signal_RF;
	unsigned short hight;
	unsigned char time;
	unsigned char cpr;
	unsigned int latitude;
	unsigned int longitude;
}AIR_LOCATION;
//地表面位置消息
typedef struct earth
{
	unsigned char type;
	unsigned char working;
	unsigned char heading;
	unsigned char flight_path;
	unsigned char time;
	unsigned char cpr;
	unsigned int latitude;
	unsigned int longitude;
}EARTH;
typedef struct id_type
{
	unsigned char type;
	unsigned char RF_mode;
	unsigned char ID_1;
	unsigned char ID_2;
	unsigned char ID_3;
	unsigned char ID_4;
	unsigned char ID_5;
	unsigned char ID_6;
	unsigned char ID_7;
	unsigned char ID_8;

}ID_TYPE;

typedef struct air_speed
{
	unsigned char type;
	unsigned char sub_type;
	unsigned char aim_change;
	unsigned char IFR;
	unsigned char navigation_indeterminacy;

	unsigned char east_or_west;
	unsigned short east_or_west_speed;
	unsigned char south_or_north;
	unsigned short south_or_north_speed;

	unsigned char course_condition;
	unsigned short course;
	unsigned char airspeed_mode;
	unsigned short airspeed;

	unsigned char vertical_source;
	unsigned char vertical_speed_sign;
	unsigned short vertical_speed;
	unsigned char nop;
	unsigned char barometric_pressure_sign;
	unsigned char barometric_pressure;

}AIR_SPEED;

typedef struct aim_condition
{
	unsigned char type;
	unsigned char sub_type;
	unsigned char vertical_data_indicate;
	unsigned char aim_high_type;
	unsigned char compatible_sign;
	unsigned char high_performance;
	unsigned char vertical_mode;
	unsigned short aim_high;
	unsigned char horizontal_data_indicate;
	unsigned short course;
	unsigned char heading;
	unsigned char horizontal_mode;
	unsigned char NACP;
	unsigned char NIC;
	unsigned char SIL;
	unsigned char nop;
	unsigned char performance_code;
	unsigned char prior;
}AIM_CONDITION;

typedef struct working_condition
{
	unsigned char type;
	unsigned char sub_type;
	unsigned short cc_code;
	unsigned short om_mode;
	unsigned char MOPS;
	unsigned char NIC;
	unsigned char NAC;
	unsigned char BAQ_or_nop;
	unsigned char SIL;
	unsigned char NIC_or_TRK;
	unsigned char HRD;
	unsigned char nop;
}WORKING_CONDITION;

typedef struct test_msg
{
	unsigned char type;
	unsigned char sub_type;
	unsigned char test_data[6];
	unsigned short A_code;
}TEST_MSG;


typedef struct pause_air_condition
{
	unsigned char type;
	unsigned char sub_type;
	unsigned char urgency;
	unsigned char data[6];
}PAUSE_AIR_CONDITION;


typedef union rssi
{
	unsigned char rev_rssi[4];
	unsigned int power;
}RSSI;
unsigned short calc_ads_ais_power(unsigned char type,unsigned int rssi);
//void write_send_buff(unsigned char type, void * source_addr,unsigned char* power, unsigned char len);
void decode_air_location(void* power);
void decode_earth_location(void* power);
void decode_id_type(void* power);
void decode_airspeed(void* power);
void decode_aim_condition(void* power);
void decode_plane_working_condition(void* power);
void decode_test(void* power);
void decode_pause_air_condition(void* power);
void phase_ads_b_msg(unsigned char *packet,unsigned char len,unsigned char data[]);

//void phase_ads_b_msg(unsigned char *packet,unsigned char len,unsigned char data[]);
EXPORT int phase_ads_b_packet(unsigned char *packet, int len);

#endif
