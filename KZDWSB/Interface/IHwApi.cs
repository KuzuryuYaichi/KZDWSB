using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KZDWSB
{
    interface IHwApi
    {
        int OpenDevice();

        int CloseDevice();

        int WriteData_ADS_Interrupt(byte[] data, int len);

        int WriteData_AIS_Interrupt(byte[] data, int len);

        int RegisterCallBackAIS(PlxApi.PDATA_CALLBACK pfunc, IntPtr pref);

        int RegisterCallBackADS(PlxApi.PDATA_CALLBACK pfunc, IntPtr pref);

        int StopCallbackFunc();
    }
}
