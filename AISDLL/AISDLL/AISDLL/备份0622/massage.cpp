#include <stdafx.h>
#include "massage.h"

#include "TOEC_AIS.h"

#include "string.h"

#include "math.h"
#include "stdlib.h"
typedef unsigned short u16;
typedef unsigned char u8;
signed char data;
int ais_end_num=0;
int AIS_CRC_right=0;
int AIS_CRC_wrong=0;
int ais_get=0;
uint16_t ais_crc=0;
uint16_t acars_crc=0;
unsigned char mark=0;
unsigned char AIS_len;

struct info_1_2_3 cinfo_1_2_3 = {0};
struct info_4_11 info_4_11 = {0};
struct info_5 info_5 = {0};
struct info_6 info_6 = {0};
struct info_7_13 info_7_13 = {0};
struct info_8 info_8 = {0};
struct info_9 info_9 = {0};
struct info_10 info_10 = {0};
struct info_12 info_12 = {0};
struct info_14 info_14 = {0};
struct info_15 info_15 = {0};
struct info_16 info_16 = {0};
struct info_17 info_17 = {0};
struct info_18 info_18 = {0};
struct info_19 info_19 = {0};
struct info_20 info_20 = {0};
struct info_21 info_21 = {0};
struct info_22 info_22 = {0};

/*--------added by zhangyu-----------*/
struct info_23 info_23 = {0};
struct info_24_A info_24_A = {0};
struct info_24_B info_24_B = {0};
struct info_25 info_25 = {0};
struct info_26 info_26 = {0};
/*--------added by zhangyu-----------*/

AIS_STRUCT ais_struct;
struct recive_setting recive_setting = {0};
struct work_mode work_mode = {0};
struct condition_report condition_report = {0};
uint8_t plx_send_buffer[300];
uint16_t signdata;
/*
extern uint8_t FPGA_type_buffer[300];
extern uint8_t FPGA_type_buffer1[300];
extern uint8_t FPGA_type_buffer2[300];
*/

 int i;
int j;
int HDLC_num=0;
int receive_times=0;

/********************************************************
利用uion联合体实现unsigned char型与float型的转换
********************************************************/
union frequence
{
	unsigned char FRE_C[4];
	float FRE_F;
};

union frequence date;



/**********************************************
CRC检测程序：
ccitt_16_table为16位CRC检测的标准系数库
u16 crc16_strait 是直接计算CRC运算量较大
u16 calc_crc16 是利用表格计算CRC
BitReverse 是高地位翻转函数
**********************************************/
const unsigned short ccitt_16_table[] = {
	0x0000, 0x1021, 0x2042, 0x3063, 0x4084, 0x50A5, 0x60C6, 0x70E7,
	0x8108, 0x9129, 0xA14A, 0xB16B, 0xC18C, 0xD1AD, 0xE1CE, 0xF1EF,
	0x1231, 0x0210, 0x3273, 0x2252, 0x52B5, 0x4294, 0x72F7, 0x62D6,
	0x9339, 0x8318, 0xB37B, 0xA35A, 0xD3BD, 0xC39C, 0xF3FF, 0xE3DE,
	0x2462, 0x3443, 0x0420, 0x1401, 0x64E6, 0x74C7, 0x44A4, 0x5485,
	0xA56A, 0xB54B, 0x8528, 0x9509, 0xE5EE, 0xF5CF, 0xC5AC, 0xD58D,
	0x3653, 0x2672, 0x1611, 0x0630, 0x76D7, 0x66F6, 0x5695, 0x46B4,
	0xB75B, 0xA77A, 0x9719, 0x8738, 0xF7DF, 0xE7FE, 0xD79D, 0xC7BC,
	0x48C4, 0x58E5, 0x6886, 0x78A7, 0x0840, 0x1861, 0x2802, 0x3823,
	0xC9CC, 0xD9ED, 0xE98E, 0xF9AF, 0x8948, 0x9969, 0xA90A, 0xB92B,
	0x5AF5, 0x4AD4, 0x7AB7, 0x6A96, 0x1A71, 0x0A50, 0x3A33, 0x2A12,
	0xDBFD, 0xCBDC, 0xFBBF, 0xEB9E, 0x9B79, 0x8B58, 0xBB3B, 0xAB1A,
	0x6CA6, 0x7C87, 0x4CE4, 0x5CC5, 0x2C22, 0x3C03, 0x0C60, 0x1C41,
	0xEDAE, 0xFD8F, 0xCDEC, 0xDDCD, 0xAD2A, 0xBD0B, 0x8D68, 0x9D49,
	0x7E97, 0x6EB6, 0x5ED5, 0x4EF4, 0x3E13, 0x2E32, 0x1E51, 0x0E70,
	0xFF9F, 0xEFBE, 0xDFDD, 0xCFFC, 0xBF1B, 0xAF3A, 0x9F59, 0x8F78,
	0x9188, 0x81A9, 0xB1CA, 0xA1EB, 0xD10C, 0xC12D, 0xF14E, 0xE16F,
	0x1080, 0x00A1, 0x30C2, 0x20E3, 0x5004, 0x4025, 0x7046, 0x6067,
	0x83B9, 0x9398, 0xA3FB, 0xB3DA, 0xC33D, 0xD31C, 0xE37F, 0xF35E,
	0x02B1, 0x1290, 0x22F3, 0x32D2, 0x4235, 0x5214, 0x6277, 0x7256,
	0xB5EA, 0xA5CB, 0x95A8, 0x8589, 0xF56E, 0xE54F, 0xD52C, 0xC50D,
	0x34E2, 0x24C3, 0x14A0, 0x0481, 0x7466, 0x6447, 0x5424, 0x4405,
	0xA7DB, 0xB7FA, 0x8799, 0x97B8, 0xE75F, 0xF77E, 0xC71D, 0xD73C,
	0x26D3, 0x36F2, 0x0691, 0x16B0, 0x6657, 0x7676, 0x4615, 0x5634,
	0xD94C, 0xC96D, 0xF90E, 0xE92F, 0x99C8, 0x89E9, 0xB98A, 0xA9AB,
	0x5844, 0x4865, 0x7806, 0x6827, 0x18C0, 0x08E1, 0x3882, 0x28A3,
	0xCB7D, 0xDB5C, 0xEB3F, 0xFB1E, 0x8BF9, 0x9BD8, 0xABBB, 0xBB9A,
	0x4A75, 0x5A54, 0x6A37, 0x7A16, 0x0AF1, 0x1AD0, 0x2AB3, 0x3A92,
	0xFD2E, 0xED0F, 0xDD6C, 0xCD4D, 0xBDAA, 0xAD8B, 0x9DE8, 0x8DC9,
	0x7C26, 0x6C07, 0x5C64, 0x4C45, 0x3CA2, 0x2C83, 0x1CE0, 0x0CC1,
	0xEF1F, 0xFF3E, 0xCF5D, 0xDF7C, 0xAF9B, 0xBFBA, 0x8FD9, 0x9FF8,
	0x6E17, 0x7E36, 0x4E55, 0x5E74, 0x2E93, 0x3EB2, 0x0ED1, 0x1EF0
};


static unsigned short crc_ccitt_byte(u16 crc, const u8 c)
{
	return (crc >> 8) ^ ccitt_16_table[(crc ^ c) & 0xff];
}

unsigned short crc_ccitt(u16 crc, u8 const *buffer, size_t len)
{
	while (len--)
		crc = crc_ccitt_byte(crc, *buffer++);
	return crc;
}
/*
static u16 crc16_strait(u8 *d, int len)
{
	u8 b=0;
	u16 crc=0x0000;
	int i=0;
	int		j=0;
	for(i=0;i<len;i++)
	{
		for(j=0;j<8;j++)
		{
			b = ((d[i]<<j) & 0x80 )^((crc&0x8000)>>8);
			crc<<=1;
			if(b!=0)
				crc^=0x1021;
		}
	}
	return crc;
}
*/
static u16 calc_AIS_crc16(u8 *d, int len)
{
	u8 hbit=0;
	u16 crc = 0xffff;
	int i;
	for(i=0;i<len;i++)
	{
		hbit = (crc&0xff00)>>8;
		crc<<=8;
		crc^=ccitt_16_table[hbit^d[i]];
	}
	return crc;
}

static u16 calc_ACARS_crc16(u8 *d, int len)
{
	u8 hbit=0;
	u16 crc = 0x0000;
	int i;
	for(i=0;i<len;i++)
	{
		hbit = (crc&0xff00)>>8;
		crc<<=8;
		crc^=ccitt_16_table[hbit^d[i]];
	}
	return crc;
}

unsigned char BitReverse(unsigned char x)
{
	int i;
	unsigned char Temp=0;
	for(i=0; ; i++)
	{
		if(x & 0x80)	Temp |= 0x80;
		if(i==7)		break;
		x	<<= 1;
		Temp >>= 1;
	}
	return Temp;
}

unsigned short BitReverse_16bit(unsigned short x)
{
	int i;
	unsigned short Temp=0;
	for(i=0; ; i++)
	{
		if(x & 0x8000)	Temp |= 0x8000;
		if(i==15)		break;
		x	<<= 1;
		Temp >>= 1;
	}
	return Temp;
}
unsigned char bit_of_buffer(unsigned char *buf, unsigned short bit_pos){
    return (buf[bit_pos/8] & (1 << (7 - bit_pos%8))) ? 1 : 0;
}
void hdlc_enc(unsigned char *buf_in, unsigned char count_in, unsigned char *buf_out, unsigned short *count_out)
{
    unsigned short bitlen_in = count_in;
    unsigned short i = 0, j = 0;
    unsigned char *buf_temp = (unsigned char*)malloc((int)(bitlen_in / 8 + 3));
    unsigned char byte_temp = 0;
    memset(buf_temp, 0, bitlen_in/8 + 3);
    while(i<bitlen_in)
		{
			unsigned char bit_val = bit_of_buffer(buf_in, i);
			byte_temp = (byte_temp << 1) | bit_val;
			buf_temp[(i + j)/8] |= bit_val << ( 7 - (i+j)%8);
			/* 5 '1's in a row detected, a following '0' should be inserted */
			if((byte_temp & 0x1f) == 0x1f){
					j++;
					byte_temp <<= 1;
			}
			i++;
    }
    *count_out = bitlen_in + j;
    memcpy(buf_out, buf_temp, (i+j)/8 + ((i+j)%8?1:0));
    free(buf_temp);
}
void hdlc_dec(unsigned char *buf_in, unsigned short count_in, unsigned char *buf_out, unsigned short *count_out)
{
    unsigned short bitlen_in = count_in;
    unsigned short bitlen_out = 0;
    unsigned short i = 0, j = 0;
    unsigned char *buf_temp = (unsigned char *)malloc((int)(bitlen_in / 8 + 1));
    unsigned char byte_temp = 0;
    memset(buf_temp, 0, bitlen_in/8 + 1);
    while(i<bitlen_in)
		{
			unsigned char bit_val = bit_of_buffer(buf_in, i);
			byte_temp = (byte_temp << 1) | bit_val;
			buf_temp[(i - j)/8] |= bit_val << ( 7 - (i-j)%8);
			/* 5 '1's in a row detected, a following '0' should be removed */
			if((byte_temp & 0x3f) == 0x3e){
					j++;
			}
			/* 6 '1's in a row indicates a packet ending, break now */
			else if((byte_temp & 0x3f) == 0x3f){
					i++;
					break;
			}
			i++;
    }
    *count_out = i - j;
    memcpy(buf_out, buf_temp, (i-j)/8 + ((i-j)%8?1:0));
    free(buf_temp);
    return;
}

