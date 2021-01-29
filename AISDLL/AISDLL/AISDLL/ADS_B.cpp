#include "stdafx.h"
#include "ADS_B.h"
#include "string.h"
#include "stdlib.h"
#include "massage.h"
#include "math.h"
#include "Power.h"
//extern unsigned int ads1_rssi[40];
//extern unsigned int ais_rssi[61];
unsigned char tmp_ads_buffer[20] = {0};
static RSSI rssi;
static double rssi_tmp = 0;
static unsigned int rss_m = 0;
unsigned int ads_b_power_out = 0;
unsigned char send_rssi = 0;
unsigned short send_rssy_code = 0;
unsigned short out_rssi = 0;
ADS_B ads_msg;
AIR_LOCATION air_my;
EARTH earth_my;
ID_TYPE id_my;
AIR_SPEED air_speed_my;
AIM_CONDITION aim_condition_my;
WORKING_CONDITION working_my;
TEST_MSG test_my;
PAUSE_AIR_CONDITION pause_my;
unsigned char sum_ads = 0;
unsigned char struct_num = 0;


extern unsigned char RJ45_COUNTER;
unsigned char ads_send_buff[300] = {0};
unsigned char ads_send_len = 0;

unsigned int write_ads_num = 0;
unsigned int phase_ads_num = 0;

extern unsigned int total_ads, total_ais, error_ais, total_gps, error_gps;
unsigned char SpiMark = 0;
extern struct RecviceInfo Recviceinfo;
unsigned short ads_power=0;

void write_send_buff(void * source_addr, unsigned char len,unsigned char data[])
{
	//	enum tcp_state state_temp = CLOSED;
	ads_send_len = 0;

	if(source_addr == NULL)
	{
		return;
	}

	if (len>200)
	{
		return;
	}

	data[0] = ads_msg.AA[0];  //Added AA to send buff
	data[1] = ads_msg.AA[1];
	data[2] = ads_msg.AA[2];
	memcpy(data+3,source_addr,len);  //change 3->6

	ads_send_len = len + 3;   //change 3->6

	if(ads_send_len >= 253)  //
	{
		return;
	}

}

