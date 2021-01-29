#include "stdafx.h"
#include "TCAS.h"
#include "string.h"
#include "massage.h"
UF_0 q_0;
UF_4 q_4;
UF_5 q_5;
UF_11 q_11;
UF_16 q_16;
UF_20 q_20;
UF_21 q_21;
UF_24 q_24;
UF_S1 q_s1;
UF_S2 q_s2;

DF_0 a_0;
DF_4 a_4;
DF_5 a_5;
DF_11 a_11;
DF_16 a_16;
DF_20 a_20;
DF_21 a_21;
DF_24 a_24;
DF_S1 a_s1;
DF_S2 a_s2;

MB_RR19_D mb_19_d;
MB_RR19_F mb_19_f;
MB_RR17_D mb_17_d;
MB_RR17_F mb_17_f;
MB_RR18 mb_18;
TTD1 ttd_1;
TTD2 ttd_2;


int send_uf(unsigned char*message, int len,unsigned char *outdata)
{
	if (len == 7 )
	{
		switch ((message[0] & 0xf8) >> 3)
		{
		case 0x00:
			decode_uf_0(message, len);
			memcpy(outdata, &q_0, sizeof(UF_0));
			return 0;
				break;
		case 0x04:
			decode_uf_4(message, len);
			memcpy(outdata, &q_4, sizeof(UF_4));
			return 4;
			break;
		case 0x05:
			decode_uf_5(message, len);
			memcpy(outdata, &q_5, sizeof(UF_5));
			return 5;
			break;
		case 0x0b:
			decode_uf_11(message, len);
			memcpy(outdata, &q_11, sizeof(UF_11));
			return 11;
			break;
		default:
			decode_uf_s1(message, len);
			memcpy(outdata, &q_s1, sizeof(UF_S1));
			return 99;
			break;
		}
	}
	else if (len == 14 && ((message[0] & 0xc0)>>3)!=0x18)
	{
		switch ((message[0] & 0xf8) >> 3)
		{
		case 0x10:
			decode_uf_16(message, len);
			memcpy(outdata, &q_16, sizeof(UF_16));
			return 16;
			break;
		case 0x14:
			decode_uf_20(message, len);
			memcpy(outdata, &q_20, sizeof(UF_20));
			return 20;
			break;
		case 0x15:
			decode_uf_21(message, len);
			memcpy(outdata, &q_21, sizeof(UF_21));
			return 21;
			break;

		default:
			decode_uf_s2(message, len);
			memcpy(outdata, &q_s2, sizeof(UF_S2));
			return 88;
			break;
		}
	}
	else if (len == 14 && ((message[0] & 0xc0) >> 3) ==0x18)
	{
			decode_uf_24(message, len);
			memcpy(outdata, &q_24, sizeof(UF_24));
			return 24;
	}
	else 
	{
		return -1;
	}
};

/******************************************         S模式询问        ************************************/

void decode_uf_0(unsigned char*message, unsigned char len)
{
	unsigned char datelength;
	datelength = sizeof(UF_0);
	memset(&q_0, 0, datelength);

	q_0.UF = ((message[0] & 0xf8) >> 3);
	q_0.standby1 = ((message[0] & 0x07));
	q_0.RL = ((message[1] & 0x80)>>7);
	q_0.standby2 = ((message[1] & 0x78) >> 3);
	q_0.AQ = ((message[1] & 0x04) >> 2);
	q_0.standby3 = ((unsigned int)(message[1] & 0x03) << 16) + ((unsigned short)(message[2] & 0xff) << 8) + ((unsigned short)(message[3] & 0xff));
	q_0.AP = ((unsigned int)(message[4] & 0xff) << 16) + ((unsigned short)(message[5] & 0xff) << 8) + ((unsigned short)(message[6] & 0xff));

};

void decode_uf_4(unsigned char*message, unsigned char len)
{
	unsigned char datelength;
	datelength = sizeof(UF_4);
	memset(&q_4, 0, datelength);

	q_4.UF = ((message[0] & 0xf8) >> 3);
	q_4.PC = ((message[0] & 0x07));
	q_4.RR = ((message[1] & 0xf8) >>3);
	q_4.DI = ((message[1] & 0x07));
	if (q_4.DI == 0)
	{
		q_4.IIS = ((message[2] & 0xf0) >> 4);
		q_4.SD_0 = ((unsigned short)(message[2] & 0x0f ) << 8) + ((unsigned short)(message[3] &0xff));
	}
	if (q_4.DI == 1)
	{
		q_4.IIS = ((message[2] & 0xf0) >> 4);
		q_4.MBS = ((message[2] & 0x0c) >> 2);
		q_4.MES = ((message[2] & 0x03) << 1) + ((message[3] & 0x80) >> 7);
		q_4.LOS = ((message[3] & 0x40) >> 6);
		q_4.RSS_2 = ((message[3] & 0x30) >> 4);
		q_4.TMS = ((message[3] & 0x0f));
	}
	if (q_4.DI > 1 && q_4.DI <7 )
	{
		q_4.SD_0 = ((unsigned short)(message[2] & 0xff) << 8) + ((unsigned short)(message[3] & 0xff));
	}
	if (q_4.DI == 7)
	{
		q_4.IIS = ((message[2] & 0xf0) >> 4);
		q_4.RSS_4 = ((message[2] & 0x0f));
		q_4.SD_1 = ((message[3] & 0x80) >> 7);
		q_4.LOS = ((message[3] & 0x40) >> 6);
		q_4.SD_2 = ((message[3] & 0x30) >> 4);
		q_4.TMS = ((message[3] & 0x0f));
	}
	else
	{
		return;
	}
	q_4.AP = ((unsigned int)(message[4] & 0xff) << 16) + ((unsigned short)(message[5] & 0xff) << 8) + ((unsigned short)(message[6] & 0xff));
};

