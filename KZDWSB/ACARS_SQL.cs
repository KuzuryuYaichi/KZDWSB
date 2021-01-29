using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZDWSB
{
    public class ACARS_SQL
    {

        public static AcarsInfo.ACARS_MessageInfo InitACARSMessage(AcarsInfo.ACARS_MessageInfo message)
        {

            message.ICAO = (message.ICAO == null ? "" : message.ICAO);
            message.Label = (message.Label == null ? "" : message.Label);
            message.FlightNumder = (message.FlightNumder == null ? "" : message.FlightNumder);
            message.FlightCompany = (message.FlightCompany == null ? "" : message.FlightCompany);
            message.AirplaneNo = (message.AirplaneNo == null ? "" : message.AirplaneNo);
            message.longitude = (message.longitude == null ? "" : message.longitude);
            message.latitude = (message.latitude == null ? "" : message.latitude);

            message.DEP = (message.DEP == null ? "" : message.DEP);
            message.DES = (message.DES == null ? "" : message.DES);
            message.ETD = (message.ETD == null ? "" : message.ETD);
            message.ETA = (message.ETA == null ? "" : message.ETA);
            message.ATA = (message.ATA == null ? "" : message.ATA);

            message.FOB_OilQuantity = (message.FOB_OilQuantity == null ? "" : message.FOB_OilQuantity);
            message.Nationality = (message.Nationality == null ? "" : message.Nationality);

            message.ALT_Height = (message.ALT_Height == null ? "" : message.ALT_Height);
            message.WD_Direction = (message.WD_Direction == null ? "" : message.WD_Direction);
            message.CAS_WindSpeed = (message.CAS_WindSpeed == null ? "" : message.CAS_WindSpeed);

            message.StartingPoint = (message.StartingPoint == null ? "" : message.StartingPoint);
            message.Destination = (message.Destination == null ? "" : message.Destination);

            message.RevData = (message.RevData == null ? "" : message.RevData);
            message.ErrorString = (message.ErrorString == null ? "" : message.ErrorString);

            return message;
        }


        //ACARS插入一行新数据(测试完)
        public static string Insert(AcarsInfo.ACARS_MessageInfo message, DateTime DateTime, string strDateTime, int ChanlNo, int CRC, int Send_Flag)
        {

            message = InitACARSMessage(message);

            string sql = "insert  into acars_table_" + DateTime.ToString("yyMM") + " (fid,ICAO,Label,FlightNumder,FlightCompany,AirplaneNo,longitude,latitude," +
                             "DEP,DES,ETD,ETA,ATA,FOB_OilQuantity,Nationality,ALT_Height,WD_Direction,CAS_WindSpeed,StartingPoint,Destination," +
                              "Channel,RevData,ErrorString,CreatDate,HourFlag,Error_Flag,Flag,Send_Flag) " +
                             "VALUE (UUID(),'" + message.ICAO + "','" + message.Label + "','" + message.FlightNumder + "','" + message.FlightCompany + "','" + message.AirplaneNo + "','" + message.longitude + "','" + message.latitude +
                             "','" + message.DEP + "','" + message.DES + "','" + message.ETD + "','" + message.ETA + "','" + message.ATA + "','" + message.FOB_OilQuantity + "','" + message.Nationality + "','" + message.ALT_Height + "','" + message.WD_Direction + "','" + message.CAS_WindSpeed + "','" + message.StartingPoint + "','" + message.Destination +
                             "'," + ChanlNo + ",'" + message.RevData + "','" + message.ErrorString + "', '" + strDateTime + "'," + DateTime.Hour + "," + CRC + ",0," + Send_Flag +  ")";

            return sql;
        }

        //ACARS插入一行新数据(测试完)
        public static string ImportInsert(AcarsInfo.ACARS_MessageInfo message, DateTime DateTime)
        {

            message = InitACARSMessage(message);

            string sql = "insert  into acars_table_import (fid,ICAO,Label,FlightNumder,FlightCompany,AirplaneNo,longitude,latitude," +
                             "DEP,DES,ETD,ETA,ATA,FOB_OilQuantity,Nationality,ALT_Height,WD_Direction,CAS_WindSpeed,StartingPoint,Destination," +
                              "RevData,ErrorString,CreatDate) " +
                             "VALUE (UUID(),'" + message.ICAO + "','" + message.Label + "','" + message.FlightNumder + "','" + message.FlightCompany + "','" + message.AirplaneNo + "','" + message.longitude + "','" + message.latitude +
                             "','" + message.DEP + "','" + message.DES + "','" + message.ETD + "','" + message.ETA + "','" + message.ATA + "','" + message.FOB_OilQuantity + "','" + message.Nationality + "','" + message.ALT_Height + "','" + message.WD_Direction + "','" + message.CAS_WindSpeed + "','" + message.StartingPoint + "','" + message.Destination +
                            "','" + message.RevData + "','" + message.ErrorString + "', '" + DateTime.ToString("yyyy/MM/dd HH:mm:ss") + "')";

            return sql;
        }

        //ACARS更新融合数据(测试完)
        public static string Updata_ToTable(string FID, AcarsInfo.ACARS_MessageInfo StaticsData, DateTime DateTime)
        {
            StaticsData = InitACARSMessage(StaticsData);


            string sql = "update acars_table_" + DateTime.ToString("yyMM") + " set FlightCompany='" + StaticsData.FlightCompany + "',DEP='" + StaticsData.DEP + "',DES='" + StaticsData.DES + "',ETD='" + StaticsData.ETD + "',Nationality='" + StaticsData.Nationality + "',StartingPoint='" + StaticsData.StartingPoint + "',Destination='" + StaticsData.Destination + "',Flag=1" +
                          "  where fid='" + FID + "'";

            return sql;
        }


        //ACARS_table 查询
        public static string GetSQL(string date, string strWhere, string strLimit)
        {
            string sql = "SELECT fid,Channel,acars.ICAO ,Label ,FlightNumder ,FlightCompany ,AirplaneNo ,longitude ,latitude,DEP ,StartingPoint ,DES ,Destination ,ETD ,ETA ,ATA  ,FOB_OilQuantity ,Nationality ,WD_Speed ,ALT_Height ,WD_Direction ,CAS_WindSpeed,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as Date,Error_Flag as CRC,Name,Send_Flag," +
                        " IF(ISNULL(Name) and ISNULL(country),0,1)as Alert_Flag " +

                       " from(((SELECT * from  " + date + "  " + strWhere + "  " + strLimit + "  ) as acars  " +
                       " LEFT JOIN  acarsalert_icao   on acars.ICAO =acarsalert_icao.ICAO )  " +
                       " LEFT JOIN   acarsalert_country  on  acars.Nationality=acarsalert_country.country  ) ";





            return sql;
        }

        //ACARS_table 导入查询
        //private static string ImportSearch(string date, string strWhere, string strLimit)
        //{
        //    string sql = "SELECT fid,acars.ICAO ,Label ,FlightNumder ,FlightCompany ,AirplaneNo ,longitude ,latitude,DEP ,StartingPoint ,DES ,Destination ,ETD ,ETA ,ATA  ,FOB_OilQuantity ,Nationality ,WD_Speed ,ALT_Height ,WD_Direction ,CAS_WindSpeed,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as Date,Error_Flag as CRC,Name," +
        //                " IF(ISNULL(Name) and ISNULL(country),0,1)as Alert_Flag " +

        //               " from(((SELECT * from  " + date + "  " + strWhere + "  " + strLimit + "  ) as acars  " +
        //               " LEFT JOIN  acarsalert_icao   on acars.ICAO =acarsalert_icao.ICAO )  " +
        //               " LEFT JOIN   acarsalert_country  on  acars.Nationality=acarsalert_country.country  ) ";

        //    return sql;
        //}




    }
}