void decode_air_location()
{
	unsigned char datelength;
	datelength=sizeof(AIR_LOCATION);
    memset(&air_my, 0, datelength);
	
	air_my.type = ((ads_msg.ME[0] & 0xf8) >> 3);
	air_my.monitor_condition = ((ads_msg.ME[0] & 0x06) >> 1);
	air_my.signal_RF = (ads_msg.ME[0] & 0x01);
	air_my.hight = (((unsigned short)ads_msg.ME[1] << 8) + (((unsigned short)ads_msg.ME[2] & 0xf0) >> 4));
	air_my.time = ((ads_msg.ME[2] & 0x08) >> 3);
	air_my.cpr = ((ads_msg.ME[2] & 0x04) >> 2);	
	air_my.latitude = ((unsigned int)(ads_msg.ME[2] & 0x03) << 15) + ((unsigned short)ads_msg.ME[3] << 7) + ((unsigned short)(ads_msg.ME[4] & 0xfe) >> 1);
	air_my.longitude = ((unsigned int)(ads_msg.ME[4] & 0x01) << 16) + ((unsigned short)ads_msg.ME[5] << 8) + ((unsigned short)ads_msg.ME[6]);
    
}
void decode_earth_location()
{
	unsigned char datelength;
	datelength=sizeof(EARTH);
  memset(&earth_my, 0, datelength);
	
	earth_my.type = ((ads_msg.ME[0] & 0xf8) >> 3);
	earth_my.working = (((unsigned short)ads_msg.ME[0] & 0x07) << 4) + (((unsigned short)ads_msg.ME[1] & 0xf0) >> 4);
	earth_my.heading = ((ads_msg.ME[1] & 0x08) >> 3);
	earth_my.flight_path = ((ads_msg.ME[1] & 0x07) << 4) + ((ads_msg.ME[2] & 0xf0) >> 4);
	earth_my.time = ((ads_msg.ME[2] & 0x08) >> 3);
	earth_my.cpr = ((ads_msg.ME[2] & 0x04) >> 2);
//	earth.latitude = ((unsigned short)(ads_msg.ME[2] & 0x03) << 15) + ((unsigned short)ads_msg.ME[3] << 7) + ((unsigned short)(ads_msg.ME[4] & 0xfe) >> 1);
//	earth.longitude = ((unsigned short)(ads_msg.ME[4] & 0x01) << 16) + ((unsigned short)ads_msg.ME[5] << 8) + ((unsigned short)ads_msg.ME[6]);
	earth_my.latitude = ((unsigned int)(ads_msg.ME[2] & 0x03) << 15) + ((unsigned short)ads_msg.ME[3] << 7) + ((unsigned short)(ads_msg.ME[4] & 0xfe) >> 1);
	earth_my.longitude = ((unsigned int)(ads_msg.ME[4] & 0x01) << 16) + ((unsigned short)ads_msg.ME[5] << 8) + ((unsigned short)ads_msg.ME[6]);
}
unsigned char GetAdsIDASCII(unsigned char data)
{
	unsigned char Ret = 0;
	if ((data & 0x30) == 0x00)//b5 b6 为0
	{
		Ret = 0x40 + (data & 0x0f);
	}
	else if ((data & 0x30) == 0x10)
	{
		Ret = 0x50 + (data & 0x0f);
	}
	else if ((data & 0x30) == 0x20)
	{
		if ((data & 0x0f) == 0x00)
		{
			Ret = 0x20;
		}
	}
	else if ((data & 0x30) == 0x30)
	{
		Ret = 0x30 + (data & 0x0f);
	}
	return Ret;
}
void decode_id_type()
{
	unsigned char datelength;
	datelength = sizeof(ID_TYPE);
	memset(&id_my, 0, datelength);

	id_my.type = ((ads_msg.ME[0] & 0xf8) >> 3);
	id_my.RF_mode = (ads_msg.ME[0] & 0x07);
	id_my.ID_1 = GetAdsIDASCII((ads_msg.ME[1] & 0xfc) >> 2);
	id_my.ID_2 = GetAdsIDASCII(((ads_msg.ME[1] & 0x03) << 4) + ((ads_msg.ME[2] & 0xf0) >> 4));
	id_my.ID_3 = GetAdsIDASCII(((ads_msg.ME[2] & 0x0f) << 2) + ((ads_msg.ME[3] & 0xc0) >> 6));
	id_my.ID_4 = GetAdsIDASCII((ads_msg.ME[3] & 0x3f));
	id_my.ID_5 = GetAdsIDASCII(((ads_msg.ME[4] & 0xfc) >> 2));
	id_my.ID_6 = GetAdsIDASCII(((ads_msg.ME[4] & 0x03) << 4) + ((ads_msg.ME[5] & 0xf0) >> 4));
	id_my.ID_7 = GetAdsIDASCII(((ads_msg.ME[5] & 0x0f) << 2) + ((ads_msg.ME[6] & 0xc0) >> 6));
	id_my.ID_8 = GetAdsIDASCII(ads_msg.ME[6] & 0x3f);

}
void decode_airspeed()
{
	unsigned char datelength;
	datelength=sizeof(AIR_SPEED);
	memset(&air_speed_my, 0, datelength);

	air_speed_my.type = ((ads_msg.ME[0] & 0xf8) >> 3);
	air_speed_my.sub_type = (ads_msg.ME[0] & 0x07);
	if(air_speed_my.sub_type == 1 || air_speed_my.sub_type == 2)
	{
		air_speed_my.aim_change = ((ads_msg.ME[1] & 0x80) >> 7);
		air_speed_my.IFR = ((ads_msg.ME[1] & 0x40) >> 6);
		air_speed_my.navigation_indeterminacy = ((ads_msg.ME[1] & 0x38) >> 3);
		air_speed_my.east_or_west = ((ads_msg.ME[1] & 0x04) >> 2);
		air_speed_my.east_or_west_speed = ((unsigned short)(ads_msg.ME[1] & 0x03) << 8) + ((unsigned short)ads_msg.ME[2]);
		air_speed_my.south_or_north = ((ads_msg.ME[3] & 0x80) >> 7);
		air_speed_my.south_or_north_speed = (((unsigned short)ads_msg.ME[3] & 0x7f) << 3) + (((unsigned short)ads_msg.ME[4] & 0xe0) >> 5);
		air_speed_my.vertical_source = ((ads_msg.ME[4] & 0x10) >> 4);
		air_speed_my.vertical_speed_sign = ((ads_msg.ME[4] & 0x08) >> 3);
		air_speed_my.vertical_speed = ((unsigned short)(ads_msg.ME[4] & 0x07) << 6) + ((unsigned short)(ads_msg.ME[5] & 0xfc) >> 2);
		air_speed_my.nop = (ads_msg.ME[5] & 0x03);
		air_speed_my.barometric_pressure_sign = ((ads_msg.ME[6] & 0x80) >> 7);
		air_speed_my.barometric_pressure = (ads_msg.ME[6] & 0x7f);
	}
	else if(air_speed_my.sub_type == 3 || air_speed_my.sub_type == 4)
	{
		air_speed_my.aim_change = ((ads_msg.ME[1] & 0x80) >> 7);
		air_speed_my.IFR = ((ads_msg.ME[1] & 0x40) >> 6);
		air_speed_my.navigation_indeterminacy = ((ads_msg.ME[1] & 0x38) >> 3);

		air_speed_my.course_condition = ((ads_msg.ME[1] & 0x04) >> 2);
		air_speed_my.course = ((unsigned short)(ads_msg.ME[1] & 0x03) << 8) + ((unsigned short)ads_msg.ME[2]);
		air_speed_my.airspeed_mode = ((ads_msg.ME[3] & 0x80) >> 7);
		air_speed_my.airspeed = (((unsigned short)ads_msg.ME[3] & 0x7f) << 3) + (((unsigned short)ads_msg.ME[4] & 0xe0) >> 5);

		air_speed_my.vertical_source = ((ads_msg.ME[4] & 0x10) >> 4);
		air_speed_my.vertical_speed_sign = ((ads_msg.ME[4] & 0x08) >> 3);
		air_speed_my.vertical_speed = ((unsigned short)(ads_msg.ME[4] & 0x07) << 6) + ((unsigned short)(ads_msg.ME[5] & 0xfc) >> 2);
		air_speed_my.nop = (ads_msg.ME[5] & 0x03);
		air_speed_my.barometric_pressure_sign = ((ads_msg.ME[6] & 0x80) >> 7);
		air_speed_my.barometric_pressure = (ads_msg.ME[6] & 0x7f);
	}
	else
	{
		return;
	}


}
void decode_aim_condition()
{
	unsigned char datelength;
	datelength=sizeof(AIM_CONDITION);
	memset(&aim_condition_my, 0, datelength);

	aim_condition_my.type = ((ads_msg.ME[0] & 0xf8) >> 3);
	aim_condition_my.sub_type = ((ads_msg.ME[0] & 0x06) >> 1);
	aim_condition_my.vertical_data_indicate = ((ads_msg.ME[0] & 0x01) << 1) + ((ads_msg.ME[1] & 0x80) >> 7);
	aim_condition_my.aim_high_type = ((ads_msg.ME[1] & 0x40) >> 6);
	aim_condition_my.compatible_sign = ((ads_msg.ME[1] & 0x20) >> 5);
	aim_condition_my.high_performance = ((ads_msg.ME[1] & 0x18) >> 3);
	aim_condition_my.vertical_mode = ((ads_msg.ME[1] & 0x06) >> 1);
	aim_condition_my.aim_high = ((unsigned short)(ads_msg.ME[1] & 0x01) << 9) + ((unsigned short)ads_msg.ME[2] << 1) + ((unsigned short)(ads_msg.ME[3] & 0x80) >> 7);
	aim_condition_my.horizontal_data_indicate = ((ads_msg.ME[3] & 0x60) >> 5);
	aim_condition_my.course = ((unsigned short)(ads_msg.ME[3] & 0x1f) << 4) + ((unsigned short)(ads_msg.ME[4] & 0xf0) >> 4);
	aim_condition_my.heading = ((ads_msg.ME[4] & 0x08) >> 3);
	aim_condition_my.horizontal_mode = ((ads_msg.ME[4] & 0x06) >> 1);
	aim_condition_my.NACP = ((ads_msg.ME[4] & 0x01) << 3) + ((ads_msg.ME[5] & 0xe0) >> 5); 
	aim_condition_my.NIC = ((ads_msg.ME[5] & 0x10) >> 4);
	aim_condition_my.SIL = ((ads_msg.ME[5] & 0x0c) >> 2);
	aim_condition_my.nop = ((ads_msg.ME[5] & 0x03) << 3) + ((ads_msg.ME[6] & 0xe0) >> 5);
	aim_condition_my.performance_code = ((ads_msg.ME[6] & 0x18) >> 3);
	aim_condition_my.prior = (ads_msg.ME[6] & 0x07);
}
void decode_plane_working_condition()
{
	unsigned char datelength;
	datelength=sizeof(WORKING_CONDITION);
	memset(&working_my, 0, datelength);

	working_my.type = ((ads_msg.ME[0] & 0xf8) >> 3);
	working_my.sub_type = (ads_msg.ME[0] & 0x07);
	working_my.cc_code = ((unsigned short)(ads_msg.ME[1]) << 8) + (unsigned short)(ads_msg.ME[2]);  //ÕâÀï½«sub_type = 0ºÍsub_type = 1µÄÇé¿öºÏ²¢µ½Ò»Æð´¦ÀíÁË¡£½«L/W·ÅÔÚcc_codeµÄ×îµÍ4Î»ÁË
	working_my.om_mode = ((unsigned short)(ads_msg.ME[3]) << 8) + (unsigned short)(ads_msg.ME[4]);
	working_my.MOPS = ((ads_msg.ME[5] & 0xe0) >> 5);
	working_my.NIC = ((ads_msg.ME[5] & 0x10) >> 4);
	working_my.NAC = (ads_msg.ME[5] & 0x0f);
	working_my.BAQ_or_nop = ((ads_msg.ME[6] & 0xc0) >> 6);
	working_my.SIL = (ads_msg.ME[6] & 0x30) >> 4;
	//	working.NIC_or_TRK = ((ads_msg.ME[6] & 0x30) >> 4);
	//	working.HRD = ((ads_msg.ME[6] & 0x08) >> 3);
	//	working.nop = (ads_msg.ME[6] & 0x07);
	working_my.NIC_or_TRK = ((ads_msg.ME[6] & 0x08) >> 3);
	working_my.HRD = ((ads_msg.ME[6] & 0x04) >> 2);
	working_my.nop = (ads_msg.ME[6] & 0x03);
}
void decode_test()
{
	unsigned char datelength;
	unsigned char i = 0;
	datelength=sizeof(TEST_MSG);
	memset(&test_my, 0, datelength);

	test_my.type = ((ads_msg.ME[0] & 0xf8) >> 3);
	test_my.sub_type = (ads_msg.ME[0] & 0x07);
	if(test_my.sub_type == 0)
	{
		//		memcpy(&test_my.test_data[0],&ads_msg.ME[1],6);
		for(i = 0; i < 6; i++)
		{
			test_my.test_data[i] = ads_msg.ME[i+1];
		}
	}
	else if(test_my.sub_type == 7)
	{
		test_my.A_code = (ads_msg.ME[1] << 5) + ((ads_msg.ME[2] & 0xf8) >> 3);  //mode A code 9~21
	}
	else
	{
		return;
	}		
}
void decode_pause_air_condition()
{
	unsigned char datelength;
	datelength=sizeof(PAUSE_AIR_CONDITION);
	memset(&pause_my, 0, datelength);

	pause_my.type = ((ads_msg.ME[0] & 0xf8) >> 3);
	pause_my.sub_type = (ads_msg.ME[0] & 0x07);
	if(pause_my.sub_type == 1)
	{
		pause_my.urgency = ((ads_msg.ME[1] & 0xe0) >> 5);
	}
	else
	{
		return;
	}

}



