using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.Concurrent;
using System.Configuration;
using System.IO;
using ADS_B_INFO;
using AIS_INFO;
namespace KZDWSB
{
    public partial class MainForm : Form
    {
        #region 全局变量

        //PCIE全局变量
        IntPtr bufADS;
        IntPtr buf2A;
        public static bool DeviceFlag = false;     //打开设备标志，此状态关闭
        public ConcurrentQueue<byte[]> IFFDataQueue = new ConcurrentQueue<byte[]>();
        public ConcurrentQueue<byte[]> AISDataQueue = new ConcurrentQueue<byte[]>();
        public PlxApi.PDATA_CALLBACK CallBackADS;
        public PlxApi.PDATA_CALLBACK CallBackAIS;
        public static int OnePackNum = 2048;
        public static int CallBackNum = 2048;
        //数据处理线程
        Thread IFFDataQueueProc;
        Thread AISDataQueueProc;
        Thread FileThread_Decode;
        Thread FileThread_Encode;
        Thread GridListRefresh;
        //时统设备
        public static string local_device = "";
        public static int localInfoInBase = 0;
        public static double dLongitude = 0;   //本地位置—经度
        public static double dLatitude = 0;   //本地位置—纬度
        public static int MaxDistance = 0;
        public static DateTime TimeArrive;
        public static bool isAisGPSConnect = false;
        public static bool isIFFGPSConnect = false;
        public static bool isGpsEnable = true;
        public static DateTime RunDisTime;
        //显示列表
        private DataTable DataGridList = new DataTable();
        //数据存储
        public static ConcurrentQueue<string> Filestr_Decode = new ConcurrentQueue<string>();
        public static ConcurrentQueue<string> Filestr_Encode = new ConcurrentQueue<string>();
        //显示变量
        public static ConcurrentQueue<GridInfoClass> GridInfoQueue = new ConcurrentQueue<GridInfoClass>();
        public static long AIS1_count = 0;
        public static long AIS2_count = 0;
        public static long ADS_count = 0;
        public static long ACARS1_count = 0;
        public static long ACARS2_count = 0;
        public static long ACARS3_count = 0;
        public static long ACARS4_count = 0;

        public static long ADSTargetNum = 0;
        public static long AISTargetNum = 0;
        public static long ACARSTargetNum = 0;

        public static UInt64 PlxRecvNum = 0;
        public static UInt64 LastPlxNum = 0;

        public static long StayTimeSet = 0;
        //频点信息

        public static double AIS1FreqSet = 0;
        public static byte AIS1AttenSet = 0;

        public static double AIS2FreqSet = 0;
        public static byte AIS2AttenSet = 0;

        public static double ACARS1FreqSet = 0;
        public static byte ACARS1AttenSet = 0;

        public static double ACARS2FreqSet = 0;
        public static byte ACARS2AttenSet = 0;

        public static double ACARS3FreqSet = 0;
        public static byte ACARS3AttenSet = 0;

        public static double ACARS4FreqSet = 0;
        public static byte ACARS4AttenSet = 0;

        #endregion
        public MainForm()
        {
            InitializeComponent();
            label_device.Text = "设备未开启";
            GridViewInit();
            Init();
            TimeRefresh.Start();
            RunDisTime = DateTime.Now;
            dLongitude = double.Parse(ConfigurationManager.AppSettings["Local_lon"]);
            dLatitude = double.Parse(ConfigurationManager.AppSettings["Local_lat"]);
            MaxDistance = int.Parse(ConfigurationManager.AppSettings["CorrectArear"]);
        }
        public void GridViewInit()
        {
            DataGridList.Columns.Add("数据类型");
            DataGridList.Columns.Add("发现时间");
            DataGridList.Columns.Add("末次时间");
            DataGridList.Columns.Add("地址编码");
            DataGridList.Columns.Add("航班号/船名");
            DataGridList.Columns.Add("纬度");
            DataGridList.Columns.Add("经度");
            DataGridList.Columns.Add("高度(m)");
            DataGridList.Columns.Add("军民属性");
            DataGridList.Columns.Add("国籍");
            DataGridList.Columns.Add("捕获次数");
            DataGridList.Columns.Add("信号功率(dBm)");
            DataGridList.Columns.Add("距本地距离(Km)");
            this.gridControl1.DataSource = DataGridList;
            this.gridView1.OptionsSelection.MultiSelect = true;
            this.gridView1.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CheckBoxRowSelect;
            this.gridView1.OptionsSelection.ShowCheckBoxSelectorInColumnHeader = DevExpress.Utils.DefaultBoolean.True;
        }
        public void Init()
        {
            try
            {
                ChannelList.Items.Add("AIS1");
                ChannelList.Items.Add("AIS2");
                ChannelList.Items.Add("ACARS1");
                ChannelList.Items.Add("ACARS2");
                ChannelList.Items.Add("ACARS3");
                ChannelList.Items.Add("ACARS4");
                ChannelList.SelectedIndex = 0;
                FreqPoint.Text = AIS1FreqSet.ToString();
                Atten.Text = AIS1AttenSet.ToString(); ;
                OpenDevice();
                AIS1_count = 0;
                AIS2_count = 0;
                ADS_count = 0;
                ACARS1_count = 0;
                ACARS2_count = 0;
                ACARS3_count = 0;
                ACARS4_count = 0;
                StayTimeSet = long.Parse(StayTime.Text);
                LocalLat.Text = "39.2346";
                LocalLon.Text = "117.4372";
            }
            catch (System.Exception ex)
            {
                ErrorRecord.ProcessError(ex.ToString());
            }
        }
        public void CloseDevice()
        {
            try
            {
                int flag = HwApi.StopCallbackFunc();
                if (flag == 0)
                {
                    HwApi.CloseDevice();
                    Marshal.FreeHGlobal(bufADS);
                    IFFDataQueueProc.Abort();
                    AISDataQueueProc.Abort();
                    FileThread_Decode.Abort();
                    FileThread_Encode.Abort();
                    GridListRefresh.Abort();
                    DeviceFlag = false;
                }
            }
            catch (System.Exception ex)
            {
                ErrorRecord.ProcessError(ex.ToString());
            }
        }

