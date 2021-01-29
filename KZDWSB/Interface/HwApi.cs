using System;
using System.Runtime.InteropServices;

namespace KZDWSB
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int D_OpenDevice();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int D_CloseDevice();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int D_WriteData_ADS_Interrupt(byte[] data, int len);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int D_WriteData_AIS_Interrupt(byte[] data, int len);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int D_RegisterCallBackAIS(PlxApi.PDATA_CALLBACK pfunc, IntPtr pref);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int D_RegisterCallBackADS(PlxApi.PDATA_CALLBACK pfunc, IntPtr pref);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int D_StopCallbackFunc();

    public static class HwApi
    {
        public static D_OpenDevice OpenDevice;

        public static D_CloseDevice CloseDevice;

        public static D_WriteData_ADS_Interrupt WriteData_ADS_Interrupt;

        public static D_WriteData_AIS_Interrupt WriteData_AIS_Interrupt;

        public static D_RegisterCallBackAIS RegisterCallBackAIS;

        public static D_RegisterCallBackADS RegisterCallBackADS;

        public static D_StopCallbackFunc StopCallbackFunc;
    }
}
