#ifndef TCAS_H
#define TCAS_H
//#include "stdio.h"



#pragma pack(1)

#define EXPORT extern "C" __declspec(dllexport)
/***************                            S模式询问                             ***************/

typedef struct uf_0 //空-空监视，短空ACAS
{
	unsigned char UF;//[5]
	unsigned char standby1;//[3]
	unsigned char RL;
	unsigned char standby2;//[4]
	unsigned char AQ;
	unsigned int standby3;//[18]
	unsigned int AP;//[24]
}UF_0;

typedef struct uf_4 //监视，高度请求
{
	unsigned char UF;//[5]
	unsigned char PC;//[3]
	unsigned char RR;//[5]
	unsigned char DI;//[3]
	//[16]unsigned int SD;
	unsigned char IIS;//[4]
	unsigned short SD_0;//[12]

	unsigned char MBS;//[2]
	unsigned char MES;//[3]
	unsigned char RSS_2;
	unsigned char LOS;//[1]
	unsigned char TMS;//[4]

	unsigned char RSS_4;
	unsigned char SD_1;//[1]
	unsigned char SD_2;//[2]

	unsigned int AP;//[24]
}UF_4;

typedef struct uf_5 //监视，识别请求
{
	unsigned char UF;//[5]
	unsigned char PC;//[3]
	unsigned char RR;//[5]
	unsigned char DI;//[3]
	//[16]unsigned int SD;
	unsigned char IIS;//[4]
	unsigned short SD_0;//[12]

	unsigned char MBS;//[2]
	unsigned char MES;//[3]
	unsigned char RSS_2;
	unsigned char RSS_4;
	unsigned char SD_1;//[1]
	unsigned char SD_2;//[2]
	unsigned char LOS;//[1]
	unsigned char TMS;//[4]
	unsigned int AP;//[24]
}UF_5;

typedef struct uf_11 //模式S全呼
{
	unsigned char UF;//[5]
	unsigned char PR;//[4]
	unsigned char II;//[4]
	unsigned int standby;//[19]
	unsigned int AP;//[24]
}UF_11;

typedef struct uf_16 //空-空监视，空长TCAS
{
	unsigned char UF;//[5]
	unsigned char standby1;//[3]
	unsigned char RL;//[1]
	unsigned char standby2;//[4]
	unsigned char AQ;//[1]
	unsigned int standby3;//[18]

	unsigned int AP;//[24]
}UF_16;

typedef struct uf_20 //监视和地-空通信，comm_A 高度请求
{
	unsigned char UF;//[5]
	unsigned char PC;//[3]
	unsigned char RR;//[5]
	unsigned char DI;//[3]
	//[16]unsigned int SD;
	unsigned char IIS;//[4]
	unsigned short SD_0;//[12]
	unsigned char MBS;//[2]
	unsigned char MES;//[3]
	unsigned char RSS_2;
	unsigned char RSS_4;
	unsigned char SD_1;//[1]
	unsigned char SD_2;//[2]
	unsigned char LOS;//[1]
	unsigned char TMS;//[4]

	//[56]unsigned char MA[7];
	unsigned char DP;//[1]
	unsigned char MP;//[1]
	unsigned char M_CH;//[6]
	unsigned char SLC;//[4]
	unsigned short MA_S[3];//[44]

	unsigned int AP;//[24]
}UF_20;

typedef struct uf_21 //监视和地-空通信，comm_A 识别请求
{
	unsigned char UF;//[5]
	unsigned char PC;//[3]
	unsigned char RR;//[5]
	unsigned char DI;//[3]
	//[16]unsigned int SD;
	unsigned char IIS;//[4]
	unsigned short SD_0;//[12]
	unsigned char MBS;//[2]
	unsigned char MES;//[3]
	unsigned char RSS_2;
	unsigned char RSS_4;
	unsigned char SD_1;//[1]
	unsigned char SD_2;//[2]
	unsigned char LOS;//[1]
	unsigned char TMS;//[4]

	//[56]unsigned char MA[7];
	unsigned char DP;//[1]
	unsigned char MP;//[1]
	unsigned char M_CH;//[6]
	unsigned char SLC;//[4]
	unsigned short MA_S[3];//[44]

	unsigned int AP;//[24]
}UF_21;