void decode_uf_5(unsigned char*message, unsigned char len)
{
	unsigned char datelength;
	datelength = sizeof(UF_5);
	memset(&q_5, 0, datelength);

	q_5.UF = ((message[0] & 0xf8) >> 3);
	q_5.PC = ((message[0] & 0x07));
	q_5.RR = ((message[1] & 0xf8) >> 3);
	q_5.DI = ((message[1] & 0x07));
	if (q_5.DI == 0)
	{
		q_5.IIS = ((message[2] & 0xf0) >> 4);
		q_5.SD_0 = ((unsigned short)(message[2] & 0x0f) << 8) + ((unsigned short)(message[3] & 0xff));
	}
	if (q_5.DI == 1)
	{
		q_5.IIS = ((message[2] & 0xf0) >> 4);
		q_5.MBS = ((message[2] & 0x0c) >> 2);
		q_5.MES = ((message[2] & 0x03) << 1) + ((message[3] & 0x80) >> 7);
		q_5.LOS = ((message[3] & 0x40) >> 6);
		q_5.RSS_2 = ((message[3] & 0x30) >> 4);
		q_5.TMS = ((message[3] & 0x0f));
	}
	if (q_5.DI > 1 && q_4.DI <7)
	{
		q_5.SD_0 = ((unsigned short)(message[2] & 0xff) << 8) + ((unsigned short)(message[3] & 0xff));
	}
	if (q_5.DI == 7)
	{
		q_5.IIS = ((message[2] & 0xf0) >> 4);
		q_5.RSS_4 = ((message[2] & 0x0f));
		q_5.SD_1 = ((message[3] & 0x80) >> 7);
		q_5.LOS = ((message[3] & 0x40) >> 6);
		q_5.SD_2 = ((message[3] & 0x30) >> 4);
		q_5.TMS = ((message[3] & 0x0f));
	}
	else
	{
		return;
	}
	q_5.AP = ((unsigned int)(message[4] & 0xff) << 16) + ((unsigned short)(message[5] & 0xff) << 8) + ((unsigned short)(message[6] & 0xff));
};


void decode_uf_11(unsigned char*message, unsigned char len)
{
	unsigned char datelength;
	datelength = sizeof(UF_11);
	memset(&q_11, 0, datelength);

	q_11.UF = ((message[0] & 0xf8) >> 3);
	q_11.PR = get_BYTE_data(message, 5, 4);
	q_11.II = get_BYTE_data(message, 9, 4);
	q_11.standby = get_u32_data(message, 13, 19);
	q_11.AP = get_u32_data(message, 32, 24);
};


void decode_uf_16(unsigned char*message, unsigned char len)
{
	unsigned char datelength;
	datelength = sizeof(UF_16);
	memset(&q_16, 0, datelength);

	q_16.UF = ((message[0] & 0xf8) >> 3);
	q_16.standby1 = ((message[0] & 0x07));
	q_16.RL = ((message[1] & 0x80) >> 7);
	q_16.standby2 = ((message[1] & 0x78) >> 3);
	q_16.AQ = ((message[1] & 0x04) >> 2);
	q_16.standby3 = ((unsigned int)(message[1] & 0x03) << 16) + ((unsigned short)(message[2] & 0xff) << 8) + ((unsigned short)(message[3] & 0xff));
	q_16.AP = ((unsigned int)(message[4] & 0xff) << 16) + ((unsigned short)(message[5] & 0xff) << 8) + ((unsigned short)(message[6] & 0xff));

};