/*
void send_date_to_pci(void)
{
	
	for(i=0;i<101;i++)
	{
		plx_send_buffer[i]=i;
		
	}
	INTX_DISABLE();
	FPGA_Write_PCI(plx_send_buffer, 0, 101);
	sum = 101;
	FPGA_Write_PCI(& sum, 0xff, 1);
	INTX_ENABLE();
}
*/
/**********************************************************************
该函数是从第start + 1个bit开始连续取出len个bit放到data中，
返回值为usnigned char型所以该函数只能提取8bit以下的数。
***********************************************************************/

unsigned char get_BYTE_data(unsigned char buffer[],unsigned char start,unsigned char len)
{
	unsigned char num_start,remainder_start;
	unsigned char num_end,remainder_end;
	unsigned char data;
	num_start = start / 8;
	remainder_start = start % 8;
	num_end = (start + len) / 8;
	remainder_end = (start + len) % 8;
	data = ((buffer[num_start] & (0xff >> remainder_start)) << ((num_end - num_start -1)*8 + remainder_end));
	i=num_end;
	while(i>num_start+1)
	{
		data = data + (buffer[i - 1] << ((num_end - i)*8 + remainder_end));
		i--;
	}
	data = data + ((buffer[num_end] & (uint8_t)(0xff00 >> remainder_end)) >> (0x08 - remainder_end));
	return data;
}	


/**********************************************************************
该函数是从第start + 1个bit开始连续取出len个bit放到data中，
返回值为short型所以该函数只能提取16bit以下的数。
***********************************************************************/
short get_u16_data(unsigned char buffer[],unsigned char start,unsigned char len)
{
	unsigned char num_start,remainder_start;
	unsigned char num_end,remainder_end;
	short data;
	num_start = start / 8;
	remainder_start = start % 8;
	num_end = (start + len) / 8;
	remainder_end = (start + len) % 8;
	data = ((buffer[num_start] & (0xff >> remainder_start)) << ((num_end - num_start -1)*8 + remainder_end));
	i=num_end;
	while(i>num_start+1)
	{
		data = data + (buffer[i - 1] << ((num_end - i)*8 + remainder_end));
		i--;
	}
	data = data + ((buffer[num_end] & (uint8_t)(0xff00 >> remainder_end)) >> (0x08 - remainder_end));
	return data;
}

/**********************************************************************
该函数是从第start + 1个bit开始连续取出len个bit放到data中，
返回值为int型所以该函数只能提取32bit以下的数。
***********************************************************************/
int get_u32_data( unsigned char buffer[],unsigned char start,unsigned char len)
{
	unsigned char num_start,remainder_start;
	unsigned char num_end,remainder_end;
	int data;
	num_start = start / 8;
	remainder_start = start % 8;
	num_end = (start + len) / 8;
	remainder_end = (start + len) % 8;
	data = ((buffer[num_start] & (0xff >> remainder_start)) << ((num_end - num_start -1)*8 + remainder_end));
	i=num_end;
	while(i>num_start+1)
	{
		data = data + (buffer[i - 1] << ((num_end - i)*8 + remainder_end));
		i--;
	}
	data = data + ((buffer[num_end] & (uint8_t)(0xff00 >> remainder_end)) >> (0x08 - remainder_end));
	return data;
}	

/**************************************************************************************
该函数是从第start + 1个bit开始连续取出len个bit放到数组data中，(数据为6bit ascii码)
***************************************************************************************/

void get_sting_data(unsigned char buffer[], char data[],unsigned char start,unsigned char len)
{
	unsigned char num_start,remainder_start;
	unsigned char num_end,remainder_end;
	unsigned char a8[200];
	unsigned len_A,len_B;
	len_A = len/8;
	len_B = len/6;
	num_start = start / 8;
	remainder_start = start % 8;
	num_end = (start + len) / 8;
	remainder_end = (start + len) % 8;
	a8[0] = ((buffer[num_start] & (0xff >> remainder_start)) << ((num_end - num_start -1)*8 + remainder_end));
	i=num_end;
	while(i>num_start+1)
	{
		a8[num_end - i +1] = (buffer[i - 1] << ((num_end - i)*8 + remainder_end));
		i--;
	}
	a8[num_start - num_end] = ((buffer[num_end] & (uint8_t)(0xff00 >> remainder_end)) >> (0x08 - remainder_end));
	for(i=0,j=0;i<len_A && j<len_B;i=i+4,j=j+3)
	{
		data[i]=((a8[j] & 0xfc) >> 2);
		data[i+1]=((a8[j] & 0x0c) << 4) + ((a8[j+1] & 0xf0) >> 4);
		data[i+2]=((a8[j+1] & 0x0f) << 2) + ((a8[j+2] & 0xc0) >> 6);
		data[i+3]=(a8[j+2] & 0x3f);
	}
}	

/**************************************************************************************************************************
			以下21个函数是根据AIS的国家标准GB—T20068-2006的消息格式进行转换解码的操作，基本是位操作，具体可以参照AIS通信协议文档。

****************************************************************************************************************************/
void demode_ship_location(unsigned char *packet)
{
	cinfo_1_2_3.ID_info_code=(packet[0] & 0xfc) >> 2;
	
	cinfo_1_2_3.sendtimes=(packet[0] & 0x03 );
	cinfo_1_2_3.User_id_code=(((int)packet[1] & 0xffffffff )<< 22)+(((int)packet[2] & 0xffffffff) << 14)+(((int)packet[3] & 0xffffffff) <<6)+(((int)packet[4] & 0xfffffffc) >> 2);
	cinfo_1_2_3.shipping_condition=((packet[4] & 0x03) << 2)+((packet[5] & 0xc0) >> 6);
	cinfo_1_2_3.turning_speeed=((packet[5] & 0x3f) << 2)+(packet[6] & 0xc0);
	cinfo_1_2_3.speed_earth=(((short int)packet[6] & 0xff3f )<< 4)+(((short int)packet[7] & 0xfff0) >> 4);
	cinfo_1_2_3.location_precision=((packet[7] & 0x08) >> 7);
	cinfo_1_2_3.longitude=(((int)packet[7] & 0xffffff07) << 25)+((int)packet[8]) << 17+((int)packet[9] << 9)+((int)packet[10] << 1)+(((int)packet[11] & 0xffffff80) >> 7);
	cinfo_1_2_3.latitude=(((int)packet[11] & 0x7f) << 20)+((int)packet[12] << 12)+((int)packet[13] << 4)+(((int)packet[14] & 0xfffffff0) >> 4);

	cinfo_1_2_3.course_earth=(((short int)packet[14] & 0xff0f) << 8)+((short int)packet[15]);
	cinfo_1_2_3.course_real=((short int)packet[16] << 1)+(((short int)packet[17] & 0xff80) >> 7);

	cinfo_1_2_3.mark_time=((packet[17] & 0x07e) >> 1);
	cinfo_1_2_3.region_keep=((packet[17] & 0x01) << 3) +((packet[18] & 0xe0) >> 5);
	cinfo_1_2_3.standby=((packet[18] & 0x10) >> 4);
	cinfo_1_2_3.mark_rami=((packet[18] & 0x08) >> 3);
	cinfo_1_2_3.comm_condition=(((int)packet[18]& 0x08) << 16)+((int)packet[18] << 8)+((int)packet[20]);
	
}

void demode_UTC_time(unsigned char *packet)
{	
	info_4_11.ID_info_code=((packet[0] & 0xfc) >> 2);
	
	info_4_11.sendtimes=(packet[0] & 0x03);
	info_4_11.User_id_code=(((int)packet[1] & 0xffffffff) << 22)+(((int)packet[2] & 0xffffffff) << 14)+(((int)packet[3] & 0xffffffff) << 6)+(((int)packet[4] & 0xfffffffc) >> 2);
	info_4_11.UTC_year=(((short int)packet[4] & 0xff03) << 12)+((short int)packet[5] << 4)+(((short int)packet[6] & 0xfff0) >> 4);
	info_4_11.UTC_month=(packet[6] & 0x0f);
	info_4_11.UTC_date=((packet[7] & 0xf8) >> 3);
	info_4_11.UTC_hour=((packet[7] & 0x07) << 2)+((packet[8] & 0xc0) >> 6);
	info_4_11.UTC_minter=(packet[8] & 0x3f);
	info_4_11.UTC_seconds=((packet[9] & 0xfc) >> 2);
	info_4_11.location_precision=((packet[9] & 0x02) >> 1); 
	info_4_11.longitude=(((int)packet[9] & 0xffffff01) << 27)+((int)packet[10] << 19)+((int)packet[11] << 11)+((int)packet[12] << 3)+(((int)packet[13] & 0xffffffe0) >> 5);
	info_4_11.latitude=(((int)packet[13] & 0xffffff1f) << 22)+((int)packet[16] << 14)+((int)packet[16] << 6)+(((int)packet[16] & 0xfffffffc) >> 2);
	info_4_11.location_type=((packet[16] & 0x03) << 2)+((packet[17] & 0xc0) >> 6);
	info_4_11.standby=(((short int)packet[17] & 0xff3f) << 4)+(((short int)packet[18] & 0xfff0) >> 4);
	info_4_11.mark_rami=((packet[18] & 0x08) >> 3);
	info_4_11.comm_condition=(((int)packet[18] & 0x07) << 16)+((int)packet[19] << 8)+((int)packet[20]);
	ais_struct.lUserID=info_4_11.User_id_code;
	/*******************************************************************/
	//ais_struct.lUserID=info_4_11.User_id_code;;
	//ais_struct.dLongitude = ((double) info_4_11.longitude/600000.00);
	//ais_struct.dLatitude = ((double) info_4_11.latitude/600000.00);
	

}