        int huser32;
        public void OpenDevice()
        {
            try
            {
                if (DeviceFlag == false)
                {
                    string[] HW_List = new string[] { @"PlxApi711.dll", @"XDMA_PCIE.dll" };

                    int iRet = -5;
                    for (int i = 0; i < HW_List.Length; i++)
                    {
                        HW_LoadLibrary.LoadFunc(ref huser32, HW_List[i]);
                        iRet = HwApi.OpenDevice();
                        if (iRet > 0)
                        {
                            AIS1FreqSet = 161.975;
                            AIS2FreqSet = 162.025;
                            if (i == 0)
                            {
                                ACARS1FreqSet = 131.45;
                                ACARS2FreqSet = 127.275;
                                ACARS3FreqSet = 133.55;
                                ACARS4FreqSet = 126.475;
                                DeviceType.Text = "CPCI 3A系统运行中";
                            }
                            else if(i == 1)
                            {
                                ACARS1FreqSet = 126.475;
                                ACARS2FreqSet = 133.55;
                                ACARS3FreqSet = 127.275;
                                ACARS4FreqSet = 131.45;
                                DeviceType.Text = "PCIE 3A系统运行中";
                                ChannelList_SelectedIndexChanged(0);

                                ACARS1FreqSet = double.Parse(FreqPoint.Text);
                                ACARS1AttenSet = byte.Parse(Atten.Text);
                                ACARS1FreqSet = 131.45;
                                ACARS2FreqSet = 131.55;
                                ACARS3FreqSet = 127.275;
                                ACARS4FreqSet = 133.025;
                                int ret1 = PlxApi.SetACARSParam(0x03, ACARS1FreqSet, ACARS1AttenSet);
                                Thread.Sleep(1000);
                                int ret2 = PlxApi.SetACARSParam(0x04, ACARS2FreqSet, ACARS2AttenSet);
                                Thread.Sleep(1000);
                                int ret3 = PlxApi.SetACARSParam(0x05, ACARS3FreqSet, ACARS3AttenSet);
                                Thread.Sleep(1000);
                                int ret4 = PlxApi.SetACARSParam(0x06, ACARS4FreqSet, ACARS4AttenSet);
                                if ((ret1 | ret2 | ret3 | ret4) != 0)
                                {
                                    MessageBox.Show("ACARS参数初始化失败");
                                }
                            }
                            ChannelList_SelectedIndexChanged(0);
                            break;
                        }
                        HW_LoadLibrary.FreeLibrary(huser32);
                    }

                    if (iRet > 0)
                    {
                        OnePackNum = int.Parse(ConfigurationManager.AppSettings["OnePackNum"]);
                        CallBackNum = int.Parse(ConfigurationManager.AppSettings["CallBackNum"]);
                        //建立非托管内存
                        bufADS = Marshal.AllocHGlobal(OnePackNum * CallBackNum);
                        buf2A = Marshal.AllocHGlobal(OnePackNum * CallBackNum);
                        //设备状态
                        DeviceFlag = true;
                        DevButten.Text = "关闭设备";
                        //Plx数据接收线程
                        CallBackADS = OnRecvDataADS;
                        CallBackAIS = OnRecvDataAis;
                        HwApi.RegisterCallBackADS(CallBackADS, bufADS);
                        HwApi.RegisterCallBackAIS(CallBackAIS,buf2A);
                        //开启IFF数据解译线程
                        IFFDataQueueProc = new Thread(IFFDataQueueProcFun);
                        IFFDataQueueProc.IsBackground = true;
                        IFFDataQueueProc.Name = "IFFDataQueueProc";
                        IFFDataQueueProc.Start();

                        //开启AIS数据解译线程
                        AISDataQueueProc = new Thread(AISDataQueueProcFun);
                        AISDataQueueProc.IsBackground = true;
                        AISDataQueueProc.Name = "AISDataQueueProc";
                        AISDataQueueProc.Start();

                        //开启文件读写线程
                        FileThread_Decode = new Thread(FileFunction_Decode);
                        FileThread_Decode.IsBackground = true;
                        FileThread_Decode.Name = "FileThread_Decode";
                        FileThread_Decode.Start();

                        FileThread_Encode = new Thread(FileFunction_Encode);
                        FileThread_Encode.IsBackground = true;
                        FileThread_Encode.Name = "FileThread_Encode";
                        FileThread_Encode.Start();

                        GridListRefresh = new Thread(GridListRefreshFun);
                        GridListRefresh.IsBackground = true;
                        GridListRefresh.Name = "GridListRefresh";
                        GridListRefresh.Start();
                    }

                    switch (iRet)
                    {
                        case 1:
                            label_device.Text = "仅识别2A板卡";
                            break;
                        case 2:
                            label_device.Text = "仅识别ADS_B板卡";
                            break;
                        case 3:
                            label_device.Text = "2A/ADS_B板卡在线";
                            break;
                        case 0:
                            label_device.Text = "设备打开成功";
                            break;
                        case -1:
                            label_device.Text = "未找到设备";
                            break;
                        case -2:
                            label_device.Text = "打开设备失败";
                            break;
                        case -3:
                            label_device.Text = "设备已经打开";
                            break;
                        default:
                            label_device.Text = "设备查找或打开失败";
                            break;
                    }
                }
                else
                {
                    MessageBox.Show("重复打开设备");
                }
                
            }
            catch (System.Exception ex)
            {
                ErrorRecord.ProcessError(ex.ToString());
            }
        }
        public void OnRecvDataAis(IntPtr buffer, int packnum)
        {
            try
            {
                PlxApi.SetThreadAffinityMask(PlxApi.GetCurrentThread(), new UIntPtr(2));
                byte[] res = new byte[CallBackNum * OnePackNum];
                Marshal.Copy(buffer, res, 0, OnePackNum * CallBackNum);
                PlxRecvNum = PlxRecvNum + (UInt64)packnum;
                for (int i = 0; i < packnum; i++)
                {
                    byte[] saveByte = new byte[OnePackNum];
                    Array.Copy(res, i * CallBackNum, saveByte, 0, OnePackNum);
                    AISDataQueue.Enqueue(saveByte);
                }
            }
            catch (System.Exception ex)
            {
                if (!ex.Message.Contains("正在中止线程"))
                {
                    ErrorRecord.ProcessError(ex.ToString());
                }
            }
        }
        public void OnRecvDataADS(IntPtr buffer, int packnum)
        {
            try
            {
                PlxApi.SetThreadAffinityMask(PlxApi.GetCurrentThread(), new UIntPtr(2));
                byte[] res = new byte[CallBackNum * OnePackNum];
                PlxRecvNum = PlxRecvNum + (UInt64)packnum;
                Marshal.Copy(buffer, res, 0, OnePackNum * CallBackNum);
                for (int i = 0; i < packnum; i++)
                {
                    byte[] saveByte = new byte[OnePackNum];
                    Array.Copy(res, i * CallBackNum, saveByte, 0, OnePackNum);
                    IFFDataQueue.Enqueue(saveByte);
                }
            }
            catch (System.Exception ex)
            {
                if (!ex.Message.Contains("正在中止线程"))
                {
                    ErrorRecord.ProcessError(ex.ToString());
                }
            }
        }