typedef struct uf_24 //comm-C扩展长度信息(ELM)通信
{
	unsigned char UF;//[2]
	unsigned char RC;//[2]
	unsigned char NC;//[4]
	unsigned char MC[10];//[80]
	unsigned int AP;//[24]
}UF_24;

typedef struct uf_s1
{
	unsigned char UF;//[5]
	unsigned int standby;//[27]
	unsigned int AP; //[24]
}UF_S1;

typedef struct uf_s2
{
	unsigned char UF;//[5]
	unsigned char standby[11];//[83]
	unsigned int AP; //[24]
}UF_S2;

/***************                                  S模式应答                                  ***************/

typedef struct df_0 ////空-空监视，短空ACAS
{
	unsigned char DF;//[5]
	unsigned char VS;//[1]
	unsigned char standby1;//[7]
	unsigned char RI;//[4]
	unsigned char standby2;//[2]
	unsigned short AC;//[13]
	unsigned int AP;//[24]
}DF_0;

typedef struct df_4 //监视，高度请求
{
	unsigned char DF;//[5]
	unsigned char FS;//[3]
	unsigned char DR;//[5]
	unsigned char IIS;//[4]
	unsigned char IDS;//[2]
	unsigned short AC;//[13]
	unsigned int AP;//[24]	
}DF_4;

typedef struct df_5 //监视，识别请求
{
	unsigned char DF;//[5]
	unsigned char FS;//[3]
	unsigned char DR;//[5]
	unsigned char IIS;//[4]
	unsigned char IDS;//[2]
	unsigned short ID;//[13]
	unsigned int AP;//[24]	
}DF_5;

typedef struct df_11 //模式S全呼
{
	unsigned char DF;//[5]
	unsigned char CA;//[3]
	unsigned int AA;//[24]
	unsigned int AP;//[24]
}DF_11;

typedef struct df_16 //空-空监视，空长TCAS
{
	unsigned char DF;//[5]
	unsigned char VS;//[1]
	unsigned char standby1;//[7]
	unsigned char RI;//[4]
	unsigned char standby2;//[2]
	unsigned short AC;//[13]
	unsigned char MV[7];//[56]
	unsigned int AP;//[24]
}DF_16;

typedef struct df_20 //监视和地-空通信，comm_B 高度请求
{
	unsigned char DF;//[5]
	unsigned char FS;//[3]
	unsigned char DR;//[5]
	unsigned char IIS;//[4]
	unsigned char IDS;//[2]
	unsigned short AC;//[13]
	unsigned char MB[7];//[56]
	unsigned int AP;//[24]
}DF_20;

typedef struct df_21 //监视和地-空通信，comm_B 识别请求
{
	unsigned char DF;//[5]
	unsigned char FS;//[3]
	unsigned char DR;//[5]
	unsigned char IIS;//[4]
	unsigned char IDS;//[2]
	unsigned short ID;//[13]
	unsigned char MB[7];//[56]
	unsigned int AP;//[24]	
}DF_21;

typedef struct df_24 //comm-D扩展长度信息(ELM)通信
{
	unsigned char DF;//[2]
	unsigned char standby; //[1]
	unsigned char KE;//[1]
	unsigned char ND;//[4]
	unsigned char MD[10];//[80]
	unsigned int AP;//[24]
}DF_24;

typedef struct df_s1
{
	unsigned char DF;//[5]
	unsigned int standby;//[27|83]
	unsigned int PI; //[2]
}DF_S1;

typedef struct df_s2
{
	unsigned char DF;//[5]
	unsigned char standby[11];//[83]
	unsigned int PI; //[2]
}DF_S2;