void demode_static_data(unsigned char *packet)
{
	info_5.ID_info_code=((packet[0] & 0xfc) >> 2);
	
	info_5.sendtimes=(packet[0] & 0x03);
	info_5.User_id_code=(((int)packet[1] & 0xffffffff) << 22)+(((int)packet[2] & 0xffffffff) << 14)+(((int)packet[3] & 0xffffffff) << 6)+(((int)packet[4] & 0xfffffffc) >> 2);
	info_5.version_AIS=(packet[4] & 0x03);
	info_5.code_IMO=((int)packet[5] << 22)+((int)packet[6] << 14)+((int)packet[7] << 6)+(((int)packet[8] & 0xfffffffc) >> 2);
	info_5.calling[0]=((packet[8] & 0x03) << 4)+(( packet[9] & 0xf0) >> 4);
	info_5.calling[1]=((packet[9] & 0x0f) << 2)+((packet[10] & 0xc0) >> 6);
	info_5.calling[2]=(packet[10] & 0x3f);
	info_5.calling[3]=((packet[11] & 0xfc) >> 2);
	info_5.calling[4]=((packet[11] & 0x03) << 4)+((packet[12] & 0xf0) >> 4);
	info_5.calling[5]=((packet[12] & 0x0f) << 2)+((packet[13] & 0xc0) >> 6);
	info_5.calling[6]=(packet[13] & 0x3f);
	info_5.ID[0]=((packet[14] & 0xfc) >> 2);
	info_5.ID[1]=((packet[14] & 0x03) << 4)+((packet[15] & 0xf0) >> 4);
	info_5.ID[2]=((packet[15] & 0x0f) << 2)+((packet[16] & 0xc0) >> 6);
	info_5.ID[3]=(packet[16] & 0x3f);
	info_5.ID[4]=((packet[17] & 0xfc) >> 2);
	info_5.ID[5]=((packet[17] & 0x03) << 4)+((packet[18] & 0xf0) >> 4);
	info_5.ID[6]=((packet[18] & 0x0f) << 2)+((packet[19] & 0xc0) >> 6);
	info_5.ID[7]=(packet[19] & 0x3f);
	info_5.ID[8]=((packet[20] & 0xfc) >> 2);
	info_5.ID[9]=((packet[20] & 0x03) << 4)+((packet[21] & 0xf0) >> 4);
	info_5.ID[10]=((packet[21] & 0x0f) <<2)+((packet[22] & 0xc0) >> 6);
	info_5.ID[11]=(packet[22] & 0x3f);
	info_5.ID[12]=((packet[23] & 0xfc) >> 2);
	info_5.ID[13]=((packet[23] & 0x03) << 4)+((packet[24] & 0xf0) >> 4);
	info_5.ID[14]=((packet[24] & 0x0f) << 2)+((packet[25] & 0xc0) >> 6);
	info_5.ID[15]=(packet[25] & 0x3f);
	info_5.ID[16]=((packet[26] & 0xfc) >> 2);
	info_5.ID[17]=((packet[26] & 0x03) << 4)+((packet[27] & 0xf0) >> 4);
	info_5.ID[18]=((packet[27] & 0x0f) << 2)+((packet[28] & 0xc0) >> 6);
	info_5.ID[19]=(packet[28] & 0x3f);	
	info_5.ship_type=packet[29];
	info_5.scale=((int)packet[30] << 22)+((int)packet[31] << 14)+((int)packet[32] << 6)+(((int)packet[33] & 0xfffffffc) >> 2);
	info_5.location_type=((packet[33] & 0x03) << 2)+((packet[34] & 0xc0) >> 6);
	info_5.time_arrive=(((int)packet[34] & 0xffffff3f) << 14)+((int)packet[35] << 6)+(((int)packet[36] & 0xffffff3c) >> 2);
	info_5.draft_static=((packet[36] & 0x03) << 6)+((packet[37] & 0xfc) >> 2);
	info_5.destination[0]=((packet[37] & 0x03) << 4)+((packet[38] & 0xf0) >> 4);
	info_5.destination[1]=((packet[38] & 0x0f) << 2)+((packet[39] & 0xc0) >> 6);
	info_5.destination[2]=(packet[39] & 0x3f);
	info_5.destination[3]=((packet[40] & 0xfc) >> 2);
	info_5.destination[4]=((packet[40] & 0x03) << 4)+((packet[41] & 0xf0) >> 4);
	info_5.destination[5]=((packet[41] & 0x0f) << 2)+((packet[42] & 0xc0) >> 6);
	info_5.destination[6]=(packet[42] & 0x3f);
	info_5.destination[7]=((packet[43] & 0xfc) >>2 );
	info_5.destination[8]=((packet[43] & 0x03) << 4)+((packet[44] & 0xf0) >> 4);
	info_5.destination[9]=((packet[44] & 0x0f) << 2)+((packet[45] & 0xc0) >>6);
	info_5.destination[10]=(packet[45] & 0x3f);
	info_5.destination[11]=((packet[46] & 0xfc) >> 2);
	info_5.destination[12]=((packet[46] & 0x03) << 4)+((packet[47] & 0xf0) >> 4);
	info_5.destination[13]=((packet[47] & 0x0f) <<2 )+((packet[48] & 0xc0) >> 6);
	info_5.destination[14]=(packet[48] & 0x3f);
	info_5.destination[15]=((packet[49] & 0xfc) >> 2);
	info_5.destination[16]=((packet[49] & 0x03) << 4)+((packet[50] & 0xf0) >> 4);
	info_5.destination[17]=((packet[50] & 0x0f) << 2)+((packet[51] & 0xc0) >> 6);
	info_5.destination[18]=(packet[51] & 0x3f);
	info_5.destination[19]=((packet[52] & 0xfc) >>2);
	for(i=0;i<20;i++)
	{
		info_5.ID[i]=tarnsform_6to_8(info_5.ID[i]);
		info_5.destination[i]=tarnsform_6to_8(info_5.destination[i]);
	}
	for(i=0;i<7;i++)
	{
		info_5.calling[i]=tarnsform_6to_8(info_5.calling[i]);
	}
	info_5.date_terminal=((packet[52] & 0x02) >> 1);
	info_5.standby=(packet[52] & 0x01);
	
	ais_struct.lUserID=info_5.User_id_code;
	ais_struct.lIMO=info_5.code_IMO;
	for(i=0;i<7;i++)
	{
		if(info_5.calling[i] == '@')
		{
			info_5.calling[i] = 0;
		}
		ais_struct.cpCallLetters[i] = info_5.calling[i];
	}
	for(i=0;i<20;i++)
	{
		if(info_5.ID[i] == '@')
		{
			info_5.ID[i] = 0;
		}
		ais_struct.cpName[i] = info_5.ID[i];   
	}		
	//ais_struct.bShipAndFreightType = info_5.ship_type;
	//ais_struct.lETA = info_5.location_type;
	//ais_struct.fShipWater = (float)info_5.draft_static;
	for(i=0;i<20;i++)
	{
		if(info_5.destination[i] == '@')
		{
			info_5.destination[i] = 0;
		}
		ais_struct.cpDestination[i] = info_5.destination[i];
	}
	//ais_struct.ucbType = info_5.version_AIS;
//	ais_struct.ichangdu = (info_5.scale & 0x000001ff) + ((info_5.scale & 0x0003fe00) >> 9);
//	ais_struct.ikuandu = ((info_5.scale & 0x000fc0000) >> 18) + ((info_5.scale & 0x3f000000) >> 24);
	/******************************************************************************************************/
//	ais_struct.lIMO=get_u32_data(packet,40,30);
//	get_sting_data(packet,ais_struct.cpCallLetters,70,42);
//	get_sting_data(packet,ais_struct.cpName,112,120);

//	ais_struct.bShipAndFreightType = info_5.ship_type;
//	ais_struct.lETA = info_5.location_type;
//	ais_struct.fShipWater = (float)info_5.draft_static;
//	for(i=0;i<20;i++)
//	{
//		ais_struct.cpDestination[i] = info_5.destination[i];
//	}
	info_5.scale = get_u32_data(packet,240,30);
	//ais_struct.ichangdu = (info_5.scale & 0x000001ff) + ((info_5.scale & 0x0003fe00) >> 9);
	//ais_struct.ikuandu = ((info_5.scale & 0x000fc0000) >> 18) + ((info_5.scale & 0x3f000000) >> 24);
	
}

void demode_addressing(unsigned char *packet)
{
	info_6.ID_info_code=((packet[0] & 0xfc) >> 2);
	ais_struct.lUserID=info_6.ID_info_code;
	info_6.sendtimes=(packet[0] & 0x03);
	info_6.ID_sender_MMSI=(((int)packet[1] & 0xffffffff) << 22)+(((int)packet[2] & 0xffffffff) << 14)+(((int)packet[3] & 0xffffffff) << 6)+(((int)packet[4] & 0xfffffffc) >> 2);
	info_6.serial_code=(packet[4] & 0x03);
	info_6.ID_sender_MMSI=((int)packet[5] << 22)+((int)packet[6] << 14)+((int)packet[7] << 6)+(((int)packet[8] & 0xfffffffc) >> 2);
	info_6.mark_resend=(packet[8] & 0x02);
	info_6.standby=((packet[8] & 0x01) << 7);
	for(i=0;i<117;i++)
	{
		info_6.date_binary[i]=packet[i+8];
	}
	
}

void demode_sefaty_comfirm(unsigned char *packet)
{
	info_7_13.ID_info_code=((packet[0] & 0xfc) >> 2);
	
	info_7_13.sendtimes=(packet[0] & 0x03);
	info_7_13.ID_sender_MMSI=((int)packet[5] << 22)+((int)packet[6] << 14)+((int)packet[7] << 6)+(((int)packet[8] & 0xfffffffc) >> 2);

	info_7_13.standby=(packet[4] & 0x03);
	info_7_13.ID_destination_MMSI_first=((int)packet[5] << 22)+((int)packet[6] << 14)+((int)packet[7] << 6)+((int)packet[8] & 0xfffffffc >> 2);
	info_7_13.serial_code_first=(packet[8] & 0x03);
	info_7_13.ID_destination_MMSI_second=((int)packet[9] << 22)+((int)packet[10] << 14)+((int)packet[11] << 6)+((int)packet[12] & 0xfffffffc >> 2);
	info_7_13.serial_code_second=(packet[12] & 0x03);
	info_7_13.ID_destination_MMSI_third=((int)packet[13] << 22)+((int)packet[14] << 14)+((int)packet[15] << 6)+(((int)packet[16] & 0xfffffffc) >> 2);
	info_7_13.serial_code_third=(packet[16] & 0x03);
	info_7_13.ID_destination_MMSI_fourth=((int)packet[17] << 22)+((int)packet[18] << 14)+((int)packet[19] << 6)+(((int)packet[20] & 0xfffffffc) >> 2);
	info_7_13.serial_code_fourth=(packet[16] & 0x03);

	
}

