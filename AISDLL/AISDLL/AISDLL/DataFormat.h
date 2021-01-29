#ifndef __DATAFORMAT_H
#define __DATAFORMAT_H

#pragma pack(1)
/***********************************************************************
接口数据结构

***********************************************************************/
//
//typedef struct PDWALL
//{
//	GENERAL head;
//
//
//}PDWALL;



//通用帧头数据结构
typedef struct GeneralHead
{
	int SyncHead;											//	同步头
	int EQID;														//设备号
	unsigned char CardNum;						//板卡号
	unsigned char ProgreamVersion;			//程序版本
	int SerialNum;											//包流水号
	unsigned char TimeScale[6];					//时标
	int Second;												//秒内计数
	int DataLen;												//负载有效长度
	short Keep;												//保留
	unsigned char RadioChannal1[8];				//射频通道1参数
	unsigned char RadioChannal2[8];				//射频通道2参数
	unsigned char RadioChannal3[8];				//射频通道3参数
	unsigned char RadioChannal4[8];				//射频通道4参数
	unsigned char RadioChannal5[8];				//射频通道5参数
	unsigned char RadioChannal6[8];				//射频通道6参数
}GENERAL;

// AIS/ACARS解调数据结构
typedef struct AIS_ACARSFormat
{
	unsigned char Type;								//数据类型标识
	unsigned char Channal;							//通道号
	unsigned char H_M_S[9];						//时分秒
	unsigned char Local_Latitude[10];		//本地纬度
	unsigned char Local_Lat_Mark;				//南北半球标志
	unsigned char Local_longitude[11];		//本地经度
	unsigned char Local_lon_mark;				//东西半球标志
	unsigned char Y_M_D;								//年月日
	int TimeStamp;											//时戳
	int Power;													//功率值
	short DataLen;											//数据长度
	unsigned char Data[512];
}A2AFORMATE;

// PDW相关数据信息

// PDWS数据包 共计26个字节
typedef struct PDWDataHead				//	 PDW数据包包头
{
	unsigned short Type;							//	数据包类型
	unsigned short FreqChannal1_2;		//通道1、2接收频点
	unsigned short FreqChannal3_4;		//通道3、4接收频点
	unsigned short FreqChannal5;			//通道5接收频点
	unsigned short FreqChannal6;			//通道6接收频点
	unsigned short FreqChannal7;			//通道7接收频点
	unsigned short FreqChannal8;			//通道8接收频点
	unsigned short Threshold;					//通道门限
	//PDW时间信息
	unsigned short Year_Day;
	unsigned short Hour;
	unsigned short Min_Sec;
	int SerialNum;//[32]
}PDWDARAHEAD;


//PWD数据区域

typedef struct PDWData
{
	unsigned char Type;						//通道标识
	unsigned char PulseWidthH;			//脉宽高8bit
	unsigned short PulseWidthL;			//脉宽低16bit
	unsigned short PluseAmplitude; //脉幅
	unsigned short Freq;					   //频率
	unsigned short Time;						//时分秒
	unsigned short ETAM;					//到达时间的中间16bit
	unsigned short ETAL;						//到达时间的低16bit
	unsigned short Keep;                     //保留
}PDWDATA;

//PDW 秒脉冲信号格式
typedef struct PDWSecondPulse
{
	unsigned char type;							//类型
	unsigned char standby1;                      //备用
	unsigned short StartTime;				//起始时间
	unsigned short NowTime;				//当前时间
	unsigned char PulseNum[6];			//当前秒脉冲对应计数器最大值
	int standby2;						//备用
}SECPULSE;

//识别结果数据子帧

typedef struct ResultsBlock
{
	unsigned char type;						//通道标识
	unsigned char SideLobe;			//主副瓣比
	unsigned short SignalAngle;		//信号到达角
	unsigned short SignalPower;		//信号强度
	unsigned short Freq;					//信号频点
	unsigned short Time;						//时分秒
	unsigned short ETAM;					//到达时间的中间16bit
	unsigned short ETAL;						//到达时间的低16bit
	unsigned char ModeNO;				//模式代码
	unsigned char DataLen;				//数据长度
	unsigned char Data[256];
}RSBLOCK;


/***********************************************************************
模式S数据结构

***********************************************************************/
//S模式询问 共计24种询问信息
// 现阶段已知的仅有 8种信息格式
// UF = 00000  ACAS信息




#endif