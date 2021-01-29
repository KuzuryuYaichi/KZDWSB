using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AcarsInfo
{

    public class ACARS_MessageInfo
    {
        //系统参数
        public int Channel { get; set; }
        public int Error_Flag { get; set; }

        public bool LonLatFlag { get; set; }

        public string ICAO { get; set; }
        public string Label { get; set; }

        public string AirplaneNo { get; set; } //机型（飞机机翼号）
        public string FlightNumder { get; set; }
        public string FlightCompany { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string DEP { get; set; }
        public string StartingPoint { get; set; }
        public string DES { get; set; }
        public string Destination { get; set; }
        public string ETD { get; set; }
        public string ETA { get; set; }
        public string ATA { get; set; }
        public string FOB_OilQuantity { get; set; }
        public string Nationality { get; set; }

        public string UTC_Date { get; set; }
        public string WD_Speed { get; set; }
        public string ALT_Height { get; set; }
        public string WD_Direction { get; set; }
        public string CAS_WindSpeed { get; set; }

        public string RevData { get; set; }
        public string ErrorString { get; set; }

        public static double Local_dLongitude = 0;

        public static double Local_dLatitude = 0;


        //设置ACARS的经度信息
        public static double SetCLon(string strlon)
        {
            double lon = 0;
            try
            {
                strlon = strlon.Replace(" ", "");
                if (strlon.Length > 6)
                {
                    string strheader = strlon.Substring(0, 1);
                    if (strheader == "E")
                    {
                        if (strlon.IndexOf(".") < 0)
                        {
                            int Lon = int.Parse(strlon.Substring(1));
                            if (strlon.Substring(1).Length == 6)
                            {
                                lon = Convert.ToDouble((double)Lon / 1000);
                            }
                            else if (strlon.Substring(1).Length == 5)
                            {
                                lon = Convert.ToDouble((double)Lon / 100);
                            }
                        }
                        else
                        {
                            lon = Convert.ToDouble(strlon.Substring(1));
                        }

                    }
                    else if (strheader == "W")
                    {
                        if (strlon.IndexOf(".") < 0)
                        {
                            int Lon = int.Parse(strlon.Substring(1));
                            if (strlon.Substring(1).Length == 6)
                            {
                                lon = -Convert.ToDouble((double)Lon / 1000);
                            }
                            else if (strlon.Substring(1).Length == 5)
                            {
                                lon = -Convert.ToDouble((double)Lon / 100);
                            }
                        }
                        else
                        {
                            lon = -Convert.ToDouble(strlon.Substring(1));
                        }

                    }

                }


            }
            catch (System.Exception ex)
            {
                //ErrorRecord.ProcessError(ex.ToString());
            }
            return lon;
        }
        //设置ACARS的纬度信息
        public static double SetCLat(string strlat)
        {
            double lat = 0;
            try
            {
                strlat = strlat.Replace(" ", "");
                if (strlat.Length > 1)
                {
                    string strheader = strlat.Substring(0, 1);
                    if (strheader == "N")
                    {
                        if (strlat.IndexOf(".") < 0)
                        {
                            int Lat = int.Parse(strlat.Substring(1));
                            lat = Convert.ToDouble((double)Lat / 1000);
                        }
                        else
                        {
                            lat = Convert.ToDouble(strlat.Substring(1));
                        }

                    }
                    else if (strheader == "S")
                    {
                        if (strlat.IndexOf(".") < 0)
                        {
                            int Lat = int.Parse(strlat.Substring(1));
                            lat = -Convert.ToDouble((double)Lat / 1000);
                        }
                        else
                        {
                            lat = -Convert.ToDouble(strlat.Substring(1));
                        }
                    }
                }


            }
            catch (System.Exception ex)
            {
                //ErrorRecord.ProcessError(ex.ToString());
            }
            return lat;
        }
        //获取ACARS的经度信息
        public static string GetLon(string strlon)
        {
            string lon = "";
            try
            {
                strlon = strlon.Replace(" ", "");
                string strheader = strlon.Substring(0, 1);
                if (Local_dLongitude != 0)
                {
                    if ((strheader == "E" && Local_dLongitude < 0) || (strheader == "W" && Local_dLongitude > 0))
                    {
                        return lon;
                    }

                }
                double dLon = double.Parse(strlon.Remove(0, 1));
                if (dLon <= 180 && dLon >= 0)
                {
                    lon = strlon.Substring(1);
                }
                else if (dLon <= 1800 && dLon > 180)
                {
                    dLon = (double)dLon / 10;
                    lon = dLon.ToString("F2");
                }
                else if (dLon <= 18000 && dLon > 1800)
                {
                    dLon = (double)dLon / 100;
                    lon = dLon.ToString("F2");
                }
                else if (dLon <= 180000 && dLon > 18000)
                {
                    dLon = (double)dLon / 1000;
                    lon = dLon.ToString("F2");
                }
                //if (dLon <= 180 && dLon >= 0)
                //{
                //    lon = strheader + " " + strlon.Substring(1);
                //}
                //else if (dLon <= 1800 && dLon > 180)
                //{
                //    dLon = (double)dLon / 10;
                //    lon = strheader + " " + dLon.ToString("F2");
                //}
                //else if (dLon <= 18000 && dLon > 1800)
                //{
                //    dLon = (double)dLon / 100;
                //    lon = strheader + " " + dLon.ToString("F2");
                //}
                //else if (dLon <= 180000 && dLon > 18000)
                //{
                //    dLon = (double)dLon / 1000;
                //    lon = strheader + " " + dLon.ToString("F2");
                //}

            }
            catch (System.Exception ex)
            {
                //ErrorRecord.ProcessError(ex.ToString());
            }
            return lon;
        }
        //获取ACARS的纬度信息
        public static string GetLat(string strlat)
        {
            string lat = "";
            try
            {
                strlat = strlat.Replace(" ", "");
                string strheader = strlat.Substring(0, 1);
                if (Local_dLatitude != 0)
                {
                    if ((strheader == "N" && Local_dLatitude < 0) || (strheader == "S" && Local_dLatitude > 0))
                    {
                        return lat;
                    }

                }
                //double dLon = double.Parse(strlat.Remove(0, 1));
                //if (dLon <= 180 && dLon >= 0)
                //{
                //    lat = strheader + " " + strlat.Substring(1);
                //}
                //else if (dLon <= 1800 && dLon > 180)
                //{
                //    dLon = (double)dLon / 10;
                //    lat = strheader + " " + dLon.ToString("F2");
                //}
                //else if (dLon <= 18000 && dLon > 1800)
                //{
                //    dLon = (double)dLon / 100;
                //    lat = strheader + " " + dLon.ToString("F2");
                //}
                //else if (dLon <= 180000 && dLon > 18000)
                //{
                //    dLon = (double)dLon / 1000;
                //    lat = strheader + " " + dLon.ToString("F2");
                //}
                double dLon = double.Parse(strlat.Remove(0, 1));
                if (dLon <= 180 && dLon >= 0)
                {
                    lat =  strlat.Substring(1);
                }
                else if (dLon <= 1800 && dLon > 180)
                {
                    dLon = (double)dLon / 10;
                    lat = dLon.ToString("F2");
                }
                else if (dLon <= 18000 && dLon > 1800)
                {
                    dLon = (double)dLon / 100;
                    lat = dLon.ToString("F2");
                }
                else if (dLon <= 180000 && dLon > 18000)
                {
                    dLon = (double)dLon / 1000;
                    lat = dLon.ToString("F2");
                }

            }
            catch (System.Exception ex)
            {
                //ErrorRecord.ProcessError(ex.ToString());
            }
            return lat;
        }
        //ACARS结构体转换
        public ACARS_MessageInfo INFO_ACARS(byte[] ACARSData, int ALength, int CRC)
        {
            ACARS_MessageInfo info = new ACARS_MessageInfo();
            try
            {
                if (CRC == 1)
                {
                    //奇校验
                    info.ErrorString = GetOddCheck(ACARSData);
                }
                //去掉校验位
                byte[] ACARSData_New = GetNewData(ACARSData);
                //ACARS接收数组第二位置为“：”
                string cstrRecv, cstrRecv1, cstrRecv2 = string.Empty;
                if (ALength > 27)
                {
                    cstrRecv1 = System.Text.Encoding.ASCII.GetString(ACARSData_New.Take(0).ToArray());
                    cstrRecv2 = System.Text.Encoding.ASCII.GetString(ACARSData_New.Skip(1).Take(ALength - 3).ToArray());
                    cstrRecv = (cstrRecv1 + ":" + cstrRecv2).Replace("'", " ");

                    //cstrRecv = ":2.B-6216104M76ACA1772POS191117,N 38.741,E117.366,1244,  339125,,,13340,194";
                    info.RevData = cstrRecv;

                    info.ICAO = cstrRecv.Substring(2, 7);  //注册号向后推1字节，byCYJ
                    if (!Regex.Match(info.ICAO.Substring(0, 1), @"[A-Z]|[0-9]").Success)  //去掉注册号里的无效字符，byCYJ
                    {
                        info.ICAO = info.ICAO.Substring(1).PadRight(7);
                        if (!Regex.Match(info.ICAO.Substring(0, 1), @"[A-Z]|[0-9]").Success)
                        {
                            info.ICAO = info.ICAO.Substring(1).PadRight(7);
                        }
                    }
                    info.ICAO = info.ICAO.Trim();
                    info.Label = cstrRecv.Substring(10, 2);
                    info.AirplaneNo = cstrRecv.Substring(14, 4);
                    //info.AirplaneNo = DataFormat.GetAirCraftType(info.ICAO.PadRight(7));  //根据飞机注册号判断jixing，byCYJ
                    //info.Nationality = DataFormat.GetAcarsCountryOrigin(info.ICAO.PadRight(7));  //根据飞机注册号判断国籍，byCYJ


                    #region 正则表达式
                    string PATTERN_FLIGHT = @"(([0-9][A-Z])|([A-Z][0-9])|([A-Z][A-Z]))([0-9]{4})";
                    string PATTERN_LONGITUDE_1 = @"[EW](|(\s))(([1][0-7][0-9]+\.([0-9]{4}))|([0-9][0-9]+\.([0-9]{4}))|((|[1])[0-7][0-9]+\.([0-9]{3}))|([0-9][0-9]+\.([0-9]{3}))|([1][0-7][0-9]+\.([0-9]{1}))|([0-9][0-9]+\.([0-9]{1})))";  //添加123.4的情况，byCYJ
                    string PATTERN_LATITUDE_1 = @"[NS](|(\s))(([0-8][0-9]+\.([0-9]{4}))|([0-8][0-9]+\.([0-9]{3}))|([0-8][0-9][0-9][0-9]+\.([0-9]{1})))";  //添加1234.5的情况，byCYJ
                    string PATTERN_LONGITUDE_2 = @"[EW](|(\s))(([0-9][0-9][0-9][0-9][0-9])|([0-9][0-9][0-9][0-9][0-9]+\.([0-9]{1}))|([0-9][0-9][0-9](|[1])[0-9][0-9][0-9]))";  //添加12345.6、123456、123 456 3种情况，byCYJ
                    string PATTERN_LATITUDE_2 = @"[NS](|(\s))([0-8][0-9][0-9][0-9][0-9])";


                    string PATTERN_DEP1 = @"[D][E][P]((\s)|(\s\s)|(\s\s\s))([A-Z][A-Z][A-Z][A-Z])";
                    string PATTERN_DEP2 = @"[O][R][G]((\s)|(\s\s)|(\s\s\s))([A-Z][A-Z][A-Z][A-Z])";  //增加起飞机场正则，byCYJ
                    string PATTERN_DEP3 = @"[A-Z][A-Z][0-9][0-9][0-9][0-9]/[A-Z][A-Z][A-Z][A-Z][A-Z][A-Z]";
                    string PATTERN_DEPDES1 = @"/[A-Z][A-Z][A-Z][A-Z]+\.[A-Z][A-Z][0-9]/[0-9][0-9][0-9][A-Z][A-Z][A-Z][A-Z]";  //增加机场正则，byCYJ
                    string PATTERN_DEPDES2 = @"[A-Z][A-Z][0-9][0-9][0-9][0-9][A-Z][A-Z][A-Z][A-Z],[A-Z][A-Z][A-Z][A-Z]";  //增加机场正则，byCYJ
                    string PATTERN_DEPDES3 = @",[A-Z][A-Z][A-Z][A-Z],[A-Z][A-Z][A-Z][A-Z]";  //增加机场正则，byCYJ
                    string PATTERN_DES = @"[D][E][S]((\s)|(\s\s)|(\s\s\s))([A-Z][A-Z][A-Z][A-Z])";

                    string PATTERN_ETD = @"[E][T][D]((\s)|(\s\s)|(\s\s\s))((([0-1][0-9])|([2][0-4]))([0-5][0-9])|([0-3][0-9]\s(([0-1][0-9])|([2][0-4]))([0-5][0-9])))";
                    string PATTERN_ETA = @"[E][T][A]((\s)|(\s\s)|(\s\s\s))((([0-1][0-9])|([2][0-4]))([0-5][0-9])|([0-3][0-9]\s(([0-1][0-9])|([2][0-4]))([0-5][0-9])))";
                    string PATTERN_ATA = @"[A][T][A]((\s)|(\s\s)|(\s\s\s))((([0-1][0-9])|([2][0-4]))([0-5][0-9])|([0-3][0-9]\s(([0-1][0-9])|([2][0-4]))([0-5][0-9])))";
                    string PATTERN_FOB = @"[F][O][B]((\s)|(\s\s)|(\s\s\s))(([0-9][0-9][0-9][0-9][0-9])|([0-9][0-9][0-9][0-9])|([0-9][0-9][0-9])|([0-9][0-9]))";

                    string PATTERN_ALT = @"[A][L][T]((\s)|(\s\s)|(\s\s\s))(([0-9][0-9][0-9][0-9][0-9])|([0-9][0-9][0-9][0-9])|([0-9][0-9][0-9])|([0-9][0-9]))";
                    string PATTERN_WD = @"[W][D]((\s)|(\s\s)|(\s\s\s))(([0-9][0-9][0-9][0-9][0-9])|([0-9][0-9][0-9][0-9])|([0-9][0-9][0-9])|([0-9][0-9]))";
                    string PATTERN_CAS = @"[C][A][S]((\s)|(\s\s)|(\s\s\s))(([0-9][0-9][0-9][0-9][0-9])|([0-9][0-9][0-9][0-9])|([0-9][0-9][0-9])|([0-9][0-9]))";


                    //正则表达式——匹配航班号
                    //Match match = Regex.Match(cstrRecv, PATTERN_FLIGHT);
                    //if (match.Success)
                    //{
                    //    info.FlightNumder = match.Groups[0].ToString().Trim();
                    //    info.FlightCompany = info.FlightNumder.Substring(0, 2);
                    //    //int a = cstrRecv.IndexOf(match.Groups[0].ToString());
                    //    //cstrRecv = cstrRecv.Substring(a + match.Groups[0].ToString().Length);
                    //}
                    //else
                    //{
                    //    info.FlightNumder = cstrRecv.Substring(18, 6);
                    //    info.FlightCompany = cstrRecv.Substring(18, 2);
                    //}
                    info.FlightNumder = cstrRecv.Substring(18, 6);
                    //info.FlightCompany = DataFormat.GetFlightCompany(info.FlightNumder.Substring(0, 2)).Replace("'", "''");

                    //正则表达式——经度
                    Match match = Regex.Match(cstrRecv, PATTERN_LONGITUDE_1);
                    if (match.Success)
                    {
                        info.longitude = match.Groups[0].ToString().Trim();
                        info.longitude = GetLon(info.longitude);
                    }
                    else
                    {
                        match = Regex.Match(cstrRecv, PATTERN_LONGITUDE_2);
                        if (match.Success)
                        {
                            info.longitude = match.Groups[0].ToString().Trim();
                            info.longitude = GetLon(info.longitude);
                        }
                        match = Regex.Match(cstrRecv, PATTERN_LATITUDE_2);
                        if (match.Success)
                        {
                            info.latitude = match.Groups[0].ToString().Trim();
                            info.latitude = GetLat(info.latitude);
                        }
                    }
                    //正则表达式——纬度
                    match = Regex.Match(cstrRecv, PATTERN_LATITUDE_1);
                    if (match.Success)
                    {
                        info.latitude = match.Groups[0].ToString().Trim();
                        info.latitude = GetLat(info.latitude);
                    }

                    //正则表达式——DEP起飞机场
                    match = Regex.Match(cstrRecv, PATTERN_DEP1);
                    if (match.Success)
                    {
                        info.DEP = match.Groups[0].ToString().Substring(4).Trim();
                        info.StartingPoint = GetAirport(info.DEP);
                    }
                    else
                    {
                        match = Regex.Match(cstrRecv, PATTERN_DEP2);  //增加的机场正则，byCYJ
                        if (match.Success)
                        {
                            info.DEP = match.Groups[0].ToString().Substring(4).Trim();
                            info.StartingPoint = GetAirport(info.DEP);
                        }
                        else
                        {
                            match = Regex.Match(cstrRecv, PATTERN_DEP3);
                            if (match.Success)
                            {
                                info.DEP = match.Groups[0].ToString().Substring(9).Trim();
                                info.StartingPoint = GetAirport(info.DEP);
                            }
                            else
                            {
                                match = Regex.Match(cstrRecv, PATTERN_DEPDES1);
                                if (match.Success)
                                {
                                    info.DEP = match.Groups[0].ToString().Substring(1, 4).Trim();
                                    info.StartingPoint = GetAirport(info.DEP);
                                }
                                else
                                {
                                    match = Regex.Match(cstrRecv, PATTERN_DEPDES2);
                                    if (match.Success)
                                    {
                                        info.DEP = match.Groups[0].ToString().Substring(6, 4).Trim();
                                        info.StartingPoint = GetAirport(info.DEP);
                                    }
                                    else
                                    {
                                        match = Regex.Match(cstrRecv, PATTERN_DEPDES3);
                                        if (match.Success)
                                        {
                                            info.DEP = match.Groups[0].ToString().Substring(1, 4).Trim();
                                            info.StartingPoint = GetAirport(info.DEP);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //正则表达式——DES目的机场
                    match = Regex.Match(cstrRecv, PATTERN_DES);
                    if (match.Success)
                    {
                        info.DES = match.Groups[0].ToString().Substring(4).Trim();
                        info.Destination = GetAirport(info.DES);
                    }
                    else
                    {
                        match = Regex.Match(cstrRecv, PATTERN_DEPDES1);  //增加的机场正则，byCYJ
                        if (match.Success)
                        {
                            info.DES = match.Groups[0].ToString().Substring(13, 4).Trim();
                            info.Destination = GetAirport(info.DES);
                        }
                        else
                        {
                            match = Regex.Match(cstrRecv, PATTERN_DEPDES2);
                            if (match.Success)
                            {
                                info.DES = match.Groups[0].ToString().Substring(11, 4).Trim();
                                info.Destination = GetAirport(info.DES);
                            }
                            else
                            {
                                match = Regex.Match(cstrRecv, PATTERN_DEPDES3);
                                if (match.Success)
                                {
                                    info.DES = match.Groups[0].ToString().Substring(6, 4).Trim();
                                    info.Destination = GetAirport(info.DES);
                                }
                            }
                        }
                    }

                    //正则表达式——ETD起飞时间
                    match = Regex.Match(cstrRecv, PATTERN_ETD);
                    if (match.Success)
                    {
                        string etd = match.Groups[0].ToString();
                        if (etd.Length == 11)
                        {
                            info.ETD = etd.Substring(7).Trim();
                        }
                        else if (etd.Length == 8)
                        {
                            info.ETD = etd.Substring(4).Trim();
                        }

                    }
                    //正则表达式——ETA预计达到时间
                    match = Regex.Match(cstrRecv, PATTERN_ETA);
                    if (match.Success)
                    {
                        string eta = match.Groups[0].ToString();
                        if (eta.Length == 11)
                        {
                            info.ETA = eta.Substring(7).Trim();
                        }
                        else if (eta.Length == 8)
                        {
                            info.ETA = eta.Substring(4).Trim();
                        }
                    }
                    //正则表达式——ATA实际到达时间
                    match = Regex.Match(cstrRecv, PATTERN_ATA);
                    if (match.Success)
                    {
                        string ata = match.Groups[0].ToString();
                        if (ata.Length == 11)
                        {
                            info.ATA = ata.Substring(7).Trim();
                        }
                        else if (ata.Length == 8)
                        {
                            info.ATA = ata.Substring(4).Trim();
                        }
                    }
                    //正则表达式——FOB载油量
                    match = Regex.Match(cstrRecv, PATTERN_FOB);
                    if (match.Success)
                    {
                        info.FOB_OilQuantity = match.Groups[0].ToString().Substring(4).Trim();
                    }
                    //正则表达式——WD航向
                    match = Regex.Match(cstrRecv, PATTERN_WD);
                    if (match.Success)
                    {
                        info.WD_Direction = match.Groups[0].ToString().Substring(3).Trim();
                    }
                    //正则表达式——ALT目标高度
                    match = Regex.Match(cstrRecv, PATTERN_ALT);
                    if (match.Success)
                    {
                        info.ALT_Height = match.Groups[0].ToString().Substring(4).Trim();
                    }
                    //正则表达式——CAS航速
                    match = Regex.Match(cstrRecv, PATTERN_CAS);
                    if (match.Success)
                    {
                        info.CAS_WindSpeed = match.Groups[0].ToString().Substring(4).Trim();
                    }

                    #endregion

                    //判断经纬度是否正确
                    double lon = 0;
                    double lat = 0;
                    info.LonLatFlag = CheckLonLatStringIsTure(info.longitude, info.latitude, out lon, out lat);
                    if (info.LonLatFlag == false)
                    {
                        info.longitude = null;
                        info.latitude = null;
                    }
                    else
                    {
                        //态势数据赋值
                        if (info.ICAO != null && info.ICAO != "" && info.longitude != null && info.latitude != null)
                        {
                            //态势数据赋值
                            //MapSQL.Add_MapACARS(info.ICAO, lon, lat, info.Nationality);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                //ErrorRecord.ProcessError(ex.ToString());
            }
            return info;
        }
        public static bool CheckLonLatStringIsTure(string strlon, string strlat, out double dlon, out double dlat)
        {

            if (strlon == null && strlat == null)
            {
                dlon = 0;
                dlat = 0;
                return true;
            }
            else
            {
                if (strlon != null && strlat != null)
                {
                    dlon = SetCLon(strlon);
                    dlat = SetCLat(strlat);
                    bool flag = true;
                    return flag;
                }
                else
                {
                    dlon = 0;
                    dlat = 0;
                    return false;
                }


            }

        }
        //ACARS奇校验算法
        public static string GetOddCheck(byte[] data)
        {
            string reStr = "";
            try
            {
                for (int n = 0; n < data.Length; n++)
                {
                    int odd = data[n] / 128;
                    int temp = 0;
                    for (int k = 0; k < 7; k++)
                    {
                        int test = (int)(data[n] / Math.Pow(2, k)) % 2;
                        temp += test;
                    }
                    if (temp % 2 == odd)
                    {
                        if (n < 8 && n > 0)
                        {
                            reStr += "[唯一航班号]错误;";
                            n = 10;
                        }
                        else if (n < 13 && n > 10)
                        {
                            reStr += "[标签]错误;";
                            n = 14;
                        }
                        else if (n < 19 && n > 14)
                        {
                            reStr += "[机型]错误;";
                            n = 18;
                        }
                        else if (n < 20 && n > 18)
                        {
                            reStr += "[公司]错误;";
                            n = 19;
                        }
                        else if (n < 25 && n > 18)
                        {
                            reStr += "[航班号]错误;";
                            n = 24;
                        }
                        else if (n > 24)
                        {
                            reStr += "[正文]错误;";
                            break;
                        }
                    }
                }
            }
            catch
            {
            }
            return reStr;
        }
        //ACARS去掉校验位
        public static byte[] GetNewData(byte[] data)
        {
            byte[] rebt = new byte[data.Length];
            try
            {
                for (int n = 0; n < data.Length; n++)
                {
                    rebt[n] = (byte)(data[n] & 0x7f);
                }
            }
            catch
            {
            }
            return rebt;
        }
        public static string GetCountry(string strflag)
        {
            string c = "其他";
            string strcountry = strflag.Substring(0, 1);
            if (Dic_Country.ContainsKey(strflag))
            {
                c = Dic_Country[strflag];
            }
            else if (strcountry == "C")
            {
                c = "加拿大";
            }
            else if (strcountry == "U")
            {
                c = "俄罗斯";
            }
            else if (strcountry == "K")
            {
                c = "美国大陆";
            }
            else if (strcountry == "Y")
            {
                c = "澳大利亚";
            }
            else if (strcountry == "Z")
            {
                c = "中华人民共和国";
            }
            else
            {
                c = "其他：国家代码" + strflag;
            }
            return c;
        }

        //起飞机场、目的机场_airport 函数
        public static string GetAirport(string strflag)
        {
            string c = "其他";
            if (Dic_AirPort.ContainsKey(strflag))
            {
                c = Dic_AirPort[strflag];
            }
            //else
            //{
            //    c = strflag;
            //}
            return c;
        }

        /// <summary>
        /// ：国家名称  country
        /// </summary>
        private static Dictionary<string, string> _Dic_Country = null;

        public static Dictionary<string, string> Dic_Country
        {
            get
            {
                if (_Dic_Country == null)
                {
                    _Dic_Country = new Dictionary<string, string>();
                    _Dic_Country.Add("AY", "巴布亚新几内亚");
                    _Dic_Country.Add("BG", "格陵兰");
                    _Dic_Country.Add("BI", "冰岛");
                    _Dic_Country.Add("DA", "阿尔及利亚");
                    _Dic_Country.Add("DB", "贝宁");
                    _Dic_Country.Add("DF", "布基纳法索");
                    _Dic_Country.Add("DG", "加纳");
                    _Dic_Country.Add("DI", "科特迪瓦");
                    _Dic_Country.Add("DN", "尼日利亚");
                    _Dic_Country.Add("DR", "尼日尔");

                    _Dic_Country.Add("DT", "突尼斯");
                    _Dic_Country.Add("DX", "多哥");
                    _Dic_Country.Add("EB", "比利时");
                    _Dic_Country.Add("ED", "德国（民航）");
                    _Dic_Country.Add("EE", "爱沙尼亚");
                    _Dic_Country.Add("EF", "芬兰");
                    _Dic_Country.Add("EG", "英国");
                    _Dic_Country.Add("EH", "荷兰");
                    _Dic_Country.Add("EI", "爱尔兰");
                    _Dic_Country.Add("EK", "丹麦");

                    _Dic_Country.Add("EL", "卢森堡");
                    _Dic_Country.Add("EN", "挪威");
                    _Dic_Country.Add("EP", "波兰");
                    _Dic_Country.Add("ES", "瑞典");
                    _Dic_Country.Add("ET", "德国 （军用）");
                    _Dic_Country.Add("EV", "拉脱维亚");
                    _Dic_Country.Add("EY", "立陶宛");
                    _Dic_Country.Add("FA", "南非");
                    _Dic_Country.Add("FB", "博茨瓦纳");
                    _Dic_Country.Add("FC", "刚果共和国");

                    _Dic_Country.Add("FD", "斯威士兰");
                    _Dic_Country.Add("FE", "中非共和国");
                    _Dic_Country.Add("FG", "赤道几内亚");
                    _Dic_Country.Add("FH", "阿森松岛");
                    _Dic_Country.Add("FI", "毛里求斯");
                    _Dic_Country.Add("FJ", "英属印度洋领地");
                    _Dic_Country.Add("FK", "喀麦隆");
                    _Dic_Country.Add("FL", "赞比亚");
                    _Dic_Country.Add("FM", "科摩罗、马达加斯加、马约特、留尼汪");
                    _Dic_Country.Add("FN", "安哥拉");

                    _Dic_Country.Add("FO", "加蓬");
                    _Dic_Country.Add("FP", "圣多美和普林西比");
                    _Dic_Country.Add("FQ", "莫桑比克");
                    _Dic_Country.Add("FS", "塞舌尔");
                    _Dic_Country.Add("FT", "乍得");
                    _Dic_Country.Add("FV", "津巴布韦");
                    _Dic_Country.Add("FW", "马拉维");
                    _Dic_Country.Add("FX", "莱索托");
                    _Dic_Country.Add("FY", "纳米比亚");
                    _Dic_Country.Add("FZ", "刚果民主共和国");

                    _Dic_Country.Add("GA", "马里");
                    _Dic_Country.Add("GB", "冈比亚");
                    _Dic_Country.Add("GC", "加那利群岛");
                    _Dic_Country.Add("GE", "休达和梅利利亚");
                    _Dic_Country.Add("GF", "塞拉利昂");
                    _Dic_Country.Add("GG", "几内亚比绍");
                    _Dic_Country.Add("GL", "利比里亚");
                    _Dic_Country.Add("GM", "摩洛哥");
                    _Dic_Country.Add("GO", "塞内加尔");
                    _Dic_Country.Add("GQ", "毛里求斯");

                    _Dic_Country.Add("GS", "西撒哈拉");
                    _Dic_Country.Add("GU", "几内亚");
                    _Dic_Country.Add("GV", "佛得角");
                    _Dic_Country.Add("HA", "埃塞俄比亚");
                    _Dic_Country.Add("HB", "布隆迪");
                    _Dic_Country.Add("HC", "索马里");
                    _Dic_Country.Add("HD", "吉布提");
                    _Dic_Country.Add("HE", "埃及");
                    _Dic_Country.Add("HF", "吉布提");
                    _Dic_Country.Add("HH", "厄立特里亚");

                    _Dic_Country.Add("HK", "肯尼亚");
                    _Dic_Country.Add("HL", "利比亚");
                    _Dic_Country.Add("HR", "卢旺达");
                    _Dic_Country.Add("HS", "苏丹");
                    _Dic_Country.Add("HT", "坦桑尼亚");
                    _Dic_Country.Add("HU", "乌干达");
                    _Dic_Country.Add("LA", "阿尔巴尼亚");
                    _Dic_Country.Add("LB", "保加利亚");
                    _Dic_Country.Add("LC", "塞浦路斯");
                    _Dic_Country.Add("LD", "克罗地亚");

                    _Dic_Country.Add("LE", "西班牙");
                    _Dic_Country.Add("LF", "法国（包括圣皮埃尔和密克隆）");
                    _Dic_Country.Add("LG", "希腊");
                    _Dic_Country.Add("LH", "匈牙利");
                    _Dic_Country.Add("LI", "意大利");
                    _Dic_Country.Add("LJ", "斯洛文尼亚");
                    _Dic_Country.Add("LK", "捷克共和国");
                    _Dic_Country.Add("LL", "以色列");
                    _Dic_Country.Add("LM", "马耳他");
                    _Dic_Country.Add("LN", "摩纳哥");

                    _Dic_Country.Add("LO", "奥地利");
                    _Dic_Country.Add("LP", "葡萄牙（包括亚速尔群岛）");
                    _Dic_Country.Add("LQ", "波斯尼亚和黑塞哥维那");
                    _Dic_Country.Add("LR", "罗马尼亚");
                    _Dic_Country.Add("LS", "瑞士");
                    _Dic_Country.Add("LT", "土耳其");
                    _Dic_Country.Add("LU", "摩尔多瓦");
                    _Dic_Country.Add("LV", "加沙地带");
                    _Dic_Country.Add("LW", "马其顿");
                    _Dic_Country.Add("LX", "直布罗陀");

                    _Dic_Country.Add("LY", "塞尔维亚和黑山");
                    _Dic_Country.Add("LZ", "斯洛伐克");
                    _Dic_Country.Add("MB", "特克斯和凯科斯群岛");
                    _Dic_Country.Add("MD", "多米尼加");
                    _Dic_Country.Add("MG", "危地马拉");
                    _Dic_Country.Add("MH", "洪都拉斯");
                    _Dic_Country.Add("MK", "牙买加");
                    _Dic_Country.Add("MM", "墨西哥");
                    _Dic_Country.Add("MN", "尼加拉瓜");
                    _Dic_Country.Add("MP", "巴拿马");

                    _Dic_Country.Add("MR", "哥斯达黎加");
                    _Dic_Country.Add("MS", "萨尔瓦多");
                    _Dic_Country.Add("MT", "海地");
                    _Dic_Country.Add("MU", "古巴");
                    _Dic_Country.Add("MW", "开曼群岛");
                    _Dic_Country.Add("MY", "巴哈马");
                    _Dic_Country.Add("MZ", "伯利兹");
                    _Dic_Country.Add("NC", "库克群岛");
                    _Dic_Country.Add("NF", "斐济、汤加");
                    _Dic_Country.Add("NG", "基里巴斯 （吉尔伯特群岛）、图瓦卢");

                    _Dic_Country.Add("NI", "纽埃岛");
                    _Dic_Country.Add("NL", "瓦利斯和富图纳");
                    _Dic_Country.Add("NS", "萨摩亚");
                    _Dic_Country.Add("NT", "法属波利尼西亚");
                    _Dic_Country.Add("NV", "瓦努阿图");
                    _Dic_Country.Add("NW", "新喀里多尼亚");
                    _Dic_Country.Add("NZ", "新西兰");
                    _Dic_Country.Add("OA", "阿富汗");
                    _Dic_Country.Add("OB", "巴林");
                    _Dic_Country.Add("OE", "沙特阿拉伯");

                    _Dic_Country.Add("OI", "伊朗");
                    _Dic_Country.Add("OJ", "约旦和约旦河西岸");
                    _Dic_Country.Add("OK", "科威特");
                    _Dic_Country.Add("OL", "黎巴嫩");
                    _Dic_Country.Add("OM", "阿拉伯联合酋长国");
                    _Dic_Country.Add("OO", "阿曼");
                    _Dic_Country.Add("OP", "巴基斯坦");
                    _Dic_Country.Add("OR", "伊拉克");
                    _Dic_Country.Add("OS", "叙利亚");
                    _Dic_Country.Add("OT", "卡塔尔");

                    _Dic_Country.Add("OY", "也门");
                    _Dic_Country.Add("PA", "阿拉斯加");
                    _Dic_Country.Add("PB", "贝克岛");
                    _Dic_Country.Add("PC", "基里巴斯（菲尼克斯群岛）");
                    _Dic_Country.Add("PF", "育空堡");
                    _Dic_Country.Add("PG", "关岛、北马里亚纳群岛");
                    _Dic_Country.Add("PH", "夏威夷");
                    _Dic_Country.Add("PJ", "约翰斯顿岛");
                    _Dic_Country.Add("PK", "马绍尔群岛");
                    _Dic_Country.Add("PL", "基里巴斯（利恩群岛）");

                    _Dic_Country.Add("PM", "中途岛");
                    _Dic_Country.Add("PO", "阿拉斯加雷达站");
                    _Dic_Country.Add("PP", "波因特莱");
                    _Dic_Country.Add("PT", "密克罗尼西亚联邦、帕劳");
                    _Dic_Country.Add("PW", "威克岛");
                    _Dic_Country.Add("RC", "中国台湾");
                    _Dic_Country.Add("RJ", "日本 （大部份）");
                    _Dic_Country.Add("RK", "韩国");
                    _Dic_Country.Add("RO", "日本 （冲绳县及与论町）");
                    _Dic_Country.Add("RP", "菲律宾");

                    _Dic_Country.Add("SA", "阿根廷");
                    _Dic_Country.Add("SB", "巴西");
                    _Dic_Country.Add("SC", "智利");
                    _Dic_Country.Add("SD", "巴西");
                    _Dic_Country.Add("SE", "厄瓜多尔");
                    _Dic_Country.Add("SF", "福克兰群岛");
                    _Dic_Country.Add("SG", "巴拉圭");
                    _Dic_Country.Add("SK", "哥伦比亚");
                    _Dic_Country.Add("SL", "玻利维亚");
                    _Dic_Country.Add("SM", "苏里南");

                    _Dic_Country.Add("SN", "巴西");
                    _Dic_Country.Add("SO", "法属圭亚那");
                    _Dic_Country.Add("SP", "秘鲁");
                    _Dic_Country.Add("SS", "巴西");
                    _Dic_Country.Add("SU", "乌拉圭");
                    _Dic_Country.Add("SV", "委内瑞拉");
                    _Dic_Country.Add("SW", "巴西");
                    _Dic_Country.Add("SY", "圭亚那");
                    _Dic_Country.Add("TA", "安提瓜和巴布达");
                    _Dic_Country.Add("TB", "巴巴多斯");

                    _Dic_Country.Add("TD", "多米尼加");
                    _Dic_Country.Add("TF", "瓜德罗普");
                    _Dic_Country.Add("TG", "格林纳达");
                    _Dic_Country.Add("TI", "美属维尔京群岛");
                    _Dic_Country.Add("TJ", "波多黎各");
                    _Dic_Country.Add("TK", "圣基茨和尼维斯");
                    _Dic_Country.Add("TL", "圣卢西亚");
                    _Dic_Country.Add("TN", "荷属安的列斯、阿鲁巴");
                    _Dic_Country.Add("TQ", "安圭拉");
                    _Dic_Country.Add("TR", "蒙特塞拉特");

                    _Dic_Country.Add("TT", "特立尼达和多巴哥");
                    _Dic_Country.Add("TU", "英属维尔京群岛");
                    _Dic_Country.Add("TV", "圣文森特和格林纳丁斯");
                    _Dic_Country.Add("TX", "百慕大");
                    _Dic_Country.Add("UA", "哈萨克斯坦斯坦、吉尔吉斯斯坦");
                    _Dic_Country.Add("UB", "阿塞拜疆");
                    _Dic_Country.Add("UG", "亚美尼亚、格鲁吉亚");
                    _Dic_Country.Add("UK", "乌克兰");
                    _Dic_Country.Add("UM", "白俄罗斯");
                    _Dic_Country.Add("UT", "塔吉克斯坦、土库曼斯坦斯坦、乌兹别克斯坦");

                    _Dic_Country.Add("VA", "印度");
                    _Dic_Country.Add("VC", "斯里兰卡");
                    _Dic_Country.Add("VD", "柬埔寨");
                    _Dic_Country.Add("VE", "印度");
                    _Dic_Country.Add("VG", "孟加拉国");
                    _Dic_Country.Add("VH", "香港");
                    _Dic_Country.Add("VI", "印度");
                    _Dic_Country.Add("VL", "老挝");
                    _Dic_Country.Add("VM", "澳门");
                    _Dic_Country.Add("VN", "尼泊尔");

                    _Dic_Country.Add("VO", "印度");
                    _Dic_Country.Add("VQ", "不丹");
                    _Dic_Country.Add("VR", "马尔代夫");
                    _Dic_Country.Add("VT", "泰国");
                    _Dic_Country.Add("VV", "越南");
                    _Dic_Country.Add("VY", "缅甸");
                    _Dic_Country.Add("WA", "印度尼西亚");
                    _Dic_Country.Add("WB", "马来西亚、文莱");
                    _Dic_Country.Add("WI", "印度尼西亚");
                    _Dic_Country.Add("WM", "马来西亚");

                    _Dic_Country.Add("WP", "东帝汶");
                    _Dic_Country.Add("WQ", "印度尼西亚");
                    _Dic_Country.Add("WR", "印度尼西亚");
                    _Dic_Country.Add("WS", "新加坡");
                    _Dic_Country.Add("ZK", "朝鲜");
                    _Dic_Country.Add("ZM", "蒙古");

                }
                return _Dic_Country;
            }
        }

        /// <summary>
        /// ：出发机场、起飞机场  airport
        /// </summary>
        private static Dictionary<string, string> _Dic_AirPort = null;

        public static Dictionary<string, string> Dic_AirPort
        {
            get
            {
                if (_Dic_AirPort == null)
                {
                    _Dic_AirPort = new Dictionary<string, string>();
                    _Dic_AirPort.Add("AGGH", "尼阿拉-索罗门");
                    _Dic_AirPort.Add("APXM", "圣诞岛-澳大利亚");
                    _Dic_AirPort.Add("BGTL", "格林兰岛-格陵兰");
                    _Dic_AirPort.Add("CYMX", "蒙特利尔-加拿大");
                    _Dic_AirPort.Add("CYOW", "渥太华-加拿大");
                    _Dic_AirPort.Add("CYQB", "魁北克-加拿大");
                    _Dic_AirPort.Add("CYUL", "蒙特利尔-加拿大");
                    _Dic_AirPort.Add("CYVR", "温哥华-加拿大");
                    _Dic_AirPort.Add("CYYZ", "多伦多-加拿大");
                    _Dic_AirPort.Add("DAAG", "阿尔及尔-阿尔及利亚");

                    _Dic_AirPort.Add("DIAP", "阿比尚-科特迪瓦");
                    _Dic_AirPort.Add("DRRN", "尼亚美-尼日尔");
                    _Dic_AirPort.Add("DTTA", "突尼斯-突尼斯");
                    _Dic_AirPort.Add("EBBR", "布鲁塞尔-比利时");
                    _Dic_AirPort.Add("EBLG", "列日市-比利时");
                    _Dic_AirPort.Add("EDDF", "法兰克福-德国");
                    _Dic_AirPort.Add("EDDH", "汉堡-德国");
                    _Dic_AirPort.Add("EDDK", "科伦-波昂-德国");
                    _Dic_AirPort.Add("EDDM", "慕尼黑-德国");
                    _Dic_AirPort.Add("EETN", "塔林-爱沙尼亚");

                    _Dic_AirPort.Add("EFHK", "赫尔辛基-芬兰");
                    _Dic_AirPort.Add("EGAC", "贝尔法斯特-北爱尔兰");
                    _Dic_AirPort.Add("EGBB", "伯明翰-英国");
                    _Dic_AirPort.Add("EGCC", "曼彻斯特-英国");
                    _Dic_AirPort.Add("EGPH", "爱丁堡-英国");
                    _Dic_AirPort.Add("EHAM", "阿姆斯特丹-荷兰");
                    _Dic_AirPort.Add("EIDW", "都柏林-爱尔兰");
                    _Dic_AirPort.Add("EKCH", "哥本哈根-丹麦");
                    _Dic_AirPort.Add("ELLX", "卢森堡-卢森堡");
                    _Dic_AirPort.Add("ENGM", "奥斯陆-挪威");

                    _Dic_AirPort.Add("EPWA", "华沙-波兰");
                    _Dic_AirPort.Add("ESSA", "斯德哥尔摩-瑞典");
                    _Dic_AirPort.Add("EVRA", "里加-拉脱维亚共和国");
                    _Dic_AirPort.Add("EYVI", "维尔纽斯-立陶宛");
                    _Dic_AirPort.Add("FAJS", "约翰内斯堡-南非");
                    _Dic_AirPort.Add("FBSK", "嘉柏隆-波札那");
                    _Dic_AirPort.Add("FCBB", "布拉萨-刚果");
                    _Dic_AirPort.Add("FGSL", "马拉波-几内亚");
                    _Dic_AirPort.Add("FIMP", "毛里求斯-毛里求斯");
                    _Dic_AirPort.Add("FKYS", "雅温得-喀麦隆");

                    _Dic_AirPort.Add("FLLS", "卢萨卡-赞比亚");
                    _Dic_AirPort.Add("FMCH", "莫罗尼-科摩洛");
                    _Dic_AirPort.Add("FNLU", "罗安达-安哥拉");
                    _Dic_AirPort.Add("FOOL", "利伯维尔-加蓬");
                    _Dic_AirPort.Add("FQMA", "马普托-莫桑比克");
                    _Dic_AirPort.Add("FSIA", "塞舌尔-塞舌尔群岛");
                    _Dic_AirPort.Add("FWLI", "里朗威-马拉维");
                    _Dic_AirPort.Add("FXMM", "马塞卢-莱索托");
                    _Dic_AirPort.Add("FYWH", "温得和克-纳米比亚");
                    _Dic_AirPort.Add("FZAA", "金沙萨-民主刚果");

                    _Dic_AirPort.Add("GBYD", "班珠尔-冈比亚共和国");
                    _Dic_AirPort.Add("GGOV", "比绍-几内亚比绍");
                    _Dic_AirPort.Add("GLRB", "蒙罗维亚-利比里亚");
                    _Dic_AirPort.Add("GMME", "拉巴特-摩洛哥");
                    _Dic_AirPort.Add("GMMN", "卡萨布兰卡-摩洛哥");
                    _Dic_AirPort.Add("GOOY", "达卡-塞内加尔");
                    _Dic_AirPort.Add("GQNN", "努瓦克肖特毛里塔尼亚");
                    _Dic_AirPort.Add("GUCY", "科纳克里-几内亚");
                    _Dic_AirPort.Add("HAAB", "阿地斯巴贝巴-埃塞");
                    _Dic_AirPort.Add("HBBA", "布琼布拉-布隆迪");

                    _Dic_AirPort.Add("HDAM", "吉布提-吉布提");
                    _Dic_AirPort.Add("HECA", "开罗-埃及");
                    _Dic_AirPort.Add("HKJK", "内罗毕-肯亚");
                    _Dic_AirPort.Add("HRYR", "基加利-卢旺达");
                    _Dic_AirPort.Add("HSSS", "喀土穆-苏丹");
                    _Dic_AirPort.Add("HTDA", "达累斯萨拉姆坦桑尼亚");
                    _Dic_AirPort.Add("HTKJ", "乞力马扎罗-坦桑尼亚");
                    _Dic_AirPort.Add("KABQ", "阿布奎基-新墨西哥州");
                    _Dic_AirPort.Add("KACY", "大西洋城-美国");
                    _Dic_AirPort.Add("KATL", "亚特兰大-乔治亚");

                    _Dic_AirPort.Add("KBFI", "西雅图波音机场-美国");
                    _Dic_AirPort.Add("KBFL", "贝克尔斯菲市-加州");
                    _Dic_AirPort.Add("KBFM", "摩比港市-亚拉巴马");
                    _Dic_AirPort.Add("KBNA", "那什维尔-田纳西");
                    _Dic_AirPort.Add("KBOI", "波依西市-爱达荷");
                    _Dic_AirPort.Add("KBOS", "波士顿-美国");
                    _Dic_AirPort.Add("KBWI", "巴尔的摩港-马里兰");
                    _Dic_AirPort.Add("KCMH", "哥伦布市-俄亥俄");
                    _Dic_AirPort.Add("KCVG", "辛辛那提市-俄亥俄");
                    _Dic_AirPort.Add("KDAB", "德通海滩市-佛罗里达");

                    _Dic_AirPort.Add("KDAY", "德通市-俄亥俄");
                    _Dic_AirPort.Add("KDEN", "丹佛-美国");
                    _Dic_AirPort.Add("KDTW", "底特律-美国");
                    _Dic_AirPort.Add("KEWR", "纽阿克-德拉瓦");
                    _Dic_AirPort.Add("KFAT", "夫勒斯诺-加州");
                    _Dic_AirPort.Add("KGSO", "格林斯堡-北卡罗莱纳");
                    _Dic_AirPort.Add("KIAD", "华盛顿杜勒斯国际机场");
                    _Dic_AirPort.Add("KIAH", "休斯顿-德州");
                    _Dic_AirPort.Add("KIND", "印第安纳波里印第安纳");
                    _Dic_AirPort.Add("KJAX", "杰克逊维-佛罗里达");

                    _Dic_AirPort.Add("KJFK", "纽约肯尼迪机场-美国");
                    _Dic_AirPort.Add("KLAS", "拉斯韦加斯-内华达");
                    _Dic_AirPort.Add("KLAX", "洛衫矶-加州");
                    _Dic_AirPort.Add("KLBB", "乐波市-德州");
                    _Dic_AirPort.Add("KLRF", "小岩城-阿肯色");
                    _Dic_AirPort.Add("KMCI", "堪萨斯城-堪萨斯");
                    _Dic_AirPort.Add("KMCO", "奥兰多市-佛罗里达");
                    _Dic_AirPort.Add("KMDT", "哈利斯堡-宾夕法尼亚");
                    _Dic_AirPort.Add("KMEM", "孟菲斯市-田纳西");
                    _Dic_AirPort.Add("KMIA", "迈阿密-佛罗里达");

                    _Dic_AirPort.Add("KMKE", "密尔瓦基-威斯康星");
                    _Dic_AirPort.Add("KMSP", "明尼阿波里斯明尼苏达");
                    _Dic_AirPort.Add("KMWH", "摩西湖-美国");
                    _Dic_AirPort.Add("KNID", "中国湖城-美国");
                    _Dic_AirPort.Add("KOAK", "奥克兰-加州");
                    _Dic_AirPort.Add("KOMA", "阿马哈市-内布拉斯加");
                    _Dic_AirPort.Add("KORD", "芝加哥欧荷机场-美国");
                    _Dic_AirPort.Add("KPDX", "波特兰-美国");
                    _Dic_AirPort.Add("KPHL", "费城-美国");
                    _Dic_AirPort.Add("KPIT", "匹兹堡-美国");

                    _Dic_AirPort.Add("KPSM", "波特兰-美国");
                    _Dic_AirPort.Add("KRDU", "洛利-北卡罗莱纳");
                    _Dic_AirPort.Add("KRNO", "雷诺-美国");
                    _Dic_AirPort.Add("KSAN", "圣地亚哥-美国");
                    _Dic_AirPort.Add("KSAV", "沙凡那港市-乔治亚");
                    _Dic_AirPort.Add("KSDF", "路易维耳市-肯德基");
                    _Dic_AirPort.Add("KSFO", "旧金山-美国");
                    _Dic_AirPort.Add("KSJC", "圣荷西-加州");
                    _Dic_AirPort.Add("KSLC", "盐湖城");
                    _Dic_AirPort.Add("KSYR", "西拉鸠斯市-纽约");

                    _Dic_AirPort.Add("KTPA", "坦帕市-佛罗里达");
                    _Dic_AirPort.Add("KTUL", "突沙市-俄克拉何马");
                    _Dic_AirPort.Add("KTUS", "土桑市-亚利桑那");
                    _Dic_AirPort.Add("LATI", "地拉那-阿尔巴尼亚");
                    _Dic_AirPort.Add("LBSF", "索非亚-保加利亚");
                    _Dic_AirPort.Add("LDPL", "普拉-克罗地亚共和国");
                    _Dic_AirPort.Add("LEBL", "巴塞罗那-西班牙");
                    _Dic_AirPort.Add("LEMD", "马德里-西班牙");
                    _Dic_AirPort.Add("LEST", "圣地亚哥-西班牙");
                    _Dic_AirPort.Add("LFML", "马赛-法国");

                    _Dic_AirPort.Add("LFPG", "巴黎戴高乐-法国");
                    _Dic_AirPort.Add("LGAT", "雅典-希腊");
                    _Dic_AirPort.Add("LHBP", "布达佩斯-匈牙利");
                    _Dic_AirPort.Add("LIEE", "卡拉里-意大利");
                    _Dic_AirPort.Add("LIMC", "米兰-意大利");
                    _Dic_AirPort.Add("LIPZ", "威尼斯-意大利");
                    _Dic_AirPort.Add("LIRP", "比萨-意大利");
                    _Dic_AirPort.Add("LJLJ", "卢布尔雅那斯诺维尼亚");
                    _Dic_AirPort.Add("LKPR", "布拉格-捷克");
                    _Dic_AirPort.Add("LLBG", "特拉维夫-以色列");

                    _Dic_AirPort.Add("LMML", "马耳他-马耳他");
                    _Dic_AirPort.Add("LOWW", "维也纳-奥地利");
                    _Dic_AirPort.Add("LPPT", "里斯本-葡萄牙");
                    _Dic_AirPort.Add("LQSA", "萨拉热窝-波黑");
                    _Dic_AirPort.Add("LROP", "布加勒斯特-罗马尼亚");
                    _Dic_AirPort.Add("LSGG", "日内瓦-瑞士");
                    _Dic_AirPort.Add("LSZH", "苏黎世-瑞士");
                    _Dic_AirPort.Add("LTAC", "安卡拉-土耳其");
                    _Dic_AirPort.Add("LTBA", "伊斯坦布尔-土耳其");
                    _Dic_AirPort.Add("LXGB", "直布罗陀-直布罗陀");

                    _Dic_AirPort.Add("LYBE", "贝尔格莱德-南斯拉夫");
                    _Dic_AirPort.Add("LZIB", "布拉迪斯拉发-捷克");
                    _Dic_AirPort.Add("MDSD", "圣多明各-多米尼加");
                    _Dic_AirPort.Add("MGGT", "危地马拉-危地马拉");
                    _Dic_AirPort.Add("MKJP", "金斯敦-牙买加");
                    _Dic_AirPort.Add("MMAA", "阿卡波可-墨西哥");
                    _Dic_AirPort.Add("MMMX", "墨西哥城-墨西哥");
                    _Dic_AirPort.Add("MPTO", "巴拿马-巴拿马");
                    _Dic_AirPort.Add("MROC", "圣荷西-哥斯达黎加");
                    _Dic_AirPort.Add("MSLP", "圣萨尔瓦多-萨尔瓦多");

                    _Dic_AirPort.Add("MUHA", "哈瓦那-古巴");
                    _Dic_AirPort.Add("MYNN", "拿骚-巴哈马群岛");
                    _Dic_AirPort.Add("NFTF", "努库阿洛法-汤加");
                    _Dic_AirPort.Add("NGFU", "富那富提-吐瓦鲁");
                    _Dic_AirPort.Add("NSFA", "阿皮亚-西萨摩亚");
                    _Dic_AirPort.Add("NSTU", "巴哥巴哥-萨摩亚群岛");
                    _Dic_AirPort.Add("NTAA", "塔希提");
                    _Dic_AirPort.Add("NZAA", "奥克兰-新西兰");
                    _Dic_AirPort.Add("NZCH", "基督城-新西兰");
                    _Dic_AirPort.Add("NZWN", "惠灵顿-新西兰");

                    _Dic_AirPort.Add("OAKB", "喀布尔-阿富汗");
                    _Dic_AirPort.Add("OBBI", "巴林-巴林");
                    _Dic_AirPort.Add("OERK", "利雅得-沙特阿拉伯");
                    _Dic_AirPort.Add("OIII", "德黑兰-伊朗");
                    _Dic_AirPort.Add("OJAM", "安曼-约旦");
                    _Dic_AirPort.Add("OKBK", "科威特-科威特");
                    _Dic_AirPort.Add("OLBA", "贝鲁特-黎巴嫩");
                    _Dic_AirPort.Add("OMAA", "阿布扎比-阿联酋");
                    _Dic_AirPort.Add("OOMS", "马斯喀特-阿曼");
                    _Dic_AirPort.Add("OPKC", "卡拉奇-巴基斯坦");

                    _Dic_AirPort.Add("OPLA", "拉合尔-巴基斯坦");
                    _Dic_AirPort.Add("ORBS", "巴格达-伊拉克");
                    _Dic_AirPort.Add("OSDI", "大马士革-叙利亚");
                    _Dic_AirPort.Add("OTBD", "多哈-卡塔尔");
                    _Dic_AirPort.Add("PANC", "安克拉治-阿拉斯加");
                    _Dic_AirPort.Add("PGSN", "塞班");
                    _Dic_AirPort.Add("PGUM", "阿加纳-关岛");
                    _Dic_AirPort.Add("PHNL", "檀香山-夏威夷州");
                    _Dic_AirPort.Add("PTYA", "雅蒲岛-密克罗西尼亚");
                    _Dic_AirPort.Add("RJSS", "仙台-日本");

                    _Dic_AirPort.Add("RJTT", "东京 羽田-日本");
                    _Dic_AirPort.Add("RJAA", "东京 成田-日本");
                    _Dic_AirPort.Add("RJNN", "名古屋-日本");
                    _Dic_AirPort.Add("RJOO", "大阪 伊丹-日本");
                    _Dic_AirPort.Add("RJBB", "大阪 关西-日本");
                    _Dic_AirPort.Add("RJFF", "福冈-日本");
                    _Dic_AirPort.Add("ROAH", "冲绳 那霸-日本");
                    _Dic_AirPort.Add("RJCO", "札幌-日本");
                    _Dic_AirPort.Add("RJCM", "女满别-日本");
                    _Dic_AirPort.Add("RJCB", "带么-日本");

                    _Dic_AirPort.Add("RJCH", "函馆-日本");
                    _Dic_AirPort.Add("RJSK", "秋田-日本");
                    _Dic_AirPort.Add("RJNK", "小松 金泽-日本");
                    _Dic_AirPort.Add("RJOA", "么岛-日本");
                    _Dic_AirPort.Add("RJOM", "松山-日本");
                    _Dic_AirPort.Add("RJOK", "高知-日本");
                    _Dic_AirPort.Add("RJFU", "长崎-日本");
                    _Dic_AirPort.Add("RJFT", "熊本-日本");
                    _Dic_AirPort.Add("RJFO", "大分-日本");
                    _Dic_AirPort.Add("RJFM", "宫崎-日本");

                    _Dic_AirPort.Add("RJFK", "鹿儿岛-日本");
                    _Dic_AirPort.Add("RJSC", "山形-日本");
                    _Dic_AirPort.Add("RJBH", "么岛西-日本");
                    _Dic_AirPort.Add("RJOB", "冈山-日本");
                    _Dic_AirPort.Add("RKSS", "汉城-韩国");
                    _Dic_AirPort.Add("RPLL", "马尼拉-菲律宾");
                    _Dic_AirPort.Add("SACO", "科多瓦-阿根廷");
                    _Dic_AirPort.Add("SAEZ", "布宜诺斯艾利斯阿根廷");
                    _Dic_AirPort.Add("SCEL", "圣地亚哥-智利");
                    _Dic_AirPort.Add("SEQU", "基多-厄瓜多尔");

                    _Dic_AirPort.Add("SGAS", "巴拉圭");
                    _Dic_AirPort.Add("SLSU", "苏克雷-玻利维亚");
                    _Dic_AirPort.Add("SOCA", "卡宴-法属圭亚那");
                    _Dic_AirPort.Add("SPHO", "秘鲁");
                    _Dic_AirPort.Add("SPIM", "利马-秘鲁");
                    _Dic_AirPort.Add("SUMU", "蒙得维的亚-乌拉圭");
                    _Dic_AirPort.Add("TBPB", "巴贝多-西印度群岛东");
                    _Dic_AirPort.Add("TGPY", "格林纳达");
                    _Dic_AirPort.Add("TJSJ", "圣胡安-波多黎各首都");
                    _Dic_AirPort.Add("TLPL", "圣卢西亚");

                    _Dic_AirPort.Add("TNCC", "大小安地列斯群岛");
                    _Dic_AirPort.Add("TTPP", "千里达");
                    _Dic_AirPort.Add("TXKF", "百慕大国际机场");
                    _Dic_AirPort.Add("UAAA", "阿马阿塔-哈萨克斯坦");
                    _Dic_AirPort.Add("UBBB", "巴库-阿塞拜疆");
                    _Dic_AirPort.Add("UGGG", "第比利斯-格鲁吉亚");
                    _Dic_AirPort.Add("UHHH", "伯力市-俄罗斯");
                    _Dic_AirPort.Add("UHWW", "海参威-俄罗斯");
                    _Dic_AirPort.Add("UIAA", "赤塔市-俄罗斯");
                    _Dic_AirPort.Add("UKBB", "基辅-乌克兰");

                    _Dic_AirPort.Add("UKOO", "敖德萨-乌克兰");
                    _Dic_AirPort.Add("URMO", "海参威-俄罗斯");
                    _Dic_AirPort.Add("UTAA", "阿什喀巴德土库曼斯坦");
                    _Dic_AirPort.Add("UTTT", "塔什干-乌兹别克斯坦");
                    _Dic_AirPort.Add("UUDD", "莫斯科-俄罗斯");
                    _Dic_AirPort.Add("UUEE", "莫斯科-俄罗斯");
                    _Dic_AirPort.Add("UUWW", "莫斯科-俄罗斯");
                    _Dic_AirPort.Add("VABB", "孟买-印度");
                    _Dic_AirPort.Add("VCBI", "科伦坡-斯里兰卡");
                    _Dic_AirPort.Add("VDPP", "金边-柬埔寨");

                    _Dic_AirPort.Add("VECC", "加尔各达-印度");
                    _Dic_AirPort.Add("VGEG", "吉大港-孟加拉国");
                    _Dic_AirPort.Add("VLVT", "永珍-老挝");
                    _Dic_AirPort.Add("VTBD", "曼谷-泰国");
                    _Dic_AirPort.Add("VTCC", "清迈-泰国");
                    _Dic_AirPort.Add("VTCT", "清菜-泰国");
                    _Dic_AirPort.Add("VVDN", "砚港-越南");
                    _Dic_AirPort.Add("VVNB", "河内-越南");
                    _Dic_AirPort.Add("VYYY", "仰光-缅甸");
                    _Dic_AirPort.Add("WIIH", "雅加达-印度尼西亚");

                    _Dic_AirPort.Add("WIII", "雅加达-印度尼西亚");
                    _Dic_AirPort.Add("WMKK", "吉隆坡-马来西亚");
                    _Dic_AirPort.Add("WRSJ", "泗水-印度尼西亚");
                    _Dic_AirPort.Add("WSSS", "新加坡樟宜-新加坡");
                    _Dic_AirPort.Add("YBBN", "布里斯本-澳大利亚");
                    _Dic_AirPort.Add("YMML", "墨尔本-澳大利亚");
                    _Dic_AirPort.Add("YSCB", "堪培拉-澳大利亚");
                    _Dic_AirPort.Add("YSSY", "悉尼-澳大利亚");
                    _Dic_AirPort.Add("ZKPY", "平壤-朝鲜");
                    _Dic_AirPort.Add("ZMUB", "乌兰巴托-蒙古");

                    _Dic_AirPort.Add("ZBAA", "北京/首都-中国");
                    _Dic_AirPort.Add("ZBBB", "北京-中国");
                    _Dic_AirPort.Add("ZBLX", "北京/良乡-中国");
                    _Dic_AirPort.Add("ZBNY", "北京/南苑-中国");
                    _Dic_AirPort.Add("ZBCF", "赤峰-中国");
                    _Dic_AirPort.Add("ZBCZ", "长治-中国");
                    _Dic_AirPort.Add("ZBDS", "东胜-中国");
                    _Dic_AirPort.Add("ZBDT", "大同-中国");
                    _Dic_AirPort.Add("ZBHH", "呼和浩特-中国");
                    _Dic_AirPort.Add("ZBLA", "海拉尔-中国");

                    _Dic_AirPort.Add("ZBOW", "包头-中国");
                    _Dic_AirPort.Add("ZBPE", "北京/区域管制中心");
                    _Dic_AirPort.Add("ZBSH", "秦皇岛-中国");
                    _Dic_AirPort.Add("ZBSJ", "石家庄-中国");
                    _Dic_AirPort.Add("ZBTJ", "天津/滨海-中国");
                    _Dic_AirPort.Add("ZBTL", "通辽-中国");
                    _Dic_AirPort.Add("ZBTX", "邢台-中国");
                    _Dic_AirPort.Add("ZBUL", "乌兰浩特-中国");
                    _Dic_AirPort.Add("ZBXH", "锡林浩特-中国");
                    _Dic_AirPort.Add("ZBYN", "太原-中国");

                    _Dic_AirPort.Add("ZGBH", "北海-中国");
                    _Dic_AirPort.Add("ZGHA", "长沙-中国");
                    _Dic_AirPort.Add("ZGDY", "张家界-中国");
                    _Dic_AirPort.Add("ZGFS", "佛山-中国");
                    _Dic_AirPort.Add("ZGGG", "广州/白云-中国");
                    _Dic_AirPort.Add("ZJHK", "海口/美兰-中国");
                    _Dic_AirPort.Add("ZGHY", "衡阳-中国");
                    _Dic_AirPort.Add("ZGHZ", "惠州-中国");
                    _Dic_AirPort.Add("ZGKL", "桂林-中国");
                    _Dic_AirPort.Add("ZGMX", "梅县-中国");

                    _Dic_AirPort.Add("ZGNN", "南宁-中国");
                    _Dic_AirPort.Add("ZGOW", "汕头-中国");
                    _Dic_AirPort.Add("ZGSD", "常德-中国");
                    _Dic_AirPort.Add("ZGSY", "三亚-中国");
                    _Dic_AirPort.Add("ZGSZ", "深圳-中国");
                    _Dic_AirPort.Add("ZGUA", "广州-中国");
                    _Dic_AirPort.Add("ZGUH", "珠海-中国");
                    _Dic_AirPort.Add("ZGWZ", "梧州-中国");
                    _Dic_AirPort.Add("ZGZH", "柳州-中国");
                    _Dic_AirPort.Add("ZGZJ", "湛江-中国");

                    _Dic_AirPort.Add("ZGZU", "广州/区域管制中心");
                    _Dic_AirPort.Add("ZHAY", "安阳-中国");
                    _Dic_AirPort.Add("ZHCC", "郑州-中国");
                    _Dic_AirPort.Add("ZHES", "恩施-中国");
                    _Dic_AirPort.Add("ZHGH", "老河口-中国");
                    _Dic_AirPort.Add("ZHHH", "武汉/天河-中国");
                    _Dic_AirPort.Add("ZHLY", "洛阳-中国");
                    _Dic_AirPort.Add("ZHNY", "南阳-中国");
                    _Dic_AirPort.Add("ZHSS", "沙市-中国");
                    _Dic_AirPort.Add("ZHWH", "武汉/区域管制中心");

                    _Dic_AirPort.Add("ZHXF", "襄樊-中国");
                    _Dic_AirPort.Add("ZHYC", "宜昌-中国");
                    _Dic_AirPort.Add("ZLAN", "兰州-中国");
                    _Dic_AirPort.Add("ZLDH", "敦煌-中国");
                    _Dic_AirPort.Add("ZLGN", "格尔木-中国");
                    _Dic_AirPort.Add("ZLHW", "兰州/区域管制中心");
                    _Dic_AirPort.Add("HANZHONG", "汉中-中国");
                    _Dic_AirPort.Add("ZLIC", "银川-中国");
                    _Dic_AirPort.Add("ZLJQ", "嘉峪关-中国");
                    _Dic_AirPort.Add("ZLLL", "兰州-中国");

                    _Dic_AirPort.Add("ZLQY", "庆阳-中国");
                    _Dic_AirPort.Add("ZLSN", "西安-中国");
                    _Dic_AirPort.Add("ZLXN", "西宁-中国");
                    _Dic_AirPort.Add("ZLXY", "西安-中国");
                    _Dic_AirPort.Add("ZLYA", "延安-中国");
                    _Dic_AirPort.Add("ZLYL", "榆林-中国");
                    _Dic_AirPort.Add("ZPBS", "保山-中国");
                    _Dic_AirPort.Add("ZPDL", "大理-中国");
                    _Dic_AirPort.Add("ZPDQ", "迪庆香格里拉-中国");
                    _Dic_AirPort.Add("ZPJH", "西双版纳-中国");
                    _Dic_AirPort.Add("ZPKM", "昆明/区域管制中心");

                    _Dic_AirPort.Add("ZPLJ", "丽江-中国");
                    _Dic_AirPort.Add("ZPMS", "芒市-中国");
                    _Dic_AirPort.Add("ZPPP", "昆明-中国");
                    _Dic_AirPort.Add("ZPSM", "思茅-中国");
                    _Dic_AirPort.Add("ZPTC", "驼峰-中国");
                    _Dic_AirPort.Add("ZPYM", "元谋-中国");
                    _Dic_AirPort.Add("ZPZT", "昭通-中国");
                    _Dic_AirPort.Add("ZSAM", "厦门-中国");
                    _Dic_AirPort.Add("ZSAQ", "安庆-中国");
                    _Dic_AirPort.Add("ZSBB", "蚌埠-中国");
                    _Dic_AirPort.Add("ZSCG", "常州-中国");

                    _Dic_AirPort.Add("ZSCN", "南昌-中国");
                    _Dic_AirPort.Add("ZSFY", "阜阳-中国");
                    _Dic_AirPort.Add("ZSFZ", "福州-中国");
                    _Dic_AirPort.Add("ZSGZ", "赣州-中国");
                    _Dic_AirPort.Add("ZSHA", "上海/区域管制中心");
                    _Dic_AirPort.Add("ZSHC", "杭州-中国");
                    _Dic_AirPort.Add("ZSJA", "吉安-中国");
                    _Dic_AirPort.Add("ZSJD", "景德镇-中国");
                    _Dic_AirPort.Add("ZSJG", "济宁-中国");
                    _Dic_AirPort.Add("ZSJJ", "九江-中国");

                    _Dic_AirPort.Add("ZSJN", "济南-中国");
                    _Dic_AirPort.Add("ZSJU", "衢州-中国");
                    _Dic_AirPort.Add("ZSLG", "连云港-中国");
                    _Dic_AirPort.Add("ZSLQ", "黄岩-中国");
                    _Dic_AirPort.Add("ZSLS", "庐山-中国");
                    _Dic_AirPort.Add("ZSLY", "临沂-中国");
                    _Dic_AirPort.Add("ZSNB", "宁波-中国");
                    _Dic_AirPort.Add("ZSNJ", "南京-中国");
                    _Dic_AirPort.Add("ZSNT", "南通-中国");
                    _Dic_AirPort.Add("ZSOF", "合肥-中国");

                    _Dic_AirPort.Add("ZSPD", "上海/浦东-中国");
                    _Dic_AirPort.Add("ZSQD", "青岛-中国");
                    _Dic_AirPort.Add("ZSQZ", "泉州-中国");
                    _Dic_AirPort.Add("ZSRG", "如皋-中国");
                    _Dic_AirPort.Add("ZSSA", "上海-中国");
                    _Dic_AirPort.Add("ZSSL", "上海/龙华-中国");
                    _Dic_AirPort.Add("ZSSS", "上海/虹桥-中国");
                    _Dic_AirPort.Add("ZSSZ", "苏州-中国");
                    _Dic_AirPort.Add("ZSTX", "黄山-中国");
                    _Dic_AirPort.Add("ZSWF", "潍纺-中国");

                    _Dic_AirPort.Add("ZSWH", "威海-中国");
                    _Dic_AirPort.Add("ZSWU", "芜湖-中国");
                    _Dic_AirPort.Add("ZSWX", "无锡-中国");
                    _Dic_AirPort.Add("ZSWY", "武夷山-中国");
                    _Dic_AirPort.Add("ZSWZ", "温州-中国");
                    _Dic_AirPort.Add("ZSXZ", "徐州-中国");
                    _Dic_AirPort.Add("ZSYA", "扬州-中国");
                    _Dic_AirPort.Add("ZSYN", "盐城-中国");
                    _Dic_AirPort.Add("ZSYT", "烟台-中国");
                    _Dic_AirPort.Add("ZSYW", "义乌-中国");

                    _Dic_AirPort.Add("ZUBD", "昌都-中国");
                    _Dic_AirPort.Add("ZUCK", "重庆-中国");
                    _Dic_AirPort.Add("ZUDX", "达县-中国");
                    _Dic_AirPort.Add("ZUDZ", "大足-中国");
                    _Dic_AirPort.Add("ZUGH", "广汉-中国");
                    _Dic_AirPort.Add("ZUGY", "贵阳-中国");
                    _Dic_AirPort.Add("ZUHE", "黑河-中国");
                    _Dic_AirPort.Add("ZULS", "拉萨-中国");
                    _Dic_AirPort.Add("ZULZ", "泸州-中国");
                    _Dic_AirPort.Add("ZUNC", "南充-中国");

                    _Dic_AirPort.Add("ZUTR", "铜仁-中国");
                    _Dic_AirPort.Add("ZUUU", "成都-中国");
                    _Dic_AirPort.Add("ZUWX", "万县-中国");
                    _Dic_AirPort.Add("ZUXC", "西昌-中国");
                    _Dic_AirPort.Add("ZUYB", "宜宾-中国");
                    _Dic_AirPort.Add("ZUZH", "攀枝花保安营-中国");
                    _Dic_AirPort.Add("ZUZY", "遵义-中国");
                    _Dic_AirPort.Add("ZWAK", "阿克苏-中国");
                    _Dic_AirPort.Add("ZWAT", "阿尔泰-中国");
                    _Dic_AirPort.Add("ZWCN", "且末-中国");
                    _Dic_AirPort.Add("ZWFY", "富蕴-中国");

                    _Dic_AirPort.Add("ZWHM", "哈密-中国");
                    _Dic_AirPort.Add("ZWKC", "库车-中国");
                    _Dic_AirPort.Add("ZWKL", "库尔勒-中国");
                    _Dic_AirPort.Add("ZWKM", "克拉玛依-中国");
                    _Dic_AirPort.Add("ZWSH", "喀什-中国");
                    _Dic_AirPort.Add("ZWSS", "鄯善-中国");
                    _Dic_AirPort.Add("ZWTC", "塔城-中国");
                    _Dic_AirPort.Add("ZWTN", "和田-中国");
                    _Dic_AirPort.Add("ZWUQ", "乌鲁木齐区域管制中心");
                    _Dic_AirPort.Add("ZWWW", "乌鲁木齐-中国");

                    _Dic_AirPort.Add("ZWYN", "伊宁-中国");
                    _Dic_AirPort.Add("ZYAS", "鞍山-中国");
                    _Dic_AirPort.Add("ZYCC", "长春-中国");
                    _Dic_AirPort.Add("ZYCH", "长海-中国");
                    _Dic_AirPort.Add("ZYCY", "朝阳-中国");
                    _Dic_AirPort.Add("ZYDD", "丹东-中国");
                    _Dic_AirPort.Add("ZYHB", "哈尔滨-中国");
                    _Dic_AirPort.Add("ZYJL", "吉林-中国");
                    _Dic_AirPort.Add("ZYJM", "佳木斯-中国");
                    _Dic_AirPort.Add("ZYJZ", "锦州-中国");

                    _Dic_AirPort.Add("ZYMD", "牡丹江-中国");
                    _Dic_AirPort.Add("ZYQQ", "齐齐哈尔-中国");
                    _Dic_AirPort.Add("ZYSH", "沈阳/区域管制中心");
                    _Dic_AirPort.Add("ZYTL", "大连-中国");
                    _Dic_AirPort.Add("ZYTN", "通化-中国");
                    _Dic_AirPort.Add("ZYTX", "沈阳/桃仙-中国");
                    _Dic_AirPort.Add("ZYXC", "兴城-中国");
                    _Dic_AirPort.Add("ZYYJ", "延吉-中国");
                    _Dic_AirPort.Add("ZYYL", "依兰-中国");
                    _Dic_AirPort.Add("ZYYY", "沈阳/东塔-中国");

                    _Dic_AirPort.Add("RCBS", "金门/尚义-中国台湾");
                    _Dic_AirPort.Add("RCGM", "桃园-中国台湾");
                    _Dic_AirPort.Add("RCKH", "高雄-中国台湾");
                    _Dic_AirPort.Add("RCKU", "嘉义-中国台湾");
                    _Dic_AirPort.Add("RCLG", "台中-中国台湾");
                    _Dic_AirPort.Add("RCNN", "台南-中国台湾");
                    _Dic_AirPort.Add("RCQC", "马公-中国台湾");
                    _Dic_AirPort.Add("RCSS", "台北/松山-中国台湾");
                    _Dic_AirPort.Add("RCTP", "台北机场-中国台湾");
                    _Dic_AirPort.Add("VHHH", "香港/国际机场");

                    _Dic_AirPort.Add("VHHK", "香港/区域管制中心");
                    _Dic_AirPort.Add("VMMC", "澳门-中国澳门");
                }
                return _Dic_AirPort;
            }
        }
    }
    

}
