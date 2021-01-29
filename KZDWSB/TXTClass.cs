using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZDWSB
{
    class TXTClass
    {

        //AIS插入一行新数据
        public static string AIS_Insert(AIS_INFO.AIS_MessageInfo message, DataClass RecData)
        {
            message.longitude = (message.longitude == null ? "181" : message.longitude);
            message.latitude = (message.latitude == null ? "91" : message.latitude);

            string sql = RecData.SaveDate + '\t' + message.MMSI.ToString("x") + '\t' + message.longitude + '\t' + message.latitude +
                         '\t' + RecData.dLon.ToString("f4") + '\t' + RecData.dLat.ToString("f4") + '\t' + "0" + '\t' + RecData.power.ToString("f2")  + '\r' + '\n';
            return sql;
        }

        //ACARS插入一行新数据
        public static string ACARS_Insert(AcarsInfo.ACARS_MessageInfo message, DataClass RecData)
        {

            string sql = RecData.SaveDate + '\t' + message.ICAO + '\t' + message.FlightNumder + '\t' + message.longitude + '\t' + message.latitude +
                        '\t' + RecData.dLon.ToString("f4") + '\t' + RecData.dLat.ToString("f4") + '\t' + "0" + '\t' + RecData.power.ToString("f2")   + '\r' + '\n';
            return sql;
        }

        //ADS插入一行新数据
        public static string ADS_Insert(ADS_B_INFO.ADS_MessageInfo message, DataClass RecData)
        {
            message.longitude = (message.longitude == null ? "181" : message.longitude);
            message.latitude = (message.latitude == null ? "91" : message.latitude);
            string sql = RecData.SaveDate + '\t' + message.ICAO.ToString() + '\t' + message.Height.ToString() + '\t' + message.longitude + '\t' + message.latitude +
                         '\t' + RecData.dLon.ToString("f4") + '\t' + RecData.dLat.ToString("f4") + '\t' + "0" + '\t' + RecData.power.ToString("f2")  + '\r' + '\n';
            return sql;
        }





    
    }
}
