using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADS_B_INFO
{
    class Ads_b_msg
    {
        public out_put_msg msg_out;
        public Ads_b_msg()
        {
            msg_out = new out_put_msg();
        }

        public struct out_put_msg
        {
            public double latitude;  //输出纬度
            public double longitude;//输出经度
            public double latitude_odd;  //奇编码输出纬度
            public double longitude_odd;//奇编码输出经度
        };
        //---------------------------------------------------------------------------------
        //  @function               : calc ADS-B msg latitude and longitude
        //  @param uintZY           ：CPR coding latitude
        //  @param uintZX           ：CPR coding longitude
        //  @param double local_lat : local latitude get through gps
        //  @param double local_lon : local longitude get through gps
        //  @param int ICAO         : the air_plan ID get through ADS_B msg
        //  @param byte cpr         : the longitude and latitude coded system type(odd or even)
        //  @return                 : the longitude and latitude out put struct
        //---------------------------------------------------------------------------------
        public out_put_msg air_local_cal(uint ZY, uint ZX, double local_lat, double local_lon, byte cpr)
        {
            int code_lat = 0;
            int code_lon = 0;
            double Dlat = 6.0;
            double Dlon = 0.0;
            double calc_cos = 0.0;
            double calc_acos = 0.0;
            double calc_l = 0.0;
            int YZ1 = 0;
            int XZ1 = 0;
            out_put_msg msg = new out_put_msg();
            if (cpr == 0)
            {
                Dlat = 6.0;
            }
            else if (cpr == 1)
            {
                Dlat = 360.0 / 59;//6.1;//6.101694915254237288135593220339
            }
            //code_lat = (int)Math.Floor(local_lat / Dlat);
            code_lat = (int)(Math.Floor(local_lat / Dlat) + Math.Floor(0.5 + (local_lat % Dlat) / Dlat - ZY / Math.Pow(2, 17)));
            calc_cos = (1 - Math.Cos(Math.PI / 30)) / (Math.Cos(Math.PI * code_lat * Dlat / 180) * Math.Cos(Math.PI * code_lat * Dlat / 180));
            calc_acos = Math.Acos(1 - calc_cos);
            calc_l = (int)Math.Floor(2 * Math.PI / calc_acos);
            Dlon = 360.0 / (Math.Floor(2 * Math.PI / (Math.Acos(1 - (1 - Math.Cos(Math.PI / 30)) / (Math.Cos(Math.PI * Math.Abs(local_lat) / 180) * Math.Cos(Math.PI * Math.Abs(local_lat) / 180))))) - cpr);
            //code_lon = (int)Math.Floor(local_lon / Dlon);
            code_lon = (int)(Math.Floor(local_lon / Dlon) + Math.Floor(0.5 + (local_lon % Dlon) / Dlon - ZX / Math.Pow(2, 17)));
            msg.latitude = code_lat * Dlat + Dlat * (ZY / Math.Pow(2, 17));
            msg.longitude = code_lon * Dlon + Dlon * (ZX / Math.Pow(2, 17));

            YZ1 = (int)(Math.Floor(Math.Pow(2, 17) * ((msg.latitude % Dlat) / Dlat) + 0.5));
            XZ1 = (int)(Math.Floor(Math.Pow(2, 17) * ((msg.longitude % Dlon) / Dlon) + 0.5));
            //msg.ICAO = ICAO;
            return msg;
        }
        public out_put_msg calc_odd_even_msg(uint YZ0, uint XZ0, uint YZ1, uint XZ1)
        {
            double temp = 0;//偶形式纬度
            double Dlat0 = 6.0; //偶编码纬度zone的大小
            double Dlat1 = 360.0 / 59; //奇编码纬度zone的大小
            double lat_odd = 0;//偶形式纬度
            double lon_odd = 0;//偶形式经度
            double lat_even = 0;
            double lon_even = 0;
            int lat_odd_zone = 0;   //奇编码所在zone序号
            int lat_even_zone = 0;  //偶编码所在zone的序号
            double Dlon0 = 0; // 偶编码经度zone大小
            double Dlon1 = 0; //奇编码经度zone大小
            int lon_even_zone = 0;
            int lon_odd_zone = 0;
            int NL0 = 0; // 对应于偶编码纬度的经度zone的个数
            int NL1 = 0; // 对应于奇编码纬度的经度zone的个数
            int lat_j = 0; //纬度索引
            int lon_m = 0; //经度索引
            int YZ2 = 0;
            int XZ2 = 0;
            out_put_msg msg = new out_put_msg();
            lat_j = (int)Math.Floor((59.0 * YZ0 - 60.0 * YZ1) / Math.Pow(2, 17) + 1.0 / 2);
            if (lat_j < 0)
            {
                //lat_odd_zone = (byte)(59 + lat_j);
                //lat_even_zone = (byte)(60 + lat_j);
                lat_odd_zone = (59 + lat_j);
                lat_even_zone = (60 + lat_j);
            }
            else
            {
                //lat_odd_zone = (byte)lat_j;
                //lat_even_zone = (byte)lat_j;
                lat_odd_zone = lat_j;
                lat_even_zone = lat_j;
            }
            temp = YZ1 / (Math.Pow(2, 17) - 1);
            lat_odd = Dlat1 * ((lat_j % 59) + YZ1 / Math.Pow(2, 17));
            double tmp1 = lat_odd;
            if (lat_odd > 90.0)
                lat_odd = lat_odd - 360.0;
            else if (lat_odd < -90.0)
                lat_odd = lat_odd + 360.0;

            lat_even = Dlat0 * ((lat_j % 60) + YZ0 / Math.Pow(2, 17));
            double tmp2 = lat_even;
            if (lat_even > 90.0)
                lat_even = lat_even - 360.0;
            else if (lat_even < -90.0)
                lat_even = lat_even + 360.0;

            //temp = Math.Cos(Math.PI * Math.Abs(lat_odd)/180);
            //temp = Math.Acos(1 - ((1 - Math.Cos(Math.PI / 30)) / (Math.Pow(Math.Cos(Math.PI * Math.Abs(lat_odd) / 180), 2))));
            //temp = 2 * Math.PI / Math.Acos(1 - ((1 - Math.Cos(Math.PI / 30)) / (Math.Pow(Math.Cos(Math.PI * Math.Abs(lat_odd) / 180), 2))));
            NL0 = (int)Math.Floor(2 * Math.PI / Math.Acos(1 - ((1 - Math.Cos(Math.PI / 30)) / (Math.Pow(Math.Cos(Math.PI * Math.Abs(lat_even) / 180), 2)))));
            NL1 = (int)Math.Floor(2 * Math.PI / Math.Acos(1 - ((1 - Math.Cos(Math.PI / 30)) / (Math.Pow(Math.Cos(Math.PI * Math.Abs(lat_odd) / 180), 2)))));
            int n0 = (int)Math.Max((double)NL0, 1.0);
            int n1 = (int)Math.Max((double)NL1 - 1, 1.0);
            if (NL0 == NL1)
            {
                Dlon0 = 360.00 / n0;
                Dlon1 = 360.00 / n1;
                lon_m = (int)Math.Floor(((double)XZ0 * (NL0 - 1) - (double)XZ1 * NL0) / Math.Pow(2, 17) + 1.0 / 2);
            }
            else { return msg; }
            if (lon_m < 0)
            {
                //lon_even_zone = (byte)(lon_m + Math.Max(NL0, 1));
                //lon_odd_zone = (byte)(lon_m + Math.Max(NL1 - 1, 1));
                lon_even_zone = (lon_m + n0);
                lon_odd_zone = (lon_m + n1);
            }
            else
            {
                //lon_even_zone = (byte)lon_m;
                //lon_odd_zone = (byte)lon_m;
                lon_even_zone = lon_m;
                lon_odd_zone = lon_m;
            }

            lon_even = Dlon0 * ((lon_m % n0) + XZ0 / Math.Pow(2, 17));
            double tmp3 = lon_even;
            if (lon_even > 180.0)
                lon_even = lon_even - 360.0;
            else if (lon_even < -180.0)
                lon_even = lon_even + 360.0;

            lon_odd = Dlon1 * ((lon_m % n1) + XZ1 / Math.Pow(2, 17));
            double tmp4 = lon_odd;
            if (lon_odd > 180.0)
                lon_odd = lon_odd - 360.0;
            else if (lon_odd < -180.0)
                lon_odd = lon_odd + 360.0;

            msg.latitude = lat_even;
            msg.longitude = lon_even;
            msg.latitude_odd = lat_odd;
            msg.longitude_odd = lon_odd;
            YZ2 = (int)(Math.Floor(Math.Pow(2, 17) * ((msg.latitude_odd % Dlat1) / Dlat1) + 0.5));
            XZ2 = (int)(Math.Floor(Math.Pow(2, 17) * ((msg.longitude_odd % Dlon1) / Dlon1) + 0.5));
            YZ2 = (int)(Math.Floor(Math.Pow(2, 17) * ((msg.latitude % Dlat0) / Dlat0) + 0.5));
            XZ2 = (int)(Math.Floor(Math.Pow(2, 17) * ((msg.longitude % Dlon0) / Dlon0) + 0.5));
            return msg;
        }
    }
}