void decode_uf_20(unsigned char*message, unsigned char len)
{
	unsigned char datelength;
	datelength = sizeof(UF_20);
	memset(&q_20, 0, datelength);

	q_20.UF = ((message[0] & 0xf8) >> 3);
	q_20.PC = ((message[0] & 0x07));
	q_20.RR = ((message[1] & 0xf8) >> 3);
	q_20.DI = ((message[1] & 0x07));
	////q_20.SD
	if (q_20.DI == 0)
	{
		q_20.IIS = ((message[2] & 0xf0) >> 4);
		q_20.SD_0 = ((unsigned short)(message[2] & 0x0f) << 8) + ((unsigned short)(message[3] & 0xff));
	}
	if (q_20.DI == 1)
	{
		q_20.IIS = ((message[2] & 0xf0) >> 4);
		q_20.MBS = ((message[2] & 0x0c) >> 2);
		q_20.MES = ((message[2] & 0x03) << 1) + ((message[3] & 0x80) >> 7);
		q_20.LOS = ((message[3] & 0x40) >> 6);
		q_20.RSS_2 = ((message[3] & 0x30) >> 4);
		q_20.TMS = ((message[3] & 0x0f));
	}
	if (q_20.DI > 1 && q_4.DI <7)
	{
		q_20.SD_0 = ((unsigned short)(message[2] & 0xff) << 8) + ((unsigned short)(message[3] & 0xff));
	}
	if (q_20.DI == 7)
	{
		q_20.IIS = ((message[2] & 0xf0) >> 4);
		q_20.RSS_4 = ((message[2] & 0x0f));
		q_20.SD_1 = ((message[3] & 0x80) >> 7);
		q_20.LOS = ((message[3] & 0x40) >> 6);
		q_20.SD_2 = ((message[3] & 0x30) >> 4);
		q_20.TMS = ((message[3] & 0x0f));
	}
	else
	{
		return;
	}
	//q_20.MA
	q_20.DP = ((message[4] & 0x80) >> 7);
	q_20.MP = ((message[4] & 0x40) >> 6);
	q_20.M_CH = ((message[4] & 0x3f));
	q_20.SLC = ((message[5] & 0xf0) >> 4);
	q_20.MA_S[0] = ((unsigned short)(message[5] & 0x0f) << 8) + ((unsigned short)(message[6] & 0xff));
	q_20.MA_S[1] = ((unsigned short)(message[7] & 0xff) << 8) + ((unsigned short)(message[8] & 0xff));
	q_20.MA_S[2] = ((unsigned short)(message[9] & 0xff) << 8) + ((unsigned short)(message[10] & 0xff));
	q_20.AP = ((unsigned int)(message[11] & 0xff) << 16) + ((unsigned short)(message[12] & 0xff) << 8) + ((unsigned short)(message[13] & 0xff));
};

void decode_uf_21(unsigned char*message, unsigned char len)
{
	unsigned char datelength;
	datelength = sizeof(UF_21);
	memset(&q_21, 0, datelength);

	q_21.UF = ((message[0] & 0xf8) >> 3);
	q_21.PC = ((message[0] & 0x07));
	q_21.RR = ((message[1] & 0xf8) >> 3);
	q_21.DI = ((message[1] & 0x07));
	////q_21.SD
	if (q_21.DI == 0)
	{
		q_21.IIS = ((message[2] & 0xf0) >> 4);
		q_21.SD_0 = ((unsigned short)(message[2] & 0x0f) << 8) + ((unsigned short)(message[3] & 0xff));
	}
	if (q_21.DI == 1)
	{
		q_21.IIS = ((message[2] & 0xf0) >> 4);
		q_21.MBS = ((message[2] & 0x0c) >> 2);
		q_21.MES = ((message[2] & 0x03) << 1) + ((message[3] & 0x80) >> 7);
		q_21.LOS = ((message[3] & 0x40) >> 6);
		q_21.RSS_2 = ((message[3] & 0x30) >> 4);
		q_21.TMS = ((message[3] & 0x0f));
	}
	if (q_21.DI > 1 && q_4.DI <7)
	{
		q_21.SD_0 = ((unsigned short)(message[2] & 0xff) << 8) + ((unsigned short)(message[3] & 0xff));
	}
	if (q_21.DI == 7)
	{
		q_21.IIS = ((message[2] & 0xf0) >> 4);
		q_21.RSS_4 = ((message[2] & 0x0f));
		q_21.SD_1 = ((message[3] & 0x80) >> 7);
		q_21.LOS = ((message[3] & 0x40) >> 6);
		q_21.SD_2 = ((message[3] & 0x30) >> 4);
		q_21.TMS = ((message[3] & 0x0f));
	}
	else
	{
		return;
	}
	//q_21.MA
	q_21.DP = ((message[4] & 0x80) >> 7);
	q_21.MP = ((message[4] & 0x40) >> 6);
	q_21.M_CH = ((message[4] & 0x3f));
	q_21.SLC = ((message[5] & 0xf0) >> 4);
	q_21.MA_S[0] = ((unsigned short)(message[5] & 0x0f) << 8) + ((unsigned short)(message[6] & 0xff));
	q_21.MA_S[1] = ((unsigned short)(message[7] & 0xff) << 8) + ((unsigned short)(message[8] & 0xff));
	q_21.MA_S[2] = ((unsigned short)(message[9] & 0xff) << 8) + ((unsigned short)(message[10] & 0xff));
	q_21.AP = ((unsigned int)(message[11] & 0xff) << 16) + ((unsigned short)(message[12] & 0xff) << 8) + ((unsigned short)(message[13] & 0xff));
};
void decode_uf_24(unsigned char*message, unsigned char len)
{
	unsigned char datelength;
	datelength = sizeof(UF_24);
	memset(&q_24, 0, datelength);
	unsigned char i = 0;


	q_24.UF = (get_BYTE_data(message, 0, 2)<<3);
	q_24.RC = get_BYTE_data(message, 2, 2);
	q_24.NC = get_BYTE_data(message, 4, 4);
	for (i = 0; i < 10; i++)
	{
		q_24.MC[i] = get_BYTE_data(message, 8 + i* 8, 8);
	}
	q_24.AP = get_u32_data(message, 88, 24);
};

