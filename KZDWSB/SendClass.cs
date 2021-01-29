using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace KZDWSB
{
     public class SendClass
    {
        public string DType{ get; set; }
        public string tableName { get; set; }
        public DateTime dtDate { get; set; }

        public string Alert_Flag { get; set; }
        public string Alert_Name { get; set; }


        public string fid { get; set; }
        public string version_AIS { get; set; }
        public string code_IMO { get; set; }
        public string calling { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string speed_earth { get; set; }
        public string country { get; set; }
        public string destination { get; set; }
        public string MMSI { get; set; }
        public string shipping_condition { get; set; }
        public DateTime CreatDate { get; set; }
        public string turning_speed { get; set; }
        public string course_earth { get; set; }
        public string course_real { get; set; }
        public string ID { get; set; }
        public string ship_type { get; set; }
        public string ship_type_ID { get; set; }
        public string ship_length { get; set; }
        public string ship_width { get; set; }
        public string location_type { get; set; }
        public string time_arrive { get; set; }
        public string draft_static { get; set; }

        //ACARS
        public string ALT_Height { get; set; }
        public string WD_Direction { get; set; }
        public string CAS_WindSpeed { get; set; }
        public string Nationality { get; set; }
        public string FlightCompany { get; set; }
        public string AirplaneNo { get; set; }
        public string DEP { get; set; }
        public string DES { get; set; }
        public string FlightNumder { get; set; }
        public string ETD { get; set; }
        public string ETA { get; set; }
        public string ATA { get; set; }
        public string ICAO { get; set; }
        public string FOB_OilQuantity { get; set; }


         //ADS-B数据

        //系统参数
        public int Channel { get; set; }
        public uint adICAO { get; set; }
        public int Type { get; set; }
        //空中位置消息
        public string MonitorState { get; set; }
        public string SingleAntenna { get; set; }
        public int Height { get; set; }
        public string adlongitude { get; set; }
        public string adlatitude { get; set; }
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

    }
}