typedef struct mb_rr19_d
{
	//RR=19  DO-185A系统[33-88]
	unsigned char BDS1;//[4]
	unsigned char BDS2;//[4]
	unsigned short ARA;//[14]有效RA
	unsigned char RAC_D;//[1]RAC补充RA
	unsigned char RAC_U;//[1]RAC补充RA
	unsigned char RAC_L;//[1]RAC补充RA
	unsigned char RAC_R;//[1]RAC补充RA
	unsigned char RAT;//[1]
	unsigned char MTE;//[1]
	unsigned char TTI;//[2]
	unsigned int TTD;//[26]
	
}MB_RR19_D;
typedef struct ttd1
{
	unsigned int TID1_d;//[24]TTI==1时，前24位
	unsigned char TID1_0;//[24]TTI==1时，后2位置零

}TTD1;
typedef struct ttd2
{
	unsigned short TID2_H;//[13]TTI==2时,高度
	unsigned char TID2_S;//[7]TTI==2时,距离
	unsigned char TID2_P;//[6]TTI==2时,方位
}TTD2;

typedef struct mb_rr19_f
{
	//RR=19  FFA TS0-C119A系统[33-88]
	unsigned char BDS1;//[4]
	unsigned char BDS2;//[4]
	unsigned short ARA;//[10]有效RA前10位
	unsigned char ARA_0;//[4]有效RA后4位
	unsigned char RAC_D;//[1]RAC补充RA
	unsigned char RAC_U;//[1]RAC补充RA
	unsigned char RAC_L;//[1]RAC补充RA
	unsigned char RAC_R;//[1]RAC补充RA
	unsigned int standby;//[30]未指定
}MB_RR19_F;

typedef struct mb_rr17_d
{
	//RR=17  DO-185A系统[33-88]
	unsigned char BDS1;//[4]
	unsigned char BDS2;//[4]
	unsigned char standby1;//[7]
	unsigned char TCAS_1;//[1]
	unsigned int standby2;//[20]
	unsigned char TCAS_2;//[1]
	unsigned char TCAS_3;//[1]
	unsigned char TCAS_4;//[1]
	unsigned int standby3;//[17]
}MB_RR17_D;

typedef struct mb_rr17_f
{
	//RR=17  FFA TS0-C119A系统[33-88]
	unsigned char BDS1;//[4]
	unsigned char BDS2;//[4]
	unsigned char standby1;//[7]
	unsigned char TCAS_1;//[1]
	unsigned int standby2;//[20]
	unsigned char analysis;//[2]
	unsigned char TCAS_2;//[1]
	unsigned int standby3;//[17]
}MB_RR17_F;

typedef struct mb_rr18
{
	//RR=18,DI!=7 or DI=7,RRS=0
	unsigned char BDS1;//[4]
	unsigned char BDS2;//[4]
	unsigned char IC[8];//[48]
}MB_RR18;

void decode_uf_0(unsigned char*message,unsigned char len);
void decode_uf_4(unsigned char*message, unsigned char len);
void decode_uf_5(unsigned char*message, unsigned char len);
void decode_uf_11(unsigned char*message, unsigned char len);
void decode_uf_16(unsigned char*message, unsigned char len);
void decode_uf_20(unsigned char*message, unsigned char len);
void decode_uf_21(unsigned char*message, unsigned char len);
void decode_uf_24(unsigned char*message, unsigned char len);
void decode_uf_s1(unsigned char*message, unsigned char len);
void decode_uf_s2(unsigned char*message, unsigned char len);
void decode_df_0(unsigned char*message, unsigned char len);
void decode_df_4(unsigned char*message, unsigned char len);
void decode_df_5(unsigned char*message, unsigned char len);
void decode_df_11(unsigned char*message, unsigned char len);
void decode_df_16(unsigned char*message, unsigned char len);
void decode_df_20(unsigned char*message, unsigned char len);
void decode_df_21(unsigned char*message, unsigned char len);
void decode_df_24(unsigned char*message, unsigned char len);
void decode_df_s1(unsigned char*message, unsigned char len);
void decode_df_s2(unsigned char*message, unsigned char len);
EXPORT int send_uf(unsigned char*message, int len, unsigned char *outdata);
EXPORT int receive_df(unsigned char*message, int len, unsigned char *outdata);
EXPORT int ModeACDemProc(unsigned char mode, unsigned char * data, int len, unsigned char *outdata, int outlen);

#endif