void decode_uf_s1(unsigned char*message, unsigned char len)
{
	unsigned char datalength;
	datalength = sizeof(UF_S1);
	memset(&q_s1, 0, datalength);

	q_s1.UF = get_BYTE_data(message, 0, 5);
	q_s1.standby = get_u32_data(message, 5, 27);
	q_s1.AP = get_u32_data(message, 32, 24);
};

void decode_uf_s2(unsigned char*message, unsigned char len)
{
	unsigned char datalength;
	datalength = sizeof(UF_S2);
	memset(&q_s2, 0, datalength);
	unsigned char i;

	q_s2.UF = get_BYTE_data(message, 0, 5);
	for (i = 0; i < 10; i++)
	{
		q_s2.standby[i] = get_BYTE_data(message, 5 + i * 8, 8);
	}
	q_s2.standby[10] = get_BYTE_data(message, 85, 3);
	q_s2.AP = get_u32_data(message, 88, 24);
};

/******************************************         S模式应答        ************************************/

int receive_df(unsigned char*message, int len,unsigned char*outdata)
{
	if (len == 7)
	{
		switch ((message[0] & 0xf8) >> 3)
		{
		case 0x00:
			decode_df_0(message, len);
			memcpy(outdata, &a_0, sizeof(DF_0));
			return 0;
			break;
		case 0x04:
			decode_df_4(message, len);
			memcpy(outdata, &a_4, sizeof(DF_4));
			return 4;
			break;
		case 0x05:
			decode_df_5(message, len);
			memcpy(outdata, &a_5, sizeof(DF_5));
			return 5;
			break;
		case 0x0b:
			decode_df_11(message, len);
			memcpy(outdata, &a_11, sizeof(DF_11));
			return 11;
			break;
		default:
			decode_df_s1(message, len);
			memcpy(outdata, &a_s1, sizeof(DF_S1));
			return 99;
			break;
		}
	}
	else if (len == 14 && ((message[0] & 0xc0) >> 3) != 0x18)
	{
		switch ((message[0] & 0xf8) >> 3)
		{
		case 0x10:
			decode_df_16(message, len);
			memcpy(outdata, &a_16, sizeof(DF_16));
			return 16;
			break;
		case 0x14:
			decode_df_20(message, len);
			memcpy(outdata, &a_20, sizeof(DF_20));
			return 20;
			break;
		case 0x15:
			decode_df_21(message, len);
			memcpy(outdata, &a_21, sizeof(DF_21));
			return 21;
			break;			
		default:
			decode_df_s2(message, len);
			memcpy(outdata, &a_s2, sizeof(DF_S2));
			return 88;
			break;
		}
	}
	else if (len == 14 && ((message[0] & 0xc0) >> 3) == 0x18)
	{
		decode_df_24(message, len);
		memcpy(outdata, &a_24, sizeof(DF_24));
		return 24;
	}
	else
	{
		return -1;
	}
	//return 0;
};

void decode_df_0(unsigned char*message, unsigned char len)
{
	unsigned char datelength;
	datelength = sizeof(DF_0);
	memset(&a_0, 0, datelength);

	a_0.DF = get_BYTE_data(message, 0, 5);
	a_0.VS = get_BYTE_data(message, 5, 1);
	a_0.standby1 = get_BYTE_data(message, 6, 7);
	a_0.RI = get_BYTE_data(message, 13, 4);
	a_0.standby2 = get_BYTE_data(message, 17, 2);
	a_0.AC = get_u16_data(message, 19, 13);
	a_0.AP = get_u32_data(message, 32, 24);
};

void decode_df_4(unsigned char*message, unsigned char len)
{
	unsigned char datalength;
	datalength = sizeof (DF_4);
	memset(&a_4, 0, datalength);
	a_4.DF = get_BYTE_data(message, 0, 5);
	a_4.FS = get_BYTE_data(message, 5, 3);
	a_4.DR = get_BYTE_data(message, 8, 5);
	a_4.IIS = get_BYTE_data(message, 13, 4);
	a_4.IDS = get_BYTE_data(message, 17, 2);
	a_4.AC = get_u16_data(message, 19, 13);
	a_4.AP = get_u32_data(message, 32, 24);
};

