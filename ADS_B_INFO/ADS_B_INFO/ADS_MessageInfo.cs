using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ADS_B_INFO
{
    public class ADS_MessageInfo
    {
        //系统参数
        public int Channel { get; set; }
        public uint ICAO { get; set; }
        public int Type { get; set; }
        //空中位置消息
        public string MonitorState { get; set; }
        public string SingleAntenna { get; set; }
        public int Height { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        //飞机身份
        public string AirPlaneID { get; set; }
        //地表面位置信息
        public string WorkingState { get; set; }
        public string PathStateFlag { get; set; }
        public string PathState { get; set; }
        //空中速度消息(1或2)
        public int SubType { get; set; }
        public string AimChange { get; set; }
        public string IFR { get; set; }
        public double AirDirection { get; set; }
        public string AirSpeedType { get; set; }
        public double AirSpeed { get; set; }
        public string VerticalSpeedSource { get; set; }
        public string VerticalSpeedFlag { get; set; }
        public string VerticalSpeed { get; set; }
        public string AtmosphereFlag { get; set; }
        public string Atmosphere { get; set; }
        //目标状态与状况
        public string AtmosphereDataSource { get; set; }
        public string HeightType { get; set; }
        public string HeightProperty { get; set; }
        public string AtmosphereMode { get; set; }
        public int TargetHeight { get; set; }
        public string LevelDataSource { get; set; }
        public int TargetDirection { get; set; }
        public string TargetDirectionFlag { get; set; }
        public string LevelMode { get; set; }
        public string PropertyCode { get; set; }
        public string EmergencyCode { get; set; }
        //飞机运行状况
        public string CC_CDTI { get; set; }
        public string CC_ARV { get; set; }
        public string CC_TS { get; set; }
        public string CC_TC { get; set; }
        public string CC_POA { get; set; }
        public string CC_B2Low { get; set; }
        public int OM_Type { get; set; }
        public string OM_RA { get; set; }
        public string OM_IDENT { get; set; }
        public string OM_ATC { get; set; }
        public string MOPS { get; set; }
        public string NACp { get; set; }
        public string SIL { get; set; }
        public string NIC_BARO { get; set; }
        public string L_WType { get; set; }
        public string TRK_HDG { get; set; }
        public string HRD { get; set; }




        static List<code_msg> code_list = new List<code_msg>();
        static List<ADS_Static> ADS_Static_Map = new List<ADS_Static>();
        private static int lat_limit = 20;
        private static int lon_limit = 20;

        //经度_longitude 函数
        public static string GetLongitude(double key)
        {
            string strReturn = "";
            if (key < 0 && key >= -180)
            {
                double num = Math.Abs(key);
                strReturn = "W " + num.ToString("f4");
            }
            else if (key > 0 && key <= 180)
            {
                double num = Math.Abs(key);
                strReturn = "E " + num.ToString("f4");
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
                strReturn = "S " + num.ToString("f4");
            }
            else if (key > 0 && key <= 180)
            {
                double num = Math.Abs(key);
                strReturn = "N " + num.ToString("f4");
            }
            else if (key == 0)
            {
                strReturn = "0";
            }

            return strReturn;

        }
        //****************************************ADS_B消息解译函数*****************************************************//
        //获取：空中位置消息(含位置信息)
        public static ADS_MessageInfo GetAirLocation(byte[] ADSData, double lon, double lat, uint GPSTime)
        {
            ADS_MessageInfo info = new ADS_MessageInfo();
            try
            {
                info.ICAO = (uint)((ADSData[0] & 0xFF) << 16 | (ADSData[1] & 0xFF) << 8 | (ADSData[2] & 0xFF));
                info.Type = (int)(ADSData[3]);
                #region 高度
                int Height = 0;
                byte[] btheight = ADSData.Skip(6).Take(2).ToArray();
                if ((btheight[1] & 0x01) == 0x01)
                {
                    byte Height_h = (byte)(btheight[1] >> 1);
                    byte Height_l = (byte)(btheight[0] >> 4);
                    int Height_Hd = (int)Height_h * 16;
                    int Height_ld = Height_l;
                    Height = (Height_Hd + Height_l) * 25 - 1025;
                    info.Height = (int)(Height * 0.3048);
                }
                else
                {
                    int CCode = 0;
                    int Gcode = 0;
                    //height[1] = 0x09;
                    //height[0] = 0x04;
                    Gcode = (int)(((btheight[0] & 0x04) << 5) | ((btheight[0] & 0x01) << 6) | ((btheight[1] & 0x40) >> 1) | (btheight[1] & 0x10) | ((btheight[1] & 0x04) << 1) | ((btheight[1] & 0x02) << 1) | ((btheight[0] & 0x08) >> 2) | ((btheight[0] & 0x02) >> 1));
                    CCode = (int)(((btheight[1] & 0x80) >> 5) | ((btheight[1] & 0x20) >> 4) | ((btheight[1] & 0x08) >> 3));
                    //Gcode = 0x07;
                    //CCode = 0x04; 对应高度396m
                    Gcode ^= Gcode >> 1;
                    Gcode ^= Gcode >> 2;
                    Gcode ^= Gcode >> 4;
                    Gcode ^= Gcode >> 8;
                    Gcode ^= Gcode >> 16;
                    if (Gcode % 2 == 0)
                    {
                        switch (CCode)
                        {
                            case 0x01:
                                CCode = 0;
                                break;
                            case 0x02:
                                CCode = 2;
                                break;
                            case 0x03:
                                CCode = 1;
                                break;
                            case 0x04:
                                CCode = 4;
                                break;
                            case 0x06:
                                CCode = 3;
                                break;

                        }
                    }
                    else
                    {
                        switch (CCode)
                        {
                            case 0x01:
                                CCode = 4;
                                break;
                            case 0x02:
                                CCode = 2;
                                break;
                            case 0x03:
                                CCode = 3;
                                break;
                            case 0x04:
                                CCode = 0;
                                break;
                            case 0x06:
                                CCode = 1;
                                break;
                        }
                    }
                    info.Height = (int)((Gcode * 5 + CCode - 12) * 100 * 0.3048);
                }

                #endregion

                #region  经纬度信息
                Ads_b_msg msg = new Ads_b_msg();
                uint ZY = 0;
                uint ZX = 0;
                byte cpr = 0;
                byte[] zy_c = new byte[4];
                byte[] zx_c = new byte[4];
                byte[] tmp = new byte[4];
                int num = 0;

                code_msg code_rew = new code_msg();
                code_rew.icao = info.ICAO;
                Array.Copy(ADSData, 10, zy_c, 0, 4);
                Array.Copy(ADSData, 14, zx_c, 0, 4);
                ZY = System.BitConverter.ToUInt32(zy_c, 0);
                ZX = System.BitConverter.ToUInt32(zx_c, 0);

                cpr = ADSData[9];
                code_rew.cpr = cpr;
                if (cpr == 0)
                {
                    code_rew.YZ0 = ZY;
                    code_rew.XZ0 = ZX;
                    code_rew.even_time = GPSTime;
                }
                else if (cpr == 1)
                {
                    code_rew.YZ1 = ZY;
                    code_rew.XZ1 = ZX;
                    code_rew.odd_time = GPSTime;
                }

                bool even_odd = list_check_msg(code_rew, out num);
                if (even_odd)//奇偶算法
                {
                    msg.msg_out = msg.calc_odd_even_msg(code_list[num].YZ0, code_list[num].XZ0, code_list[num].YZ1, code_list[num].XZ1);
                    //if (CheckRegion(msg.msg_out.latitude, msg.msg_out.longitude, lat, lon))//未超过经纬度界限
                    {
                        if (info.ICAO != 0)
                        {
                            //info.longitude = GetLongitude(msg.msg_out.longitude);
                            //info.latitude = GetLatitude(msg.msg_out.latitude);
                            info.longitude = msg.msg_out.longitude.ToString("f4");
                            info.latitude = msg.msg_out.latitude.ToString("f4");
                            //for (int i = 0; i < ADS_Static_Map.Count; i++)
                            //{
                            //    if (info.ICAO == ADS_Static_Map[i].ICAO)
                            //    {
                            //        info.AirPlaneID = ADS_Static_Map[i].AirPlaneID;
                            //        return info;
                            //    }
                            //}
                            //态势数据Map赋值
                            //MapSQL.Add_MapADS(info.ICAO, msg.msg_out.longitude, msg.msg_out.latitude);
                        }
                        else
                        {
                            info.ICAO = 0;
                        }
                    }
                }
                else
                {
                    info.ICAO = 0;
                }
                #endregion



            }
            catch (System.Exception ex)
            {
                // ErrorRecord.ProcessError(ex.ToString());
            }
            return info;
        }
        //获取：地表面位置消息(含位置信息)
        public static ADS_MessageInfo GetEarthLocation(byte[] ADSData, double lon, double lat, uint GPSTime)
        {
            ADS_MessageInfo info = new ADS_MessageInfo();
            try
            {
                info.ICAO = (uint)((ADSData[0] & 0xFF) << 16 | (ADSData[1] & 0xFF) << 8 | (ADSData[2] & 0xFF));
                info.Type = (int)(ADSData[3]);

                #region  经纬度信息
                Ads_b_msg msg = new Ads_b_msg();
                uint ZY = 0;
                uint ZX = 0;
                byte cpr = 0;
                byte[] zy_c = new byte[4];
                byte[] zx_c = new byte[4];
                byte[] tmp = new byte[4];
                int num = 0;

                code_msg code_rew = new code_msg();
                code_rew.icao = info.ICAO;
                Array.Copy(ADSData, 9, zy_c, 0, 4);
                Array.Copy(ADSData, 13, zx_c, 0, 4);
                ZY = System.BitConverter.ToUInt32(zy_c, 0);
                ZX = System.BitConverter.ToUInt32(zx_c, 0);

                cpr = ADSData[8];
                code_rew.cpr = cpr;
                if (cpr == 0)
                {
                    code_rew.YZ0 = ZY;
                    code_rew.XZ0 = ZX;
                    code_rew.even_time = GPSTime;
                }
                else if (cpr == 1)
                {
                    code_rew.YZ1 = ZY;
                    code_rew.XZ1 = ZX;
                    code_rew.odd_time = GPSTime;
                }

                bool even_odd = list_check_msg(code_rew, out num);
                if (even_odd)//奇偶算法
                {
                    msg.msg_out = msg.calc_odd_even_msg(code_list[num].YZ0, code_list[num].XZ0, code_list[num].YZ1, code_list[num].XZ1);
                    //if (CheckRegion(msg.msg_out.latitude, msg.msg_out.longitude, lat, lon))//未超过经纬度界限
                    {
                        if (info.ICAO != 0 && info.longitude != null && info.latitude != null)
                        {
                            if (msg.msg_out.longitude > -180 && msg.msg_out.longitude < 180 && msg.msg_out.latitude < 90 && msg.msg_out.latitude > -90)
                            {
                                info.longitude = GetLongitude(msg.msg_out.longitude);
                                info.latitude = GetLatitude(msg.msg_out.latitude);
                                //for (int i = 0; i < ADS_Static_Map.Count; i++)
                                //{
                                //    if (info.ICAO == ADS_Static_Map[i].ICAO)
                                //    {
                                //        info.AirPlaneID = ADS_Static_Map[i].AirPlaneID;
                                //        return info;
                                //    }
                                //}
                                //态势数据Map赋值
                                //MapSQL.Add_MapADS(info.ICAO, msg.msg_out.longitude, msg.msg_out.latitude);
                            }
                            else
                            {
                                info.ICAO = 0;
                            }
                        }
                    }
                }
                else
                {
                    info.ICAO = 0;
                }
                #endregion
            }
            catch (System.Exception ex)
            {
                // ErrorRecord.ProcessError(ex.ToString());
            }
            return info;
        }
        public static int checkAriPlanID(uint icao)
        {
            for (int i = 0; i < ADS_Static_Map.Count; i++)
            {
                if (icao == ADS_Static_Map[i].ICAO)
                {
                    return -1;
                }
            }
            return 0;
        }
        //获取：飞机身份ID  
        public static ADS_MessageInfo GetAirPlaneID(byte[] ADSData)
        {
            ADS_MessageInfo info = new ADS_MessageInfo();
            try
            {
                byte[] ID = new byte[8];
                info.ICAO = (uint)((ADSData[0] & 0xFF) << 16 | (ADSData[1] & 0xFF) << 8 | (ADSData[2] & 0xFF));
                info.Type = (int)(ADSData[3]);
                Array.Copy(ADSData, 5, ID, 0, 8);
                info.AirPlaneID = System.Text.Encoding.ASCII.GetString(ID, 0, 8);
                ADS_Static tmp = new ADS_Static();
                tmp.ICAO = info.ICAO;
                tmp.AirPlaneID = info.AirPlaneID;
                //if (checkAriPlanID(tmp.ICAO) == 0)
                //{
                //    if (ADS_Static_Map.Count > 100)
                //    {
                //        ADS_Static_Map.RemoveAt(0);
                //        ADS_Static_Map.Add(tmp);
                //    }
                //    else
                //    {
                //        ADS_Static_Map.Add(tmp);
                //    }
                //}
                
                //info.AirPlaneID = ADS_DicMessage.GetAirplaneID(info.Type, (int)(ADSData[4]));
            }
            catch (System.Exception ex)
            {
                // ErrorRecord.ProcessError(ex.ToString());
            }
            return info;

        }
        //获取：空中速度消息
        public static ADS_MessageInfo GetAirSpeed(byte[] ADSData)
        {
            ADS_MessageInfo info = new ADS_MessageInfo();
            try
            {
                info.SubType = (int)(ADSData[4]);
                if (info.SubType == 1 || info.SubType == 2)
                {
                    byte west_east_flag = ADSData[8];
                    byte nouth_south_flag = ADSData[11];
                    UInt16 west_east_speed = (UInt16)((ADSData[9] & 0xFF) | (ADSData[10] & 0xFF) << 8);
                    UInt16 nouth_south_speed = (UInt16)((ADSData[12] & 0xFF) | (ADSData[13] & 0xFF) << 8);
                    info.AirDirection = ADS_DicMessage.GetDirectionofSpeed(info.SubType, west_east_flag, nouth_south_flag, west_east_speed, nouth_south_speed);
                    info.AirSpeed = ADS_DicMessage.GetAirSpeed(info.SubType, info.AirDirection, west_east_speed, nouth_south_speed);
                }
                else if (info.SubType == 3 || info.SubType == 4)
                {
                    byte direction_flag = ADSData[14];
                    int speed_flag = (int)ADSData[17];
                    UInt16 direction = (UInt16)((ADSData[15] & 0xFF) | (ADSData[16] & 0xFF) << 8);
                    UInt16 speed = (UInt16)((ADSData[18] & 0xFF) | (ADSData[19] & 0xFF) << 8);

                    info.AirDirection = ADS_DicMessage.GetDirectionFor34(direction_flag, direction);
                    info.AirSpeedType = ADS_DicMessage.GetAirSpeedType(speed_flag);
                    info.AirSpeed = ADS_DicMessage.GetAirSpeedFor34(info.SubType, speed);
                }
                else
                {
                    return info;
                }
                info.ICAO = (uint)((ADSData[0] & 0xFF) << 16 | (ADSData[1] & 0xFF) << 8 | (ADSData[2] & 0xFF));
                info.Type = (int)(ADSData[3]);

                info.AimChange = ADS_DicMessage.GetAimChange((int)(ADSData[5]));
                info.IFR = ADS_DicMessage.GetIFR((int)(ADSData[6]));

                info.VerticalSpeedSource = ADS_DicMessage.GetVerticalSpeedSource((int)(ADSData[20]));
                info.VerticalSpeedFlag = ADS_DicMessage.GetVerticalSpeedFlag((int)(ADSData[21]));
                info.VerticalSpeed = ADS_DicMessage.GetVerticalSpeed((int)((ADSData[22] & 0xFF) | (ADSData[23] & 0xFF) << 8));

                info.AtmosphereFlag = ADS_DicMessage.GetAtmosphereFlag((int)(ADSData[25]));
                info.Atmosphere = ADS_DicMessage.GetAtmosphere((int)(ADSData[26]));

            }
            catch (System.Exception ex)
            {
                // ErrorRecord.ProcessError(ex.ToString());
            }
            return info;

        }
        //获取：目标状态与状况
        public static ADS_MessageInfo GetTargetState(byte[] ADSData)
        {
            ADS_MessageInfo info = new ADS_MessageInfo();
            try
            {
                info.SubType = (int)(ADSData[4]);

                if (info.SubType == 0)
                {
                    info.ICAO = (uint)((ADSData[0] & 0xFF) << 16 | (ADSData[1] & 0xFF) << 8 | (ADSData[2] & 0xFF));
                    info.Type = (int)(ADSData[3]);
                    info.AtmosphereDataSource = ADS_DicMessage.GetAtmosphereDataSource((int)(ADSData[5]));
                    info.HeightType = ADS_DicMessage.GetHeightType((int)(ADSData[6]));
                    info.HeightProperty = ADS_DicMessage.GetHeightProperty((int)ADSData[8]);
                    info.AtmosphereMode = ADS_DicMessage.GetAtmosphereLevelMode((int)ADSData[9]);
                    info.TargetHeight = ADS_DicMessage.GetTargetHeight((UInt16)((ADSData[10] & 0xFF) | (ADSData[11] & 0xFF) << 8));
                    info.LevelDataSource = ADS_DicMessage.GetLevelDataSource((int)ADSData[12]);
                    info.TargetDirection = (int)((ADSData[13] & 0xFF) | (ADSData[14] & 0xFF) << 8);
                    info.TargetDirectionFlag = ADS_DicMessage.GetTargetDirectionFlag((int)ADSData[15]);
                    info.LevelMode = ADS_DicMessage.GetAtmosphereLevelMode((int)ADSData[16]);
                    info.PropertyCode = ADS_DicMessage.GetPropertyCode((int)ADSData[21]);
                    info.EmergencyCode = ADS_DicMessage.GetEmergencyCode((int)ADSData[22]);
                }
            }
            catch (System.Exception ex)
            {
                // ErrorRecord.ProcessError(ex.ToString());
            }
            return info;

        }
        //获取：飞机运行状况
        public static ADS_MessageInfo GetAirCondition(byte[] ADSData)
        {
            ADS_MessageInfo info = new ADS_MessageInfo();
            try
            {
                info.SubType = (int)(ADSData[4]);

                if (info.SubType == 0)
                {
                    info.ICAO = (uint)((ADSData[0] & 0xFF) << 16 | (ADSData[1] & 0xFF) << 8 | (ADSData[2] & 0xFF));
                    info.Type = (int)(ADSData[3]);
                    info.MOPS = ADS_DicMessage.GetMOPS((int)(ADSData[9]));
                    info.CC_CDTI = ADS_DicMessage.GetCC_CDTI((int)((ADSData[6] & 0x10) >> 4));
                    if (ADSData[9] == 0x01)
                    {
                        info.CC_ARV = ADS_DicMessage.GetCC_ARV((int)((ADSData[6] & 0x02) >> 1));
                        info.CC_TS = ADS_DicMessage.GetCC_TS((int)(ADSData[6] & 0x01));
                        info.CC_TC = ADS_DicMessage.GetCC_TC((int)((ADSData[5] & 0xC0) >> 6));
                    }
                    info.OM_Type = (int)((ADSData[8] & 0xC0) >> 6);
                    if (info.OM_Type == 0)
                    {
                        info.OM_RA = ADS_DicMessage.GetOM_RA((int)((ADSData[8] & 0x20) >> 5));
                        info.OM_IDENT = ADS_DicMessage.GetOM_IDENT(((int)(ADSData[8] & 0x10) >> 4));
                        info.OM_ATC = ADS_DicMessage.GetOM_ATC(((int)(ADSData[8] & 0x08) >> 3));
                    }
                    info.NACp = ADS_DicMessage.GetNACp((int)ADSData[11]);
                    info.SIL = ADS_DicMessage.GetSIL((int)ADSData[13]);
                    info.NIC_BARO = ADS_DicMessage.GetNIC_BARO((int)ADSData[14]);
                    info.HRD = ADS_DicMessage.GetHRD((int)ADSData[15]);
                }
                else if (info.SubType == 1)
                {
                    info.ICAO = (uint)((ADSData[0] & 0xFF) << 16 | (ADSData[1] & 0xFF) << 8 | (ADSData[2] & 0xFF));
                    info.Type = (int)(ADSData[3]);
                    info.MOPS = ADS_DicMessage.GetMOPS((int)(ADSData[9]));
                    info.L_WType = ADS_DicMessage.GetL_WType((int)(ADSData[5] & 0x0F));
                    info.CC_CDTI = ADS_DicMessage.GetCC_CDTI((int)((ADSData[6] & 0x10) >> 4));
                    if (ADSData[9] == 0x01)
                    {
                        info.CC_POA = ADS_DicMessage.GetCC_POA((int)((ADSData[6] & 0x20) >> 5));
                        info.CC_B2Low = ADS_DicMessage.GetCC_B2Low((int)((ADSData[6] & 0x02) >> 1));
                    }
                    info.OM_Type = (int)((ADSData[8] & 0xC0) >> 6);
                    if (info.OM_Type == 0)
                    {
                        info.OM_RA = ADS_DicMessage.GetOM_RA((int)((ADSData[8] & 0x20) >> 5));
                        info.OM_IDENT = ADS_DicMessage.GetOM_IDENT(((int)(ADSData[8] & 0x10) >> 4));
                        info.OM_ATC = ADS_DicMessage.GetOM_ATC(((int)(ADSData[8] & 0x08) >> 3));
                    }
                    info.NACp = ADS_DicMessage.GetNACp((int)ADSData[11]);
                    info.SIL = ADS_DicMessage.GetSIL((int)ADSData[13]);
                    info.TRK_HDG = ADS_DicMessage.GetTRK_HDG((int)ADSData[14]);
                    info.HRD = ADS_DicMessage.GetHRD((int)ADSData[15]);
                }
            }
            catch (System.Exception ex)
            {
                // ErrorRecord.ProcessError(ex.ToString());
            }
            return info;

        }
        //获取：扩展间歇震荡飞机状况消息
        public static ADS_MessageInfo GetPauseAirCondition(byte[] ADSData)
        {
            ADS_MessageInfo info = new ADS_MessageInfo();
            try
            {
                info.SubType = (int)(ADSData[4]);

                if (info.SubType == 1)
                {
                    info.ICAO = (uint)((ADSData[0] & 0xFF) << 16 | (ADSData[1] & 0xFF) << 8 | (ADSData[2] & 0xFF));
                    info.Type = (int)(ADSData[3]);
                    info.EmergencyCode = ADS_DicMessage.GetEmergencyCode((int)(ADSData[5]));
                }
            }
            catch (System.Exception ex)
            {
                // ErrorRecord.ProcessError(ex.ToString());
            }
            return info;

        }
        //判断算法类型(本地算法、奇偶算法)
        static bool list_check_msg(code_msg tmp, out int index)
        {
            try
            {
                if (tmp.cpr == 0)
                {
                    DeleVector(tmp.even_time);
                }
                else
                {
                    DeleVector(tmp.odd_time);
                }
                for (int i = code_list.Count-1;i>-1; i--)
                {
                    if (tmp.icao == code_list[i].icao)
                    {
                        index = i;
                        if (tmp.cpr == 0)
                        {
                            code_list[i].XZ0 = tmp.XZ0;
                            code_list[i].YZ0 = tmp.YZ0;
                            code_list[i].even_time = tmp.even_time;
                            code_list[i].even_sign = 1;

                            if (code_list[i].odd_sign == 1)//另一个有数
                            {
                                if (Math.Abs(code_list[i].odd_time - code_list[i].even_time) < 40)
                                {
                                    return true;//用奇偶算法
                                }
                                else
                                {
                                    //code_list.Add(tmp);
                                    ////DeleVector(tmp.even_time);
                                    //index = code_list.Count - 1;
                                    return false;//用本地算法
                                }
                            }
                            else
                            {
                                //code_list.Add(tmp);
                                ////DeleVector(tmp.even_time);
                                //index = code_list.Count - 1;
                                return false;//用本地算法
                            }

                        }
                        else// if (tmp.cpr == 1)
                        {
                            code_list[i].XZ1 = tmp.XZ1;
                            code_list[i].YZ1 = tmp.YZ1;
                            code_list[i].odd_time = tmp.odd_time;
                            code_list[i].odd_sign = 1;

                            if (code_list[i].even_sign == 1)//另一个有数
                            {
                                if (Math.Abs(code_list[i].odd_time - code_list[i].even_time) < 40)
                                {
                                    return true;
                                }
                                else
                                {
                                    //code_list.Add(tmp);
                                    ////DeleVector(tmp.even_time);
                                    //index = code_list.Count - 1;
                                    return false;
                                }
                            }
                            else
                            {
                                //code_list.Add(tmp);
                                //DeleVector(tmp.even_time);
                                //index = code_list.Count - 1;
                                return false;
                            }
                        }
                    }
                }

            }
            catch (System.Exception ex)
            {
                // ErrorRecord.ProcessError(ex.ToString());
            }
            code_list.Add(tmp);
            //DeleVector(tmp.even_time);
            index = code_list.Count - 1;
            return false;
        }
        //判断是未超过经纬度界限
        private static bool CheckRegion(double lat, double lon, double lat_c, double lon_c)
        {
            try
            {
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
                    if (Math.Abs(lat - lat_c) < lat_limit && Math.Abs(lon - lon_c) < lon_limit)
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
        //判断是全局变量是否超出范围
        private static void DeleVector(uint time)
        {
            int k = 0;
            uint new_time = 0;

            for (int i = 0; i < code_list.Count(); i++)
            {
                new_time = code_list[i].even_time;
                if ((new_time - time) <= 60 && (time - new_time) <= 60)
                {
                    k = i;
                    break;
                }
            }
            code_list.RemoveRange(0, k);
        }



        public ADS_MessageInfo INFO_ADS(ADS_MessageInfo ADS_info, byte[] test, int CRC, UInt32 ads_time)
        {
            try
            {
                switch (test[3])//协议解析
                {
                    case 0:
                        break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                        ADS_info = ADS_MessageInfo.GetAirPlaneID(test);
                   
                        break;
                    case 5:
                    case 6:
                    case 7:
                    case 8:
                        //ADS_info = ADS_MessageInfo.GetEarthLocation(test, RecData.dLon, RecData.dLat, RecData.ads_time);
                        ADS_info = ADS_MessageInfo.GetEarthLocation(test, 0, 0, ads_time);
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
                        ADS_info = ADS_MessageInfo.GetAirLocation(test,0, 0, ads_time);
                        
                        break;
                    case 19:
                        ADS_info = ADS_MessageInfo.GetAirSpeed(test);
                      
                        break;
                    case 20:
                    case 21:
                    case 22:
                        ADS_info = ADS_MessageInfo.GetAirLocation(test, 0, 0, ads_time);
                        
                        break;
                    case 28:
                        ADS_info = ADS_MessageInfo.GetPauseAirCondition(test);
                       
                        break;
                    case 29:
                        ADS_info = ADS_MessageInfo.GetTargetState(test);
                        
                        break;
                    case 31:
                        ADS_info = ADS_MessageInfo.GetAirCondition(test);
                        
                        break;
                    default:
                        break;
                }

            }
            catch (System.Exception ex)
            {
                //ErrorRecord.ProcessError(ex.ToString());
            }

            return ADS_info;
        }

    }
}