/**********************************************************
ADS-B使用的传输格式：DF17,DF=18且CF=/1，DF=19且AF=0，
DF=19格式预留军事应用，非军事应用的ADS_B系统不应使用此格式。
***********************************************************/
int phase_ads_b_packet(unsigned char *packet,int len)
{
	unsigned char ads_type_1;
	if(packet == NULL || len != 14)
	{
	  return 0;
	}
	memset(&ads_msg,0,sizeof(ADS_B));
	ads_type_1 = packet[0] & 0xf8;
	ads_msg.DF = (ads_type_1 >> 3);
	ads_msg.CA_CF = (ads_type_1 & 0x07);
	if(ads_msg.DF != 17 && ads_msg.DF != 18 && ads_msg.DF != 19)
	{
		return 0;
	}
	memcpy(ads_msg.AA,packet+1,len-1);	
	if(ads_msg.DF == 17)	
	{
		return 1;
	}
	if((ads_msg.DF == 18) && (ads_msg.CA_CF == 0))
	{
		return 1;
	}
//	if((ads_msg.DF == 18) && (ads_msg.CA_CF == 1))
//	{
//		return 1;
//	}
	if((ads_msg.DF == 18) && (ads_msg.CA_CF == 2))
	{
		return 1;
	}
	if((ads_msg.DF == 18) && (ads_msg.CA_CF == 3))
	{
		return 1;
	}
	if((ads_msg.DF == 18) && (ads_msg.CA_CF == 5))
	{
		return 1;
	}
	if((ads_msg.DF == 19) && (ads_msg.CA_CF == 0))
	{
		return 1;
	}
		return 0;


}