void decode_df_5(unsigned char*message, unsigned char len)
{
	unsigned char datalength;
	datalength = sizeof (DF_5);
	memset(&a_5, 0, datalength);
	a_5.DF = get_BYTE_data(message, 0, 5);
	a_5.FS = get_BYTE_data(message, 5, 3);
	a_5.DR = get_BYTE_data(message, 8, 5);
	a_5.IIS = get_BYTE_data(message, 13, 4);
	a_5.IDS = get_BYTE_data(message, 17, 2);
	a_5.ID = get_u16_data(message, 19, 13);
	a_5.AP = get_u32_data(message, 32, 24);
};

void decode_df_11(unsigned char *message, unsigned char len)
{
	unsigned char datalength;
	datalength = sizeof(DF_11);
	memset(&a_11, 0, datalength);

	a_11.DF = get_BYTE_data(message, 0, 5);
	a_11.CA = get_BYTE_data(message, 5, 3);
	a_11.AA = get_u32_data(message, 8, 24);
	a_11.AP = get_u32_data(message, 32, 24);
};

void decode_df_16(unsigned char*message, unsigned char len)
{
	unsigned char datelength;
	datelength = sizeof(DF_16);
	memset(&a_16, 0, datelength);
	unsigned char i;
	a_16.DF = get_BYTE_data(message, 0, 5);
	a_16.VS = get_BYTE_data(message, 5, 1);
	a_16.standby1 = get_BYTE_data(message, 6, 7);
	a_16.RI = get_BYTE_data(message, 13, 4);
	a_16.standby2 = get_BYTE_data(message, 17, 2);
	a_16.AC = get_u16_data(message, 19, 13);
	for (i = 0; i < 7; i++)
	{
		a_16.MV[i] = get_BYTE_data(message, 32 + i * 8, 8);
	};
	a_16.AP = get_u32_data(message, 88, 24);
};

