using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KZDWSB
{
    public class AIS_SQL
    {
        //(测试完)
        public static AIS_INFO.AIS_MessageInfo InitAISMessage(AIS_INFO.AIS_MessageInfo message)
        {

            message.sendtimes = (message.sendtimes == null ? "" : message.sendtimes);
            message.location_precision = (message.location_precision == null ? "" : message.location_precision);
            message.longitude = (message.longitude == null ? "" : message.longitude);
            message.latitude = (message.latitude == null ? "" : message.latitude);
            message.mark_rami = (message.mark_rami == null ? "" : message.mark_rami);
            message.shipping_condition = (message.shipping_condition == null ? "" : message.shipping_condition);
            message.turning_speed = (message.turning_speed == null ? "" : message.turning_speed);
            message.location_type = (message.location_type == null ? "" : message.location_type);

            message.version_AIS = (message.version_AIS == null ? "" : message.version_AIS);
            message.calling = (message.calling == null ? "" : message.calling);
            message.ID = (message.ID == null ? "" : message.ID);
            message.ship_type = (message.ship_type == null ? "" : message.ship_type);
            message.scale = (message.scale == null ? "" : message.scale);
            message.time_arrive = (message.time_arrive == null ? "" : message.time_arrive);
            message.destination = (message.destination == null ? "" : message.destination);
            message.date_terminal = (message.date_terminal == null ? "" : message.date_terminal);
            message.mark_resend = (message.mark_resend == null ? "" : message.mark_resend);
            message.date_binary = (message.date_binary == null ? "" : message.date_binary);

            message.information_safety = (message.information_safety == null ? "" : message.information_safety);
            message.mark_comm_condition = (message.mark_comm_condition == null ? "" : message.mark_comm_condition);
            message.navigation_type = (message.navigation_type == null ? "" : message.navigation_type);
            message.navigation_name = (message.navigation_name == null ? "" : message.navigation_name);
            message.location_offset = (message.location_offset == null ? "" : message.location_offset);

            message.send_or_receive_mode = (message.send_or_receive_mode == null ? "" : message.send_or_receive_mode);
            message.power = (message.power == null ? "" : message.power);
            message.longitude_first = (message.longitude_first == null ? "" : message.longitude_first);
            message.latitude_first = (message.latitude_first == null ? "" : message.latitude_first);
            message.longitude_second = (message.longitude_second == null ? "" : message.longitude_second);
            message.latitude_second = (message.latitude_second == null ? "" : message.latitude_second);
            message.mark_broadcast_information = (message.mark_broadcast_information == null ? "" : message.mark_broadcast_information);
            message.bandwidth_A = (message.bandwidth_A == null ? "" : message.bandwidth_A);
            message.bandwidth_B = (message.bandwidth_B == null ? "" : message.bandwidth_B);

            message.station_type = (message.station_type == null ? "" : message.station_type);
            message.report_time = (message.report_time == null ? "" : message.report_time);
            message.silence_time = (message.silence_time == null ? "" : message.silence_time);

            message.seller_ID = (message.seller_ID == null ? "" : message.seller_ID);
            message.destination_instruction_sign = (message.destination_instruction_sign == null ? "" : message.destination_instruction_sign);
            message.binary_data_sign = (message.binary_data_sign == null ? "" : message.binary_data_sign);

            message.time_internal_2_add = (message.time_internal_2_add == null ? "" : message.time_internal_2_add);
            message.time_internal_3_add = (message.time_internal_3_add == null ? "" : message.time_internal_3_add);
            message.time_internal_4_add = (message.time_internal_4_add == null ? "" : message.time_internal_4_add);
            message.time_internal_5_add = (message.time_internal_5_add == null ? "" : message.time_internal_5_add);

            message.excessive_area = (message.excessive_area == null ? "" : message.excessive_area);
            message.country = (message.country == null ? "" : message.country);

            return message;
        }

        //AIS插入一行新数据
        public static string Insert_Static(AIS_INFO.AIS_MessageInfo message, string M_B, DateTime DateTime, string strDateTime, int ChanlNo, int CRC, int Send_Flag)
        {
            message=InitAISMessage(message);

            string sql = "insert  into ais_table_static_" + DateTime.ToString("yyMM") + " (fid,ID_info_code,sendtimes,MMSI," +
                             "location_type ,version_AIS ,code_IMO,calling,ID,ship_type_ID,ship_type,scale,time_arrive, " +
                             "draft_static,destination,date_terminal, " +
                             "standby_1,standby_2, information_safety," +
                              "channel_A,channel_B,send_or_receive_mode,power,longitude_first,latitude_first,longitude_second,latitude_second,mark_broadcast_information,bandwidth_A, " +
                             "bandwidth_B,excessive_area,station_type,report_time,silence_time,selection_number,seller_ID," +
                             "ship_length,ship_width,country," +
                              "M_B,CreatDate,Channel,Error_Flag,Finish_Flag,Send_Flag,Flag) " +
                             "VALUE (UUID()," + message.ID_info_code + ",'" + message.sendtimes + "'," + message.MMSI + 
                              ",'" + message.location_type + "','" + message.version_AIS + "'," + message.code_IMO + ",'" + message.calling + "','" + message.ID + "'," + message.ship_type_ID + ",'" + message.ship_type + "','" + message.scale + "','" + message.time_arrive +
                             "','" + message.draft_static + "','" + message.destination + "','" + message.date_terminal +
                             "'," + message.standby_1 + "," + message.standby_2 + ",'" + message.information_safety +                            
                             "'," + message.channel_A + "," + message.channel_B + ",'" + message.send_or_receive_mode + "','" + message.power + "','" + message.longitude_first + "','" + message.latitude_first + "','" + message.longitude_second + "','" + message.latitude_second + "','" + message.mark_broadcast_information + "','" + message.bandwidth_A +
                             "','" + message.bandwidth_B + "','" + message.excessive_area + "','" + message.station_type + "','" + message.report_time + "','" + message.silence_time + "'," + message.selection_number + ",'" + message.seller_ID +
                             "'," + message.ship_length + "," + message.ship_width + ",'" + message.country +
                             "','" + M_B + "','" + strDateTime + "'," + ChanlNo + "," + CRC + "," + message.Finish_Flag + "," + Send_Flag +  ",0)";

            return sql;
        }
        //AIS插入一行新数据
        public static string Insert_State(AIS_INFO.AIS_MessageInfo message, string M_B, DateTime DateTime, string strDateTime, int ChanlNo, int CRC, int Send_Flag)
        {
            message = InitAISMessage(message);

            string sql = "insert  into ais_table_state_" + DateTime.ToString("yyMM") + " (fid,ID_info_code,sendtimes,MMSI,location_precision,longitude,latitude," +
                             "mark_time,mark_rami,comm_condition," +
                             "location_type ,calling,ship_type_ID,ship_type,scale," +
                             "serial_code,mark_resend,date_binary,ID_destination_MMSI_first,serial_code_first, " +
                             "ID_destination_MMSI_second,serial_code_second,ID_destination_MMSI_third,serial_code_third,ID_destination_MMSI_fourth,serial_code_fourth,standby_1,standby_2, information_safety," +
                             "ID_information_code_1_1,ID_information_code_1_2,standby_3,ID_information_code_2_1,standby_4, " +
                             "region_keep_1,mark_comm_condition," +
                             "time_offset_num_first,time_num_first,timeout_first,incremental_first,time_offset_num_second,time_num_second,timeout_second,incremental_second,time_offset_num_third, " +
                             "time_num_third,timeout_third,incremental_third,time_offset_num_fourth,time_num_fourth,timeout_fourth,incremental_fourth,navigation_type,navigation_name,location_offset, " +                             
                             "destination_instruction_sign,binary_data_sign," +
                             "time_internal_2_add,time_internal_3_add,time_internal_4_add,time_internal_5_add,ship_length,ship_width,country," +
                              "M_B,CreatDate,Channel,Error_Flag,Finish_Flag,Send_Flag,Flag) " +

                             "VALUE (UUID()," + message.ID_info_code + ",'" + message.sendtimes + "'," + message.MMSI +", '" + message.location_precision + "','" + message.longitude + "','" + message.latitude + 
                             "'," + message.mark_time + ",'" + message.mark_rami + "'," + message.comm_condition +
                              ",'" + message.location_type + "','" + message.calling + "'," + message.ship_type_ID + ",'" + message.ship_type + "','" + message.scale + 
                              "'," + message.serial_code + ",'" + message.mark_resend + "','" + message.date_binary + "'," + message.ID_destination_MMSI_first + "," + message.serial_code_first +
                             "," + message.ID_destination_MMSI_second + "," + message.serial_code_second + "," + message.ID_destination_MMSI_third + "," + message.serial_code_third + "," + message.ID_destination_MMSI_fourth + "," + message.serial_code_fourth +  "," + message.standby_1 + "," + message.standby_2 + ",'" + message.information_safety +
                             "'," + message.ID_information_code_1_1 + "," + message.ID_information_code_1_2 + "," + message.standby_3 + "," + message.ID_information_code_2_1 + "," + +message.standby_4 +
                             "," + message.region_keep_1 + ",'" + message.mark_comm_condition +
                             "'," + message.time_offset_num_first + "," + message.time_num_first + "," + message.timeout_first + "," + message.incremental_first + "," + message.time_offset_num_second + "," + message.time_num_second + "," + message.timeout_second + "," + message.incremental_second + "," + message.time_offset_num_third +
                             "," + message.time_num_third + "," + message.timeout_third + "," + message.incremental_third + "," + message.time_offset_num_fourth + "," + message.time_num_fourth + "," + message.timeout_fourth + "," + message.incremental_fourth + ",'" + message.navigation_type + "','" + message.navigation_name + "','" + message.location_offset +                            
                             "','" + message.destination_instruction_sign + "','" + message.binary_data_sign +
                             "','" + message.time_internal_2_add + "','" + message.time_internal_3_add + "','" + message.time_internal_4_add + "','" + message.time_internal_5_add + "'," + message.ship_length + "," + message.ship_width + ",'" + message.country +
                             "','" + M_B + "','" + strDateTime + "'," + ChanlNo + "," + CRC + "," + message.Finish_Flag + "," + Send_Flag +  ",0)";

            return sql;
        }
        //AIS插入一行新数据
        public static string Insert_Dynamic(AIS_INFO.AIS_MessageInfo message, string M_B, DateTime DateTime, string strDateTime, int ChanlNo, int CRC, int Send_Flag)
        {
            message = InitAISMessage(message);

            string sql = "insert into ais_table_dynamic_" + DateTime.ToString("yyMM") + " (fid,ID_info_code,sendtimes,MMSI,shipping_condition,turning_speed,speed_earth,location_precision,longitude,latitude,course_earth," +
                             "course_real,mark_time,mark_rami,comm_condition,UTC_year,UTC_month,ais_table_dynamic_" + DateTime.ToString("yyMM") + ".UTC_date,UTC_hour," +
                             "UTC_minter,UTC_seconds,location_type ,version_AIS ,code_IMO,calling,ID,ship_type_ID,ship_type,scale,time_arrive, " +
                             "draft_static,destination,date_terminal ,ID_destination_MMSI_first,altitute, " +
                             "standby_1,information_safety," + 
                             "region_keep_1,region_keep_2,mark_comm_condition," +
                             "send_or_receive_mode," +
                             "station_type,report_time,silence_time,seller_ID," +
                             "ship_length,ship_width,country," +
                              "M_B,CreatDate,Channel,Error_Flag,Finish_Flag,Send_Flag,Flag) " +

                             "VALUE (UUID()," + message.ID_info_code + ",'" + message.sendtimes + "'," + message.MMSI + ",'" + message.shipping_condition + "','" + message.turning_speed + "','" + message.speed_earth + "', '" + message.location_precision + "','" + message.longitude + "','" + message.latitude + "','" + message.course_earth +
                             "'," + message.course_real + "," + message.mark_time + ",'" + message.mark_rami + "'," + message.comm_condition + "," + message.UTC_year + "," + message.UTC_month + "," + message.UTC_date + "," + message.UTC_hour +
                             "," + message.UTC_minter + "," + message.UTC_seconds + ",'" + message.location_type + "','" + message.version_AIS + "'," + message.code_IMO + ",'" + message.calling + "','" + message.ID + "'," + message.ship_type_ID + ",'" + message.ship_type + "','" + message.scale + "','" + message.time_arrive +
                             "','" + message.draft_static + "','" + message.destination + "','" + message.date_terminal + "'," + message.ID_destination_MMSI_first + "," + message.altitute +
                            "," + message.standby_1 + ",'" + message.information_safety +
                          
                             "'," + message.region_keep_1 + "," + message.region_keep_2 + ",'" + message.mark_comm_condition +
                           
                              "','" + message.send_or_receive_mode +
                             "','" + message.station_type + "','" + message.report_time + "','" + message.silence_time + "','" + message.seller_ID + 
                             "'," + message.ship_length + "," + message.ship_width + ",'" + message.country +
                             "','" + M_B + "','" + strDateTime + "'," + ChanlNo + "," + CRC + "," + message.Finish_Flag + "," + Send_Flag + ",0)";

            return sql;
        }

        //AIS插入一行新数据
        public static string Insert(AIS_INFO.AIS_MessageInfo message, string M_B, DateTime DateTime)
        {
            message = InitAISMessage(message);

            string sql = "insert  into ais_table_import (fid,ID_info_code,sendtimes,MMSI,shipping_condition,turning_speed,speed_earth,location_precision,longitude,latitude,course_earth," +
                             "course_real,mark_time,mark_rami,comm_condition,UTC_year,UTC_month,ais_table_import.UTC_date,UTC_hour," +
                             "UTC_minter,UTC_seconds,location_type ,version_AIS ,code_IMO,calling,ID,ship_type_ID,ship_type,scale,time_arrive, " +
                             "draft_static,destination,date_terminal ,serial_code,mark_resend,date_binary,ID_destination_MMSI_first,serial_code_first, " +
                             "ID_destination_MMSI_second,serial_code_second,ID_destination_MMSI_third,serial_code_third,ID_destination_MMSI_fourth,serial_code_fourth,altitute,standby_1,standby_2, information_safety," +
                             "ID_information_code_1_1,ID_information_code_1_2,standby_3,ID_information_code_2_1,standby_4, " +
                             "region_keep_1,region_keep_2,mark_comm_condition," +
                             "time_offset_num_first,time_num_first,timeout_first,incremental_first,time_offset_num_second,time_num_second,timeout_second,incremental_second,time_offset_num_third, " +
                             "time_num_third,timeout_third,incremental_third,time_offset_num_fourth,time_num_fourth,timeout_fourth,incremental_fourth,navigation_type,navigation_name,location_offset, " +
                             "channel_A,channel_B,send_or_receive_mode,power,longitude_first,latitude_first,longitude_second,latitude_second,mark_broadcast_information,bandwidth_A, " +
                             "bandwidth_B,excessive_area,station_type,report_time,silence_time,selection_number,seller_ID,destination_instruction_sign,binary_data_sign," +
                             "time_internal_2_add,time_internal_3_add,time_internal_4_add,time_internal_5_add,ship_length,ship_width,country," +
                              "M_B,CreatDate) " +

                             "VALUE (UUID()," + message.ID_info_code + ",'" + message.sendtimes + "'," + message.MMSI + ",'" + message.shipping_condition + "','" + message.turning_speed + "','" + message.speed_earth + "', '" + message.location_precision + "','" + message.longitude + "','" + message.latitude + "','" + message.course_earth +
                             "'," + message.course_real + "," + message.mark_time + ",'" + message.mark_rami + "'," + message.comm_condition + "," + message.UTC_year + "," + message.UTC_month + "," + message.UTC_date + "," + message.UTC_hour +
                             "," + message.UTC_minter + "," + message.UTC_seconds + ",'" + message.location_type + "','" + message.version_AIS + "'," + message.code_IMO + ",'" + message.calling + "','" + message.ID + "'," + message.ship_type_ID + ",'" + message.ship_type + "','" + message.scale + "','" + message.time_arrive +
                             "','" + message.draft_static + "','" + message.destination + "','" + message.date_terminal + "'," + message.serial_code + ",'" + message.mark_resend + "','" + message.date_binary + "'," + message.ID_destination_MMSI_first + "," + message.serial_code_first +
                             "," + message.ID_destination_MMSI_second + "," + message.serial_code_second + "," + message.ID_destination_MMSI_third + "," + message.serial_code_third + "," + message.ID_destination_MMSI_fourth + "," + message.serial_code_fourth + "," + message.altitute + "," + message.standby_1 + "," + message.standby_2 + ",'" + message.information_safety +
                             "'," + message.ID_information_code_1_1 + "," + message.ID_information_code_1_2 + "," + message.standby_3 + "," + message.ID_information_code_2_1 + "," + +message.standby_4 +
                             "," + message.region_keep_1 + "," + message.region_keep_2 + ",'" + message.mark_comm_condition +
                             "'," + message.time_offset_num_first + "," + message.time_num_first + "," + message.timeout_first + "," + message.incremental_first + "," + message.time_offset_num_second + "," + message.time_num_second + "," + message.timeout_second + "," + message.incremental_second + "," + message.time_offset_num_third +
                             "," + message.time_num_third + "," + message.timeout_third + "," + message.incremental_third + "," + message.time_offset_num_fourth + "," + message.time_num_fourth + "," + message.timeout_fourth + "," + message.incremental_fourth + ",'" + message.navigation_type + "','" + message.navigation_name + "','" + message.location_offset +
                             "'," + message.channel_A + "," + message.channel_B + ",'" + message.send_or_receive_mode + "','" + message.power + "','" + message.longitude_first + "','" + message.latitude_first + "','" + message.longitude_second + "','" + message.latitude_second + "','" + message.mark_broadcast_information + "','" + message.bandwidth_A +
                             "','" + message.bandwidth_B + "','" + message.excessive_area + "','" + message.station_type + "','" + message.report_time + "','" + message.silence_time + "'," + message.selection_number + ",'" + message.seller_ID + "','" + message.destination_instruction_sign + "','" + message.binary_data_sign +
                             "','" + message.time_internal_2_add + "','" + message.time_internal_3_add + "','" + message.time_internal_4_add + "','" + message.time_internal_5_add + "'," + message.ship_length + "," + message.ship_width + ",'" + message.country +
                             "','" + M_B + "','" + DateTime.ToString("yyyy/MM/dd HH:mm:ss") + "')";

            return sql;
        }

        //AIS_table更新融合数据
        public static string Updata_ToTable(string FID, AIS_INFO.AIS_MessageInfo StaticsData, DateTime DateTime)
        {
            StaticsData = InitAISMessage(StaticsData);

            string sql = "update ais_table_dynamic_" + DateTime.ToString("yyMM") + " set version_AIS='" + StaticsData.version_AIS + "',code_IMO=" + StaticsData.code_IMO + ",calling='" + StaticsData.calling + "',ID='" + StaticsData.ID + "',ship_type_ID=" + StaticsData.ship_type_ID + ",ship_type='" + StaticsData.ship_type +
                                "',scale='" + StaticsData.scale + "',location_type='" + StaticsData.location_type + "',time_arrive='" + StaticsData.time_arrive + "',draft_static='" + StaticsData.draft_static + "',destination='" + StaticsData.destination +
                                "',date_terminal='" + StaticsData.date_terminal +
                                "',station_type='" + StaticsData.station_type + "',send_or_receive_mode='" + StaticsData.send_or_receive_mode + "',report_time='" + StaticsData.report_time + "',silence_time='" + StaticsData.silence_time +

                                "',seller_ID='" + StaticsData.seller_ID + "',ship_length=" + StaticsData.ship_length + ",ship_width=" + StaticsData.ship_width + ",country='" + StaticsData.country + "',Flag=1" +
                                " where  CreatDate >'" + DateTime.ToString("yyyy-MM-dd") + "' and CreatDate <'" + DateTime.AddDays(1).ToString("yyyy-MM-dd") + "' and fid='" + FID + "'";

            return sql;
        }


 
        public static string GetSQL(string code, string date, string strWhere, string strLimit)
        {
            string sql = "";
            //if (date == "ais_table_import")
            //{
            //    sql = GetImportSQL(code, date, strWhere, strLimit);
            //}
            //else
            //{
            //    sql = GetSearchSQL(code, date, strWhere, strLimit);

            //}

            sql = GetSearchSQL(code, date, strWhere, strLimit);
            return sql;
        }


        /*public static string GetImportSQL(string code, string StrTableName, string strWhere, string strLimit)
        {
            string SearchSql = "";
            switch (code)
            {
                case "1":
                    SearchSql = AIS_SQL.ImportSearch_1_2_3(StrTableName, strWhere, strLimit);
                    break;
                case "4":
                    SearchSql = AIS_SQL.ImportSearch_4(StrTableName, strWhere, strLimit);
                    break;
                case "5":
                    SearchSql = AIS_SQL.ImportSearch_5(StrTableName, strWhere, strLimit);
                    break;
                case "6":
                    SearchSql = AIS_SQL.ImportSearch_6(StrTableName, strWhere, strLimit);
                    break;
                case "7":
                    SearchSql = AIS_SQL.ImportSearch_7(StrTableName, strWhere, strLimit);
                    break;
                case "8":
                    SearchSql = AIS_SQL.ImportSearch_8(StrTableName, strWhere, strLimit); ;
                    break;
                case "9":
                    SearchSql = AIS_SQL.ImportSearch_9(StrTableName, strWhere, strLimit);
                    break;
                case "10":
                    SearchSql = AIS_SQL.ImportSearch_10(StrTableName, strWhere, strLimit);
                    break;
                case "11":
                    SearchSql = AIS_SQL.ImportSearch_11(StrTableName, strWhere, strLimit);
                    break;
                case "12":
                    SearchSql = AIS_SQL.ImportSearch_12(StrTableName, strWhere, strLimit);
                    break;
                case "13":
                    SearchSql = AIS_SQL.ImportSearch_13(StrTableName, strWhere, strLimit);
                    break;
                case "14":
                    SearchSql = AIS_SQL.ImportSearch_14(StrTableName, strWhere, strLimit);
                    break;
                case "15":
                    SearchSql = AIS_SQL.ImportSearch_15(StrTableName, strWhere, strLimit);
                    break;
                case "16":
                    SearchSql = AIS_SQL.ImportSearch_16(StrTableName, strWhere, strLimit);
                    break;
                case "17":
                    SearchSql = AIS_SQL.ImportSearch_17(StrTableName, strWhere, strLimit);
                    break;
                case "18":
                    SearchSql = AIS_SQL.ImportSearch_18(StrTableName, strWhere, strLimit);
                    break;
                case "19":
                    SearchSql = AIS_SQL.ImportSearch_19(StrTableName, strWhere, strLimit);
                    break;
                case "20":
                    SearchSql = AIS_SQL.ImportSearch_20(StrTableName, strWhere, strLimit);
                    break;
                case "21":
                    SearchSql = AIS_SQL.ImportSearch_21(StrTableName, strWhere, strLimit);
                    break;
                case "22":
                    SearchSql = AIS_SQL.ImportSearch_22(StrTableName, strWhere, strLimit);
                    break;
                case "23":
                    SearchSql = AIS_SQL.ImportSearch_23(StrTableName, strWhere, strLimit);
                    break;
                case "24":
                    SearchSql = AIS_SQL.ImportSearch_24(StrTableName, strWhere, strLimit);
                    break;
                case "25":
                    SearchSql = AIS_SQL.ImportSearch_25(StrTableName, strWhere, strLimit);
                    break;
                case "26":
                    SearchSql = AIS_SQL.ImportSearch_26(StrTableName, strWhere, strLimit);
                    break;
                default: break;

            }

            return SearchSql;
        }*/

        public static string GetSearchSQL(string code, string StrTableName, string strWhere, string strLimit)
        {
            string SearchSql = "";
            switch (code)
            {
                case "1":
                    SearchSql = AIS_SQL.Search_1_2_3(StrTableName, strWhere, strLimit);
                    break;
                case "4":
                    SearchSql = AIS_SQL.Search_4(StrTableName, strWhere, strLimit);
                    break;
                case "5":
                    SearchSql = AIS_SQL.Search_5(StrTableName, strWhere, strLimit);
                    break;
                case "6":
                    SearchSql = AIS_SQL.Search_6(StrTableName, strWhere, strLimit); ;
                    break;
                case "7":
                    SearchSql = AIS_SQL.Search_7(StrTableName, strWhere, strLimit);
                    break;
                case "8":
                    SearchSql = AIS_SQL.Search_8(StrTableName, strWhere, strLimit);
                    break;
                case "9":
                    SearchSql = AIS_SQL.Search_9(StrTableName, strWhere, strLimit);
                    break;
                case "10":
                    SearchSql = AIS_SQL.Search_10(StrTableName, strWhere, strLimit);
                    break;
                case "11":
                    SearchSql = AIS_SQL.Search_11(StrTableName, strWhere, strLimit);
                    break;
                case "12":
                    SearchSql = AIS_SQL.Search_12(StrTableName, strWhere, strLimit);
                    break;
                case "13":
                    SearchSql = AIS_SQL.Search_13(StrTableName, strWhere, strLimit);
                    break;
                case "14":
                    SearchSql = AIS_SQL.Search_14(StrTableName, strWhere, strLimit);
                    break;
                case "15":
                    SearchSql = AIS_SQL.Search_15(StrTableName, strWhere, strLimit);
                    break;
                case "16":
                    SearchSql = AIS_SQL.Search_16(StrTableName, strWhere, strLimit);
                    break;
                case "17":
                    SearchSql = AIS_SQL.Search_17(StrTableName, strWhere, strLimit);
                    break;
                case "18":
                    SearchSql = AIS_SQL.Search_18(StrTableName, strWhere, strLimit);
                    break;
                case "19":
                    SearchSql = AIS_SQL.Search_19(StrTableName, strWhere, strLimit);
                    break;
                case "20":
                    SearchSql = AIS_SQL.Search_20(StrTableName, strWhere, strLimit);
                    break;
                case "21":
                    SearchSql = AIS_SQL.Search_21(StrTableName, strWhere, strLimit);
                    break;
                case "22":
                    SearchSql = AIS_SQL.Search_22(StrTableName, strWhere, strLimit);
                    break;
                case "23":
                    SearchSql = AIS_SQL.Search_23(StrTableName, strWhere, strLimit);
                    break;
                case "24":
                    SearchSql = AIS_SQL.Search_24(StrTableName, strWhere, strLimit);
                    break;
                case "25":
                    SearchSql = AIS_SQL.Search_25(StrTableName, strWhere, strLimit);
                    break;
                case "26":
                    SearchSql = AIS_SQL.Search_26(StrTableName, strWhere, strLimit);
                    break;
                default: break;

            }


            return SearchSql;
        }


        #region AIS导入查询语句
        //AIS_table 查询INFO_1_2_3
        public static string ImportSearch_1_2_3(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区," +
                                "shipping_condition as 航行状态,turning_speed as 转向率,speed_earth as 对地航速,location_precision as 船位精确度,longitude as 经度,latitude as 纬度,course_earth as 对地航向,course_real as 真航向,mark_time as 时间标记, " +
                                 "version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,seller_ID as 卖主ID,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,scale as 尺寸,location_type as 定位装置类型,station_type as 台站类型,send_or_receive_mode as 收发模式,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,report_time as 报告时间,silence_time as 寂静时间,ship_length as 船长度,ship_width as 船宽度," +
                                "region_keep_1 as 地区性保留,standby_1 as 备用位,mark_rami as RAIM标志,comm_condition as 通信状态,M_B, DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +

                        " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code in(1,2,3)  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_4
        public static string ImportSearch_4(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区," +
                               "UTC_year as UTC年份,UTC_month as UTC月份,ais.UTC_date as UTC日期,UTC_hour as UTC小时,UTC_minter as UTC分钟,UTC_seconds as UTC秒,location_precision as 船位精确度,longitude as 经度,latitude as 纬度, location_type as 定位装置类型," +
                               "version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,seller_ID as 卖主ID,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,scale as 尺寸,location_type as 定位装置类型,station_type as 台站类型,send_or_receive_mode as 收发模式,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,report_time as 报告时间,silence_time as 寂静时间,ship_length as 船长度,ship_width as 船宽度," +
                               "standby_1 as 备用位,mark_rami as RAIM标志,comm_condition as 通信状态,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                        " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =4  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_5
        public static string ImportSearch_5(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,ship_type as 船舶及载货类型,scale as 尺寸,ship_length as 船长度,ship_width as 船宽度,location_type as 定位装置类型,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,standby_1 as 备用位,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =5  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_6
        public static string ImportSearch_6(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,serial_code as 序列号码 ,ID_destination_MMSI_first as 目的台识别码,mark_resend as 重发标志,standby_1 as 备用码,date_binary as 二进制数据,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =6  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_7
        public static string ImportSearch_7(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,standby_1 as 备用码,ID_destination_MMSI_first as ID1 ,serial_code_first as ID1序列号,ID_destination_MMSI_second as ID2 ,serial_code_second as ID2序列号,ID_destination_MMSI_third as ID3 ,serial_code_third as ID3序列号,ID_destination_MMSI_fourth as ID4 ,serial_code_fourth as ID4序列号,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =7  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_8
        public static string ImportSearch_8(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,standby_1 as 备用码,date_binary as 二进制数据,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =8  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_9
        public static string ImportSearch_9(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区," +
            "altitute as 高度海拔,speed_earth as 对地航速,location_precision as 位置精准度, longitude as 经度,latitude as 纬度,course_earth as 对地航向, mark_time as 时间标记," +
            "version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,seller_ID as 卖主ID,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,scale as 尺寸,location_type as 定位装置类型,station_type as 台站类型,send_or_receive_mode as 收发模式,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,report_time as 报告时间,silence_time as 寂静时间,ship_length as 船长度,ship_width as 船宽度," +
            "region_keep_1 as 地区性保留,date_terminal as 数据终端,standby_1 as 备用位,mark_rami as RAIM标志,comm_condition as 通信状态,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =9  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_10
        public static string ImportSearch_10(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,ID_destination_MMSI_first as 目的台识别码 ," +
                           "version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,seller_ID as 卖主ID,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,scale as 尺寸,location_type as 定位装置类型,station_type as 台站类型,send_or_receive_mode as 收发模式,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,report_time as 报告时间,silence_time as 寂静时间,ship_length as 船长度,ship_width as 船宽度," +
                           "standby_1 as 备用码1,standby_2 as 备用码2,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                  " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =10  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_11
        public static string ImportSearch_11(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区," +
                        "UTC_year as UTC年份,UTC_month as UTC月份,ais.UTC_date as UTC日期,UTC_hour as UTC小时,UTC_minter as UTC分钟,UTC_seconds as UTC秒,location_precision as 船位精确度,longitude as 经度,latitude as 纬度, location_type as 定位装置类型," +
                        "version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,seller_ID as 卖主ID,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,scale as 尺寸,location_type as 定位装置类型,station_type as 台站类型,send_or_receive_mode as 收发模式,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,report_time as 报告时间,silence_time as 寂静时间,ship_length as 船长度,ship_width as 船宽度," +
                        "standby_1 as 备用位,mark_rami as RAIM标志,comm_condition as 通信状态,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =11  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_12
        public static string ImportSearch_12(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,ID_destination_MMSI_first as 目的台识别码 ,mark_resend as 重发标志,standby_1 as 备用码1,information_safety as 安全信息内容,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                   " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =12  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_13
        public static string ImportSearch_13(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,standby_1 as 备用码,ID_destination_MMSI_first as ID1 ,serial_code_first as ID1序列号,ID_destination_MMSI_second as ID2 ,serial_code_second as ID2序列号,ID_destination_MMSI_third as ID3 ,serial_code_third as ID3序列号,ID_destination_MMSI_fourth as ID4 ,serial_code_fourth as ID4序列号,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =13  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_14
        public static string ImportSearch_14(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,standby_1 as 备用码1,information_safety as 安全信息内容,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                   " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =14  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_15
        public static string ImportSearch_15(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI,ais.country as 国家地区,standby_1 as 备用码1,ID_destination_MMSI_first as 目的台识别码ID1,ID_information_code_1_1 as 信息识别码ID1_1,time_offset_num_first as 时隙偏移1_1,standby_2 as 备用码2,ID_information_code_1_2 as 信息识别码ID1_2,time_offset_num_second as 时隙偏移1_2,standby_3 as 备用码3,ID_destination_MMSI_second as 目的台识别码ID2,ID_information_code_2_1 as 信息识别码ID2_1,time_offset_num_third as 时隙偏移2_1,standby_4 as 备用码4,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =15  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_16
        public static string ImportSearch_16(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,standby_1 as 备用码1,ID_destination_MMSI_first as 目的台识别码IDA,time_offset_num_first as 时隙偏移A,incremental_first as 增量A,ID_destination_MMSI_second as 目的台识别码IDB,time_offset_num_second as 时隙偏移B,incremental_second as 增量B,standby_2 as 备用码2,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =16  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_17
        public static string ImportSearch_17(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,longitude as 经度,latitude as 纬度,information_safety as 数据," +
                 "version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,seller_ID as 卖主ID,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,scale as 尺寸,location_type as 定位装置类型,station_type as 台站类型,send_or_receive_mode as 收发模式,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,report_time as 报告时间,silence_time as 寂静时间,ship_length as 船长度,ship_width as 船宽度," +
                    "standby_1 as 备用码1,standby_2 as 备用码2,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =17  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_18
        public static string ImportSearch_18(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI,ais.country as 国家地区," +
            "speed_earth as 对地航速,location_precision as 船位精确度,longitude as 经度,latitude as 纬度,course_earth as 对地航向,course_real as 真航向,mark_time as 时间标记," +
               "version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,seller_ID as 卖主ID,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,scale as 尺寸,location_type as 定位装置类型,station_type as 台站类型,send_or_receive_mode as 收发模式,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,report_time as 报告时间,silence_time as 寂静时间,ship_length as 船长度,ship_width as 船宽度," +
            "region_keep_1 as 地区性保留1,region_keep_2 as 地区性保留2,standby_1 as 备用位,mark_rami as RAIM标志,mark_comm_condition as 通信状态标志,comm_condition as 通信状态,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =18  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_19
        public static string ImportSearch_19(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区," +
            "speed_earth as 对地航速,location_precision as 船位精确度,longitude as 经度,latitude as 纬度,course_earth as 对地航向COG ,course_real as 真航向,mark_time as 时间标记," +
              "version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,seller_ID as 卖主ID,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,scale as 尺寸,location_type as 定位装置类型,station_type as 台站类型,send_or_receive_mode as 收发模式,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,report_time as 报告时间,silence_time as 寂静时间,ship_length as 船长度,ship_width as 船宽度," +
            "region_keep_1 as 地区性保留1,region_keep_2 as 地区性保留2,mark_rami as RAIM标志,standby_1 as 备用位,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                   " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =19  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_20
        public static string ImportSearch_20(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,standby_1 as 备用码1,time_offset_num_first as 时隙偏移数1,time_num_first as 时隙数量1,timeout_first as 超时1,incremental_first as 增量1,time_offset_num_second as 时隙偏移数2,time_num_second as 时隙数量2,timeout_second as 超时2,incremental_second as 增量2,time_offset_num_third as 时隙偏移数3,time_num_third as 时隙数量3,timeout_third as 超时3,incremental_third as 增量3,time_offset_num_fourth as 时隙偏移数4,time_num_fourth as 时隙数量4,timeout_fourth as 超时4,incremental_fourth as 增量4,standby_2 as 备用码2,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                   " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =20  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_21
        public static string ImportSearch_21(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,navigation_type as 助航类型,navigation_name as 助航名称,location_precision as 船位精确度,longitude as 经度,latitude as 纬度,scale as 尺度位置参照,ship_length as 船长度,ship_width as 船宽度,location_type as 定位装置类型,mark_time as 时间标记,location_offset as 偏移位置标志,region_keep_1 as 地区性保留,mark_rami as RAIM标志,standby_1 as 备用码,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =21  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_22
        public static string ImportSearch_22(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI,ais.country as 国家地区,standby_1 as 备用码,channel_A  as 信道A,channel_B as 信道B,send_or_receive_mode as 收发模式,power as 功率 ,longitude_first as 经度1,latitude_first as 纬度1,longitude_second as 经度2 ,latitude_second as 纬度2,mark_broadcast_information as 编址和广播信息标志,bandwidth_A as 信道A带宽,bandwidth_B as 信道B带宽,excessive_area as 过渡区大小,standby_2 as 备用码2,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                   " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =22  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_23
        public static string ImportSearch_23(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI,ais.country as 国家地区," +
                   " version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,seller_ID as 卖主ID,ship_type as 船舶及载货类型,scale as 尺寸,location_type as 定位装置类型,station_type as 台站类型,send_or_receive_mode as 收发模式,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,longitude_first as 经度1,latitude_first as 纬度1,longitude_second as 经度2 ,latitude_second as 纬度2,report_time as 报告时间,silence_time as 寂静时间,ship_length as 船长度,ship_width as 船宽度," +
                  " standby_1 as 备用码,standby_2 as 备用码2,standby_3 as 备用码3,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =23  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_24
        public static string ImportSearch_24(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,selection_number as 识别码,ID as 名称,ship_type as 船舶及载货类型,seller_ID as 卖主ID,calling as 呼号,scale as 尺寸,ship_length as 船长度,ship_width as 船宽度,standby_1 as 备用码,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =24  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_25
        public static string ImportSearch_25(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,destination_instruction_sign as 目的地指示符,binary_data_sign as 二进制数据标记,ID_destination_MMSI_first as 目的地ID,date_binary as 二进制数据,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =25  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_26
        public static string ImportSearch_26(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,destination_instruction_sign as 目的地指示符,binary_data_sign as 二进制数据标记,ID_destination_MMSI_first as 目的地ID,date_binary as 二进制数据,time_internal_2_add as 第2时隙二进制数据增加,time_internal_3_add as 第3时隙二进制数据增加,time_internal_4_add as 第4时隙二进制数据增加,time_internal_5_add as 第5时隙二进制数据增加,mark_comm_condition as 通信状态选择标志,comm_condition as 通信状态,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =26  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }
        #endregion


        #region AIS查询语句
        //AIS_table 查询INFO_1_2_3
        public static string Search_1_2_3(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区," +
                                "shipping_condition as 航行状态,turning_speed as 转向率,speed_earth as 对地航速,location_precision as 船位精确度,longitude as 经度,latitude as 纬度,course_earth as 对地航向,course_real as 真航向,mark_time as 时间标记, " +
                                 "version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,seller_ID as 卖主ID,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,scale as 尺寸,location_type as 定位装置类型,station_type as 台站类型,send_or_receive_mode as 收发模式,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,report_time as 报告时间,silence_time as 寂静时间,ship_length as 船长度,ship_width as 船宽度," +
                                "region_keep_1 as 地区性保留,standby_1 as 备用位,mark_rami as RAIM标志,comm_condition as 通信状态,M_B, DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +

                        " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code in(1,2,3)  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_4
        public static string Search_4(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区," +
                               "UTC_year as UTC年份,UTC_month as UTC月份,ais.UTC_date as UTC日期,UTC_hour as UTC小时,UTC_minter as UTC分钟,UTC_seconds as UTC秒,location_precision as 船位精确度,longitude as 经度,latitude as 纬度, location_type as 定位装置类型," +
                               "version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,seller_ID as 卖主ID,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,scale as 尺寸,location_type as 定位装置类型,station_type as 台站类型,send_or_receive_mode as 收发模式,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,report_time as 报告时间,silence_time as 寂静时间,ship_length as 船长度,ship_width as 船宽度," +
                               "standby_1 as 备用位,mark_rami as RAIM标志,comm_condition as 通信状态,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                        " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =4  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_5
        public static string Search_5(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,scale as 尺寸,ship_length as 船长度,ship_width as 船宽度,location_type as 定位装置类型,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,standby_1 as 备用位,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =5  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_6
        public static string Search_6(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,serial_code as 序列号码 ,ID_destination_MMSI_first as 目的台识别码,mark_resend as 重发标志,standby_1 as 备用码,date_binary as 二进制数据,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =6  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_7
        public static string Search_7(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,standby_1 as 备用码,ID_destination_MMSI_first as ID1 ,serial_code_first as ID1序列号,ID_destination_MMSI_second as ID2 ,serial_code_second as ID2序列号,ID_destination_MMSI_third as ID3 ,serial_code_third as ID3序列号,ID_destination_MMSI_fourth as ID4 ,serial_code_fourth as ID4序列号,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =7  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_8
        public static string Search_8(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,standby_1 as 备用码,date_binary as 二进制数据,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =8  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_9
        public static string Search_9(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区," +                 
            "altitute as 高度海拔,speed_earth as 对地航速,location_precision as 位置精准度, longitude as 经度,latitude as 纬度,course_earth as 对地航向, mark_time as 时间标记,"+
            "version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,seller_ID as 卖主ID,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,scale as 尺寸,location_type as 定位装置类型,station_type as 台站类型,send_or_receive_mode as 收发模式,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,report_time as 报告时间,silence_time as 寂静时间,ship_length as 船长度,ship_width as 船宽度," +
            "region_keep_1 as 地区性保留,date_terminal as 数据终端,standby_1 as 备用位,mark_rami as RAIM标志,comm_condition as 通信状态,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =9  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_10
        public static string Search_10(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,ID_destination_MMSI_first as 目的台识别码 ," +
                           "version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,seller_ID as 卖主ID,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,scale as 尺寸,location_type as 定位装置类型,station_type as 台站类型,send_or_receive_mode as 收发模式,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,report_time as 报告时间,silence_time as 寂静时间,ship_length as 船长度,ship_width as 船宽度," +
                           "standby_1 as 备用码1,standby_2 as 备用码2,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                  " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =10  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_11
        public static string Search_11(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区," +
                        "UTC_year as UTC年份,UTC_month as UTC月份,ais.UTC_date as UTC日期,UTC_hour as UTC小时,UTC_minter as UTC分钟,UTC_seconds as UTC秒,location_precision as 船位精确度,longitude as 经度,latitude as 纬度, location_type as 定位装置类型," +
                        "version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,seller_ID as 卖主ID,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,scale as 尺寸,location_type as 定位装置类型,station_type as 台站类型,send_or_receive_mode as 收发模式,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,report_time as 报告时间,silence_time as 寂静时间,ship_length as 船长度,ship_width as 船宽度," +
                        "standby_1 as 备用位,mark_rami as RAIM标志,comm_condition as 通信状态,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =11  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_12
        public static string Search_12(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,ID_destination_MMSI_first as 目的台识别码 ,mark_resend as 重发标志,standby_1 as 备用码1,information_safety as 安全信息内容,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                   " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =12  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_13
        public static string Search_13(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,standby_1 as 备用码,ID_destination_MMSI_first as ID1 ,serial_code_first as ID1序列号,ID_destination_MMSI_second as ID2 ,serial_code_second as ID2序列号,ID_destination_MMSI_third as ID3 ,serial_code_third as ID3序列号,ID_destination_MMSI_fourth as ID4 ,serial_code_fourth as ID4序列号,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =13  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_14
        public static string Search_14(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,standby_1 as 备用码1,information_safety as 安全信息内容,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                   " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =14  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_15
        public static string Search_15(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI,ais.country as 国家地区,standby_1 as 备用码1,ID_destination_MMSI_first as 目的台识别码ID1,ID_information_code_1_1 as 信息识别码ID1_1,time_offset_num_first as 时隙偏移1_1,standby_2 as 备用码2,ID_information_code_1_2 as 信息识别码ID1_2,time_offset_num_second as 时隙偏移1_2,standby_3 as 备用码3,ID_destination_MMSI_second as 目的台识别码ID2,ID_information_code_2_1 as 信息识别码ID2_1,time_offset_num_third as 时隙偏移2_1,standby_4 as 备用码4,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =15  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_16
        public static string Search_16(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,standby_1 as 备用码1,ID_destination_MMSI_first as 目的台识别码IDA,time_offset_num_first as 时隙偏移A,incremental_first as 增量A,ID_destination_MMSI_second as 目的台识别码IDB,time_offset_num_second as 时隙偏移B,incremental_second as 增量B,standby_2 as 备用码2,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =16  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_17
        public static string Search_17(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,longitude as 经度,latitude as 纬度,information_safety as 数据," +
                 "version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,seller_ID as 卖主ID,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,scale as 尺寸,location_type as 定位装置类型,station_type as 台站类型,send_or_receive_mode as 收发模式,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,report_time as 报告时间,silence_time as 寂静时间,ship_length as 船长度,ship_width as 船宽度," +
                    "standby_1 as 备用码1,standby_2 as 备用码2,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =17  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_18
        public static string Search_18(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI,ais.country as 国家地区," +            
            "speed_earth as 对地航速,location_precision as 船位精确度,longitude as 经度,latitude as 纬度,course_earth as 对地航向,course_real as 真航向,mark_time as 时间标记,"+
               "version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,seller_ID as 卖主ID,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,scale as 尺寸,location_type as 定位装置类型,station_type as 台站类型,send_or_receive_mode as 收发模式,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,report_time as 报告时间,silence_time as 寂静时间,ship_length as 船长度,ship_width as 船宽度," +
            "region_keep_1 as 地区性保留1,region_keep_2 as 地区性保留2,standby_1 as 备用位,mark_rami as RAIM标志,mark_comm_condition as 通信状态标志,comm_condition as 通信状态,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =18  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_19
        public static string Search_19(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区," +              
            "speed_earth as 对地航速,location_precision as 船位精确度,longitude as 经度,latitude as 纬度,course_earth as 对地航向COG ,course_real as 真航向,mark_time as 时间标记,"+
              "version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,seller_ID as 卖主ID,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,scale as 尺寸,location_type as 定位装置类型,station_type as 台站类型,send_or_receive_mode as 收发模式,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,report_time as 报告时间,silence_time as 寂静时间,ship_length as 船长度,ship_width as 船宽度," +
            "region_keep_1 as 地区性保留1,region_keep_2 as 地区性保留2,mark_rami as RAIM标志,standby_1 as 备用位,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                   " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =19  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_20
        public static string Search_20(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,standby_1 as 备用码1,time_offset_num_first as 时隙偏移数1,time_num_first as 时隙数量1,timeout_first as 超时1,incremental_first as 增量1,time_offset_num_second as 时隙偏移数2,time_num_second as 时隙数量2,timeout_second as 超时2,incremental_second as 增量2,time_offset_num_third as 时隙偏移数3,time_num_third as 时隙数量3,timeout_third as 超时3,incremental_third as 增量3,time_offset_num_fourth as 时隙偏移数4,time_num_fourth as 时隙数量4,timeout_fourth as 超时4,incremental_fourth as 增量4,standby_2 as 备用码2,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                   " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =20  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_21
        public static string Search_21(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,navigation_type as 助航类型,navigation_name as 助航名称,location_precision as 船位精确度,longitude as 经度,latitude as 纬度,scale as 尺度位置参照,ship_length as 船长度,ship_width as 船宽度,location_type as 定位装置类型,mark_time as 时间标记,location_offset as 偏移位置标志,region_keep_1 as 地区性保留,mark_rami as RAIM标志,standby_1 as 备用码,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =21  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_22
        public static string Search_22(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI,ais.country as 国家地区,standby_1 as 备用码,channel_A  as 信道A,channel_B as 信道B,send_or_receive_mode as 收发模式,power as 功率 ,longitude_first as 经度1,latitude_first as 纬度1,longitude_second as 经度2 ,latitude_second as 纬度2,mark_broadcast_information as 编址和广播信息标志,bandwidth_A as 信道A带宽,bandwidth_B as 信道B带宽,excessive_area as 过渡区大小,standby_2 as 备用码2,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                   " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =22  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_23
        public static string Search_23(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI,ais.country as 国家地区," +
                   " version_AIS as AIS版本 ,code_IMO as IMO号码,calling as 呼号,ID as 名称,seller_ID as 卖主ID,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,scale as 尺寸,location_type as 定位装置类型,station_type as 台站类型,send_or_receive_mode as 收发模式,time_arrive as 预计到达时间,draft_static as 吃水深度,destination as 目的地,date_terminal as  数据终端指示符,longitude_first as 经度1,latitude_first as 纬度1,longitude_second as 经度2 ,latitude_second as 纬度2,report_time as 报告时间,silence_time as 寂静时间,ship_length as 船长度,ship_width as 船宽度," +
                  " standby_1 as 备用码,standby_2 as 备用码2,standby_3 as 备用码3,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =23  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
                  return sql;
        }

        //AIS_table 查询INFO_24
        public static string Search_24(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,selection_number as 识别码,ID as 名称,ship_type as 船舶及载货类型,ship_type_ID as 船舶编号,seller_ID as 卖主ID,calling as 呼号,scale as 尺寸,ship_length as 船长度,ship_width as 船宽度,standby_1 as 备用码,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =24  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_25
        public static string Search_25(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,destination_instruction_sign as 目的地指示符,binary_data_sign as 二进制数据标记,ID_destination_MMSI_first as 目的地ID,date_binary as 二进制数据,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =25  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }

        //AIS_table 查询INFO_26
        public static string Search_26(string date, string strWhere, string strLimit)
        {
            string sql = "select fid,ID_info_code as 消息识别码,sendtimes as 转发指示符,ais.MMSI ,ais.country as 国家地区,destination_instruction_sign as 目的地指示符,binary_data_sign as 二进制数据标记,ID_destination_MMSI_first as 目的地ID,date_binary as 二进制数据,time_internal_2_add as 第2时隙二进制数据增加,time_internal_3_add as 第3时隙二进制数据增加,time_internal_4_add as 第4时隙二进制数据增加,time_internal_5_add as 第5时隙二进制数据增加,mark_comm_condition as 通信状态选择标志,comm_condition as 通信状态,M_B,DATE_FORMAT(CreatDate,'%Y/%m/%d %T') as 接收时间 ,Error_Flag as CRC校验,Send_Flag as 发送状态,Name as 告警名称,IF(ISNULL(Name) and ISNULL(aisalert_country.country),0,1)as 告警标志 " +
                                    " from   (((SELECT * from " + date + "  where " + strWhere + " and  ID_info_code =26  ORDER BY CreatDate desc   " + strLimit + " ) as ais  " +
            " LEFT JOIN  aisalert_mmsi   on ais.MMSI =aisalert_mmsi.MMSI )   " +
            " LEFT JOIN   aisalert_country  on  ais.country=aisalert_country.country  ) ";
            return sql;
        }
        #endregion

    
    }
}