void demode_broadcast_binary(unsigned char *packet)
{
	info_8.ID_info_code=((packet[0] & 0xfc) >> 2);
	info_8.sendtimes=(packet[0] & 0x03);
	info_8.ID_sender_MMSI=((int)packet[5] << 22)+((int)packet[6] << 14)+((int)packet[7] << 6)+(((int)packet[8] & 0xfffffffc) >> 2);
	info_8.standby=(packet[4] & 0x03);
	for(i=0;i<968;i++)
	{
		info_8.date_binary[i]=packet[i+4];
	}

	
}

void demode_searchrescue_info(unsigned char *packet)
{
	info_9.ID_info_code=((packet[0] & 0xfc) >> 2);
	info_9.sendtimes=(packet[0] & 0x03);
	info_9.ID_sender_MMSI=((int)packet[1] << 22)+((int)packet[2] << 14)+((int)packet[3] << 6)+(((int)packet[4] & 0xfffffffc) >> 2);
	
	info_9.altitute=(((short int)packet[4] & 0xff03) >> 10)+((short int)packet[5] << 2)+(((short int)packet[6] & 0xffc0) >> 6);
	info_9.speed_earth=(((short int)packet[6] & 0xff3f) << 4)+(((short int)packet[7] & 0xfff0) >> 4);
	info_9.location_precision=((packet[7] & 0x08) >> 3);
	info_9.longitude=(((int)packet[7] & 0xffffff07) << 25)+((int)packet[8] << 17)+((int)packet[9] << 9)+((int)packet[10] << 1)+(((int)packet[11] & 0xffffff80) >> 7);
	info_9.latitude=(((int)packet[11] & 0xffffff7f) << 20)+((int)packet[12] << 12)+((int)packet[13] << 4)+(((int)packet[14] & 0xfffffff0) >> 4);
	info_9.course_earth=(((short int)packet[14] & 0xff0f) << 8)+((short int)packet[15]);
	info_9.mark_time=(packet[16] & 0xfc);
	info_9.region_keep=((packet[16] & 0x03) << 6)+((packet[17] & 0xfc) >> 2);
	info_9.date_terminal=((packet[17] & 0x02) >> 1);
	info_9.standby=((packet[17] & 0x01) << 4)+((packet[18] & 0xf0) >> 4);
	info_9.mark_rami=((packet[18] & 0x08) >> 3);
	info_9.comm_condition=(((int)packet[18] & 0xffffff07) << 16)+((int)packet[19] << 8)+((int)packet[20]);

}

void demode_UTCtime_calling(unsigned char *packet)
{
	info_10.ID_info_code=((packet[0] & 0xfc) >> 2);
	info_10.sendtimes=(packet[0] & 0x03);
	info_10.ID_sender_MMSI=((int)packet[1] << 22)+((int)packet[2] << 14)+((int)packet[3] << 6)+(((int)packet[4] & 0xfffffffc) >> 2);
	info_10.standby_1=(packet[4] & 0x03);
	info_10.ID_destination_MMSI=((int)packet[5] << 22)+((int)packet[6] << 14)+((int)packet[7] << 6)+(((int)packet[8] & 0xfffffffc) >> 2);
	info_10.standby_2=(packet[8] & 0x03);

}


void demode_addressing_safety(unsigned char *packet)
{
	info_12.ID_info_code=((packet[0] & 0xfc) >> 2);
	info_12.sendtimes=(packet[0] & 0x03);
	info_12.ID_sender_MMSI=((int)packet[1] << 22)+((int)packet[2] << 14)+((int)packet[3] << 6)+(((int)packet[4] & 0xfffffffc) >> 2);
	info_12.serial_code=(packet[4] & 0x03);
	info_12.ID_destination_MMSI=((int)packet[5] << 22)+((int)packet[6] << 14)+((int)packet[7] << 6)+(((int)packet[8] & 0xfffffffc) >> 2);
	info_12.mark_resend=((packet[8] & 0x02) >> 1);
	info_12.standby=(packet[8] & 0x01);
	for(i=0,j=0;i<156;j++,i=4*j)
	{
		info_12.information_safety[i]=((packet[3*j+9] & 0xfc) >> 2);
		info_12.information_safety[i+1]=((packet[3*j+9] & 0x03) << 4)+((packet[3*j+10] & 0xf0) >> 4);
		info_12.information_safety[i+2]=((packet[3*j+10] & 0x0f) <<2 )+((packet[3*j+11] & 0xc0) >> 6);
		info_12.information_safety[i+3]=(packet[3*j+11] & 0xcf);
	}
	for(i=0;i<156;i++)
	{
		info_12.information_safety[i]=tarnsform_6to_8(info_12.information_safety[i]);
	}

}
	
void demode_safety(unsigned char *packet)
{
	info_14.ID_info_code=((packet[0] & 0xfc) >> 2);
	info_14.sendtimes=(packet[0] & 0x03);
	info_14.ID_sender_MMSI=((int)packet[1] << 22)+((int)packet[2] << 14)+((int)packet[3] << 6)+(((int)packet[4] & 0xfffffffc) >> 2);
	info_14.standby=(packet[4] & 0x03);
	for(i=0,j=0;i<156;j++,i=4*j)
	{
		info_14.information_safety[i]=((packet[3*j+5] & 0xfc) >> 2);
		info_14.information_safety[i+1]=((packet[3*j+5] & 0x03) << 4)+((packet[3*j+6] & 0xf0) >> 4);
		info_14.information_safety[i+2]=((packet[3*j+6] & 0x0f) << 2)+((packet[3*j+7] & 0xc0) >> 6);
		info_14.information_safety[i+3]=(packet[3*j+7] & 0xcf);
	}
	for(i=0;i<156;i++)
	{
		info_14.information_safety[i]=tarnsform_6to_8(info_12.information_safety[i]);
	}

}

void demode_calling(unsigned char *packet)
{
	info_15.ID_info_code=((packet[0] & 0xfc) >> 2);
	info_15.sendtimes=(packet[0] & 0x03);
	info_15.ID_sender_MMSI=((int)packet[1] << 22)+((int)packet[2] << 14)+((int)packet[3] << 6)+(((int)packet[4] & 0xfffffffc) >> 2);
	info_15.standby_1=(packet[4] & 0x03);
	info_15.ID_destination_MMSI_first=((int)packet[5] << 22)+((int)packet[6] << 14)+((int)packet[7] << 6)+(((int)packet[8] & 0xfffffffc) >> 2);
	info_15.ID_information_code_1_1=((packet[8] & 0x03) << 4)+((packet[9] & 0xf0) >> 4);
	info_15.time_offset_1_1=(((short int)packet[9] & 0xff0f) << 8)+((short int)packet[10]);
	info_15.standby_2=((packet[11] & 0xc0) >> 6);
	info_15.ID_information_code_1_2=(packet[11] & 0x3f);
	info_15.time_offset_1_2=((short int)packet[12] << 4)+(((short int)packet[13] & 0xfff0) >> 4);
	info_15.standby_3=(packet[13] & 0x0c);
	info_15.ID_destination_MMSI_second=(((int)packet[13] & 0xffffff03) << 28)+((int)packet[14] << 20)+((int)packet[15] << 12)+((int)packet[16] << 4)+(((int)packet[17] & 0xfffffff0) >> 4);
	info_15.ID_information_code_2_1=((packet[17] & 0x0f) << 2)+((packet[18] & 0xc0) >> 6);
	info_15.time_offset_2_1=(((short int)packet[18] & 0xff3f) << 6)+(((short int)packet[19] & 0xfffc) >> 2);
	info_15.standby_4=(packet[19] & 0x03);

}


void demode_allocation_order(unsigned char *packet)//感觉数据不对
{
	info_16.ID_info_code=((packet[0] & 0xfc) >> 2);
	info_16.sendtimes=(packet[0] & 0x03);
	info_16.ID_sender_MMSI=((int)packet[1] << 22)+((int)packet[2] << 14)+((int)packet[3] << 6)+(((int)packet[4] & 0xfffffffc) >> 2);
	info_16.standby_1=(packet[4] & 0x03);
	info_16.ID_destination_MMSI_A=((int)packet[5] << 22)+((int)packet[6] << 14)+((int)packet[7] << 6)+(((int)packet[8] & 0xfffffffc) >> 2);
	info_16.time_offset_A=(((short int)packet[8] & 0xff03) << 10)+((short int)packet[9] << 2)+(((short int)packet[10] & 0xffc0) >> 6);
	info_16.incremental_A=(((short int)packet[10] & 0xff3f) << 4)+(((short int)packet[11] & 0xfff0) >> 4);
	info_16.ID_destination_MMSI_B=(((int)packet[11] & 0xffffff0f) << 26)+((int)packet[12] << 18)+((int)packet[13] << 10)+((int)packet[14] << 2)+(((int)packet[15] & 0xffffffc0) >> 6);
	info_16.time_offset_B=(((short int)packet[15] & 0xff3f) << 6)+(((short int)packet[16] & 0xfffc) >> 2);
	info_16.incremental_B=(((short int)packet[16] & 0xff03) << 8)+((short int)packet[17]);
	info_16.standby_2=((packet[18] & 0xf0) >> 4);
	
}


void demode_GNSS_calling(unsigned char *packet)
{
	info_17.ID_info_code=((packet[0] & 0xfc) >> 2);
	info_17.sendtimes=(packet[0] & 0x03);
	info_17.ID_sender_MMSI=((int)packet[1] << 22)+((int)packet[2] << 14)+((int)packet[3] << 6)+(((int)packet[4] & 0xfffffffc) >> 2);
	info_17.standby_1=(packet[4] & 0x03);
	info_17.longitude=((int)packet[5] << 10)+((int)packet[6] << 2)+(((int)packet[7] & 0xffffffc0) >> 6);
	info_17.latitude=(((int)packet[7] & 0xffffff3f) << 11)+((int)packet[8] << 3)+(((int)packet[9] & 0xffffffe0) >> 5);
	info_17.standby_2=(packet[9] & 0xffffff1f);
	for(i=0;i<92;i++)
	{
		info_17.date[i]=packet[10+i];
	}
	ais_struct.lUserID=info_17.ID_sender_MMSI;
	ais_struct.dLongitude = (double) (info_17.longitude / 600000.00);
	ais_struct.dLatitude = (double) (info_17.latitude / 600000.00);
}

