using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace KZDWSB
{
    public static class HW_LoadLibrary
    {
        [DllImport("Kernel32")]
        public static extern int GetProcAddress(int handle, String funcname);
        [DllImport("Kernel32")]
        public static extern int LoadLibrary(String funcname);
        [DllImport("Kernel32")]
        public static extern int FreeLibrary(int handle);

        private static Delegate GetAddress(int dllModule, string functionname, Type t)
        {
            int addr = GetProcAddress(dllModule, functionname);
            if (addr == 0)
                return null;
            else
                return Marshal.GetDelegateForFunctionPointer(new IntPtr(addr), t);
        }

        public static void LoadFunc(ref int huser32, string fileName)
        {
            string dir = Application.StartupPath;
            huser32 = LoadLibrary(dir + @"\" + fileName);
            HwApi.OpenDevice = (D_OpenDevice)GetAddress(huser32, "OpenDevice", typeof(D_OpenDevice));
            HwApi.CloseDevice = (D_CloseDevice)GetAddress(huser32, "CloseDevice", typeof(D_CloseDevice));
            HwApi.WriteData_ADS_Interrupt = (D_WriteData_ADS_Interrupt)GetAddress(huser32, "WriteData_ADS_Interrupt", typeof(D_WriteData_ADS_Interrupt));
            HwApi.WriteData_AIS_Interrupt = (D_WriteData_AIS_Interrupt)GetAddress(huser32, "WriteData_AIS_Interrupt", typeof(D_WriteData_AIS_Interrupt));
            HwApi.RegisterCallBackAIS = (D_RegisterCallBackAIS)GetAddress(huser32, "RegisterCallBackAIS", typeof(D_RegisterCallBackAIS));
            HwApi.RegisterCallBackADS = (D_RegisterCallBackADS)GetAddress(huser32, "RegisterCallBackADS", typeof(D_RegisterCallBackADS));
            HwApi.StopCallbackFunc = (D_StopCallbackFunc)GetAddress(huser32, "StopCallbackFunc", typeof(D_StopCallbackFunc));
        }
    }
}