        /*********************************写文件线程****************************/
        private void FileFunction_Decode()
        {
            string strType = "";
            string strTXT = "";
            while (true)
            {
                try
                {
                    if (Filestr_Decode.IsEmpty == false)
                    {
                        Filestr_Decode.TryDequeue(out strTXT);

                        int num = strTXT.IndexOf(" ");
                        strType = strTXT.Remove(num);
                        strTXT = strTXT.Remove(0, num + 1);

                        //存文件
                        string path;
                        DateTime now = DateTime.Now;

                        path = @"Data\Decode\" + strType + "\\" + now.Date.ToString("yyyy-MM-dd");

                        DirectoryInfo fi = new DirectoryInfo(path);
                        if (!fi.Exists)
                            fi.Create();
                        byte[] a = Encoding.UTF8.GetBytes(strTXT.ToString());
                        FileStream fs = new FileStream(path + "\\" + now.ToString("HH") + ".txt", FileMode.OpenOrCreate | FileMode.Append);
                        fs.Write(a, 0, (int)a.Length);
                        fs.Close();
                    }
                    else
                    {
                        Thread.Sleep(2);
                    }

                }
                catch
                {

                }

            }

        }
        private void FileFunction_Encode()
        {
            string strType = "";
            string strTXT = "";
            while (true)
            {
                try
                {
                    if (Filestr_Encode.IsEmpty == false)
                    {
                        Filestr_Encode.TryDequeue(out strTXT);

                        int num = strTXT.IndexOf(" ");
                        strType = strTXT.Remove(num);
                        strTXT = strTXT.Remove(0, num + 1);

                        //存文件
                        string path;
                        DateTime now = DateTime.Now;

                        path = @"Data\Encode\" + strType + "\\" + now.Date.ToString("yyyy-MM-dd");

                        DirectoryInfo fi = new DirectoryInfo(path);
                        if (!fi.Exists)
                            fi.Create();
                        byte[] a = Encoding.UTF8.GetBytes(strTXT.ToString());
                        FileStream fs = new FileStream(path + "\\" + now.ToString("HH") + ".txt", FileMode.OpenOrCreate | FileMode.Append);
                        fs.Write(a, 0, (int)a.Length);
                        fs.Close();
                    }
                    else
                    {
                        Thread.Sleep(2);
                    }

                }
                catch
                {

                }
            }
        }
        public int GirdDataFind(DataTable dt, GridInfoClass GridInfo)
        {
            int iRet = -1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((GridInfo.DataType == dt.Rows[i]["数据类型"].ToString()) && (GridInfo.ID == dt.Rows[i]["地址编码"].ToString()))
                {
                    dt.Rows[i]["末次时间"] = GridInfo.LastTime;
                    if (GridInfo.Name != null && GridInfo.Name != "")
                    {
                        dt.Rows[i]["航班号/船名"] = GridInfo.Name;
                    }
                    if (GridInfo.Lat != null && GridInfo.Lon != null)
                    {
                        dt.Rows[i]["距本地距离(Km)"] = GridInfo.Distance;
                        dt.Rows[i]["纬度"] = GridInfo.Lat;
                        dt.Rows[i]["经度"] = GridInfo.Lon;
                    }
                    if (GridInfo.Height != "0" && GridInfo.Height != null)
                    {
                        dt.Rows[i]["高度(m)"] = GridInfo.Height;
                    }
                    dt.Rows[i]["军民属性"] = GridInfo.Military;
                    dt.Rows[i]["国籍"] = GridInfo.National;
                    dt.Rows[i]["捕获次数"] = (int.Parse(dt.Rows[i]["捕获次数"].ToString()) + 1).ToString();
                    dt.Rows[i]["信号功率(dBm)"] = GridInfo.PA;
                    return i;
                }
            }
            return iRet;
        }
        public void GridListRefreshFun()
        { 
            while(true)
            {
                try
                {
                    if (GridInfoQueue.IsEmpty == false)
                    {
                        GridInfoClass GridInfo = new GridInfoClass();
                        GridInfoQueue.TryDequeue(out GridInfo);
                        gridControl1.BeginInvoke(new Action(() =>
                        {
                            ClearGridViewData(GridInfo.LastTime);
                            int iRet = GirdDataFind(DataGridList, GridInfo);
                            if (iRet == -1)
                            {
                                if (GridInfo.ID != null && GridInfo.ID != "0" && GridInfo.ID != "")
                                {
                                    if(GridInfo.DataType == "ADS-B")
                                    {
                                        ADSTargetNum++;
                                    }
                                    else if (GridInfo.DataType == "AIS")
                                    {
                                        AISTargetNum++;
                                    }
                                    else if (GridInfo.DataType == "ACARS")
                                    {
                                        ACARSTargetNum++;
                                    }
                                    DataRow dr = DataGridList.NewRow();
                                    dr["数据类型"] = GridInfo.DataType;
                                    dr["发现时间"] = GridInfo.FirstTime;
                                    dr["末次时间"] = GridInfo.LastTime;
                                    dr["地址编码"] = GridInfo.ID;
                                    dr["航班号/船名"] = GridInfo.Name;
                                    dr["纬度"] = GridInfo.Lat;
                                    dr["经度"] = GridInfo.Lon;
                                    dr["高度(m)"] = GridInfo.Height;
                                    dr["军民属性"] = GridInfo.Military;
                                    dr["国籍"] = GridInfo.National;
                                    dr["捕获次数"] = "1";
                                    dr["信号功率(dBm)"] = GridInfo.PA;
                                    dr["距本地距离(Km)"] = GridInfo.Distance;
                                    DataGridList.Rows.Add(dr);
                                }
                            }
                            gridControl1.DataSource = DataGridList;
                        }));
                    }
                    else
                    {
                        Thread.Sleep(10);
                    }
                }
                catch (System.Exception ex)
                {
                    ErrorRecord.ProcessError(ex.ToString());
                }

            }
        }
        public byte[] ByteReversal(byte[] data, int Index, int len)
        {
            byte[] BackData = new byte[len];
            for (int i = 0; i < len; i++)
            {
                BackData[i] = data[Index + len - 1 - i];
            }
            return BackData;
        }