void demode_location_B(unsigned char *packet)
{
	info_18.ID_info_code=((packet[0] & 0xfc) >> 2);
	info_18.sendtimes=(packet[0] & 0x03);
	info_18.ID_sender_MMSI=((int)packet[1] << 22)+((int)packet[2] << 14)+((int)packet[3] << 6)+(((int)packet[4] & 0xfffffffc) >> 2);
	info_18.region_keep_1=((packet[4] & 0x03) << 6)+((packet[5] & 0xfc) >> 2);
	info_18.speed_earth=((short int)packet[5] & 0x03)+((short int)packet[6]);
	info_18.location_precision=(packet[7] & 0x80);
	info_18.longitude=(((int)packet[7] & 0xffffff7f) << 21)+((int)packet[8] << 13)+((int)packet[9] << 5)+(((int)packet[10] & 0xfffffff8) >> 3);
	info_18.latitude=(((int)packet[10] & 0xffffff07) << 24)+((int)packet[11] << 16)+((int)packet[12] << 8)+((int)packet[13]);
	info_18.course_earth=((short int)packet[14] << 4)+(((short int)packet[15] & 0xfff0) >> 4);
	info_18.course_real=(((short int)packet[15] & 0xff0f) << 5)+(((short int)packet[16] & 0xfff8) >> 3);
	info_18.mark_time=((packet[16] & 0x07) << 3)+((packet[17] & 0xe0) >> 5);
	info_18.region_keep_2=((packet[17] & 0x1e) >> 1);
	info_18.standby=(packet[17] & 0x01)+(packet[18] & 0xe0);
	info_18.mark_rami=((packet[18] & 0x10) >> 4);
	info_18.mark_comm_condition=((packet[18] & 0x08) >> 3);
	info_18.comm_condition=((int)packet[18] & 0x07)+((int)packet[19])+((int)packet[20]);
	ais_struct.lUserID=info_18.ID_sender_MMSI;
	ais_struct.iSOG=info_18.speed_earth;
	ais_struct.dLongitude = (double) (info_18.longitude/600000.00);
	ais_struct.dLatitude = (double) (info_18.latitude/600000.00);
	ais_struct.dCOG = (double)info_18.course_earth;
	ais_struct.iVerityHeading=info_18.course_real;
}


void demode_expand_locationB(unsigned char *packet)
{
	info_19.ID_info_code=((packet[0] & 0xfc) >> 2);
	info_19.sendtimes=(packet[0] & 0x03);
	info_19.ID_sender_MMSI=((int)packet[1] << 22)+((int)packet[2] << 14)+((int)packet[3] << 6)+(((int)packet[4] & 0xfffffffc) >> 2);
	info_19.region_keep_1=((packet[4] & 0x03) << 6)+((packet[5] & 0xfc) >> 2);
	info_19.speed_earth=(((short int)packet[5] & 0x03) <<8)+(packet[6]);
	info_19.location_precision=(packet[7] & 0x80);
	info_19.longitude=(((int)packet[7] & 0xffffffef) << 21)+((int)packet[8] << 13)+((int)packet[8] << 5)+(((int)packet[10] & 0xfffffff8) >> 3);
	info_19.latitude=(((int)packet[10] & 0xffffff07) << 24)+((int)packet[11] << 16)+((int)packet[12] << 8)+((int)packet[13]);
	info_19.course_earth_COG=((short int)packet[14] << 4)+(((short int)packet[15] & 0xfff0) >> 4);
	info_19.course_real=(((short int)packet[15] & 0xff0f) << 5)+(((short int)packet[16] & 0xfff8) >> 3);
	info_19.mark_time=((packet[16] & 0x07) << 4)+((packet[17] & 0xe0) >> 5);
	info_19.region_keep_2=((packet[17] & 0x1e) >> 1);
	for(i=0,j=0;i<20;j++,i=4*j)
	{
		info_19.name[i]=((packet[17+3*j] & 0x01) << 5)+((packet[18+3*j] & 0xf8) >> 3);
		info_19.name[i+1]=((packet[18+3*j] & 0x07) << 3)+((packet[19+3*j] & 0xe0) >> 5);
		info_19.name[i+2]=((packet[19+3*j] & 0x1f) << 1)+((packet[20+3*j] & 0x80) >> 7);
		info_19.name[i+3]=((packet[20+3*j] & 0x7e) >> 1);
	}
	for(i=0;i<20;i++)
	{
		info_19.name[i]=tarnsform_6to_8(info_19.name[i]);
	}
	
	info_19.ship_type=((packet[33] & 0x01) << 7)+((packet[33] & 0xfe) >> 1);
	info_19.scale=(((int)packet[33] & 0xffffff01) << 29)+((int)packet[34] << 21)+((int)packet[35] << 13)+((int)packet[36] << 5)+(((int)packet[37] & 0xfffffff8) >> 3);	
	info_19.location_type=((packet[37] & 0x07) << 1)+((packet[37] & 0x80) >> 7);
	info_19.mark_rami=((packet[37] & 0x40) >> 6);
	info_19.date_terminal_condition=((packet[37] & 0x20) >> 5);
	info_19.standby=(packet[37] & 0x1f);
	ais_struct.lUserID=info_19.ID_sender_MMSI;
	ais_struct.iSOG = info_19.speed_earth;
	ais_struct.dLongitude = (double) (info_19.longitude/600000.00);
	ais_struct.dLatitude = (double) (info_19.latitude/600000.00);
	ais_struct.dCOG = (double)info_19.course_earth_COG;
	ais_struct.iVerityHeading=info_19.course_real;
	for(i=0;i<20;i++)
	{
		ais_struct.cpName[i]=info_19.name[i];
	}
	ais_struct.bShipAndFreightType=info_19.ship_type;
	ais_struct.ichangdu = (info_19.scale & 0x000001ff) + ((info_19.scale & 0x0003fe00) >> 9);
	ais_struct.ikuandu = ((info_19.scale & 0x000fc0000) >> 18) + ((info_19.scale & 0x3f000000) >> 24);
}

void demode_datalink(unsigned char *packet)
{
	info_20.ID_info_code=((packet[0] & 0xfc) >> 2);
	info_20.sendtimes=(packet[0] & 0x03);
	info_20.ID_sender_MMSI=((int)packet[1] << 22)+((int)packet[2] << 14)+((int)packet[3] << 6)+(((int)packet[4] & 0xfffffffc) >> 2);
	info_20.standby_1=(packet[4] & 0x03);
	info_20.time_offset_num_first=((short int)packet[5] << 4)+(((short int)packet[6] & 0xfff0) >> 4);
	info_20.time_num_first=(packet[6] & 0x0f);
	info_20.timeout_first=((packet[7] & 0xe0) >> 5);
	info_20.incremental_first=(((short int)packet[7] & 0xff1f) << 6)+(((short int)packet[8] & 0xfffc) >> 2);
	info_20.time_offset_num_second=(((short int)packet[8] & 0xff03) << 10)+((short int)packet[9] << 2)+(((short int)packet[10] & 0xffc0) >> 6);
	info_20.time_num_second=((packet[10] & 0x3c) >> 2);
	info_20.timeout_second=((packet[10] & 0x03) << 1)+((packet[11] & 0x80) >> 7);
	info_20.incremental_second=(((short int)packet[11] & 0xff7f) << 4)+(((short int)packet[12] & 0xf0) >> 4);
	info_20.time_offset_num_third=(((short int)packet[12] & 0xff0f) << 8)+((short int)packet[13]);
	info_20.time_num_third=((packet[14] & 0xf0) >> 4);
	info_20.timeout_third=((packet[14] & 0x0e) >> 1);
	info_20.incremental_third=(((short int)packet[14] & 0x01) << 10)+((short int)packet[15] << 2)+(((short int)packet[16] & 0xffc0) >> 6);
	info_20.time_offset_num_fourth=(((short int)packet[16] & 0xffcf) << 6)+(((short int)packet[17] & 0xfffc) >> 2);
	info_20.time_num_fourth=((packet[17] & 0x03) << 2)+((packet[18] & 0xc0) >> 6);
	info_20.timeout_fourth=((packet[18] & 0x38) >> 3);
	info_20.incremental_fourth=(((short int)packet[18] & 0xff07) << 8)+((short int)packet[19]);
	info_20.standby_2=((packet[20] & 0xfc) >> 2);
	

}

void demode_navaids_report(unsigned char *packet)
{
	info_21.ID_info_code=((packet[0] & 0xfc) >> 2);
	info_21.sendtimes=(packet[0] & 0x03);
	info_21.ID_sender_MMSI=((int)packet[1] << 22)+((int)packet[2] << 14)+((int)packet[3] << 6)+(((int)packet[4] & 0xfffffffc) >> 2);
	info_21.navigation_type=((packet[4] & 0x03) << 3)+((packet[5] & 0xe0) >> 5);
	for(i=0,j=0;i<20;j++,i=4*j)
	{
		info_21.navigation_name[i]=((packet[5+3*j] & 0x1f) << 1)+((packet[6+3*j] & 0x80) << 7);
		info_21.navigation_name[i+1]=((packet[6+3*j] & 0x7e) >> 1);
		info_21.navigation_name[i+2]=((packet[6+3*j] & 0x01) << 5)+((packet[7+3*j] & 0xf8) >> 3);
		info_21.navigation_name[i+3]=((packet[7+3*j] & 0x07) << 3)+((packet[8+3*j] & 0xe0) >> 5);
	}
	for(i=0;i<20;i++)
	{
		info_21.navigation_name[i]=tarnsform_6to_8(info_21.navigation_name[i]);
	}
	info_21.location_precision=((packet[20] & 0x10) >> 4);
	info_21.longitude=(((int)packet[20] & 0xffffff0f) << 24)+((int)packet[21] << 16)+((int)packet[22] << 8)+((int)packet[23]);
	info_21.latitude=((int)packet[24] << 19)+((int)packet[25] << 11)+((int)packet[26] << 3)+(((int)packet[27] & 0xffffffe0) >> 5);
	info_21.scale=(((int)packet[27] & 0xffffff1f) << 25)+((int)packet[28] << 17)+((int)packet[29] << 9)+((int)packet[30] << 1)+(((int)packet[31] & 0xffffff80) >> 7);
	info_21.location_type=((packet[31] & 0x78) >> 3);
	info_21.mark_time=((packet[31] & 0x07) << 3)+((packet[32] & 0xe0) >> 5);
	info_21.location_offset=((packet[32] & 0x10) >> 4);
	info_21.region_keep=((packet[32] & 0x0f) << 4)+((packet[33] & 0xf0) >> 4);
	info_21.mark_rami=((packet[33] & 0x080) >> 3);
	info_21.standby=(packet[33] & 0x007);//报文说明不用设定为零，
	
	ais_struct.lUserID=info_21.ID_sender_MMSI;
	ais_struct.dLongitude = (double) (info_21.longitude/600000.00);
	ais_struct.dLatitude = (double) (info_21.latitude/600000.00);
}