void decode_df_20(unsigned char*message, unsigned char len)
{
	int i = 0;
	unsigned char datalength;
	datalength = sizeof (DF_20);
	memset(&a_20, 0, datalength);

	unsigned char dt_mb_rr17_d;
	dt_mb_rr17_d = sizeof (MB_RR17_D);
	unsigned char dt_mb_rr17_f;
	dt_mb_rr17_f = sizeof (MB_RR17_F);
	
	unsigned char dt_mb_rr19_d;
	dt_mb_rr19_d = sizeof (MB_RR19_D);
	unsigned char dt_mb_rr19_f;
	dt_mb_rr19_f = sizeof (MB_RR19_F);

	unsigned char dt_mb_rr18;
	dt_mb_rr18 = sizeof (MB_RR18);

	unsigned char dt_ttd1;
	dt_ttd1 = sizeof(TTD1);
	unsigned char dt_ttd2;
	dt_ttd2 = sizeof(TTD2);

	a_20.DF = get_BYTE_data(message, 0, 5);
	a_20.FS = get_BYTE_data(message, 5, 3);
	a_20.DR = get_BYTE_data(message, 8, 5);
	a_20.IIS = get_BYTE_data(message, 13, 4);
	a_20.IDS = get_BYTE_data(message, 17, 2);
	a_20.AC = get_u16_data(message, 19, 13);

	if (q_4.RR == 19 || q_5.RR == 19 || q_20.RR == 19 || q_21.RR == 19)
	{
		mb_19_d.BDS1 = get_BYTE_data(message, 32, 4);
		mb_19_d.BDS2 = get_BYTE_data(message, 36, 4);
		mb_19_d.ARA = get_u16_data(message, 40, 14);
		mb_19_d.RAC_D = get_BYTE_data(message, 54, 1);
		mb_19_d.RAC_U = get_BYTE_data(message, 55, 1);
		mb_19_d.RAC_L = get_BYTE_data(message, 56, 1);
		mb_19_d.RAC_R = get_BYTE_data(message, 57, 1);
		mb_19_d.RAT = get_BYTE_data(message, 58, 1);
		mb_19_d.MTE = get_BYTE_data(message, 59, 1);
		mb_19_d.TTI = get_BYTE_data(message, 60, 2);
		if (mb_19_d.TTI == 1)
		{
			ttd_1.TID1_d = get_u32_data(message, 62, 24);
			ttd_1.TID1_0 = get_BYTE_data(message, 86, 2);
			_memccpy(&mb_19_d.TTD, &ttd_1, 0, dt_ttd1);
		}
		else if (mb_19_d.TTI == 2)
		{
			ttd_2.TID2_H = get_u16_data(message, 62, 13);
			ttd_2.TID2_S = get_BYTE_data(message, 75, 7);
			ttd_2.TID2_P = get_BYTE_data(message, 82, 6);
			_memccpy(&mb_19_d.TTD, &ttd_2, 0, dt_ttd2);
		}
		else
		{
			return;
		}
		_memccpy(&a_20.MB, &mb_19_d, 0, dt_mb_rr19_d);	
	}
	else if (q_4.RR == 19 || q_5.RR == 19 || q_20.RR == 19 || q_21.RR == 19)
	{
		mb_19_f.BDS1 = get_BYTE_data(message, 32, 4);
		mb_19_f.BDS2 = get_BYTE_data(message, 36, 4);
		mb_19_f.ARA = get_u16_data(message, 40, 10);
		mb_19_f.ARA_0 = get_BYTE_data(message, 50, 4);
		mb_19_f.RAC_D = get_BYTE_data(message, 54, 1);
		mb_19_f.RAC_U = get_BYTE_data(message, 55, 1);
		mb_19_f.RAC_L = get_BYTE_data(message, 56, 1);
		mb_19_f.RAC_R = get_BYTE_data(message, 57, 1);
		mb_19_f.standby= get_u32_data(message, 58, 30);
		_memccpy(&a_20.MB, &mb_19_f, 0, dt_mb_rr19_f);
	}
	else if (q_4.RR == 17 || q_5.RR == 17 || q_20.RR == 17 || q_21.RR == 17)
	{
		mb_17_d.BDS1 = get_BYTE_data(message, 32, 4);
		mb_17_d.BDS2 = get_BYTE_data(message, 36, 4);
		mb_17_d.standby1 = get_BYTE_data(message, 40, 7);
		mb_17_d.TCAS_1 = get_BYTE_data(message, 47, 1);
		mb_17_d.standby2 = get_u32_data(message, 48, 20);
		mb_17_d.TCAS_2 = get_BYTE_data(message, 68, 1);
		mb_17_d.TCAS_3 = get_BYTE_data(message, 69, 1);
		mb_17_d.TCAS_4 = get_BYTE_data(message, 70, 1);
		mb_17_d.standby3 = get_u32_data(message, 71, 17);
		_memccpy(&a_20.MB, &mb_17_d, 0, dt_mb_rr17_d);
	}
	else if (q_4.RR == 17 || q_5.RR == 17 || q_20.RR == 17 || q_21.RR == 17)
	{
		mb_17_f.BDS1 = get_BYTE_data(message, 32, 4);
		mb_17_f.BDS2 = get_BYTE_data(message, 36, 4);
		mb_17_f.standby1 = get_BYTE_data(message, 40, 7);
		mb_17_f.TCAS_1 = get_BYTE_data(message, 47, 1);
		mb_17_f.standby2 = get_u32_data(message, 48, 20);
		mb_17_f.analysis = get_BYTE_data(message, 68, 2);
		mb_17_f.TCAS_2= get_BYTE_data(message, 70, 1);
		mb_17_f.standby3 = get_u32_data(message, 77, 17);
		_memccpy(&a_20.MB, &mb_17_f, 0, dt_mb_rr17_f);
	}
	else if (q_4.RR == 18 && q_4.DI != 7 || q_4.DI == 7 && q_4.RSS_4 == 0 || q_5.RR == 18 && q_5.DI != 7 || q_5.DI == 7 && q_5.RSS_4 == 0 || q_20.RR == 18 && q_20.DI != 7 || q_20.DI == 7 && q_20.RSS_4 == 0 || q_21.RR == 18 && q_21.DI != 7 || q_21.DI == 7 && q_21.RSS_4 == 0)
	{
		mb_18.BDS1 = get_BYTE_data(message, 32, 4);
		mb_18.BDS2 = get_BYTE_data(message, 36, 4);
		for (i = 0; i < 8; i++)
		{
			mb_18.IC[i] = get_BYTE_data(message, 40 + i * 6, 6);
		}
		_memccpy(&a_20.MB, &mb_18, 0, dt_mb_rr18);
	}
	else
	{
		return;
	}
	a_20.AP = get_u32_data(message, 88, 24);
};

