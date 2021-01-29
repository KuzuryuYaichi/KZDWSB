using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace KZDWSB
{
    class DataClass
    {
        public byte DataType { get; set; }
        public byte ChalType { get; set; }
        public int ChalNo { get; set; }
        public int DataLength { get; set; }
        public double dLon { get; set; }//by cgd
        public double dLat { get; set; }//by cgd
        public string StrDateTime { get; set; }
        public DateTime dtDateTime { get; set; }
        public double power { get; set; }//by cgd
        public uint ads_time { get; set; }//by cgd
        //public byte[] OldMessageinfo { get; set; }//by cgd
        public byte[] Messageinfo { get; set; }

        public string SaveDate { get; set; }

        public bool GetDatetime(string strSecond, string strTime, string strDate)
        {
            //StrDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            StrDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            dtDateTime = DateTime.Now;
            try
            {
                bool SecondFlag = Regex.IsMatch(strSecond, @"([0-9][0-9][0-9][0-9][0-9][0-9])");
                bool TimeFlag = Regex.IsMatch(strTime, @"((([0-1][0-9])|([2][0-3]))[0-5][0-9][0-5][0-9])");
                bool DataFlag = Regex.IsMatch(strDate, @"([0-9][0-9][0-9][0-9][0-9][0-9])");


                if (strDate.Length == 6 && strSecond.Length == 7 && strTime.Length == 6 && SecondFlag == true && TimeFlag == true && DataFlag == true)
                {
                    string date = "20" + strDate.Substring(4, 2) + "/" + strDate.Substring(2, 2) + "/" + strDate.Substring(0, 2);
                    string time = strTime.Substring(0, 2) + ":" + strTime.Substring(2, 2) + ":" + strTime.Substring(4, 2);
                    string strDateTime = date + " " + time;
                    DateTime dt = DateTime.ParseExact(strDateTime, "yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.CurrentCulture);
                    DateTime Newdt = dt.AddHours(8);
                    string NewTime = Newdt.ToString("yyyy/MM/dd HH:mm:ss");
                    strSecond = strSecond.Substring(0, 6);
                    StrDateTime = NewTime + "." + strSecond;
                    dtDateTime = DateTime.Parse(NewTime);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        //设置ACARS的GPS经度纬度信息（原始数据除以10）
        //参数1：纬度半球，参数2：纬度
        public static double GetGPSLonLat(string flag,string strlon)
        {
            double lon = 0;
            try
            {
                bool LonFlag = Regex.IsMatch(strlon, @"([0-9][0-9][0-9][0-9][0-9]+\.[0-9][0-9][0-9][0-9][0-9])");
                bool LatFlag = Regex.IsMatch(strlon, @"([0-9][0-9][0-9][0-9]+\.[0-9][0-9][0-9][0-9][0-9])");
                if (LonFlag == true)
                {
                    strlon = strlon.Replace(" ", "");
                    int LonFiret = Convert.ToInt32(strlon.Substring(0, 3));
                    double LonEnd = Convert.ToDouble(strlon.Substring(3)) / 60;
                    lon = LonFiret + LonEnd;
                    if (flag == "W")
                    {
                        lon = -lon;
                    }
                }
                else if (LatFlag == true)
                {
                    strlon = strlon.Replace(" ", "");
                    strlon = strlon.Replace(" ", "");
                    int LonFiret = Convert.ToInt32(strlon.Substring(0, 2));
                    double LonEnd = Convert.ToDouble(strlon.Substring(2)) / 60;
                    lon = LonFiret + LonEnd;
                    if (flag == "S")
                    {
                        lon = -lon;
                    }
                }
   
 
            }
            catch
            {
            }
            return lon;
        }

        //获取ADS_B 所需时间  from GPS
        public static uint GetADStimeForGPS(string strTime)
        {
            uint Utime = 0;
            try
            {
                bool TimeFlag = Regex.IsMatch(strTime, @"((([0-1][0-9])|([2][0-3]))[0-5][0-9][0-5][0-9])");

                if (strTime.Length == 6 && TimeFlag == true)
                {
                    uint miunte = Convert.ToUInt32(strTime.Substring(2, 2));
                    uint second = Convert.ToUInt32(strTime.Substring(4, 2));
                    Utime = miunte * 60 + second;
                }
            }
            catch
            {
            }
            return Utime;
        }
        //获取ADS_B 所需时间  from GPS
        public static uint GetADStime(DateTime data)
        {
            uint Utime = 0;
            try
            {
                Utime = (uint)((data.Minute) * 60 + data.Second);
            }
            catch
            {
            }
            return Utime;
        }

    }
}