        public int TimeCheckOut(int year, int month, int day, int hour, int minter, int second)
        {
            try
            {
                DateTime tmie = new DateTime(year, month, day, hour, minter, second);
                return 0;
            }
            catch
            {
                return -1;
            }
        }
        //判断是未超过经纬度界限
        private static bool CheckRegion(double lat, double lon, double lat_c, double lon_c)
        {
            double ArearLimt = (double)MaxDistance / 111;
            try
            {
                if (ArearLimt < 1)
                {
                    ArearLimt = 6;
                }
                if (lat_c == 0 && lon_c == 0)
                {
                    if (lat > -90 && lat < 90 && lon < 180 && lon > -180)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    if (Math.Abs(lat - lat_c) < ArearLimt && Math.Abs(lon - lon_c) < ArearLimt)
                        return true;
                    else
                        return false;
                }
            }
            catch (System.Exception ex)
            {
                // ErrorRecord.ProcessError(ex.ToString());
            }
            return false;
        }
        //引用协议版本为CPCI2A_20200918程序设计说明
        /***********************************
         * 112字节数据帧头*1936字节数据信息*
         * ********************************/
        public void IFFDataQueueProcFun()
        {
            PlxApi.SetThreadAffinityMask(PlxApi.GetCurrentThread(), new UIntPtr(32));
            DataClass RecData = new DataClass();

            IntPtr Revbuf = Marshal.AllocHGlobal(300);
            IntPtr OutData = Marshal.AllocHGlobal(300);
            byte[] outByte = new byte[300];
            while (true && (DeviceFlag == true))
            {
                try
                {
                    if (IFFDataQueue.IsEmpty == false)
                    {
                        byte[] saveByte = new byte[OnePackNum];
                        IFFDataQueue.TryDequeue(out saveByte);
                        if (saveByte[0] != 0x01 || saveByte[1] != 0x23 || saveByte[2] != 0x45 || saveByte[3] != 0x67)
                            continue;
                        #region 计算本地经纬度
                        if (saveByte[80] == 0x0E || saveByte[80] == 0x03)
                        {
                            int HourType = saveByte[17] & 0x01;
                            int second = (saveByte[19] & 0x3f);  //TOA[37:32]
                            int minter = ((saveByte[18] & 0x0f) * 4) + ((saveByte[19] & 0xc0) / 64);
                            int hour = 0;
                            int day = ((saveByte[17] & 0x3e) / 2);
                            int month = ((saveByte[17] & 0xc0) / 64) + ((saveByte[16] & 0x03) * 4);
                            int year1 = ((saveByte[16] & 0xfc) / 4);
                            int year = 2000 + year1;
                            if (HourType != 0)
                            {
                                hour = ((saveByte[18] & 0xf0) / 16) + 12 + 8;//伦敦时间

                            }
                            else
                            {
                                hour = ((saveByte[18] & 0xf0) / 16) + 8;
                            }
                            if (hour > 23)
                            {
                                hour = hour - 24;
                                day = day + 1;
                            }
                            double minsecond = ((double)saveByte[20] * 16777216 + (double)saveByte[21] * 66536 + (double)saveByte[22] * 256 + (double)saveByte[23]) / (double)30720000;
                            if (TimeCheckOut(year, month, day, hour, minter, second) == 0)
                            {
                                DateTime tmie = new DateTime(year, month, day, hour, minter, second);
                                TimeArrive = tmie;
                                RecData.dtDateTime = tmie;
                                RecData.StrDateTime = tmie.ToString("yyyy/MM/dd HH:mm:ss");
                                RecData.SaveDate = tmie.ToString("yyyy/MM/dd HH:mm:ss") + minsecond.ToString().Substring(1, minsecond.ToString().Length - 1);
                                RecData.ads_time = (uint)RecData.dtDateTime.Hour*3600 + (uint)RecData.dtDateTime.Minute * 60 + (uint)RecData.dtDateTime.Second;
                            }
                            else
                            {
                                DateTime tmie = DateTime.Now;
                                RecData.dtDateTime = tmie;
                                RecData.StrDateTime = tmie.ToString("yyyy/MM/dd HH:mm:ss");
                                RecData.SaveDate = tmie.ToString("yyyy/MM/dd HH:mm:ss") + minsecond.ToString().Substring(1, minsecond.ToString().Length - 1);
                                RecData.ads_time = (uint)RecData.dtDateTime.Hour * 3600 + (uint)RecData.dtDateTime.Minute * 60 + (uint)RecData.dtDateTime.Second;
                            }
                            string flaglat = saveByte[80] == 0x0E ? "N" : "S";
                            string flaglon = Convert.ToInt32(Convert.ToString(saveByte[88], 2).PadLeft(8, '0').Substring(0, 4)) == 7 ? "W" : "E";
                            double LatInteger = (double)((saveByte[81] & 0xf0) >> 4) * 1000 + (double)(saveByte[81] & 0x0f) * 100 + (double)((saveByte[82] & 0xf0) >> 4) * 10 + (double)((saveByte[82] & 0x0f));
                            double LatPoint = (double)((saveByte[83] & 0x0f)) / 10 + (double)((saveByte[84] & 0xf0) >> 4) / 100 + (double)((saveByte[84] & 0x0f)) / 1000 + (double)((saveByte[85] & 0xf0) >> 4) / 10000 + (double)((saveByte[85] & 0x0f)) / 100000 + (double)((saveByte[86] & 0xf0 >> 4)) / 1000000 + (double)((saveByte[86] & 0x0f)) / 10000000;
                            double GPSLat = saveByte[80] == 0x0E ? (LatInteger + LatPoint) : (0.0 - (LatInteger + LatPoint));
                            double LonInteger = (double)(saveByte[88] & 0x0f) * 10000 + (double)((saveByte[89] & 0xf0) >> 4) * 1000 + (double)(saveByte[89] & 0x0f) * 100 + (double)((saveByte[90] & 0xf0) >> 4) * 10 + (double)((saveByte[90] & 0x0f));
                            double LonPoint = (double)((saveByte[91] & 0x0f)) / 10 + (double)((saveByte[92] & 0xf0) >> 4) / 100 + (double)((saveByte[92] & 0x0f)) / 1000 + (double)((saveByte[93] & 0xf0) >> 4) / 10000 + (double)((saveByte[93] & 0x0f)) / 100000 + (double)((saveByte[94] & 0xf0 >> 4)) / 1000000 + (double)((saveByte[94] & 0x0f)) / 10000000;
                            double GPSLon = ((saveByte[88] & 0xf0) >> 4) == 7 ? (0 - (LonInteger + LonPoint)) : (LonInteger + LonPoint);

                            GPSLat = (double)((double)(GPSLat - (int)(GPSLat / 100) * 100) / 60.0 + (int)(GPSLat / 100));
                            GPSLon = (double)((double)(GPSLon - (int)(GPSLon / 100) * 100) / 60.0 + (int)(GPSLon / 100));
                            dLongitude = GPSLon;
                            dLatitude = GPSLat;

                            if (localInfoInBase % 80000 == 0)
                            {
                                Device_SaveToSql(local_device, GPSLon, GPSLat, RecData.StrDateTime);

                            }
                            localInfoInBase++;
                        }
                        else
                        {
                            DateTime tmie = DateTime.Now;
                            RecData.dtDateTime = tmie;
                            RecData.SaveDate = tmie.ToString("yyyy/MM/dd HH:mm:ss");
                            RecData.StrDateTime = tmie.ToString("yyyy/MM/dd HH:mm:ss");
                            RecData.ads_time = (uint)RecData.dtDateTime.Hour * 3600 + (uint)RecData.dtDateTime.Minute * 60 + (uint)RecData.dtDateTime.Second;
                        }
                        #endregion
                        if (saveByte.Length > (128))
                        {
                            
                            RecData.DataType = saveByte[125];   //数据类型
                            RecData.ChalType = saveByte[127];   //数据格式
                            RecData.ChalNo = saveByte[129];     //数据通道号
                            RecData.DataLength = (saveByte[174] << 8) + saveByte[175];
                            RecData.Messageinfo = saveByte.Skip(176).Take(RecData.DataLength).ToArray();
                            byte[] PowerByte = new byte[4];
                            PowerByte = saveByte.Skip(168).Take(4).ToArray();
                            RecData.dLat = dLatitude;
                            RecData.dLon = dLongitude;
                            RecData.power = GetADSignalPower(PowerByte);
                            #region
                            //识别结果数据包
                            if (RecData.DataType == 0x01 && RecData.ChalType == 0x03)
                            {
                                #region ADS_B解译

                                //RecData.DataLength = 14;
                                Marshal.Copy(RecData.Messageinfo, 0, Revbuf, RecData.DataLength);
                                int strCRC = PlxApi.phase_ads_packet(Revbuf, RecData.DataLength, OutData);
                                Marshal.Copy(OutData, outByte, 0, 300);
                                if (strCRC == 0)
                                {
                                    byte Military = (byte)(RecData.Messageinfo[0]/8);
                                    ADS_B_INFO.ADS_MessageInfo ADS_info = new ADS_B_INFO.ADS_MessageInfo();
                                    ADS_SaveToSql(ADS_info, outByte, RecData.dtDateTime, RecData, strCRC, Military);
                                    
                                }

                                #endregion                  
                            }

                            #endregion

                        }

                    }
                    else
                    {
                        Thread.Sleep(5);
                    }
                }
                catch (System.Exception ex)
                {
                    ErrorRecord.ProcessError(ex.ToString());
                }
            }
        }
        //  ACARS PA = 10 * Lg(x - 14) -113.7
        public double GetACARSSignalPower(byte[] power)
        {
            double PA = 0;
            int BytePow = BitConverter.ToInt32(ByteReversal(power, 0, 4), 0);
            if (BytePow > 14)
            {
                PA = 10 * Math.Log10(BytePow - 14) - 113.7;
            }
            else
            {
                PA = -103 - (14 - BytePow);
            }
            return PA;
        }
        //  AIS PA = 10 * Lg(x + 103.2) - 131.25
        public double GetAISSignalPower(byte[] power)
        {
            double PA = 0;
            int BytePow = BitConverter.ToInt32(ByteReversal(power, 0, 4), 0);
            PA = 10 * Math.Log10(BytePow + 103.2) - 131.25;
            return PA;
        }
        public double GetADSignalPower(byte[] power)
        {
            double PA = 0;
            int BytePow = BitConverter.ToInt32(ByteReversal(power, 0, 4), 0);
            PA = 10 * Math.Log10(BytePow - 8983) - 131.75;
            return PA;
        }
        public void AISDataQueueProcFun()
        {
            PlxApi.SetThreadAffinityMask(PlxApi.GetCurrentThread(), new UIntPtr(32));
            DataClass RecData = new DataClass();

            IntPtr Revbuf = Marshal.AllocHGlobal(300);
            IntPtr OutData = Marshal.AllocHGlobal(300);
            byte[] outByte = new byte[300];
            while (true && (DeviceFlag == true))
            {
                try
                {
                    if (AISDataQueue.IsEmpty == false)
                    {
                        byte[] saveByte = new byte[OnePackNum];
                        AISDataQueue.TryDequeue(out saveByte);
                        if (saveByte[0] != 0x01 || saveByte[1] != 0x23 || saveByte[2] != 0x45 || saveByte[3] != 0x67)
                            continue;
                        #region 计算本地经纬度
                        if (saveByte[80] == 0x0E || saveByte[80] == 0x03)
                        {
                            int HourType = saveByte[17] & 0x01;
                            int second = (saveByte[19] & 0x3f);  //TOA[37:32]
                            int minter = ((saveByte[18] & 0x0f) * 4) + ((saveByte[19] & 0xc0) / 64);
                            int hour = 0;
                            int day = ((saveByte[17] & 0x3e) / 2);
                            int month = ((saveByte[17] & 0xc0) / 64) + ((saveByte[16] & 0x03) * 4);
                            int year1 = ((saveByte[16] & 0xfc) / 4);
                            int year = 2000 + year1;
                            if (HourType != 0)
                            {
                                hour = ((saveByte[18] & 0xf0) / 16) + 12 + 8;//伦敦时间

                            }
                            else
                            {
                                hour = ((saveByte[18] & 0xf0) / 16) + 8;
                            }
                            if (hour > 23)
                            {
                                hour = hour - 24;
                                day = day + 1;
                            }
                            double minsecond = ((double)saveByte[20] * 16777216 + (double)saveByte[21] * 66536 + (double)saveByte[22] * 256 + (double)saveByte[23]) / (double)30720000;
                            if (TimeCheckOut(year, month, day, hour, minter, second) == 0)
                            {
                                DateTime tmie = new DateTime(year, month, day, hour, minter, second);
                                RecData.dtDateTime = tmie;
                                TimeArrive = tmie;
                                RecData.StrDateTime = tmie.ToString("yyyy/MM/dd HH:mm:ss");
                                RecData.SaveDate = tmie.ToString("yyyy/MM/dd HH:mm:ss") + minsecond.ToString().Substring(1, minsecond.ToString().Length - 1);
                            }
                            else
                            {
                                DateTime tmie = DateTime.Now;
                                RecData.dtDateTime = tmie;
                                RecData.StrDateTime = tmie.ToString("yyyy/MM/dd HH:mm:ss");
                                RecData.SaveDate = tmie.ToString("yyyy/MM/dd HH:mm:ss") + minsecond.ToString().Substring(1, minsecond.ToString().Length - 1);
                            }
                            string flaglat = saveByte[80] == 0x0E ? "N" : "S";
                            string flaglon = Convert.ToInt32(Convert.ToString(saveByte[88], 2).PadLeft(8, '0').Substring(0, 4)) == 7 ? "W" : "E";
                            double LatInteger = (double)((saveByte[81] & 0xf0) >> 4) * 1000 + (double)(saveByte[81] & 0x0f) * 100 + (double)((saveByte[82] & 0xf0) >> 4) * 10 + (double)((saveByte[82] & 0x0f));
                            double LatPoint = (double)((saveByte[83] & 0x0f)) / 10 + (double)((saveByte[84] & 0xf0) >> 4) / 100 + (double)((saveByte[84] & 0x0f)) / 1000 + (double)((saveByte[85] & 0xf0) >> 4) / 10000 + (double)((saveByte[85] & 0x0f)) / 100000 + (double)((saveByte[86] & 0xf0 >> 4)) / 1000000 + (double)((saveByte[86] & 0x0f)) / 10000000;
                            double GPSLat = saveByte[80] == 0x0E ? (LatInteger + LatPoint) : (0.0 - (LatInteger + LatPoint));
                            double LonInteger = (double)(saveByte[88] & 0x0f) * 10000 + (double)((saveByte[89] & 0xf0) >> 4) * 1000 + (double)(saveByte[89] & 0x0f) * 100 + (double)((saveByte[90] & 0xf0) >> 4) * 10 + (double)((saveByte[90] & 0x0f));
                            double LonPoint = (double)((saveByte[91] & 0x0f)) / 10 + (double)((saveByte[92] & 0xf0) >> 4) / 100 + (double)((saveByte[92] & 0x0f)) / 1000 + (double)((saveByte[93] & 0xf0) >> 4) / 10000 + (double)((saveByte[93] & 0x0f)) / 100000 + (double)((saveByte[94] & 0xf0 >> 4)) / 1000000 + (double)((saveByte[94] & 0x0f)) / 10000000;
                            double GPSLon = ((saveByte[88] & 0xf0) >> 4) == 7 ? (0 - (LonInteger + LonPoint)) : (LonInteger + LonPoint);

                            GPSLat = (double)((double)(GPSLat - (int)(GPSLat / 100) * 100) / 60.0 + (int)(GPSLat / 100));
                            GPSLon = (double)((double)(GPSLon - (int)(GPSLon / 100) * 100) / 60.0 + (int)(GPSLon / 100));
                            dLongitude = GPSLon;
                            dLatitude = GPSLat;

                            if (localInfoInBase % 80000 == 0)
                            {
                                Device_SaveToSql(local_device, GPSLon, GPSLat, RecData.StrDateTime);

                            }
                            localInfoInBase++;
                        }
                        else
                        {
                            RecData.dtDateTime = DateTime.Now;
                            RecData.StrDateTime = RecData.dtDateTime.ToString("yyyy/MM/dd HH:mm:ss");
                            RecData.SaveDate = RecData.dtDateTime.ToString("yyyy/MM/dd HH:mm:ss");
                        }

                        #endregion
                        if (saveByte.Length > (128))
                        {
                            RecData.DataType = saveByte[125];   //数据类型
                            RecData.ChalType = saveByte[127];   //数据格式
                            RecData.ChalNo = saveByte[129];     //数据通道号
                            RecData.DataLength = (saveByte[174] << 8) + saveByte[175];
                            RecData.Messageinfo = saveByte.Skip(176).Take(RecData.DataLength).ToArray();
                            RecData.dLat = dLatitude;
                            RecData.dLon = dLongitude;
                            byte[] PowerByte = new byte[4];
                            PowerByte = saveByte.Skip(168).Take(4).ToArray();
                            
                            #region
                            //处理AIS/ACARS数据信息
                            if (RecData.DataType == 0x01)//识别结果/解调结果
                            {
                                if (RecData.ChalType == 0x01)//AIS数据信息
                                {
                                    #region  AIS解译
                                    RecData.power = GetAISSignalPower(PowerByte);
                                    Marshal.Copy(RecData.Messageinfo, 0, Revbuf, RecData.DataLength);
                                    int strCRC = PlxApi.phase_ais_packet(Revbuf, RecData.DataLength, OutData);
                                    byte[] bt = new byte[300];
                                    Marshal.Copy(OutData, bt, 0, 300);
                                    if (strCRC == 0)
                                    {
                                        int DataLength = 0;
                                        switch (bt[0])//消息识别码
                                        {
                                            case 0x06:
                                                DataLength = RecData.DataLength - 9;
                                                break;
                                            case 0x0c:
                                                if ((RecData.DataLength - 9) * 8 % 6 == 0)
                                                    DataLength = (RecData.DataLength - 9) * 8 / 6 - 1;
                                                else
                                                    DataLength = (RecData.DataLength - 9) * 8 / 6;
                                                break;
                                            case 0x08:
                                                DataLength = RecData.DataLength - 5;
                                                break;
                                            case 0x0e:
                                                if ((RecData.DataLength - 5) * 8 % 6 == 0)
                                                    DataLength = (RecData.DataLength - 5) * 8 / 6 - 1;
                                                else
                                                    DataLength = (RecData.DataLength - 5) * 8 / 6;
                                                break;
                                            case 0x11:
                                                DataLength = RecData.DataLength - 10;
                                                break;
                                            default:
                                                break;
                                        }
                                        AIS_INFO.AIS_MessageInfo AISinfo = new AIS_INFO.AIS_MessageInfo();
                                        AIS_SaveToSql(AISinfo, bt, DataLength, RecData, strCRC);
                                        
                                    }
                                    if (strCRC == 0 || strCRC == 1)
                                        SaveOriginalData("AIS", RecData.ChalNo, RecData.Messageinfo, RecData.dtDateTime);
                                    #endregion
                                }
                                else if (RecData.ChalType == 0x02)//ACARS数据信息
                                {
                                    #region ACARS解译
                                    RecData.power = GetACARSSignalPower(PowerByte);
                                    Marshal.Copy(RecData.Messageinfo, 0, Revbuf, RecData.DataLength);

                                    int strCRC = PlxApi.phase_acars_packet(Revbuf, RecData.DataLength, OutData);
                                    if (strCRC == 0 || strCRC == 1)
                                    {
                                        byte[] bt = new byte[RecData.DataLength];
                                        Marshal.Copy(OutData, bt, 0, RecData.DataLength);
                                        //进行CRC筛选，全部入库（因为需要进行奇校验）
                                        AcarsInfo.ACARS_MessageInfo ACARSinfo = new AcarsInfo.ACARS_MessageInfo();
                                        ACARS_SaveToSql(ACARSinfo, bt, RecData,strCRC, 0);
                                        SaveOriginalData("ACARS", RecData.ChalNo, RecData.Messageinfo, RecData.dtDateTime);
                                        
                                    }
                                    #endregion
                                }
                            }

                            #endregion

                        }

                    }
                    else
                    {
                        Thread.Sleep(5);
                    }
                }
                catch (System.Exception ex)
                {
                    ErrorRecord.ProcessError(ex.ToString());
                }
            }
        }

        

    
        /*********************************数据处理线程****************************/
        //函数——接收AIS处理并存入数据库(修改后)
        private void AIS_SaveToSql(AIS_INFO.AIS_MessageInfo info, byte[] test,int length, DataClass RecData, int CRC)
        {
            try
            {
                AIS_INFO.AIS_MessageInfo MessageOut = new AIS_MessageInfo();
                string strTXT = "";
                switch (test[0])
                {
                    case 1:
                    case 2:
                    case 3:
                        MessageOut = info.INFO_1_2_3(test);
                        strTXT = TXTClass.AIS_Insert(MessageOut, RecData);
                        break;
                    case 4:
                        MessageOut = info.INFO_4_11(test);
                        strTXT = TXTClass.AIS_Insert(MessageOut, RecData);
                        break;
                    case 5:
                        MessageOut = info.INFO_5(test);
                        strTXT = TXTClass.AIS_Insert(MessageOut, RecData);
                        break;
                    case 6:
                        MessageOut = info.INFO_6(test,test.Length);
                        strTXT = TXTClass.AIS_Insert(MessageOut, RecData);
                        break;
                    case 11:
                        MessageOut = info.INFO_4_11(test);
                        strTXT = TXTClass.AIS_Insert(MessageOut, RecData);
                        break;
                    case 17:
                        MessageOut = info.INFO_17(test, test.Length);
                        strTXT = TXTClass.AIS_Insert(MessageOut, RecData);
                        break;
                    case 18:
                        MessageOut = info.INFO_18(test);
                        strTXT = TXTClass.AIS_Insert(MessageOut, RecData);
                        break;
                    case 19:
                        MessageOut = info.INFO_19(test);
                        strTXT = TXTClass.AIS_Insert(MessageOut, RecData);
                        break;
                    case 21:
                        MessageOut = info.INFO_21(test);
                        strTXT = TXTClass.AIS_Insert(MessageOut, RecData);
                        break;
                }

                AISDataGridViewLoad(MessageOut, RecData.StrDateTime, RecData.power.ToString("f2"));
                if (strTXT != "")
                {
                    if (RecData.ChalNo == 0x31)
                    {
                        AIS1_count++;
                    }
                    else if (RecData.ChalNo == 0x32)
                    {
                        AIS2_count++;
                    }
                    //界面显示和计数
                    strTXT = "AIS " + strTXT;
                    Filestr_Decode.Enqueue(strTXT);
                }
                

            }
            catch (System.Exception ex)
            {
                ErrorRecord.ProcessError(ex.ToString());
            }
        }