void demode_channel_manage(unsigned char *packet)
{
	info_22.ID_info_code=((packet[0] & 0xfc) >> 2);
	info_22.sendtimes=(packet[0] & 0x03);
	info_22.ID_sender_MMSI=((int)packet[1] << 22)+((int)packet[2] << 14)+((int)packet[3] << 6)+(((int)packet[4] & 0xfffffffc) >> 2);
	info_22.standby_1=(packet[4] & 0x03);
	info_22.channel_A=((short int)packet[5] << 4)+(((short int)packet[6] & 0xfff0) >> 4);
	info_22.channel_B=(((short int)packet[6] & 0xff0f) << 8)+((short int)packet[7]);
	info_22.send_or_receive_mode=((packet[8] & 0xf0) >> 4);
	info_22.power=((packet[8] & 0x08) >> 3);
	info_22.longitude_first=(((int)packet[8] & 0xffffff07) << 15)+((int)packet[9] << 7)+(((int)packet[10] & 0xfffffffe) >> 1);
	info_22.latitude_first=(((int)packet[10] & 0xffffff01) << 16)+((int)packet[11] << 8)+((int)packet[12]);
	info_22.longitude_second=((int)packet[13] << 10)+((int)packet[14] << 2)+(((int)packet[15] & 0xffffffc0) >> 6 );
	info_22.latitude_second=(((int)packet[15] & 0xffffff3f) << 11)+((int)packet[16] << 3)+(((int)packet[17] & 0xffffffe0) >> 5 );
	info_22.mark_broadcast_information=((packet[17] & 0x10) >> 4);
	info_22.bandwidth_A=((packet[17] & 0x08) >> 3);
	info_22.bandwidth_B=((packet[17] & 0x04) >> 2);
	info_22.excessive_area=((packet[17] & 0x03) << 1)+((packet[17] & 0x80) >> 7);
	info_22.standby_2=(((int)packet[18] & 0xffffff7f) << 16)+((int)packet[19] << 8)+((int)packet[20]);
}

//消息23:群组指配命令
void groups_allocate_command(unsigned char *packet)//将位流转化为字节流
{
	info_23.ID_info_code=((packet[0] & 0xfc) >> 2);//6
	info_23.sendtimes=(packet[0]&0x03);//2
	info_23.ID_sender_MMSI=((int)packet[1] << 22)+((int)packet[2] << 14)+((int)packet[3] << 6)+(((int)packet[4] & 0xfffffffc) >> 2);
	info_23.standby_1 = (packet[4]&0x03);//2
	info_23.longitude_first = ((int)packet[5]<<10)+((int)packet[6]<<2)+(((int)packet[7]&0xffffffc0)>>6);//18
	info_23.latitude_first = (((int)packet[7]&0xffffff3f)<<11)|((int)packet[8]<<3)|(((int)packet[9]&0xffffffe0)>>5);//17
	info_23.longitude_second = (((int)packet[9]&0xffffff1f)<<13)|((int)packet[10]<<5)|(((int)packet[11]&0xfffffff8)>>3);//18
	info_23.latitude_second = (((int)packet[11]&0xffffff07)<<14)|((int)packet[12]<<6)|(((int)packet[13]&0xfffffffc)>>2);//17

	info_23.station_type = ((packet[13]&0x03)<<2)|(packet[14]&0xc0)>>6;//4
	info_23.ship_type = ((packet[14]&0x3f)<<2)|((packet[15]&0xC0)>>6);//8

	info_23.standby_2 =((int)(packet[15])&0x3f<<14)|((int)packet[16]<<8)|((int)packet[17]);//22
	info_23.send_or_receive_mode = (packet[18]&0xC0)>>6;//2
	info_23.report_time = (packet[18]&0x3C)>>2;//4
	info_23.silence_time = (packet[18]&0x03)<<2|(packet[19]&0xC0)>>6;//4
	info_23.standby_3 = packet[19]&0x3f;//6
	//总共20个字节：20*8 = 160个字节
}

//消息24：静态数据报告A
void static_data_report_A(unsigned char *packet)
{
	info_24_A.ID_info_code=((packet[0] & 0xfc)>>2);//6
	info_24_A.sendtimes=(packet[0]&0x03);//2
    info_24_A.ID_sender_MMSI=((int)packet[1] << 22)+((int)packet[2] << 14)+((int)packet[3] << 6)+(((int)packet[4] & 0xfffffffc) >> 2);
	info_24_A.selection_number = (packet[4]&0x03);//2
	//问题是这个！！
	for(i=0;i<20;i++)
	{
		info_24_A.name[i] = packet[i+5];//不需要移位
	}
	for(i=0;i<20;i++)
	{
		info_24_A.name[i]=tarnsform_6to_8(info_24_A.name[i]);	
	}
}

//消息24：静态数据报告B
void static_data_report_B(unsigned char *packet)
{
	info_24_B.ID_info_code=((packet[0] & 0xfc)>>2);//6
	info_24_B.sendtimes=(packet[0]&0x03);//2
	info_24_B.ID_sender_MMSI=((int)packet[1] << 22)+((int)packet[2] << 14)+((int)packet[3] << 6)+(((int)packet[4] & 0xfffffffc) >> 2);
	info_24_B.selection_number = (packet[4]&0x03);//2
	info_24_B.ship_type = packet[5]&0xff;//8
	//卖主ID
	for(i=0;i<5;i++)//写40bits = 5*8
	{
		info_24_B.seller_ID[i] = packet[i+6];//packet[0]~packet[4]
	}
	info_24_B.seller_ID[5] = (packet[11]&0xC0)>>6;
	//呼叫
	for(i=0;i<5;i++)
	{
		info_24_B.calling[i] = ((packet[11+i]&0x3f)<<2)|((packet[12+i]&0xC0)>>6);
	}
	info_24_B.calling[5] = (packet[16]&0x30)>>4;
	info_24_B.scale = ((int)packet[16]&0x0f)<<26|((int)packet[17]&0xf0)<<22
		|((int)packet[17]&0x0f)<<18|((int)packet[18]&0xf0)<<14|((int)packet[18]&0x0f)<<10|((int)packet[19]&0xf0)<<6|
		((int)packet[20]&0xC0)>>2;

	info_24_B.standby = packet[20]&0x3f;
}

//消息25：单时隙二进制消息	最大168
void single_interval_binary_message(unsigned char *packet)
{
	info_25.ID_info_code=((packet[0] & 0xfc)>>2);//6
	info_25.sendtimes=(packet[0]&0x03);//2
	info_25.ID_sender_MMSI=((int)packet[1] << 22)+((int)packet[2] << 14)+((int)packet[3] << 6)+(((int)packet[4] & 0xfffffffc) >> 2);//30
	info_25.destination_instruction_sign = (packet[4]&0x02)>>1;//1
	info_25.binary_data_sign = (packet[4]&0x01)>>0;//1

	//改动
	if(info_25.destination_instruction_sign == 0)
	{
		info_25.ID_destination_MMSI = 0;
		if(info_25.binary_data_sign == 1)//16bits
		{
			info_25.date_binary[0] = get_BYTE_data(packet,40,8);//8
			info_25.date_binary[1] = get_BYTE_data(packet,48,8);//8   48+8=56
			//------------------------------
		}
		else if(info_25.binary_data_sign == 0)   
		{
			info_25.ID_destination_MMSI = 0;

			//112bits
			for(i=0,j=0;i<14,j<112;i=i+1,j=j+8)
			{
				info_25.date_binary[i] = get_BYTE_data(packet,56+j,8);//88
			}		
		}
	}

	else if(info_25.destination_instruction_sign == 1)
	{
		info_25.ID_destination_MMSI = ((int)packet[5] << 22)+((int)packet[6] << 14)+((int)packet[7] << 6)+(((int)packet[8] & 0xfffffffc) >> 2);//30;
		if(info_25.binary_data_sign == 1)//16bits
		{
			//格式11   16
			info_25.date_binary[0] = get_BYTE_data(packet,70,8);//8
			info_25.date_binary[1] = get_BYTE_data(packet,78,8);//8   78+8=88
		}

		else if(info_25.binary_data_sign == 0)//
		{
			//格式10	 82
			for(i=0,j=0;i<10,j<80;i=i+1,j=j+8)
			{
				info_25.date_binary[i] = get_BYTE_data(packet,70+j,8);//70+j到142
			}
			info_25.date_binary[10] = get_BYTE_data(packet,150,2);//  
		}
	}

}