void decode_df_21(unsigned char*message, unsigned char len)
{
	int i = 0;
	unsigned char datalength;
	datalength = sizeof (DF_21);
	memset(&a_21, 0, datalength);

	unsigned char dt_mb_rr17_d;
	dt_mb_rr17_d = sizeof (MB_RR17_D);
	unsigned char dt_mb_rr17_f;
	dt_mb_rr17_f = sizeof (MB_RR17_F);

	unsigned char dt_mb_rr19_d;
	dt_mb_rr19_d = sizeof (MB_RR19_D);
	unsigned char dt_mb_rr19_f;
	dt_mb_rr19_f = sizeof (MB_RR19_F);

	unsigned char dt_mb_rr18;
	dt_mb_rr18 = sizeof (MB_RR18);

	unsigned char dt_ttd1;
	dt_ttd1 = sizeof(TTD1);
	unsigned char dt_ttd2;
	dt_ttd2 = sizeof(TTD2);

	a_21.DF = get_BYTE_data(message, 0, 5);
	a_21.FS = get_BYTE_data(message, 5, 3);
	a_21.DR = get_BYTE_data(message, 8, 5);
	a_21.IIS = get_BYTE_data(message, 13, 4);
	a_21.IDS = get_BYTE_data(message, 17, 2);
	a_21.ID = get_u16_data(message, 19, 13);

	if (q_4.RR == 19 || q_5.RR == 19 || q_20.RR == 19 || q_21.RR == 19)
	{
		mb_19_d.BDS1 = get_BYTE_data(message, 32, 4);
		mb_19_d.BDS2 = get_BYTE_data(message, 36, 4);
		mb_19_d.ARA = get_u16_data(message, 40, 14);
		mb_19_d.RAC_D = get_BYTE_data(message, 54, 1);
		mb_19_d.RAC_U = get_BYTE_data(message, 55, 1);
		mb_19_d.RAC_L = get_BYTE_data(message, 56, 1);
		mb_19_d.RAC_R = get_BYTE_data(message, 57, 1);
		mb_19_d.RAT = get_BYTE_data(message, 58, 1);
		mb_19_d.MTE = get_BYTE_data(message, 59, 1);
		mb_19_d.TTI = get_BYTE_data(message, 60, 2);
		if (mb_19_d.TTI == 1)
		{
			ttd_1.TID1_d = get_u32_data(message, 62, 24);
			ttd_1.TID1_0 = get_BYTE_data(message, 86, 2);
			_memccpy(&mb_19_d.TTD, &ttd_1, 0, dt_ttd1);
		}
		else if (mb_19_d.TTI == 2)
		{
			ttd_2.TID2_H = get_u16_data(message, 62, 13);
			ttd_2.TID2_S = get_BYTE_data(message, 75, 7);
			ttd_2.TID2_P = get_BYTE_data(message, 82, 6);
			_memccpy(&mb_19_d.TTD, &ttd_2, 0, dt_ttd2);
		}
		else
		{
			return;
		}
		_memccpy(&a_21.MB, &mb_19_d, 0, dt_mb_rr19_d);
	}
	else if (q_4.RR == 19 || q_5.RR == 19 || q_20.RR == 19 || q_21.RR == 19)
	{
		mb_19_f.BDS1 = get_BYTE_data(message, 32, 4);
		mb_19_f.BDS2 = get_BYTE_data(message, 36, 4);
		mb_19_f.ARA = get_u16_data(message, 40, 10);
		mb_19_f.ARA_0 = get_BYTE_data(message, 50, 4);
		mb_19_f.RAC_D = get_BYTE_data(message, 54, 1);
		mb_19_f.RAC_U = get_BYTE_data(message, 55, 1);
		mb_19_f.RAC_L = get_BYTE_data(message, 56, 1);
		mb_19_f.RAC_R = get_BYTE_data(message, 57, 1);
		mb_19_f.standby = get_u32_data(message, 58, 30);
		_memccpy(&a_21.MB, &mb_19_f, 0, dt_mb_rr19_f);
	}
	else if (q_4.RR == 17 || q_5.RR == 17 || q_20.RR == 17 || q_21.RR == 17)
	{
		mb_17_d.BDS1 = get_BYTE_data(message, 32, 4);
		mb_17_d.BDS2 = get_BYTE_data(message, 36, 4);
		mb_17_d.standby1 = get_BYTE_data(message, 40, 7);
		mb_17_d.TCAS_1 = get_BYTE_data(message, 47, 1);
		mb_17_d.standby2 = get_u32_data(message, 48, 20);
		mb_17_d.TCAS_2 = get_BYTE_data(message, 68, 1);
		mb_17_d.TCAS_3 = get_BYTE_data(message, 69, 1);
		mb_17_d.TCAS_4 = get_BYTE_data(message, 70, 1);
		mb_17_d.standby3 = get_u32_data(message, 71, 17);
		_memccpy(&a_21.MB, &mb_17_d, 0, dt_mb_rr17_d);
	}
	else if (q_4.RR == 17 || q_5.RR == 17 || q_20.RR == 17 || q_21.RR == 17)
	{
		mb_17_f.BDS1 = get_BYTE_data(message, 32, 4);
		mb_17_f.BDS2 = get_BYTE_data(message, 36, 4);
		mb_17_f.standby1 = get_BYTE_data(message, 40, 7);
		mb_17_f.TCAS_1 = get_BYTE_data(message, 47, 1);
		mb_17_f.standby2 = get_u32_data(message, 48, 20);
		mb_17_f.analysis = get_BYTE_data(message, 68, 2);
		mb_17_f.TCAS_2 = get_BYTE_data(message, 70, 1);
		mb_17_f.standby3 = get_u32_data(message, 71, 17);
		_memccpy(&a_21.MB, &mb_17_f, 0, dt_mb_rr17_f);
	}
	else if (q_4.RR == 18 && q_4.DI != 7 || q_4.DI == 7 && q_4.RSS_4 == 0 || q_5.RR == 18 && q_5.DI != 7 || q_5.DI == 7 && q_5.RSS_4 == 0 || q_20.RR == 18 && q_20.DI != 7 || q_20.DI == 7 && q_20.RSS_4 == 0 || q_21.RR == 18 && q_21.DI != 7 || q_21.DI == 7 && q_21.RSS_4 == 0)
	{
		mb_18.BDS1 = get_BYTE_data(message, 32, 4);
		mb_18.BDS2 = get_BYTE_data(message, 36, 4);
		for (i = 0; i < 8; i++)
		{
			mb_18.IC[i] = get_BYTE_data(message, 40+i*6, 6);
		}
		_memccpy(&a_21.MB, &mb_18, 0, dt_mb_rr18);
	}
	else
	{
		return;
	}
	a_21.AP = get_u32_data(message, 88, 24);
};