        private void ACARS_SaveToSql(AcarsInfo.ACARS_MessageInfo ACARS_info, byte[] test, DataClass RecData, int CRC, int IQ_Flag)
        {
            try
            {
                string strSql;
                AcarsInfo.ACARS_MessageInfo ACARS_Precess = new AcarsInfo.ACARS_MessageInfo();
                ACARS_info = ACARS_Precess.INFO_ACARS(test, test.Length, CRC);
                ACARSDataGridViewLoad(ACARS_info, RecData.StrDateTime,RecData.power.ToString("f2"));
                if (ACARS_info.RevData != null)
                {
                    string strTXT;


                        strTXT = TXTClass.ACARS_Insert(ACARS_info, RecData);
                        if (strTXT != "")
                        {
                            //界面显示和计数
                            if (RecData.ChalNo == 0x33)
                            {
                                ACARS1_count++;
                            }
                            else if (RecData.ChalNo == 0x34)
                            {
                                ACARS2_count++;
                            }
                            else if (RecData.ChalNo == 0x35)
                            {
                                ACARS3_count++;
                            }
                            else if (RecData.ChalNo == 0x36)
                            {
                                ACARS4_count++;
                            }
                            strTXT = "ACARS " + strTXT;
                            Filestr_Decode.Enqueue(strTXT);

                        }
                        
                    }
                
            }
            catch (System.Exception ex)
            {
                ErrorRecord.ProcessError(ex.ToString());
            }
        }
        public static string ByteArrayToString(byte[] buffer)
        {
            string hexString = string.Empty;

            if (buffer != null)
            {
                StringBuilder strB = new StringBuilder();
                for (int i = 0; i < buffer.Length; i++)
                {
                    strB.Append(buffer[i].ToString("X2"));
                    strB.Append(" ");
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        //函数——保存原始数据(修改后)
        private void SaveOriginalData(string Dtype, int ChannelNo, byte[] btdata, DateTime dtDate)
        {
            try
            {
                string tableName = "original_data_";
                DateTime dtTime = DateTime.Now;
                switch (Dtype)
                {
                    case "AIS":
                        tableName = tableName + Dtype + "_";
                        break;
                    case "ACARS":
                        tableName = tableName + Dtype + "_";
                        break;
                    case "ADS":
                        tableName = tableName + Dtype + "_";
                        break;
                }

            }
            catch (System.Exception ex)
            {
                ErrorRecord.ProcessError(ex.ToString());
            }
        }
       

        //经度_longitude 函数
        public static string GetLongitude(double key)
        {
            string strReturn = "";
            if (key < 0 && key >= -180)
            {
                double num = Math.Abs(key);
                strReturn = "W " + num.ToString("f3");
            }
            else if (key > 0 && key <= 180)
            {
                double num = Math.Abs(key);
                strReturn = "E " + num.ToString("f3");
            }
            else if (key == 0)
            {
                strReturn = "0";
            }

            return strReturn;
        }

        //纬度_latitude 函数
        public static string GetLatitude(double key)
        {
            string strReturn = "";
            if (key < 0 && key >= -180)
            {
                double num = Math.Abs(key);
                strReturn = "S " + num.ToString("f3");
            }
            else if (key > 0 && key <= 180)
            {
                double num = Math.Abs(key);
                strReturn = "N " + num.ToString("f3");
            }
            else if (key == 0)
            {
                strReturn = "0";
            }

            return strReturn;

        }
        //函数——接收设备信息并存入数据库(修改后)
        private void Device_SaveToSql(string Datatype, double lon, double lat, string date)
        {
            try
            {
                if (lon != 0 && lat != 0)
                {
                    string strLon = GetLongitude(lon);
                    string strLat = GetLatitude(lat);
                    string DataFlag = "";
                    if (Datatype == "ST")
                        DataFlag = "时统";
                    else if (Datatype == "BZ")
                        DataFlag = "北斗";
                    else
                        DataFlag = "本地配置";
                    string strSql = "insert into device_info (fid,DataType,CreatDate,longitude,latitude)VALUES(UUID(),'" + DataFlag + "','" + date + "','" + strLon + "','" + strLat + "')";
                    //SqlCon.getInsert(strSql);
                }
            }
            catch (System.Exception ex)
            {
                ErrorRecord.ProcessError(ex.ToString());
            }
        }

        public static void Add_MapADS(uint icao, string lon, string lat)
        {
            try
            {
                string data = DateTime.Now.ToString();
                string sql = "INSERT INTO map_ads (fid,ICAO,longitude,latitude,CreatDate) VALUE(UUID()," + icao + ",'" + lon + "','" + lat + "','" + data + "')";
                //MainForm.SqlCon.getInsert(sql);
            }
            catch (System.Exception ex)
            {
                ErrorRecord.ProcessError(ex.ToString());
            }
        }
        //函数——接收ADS处理并存入数据库
        private void ADS_SaveToSql(ADS_B_INFO.ADS_MessageInfo ADS_info, byte[] test, DateTime date, DataClass RecData, int CRC, byte Military)
        {
            try
            {
                ADS_B_INFO.ADS_MessageInfo info = new ADS_MessageInfo();
                string strTXT;
                string MilitaryType;
                /*************************写解译文件*******************************/
                //if (RecData.Messageinfo[4] == 0)
                {
                    if (Military == 19)
                    {
                        MilitaryType = "军机";
                    }
                    else
                    {
                        MilitaryType = "民机";
                    }
                    switch (test[3])
                    {
                        case 0:
                            break;
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                            info = ADS_MessageInfo.GetAirPlaneID(test);
                            ADSDataGridViewLoad(info, RecData.StrDateTime, MilitaryType, RecData.power.ToString("f2"));
                            break;
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                            info = ADS_MessageInfo.GetEarthLocation(test, RecData.dLon, RecData.dLat, RecData.ads_time);
                            ADSDataGridViewLoad(info, RecData.StrDateTime, MilitaryType, RecData.power.ToString("f2"));
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
                            info = ADS_MessageInfo.GetAirLocation(test, RecData.dLon, RecData.dLat, RecData.ads_time);
                            ADSDataGridViewLoad(info, RecData.StrDateTime, MilitaryType, RecData.power.ToString("f2"));
                            break;
                        case 19:
                            info = ADS_MessageInfo.GetAirSpeed(test);
                            ADSDataGridViewLoad(info, RecData.StrDateTime, MilitaryType, RecData.power.ToString("f2"));
                            break;
                        case 20:
                        case 21:
                        case 22:
                            info = ADS_MessageInfo.GetAirLocation(test, RecData.dLon, RecData.dLat, RecData.ads_time);
                            ADSDataGridViewLoad(info, RecData.StrDateTime, MilitaryType, RecData.power.ToString("f2"));
                            break;
                        case 28:
                            info = ADS_MessageInfo.GetPauseAirCondition(test);
                            ADSDataGridViewLoad(info, RecData.StrDateTime, MilitaryType, RecData.power.ToString("f2"));
                            break;
                        case 29:
                            info = ADS_MessageInfo.GetTargetState(test);
                            ADSDataGridViewLoad(info, RecData.StrDateTime, MilitaryType, RecData.power.ToString("f2"));
                            break;
                        case 31:
                            info = ADS_MessageInfo.GetAirCondition(test);
                            ADSDataGridViewLoad(info, RecData.StrDateTime, MilitaryType, RecData.power.ToString("f2"));
                            break;
                        default:
                            break;
                    }

                    if (info.ICAO != 0)
                    {
                        strTXT = TXTClass.ADS_Insert(info, RecData);

                        if (strTXT != "")
                        {
                            //数据显示
                            ADS_count++;
                            //存文件
                            strTXT = "ADS-B " + strTXT;
                            Filestr_Decode.Enqueue(strTXT);
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                ErrorRecord.ProcessError(ex.ToString());
            }
        }

        private void DevButten_Click(object sender, EventArgs e)
        {
            if (DevButten.Text == "打开设备" && DeviceFlag == false)
            {
                DevButten.Text = "关闭设备";
                OpenDevice();
                AIS1_count = 0;
                AIS2_count = 0;
                ADS_count = 0;
                ACARS1_count = 0;
                ACARS2_count = 0;
                ACARS3_count = 0;
                ACARS4_count = 0;
            }
            else if (DevButten.Text == "关闭设备" && DeviceFlag == true)
            {
                DevButten.Text = "打开设备";
                CloseDevice();
                label_device.Text = "设备关闭";
            }
        }
        public double GetDistance(double lat, double lon, double LocalLat, double LocalLon)
        {
            double distance = 0;
            distance = Math.Sqrt(Math.Pow(Math.Abs(lat - LocalLat) * 111, 2) + Math.Pow(Math.Abs(lon - LocalLon) * 111, 2));
            return distance;
        }
        public void ADSDataGridViewLoad(ADS_B_INFO.ADS_MessageInfo ADS_info, string dt, string Military,string PA)
        {
            try
            {
                GridInfoClass GridInfo = new GridInfoClass();
                GridInfo.DataType = "ADS-B";
                GridInfo.FirstTime = dt;
                GridInfo.LastTime = dt;
                GridInfo.ID = ADS_info.ICAO.ToString("x");
                if (ADS_info.longitude != null && ADS_info.latitude != null)
                {
                    if (CheckRegion(double.Parse(ADS_info.latitude), double.Parse(ADS_info.longitude), dLatitude, dLongitude))
                    {
                        GridInfo.Lon = ADS_info.longitude;
                        GridInfo.Lat = ADS_info.latitude;
                        GridInfo.Distance = GetDistance(double.Parse(GridInfo.Lat), double.Parse(GridInfo.Lon), dLatitude, dLongitude).ToString("f4");
                    }
                }
                GridInfo.Height = ADS_info.Height.ToString();
                GridInfo.Military = Military;
                GridInfo.Name = ADS_info.AirPlaneID;
                GridInfo.National = DataFormat.GetICAOCountryName(ADS_info.ICAO);
                GridInfo.CatchNum = "1";
                GridInfo.PA = PA;
                if (GridInfo.Lat != null && GridInfo.Lon != null)
                {
                    GridInfo.Distance = GetDistance(double.Parse(GridInfo.Lat), double.Parse(GridInfo.Lon), dLatitude, dLongitude).ToString("f4");
                }
                GridInfoQueue.Enqueue(GridInfo);
                
            }
            catch (System.Exception ex)
            {
                ErrorRecord.ProcessError(ex.ToString());
            }
        }
        public void ClearGridViewData(string datetime)
        {
            for (int i = 0; i < DataGridList.Rows.Count; i++)
            {
                DateTime Ndt = Convert.ToDateTime(datetime);
                DateTime Odt = Convert.ToDateTime(DataGridList.Rows[i]["末次时间"]);
                TimeSpan sp = Odt - Ndt;
                if (Math.Abs(sp.TotalSeconds) > (StayTimeSet*60))
                {
                    DataGridList.Rows.RemoveAt(i);
                }
            }
        }
        public void AISDataGridViewLoad(AIS_INFO.AIS_MessageInfo AIS_info, string dt,string PA)
        {
            try
            {
                GridInfoClass GridInfo = new GridInfoClass();
                GridInfo.DataType = "AIS";
                GridInfo.FirstTime = dt;
                GridInfo.LastTime = dt;
                GridInfo.ID = AIS_info.MMSI.ToString("x");
                GridInfo.Height = "";
                GridInfo.Military = "民用";
                GridInfo.Name = AIS_info.ID;
                GridInfo.National = DataFormat.GetCountryName((int)AIS_info.MMSI);
                GridInfo.CatchNum = "1";
                GridInfo.PA = PA;
                GridInfo.Lon = AIS_info.longitude;
                GridInfo.Lat = AIS_info.latitude;
                if (GridInfo.Lat != null && GridInfo.Lon != null && GridInfo.Lat != "0" && GridInfo.Lon != "0" && GridInfo.Lat != "" && GridInfo.Lon != "")
                {
                    if (dLatitude > -90 && dLatitude < 90 && dLongitude > -180 && dLongitude < 180)
                    {
                        if (CheckRegion(double.Parse(AIS_info.latitude), double.Parse(AIS_info.longitude), dLatitude, dLongitude))
                        {
                            GridInfo.Distance = GetDistance(double.Parse(GridInfo.Lat), double.Parse(GridInfo.Lon), dLatitude, dLongitude).ToString("f4");
                        }
                    }
                }
                GridInfoQueue.Enqueue(GridInfo);
            }
            catch (System.Exception ex)
            {
                ErrorRecord.ProcessError(ex.ToString());
            }
        }

        public void ACARSDataGridViewLoad(AcarsInfo.ACARS_MessageInfo ACARS_info, string dt,string PA)
        {
            try
            {
                GridInfoClass GridInfo = new GridInfoClass();
                GridInfo.DataType = "ACARS";
                GridInfo.FirstTime = dt;
                GridInfo.LastTime = dt;
                GridInfo.ID = ACARS_info.ICAO;
                GridInfo.Lon = ACARS_info.longitude;
                GridInfo.Lat = ACARS_info.latitude;
                GridInfo.Height = ACARS_info.ALT_Height;
                GridInfo.Military = "民用";
                GridInfo.Name = ACARS_info.FlightNumder;
                GridInfo.National = ACARS_info.Nationality;
                GridInfo.CatchNum = "1";
                GridInfo.PA = PA;
                if (GridInfo.Lat != null && GridInfo.Lon != null)
                {
                    GridInfo.Distance = GetDistance(double.Parse(GridInfo.Lat), double.Parse(GridInfo.Lon), dLatitude, dLongitude).ToString("f4");
                }
                GridInfoQueue.Enqueue(GridInfo);
            }
            catch (System.Exception ex)
            {
                ErrorRecord.ProcessError(ex.ToString());
            }
        }
        private void TimeRefresh_Tick(object sender, EventArgs e)
        {
            
            ADSCount.Text = ADS_count.ToString();
            AIS1Count.Text = AIS1_count.ToString();
            AIS2Count.Text = AIS2_count.ToString();
            ACARS1Count.Text = ACARS1_count.ToString();
            ACARS2Count.Text = ACARS2_count.ToString();
            ACARS3Count.Text = ACARS3_count.ToString();
            ACARS4Count.Text = ACARS4_count.ToString();
            label13.Text = dLongitude.ToString("f4");
            label15.Text = dLatitude.ToString("f4");
            label9.Text = PlxRecvNum.ToString();
            double PlxSpeed = (double)(((double)PlxRecvNum - (double)LastPlxNum) / (double)64);
            LastPlxNum = PlxRecvNum;
            label11.Text = PlxSpeed.ToString("f4") + "Mb/s";
            TimeSpan sp = DateTime.Now - RunDisTime;
            RunTime.Text = ((int)sp.TotalDays).ToString() + "天" + ((int)sp.TotalHours % 24).ToString() + "小时" + ((int)sp.TotalMinutes % 60).ToString() + "分钟" + ((int)sp.TotalSeconds  % 60).ToString() + "秒";
            ADS_BIdNum.Text = ADSTargetNum.ToString();
            AISIdNum.Text = AISTargetNum.ToString();
            ACARSIdNum.Text = ACARSTargetNum.ToString();
        }

        private void ParamSet_Click(object sender, EventArgs e)
        {
            int iRet = -1;
            
            switch (ChannelList.SelectedIndex)
            { 
                case 0 :
                    AIS1FreqSet = double.Parse(FreqPoint.Text);
                    AIS1AttenSet = byte.Parse(Atten.Text);
                    iRet = PlxApi.SetAISParam(0x01, AIS1FreqSet, AIS1AttenSet);
                    if (iRet == 0)
                    { 
                        MessageBox.Show("参数设置成功");
                    }
                    else
                    {
                        MessageBox.Show("参数设置失败");
                    }
                    break;
                case 1:
                    AIS2FreqSet = double.Parse(FreqPoint.Text);
                    AIS2AttenSet = byte.Parse(Atten.Text);
                    iRet = PlxApi.SetAISParam(0x02, AIS2FreqSet, AIS2AttenSet);
                    if (iRet == 0)
                    {
                        MessageBox.Show("参数设置成功");
                    }
                    else
                    {
                        MessageBox.Show("参数设置失败");
                    }
                    break;
                case 2:
                    ACARS1FreqSet = double.Parse(FreqPoint.Text);
                    ACARS1AttenSet = byte.Parse(Atten.Text);
                    iRet = PlxApi.SetACARSParam(0x03, ACARS1FreqSet, ACARS1AttenSet);
                    if (iRet == 0)
                    {
                        MessageBox.Show("参数设置成功");
                    }
                    else
                    {
                        MessageBox.Show("参数设置失败");
                    }
                    break;
                case 3:
                    ACARS2FreqSet = double.Parse(FreqPoint.Text);
                    ACARS2AttenSet = byte.Parse(Atten.Text);
                    iRet =  PlxApi.SetACARSParam(0x04, ACARS2FreqSet, ACARS2AttenSet);
                    if (iRet == 0)
                    {
                        MessageBox.Show("参数设置成功");
                    }
                    else
                    {
                        MessageBox.Show("参数设置失败");
                    }
                    break;
                case 4:
                    ACARS3FreqSet = double.Parse(FreqPoint.Text);
                    ACARS3AttenSet = byte.Parse(Atten.Text);
                    iRet = PlxApi.SetACARSParam(0x05, ACARS3FreqSet, ACARS3AttenSet);
                    if (iRet == 0)
                    {
                        MessageBox.Show("参数设置成功");
                    }
                    else
                    {
                        MessageBox.Show("参数设置失败");
                    }
                    break;
                case 5:
                    ACARS4FreqSet = double.Parse(FreqPoint.Text);
                    ACARS4AttenSet = byte.Parse(Atten.Text);
                    iRet = PlxApi.SetACARSParam(0x06, ACARS4FreqSet, ACARS4AttenSet);
                    if (iRet == 0)
                    {
                        MessageBox.Show("参数设置成功");
                    }
                    else
                    {
                        MessageBox.Show("参数设置失败");
                    }
                    break;
                default: break;
            }
        }

        private void ChannelList_SelectedIndexChanged(int SelectedIndex)
        {
            switch (SelectedIndex)
            {
                case 0:
                    FreqPoint.Text = AIS1FreqSet.ToString();
                    Atten.Text = AIS1AttenSet.ToString();
                    break;
                case 1:
                    FreqPoint.Text = AIS2FreqSet.ToString();
                    Atten.Text = AIS2AttenSet.ToString();
                    break;
                case 2:
                    FreqPoint.Text = ACARS1FreqSet.ToString();
                    Atten.Text = ACARS1AttenSet.ToString();
                    break;
                case 3:
                    FreqPoint.Text = ACARS2FreqSet.ToString();
                    Atten.Text = ACARS2AttenSet.ToString();
                    break;
                case 4:
                    FreqPoint.Text = ACARS3FreqSet.ToString();
                    Atten.Text = ACARS3AttenSet.ToString();
                    break;
                case 5:
                    FreqPoint.Text = ACARS4FreqSet.ToString();
                    Atten.Text = ACARS4AttenSet.ToString();
                    break;
                default: break;
            }
        }

        private void ChannelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChannelList_SelectedIndexChanged(ChannelList.SelectedIndex);
        }

        private void ProperSetButten_Click(object sender, EventArgs e)
        {
            StayTimeSet = long.Parse(StayTime.Text);
            if (radioButton1.Checked)
            {
                isGpsEnable = false;
                dLongitude = double.Parse(LocalLon.Text);
                dLatitude = double.Parse(LocalLat.Text);
            }
            else
            {
                isGpsEnable = true;
                LocalLon.Text = dLongitude.ToString("f4");
                LocalLat.Text = dLatitude.ToString("f4");
            }
        }
    }
}
