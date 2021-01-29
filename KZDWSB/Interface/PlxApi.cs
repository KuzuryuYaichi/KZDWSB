using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace KZDWSB
{
    public static class PlxApi
    {
        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate void PDATA_CALLBACK(IntPtr buffer, int len);

        #region 解译DLL

        [DllImport("AISDLL.dll", EntryPoint = "check_AIS_massage", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public extern static int check_AIS_massage(IntPtr packet, int pacekt_len, IntPtr outData);

        [DllImport("AISDLL.dll", EntryPoint = "phase_ais_packet", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public extern static int phase_ais_packet(IntPtr packet, int packet_len, IntPtr outData);

        [DllImport("AISDLL.dll", EntryPoint = "phase_acars_packet", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public extern static int phase_acars_packet(IntPtr packet, int packet_len, IntPtr outData);

        [DllImport("AISDLL.dll", EntryPoint = "BitReverseData", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public extern static int BitReverseData(IntPtr data, int length);
        //by cgd
        [DllImport("AISDLL.dll", EntryPoint = "phase_ads_packet", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public extern static int phase_ads_packet(IntPtr packet, int packet_len, IntPtr outData);

        [DllImport("AISDLL.dll", EntryPoint = "phase_mode_s_packet", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public extern static int phase_mode_s_packet(IntPtr packet, int packet_len, IntPtr outData);

        [DllImport("AISDLL.dll", EntryPoint = "GetPower", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public extern static float GetPower(byte flag, byte[] num);
        
        [DllImport("AISDLL.dll", EntryPoint = "send_uf", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]

        public extern static int send_uf(byte[] message, int len, byte[] outdata);

        [DllImport("AISDLL.dll", EntryPoint = "receive_df", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        
        public extern static int receive_df(byte[] message, int len, byte[] outdata);

        [DllImport("AISDLL.dll", EntryPoint = "phase_ads_b_packet", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        
        public extern static int phase_ads_b_packet(byte [] packet,int len);

        [DllImport("AISDLL.dll", EntryPoint = "ModeACDemProc", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]

        public extern static int ModeACDemProc(byte mode, IntPtr data, int len, IntPtr outdata, int outlen);

        #endregion

        //线程
        [DllImport("Kernel32.dll")]
        public extern static UIntPtr SetThreadAffinityMask(IntPtr hThread,UIntPtr dwThreadAffinityMask);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetCurrentThread();

        [DllImport("kernel32.dll")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        [DllImport("IpHlpApi.dll")]
        public static extern  uint GetIfTable(byte[] pIfTable, ref uint pdwSize, bool bOrder);

        #region 设置2A射频指令
        public static int SetAISParam(byte channal, double Freq, byte Gain)
        {
            int Ret = 0;
            byte[] Cmd = new byte[20];
            int FreqU32 = 0;
            byte[] tmp32 = new byte[4];
            //数据头
            Cmd[0] = 0xFA;
            Cmd[1] = 0xA5;
            Cmd[2] = 0xFB;
            Cmd[3] = 0xB5;
            Cmd[4] = 0xFC;
            Cmd[5] = 0xC5;
            //数据尾
            Cmd[18] = 0xF5;
            Cmd[19] = 0x5F;
            //数据指向
            Cmd[6] = 0xFA;
            //数据类型
            Cmd[7] = 0x40;
            //通道号
            Cmd[8] = channal;

            //频率
            FreqU32 = (int)(Freq * 1000);
            tmp32 = BitConverter.GetBytes(FreqU32);
            Cmd[9] = tmp32[3];
            Cmd[10] = tmp32[2];
            Cmd[11] = tmp32[1];
            Cmd[12] = tmp32[0];
            //增益
            Cmd[13] = Gain;
            //带宽
            Cmd[14] = 0x00;
            //模式
            Cmd[15] = 0x00;
            Ret = HwApi.WriteData_AIS_Interrupt(Cmd, 20);
            return Ret;
        }

        public static int SetACARSParam(byte channal, double Freq, byte Gain)
        {
            int Ret = 0;
            byte[] Cmd = new byte[20];
            int FreqU32 = 0;
            byte[] tmp32 = new byte[4];
            //数据头
            Cmd[0] = 0xFA;
            Cmd[1] = 0xA5;
            Cmd[2] = 0xFB;
            Cmd[3] = 0xB5;
            Cmd[4] = 0xFC;
            Cmd[5] = 0xC5;
            //数据尾
            Cmd[18] = 0xF5;
            Cmd[19] = 0x5F;
            //数据指向
            Cmd[6] = 0xFB;
            //数据类型
            Cmd[7] = 0x40;
            //通道号
            Cmd[8] = channal;

            //频率
            FreqU32 = (int)(Freq * 1000);
            tmp32 = BitConverter.GetBytes(FreqU32);
            Cmd[9] = tmp32[3];
            Cmd[10] = tmp32[2];
            Cmd[11] = tmp32[1];
            Cmd[12] = tmp32[0];
            //增益
            Cmd[13] = Gain;
            //带宽
            Cmd[14] = 0x00;
            //模式
            Cmd[15] = 0x00;
            Ret = HwApi.WriteData_AIS_Interrupt(Cmd, 20);
            return Ret;
        }
        #endregion
    }
}
