#ifndef __DATAFORMAT_H
#define __DATAFORMAT_H

#pragma pack(1)
/***********************************************************************
�ӿ����ݽṹ

***********************************************************************/
//
//typedef struct PDWALL
//{
//	GENERAL head;
//
//
//}PDWALL;



//ͨ��֡ͷ���ݽṹ
typedef struct GeneralHead
{
	int SyncHead;											//	ͬ��ͷ
	int EQID;														//�豸��
	unsigned char CardNum;						//�忨��
	unsigned char ProgreamVersion;			//����汾
	int SerialNum;											//����ˮ��
	unsigned char TimeScale[6];					//ʱ��
	int Second;												//���ڼ���
	int DataLen;												//������Ч����
	short Keep;												//����
	unsigned char RadioChannal1[8];				//��Ƶͨ��1����
	unsigned char RadioChannal2[8];				//��Ƶͨ��2����
	unsigned char RadioChannal3[8];				//��Ƶͨ��3����
	unsigned char RadioChannal4[8];				//��Ƶͨ��4����
	unsigned char RadioChannal5[8];				//��Ƶͨ��5����
	unsigned char RadioChannal6[8];				//��Ƶͨ��6����
}GENERAL;

// AIS/ACARS������ݽṹ
typedef struct AIS_ACARSFormat
{
	unsigned char Type;								//�������ͱ�ʶ
	unsigned char Channal;							//ͨ����
	unsigned char H_M_S[9];						//ʱ����
	unsigned char Local_Latitude[10];		//����γ��
	unsigned char Local_Lat_Mark;				//�ϱ������־
	unsigned char Local_longitude[11];		//���ؾ���
	unsigned char Local_lon_mark;				//���������־
	unsigned char Y_M_D;								//������
	int TimeStamp;											//ʱ��
	int Power;													//����ֵ
	short DataLen;											//���ݳ���
	unsigned char Data[512];
}A2AFORMATE;

// PDW���������Ϣ

// PDWS���ݰ� ����26���ֽ�
typedef struct PDWDataHead				//	 PDW���ݰ���ͷ
{
	unsigned short Type;							//	���ݰ�����
	unsigned short FreqChannal1_2;		//ͨ��1��2����Ƶ��
	unsigned short FreqChannal3_4;		//ͨ��3��4����Ƶ��
	unsigned short FreqChannal5;			//ͨ��5����Ƶ��
	unsigned short FreqChannal6;			//ͨ��6����Ƶ��
	unsigned short FreqChannal7;			//ͨ��7����Ƶ��
	unsigned short FreqChannal8;			//ͨ��8����Ƶ��
	unsigned short Threshold;					//ͨ������
	//PDWʱ����Ϣ
	unsigned short Year_Day;
	unsigned short Hour;
	unsigned short Min_Sec;
	int SerialNum;//[32]
}PDWDARAHEAD;


//PWD��������

typedef struct PDWData
{
	unsigned char Type;						//ͨ����ʶ
	unsigned char PulseWidthH;			//�����8bit
	unsigned short PulseWidthL;			//�����16bit
	unsigned short PluseAmplitude; //����
	unsigned short Freq;					   //Ƶ��
	unsigned short Time;						//ʱ����
	unsigned short ETAM;					//����ʱ����м�16bit
	unsigned short ETAL;						//����ʱ��ĵ�16bit
	unsigned short Keep;                     //����
}PDWDATA;

//PDW �������źŸ�ʽ
typedef struct PDWSecondPulse
{
	unsigned char type;							//����
	unsigned char standby1;                      //����
	unsigned short StartTime;				//��ʼʱ��
	unsigned short NowTime;				//��ǰʱ��
	unsigned char PulseNum[6];			//��ǰ�������Ӧ���������ֵ
	int standby2;						//����
}SECPULSE;

//ʶ����������֡

typedef struct ResultsBlock
{
	unsigned char type;						//ͨ����ʶ
	unsigned char SideLobe;			//�������
	unsigned short SignalAngle;		//�źŵ����
	unsigned short SignalPower;		//�ź�ǿ��
	unsigned short Freq;					//�ź�Ƶ��
	unsigned short Time;						//ʱ����
	unsigned short ETAM;					//����ʱ����м�16bit
	unsigned short ETAL;						//����ʱ��ĵ�16bit
	unsigned char ModeNO;				//ģʽ����
	unsigned char DataLen;				//���ݳ���
	unsigned char Data[256];
}RSBLOCK;


/***********************************************************************
ģʽS���ݽṹ

***********************************************************************/
//Sģʽѯ�� ����24��ѯ����Ϣ
// �ֽ׶���֪�Ľ��� 8����Ϣ��ʽ
// UF = 00000  ACAS��Ϣ




#endif