void decode_df_24(unsigned char*message, unsigned char len)
{
	unsigned char datalength;
	datalength = sizeof(DF_24);
	memset(&a_24, 0, datalength);
	//unsigned char i;
//	unsigned char x = { 000 };

	a_24.DF = (get_BYTE_data(message, 0, 2) << 3) ;
	a_24.standby = get_BYTE_data(message, 2, 1);
	a_24.KE = get_BYTE_data(message, 3, 1);
	a_24.ND = get_BYTE_data(message, 4, 4);
	//for (i = 0; i < 10; i++)
	//{
	//	a_24.MD[i] = get_BYTE_data(message, 8 + i * 8, 8);
	//}

	get_sting_data(message,(char*)a_24.MD,8,8*10 );
	a_24.AP = get_u32_data(message, 88, 24);
};

void decode_df_s1(unsigned char*message, unsigned char len)
{
	unsigned char datalength;
	datalength = sizeof(DF_S1);
	memset(&a_s1, 0, datalength);

	a_s1.DF = get_BYTE_data(message, 0, 5);
	a_s1.standby = get_u32_data(message, 5, 27);
	a_s1.PI = get_u32_data(message, 32, 24);	
};

void decode_df_s2(unsigned char*message, unsigned char len)
{
	unsigned char datalength;
	datalength = sizeof(DF_S2);
	memset(&a_s2, 0, datalength);
	unsigned char i;

	a_s2.DF = get_BYTE_data(message, 0, 5);
	for (i = 0; i < 10; i++)
	{
		a_s2.standby[i] = get_BYTE_data(message, 5 + i * 8, 8);
	}
	a_s2.standby[10] = get_BYTE_data(message, 85, 3);
	a_s2.PI = get_u32_data(message, 88, 24);
};

int ModeACDemProc(unsigned char mode, unsigned char * data, int len, unsigned char *outdata, int outlen)
{
	unsigned char A = 0;
	unsigned char B = 0;
	unsigned char C = 0;
	unsigned char D = 0;
	unsigned char biaozhunxunhuan[9] = { 0 };
	unsigned char biaozhunxunhuan_d[9] = { 0 };
	unsigned char wuzhunxunhuan[3] = { 0 };
	int Height = 0;
	int Height_t = 0;
	if (mode == 21)//识别码应答 mode A
	{
		memcpy(outdata, data, len);
		outlen = len;
	}
	else if (mode == 22 && len > 1)//高度应答 mode C
	{
		A = ((data[0] & 0xf0) >> 4);
		B = (data[0] & 0x0f);
		C = ((data[1] & 0xf0) >> 4);
		D = (data[1] & 0x0f);
		biaozhunxunhuan[0] = (D & 0x01);
		biaozhunxunhuan[1] = ((D & 0x20) >> 1);
		biaozhunxunhuan[2] = ((D & 0x40) >> 3);
		biaozhunxunhuan[3] = (A & 0x01);
		biaozhunxunhuan[4] = ((A & 0x20) >> 1);
		biaozhunxunhuan[5] = ((A & 0x40) >> 3);
		biaozhunxunhuan[6] = (B & 0x01);
		biaozhunxunhuan[7] = ((B & 0x20) >> 1);
		biaozhunxunhuan[8] = ((B & 0x40) >> 3);
		for (int i = 1; i < 9; i++)
		{
			biaozhunxunhuan_d[i] = biaozhunxunhuan_d[i - 1] + biaozhunxunhuan[i];
			Height_t = Height_t * 2 + biaozhunxunhuan_d[i];
		}
		if (Height_t % 2)
		{
			switch (C)
			{
			case 0x04:
				Height = Height_t * 500 + 0;
				break;
			case 0x06:
				Height = Height_t * 500 + 100;
				break;
			case 0x02:
				Height = Height_t * 500 + 200;
				break;
			case 0x03:
				Height = Height_t * 500 + 300;
				break;
			case 0x01:
				Height = Height_t * 500 + 400;
				break;
			}

		}
		else
		{
			switch (C)
			{
			case 0x01:
				Height = Height_t * 500 + 0;
				break;
			case 0x03:
				Height = Height_t * 500 + 100;
				break;
			case 0x02:
				Height = Height_t * 500 + 200;
				break;
			case 0x06:
				Height = Height_t * 500 + 300;
				break;
			case 0x04:
				Height = Height_t * 500 + 400;
				break;
			}
		}
		outlen = sizeof(int);
		memcpy(outdata, &Height, outlen);

	}

	return 0;
}