using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ADS_B_INFO
{
    public class ADS_DicMessage
    {
        //发射器的飞机身份
        public static string GetAirplaneID(int type,int  idcode)
        {
            string c = "无";
            switch (type)
            {
                case 4:
                    if (Dic_AirplaneID_A.ContainsKey(idcode))
                    {
                        c = Dic_AirplaneID_A[idcode];
                    }
                    break;
                case 3:
                    if (Dic_AirplaneID_B.ContainsKey(idcode))
                    {
                        c = Dic_AirplaneID_B[idcode];
                    }
                    break;
                case 2:
                    if (Dic_AirplaneID_C.ContainsKey(idcode))
                    {
                        c = Dic_AirplaneID_C[idcode];
                    }
                    break;
                default:
                    break;                   
            }
            return c;
        }
        //监视状况
        public static string GetMonitorState( int idcode)
        {
            string c = "无";
            if (Dic_MonitorState.ContainsKey(idcode))
            {
                c = Dic_MonitorState[idcode];
            }
            return c;
        }
        //单天线
        public static string GetSingleAntenna(int idcode)
        {
            string c = "无";
            if (Dic_SingleAntenna.ContainsKey(idcode))
            {
                c = Dic_SingleAntenna[idcode];
            }
            return c;
        }
        //地表面位置信息—运行
        public static string GetWorkingState(int code)
        {
            string c = "无可用运行信息";
            if (code == 1)
            {
                c = "飞机停止";
            }
            else if (code >1&&code<=8)
            {
                c = (0.125*code).ToString()+"哩/小时";
            }
            else if (code > 8 && code <= 12)
            {
                c = (1 + 0.25 * (code-8)).ToString() + "哩/小时";
            }
            else if (code > 12 && code <= 38)
            {
                c = (2 + 0.5 * (code -12)).ToString() + "哩/小时";
            }
            else if (code > 38 && code <= 93)
            {
                c = (code - 23).ToString() + "哩/小时";
            }
            else if (code > 93 && code <= 108)
            {
                c = (70 + 2 * (code - 93)).ToString() + "哩/小时";
            }
            else if (code > 108 && code <= 123)
            {
                c = (100 + 5 * (code - 108)).ToString() + "哩/小时";
            }
            else if (code ==124)
            {
                c = ">175哩/小时";
            }
            else if (code == 125)
            {
                c = "为飞机减速预留";
            }
            else if (code == 126)
            {
                c = "为飞机加速预留";
            }
            else if (code == 127)
            {
                c = "为飞机转向预留";
            }

            return c;
        }
        //地表面消息—航向/地面航迹状况标志位
        public static string GetPathStateFlag(int idcode)
        {
            string c = "无";
            if (Dic_PathStateFlag.ContainsKey(idcode))
            {
                c = Dic_PathStateFlag[idcode];
            }
            return c;
        }
        //地表面位置信息—航向/地面航迹
        public static string GetPathState(int code)
        {
            string c = "";
            c = (2.8125 * code).ToString() + "度";
            return c;
        }
        //空中速度信息—意图变更标志
        public static string GetAimChange(int code)
        {
            string c = "无";
            if (Dic_AimChange.ContainsKey(code))
            {
                c = Dic_AimChange[code];
            }
            return c;
        }
        //空中速度信息—IFR
        public static string GetIFR(int code)
        {
            string c = "无";
            if (Dic_IFR.ContainsKey(code))
            {
                c = Dic_IFR[code];
            }
            return c;
        }
        //空中速度信息—飞行航向
        public static double GetDirectionofSpeed(int type,byte west_east_flag, byte nouth_south__flag, UInt16 speedCode1, UInt16 speedCode2)
        {
            try
            {
                int sep1 = 0;
                int sep2 = 0;
                if (speedCode1 == 0 || speedCode2 == 0 || (speedCode1 == 1 && speedCode2 == 1) || speedCode1 == 1023 || speedCode2 == 1023)//无可用或速度为零或无法确定方向
                {
                    if (west_east_flag == 1 && nouth_south__flag == 1)
                        return 225;
                    else if (west_east_flag == 1 && nouth_south__flag == 0)
                        return 315;
                    else if (west_east_flag == 0 && nouth_south__flag == 1)
                        return 135;
                    else
                        return 45;
                }
                else if (speedCode2 == 1)//南北速度为零
                {
                    if (west_east_flag == 1)
                        return 270;
                    else
                        return 90;
                }
                else if (speedCode1 == 1)//东西速度为零
                {
                    if (nouth_south__flag == 1)
                        return 180;
                    else
                        return 0;
                }
                else
                {
                    if (type == 1)
                    {
                        sep1 = speedCode1 - 1;
                        sep2 = speedCode2 - 1;
                    }
                    else if (type == 2)
                    {
                        sep1 =(speedCode1 - 1) * 4;
                        sep2 = (speedCode2 - 1) * 4;
                    }


                    UInt16 angle = (UInt16)(180.0 * Math.Atan((double)sep1 / sep2) / Math.PI);
                    if (west_east_flag == 1 && nouth_south__flag == 1)
                        return (UInt16)(180 + angle);
                    else if (west_east_flag == 1 && nouth_south__flag == 0)
                        return (UInt16)(270 + angle);
                    else if (west_east_flag == 0 && nouth_south__flag == 1)
                        return (UInt16)(90 + angle);
                    else
                        return angle;
                }
            }
            catch (System.Exception ex)
            {
                return 361;
            }
        }
        //空中速度信息—飞行航速
        public static double  GetAirSpeed(int type,double Direction, UInt16 speedCode1, UInt16 speedCode2)
        {
            double speed = 0;
            try
            {
                int sep1 = 0;
                int sep2 = 0;
                if (type == 1)
                {
                    sep1 = speedCode1 - 1;
                    sep2 = speedCode2 - 1;
                }
                else if (type == 2)
                {
                    sep1 = (speedCode1 - 1)*4;
                    sep2 = (speedCode2 - 1) * 4;
                }



                if (Direction == 0 || Direction == 180)
                {
                    speed = sep2;
                }
                else if (Direction == 90 || Direction == 270)
                {
                    speed = sep1;
                }
                else if ((Direction > 0  && Direction < 90))
                {
                    speed = sep1 / Math.Sin(2 * Math.PI /360 * Direction);
                }
                else if ((Direction > 90  && Direction < 180))
                {
                    speed = sep1 / Math.Cos(2 * Math.PI / 360 *( Direction - 90));
                }
                else if ((Direction > 180  && Direction < 270))
                {
                    speed = sep1 / Math.Sin(2 * Math.PI / 360 *( Direction - 180));
                }
                else if ((Direction > 270 && Direction < 360))
                {
                    speed = sep1 / Math.Cos(2 * Math.PI / 360 * (Direction - 270));
                }


            }
            catch (System.Exception ex)
            {
                // ErrorRecord.ProcessError(ex.ToString());
            }
            return speed;
        }
        //空中速度信息—垂直速度来源标志位
        public static string GetVerticalSpeedSource(int code)
        {
            string c = "无";
            if (Dic_VerticalSpeedSource.ContainsKey(code))
            {
                c = Dic_VerticalSpeedSource[code];
            }
            return c;
        }
        //空中速度信息—垂直速度来源标志位
        public static string GetVerticalSpeedFlag(int code)
        {
            string c = "无";
            if (Dic_VerticalSpeedFlag.ContainsKey(code))
            {
                c = Dic_VerticalSpeedFlag[code];
            }
            return c;
        }
        //空中速度信息—垂直速度
        public static string GetVerticalSpeed(int code)
        {
            string c = "无可用垂直速度信息";
            if (code>0&&code<511)
            {
                c = ((code - 1) * 64).ToString();
            }
            else if (code == 511)
            {
                c = ">32608";
            }
            return c;
        }
        //空中速度信息—源于大气压高度差符号标志位
        public static string GetAtmosphereFlag(int code)
        {
            string c = "无";
            if (Dic_AtmosphereFlag.ContainsKey(code))
            {
                c = Dic_AtmosphereFlag[code];
            }
            return c;
        }
        //空中速度信息—源于大气压高度差
        public static string GetAtmosphere(int code)
        {
            string c = "无可用GNSS高度源数据差信息";
            if (code > 0 && code < 127)
            {
                c = ((code - 1) * 25).ToString()+"英尺";
            }
            else if (code == 127)
            {
                c = ">3137.5英尺";
            }
            return c;
        }
        //空中速度信息—飞行航向
        public static double GetDirectionFor34(byte flag, UInt16 direction)
        {
            double c = 0.00;
            try
            {
                if (flag == 0x01)
                {
                    c = direction * 0.3515625;
                }
            }
            catch
            {
            }
            return c;
        }
        //空中速度信息—飞行航速
        public static double GetAirSpeedFor34(int type, UInt16 speedold)
        {
            double speed = 0;
            try
            {
                if (type == 3)
                {
                    speed = speedold - 1;
                }
                else
                {
                    speed = (speedold - 1) * 4;
                }

            }
            catch (System.Exception ex)
            {
                // ErrorRecord.ProcessError(ex.ToString());
            }
            return speed;
        }
        //空中速度信息—空速类型
        public static string GetAirSpeedType(int code)
        {
            string c = "无";
            if (Dic_AirSpeedType.ContainsKey(code))
            {
                c = Dic_AirSpeedType[code];
            }
            return c;
        }
        //目标状态与状况消息—垂直数据可用/源指示器
        public static string GetAtmosphereDataSource(int code)
        {
            string c = "无";
            if (Dic_AtmosphereDataSource.ContainsKey(code))
            {
                c = Dic_AtmosphereDataSource[code];
            }
            return c;
        }
        //目标状态与状况消息—目标高度类型
        public static string GetHeightType(int code)
        {
            string c = "无";
            if (Dic_HeightType.ContainsKey(code))
            {
                c = Dic_HeightType[code];
            }
            return c;
        }
        //目标状态与状况消息—目标高度性能
        public static string GetHeightProperty(int code)
        {
            string c = "无";
            if (Dic_HeightProperty.ContainsKey(code))
            {
                c = Dic_HeightProperty[code];
            }
            return c;
        }
        //目标状态与状况消息—垂直水平模式指示器
        public static string GetAtmosphereLevelMode(int code)
        {
            string c = "无";
            if (Dic_AtmosphereLevelMode.ContainsKey(code))
            {
                c = Dic_AtmosphereLevelMode[code];
            }
            return c;
        }
        //目标状态与状况消息—目标高度
        public static int GetTargetHeight(UInt16 code)
        {
            int height = 0;
            try
            {
                height = code * 100 - 1000;
            }
            catch (System.Exception ex)
            {
                // ErrorRecord.ProcessError(ex.ToString());
            }
            return height;
        }
        //目标状态与状况消息—水平数据可用/源指示器
        public static string GetLevelDataSource(int code)
        {
            string c = "无";
            if (Dic_LevelDataSource.ContainsKey(code))
            {
                c = Dic_LevelDataSource[code];
            }
            return c;
        }
        //目标状态与状况消息—目标航向/航迹指示器
        public static string GetTargetDirectionFlag(int code)
        {
            string c = "无";
            if (Dic_TargetDirectionFlag.ContainsKey(code))
            {
                c = Dic_TargetDirectionFlag[code];
            }
            return c;
        }
        //目标状态与状况消息—性能/模式代码
        public static string GetPropertyCode(int code)
        {
            string c = "无";
            switch(code)
            {
                case 0:
                    c = "TCAS/ACAS运行且非TCAS/ACAS分辨力提示起作用";
                    break;
                case 1:
                    c = "TCAS/ACAS运行且TCAS/ACAS分辨力提示起作用";
                    break;
                case 2:
                    c = "TCAS/ACAS没有运行且非TCAS/ACAS分辨力提示起作用";
                    break;
                case 3:
                    c = "TCAS/ACAS没有运行且TCAS/ACAS分辨力提示起作用";
                    break;
            }
            return c;
        }
        //目标状态与状况消息—紧急/优先
        public static string GetEmergencyCode(int code)
        {
            string c = "无";
            if (Dic_EmergencyCode.ContainsKey(code))
            {
                c = Dic_EmergencyCode[code];
            }
            return c;
        }
        //飞机工作运行状况—CDTI交通显示性能
        public static string GetCC_CDTI(int code)
        {
            string c = "无";
            if (Dic_CDTI.ContainsKey(code))
            {
                c = Dic_CDTI[code];
            }
            return c;
        }
        //飞机工作运行状况—ARV报告功能
        public static string GetCC_ARV(int code)
        {
            string c = "无";
            if (Dic_ARV.ContainsKey(code))
            {
                c = Dic_ARV[code];
            }
            return c;
        }
        //飞机工作运行状况—TS报告功能
        public static string GetCC_TS(int code)
        {
            string c = "无";
            if (Dic_TS.ContainsKey(code))
            {
                c = Dic_TS[code];
            }
            return c;
        }
        //飞机工作运行状况—TC报告功能
        public static string GetCC_TC(int code)
        {
            string c = "无";
            if (Dic_TC.ContainsKey(code))
            {
                c = Dic_TC[code];
            }
            return c;
        }
        //飞机工作运行状况—位置偏移应用
        public static string GetCC_POA(int code)
        {
            string c = "无";
            if (Dic_POA.ContainsKey(code))
            {
                c = Dic_POA[code];
            }
            return c;
        }
        //飞机工作运行状况—B2Low
        public static string GetCC_B2Low(int code)
        {
            string c = "无";
            if (Dic_B2Low.ContainsKey(code))
            {
                c = Dic_B2Low[code];
            }
            return c;
        }
        //飞机工作运行状况—RA激活
        public static string GetOM_RA(int code)
        {
            string c = "无";
            if (Dic_RA.ContainsKey(code))
            {
                c = Dic_RA[code];
            }
            return c;
        }
        //飞机工作运行状况—IDENT开关激活
        public static string GetOM_IDENT(int code)
        {
            string c = "无";
            if (Dic_IDENT.ContainsKey(code))
            {
                c = Dic_IDENT[code];
            }
            return c;
        }
        //飞机工作运行状况—接收ATC服务
        public static string GetOM_ATC(int code)
        {
            string c = "无";
            if (Dic_ATC.ContainsKey(code))
            {
                c = Dic_ATC[code];
            }
            return c;
        }
        //飞机工作运行状况—版本号MOPS
        public static string GetMOPS(int code)
        {
            string c = "无";
            if (Dic_MOPS.ContainsKey(code))
            {
                c = Dic_MOPS[code];
            }
            return c;
        }
        //飞机工作运行状况—位置导航精准度类别
        public static string GetNACp(int code)
        {
            string c = "无";
            if (Dic_NACp.ContainsKey(code))
            {
                c = Dic_NACp[code];
            }
            return c;
        }
        //飞机工作运行状况—临视完整度级别
        public static string GetSIL(int code)
        {
            string c = "无";
            if (Dic_SIL.ContainsKey(code))
            {
                c = Dic_SIL[code];
            }
            return c;
        }
        //飞机工作运行状况—大气压高度完整度代码
        public static string GetNIC_BARO(int code)
        {
            string c = "无";
            if (Dic_NIC_BARO.ContainsKey(code))
            {
                c = Dic_NIC_BARO[code];
            }
            return c;
        }
        //飞机工作运行状况—航迹角/航向
        public static string GetTRK_HDG(int code)
        {
            string c = "无";
            if (Dic_TRK_HDG.ContainsKey(code))
            {
                c = Dic_TRK_HDG[code];
            }
            return c;
        }
        //飞机工作运行状况—飞机车辆长度和宽度代码
        public static string GetL_WType(int code)
        {
            string c = "无";
            if (Dic_L_WType.ContainsKey(code))
            {
                c = Dic_L_WType[code];
            }
            return c;
        }
        //飞机工作运行状况—水平参考方向
        public static string GetHRD(int code)
        {
            string c = "无";
            if (Dic_HRD.ContainsKey(code))
            {
                c = Dic_HRD[code];
            }
            return c;
        }


        //****************************************ADS_B消息字典*****************************************************//
        /// <summary>
        /// 发射器类别“A”的飞机身份：  AirplaneID_A
        /// </summary>
        private static Dictionary<int, string> _Dic_AirplaneID_A = null;

        public static Dictionary<int, string> Dic_AirplaneID_A
        {
            get
            {
                if (_Dic_AirplaneID_A == null)
                {
                    _Dic_AirplaneID_A = new Dictionary<int, string>();
                    _Dic_AirplaneID_A.Add(1, "轻型(<15500磅)");
                    _Dic_AirplaneID_A.Add(2, "小型(15500到75000磅)");
                    _Dic_AirplaneID_A.Add(3, "大型(75000到300000磅)");
                    _Dic_AirplaneID_A.Add(4, "高漩涡式大型(如B-757飞机)");
                    _Dic_AirplaneID_A.Add(5, "重型(>300000磅)");
                    _Dic_AirplaneID_A.Add(6, "高性能(>5g加速度且>400哩/小时)");
                    _Dic_AirplaneID_A.Add(7, "旋翼飞机");           
                }
                return _Dic_AirplaneID_A;
            }
        }

        /// <summary>
        /// 发射器类别“B”的飞机身份：  AirplaneID_B
        /// </summary>
        private static Dictionary<int, string> _Dic_AirplaneID_B = null;

        public static Dictionary<int, string> Dic_AirplaneID_B
        {
            get
            {
                if (_Dic_AirplaneID_B == null)
                {
                    _Dic_AirplaneID_B = new Dictionary<int, string>();
                    _Dic_AirplaneID_B.Add(1, "滑翔机");
                    _Dic_AirplaneID_B.Add(2, "Light-than-air超轻型");
                    _Dic_AirplaneID_B.Add(3, "Parachutist/skydriver");
                    _Dic_AirplaneID_B.Add(4, "超轻型/悬挂式滑翔机/翼伞飞行器");
                    _Dic_AirplaneID_B.Add(6, "无人飞行器");
                    _Dic_AirplaneID_B.Add(7, "Space/trans-atmospheric");
                }
                return _Dic_AirplaneID_B;
            }
        }

        /// <summary>
        /// 发射器类别“C”的飞机身份：  AirplaneID_C
        /// </summary>
        private static Dictionary<int, string> _Dic_AirplaneID_C = null;

        public static Dictionary<int, string> Dic_AirplaneID_C
        {
            get
            {
                if (_Dic_AirplaneID_C == null)
                {
                    _Dic_AirplaneID_C = new Dictionary<int, string>();
                    _Dic_AirplaneID_C.Add(1, "水面航行器-应急服务");
                    _Dic_AirplaneID_C.Add(2, "水面航行器-服务设备");
                    _Dic_AirplaneID_C.Add(3, "单点式障碍物(包括系留气球)");
                    _Dic_AirplaneID_C.Add(4, "簇型障碍物");
                    _Dic_AirplaneID_C.Add(5, "线型障碍物");
                }
                return _Dic_AirplaneID_C;
            }
        }

        /// <summary>
        /// 空中消息-监视状况：  MonitorState
        /// </summary>
        private static Dictionary<int, string> _Dic_MonitorState = null;

        public static Dictionary<int, string> Dic_MonitorState
        {
            get
            {
                if (_Dic_MonitorState == null)
                {
                    _Dic_MonitorState = new Dictionary<int, string>();
                    _Dic_MonitorState.Add(0, "无情景信息");
                    _Dic_MonitorState.Add(1, "长时间的告警情形(紧急情况)");
                    _Dic_MonitorState.Add(2, "短暂的告警情形(除紧急情况外,模A身份代码变动)");
                    _Dic_MonitorState.Add(3, "特殊的位置识别(SPI)情形");
                }
                return _Dic_MonitorState;
            }
        }

        /// <summary>
        /// 空中消息-单天线：  SingleAntenna
        /// </summary>
        private static Dictionary<int, string> _Dic_SingleAntenna = null;

        public static Dictionary<int, string> Dic_SingleAntenna
        {
            get
            {
                if (_Dic_SingleAntenna == null)
                {
                    _Dic_SingleAntenna = new Dictionary<int, string>();
                    _Dic_SingleAntenna.Add(0, "双天线通道运行");
                    _Dic_SingleAntenna.Add(1, "单天线运行");

                }
                return _Dic_SingleAntenna;
            }
        }

        /// <summary>
        /// 地表面消息-航向/地面航迹状况标志位：  PathStateFlag
        /// </summary>
        private static Dictionary<int, string> _Dic_PathStateFlag = null;

        public static Dictionary<int, string> Dic_PathStateFlag
        {
            get
            {
                if (_Dic_PathStateFlag == null)
                {
                    _Dic_PathStateFlag = new Dictionary<int, string>();
                    _Dic_PathStateFlag.Add(0, "航向/地面航迹数据无效");
                    _Dic_PathStateFlag.Add(1, "航向/地面航迹数据有效");
                }
                return _Dic_PathStateFlag;
            }
        }

        /// <summary>
        /// 空中速度消息-意图变更标志：  AimChange
        /// </summary>
        private static Dictionary<int, string> _Dic_AimChange = null;

        public static Dictionary<int, string> Dic_AimChange
        {
            get
            {
                if (_Dic_AimChange == null)
                {
                    _Dic_AimChange = new Dictionary<int, string>();
                    _Dic_AimChange.Add(0, "意图无改变");
                    _Dic_AimChange.Add(1, "意图改变");
                }
                return _Dic_AimChange;
            }
        }

        /// <summary>
        /// 空中速度消息-IFR能力标志：  IFR
        /// </summary>
        private static Dictionary<int, string> _Dic_IFR = null;

        public static Dictionary<int, string> Dic_IFR
        {
            get
            {
                if (_Dic_IFR == null)
                {
                    _Dic_IFR = new Dictionary<int, string>();
                    _Dic_IFR.Add(0, "发射飞机没有ADS_B设备等级“A1”或以上的应用需求");
                    _Dic_IFR.Add(1, "发射飞机有ADS_B设备等级“A1”或以上的应用需求");
                }
                return _Dic_IFR;
            }
        }

        /// <summary>
        /// 空中速度消息-垂直速度来源标志位： VerticalSpeed
        /// </summary>
        private static Dictionary<int, string> _Dic_VerticalSpeedSource = null;

        public static Dictionary<int, string> Dic_VerticalSpeedSource
        {
            get
            {
                if (_Dic_VerticalSpeedSource == null)
                {
                    _Dic_VerticalSpeedSource = new Dictionary<int, string>();
                    _Dic_VerticalSpeedSource.Add(0, "几何数据源");
                    _Dic_VerticalSpeedSource.Add(1, "大气压源");
                }
                return _Dic_VerticalSpeedSource;
            }
        }

        /// <summary>
        /// 空中速度消息-垂直速度符号标志位： VerticalSpeedFlag
        /// </summary>
        private static Dictionary<int, string> _Dic_VerticalSpeedFlag = null;

        public static Dictionary<int, string> Dic_VerticalSpeedFlag
        {
            get
            {
                if (_Dic_VerticalSpeedFlag == null)
                {
                    _Dic_VerticalSpeedFlag = new Dictionary<int, string>();
                    _Dic_VerticalSpeedFlag.Add(0, "向上");
                    _Dic_VerticalSpeedFlag.Add(1, "向下");
                }
                return _Dic_VerticalSpeedFlag;
            }
        }

        /// <summary>
        /// 空中速度消息-源于大气压高度差符号标志位： AtmosphereFlag
        /// </summary>
        private static Dictionary<int, string> _Dic_AtmosphereFlag = null;

        public static Dictionary<int, string> Dic_AtmosphereFlag
        {
            get
            {
                if (_Dic_AtmosphereFlag == null)
                {
                    _Dic_AtmosphereFlag = new Dictionary<int, string>();
                    _Dic_AtmosphereFlag.Add(0, "几何高度源数据大于大气压高度源数据");
                    _Dic_AtmosphereFlag.Add(1, "几何高度源数据小于大气压高度源数据");
                }
                return _Dic_AtmosphereFlag;
            }
        }

        /// <summary>
        /// 空中速度消息-空速类型： AirSpeedType
        /// </summary>
        private static Dictionary<int, string> _Dic_AirSpeedType = null;

        public static Dictionary<int, string> Dic_AirSpeedType
        {
            get
            {
                if (_Dic_AirSpeedType == null)
                {
                    _Dic_AirSpeedType = new Dictionary<int, string>();
                    _Dic_AirSpeedType.Add(0, "IAS");
                    _Dic_AirSpeedType.Add(1, "TAS");
                }
                return _Dic_AirSpeedType;
            }
        }

        /// <summary>
        ///目标状态与状况消息-垂直数据可用/源指示器： AtmosphereDataSource
        /// </summary>
        private static Dictionary<int, string> _Dic_AtmosphereDataSource = null;

        public static Dictionary<int, string> Dic_AtmosphereDataSource
        {
            get
            {
                if (_Dic_AtmosphereDataSource == null)
                {
                    _Dic_AtmosphereDataSource = new Dictionary<int, string>();
                    _Dic_AtmosphereDataSource.Add(0, "无有效垂直目标状态数据可用");
                    _Dic_AtmosphereDataSource.Add(1, "可选择值得自动驾驶控制面板");
                    _Dic_AtmosphereDataSource.Add(2, "保持高度");
                    _Dic_AtmosphereDataSource.Add(3, "FMS/RNAV系统");
                }
                return _Dic_AtmosphereDataSource;
            }
        }

        /// <summary>
        ///目标状态与状况消息-目标高度类型： HeightType
        /// </summary>
        private static Dictionary<int, string> _Dic_HeightType = null;

        public static Dictionary<int, string> Dic_HeightType
        {
            get
            {
                if (_Dic_HeightType == null)
                {
                    _Dic_HeightType = new Dictionary<int, string>();
                    _Dic_HeightType.Add(0, "参考压力高度的目标高度");
                    _Dic_HeightType.Add(1, "参考修正压力高度的目标高度");
                }
                return _Dic_HeightType;
            }
        }

        /// <summary>
        ///目标状态与状况消息-目标高度性能： HeightProperty
        /// </summary>
        private static Dictionary<int, string> _Dic_HeightProperty = null;

        public static Dictionary<int, string> Dic_HeightProperty
        {
            get
            {
                if (_Dic_HeightProperty == null)
                {
                    _Dic_HeightProperty = new Dictionary<int, string>();
                    _Dic_HeightProperty.Add(0, "仅报告保持高度的性能");
                    _Dic_HeightProperty.Add(1, "报告保持高度或报告自动驾驶控制板选择高度的能力");
                    _Dic_HeightProperty.Add(2, "报告保持高度、自动驾驶控制板选择高度、或任何FMS/RNAV调整平面高度能力");
                    _Dic_HeightProperty.Add(3, "保留");
                }
                return _Dic_HeightProperty;
            }
        }

        /// <summary>
        ///目标状态与状况消息-垂直模式指示器： AtmosphereLevelMode
        /// </summary>
        private static Dictionary<int, string> _Dic_AtmosphereLevelMode = null;

        public static Dictionary<int, string> Dic_AtmosphereLevelMode
        {
            get
            {
                if (_Dic_AtmosphereLevelMode == null)
                {
                    _Dic_AtmosphereLevelMode = new Dictionary<int, string>();
                    _Dic_AtmosphereLevelMode.Add(0, "未知模式或无法获得信息");
                    _Dic_AtmosphereLevelMode.Add(1, "“获取”模式");
                    _Dic_AtmosphereLevelMode.Add(2, "“捕获”或“保持”模式");
                    _Dic_AtmosphereLevelMode.Add(3, "保留");
                }
                return _Dic_AtmosphereLevelMode;
            }
        }

        /// <summary>
        ///目标状态与状况消息-水平数据可用/源指示器： LevelDataSource
        /// </summary>
        private static Dictionary<int, string> _Dic_LevelDataSource = null;

        public static Dictionary<int, string> Dic_LevelDataSource
        {
            get
            {
                if (_Dic_LevelDataSource == null)
                {
                    _Dic_LevelDataSource = new Dictionary<int, string>();
                    _Dic_LevelDataSource.Add(0, "无有效水平目标状态数据可获得");
                    _Dic_LevelDataSource.Add(1, "可选择值得自动驾驶控制面板");
                    _Dic_LevelDataSource.Add(2, "保持当前航向或航迹角度");
                    _Dic_LevelDataSource.Add(3, "FMS/RNAV系统");
                }
                return _Dic_LevelDataSource;
            }
        }

        /// <summary>
        ///目标状态与状况消息-目标航向/航迹指示器： TargetDirectionFlag
        /// </summary>
        private static Dictionary<int, string> _Dic_TargetDirectionFlag = null;

        public static Dictionary<int, string> Dic_TargetDirectionFlag
        {
            get
            {
                if (_Dic_TargetDirectionFlag == null)
                {
                    _Dic_TargetDirectionFlag = new Dictionary<int, string>();
                    _Dic_TargetDirectionFlag.Add(0, "报告的目标航向角度");
                    _Dic_TargetDirectionFlag.Add(1, "报告的目标航迹角度");
                }
                return _Dic_TargetDirectionFlag;
            }
        }

        /// <summary>
        ///目标状态与状况消息-应急/优先： EmergencyCode
        /// </summary>
        private static Dictionary<int, string> _Dic_EmergencyCode = null;

        public static Dictionary<int, string> Dic_EmergencyCode
        {
            get
            {
                if (_Dic_EmergencyCode == null)
                {
                    _Dic_EmergencyCode = new Dictionary<int, string>();
                    _Dic_EmergencyCode.Add(0, "不紧急");
                    _Dic_EmergencyCode.Add(1, "一般紧急");
                    _Dic_EmergencyCode.Add(2, "救生/医疗应急");
                    _Dic_EmergencyCode.Add(3, "极少燃料");
                    _Dic_EmergencyCode.Add(4, "无通信");
                    _Dic_EmergencyCode.Add(5, "非法接口");
                    _Dic_EmergencyCode.Add(6, "飞机下降");
                    _Dic_EmergencyCode.Add(7, "保留");

                }
                return _Dic_EmergencyCode;
            }
        }

        /// <summary>
        ///飞机工作运行状况-CDTI交通显示性能： CDTI
        /// </summary>
        private static Dictionary<int, string> _Dic_CDTI = null;

        public static Dictionary<int, string> Dic_CDTI
        {
            get
            {
                if (_Dic_CDTI == null)
                {
                    _Dic_CDTI = new Dictionary<int, string>();
                    _Dic_CDTI.Add(0, "无CDTI交通显示性能");
                    _Dic_CDTI.Add(1, "发射飞机安装和运行CDTI");
                }
                return _Dic_CDTI;
            }
        }

        /// <summary>
        ///飞机工作运行状况-ARV报告功能： ARV
        /// </summary>
        private static Dictionary<int, string> _Dic_ARV = null;

        public static Dictionary<int, string> Dic_ARV
        {
            get
            {
                if (_Dic_ARV == null)
                {
                    _Dic_ARV = new Dictionary<int, string>();
                    _Dic_ARV.Add(0, "不能发送消息去支持空中参考速度报告");
                    _Dic_ARV.Add(1, "能发送消息去支持空中参考速度报告");
                }
                return _Dic_ARV;
            }
        }

        /// <summary>
        ///飞机工作运行状况-TS报告功能： TS
        /// </summary>
        private static Dictionary<int, string> _Dic_TS = null;

        public static Dictionary<int, string> Dic_TS
        {
            get
            {
                if (_Dic_TS == null)
                {
                    _Dic_TS = new Dictionary<int, string>();
                    _Dic_TS.Add(0, "不能发送消息去支持目标状态报告");
                    _Dic_TS.Add(1, "能发送消息去支持目标状态报告");
                }
                return _Dic_TS;
            }
        }

        /// <summary>
        ///飞机工作运行状况-TC报告功能： TC
        /// </summary>
        private static Dictionary<int, string> _Dic_TC = null;

        public static Dictionary<int, string> Dic_TC
        {
            get
            {
                if (_Dic_TC == null)
                {
                    _Dic_TC = new Dictionary<int, string>();
                    _Dic_TC.Add(0, "不能发送消息去支持轨迹改变报告");
                    _Dic_TC.Add(1, "仅能发送消息去支持TC+0报告");
                    _Dic_TC.Add(2, "能为多个TC报告发送消息");
                    _Dic_TC.Add(3, "预留");
                }
                return _Dic_TC;
            }
        }

        /// <summary>
        ///飞机工作运行状况-位置偏移应用： POA
        /// </summary>
        private static Dictionary<int, string> _Dic_POA = null;

        public static Dictionary<int, string> Dic_POA
        {
            get
            {
                if (_Dic_POA == null)
                {
                    _Dic_POA = new Dictionary<int, string>();
                    _Dic_POA.Add(0, "以A/V的ADS-B位置参考点作为参考，水平位置消息中发射的位置未知");
                    _Dic_POA.Add(1, "以A/V的ADS-B位置参考点作为参考，水平位置消息中发射的位置可知");
                }
                return _Dic_POA;
            }
        }

        /// <summary>
        ///飞机工作运行状况-B2Low： B2Low
        /// </summary>
        private static Dictionary<int, string> _Dic_B2Low = null;

        public static Dictionary<int, string> Dic_B2Low
        {
            get
            {
                if (_Dic_B2Low == null)
                {
                    _Dic_B2Low = new Dictionary<int, string>();
                    _Dic_B2Low.Add(0, "地面设备发射功率小于70瓦");
                    _Dic_B2Low.Add(1, "地面设备发射功率小于70瓦之外，但是满足B2级别需求的非应答的发射子系统");
                }
                return _Dic_B2Low;
            }
        }

        /// <summary>
        ///飞机工作运行状况-RA激活： RA
        /// </summary>
        private static Dictionary<int, string> _Dic_RA = null;

        public static Dictionary<int, string> Dic_RA
        {
            get
            {
                if (_Dic_RA == null)
                {
                    _Dic_RA = new Dictionary<int, string>();
                    _Dic_RA.Add(0, "TCAS II或ACAS分辨力告警无效");
                    _Dic_RA.Add(1, "TCAS II或ACAS分辨力告警有效");
                }
                return _Dic_RA;
            }
        }

        /// <summary>
        ///飞机工作运行状况-IDENT开关激活： IDENT
        /// </summary>
        private static Dictionary<int, string> _Dic_IDENT = null;

        public static Dictionary<int, string> Dic_IDENT
        {
            get
            {
                if (_Dic_IDENT == null)
                {
                    _Dic_IDENT = new Dictionary<int, string>();
                    _Dic_IDENT.Add(0, "IDENT开关激活");
                    _Dic_IDENT.Add(1, "IDENT开关未激活");
                }
                return _Dic_IDENT;
            }
        }

        /// <summary>
        ///飞机工作运行状况-接收ATC服务： ATC
        /// </summary>
        private static Dictionary<int, string> _Dic_ATC = null;

        public static Dictionary<int, string> Dic_ATC
        {
            get
            {
                if (_Dic_ATC == null)
                {
                    _Dic_ATC = new Dictionary<int, string>();
                    _Dic_ATC.Add(0, "ADS-B发射子系统接收ATC服务");
                    _Dic_ATC.Add(1, "ADS-B发射子系统未接收ATC服务");
                }
                return _Dic_ATC;
            }
        }

        /// <summary>
        ///飞机工作运行状况-版本号： MOPS
        /// </summary>
        private static Dictionary<int, string> _Dic_MOPS = null;

        public static Dictionary<int, string> Dic_MOPS
        {
            get
            {
                if (_Dic_MOPS == null)
                {
                    _Dic_MOPS = new Dictionary<int, string>();
                    _Dic_MOPS.Add(0, "遵循DO-260与DO-242");
                    _Dic_MOPS.Add(1, "遵循DO-260A与DO-242A");
                }
                return _Dic_MOPS;
            }
        }

        /// <summary>
        ///飞机工作运行状况-位置导航精准度类别： NACp
        /// </summary>
        private static Dictionary<int, string> _Dic_NACp = null;

        public static Dictionary<int, string> Dic_NACp
        {
            get
            {
                if (_Dic_NACp == null)
                {
                    _Dic_NACp = new Dictionary<int, string>();
                    _Dic_NACp.Add(0, "未知精准度");
                    _Dic_NACp.Add(1, "RNP-10精准度");
                    _Dic_NACp.Add(2, "RNP-4精准度");
                    _Dic_NACp.Add(3, "RNP-2精准度");
                    _Dic_NACp.Add(4, "RNP-1精准度");
                    _Dic_NACp.Add(5, "RNP-0.5精准度");
                    _Dic_NACp.Add(6, "RNP-0.3精准度");
                    _Dic_NACp.Add(7, "RNP-0.1精准度");
                    _Dic_NACp.Add(8, "即GPS(SA开)");
                    _Dic_NACp.Add(9, "即GPS(SA关)");
                    _Dic_NACp.Add(10, "即WAAS");
                    _Dic_NACp.Add(11, "即LAAS");
                }
                return _Dic_NACp;
            }
        }

        /// <summary>
        ///飞机工作运行状况-临视完整度级别： SIL
        /// </summary>
        private static Dictionary<int, string> _Dic_SIL = null;

        public static Dictionary<int, string> Dic_SIL
        {
            get
            {
                if (_Dic_SIL == null)
                {
                    _Dic_SIL = new Dictionary<int, string>();
                    _Dic_SIL.Add(0, "未知");
                    _Dic_SIL.Add(1, "每一飞行小时或每一次操作0.001");
                    _Dic_SIL.Add(2, "每一飞行小时或每一次操作0.00001");
                    _Dic_SIL.Add(3, "每一飞行小时或每一次操作0.0000001");
                }
                return _Dic_SIL;
            }
        }

        /// <summary>
        ///飞机工作运行状况-大气压高度完整度代码： NIC_BARO
        /// </summary>
        private static Dictionary<int, string> _Dic_NIC_BARO = null;

        public static Dictionary<int, string> Dic_NIC_BARO
        {
            get
            {
                if (_Dic_NIC_BARO == null)
                {
                    _Dic_NIC_BARO = new Dictionary<int, string>();
                    _Dic_NIC_BARO.Add(0, "空中位置消息所报告的大气压高度是基于Gilham编码输入，该输入不会与另一个压力高度源反复查对");
                    _Dic_NIC_BARO.Add(1, "空中位置消息所报告的大气压高度是基于Gilham编码输入，该输入与另一个压力高度源反复查对，并对其一致性进行了核对：或者该大气压高度是基于非Gilham编码源");
                }
                return _Dic_NIC_BARO;
            }
        }

        /// <summary>
        ///飞机工作运行状况-航迹角/航向： TRK_HDG
        /// </summary>
        private static Dictionary<int, string> _Dic_TRK_HDG = null;

        public static Dictionary<int, string> Dic_TRK_HDG
        {
            get
            {
                if (_Dic_TRK_HDG == null)
                {
                    _Dic_TRK_HDG = new Dictionary<int, string>();
                    _Dic_TRK_HDG.Add(0, "航迹角");
                    _Dic_TRK_HDG.Add(1, "航向");
                }
                return _Dic_TRK_HDG;
            }
        }

        /// <summary>
        ///飞机工作运行状况-飞机车辆长度和宽度代码： L_WType
        /// </summary>
        private static Dictionary<int, string> _Dic_L_WType = null;

        public static Dictionary<int, string> Dic_L_WType
        {
            get
            {
                if (_Dic_L_WType == null)
                {
                    _Dic_L_WType = new Dictionary<int, string>();
                    _Dic_L_WType.Add(0, "L<15,W<11.5");
                    _Dic_L_WType.Add(1, "L<15,W<23");
                    _Dic_L_WType.Add(2, "L<25,W<28.5");
                    _Dic_L_WType.Add(3, "L<25,W<34");
                    _Dic_L_WType.Add(4, "L<35,W<33");
                    _Dic_L_WType.Add(5, "L<35,W<38");
                    _Dic_L_WType.Add(6, "L<45,W<39.5");
                    _Dic_L_WType.Add(7, "L<45,W<45");
                    _Dic_L_WType.Add(8, "L<55,W<45");
                    _Dic_L_WType.Add(9, "L<55,W<52");
                    _Dic_L_WType.Add(10, "L<65,W<59.5");
                    _Dic_L_WType.Add(11, "L<65,W<67");
                    _Dic_L_WType.Add(12, "L<75,W<72.5");
                    _Dic_L_WType.Add(13, "L<75,W<80");
                    _Dic_L_WType.Add(14, "L<200,W<80");
                    _Dic_L_WType.Add(15, "L<200,W>=80");
                }
                return _Dic_L_WType;
            }
        }

        /// <summary>
        ///飞机工作运行状况-水平参考方向： HRD
        /// </summary>
        private static Dictionary<int, string> _Dic_HRD = null;

        public static Dictionary<int, string> Dic_HRD
        {
            get
            {
                if (_Dic_HRD == null)
                {
                    _Dic_HRD = new Dictionary<int, string>();
                    _Dic_HRD.Add(0, "正北");
                    _Dic_HRD.Add(1, "磁场北");
                }
                return _Dic_HRD;
            }
        }





    }
}
