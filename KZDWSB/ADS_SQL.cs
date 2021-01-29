using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace KZDWSB
{
    public class ADS_SQL
    {

        //ADS_B插入一行动态数据
        public static string Insert(ADS_B_INFO.ADS_MessageInfo message, DateTime dateTime, string Datetime, int ChanlNo, int CRC, int Send_Flag)
        {
            message.MonitorState = (message.MonitorState == null ? "" : message.MonitorState);
            message.SingleAntenna = (message.SingleAntenna == null ? "" : message.SingleAntenna);
            message.longitude = (message.longitude == null ? "" : message.longitude);
            message.latitude = (message.latitude == null ? "" : message.latitude);

            message.AirPlaneID = (message.AirPlaneID == null ? "" : message.AirPlaneID);
            message.WorkingState = (message.WorkingState == null ? "" : message.WorkingState);
            message.PathStateFlag = (message.PathStateFlag == null ? "" : message.PathStateFlag);
            message.PathState = (message.PathState == null ? "" : message.PathState);

            message.AimChange = (message.AimChange == null ? "" : message.AimChange);
            message.IFR = (message.IFR == null ? "" : message.IFR);
            message.AirSpeedType = (message.AirSpeedType == null ? "" : message.AirSpeedType);
            message.VerticalSpeedSource = (message.VerticalSpeedSource == null ? "" : message.VerticalSpeedSource);
            message.VerticalSpeedFlag = (message.VerticalSpeedFlag == null ? "" : message.VerticalSpeedFlag);
            message.VerticalSpeed = (message.VerticalSpeed == null ? "" : message.VerticalSpeed);
            message.AtmosphereFlag = (message.AtmosphereFlag == null ? "" : message.AtmosphereFlag);
            message.Atmosphere = (message.Atmosphere == null ? "" : message.Atmosphere);

            message.AtmosphereDataSource = (message.AtmosphereDataSource == null ? "" : message.AtmosphereDataSource);
            message.HeightType = (message.HeightType == null ? "" : message.HeightType);
            message.HeightProperty = (message.HeightProperty == null ? "" : message.HeightProperty);
            message.AtmosphereMode = (message.AtmosphereMode == null ? "" : message.AtmosphereMode);
            message.LevelDataSource = (message.LevelDataSource == null ? "" : message.LevelDataSource);
            message.TargetDirectionFlag = (message.TargetDirectionFlag == null ? "" : message.TargetDirectionFlag);
            message.LevelMode = (message.LevelMode == null ? "" : message.LevelMode);
            message.PropertyCode = (message.PropertyCode == null ? "" : message.PropertyCode);
            message.EmergencyCode = (message.EmergencyCode == null ? "" : message.EmergencyCode);

            message.CC_CDTI = (message.CC_CDTI == null ? "" : message.CC_CDTI);
            message.CC_ARV = (message.CC_ARV == null ? "" : message.CC_ARV);
            message.CC_TS = (message.CC_TS == null ? "" : message.CC_TS);
            message.CC_TC = (message.CC_TC == null ? "" : message.CC_TC);
            message.CC_POA = (message.CC_POA == null ? "" : message.CC_POA);
            message.CC_B2Low = (message.CC_B2Low == null ? "" : message.CC_B2Low);
            message.OM_RA = (message.OM_RA == null ? "" : message.OM_RA);
            message.OM_IDENT = (message.OM_IDENT == null ? "" : message.OM_IDENT);
            message.OM_ATC = (message.OM_ATC == null ? "" : message.OM_ATC);
            message.MOPS = (message.MOPS == null ? "" : message.MOPS);
            message.NACp = (message.NACp == null ? "" : message.NACp);
            message.SIL = (message.SIL == null ? "" : message.SIL);
            message.NIC_BARO = (message.NIC_BARO == null ? "" : message.NIC_BARO);
            message.L_WType = (message.L_WType == null ? "" : message.L_WType);
            message.TRK_HDG = (message.TRK_HDG == null ? "" : message.TRK_HDG);
            message.HRD = (message.HRD == null ? "" : message.HRD);


            string sql = "insert  into ads_table_"+ dateTime.ToString("yyMM") + " (fid,ICAO,Type,MonitorState,SingleAntenna,Height,longitude,latitude," +
                             "AirPlaneID,WorkingState,PathStateFlag,PathState,SubType,AimChange,IFR,AirDirection,AirSpeedType,AirSpeed,VerticalSpeedSource,VerticalSpeedFlag," +
                             "VerticalSpeed,AtmosphereFlag,Atmosphere,AtmosphereDataSource,HeightType,HeightProperty,AtmosphereMode,TargetHeight,LevelDataSource,TargetDirection," +
                             "TargetDirectionFlag,LevelMode,PropertyCode,EmergencyCode,CC_CDTI,CC_ARV,CC_TS,CC_TC,CC_POA,CC_B2Low," +
                             "OM_Type,OM_RA,OM_IDENT,OM_ATC,MOPS,NACp,SIL,NIC_BARO,L_WType,TRK_HDG,HRD," +
                              "Channel,CreatDate,Error_Flag,Flag,Send_Flag) " +
                             "VALUE (UUID()," + message.ICAO + "," + message.Type + ",'" + message.MonitorState + "','" + message.SingleAntenna + "'," + message.Height + ",'" + message.longitude + "','" + message.latitude +
                             "','" + message.AirPlaneID + "','" + message.WorkingState + "','" + message.PathStateFlag + "','" + message.PathState + "'," + message.SubType + ",'" + message.AimChange + "','" + message.IFR + "','" + message.AirDirection + "','" + message.AirSpeedType + "','" + message.AirSpeed + "','" + message.VerticalSpeedSource + "','" + message.VerticalSpeedFlag +
                             "','" + message.VerticalSpeed + "','" + message.AtmosphereFlag + "','" + message.Atmosphere + "','" + message.AtmosphereDataSource + "','" + message.HeightType + "','" + message.HeightProperty + "','" + message.AtmosphereMode + "'," + message.TargetHeight + ",'" + message.LevelDataSource + "'," + message.TargetDirection +
                             ",'" + message.TargetDirectionFlag + "','" + message.LevelMode + "','" + message.PropertyCode + "','" + message.EmergencyCode + "','" + message.CC_CDTI + "','" + message.CC_ARV + "','" + message.CC_TS + "','" + message.CC_TC + "','" + message.CC_POA + "','" + message.CC_B2Low +
                             "'," + message.OM_Type + ",'" + message.OM_RA + "','" + message.OM_IDENT + "','" + message.OM_ATC + "','" + message.MOPS + "','" + message.NACp + "','" + message.SIL + "','" + message.NIC_BARO + "','" + message.L_WType + "','" + message.TRK_HDG + "','" + message.HRD +                                         
                             "','" + ChanlNo + "','" + Datetime + "'," + CRC + ",0," + Send_Flag + ")";

            return sql;
        }

        //ADS_B插入一行静态数据
        public static string Insert_Static(ADS_B_INFO.ADS_MessageInfo message, DateTime dateTime, string Datetime, int ChanlNo, int CRC, int Send_Flag)
        {
            message.AirPlaneID = (message.AirPlaneID == null ? "" : message.AirPlaneID);
            message.CC_CDTI = (message.CC_CDTI == null ? "" : message.CC_CDTI);
            message.CC_ARV = (message.CC_ARV == null ? "" : message.CC_ARV);
            message.CC_TS = (message.CC_TS == null ? "" : message.CC_TS);
            message.CC_TC = (message.CC_TC == null ? "" : message.CC_TC);
            message.CC_POA = (message.CC_POA == null ? "" : message.CC_POA);
            message.CC_B2Low = (message.CC_B2Low == null ? "" : message.CC_B2Low);
            message.OM_RA = (message.OM_RA == null ? "" : message.OM_RA);
            message.OM_IDENT = (message.OM_IDENT == null ? "" : message.OM_IDENT);
            message.OM_ATC = (message.OM_ATC == null ? "" : message.OM_ATC);
            message.MOPS = (message.MOPS == null ? "" : message.MOPS);
            message.NACp = (message.NACp == null ? "" : message.NACp);
            message.SIL = (message.SIL == null ? "" : message.SIL);
            message.NIC_BARO = (message.NIC_BARO == null ? "" : message.NIC_BARO);
            message.TRK_HDG = (message.TRK_HDG == null ? "" : message.TRK_HDG);
            message.HRD = (message.HRD == null ? "" : message.HRD);

            string sql = "insert  into ads_table_static_" + dateTime.ToString("yyMM") + " (fid,ICAO,Type," +
                             "AirPlaneID," +
                             "CC_CDTI,CC_ARV,CC_TS,CC_TC,CC_POA,CC_B2Low," +
                             "OM_Type,OM_RA,OM_IDENT,OM_ATC,MOPS,NACp,SIL,NIC_BARO,TRK_HDG,HRD," +
                              "Channel,CreatDate,Error_Flag,Flag,Send_Flag) " +
                             "VALUE (UUID()," + message.ICAO + "," + message.Type +
                             ",'" + message.AirPlaneID +
                             "','" + message.CC_CDTI + "','" + message.CC_ARV + "','" + message.CC_TS + "','" + message.CC_TC + "','" + message.CC_POA + "','" + message.CC_B2Low +
                             "'," + message.OM_Type + ",'" + message.OM_RA + "','" + message.OM_IDENT + "','" + message.OM_ATC + "','" + message.MOPS + "','" + message.NACp + "','" + message.SIL + "','" + message.NIC_BARO + "','" + message.TRK_HDG + "','" + message.HRD +
                             "','" + ChanlNo + "','" + Datetime + "'," + CRC + ",0," + Send_Flag + ")";

            return sql;
        }


        //ADS_B更新融合数据
        public static string Updata_ToTable(string FID, ADS_B_INFO.ADS_MessageInfo StaticsData, DateTime DateTime)
        {
            StaticsData.AirPlaneID = (StaticsData.AirPlaneID == null ? "" : StaticsData.AirPlaneID);
            StaticsData.CC_CDTI = (StaticsData.CC_CDTI == null ? "" : StaticsData.CC_CDTI);
            StaticsData.CC_ARV = (StaticsData.CC_ARV == null ? "" : StaticsData.CC_ARV);
            StaticsData.CC_TS = (StaticsData.CC_TS == null ? "" : StaticsData.CC_TS);
            StaticsData.CC_TC = (StaticsData.CC_TC == null ? "" : StaticsData.CC_TC);
            StaticsData.CC_POA = (StaticsData.CC_POA == null ? "" : StaticsData.CC_POA);
            StaticsData.CC_B2Low = (StaticsData.CC_B2Low == null ? "" : StaticsData.CC_B2Low);

            StaticsData.L_WType = (StaticsData.L_WType == null ? "" : StaticsData.L_WType);


            string sql = "update ads_table_" + DateTime.ToString("yyMM") + " set AirPlaneID='" + StaticsData.AirPlaneID + "',CC_CDTI='" + StaticsData.CC_CDTI + "',CC_ARV='" + StaticsData.CC_ARV + "',CC_TS='" + StaticsData.CC_TS + "',CC_TC='" + StaticsData.CC_TC + "',CC_POA='" + StaticsData.CC_POA + "',CC_B2Low='" + StaticsData.CC_B2Low + "',L_WType='" + StaticsData.L_WType + "',Flag=1" +
                          " where  CreatDate >'" + DateTime.ToString("yyyy-MM-dd") + "' and CreatDate <'" + DateTime.AddDays(1).ToString("yyyy-MM-dd") + "' and fid='" + FID + "'";

            return sql;
        }

        #region ADS查询语句
        //ADS_table 查询   飞机身份
        public static string Search_AirPlaneID(string strName, string strWhere, string strLimit)
        {
            string sql = "select fid,ICAO,Type as 消息类型,AirPlaneID as 飞机ID ," +
                                " DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Error_Flag as CRC校验  " +
                                "from (select * from  " + strName + " where  " + strWhere + " and  Type in(1,2,3,4) ORDER BY CreatDate desc " + strLimit + " ) as ads ";

            return sql;
        }
        //ADS_table 查询     空中位置消息
        public static string Search_AirLocation(string strName, string strWhere, string strLimit)
        {
            string sql = "select fid,ICAO,Type as 消息类型,MonitorState as 监视状况 ,SingleAntenna as 单天线," +
                                "Height as 飞行高度,longitude as 经度,latitude as 纬度, " +
                                " DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Error_Flag as CRC校验  "+
                                "from (select * from  " + strName + " where  " + strWhere + " and  Type in(9,10,11,12,13,14,15,16,17,18,20,21,22) ORDER BY CreatDate desc " + strLimit + " ) as ads ";
            return sql;
        }
        //ADS_table 查询     地表面位置消息
        public static string Search_EarthLocation(string strName, string strWhere, string strLimit)
        {
            string sql = "select fid,ICAO,Type as 消息类型,WorkingState as 运行 ,PathStateFlag as 航向_地面航迹状况,PathState as 航向_地面航迹," +
                                "longitude as 经度,latitude as 纬度, " +
                                " DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Error_Flag as CRC校验  "+
                                "from (select * from  " + strName + " where  " + strWhere + " and  Type in(5,6,7,8) ORDER BY CreatDate desc " + strLimit + " ) as ads ";
            return sql;
        }
        //ADS_table 查询     空中速度消息
        public static string Search_AirSpeed(string strName, string strWhere, string strLimit)
        {
            string sql = "select fid,ICAO,Type as 消息类型,SubType as 子类型 ,AirDirection as 飞机航向,AirSpeedType as 空速类型,AirSpeed as 飞机航速," +
                                "AimChange as 意图变更能力,IFR as IFR能力, VerticalSpeedSource as 垂直速度源,VerticalSpeedFlag as 垂直速度符号, VerticalSpeed as 垂直速度,AtmosphereFlag as 大气压高度差符号,Atmosphere as 大气压高度差, " +
                                " DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Error_Flag as CRC校验  "+
                                "from (select * from  " + strName + " where  " + strWhere + " and  Type = 19 ORDER BY CreatDate desc " + strLimit + " ) as ads ";
            return sql;
        }
        //ADS_table 查询     目标状态与状况
        public static string Search_TargetState(string strName, string strWhere, string strLimit)
        {
            string sql = "select fid,ICAO,Type as 消息类型,SubType as 子类型 ,AtmosphereDataSource as 垂直数据可用源指示器,TargetDirectionFlag as 目标高度类型,HeightProperty as 目标高度性能," +
                                "AtmosphereMode as 垂直模式指示器,TargetHeight as 目标高度, LevelDataSource as 水平数据可用源指示器,TargetDirection as 目标航向_航迹角, TargetDirectionFlag as 目标航向_航迹指示器,LevelMode as 水平模式指示器,PropertyCode as 性能_模式编码,EmergencyCode as  应急_优先," +
                                " DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Error_Flag as CRC校验  "+
                                "from (select * from  " + strName + " where  " + strWhere + " and  Type = 29 ORDER BY CreatDate desc " + strLimit + " ) as ads ";
            return sql;
        }
        //ADS_table 查询     飞机运行状况
        public static string Search_AirCondition(string strName, string strWhere, string strLimit)
        {
            //L_WType as 飞机_车辆长度和宽度代码,
            string sql = "select fid,ICAO,Type as 消息类型,CC_CDTI as CDTI交通显示性能 ,CC_ARV as ARV报告功能,CC_TS as TS报告功能,CC_TC as TC报告功能,CC_POA  as 位置偏移应用,CC_B2Low as B2Low,MOPS as  MOPS版本号," +
                                "OM_RA as RA激活,OM_IDENT as IDENT开关激活, OM_ATC as 接收ATC服务,NACp as 位置导航精确度类别, SIL as 监视完整度级别,NIC_BARO as 大气压高度完整度代码,TRK_HDG as 航迹角_航向,HRD as 水平参考方向," +
                                " DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Error_Flag as CRC校验  "+
                                "from (select * from  " + strName + " where  " + strWhere + " and  Type = 31 ORDER BY CreatDate desc " + strLimit + " ) as ads ";
           
            return sql;
        }
        //ADS_table 查询     扩展间歇震荡飞机状况消息
        public static string Search_PauseAirCondition(string strName, string strWhere, string strLimit)
        {
            string sql = "select fid,ICAO,Type as 消息类型,EmergencyCode as 紧急_优先状况 ," +                       
                                " DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Error_Flag as CRC校验  "+
                                "from (select * from  " + strName + " where  " + strWhere + " and  Type = 28 ORDER BY CreatDate desc " + strLimit + " ) as ads ";
            return sql;
        }

        #endregion


        //ADS_table 查询
        public static string GetSQL(string code, string strName, string strWhere, string strLimit)
        {
            string sql = "";

            switch(code)
            {
                case "0":
                    sql = Search_AirPlaneID(strName, strWhere, strLimit);
                    break;
                case "1":
                    sql = Search_AirLocation(strName, strWhere, strLimit);                    
                    break;
                case "2":
                    sql = Search_EarthLocation(strName, strWhere, strLimit);
                    break;
                case "3":
                    sql = Search_AirSpeed(strName, strWhere, strLimit);
                    break;
                case "6":
                    sql = Search_PauseAirCondition(strName, strWhere, strLimit);
                    break;
                case "4":
                    sql = Search_TargetState(strName, strWhere, strLimit);
                    break; 
                case "5":
                    sql = Search_AirCondition(strName, strWhere, strLimit);
                    break;
                default: break;
            }
            return sql;
        }


    }
}