/**********************************************************
按照ADS_B 1090MHz ES标准中表2-14
**********************************************************/
void phase_ads_b_msg(unsigned char *packet,unsigned char len,unsigned char data[])
{
	if(phase_ads_b_packet(packet,len))
	{
		switch((ads_msg.ME[0] & 0xf8) >> 3)
		{
		case 0:
			break;
		case 1:
		case 2:
		case 3:
		case 4:
			decode_id_type();
			write_send_buff(&id_my, sizeof(id_my),data);
			break;
		case 5:
		case 6:
		case 7:
		case 8:
			decode_earth_location();
			write_send_buff(&earth_my, sizeof(earth_my),data);
			memset(&earth_my,0,sizeof(earth_my));
			break;
		case 9:
		case 10:
		case 11:
		case 12:
		case 13:
		case 14:
		case 15:
		case 16:
		case 17:
		case 18:
			decode_air_location();
			write_send_buff(&air_my, sizeof(air_my),data);
			memset(&air_my,0,sizeof(air_my));
			break;	
		case 19:
			if((ads_msg.ME[0] & 0x07) > 0 && (ads_msg.ME[0] & 0x07) < 5)
			{
				decode_airspeed();
				write_send_buff(&air_speed_my, sizeof(air_speed_my),data);
			}
			break;
		case 20:
		case 21:
		case 22:
			decode_air_location();
			write_send_buff(&air_my, sizeof(air_my),data);
			break;
		case 23:
			if((ads_msg.ME[0] & 0x07) == 0 || (ads_msg.ME[0] & 0x07) == 7)
			{
				decode_test();
				write_send_buff(&test_my, sizeof(test_my),data);
			}
			break;
		case 28:
			if((ads_msg.ME[0]& 0x07) == 1)
			{
				decode_pause_air_condition();
				write_send_buff(&pause_my, sizeof(pause_my),data);
			}
			break;
		case 29:
			if((ads_msg.ME[0]& 0x06) == 0)
			{
				decode_aim_condition();
				write_send_buff(&aim_condition_my, sizeof(aim_condition_my),data);
			}
			break;
		case 31:
			if((ads_msg.ME[0] & 0x07) == 0 || (ads_msg.ME[0] & 0x07) == 1)
			{
				decode_plane_working_condition();
				write_send_buff(&working_my, sizeof(working_my),data);
			}
			break;
		default:
			break;
		}
  }

	memset(&ads_msg,0,sizeof(ads_msg));
	return;
}