//消息26：带有通信状态的多时隙的二进制消息
void many_interval_binary_message(unsigned char *packet)
{
	info_26.ID_info_code=((packet[0]&0xfc)>>2);//6
	info_26.sendtimes=(packet[0]&0x03);//2
	info_26.ID_sender_MMSI=((int)packet[1] << 22)+((int)packet[2] << 14)+((int)packet[3] << 6)+(((int)packet[4] & 0xfffffffc) >> 2);//30
	info_26.destination_instruction_sign = (packet[4]&0x02)>>1;//1
	info_26.binary_data_sign = (packet[4]&0x01)>>0;//1

	//目的地ID1
	if(info_26.destination_instruction_sign == 0)
	{
		info_26.ID_destination_MMSI = 0;//0
		if(info_26.binary_data_sign == 1)//16bits    0
		{
			info_26.date_binary[0] = get_BYTE_data(packet,40,8);//8
			info_26.date_binary[1] = get_BYTE_data(packet,48,8);//8   
			for(i=0,j=0;i<28,j<224;i=i+1,j=j+8)
			{
				info_26.time_internal_2_add[i] = get_BYTE_data(packet,56+j,8);//28*8
			}
			for(i=0,j=0;i<28,j<224;i=i+1,j=j+8)
			{
				info_26.time_internal_3_add[i] = get_BYTE_data(packet,56+224+j,8);//28*8
			}
			for(i=0,j=0;i<28,j<224;i=i+1,j=j+8)
			{
				info_26.time_internal_4_add[i] = get_BYTE_data(packet,56+224*2+j,8);//28*8
			}
			for(i=0,j=0;i<28,j<224;i=i+1,j=j+8)
			{
				info_26.time_internal_5_add[i] = get_BYTE_data(packet,56+224*3+j,8);//28*8
			}
			info_26.communication_state_choose = get_BYTE_data(packet,952,1);//1
			info_26.comm_condition = get_u32_data(packet,953,19);//19
		}
		else if(info_26.binary_data_sign == 0)   
		{
			//---------------------------------------------------------
			for(i=0,j=0;i<12,j<88;i=i+1,j=j+8)
			{
				info_26.date_binary[i] = get_BYTE_data(packet,40+j,8);//88
			}
			info_26.date_binary[12] = get_BYTE_data(packet,40+88,4);//4   128+4=132
			//-----------------------------------------------------------
			//-----------------------------------------------------------
			for(i=0,j=0;i<28,j<224;i=i+1,j=j+8)
			{
				info_26.time_internal_2_add[i] = get_BYTE_data(packet,132+j,8);//28*8
			}
			for(i=0,j=0;i<28,j<224;i=i+1,j=j+8)
			{
				info_26.time_internal_3_add[i] = get_BYTE_data(packet,132+224+j,8);//28*8
			}
			for(i=0,j=0;i<28,j<224;i=i+1,j=j+8)
			{
				info_26.time_internal_4_add[i] = get_BYTE_data(packet,132+224*2+j,8);//28*8
			}
			for(i=0,j=0;i<28,j<224;i=i+1,j=j+8)
			{
				info_26.time_internal_5_add[i] = get_BYTE_data(packet,132+224*3+j,8);//28*8
			}
			info_26.communication_state_choose = get_BYTE_data(packet,1028,1);//1
			info_26.comm_condition = get_u32_data(packet,1029,19);//19

		}
	}
	else if(info_26.destination_instruction_sign == 1)
	{
		info_26.ID_destination_MMSI = get_u32_data(packet,40,30);//30
		if(info_26.binary_data_sign == 0) 
		{
			//格式10
			//-----------------------------------------------------------
			for(i=0,j=0;i<7,j<56;i=i+1,j=j+8)
			{
				info_26.date_binary[i] = get_BYTE_data(packet,70+j,8);//88
			}
			info_26.date_binary[7] = get_BYTE_data(packet,70+56,6);//4   70+56+6 = 132
			//-----------------------------------------------------------

			for(i=0,j=0;i<28,j<224;i=i+1,j=j+8)
			{
				info_26.time_internal_2_add[i] = get_BYTE_data(packet,132+j,8);//28*8
			}
			for(i=0,j=0;i<28,j<224;i=i+1,j=j+8)
			{
				info_26.time_internal_3_add[i] = get_BYTE_data(packet,132+224+j,8);//28*8
			}
			for(i=0,j=0;i<28,j<224;i=i+1,j=j+8)
			{
				info_26.time_internal_4_add[i] = get_BYTE_data(packet,132+224*2+j,8);//28*8
			}
			for(i=0,j=0;i<28,j<224;i=i+1,j=j+8)
			{
				info_26.time_internal_5_add[i] = get_BYTE_data(packet,132+224*3+j,8);//28*8
			}
			info_26.communication_state_choose = get_BYTE_data(packet,1028,1);//1
			info_26.comm_condition = get_u32_data(packet,1029,19);//19
		}
		else if(info_26.binary_data_sign == 1)
		{
			info_26.ID_destination_MMSI = get_u32_data(packet,40,30);//30
			//-----------------------------------------------------------
			info_26.date_binary[0] = get_BYTE_data(packet,70,8);//8
			info_26.date_binary[1] = get_BYTE_data(packet,78,8);//8   78+8=86
			//-----------------------------------------------------------
			for(i=0,j=0;i<28,j<224;i=i+1,j=j+8)//i:0-27,j:0-216
			{
				info_26.time_internal_2_add[i] = get_BYTE_data(packet,86+j,8);//28*8
			}
			for(i=0,j=0;i<28,j<224;i=i+1,j=j+8)
			{
				info_26.time_internal_3_add[i] = get_BYTE_data(packet,86+224+j,8);//28*8
			}
			for(i=0,j=0;i<28,j<224;i=i+1,j=j+8)
			{
				info_26.time_internal_4_add[i] = get_BYTE_data(packet,86+224*2+j,8);//28*8
			}
			for(i=0,j=0;i<28,j<224;i=i+1,j=j+8)
			{
				info_26.time_internal_5_add[i] = get_BYTE_data(packet,86+224*3+j,8);//28*8
			}
			info_26.communication_state_choose = get_BYTE_data(packet,982,1);//1
			info_26.comm_condition = get_u32_data(packet,983,19);//19
		}
		else{}
	}
	else{}
}





/****************************************************
对发送结构体AIS——structure的初始化
****************************************************/

void initialization_ais_struct()
{
	ais_struct.lUserID = 0;
	for(i=0;i<20;i++)
	{
		ais_struct.cpNation[i] = 0;
	}
	ais_struct.lIMO = 0;
	for(i=0;i<7;i++)
	{
		ais_struct.cpCallLetters[i] = '@';
	}
	for(i=0;i<20;i++)
	{
		ais_struct.cpName[i] = '@';
	}
	ais_struct.bShipAndFreightType = 0;
	ais_struct.lETA = 0x0000063c;
	ais_struct.fShipWater = 0;
	for(i=0;i<20;i++)
	{
		ais_struct.cpDestination[i] = '@';
	}
	ais_struct.bnavigationState = 0x0f;
	ais_struct.iSOG = 0x000003ff;
	ais_struct.dLongitude = 181.00;
	ais_struct.dLatitude = 91.00;
	ais_struct.dCOG = 0x00000e10;
	ais_struct.iVerityHeading = 0x000001ff;
	ais_struct.ucbType = 0;
	ais_struct.ichangdu = 0;
	ais_struct.ikuandu = 0;
}
/*********************************************************
数据打包函数
函数头：AA 55 AA BB
数据类型： A1 A2 C1 C2 C3 C4（A代表AIS数据C代表ARCRS数据、数字代表通道号）
数据长度：0~0xff
数据：
尾： AB AB
********************************************************/
void write_info(uint8_t type)
{
	int datelength;
	datelength=sizeof(ais_struct);

	plx_send_buffer[0]='A';
	/*
	plx_send_buffer[2]=(uint8_t)(((status) &0xff00) >> 8);	
	plx_send_buffer[1]=(uint8_t)((status) &0x00ff);
	*/
	plx_send_buffer[1]=(uint8_t)((type) & 0x0f) + '0';
	plx_send_buffer[2]= sizeof(ais_struct);	
	memcpy(&(plx_send_buffer[3]),& ais_struct,datelength);
}

//uint8_t type, uint8_t *packet, uint8_t pacekt_len,AIS_STRUCT *outData
void check_AIS_massage(int  type, unsigned char *packet, int pacekt_len,unsigned char data[])
{
	//unsigned char lat[8] = {0};
	//unsigned char lon[8] = {0};
	//unsigned char data[128] = {0};
	//initialization_ais_struct();
	switch((packet[0] & 0xfc) >>2)
	{
		case 0x01:
		case 0x02:
		case 0x03:
			signdata=0x0001;
			demode_ship_location(packet);
			memcpy(data,&cinfo_1_2_3,sizeof(cinfo_1_2_3));
			//memcpy(outData,&cinfo_1_2_3,sizeof(cinfo_1_2_3));
			break;
		case 0x04: 
		case 0x0b: 					
			signdata=0x0002;
			demode_UTC_time(packet);
			memcpy(data,&info_4_11,sizeof(info_4_11));
			//memcpy(outData,&info_4_11,sizeof(info_4_11));
			break;
		case 0x05: 
			signdata=0x0003;
			demode_static_data(packet);
			memcpy(data,&info_5,sizeof(info_5));
			//memcpy(outData,&info_5,sizeof(info_5));
			break;
		case 0x06: 
			signdata=0x0004;
			demode_addressing(packet);
			memcpy(data,&info_6,sizeof(info_6));
			//memcpy(outData,&info_6,sizeof(info_6));
			break;
		case 0x07: 
			signdata=0x0005;
			demode_sefaty_comfirm(packet);
			memcpy(data,&info_7_13,sizeof(info_7_13));
			//memcpy(outData,&info_7_13,sizeof(info_7_13));
			break;
		case 0x08: 
			signdata=0x0006;
			demode_broadcast_binary(packet);
			memcpy(data,&info_8,sizeof(info_8));
			//memcpy(outData,&info_8,sizeof(info_8));
			break;
		case 0x09: 
			signdata=0x0007;
			demode_searchrescue_info(packet);
			memcpy(data,&info_9,sizeof(info_9));
			//memcpy(outData,&info_9,sizeof(info_9));
			break;
		case 0x0a: 
			signdata=0x000d;
			demode_UTCtime_calling(packet);
			memcpy(data,&info_10,sizeof(info_10));
			//memcpy(outData,&info_10,sizeof(info_10));
			break;
		case 0x0c: 
			signdata=0x0008;
			demode_addressing_safety(packet);
			memcpy(data,&info_12,sizeof(info_12));
			//memcpy(outData,&info_12,sizeof(info_12));
			break;
		case 0x0d: 
			signdata=0x0005;
			demode_sefaty_comfirm(packet);
			memcpy(data,&info_7_13,sizeof(info_7_13));
		//	memcpy(outData,&info_7_13,sizeof(info_7_13));
			break;
		case 0x0e: 
			signdata=0x0009;
			demode_safety(packet);
			memcpy(data,&info_14,sizeof(info_14));
			//memcpy(outData,&info_14,sizeof(info_14));
			break;
		case 0x0f: 
			signdata=0x000e;
			demode_calling(packet);
			memcpy(data,&info_15,sizeof(info_15));
			//memcpy(outData,&info_15,sizeof(info_15));
			break;
		case 0x10: 
			signdata=0x000f;
			demode_allocation_order(packet);
			memcpy(data,&info_16,sizeof(info_16));
			//memcpy(outData,&info_16,sizeof(info_16));
			break;
		case 0x11: 
			signdata=0x0010;
			demode_GNSS_calling(packet);
			memcpy(data,&info_17,sizeof(info_17));
			//memcpy(outData,&info_17,sizeof(info_17));
			break;
		case 0x12: 
			signdata=0x000a;
			demode_location_B(packet);
			memcpy(data,&info_18,sizeof(info_18));
			//memcpy(outData,&info_18,sizeof(info_18));
			break;
		case 0x13: 
			signdata=0x000b;
			demode_expand_locationB(packet);
			memcpy(data,&info_19,sizeof(info_19));
			//memcpy(outData,&info_19,sizeof(info_19));
			break;
		case 0x14: 
			signdata=0x0011;
			demode_datalink(packet);
			memcpy(data,&info_20,sizeof(info_20));
			//memcpy(outData,&info_20,sizeof(info_20));
			break;
		case 0x15: 
			signdata=0x000c;
			demode_navaids_report(packet);
			memcpy(data,&info_21,sizeof(info_21));
			//memcpy(outData,&info_21,sizeof(info_21));
			break;
		case 0x16: 
			signdata=0x0012;
			demode_channel_manage(packet);
			memcpy(data,&info_22,sizeof(info_22));
			//memcpy(outData,&info_22,sizeof(info_22));
			break;

		case 0x17: 
			groups_allocate_command(packet);
			memcpy(data,&info_23,sizeof(info_23));
			//memcpy(outData,&info_23,sizeof(info_23));
			break;
		case 0x18: 
			if((packet[4]&0x03) ==1)
			{
				static_data_report_A(packet);
				memcpy(data,&info_24_A,sizeof(info_24_A));
				//memcpy(outData,&info_24_A,sizeof(info_24_A));
			}else if((packet[4]&0x03) ==2)
			{
			    static_data_report_B(packet);
				memcpy(data,&info_24_B,sizeof(info_24_B));
				//memcpy(outData,&info_24_B,sizeof(info_24_B));
			}
			break;
		case 0x19: 
			single_interval_binary_message(packet);
			memcpy(data,&info_25,sizeof(info_25));
			//memcpy(outData,&info_25,sizeof(info_25));
			break;
		case 0x1A: 
			many_interval_binary_message(packet);
			memcpy(data,&info_26,sizeof(info_26));
			//memcpy(outData,&info_26,sizeof(info_26));
			break;

		default: break;
	}

}
/*
void check_HDLC(unsigned char date[],unsigned char length)
{
	unsigned char HDLC_NUM=0;
	unsigned char HDLC_code[1600];
	unsigned char send_fpga[300];
	for(i=0;i<length;i++)
	{
		for(j=0;j<8;j++)
		{
			if(date[i] & (0x80 >> j))
				{
					HDLC_code[8*i+j]=1;
				}
				else
				{
					HDLC_code[8*i+j]=0;
				}
		}
	}
	for(i=0;i<(length * 8);i++)
	{
		if((HDLC_code[i] == 1) && (HDLC_code[i+1] == 1) && (HDLC_code[i+2] == 1) && (HDLC_code[i+3] == 1) && (HDLC_code[i+4] == 1))
		{
			if((HDLC_code[i+5] == 0) && (i-HDLC_NUM) != 1)
			{
				for(j=i+5;j<(length*8);j++)
				{
					HDLC_code[j]=HDLC_code[j+1];
				}
				i=i+4;
			}
			HDLC_NUM = i;
		}
	}
	for(i=0;i<(length * 8);i++)
	{
		for(j=0;j<8;j++)
		{
			if(HDLC_code[i*8+j] == 1)
			{
				send_fpga[i]=(send_fpga[i] | (0x80 >> j));
			}
			else
			{
				
			}
		}
	}
	for(i=0;i<length;i++)
	{
		dec_buffer.buffer[i] = 0;
		if(send_fpga[i] == 0x7e)
		{
			dec_buffer.end = i;
		}
		dec_buffer.buffer[i]=send_fpga[i];
	}
}
*/

int phase_ais_packet(uint8_t type, uint8_t *packet, uint8_t packet_len,AIS_STRUCT *outData)
{
	uint16_t len_after_hdlc;
	uint16_t crc_calc, crc_actual;
	if(packet[0]==0x7e)
	{
		//ais_recv_num	++;
		/* First do a byte level bit-reverse (FPGA reuired) to prepare the HDLC decoding */
//		for(i=0;i<packet_len;i++){
//			packet[i]=BitReverse(packet[i]);
//		}
		/* Entire packet shoud do a HDLC decoding, except the 'START' byte (0x7E) */
		hdlc_dec(packet + 1, 8*(packet_len - 1), packet + 1, &len_after_hdlc);
		/* */
		len_after_hdlc = (len_after_hdlc / 8) + 1 + ((len_after_hdlc%8)?1:0);
		/* A right packet should be ended with a '0x7E'*/
		if(packet[len_after_hdlc - 1] == 0x7e){
			crc_actual = (packet[len_after_hdlc - 3] << 8) + packet[len_after_hdlc - 2];
			/* Do a CCITT CRC-16 check (except a Heading '0x7E', an ending '0x7E', and 2 CRC checksum bytes)*/
			crc_calc = ~calc_AIS_crc16(packet + 1,len_after_hdlc - 4);
			if(crc_actual == crc_calc){
				/* Reverse back when HDLC and CRC is done */
				for(i=0;i<len_after_hdlc;i++){
					packet[i]=BitReverse(packet[i]);
				}
				/* Chop off Heading, Ending, and CRC bytes and analyze */
		//		check_AIS_massage(type, packet + 1, len_after_hdlc - 4,outData);
				return 0;
			}
			else
			{
				return -2;
			}
		}
		else
		{
			return -1;
		}
		
	}
	else
	{
		return -1;
	}
	/* Don't forget to free the buffer. */
	free(packet);
}

u8 check_crc_data(uint8_t *packet,uint8_t ACARS_count)
{
	unsigned char Sendbuffer[300];
	int i =0;
	for(i=1;i<ACARS_count-2;i++)
	{
		Sendbuffer[i-1]=BitReverse(packet[i]);
	}
	acars_crc =((uint16_t)(BitReverse(packet[ACARS_count-2]) << 8) + (uint16_t)(BitReverse(packet[ACARS_count-1])));
//	acars_recv_num	++;
	if((acars_crc ==  calc_ACARS_crc16(Sendbuffer, ACARS_count-3)) && (ACARS_count > 6))
	{
		return 1;
	}
	return 0;
}


void phase_acars_packet(uint8_t type, uint8_t *packet, uint8_t packet_len)
{
	//int i = 0;
	//uint8_t ACARS_count = 0;
	//unsigned char Sendbuffer[300];
	//if(packet[0] == 0x01)
	//{
	//	for(i=3;i<packet_len;i++)
	//	{
	//		if((packet[i]==0x7f) && ((packet[i-3]==0x83)||((packet[i-3]==0x97))))
	//		{
	//			ACARS_count=i;
	//			break;
	//		}
	//	}
	//	for(i=1;i<ACARS_count-2;i++)
	//	{
	//		Sendbuffer[i-1]=BitReverse(packet[i]);
	//	}
	//	acars_crc =((uint16_t)(BitReverse(packet[ACARS_count-2]) << 8) + (uint16_t)(BitReverse(packet[ACARS_count-1])));
	//	acars_recv_num	++;
	//	//		if((acars_crc ==  calc_ACARS_crc16(Sendbuffer, ACARS_count-3)) && (ACARS_count > 6))
	//	//		{
	//	acars_right_all_num	++;
	//	if((type & 0x0f) == 0x03) 
	//	{ 
	//		acars1_right_num	++;
	//	}
	//	else if((type & 0x0f) == 0x04) 
	//	{ 
	//		acars2_right_num	++;
	//	} 
	//	else if((type & 0x0f) == 0x05) 
	//	{ 
	//		acars3_right_num	++;
	//	}
	//	else if((type & 0x0f) == 0x06) 
	//	{ 
	//		acars4_right_num	++;
	//	}
	//	for(i=0;i<ACARS_count+1;i++)
	//	{
	//		plx_send_buffer[i+3]=(packet[i] & 0x7f);
	//	}
	//	plx_send_buffer[0]=0;
	//	plx_send_buffer[1]=0;
	//	plx_send_buffer[2]=0;
	//	sum=ACARS_count+4;
	//	plx_send_buffer[0]='C';
	//	plx_send_buffer[1]=(type & 0x0f)+0x30;
	//	plx_send_buffer[2]=ACARS_count+1;	
	//	INTX_DISABLE();
	//	FPGA_Write_PCI(plx_send_buffer, 0 , ACARS_count+4);
	//	FPGA_Write_PCI(& sum, 0xff , 1);
	//	INTX_ENABLE();
	//	memset(plx_send_buffer,0,sizeof(plx_send_buffer));
	//	//		}
	//	//		else
	//	//		{
	//	//			acars_error_all_num	++;
	//	//		}
	//}
	//free(packet);
}

void check_adc_code(unsigned char comd[])
{
	
}


/***************************************************************
ACARS CRC校验
此函数为检查从FPGA读取的信息packet[0]的头六位是信息的识别位，然后调用解码函数将信息写入相应的结构体，并求信息的长度，根据消息包的格式打包发送。

****************************************************************/
int handle_message(uint8_t type, uint8_t packet_len, uint8_t *packet,AIS_STRUCT *outData)
{
	switch(type & 0xf0){
		/* 0 indicates ACARS message */
		case 0xc0:
			phase_acars_packet(type, packet, packet_len);
			break; 
		/* 0 indicates AIS message */
		case 0xa0:
			 return phase_ais_packet(type, packet, packet_len,outData);
			break;		
		case 0xb0:
			//check_adc_code(dec_buffer.comd);
			break;	

	}

}	
	

signed char tarnsform_6to_8(uint8_t dat)
{
	if(dat<0x20)
	{
		dat=dat|0x40;
		return dat;
	}
	if(dat<0x40 && dat>0x1f)
	{
		return dat;
	}
	return -1;
}
signed char tarnsform_8to_6(uint8_t dat)
{
	if(dat<0x60 && dat>0x3f)
	{
		dat=dat&0x1f;
		return dat;
	}
	if(dat<0x40 && dat>0x1f)
	{
		return dat;
	}
	return -1;
}

int PraseAISData(unsigned char* buffer, int bufferLen, AIS_STRUCT * outData)
{

	int rc = handle_message(0xa0, bufferLen, buffer,outData);
	return rc;
}