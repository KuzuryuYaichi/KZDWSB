using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;
using System.Configuration;
using System.IO;

namespace KZDWSB
{
    public class DataFormat
    {
        //计数器
        public static ushort packNum = 1;
        public static UInt32 frameNum = 1;
        public static UInt32 aisCount = 19999999;
        public static UInt32 acarsCount = 59999999;


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public struct TargetIdentification
        {
            public uint head;  //0xa5a5a5a5
            public uint length; //报文长度
            public ushort deviceNum;  //设备编号 3979
            public ushort PH; //批号
            public uint produceDate; //从2000年1月1日起的天数，负数表示2000年以前的日期，无效值为0
            public uint produceTime; //以当天00:00:00为基准，单位：1毫秒，无效值为0
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public char[] ICAO; //ICAO地址码
            public int planeType; //机型
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
            public char[] flightNumber; //航班号
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] registrationNum; //注册号
            public int area; //国家 地区
            public ushort flyCode; //飞行代码
            public int IFF; //敌我属性 目标敌我属性，1-不明；2-我方；3-地方；4-友方；5-中立；6-敌方同盟；7-未识别
            public double bak1; //经度 度
            public double bak2; //纬度 度
            public double bak3; //高度 米
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public char[] bak;
        };

        private static int DateDiff(DateTime dateStart, DateTime dateEnd)
        {
            DateTime start = Convert.ToDateTime(dateStart.ToShortDateString());
            DateTime end = Convert.ToDateTime(dateEnd.ToShortDateString());
            TimeSpan sp = end.Subtract(start);
            return sp.Days;
        }
        public static int getMSecond(DateTime mSecond)
        {
            TimeSpan ts = mSecond.TimeOfDay;
            int sp = (int)ts.TotalMilliseconds;
            return sp;
        }
        private static char[] Convertchar(string strc)
        {
            uint ver = Convert.ToUInt32(strc);
            byte[] hex = new byte[3];
            hex[0] = (byte)(ver & 0xFF);
            hex[1] = (byte)((ver >> 8) & 0xFF);
            hex[2] = (byte)((ver >> 16) & 0xFF);
            char[] tmp = new char[6];
            string str = null;
            for (int i = hex.Length - 1; i >= 0; i--)
            {
                str += hex[i].ToString("X2");
            }
            for (int i = 0; i < tmp.Length; i++)
            {
                tmp[i] = str[i];
            }
            return tmp;
        }
        public static byte[] StructToBytes(object structObj)
        {
            int size = Marshal.SizeOf(structObj);
            byte[] bytes = new byte[size];
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structObj, structPtr, false);
            Marshal.Copy(structPtr, bytes, 0, size);
            Marshal.FreeHGlobal(structPtr);
            return bytes;
        }
        public static string GetCountryName(int idcode)
        {
            string c = "无";
            if (Dic_CountryName.ContainsKey(idcode))
            {
                c = Dic_CountryName[idcode];
            }
            return c;
        }
        #region ICAO国籍信息

        //string转换为byte[]
        public static byte[] StringToByteArray(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < hexString.Length / 2; i++)
            {
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return returnBytes;
        }

        //byte[]转换为string
        public static string ByteArrayToString(byte[] buffer)
        {
            string hexString = string.Empty;

            if (buffer != null)
            {
                StringBuilder strB = new StringBuilder();
                for (int i = 0; i < buffer.Length; i++)
                {
                    strB.Append(buffer[i].ToString("X2"));
                    strB.Append(" ");
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        //byte[]转换为二进制string
        public static string ByteArrayToString_2(byte[] buffer)
        {
            string hexString = string.Empty;

            if (buffer != null)
            {
                StringBuilder strB = new StringBuilder();
                for (int i = 0; i < buffer.Length; i++)
                {
                    strB.Append(Convert.ToString(buffer[i], 2).PadLeft(8, '0'));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        public static string UintToString_2(uint data)
        {
            string hexString = string.Empty;
            StringBuilder strb = new StringBuilder();
            hexString = Convert.ToString(data, 2).PadLeft(24, '0');

            return hexString;
        }
        public static string GetICAOCountryName(uint num)
        {
            string c = "无";
            KeyValuePair<string, string> d;
            for (int i = 0; i < Dic_AdsCountryName.Count; i++)
            {
                d = Dic_AdsCountryName.ElementAt(i);
                string istr = UintToString_2(num).Substring(0, d.Key.Length);
                if (string.Equals(istr, d.Key))
                {
                    c = d.Value;
                    return c;
                }

            }
            return c;
        }
        private static Dictionary<string , string> _Dic_AdsCountryName = null;

        public static Dictionary<string , string> Dic_AdsCountryName
        {
            get
            {
                if (_Dic_AdsCountryName == null)
                {
                    _Dic_AdsCountryName = new Dictionary<string, string>();
                    _Dic_AdsCountryName.Add("011100000000", "Afghanistan");
                    _Dic_AdsCountryName.Add("01010000000100", "Albania");
                    _Dic_AdsCountryName.Add("000010100", "Algeria");
                    _Dic_AdsCountryName.Add("000010010000", "Angola");
                    _Dic_AdsCountryName.Add("00001100101000", "AntiguaandBarbuda");
                    _Dic_AdsCountryName.Add("111000", "Argentina");
                    _Dic_AdsCountryName.Add("01100000000000", "Armenia");
                    _Dic_AdsCountryName.Add("011111", "Australia");
                    _Dic_AdsCountryName.Add("010001000", "Austria");
                    _Dic_AdsCountryName.Add("01100000000010", "Azerbaijan");
                    _Dic_AdsCountryName.Add("000010101000", "Bahamas");
                    _Dic_AdsCountryName.Add("100010010100", "Bahrain");
                    _Dic_AdsCountryName.Add("011100000010", "Bangladesh");
                    _Dic_AdsCountryName.Add("00001010101000", "Barbados");
                    _Dic_AdsCountryName.Add("01010001000000", "Belarus");
                    _Dic_AdsCountryName.Add("010001001", "Belgium");
                    _Dic_AdsCountryName.Add("00001010101100", "Belize");
                    _Dic_AdsCountryName.Add("00001001010000", "Benin");
                    _Dic_AdsCountryName.Add("01101000000000", "Bhutan");
                    _Dic_AdsCountryName.Add("111010010100", "Bolivia");
                    _Dic_AdsCountryName.Add("01010001001100", "BosniaandHerzegovina");
                    _Dic_AdsCountryName.Add("00000011000000", "Botswana");
                    _Dic_AdsCountryName.Add("111001", "Brazil");
                    _Dic_AdsCountryName.Add("10001001010100", "BruneiDarussalam");
                    _Dic_AdsCountryName.Add("010001010", "Bulgaria");
                    _Dic_AdsCountryName.Add("000010011100", "BurkinaFaso");
                    _Dic_AdsCountryName.Add("000000110010", "Burundi");
                    _Dic_AdsCountryName.Add("011100001110", "Cambodia");
                    _Dic_AdsCountryName.Add("000000110100", "Cameroon");
                    _Dic_AdsCountryName.Add("110000", "Canada");
                    _Dic_AdsCountryName.Add("00001001011000", "CapeVerde");
                    _Dic_AdsCountryName.Add("000001101100", "CentralAfricanRepublic");
                    _Dic_AdsCountryName.Add("000010000100", "Chad");
                    _Dic_AdsCountryName.Add("111010000000", "Chile");
                    _Dic_AdsCountryName.Add("011110", "China");
                    _Dic_AdsCountryName.Add("000010101100", "Colombia");
                    _Dic_AdsCountryName.Add("00000011010100", "Comoros");
                    _Dic_AdsCountryName.Add("000000110110", "Congo");
                    _Dic_AdsCountryName.Add("10010000000100", "CookIslands");
                    _Dic_AdsCountryName.Add("000010101110", "CostaRica");
                    _Dic_AdsCountryName.Add("000000111000", "CôtedIvoire");
                    _Dic_AdsCountryName.Add("01010000000111", "Croatia");
                    _Dic_AdsCountryName.Add("000010110000", "Cuba");
                    _Dic_AdsCountryName.Add("01001100100000", "Cyprus");
                    _Dic_AdsCountryName.Add("010010011", "CzechRepublic");
                    _Dic_AdsCountryName.Add("011100100", "DemocraticPeople'sRepublicofKorea");
                    _Dic_AdsCountryName.Add("000010001100", "DemocraticRepublicoftheCongo");
                    _Dic_AdsCountryName.Add("010001011", "Denmark");
                    _Dic_AdsCountryName.Add("00001001100000", "Djibouti");
                    _Dic_AdsCountryName.Add("000011000100", "DominicanRepublic");
                    _Dic_AdsCountryName.Add("111010000100", "Ecuador");
                    _Dic_AdsCountryName.Add("000000010", "Egypt");
                    _Dic_AdsCountryName.Add("000010110010", "ElSalvador");
                    _Dic_AdsCountryName.Add("000001000010", "EquatorialGuinea");
                    _Dic_AdsCountryName.Add("00100000001000", "Eritrea");
                    _Dic_AdsCountryName.Add("01010001000100", "Estonia");
                    _Dic_AdsCountryName.Add("000001000000", "Ethiopia");
                    _Dic_AdsCountryName.Add("110010001000", "Fiji");
                    _Dic_AdsCountryName.Add("010001100", "Finland");
                    _Dic_AdsCountryName.Add("001110", "France");
                    _Dic_AdsCountryName.Add("000000111110", "Gabon");
                    _Dic_AdsCountryName.Add("000010011010", "Gambia");
                    _Dic_AdsCountryName.Add("01010001010000", "Georgia");
                    _Dic_AdsCountryName.Add("001111", "Germany");
                    _Dic_AdsCountryName.Add("000001000100", "Ghana");
                    _Dic_AdsCountryName.Add("010001101", "Greece");
                    _Dic_AdsCountryName.Add("00001100110000", "Grenada");
                    _Dic_AdsCountryName.Add("000010110100", "Guatemala");
                    _Dic_AdsCountryName.Add("000001000110", "Guinea");
                    _Dic_AdsCountryName.Add("00000100100000", "GuineaBissau");
                    _Dic_AdsCountryName.Add("000010110110", "Guyana");
                    _Dic_AdsCountryName.Add("000010111000", "Haiti");
                    _Dic_AdsCountryName.Add("000010111010", "Honduras");
                    _Dic_AdsCountryName.Add("010001110", "Hungary");
                    _Dic_AdsCountryName.Add("010011001100", "Iceland");
                    _Dic_AdsCountryName.Add("100000", "India");
                    _Dic_AdsCountryName.Add("100010100", "Indonesia");
                    _Dic_AdsCountryName.Add("011100110", "Iran,IslamicRepublicof");
                    _Dic_AdsCountryName.Add("011100101", "Iraq");
                    _Dic_AdsCountryName.Add("010011001010", "Ireland");
                    _Dic_AdsCountryName.Add("011100111", "Israel");
                    _Dic_AdsCountryName.Add("001100", "Italy");
                    _Dic_AdsCountryName.Add("000010111110", "Jamaica");
                    _Dic_AdsCountryName.Add("100001", "Japan");
                    _Dic_AdsCountryName.Add("011101000", "Jordan");
                    _Dic_AdsCountryName.Add("01101000001100", "Kazakhstan");
                    _Dic_AdsCountryName.Add("000001001100", "Kenya");
                    _Dic_AdsCountryName.Add("11001000111000", "Kiribati");
                    _Dic_AdsCountryName.Add("011100000110", "Kuwait");
                    _Dic_AdsCountryName.Add("01100000000100", "Kyrgyzstan");
                    _Dic_AdsCountryName.Add("011100001000", "LaoPeople'sDemocraticRepublic");
                    _Dic_AdsCountryName.Add("01010000001011", "Latvia");
                    _Dic_AdsCountryName.Add("011101001", "Lebanon");
                    _Dic_AdsCountryName.Add("00000100101000", "Lesotho");
                    _Dic_AdsCountryName.Add("000001010000", "Liberia");
                    _Dic_AdsCountryName.Add("000000011", "LibyanArabJamahiriya");
                    _Dic_AdsCountryName.Add("01010000001111", "Lithuania");
                    _Dic_AdsCountryName.Add("01001101000000", "Luxembourg");
                    _Dic_AdsCountryName.Add("000001010100", "Madagascar");
                    _Dic_AdsCountryName.Add("000001011000", "Malawi");
                    _Dic_AdsCountryName.Add("011101010", "Malaysia");
                    _Dic_AdsCountryName.Add("00000101101000", "Maldives");
                    _Dic_AdsCountryName.Add("000001011100", "Mali");
                    _Dic_AdsCountryName.Add("01001101001000", "Malta");
                    _Dic_AdsCountryName.Add("10010000000000", "MarshallIslands");
                    _Dic_AdsCountryName.Add("00000101111000", "Mauritania");
                    _Dic_AdsCountryName.Add("00000110000000", "Mauritius");
                    _Dic_AdsCountryName.Add("000011010", "Mexico");
                    _Dic_AdsCountryName.Add("01101000000100", "Micronesia,FederatedStatesof");
                    _Dic_AdsCountryName.Add("01001101010000", "Monaco");
                    _Dic_AdsCountryName.Add("01101000001000", "Mongolia");
                    _Dic_AdsCountryName.Add("000000100", "Morocco");
                    _Dic_AdsCountryName.Add("000000000110", "Mozambique");
                    _Dic_AdsCountryName.Add("011100000100", "Myanmar");
                    _Dic_AdsCountryName.Add("00100000000100", "Namibia");
                    _Dic_AdsCountryName.Add("11001000101000", "Nauru");
                    _Dic_AdsCountryName.Add("011100001010", "Nepal");
                    _Dic_AdsCountryName.Add("010010000", "Netherlands,Kingdomofthe");
                    _Dic_AdsCountryName.Add("110010000", "NewZealand");
                    _Dic_AdsCountryName.Add("000011000000", "Nicaragua");
                    _Dic_AdsCountryName.Add("000001100010", "Niger");
                    _Dic_AdsCountryName.Add("000001100100", "Nigeria");
                    _Dic_AdsCountryName.Add("010001111", "Norway");
                    _Dic_AdsCountryName.Add("01110000110000", "Oman");
                    _Dic_AdsCountryName.Add("011101100", "Pakistan");
                    _Dic_AdsCountryName.Add("01101000010000", "Palau");
                    _Dic_AdsCountryName.Add("000011000010", "Panama");
                    _Dic_AdsCountryName.Add("100010011000", "PapuaNewGuinea");
                    _Dic_AdsCountryName.Add("111010001000", "Paraguay");
                    _Dic_AdsCountryName.Add("111010001100", "Peru");
                    _Dic_AdsCountryName.Add("011101011", "Philippines");
                    _Dic_AdsCountryName.Add("010010001", "Poland");
                    _Dic_AdsCountryName.Add("010010010", "Portugal");
                    _Dic_AdsCountryName.Add("00000110101000", "Qatar");
                    _Dic_AdsCountryName.Add("011100011", "RepublicofKorea");
                    _Dic_AdsCountryName.Add("01010000010011", "RepublicofMoldova");
                    _Dic_AdsCountryName.Add("010010100", "Romania");
                    _Dic_AdsCountryName.Add("0001", "RussianFederation");
                    _Dic_AdsCountryName.Add("000001101110", "Rwanda");
                    _Dic_AdsCountryName.Add("11001000110000", "SaintLucia");
                    _Dic_AdsCountryName.Add("00001011110000", "SaintVincentandtheGrenadines");
                    _Dic_AdsCountryName.Add("10010000001000", "Samoa");
                    _Dic_AdsCountryName.Add("01010000000000", "SanMarino");
                    _Dic_AdsCountryName.Add("00001001111000", "SaoTomeandPrincipe");
                    _Dic_AdsCountryName.Add("011100010", "SaudiArabia");
                    _Dic_AdsCountryName.Add("000001110000", "Senegal");
                    _Dic_AdsCountryName.Add("00000111010000", "Seychelles");
                    _Dic_AdsCountryName.Add("00000111011000", "SierraLeone");
                    _Dic_AdsCountryName.Add("011101101", "Singapore");
                    _Dic_AdsCountryName.Add("01010000010111", "Slovakia");
                    _Dic_AdsCountryName.Add("01010000011011", "Slovenia");
                    _Dic_AdsCountryName.Add("10001001011100", "SolomonIslands");
                    _Dic_AdsCountryName.Add("000001111000", "Somalia");
                    _Dic_AdsCountryName.Add("000000001", "SouthAfrica");
                    _Dic_AdsCountryName.Add("001101", "Spain");
                    _Dic_AdsCountryName.Add("011101110", "SriLanka");
                    _Dic_AdsCountryName.Add("000001111100", "Sudan");
                    _Dic_AdsCountryName.Add("000011001000", "Suriname");
                    _Dic_AdsCountryName.Add("00000111101000", "Swaziland");
                    _Dic_AdsCountryName.Add("010010101", "Sweden");
                    _Dic_AdsCountryName.Add("010010110", "Switzerland");
                    _Dic_AdsCountryName.Add("011101111", "SyrianArabRepublic");
                    _Dic_AdsCountryName.Add("01010001010100", "Tajikistan");
                    _Dic_AdsCountryName.Add("100010000", "Thailand");
                    _Dic_AdsCountryName.Add("01010001001000", "TheformerYugoslavRepublicofMacedonia");
                    _Dic_AdsCountryName.Add("000010001000", "Togo");
                    _Dic_AdsCountryName.Add("11001000110100", "Tonga");
                    _Dic_AdsCountryName.Add("000011000110", "TrinidadandTobago");
                    _Dic_AdsCountryName.Add("000000101", "Tunisia");
                    _Dic_AdsCountryName.Add("010010111", "Turkey");
                    _Dic_AdsCountryName.Add("01100000000110", "Turkmenistan");
                    _Dic_AdsCountryName.Add("000001101000", "Uganda");
                    _Dic_AdsCountryName.Add("010100001", "Ukraine");
                    _Dic_AdsCountryName.Add("100010010110", "UnitedArabEmirates");
                    _Dic_AdsCountryName.Add("010000", "UnitedKingdom");
                    _Dic_AdsCountryName.Add("000010000000", "UnitedRepublicofTanzania");
                    _Dic_AdsCountryName.Add("1010", "UnitedStates");
                    _Dic_AdsCountryName.Add("111010010000", "Uruguay");
                    _Dic_AdsCountryName.Add("01010000011111", "Uzbekistan");
                    _Dic_AdsCountryName.Add("11001001000000", "Vanuatu");
                    _Dic_AdsCountryName.Add("000011011", "Venezuela");
                    _Dic_AdsCountryName.Add("100010001", "VietNam");
                    _Dic_AdsCountryName.Add("100010010000", "Yemen");
                    _Dic_AdsCountryName.Add("000010001010", "Zambia");
                    _Dic_AdsCountryName.Add("00000000010000", "Zimbabwe");
                    _Dic_AdsCountryName.Add("010011000", "Yugoslavia");
                }
                  
                return _Dic_AdsCountryName;
            }
        }

        #endregion

        private static Dictionary<int, string> _Dic_CountryName = null;

        public static Dictionary<int, string> Dic_CountryName
        {
            get
            {
                if (_Dic_CountryName == null)
                {
                    _Dic_CountryName = new Dictionary<int, string>();
                    _Dic_CountryName.Add(0x501000, "阿尔巴尼亚");
                    _Dic_CountryName.Add(0xA0000, "阿尔及利亚");
                    _Dic_CountryName.Add(0x700000, "阿富汗");
                    _Dic_CountryName.Add(0xE00000, "阿根廷");
                    _Dic_CountryName.Add(0x896000, "阿联酋");
                    _Dic_CountryName.Add(0x70C000, "阿曼 ");
                    _Dic_CountryName.Add(0x600800, "阿塞拜疆 ");
                    _Dic_CountryName.Add(0x10000, "埃及");
                    _Dic_CountryName.Add(0x40000, "埃塞俄比亚");
                    _Dic_CountryName.Add(0x4CA000, "爱尔兰");
                    _Dic_CountryName.Add(0x511000, "爱沙尼亚");
                    _Dic_CountryName.Add(0x90000, "安哥拉");
                    _Dic_CountryName.Add(0xCA000, "安提瓜和巴布达");
                    _Dic_CountryName.Add(0x440000, "奥地利");
                    _Dic_CountryName.Add(0x7C0000, "澳大利亚");
                    _Dic_CountryName.Add(0xAA000, "巴巴多斯");
                    _Dic_CountryName.Add(0x898000, "巴布亚新几内亚");
                    _Dic_CountryName.Add(0xA8000, "巴哈马");
                    _Dic_CountryName.Add(0x760000, "巴基斯坦");
                    _Dic_CountryName.Add(0xE88000, "巴拉圭");
                    _Dic_CountryName.Add(0x894000, "巴林");
                    _Dic_CountryName.Add(0xC2000, "巴拿马");
                    _Dic_CountryName.Add(0xE40000, "巴西");
                    _Dic_CountryName.Add(0x510000, "白俄罗斯");
                    _Dic_CountryName.Add(0x450000, "保加利亚");
                    _Dic_CountryName.Add(0x94000, "贝宁");
                    _Dic_CountryName.Add(0x448000, "比利时");
                    _Dic_CountryName.Add(0x4CC000, "冰岛");
                    _Dic_CountryName.Add(0xE94000, "玻利维亚");
                    _Dic_CountryName.Add(0x513000, "波黑");
                    _Dic_CountryName.Add(0x488000, "波兰");
                    _Dic_CountryName.Add(0x30000, "博茨瓦纳");
                    _Dic_CountryName.Add(0xAB000, "伯利兹");
                    _Dic_CountryName.Add(0x680000, "不丹");
                    _Dic_CountryName.Add(0x9C000, "布基纳法索");
                    _Dic_CountryName.Add(0x32000, "布隆迪");
                    _Dic_CountryName.Add(0x42000, "赤道几内亚");
                    _Dic_CountryName.Add(0x458000, "丹麦");
                    _Dic_CountryName.Add(0x3C0000, "德国");
                    _Dic_CountryName.Add(0x88000, "多哥");
                    _Dic_CountryName.Add(0xC4000, "多米尼加");
                    _Dic_CountryName.Add(0x100000, "俄罗斯");
                    _Dic_CountryName.Add(0xE84000, "厄瓜多尔");
                    _Dic_CountryName.Add(0x202000, "厄立特里亚");
                    _Dic_CountryName.Add(0x380000, "法国");
                    _Dic_CountryName.Add(0x758000, "菲律宾 ");
                    _Dic_CountryName.Add(0x460000, "芬兰");
                    _Dic_CountryName.Add(0x96000, "佛得角");
                    _Dic_CountryName.Add(0x9A000, "冈比亚");
                    _Dic_CountryName.Add(0x36000, "刚果（布）");
                    _Dic_CountryName.Add(0xAC000, "哥伦比亚");
                    _Dic_CountryName.Add(0xAE000, "哥斯达黎加");
                    _Dic_CountryName.Add(0xCC000, "格林纳达");
                    _Dic_CountryName.Add(0x514000, "格鲁吉亚");
                    _Dic_CountryName.Add(0xB0000, "古巴");
                    _Dic_CountryName.Add(0xB6000, "圭亚那");
                    _Dic_CountryName.Add(0x683000, "哈萨克斯坦");
                    _Dic_CountryName.Add(0xB8000, "海地");
                    _Dic_CountryName.Add(0x480000, "荷兰");
                    _Dic_CountryName.Add(0xBA000, "洪都拉斯");
                    _Dic_CountryName.Add(0xC8E000, "基里巴斯");
                    _Dic_CountryName.Add(0x98000, "吉布提");
                    _Dic_CountryName.Add(0x601000, "吉尔吉斯斯坦");
                    _Dic_CountryName.Add(0x48000, "几内亚比绍");
                    _Dic_CountryName.Add(0xC00000, "加拿大 ");
                    _Dic_CountryName.Add(0x44000, "加纳");
                    _Dic_CountryName.Add(0x3E000, "加蓬");
                    _Dic_CountryName.Add(0x70E000, "柬埔寨");
                    _Dic_CountryName.Add(0x498000, "捷克");
                    _Dic_CountryName.Add(0x4000, "津巴布韦");
                    _Dic_CountryName.Add(0x34000, "喀麦隆 ");
                    _Dic_CountryName.Add(0x6A000, "卡塔尔");
                    _Dic_CountryName.Add(0x35000, "科摩罗");
                    _Dic_CountryName.Add(0x38000, "科特迪瓦 ");
                    _Dic_CountryName.Add(0x706000, "科威特");
                    _Dic_CountryName.Add(0x501C00, "克罗地亚");
                    _Dic_CountryName.Add(0x4C000, "肯尼亚");
                    _Dic_CountryName.Add(0x901000, "库克群岛");
                    _Dic_CountryName.Add(0x502C00, "拉脱维亚 ");
                    _Dic_CountryName.Add(0x4A000, "莱索托");
                    _Dic_CountryName.Add(0x708000, "老挝");
                    _Dic_CountryName.Add(0x748000, "黎巴嫩");
                    _Dic_CountryName.Add(0x50000, "利比里亚");
                    _Dic_CountryName.Add(0x18000, "利比亚");
                    _Dic_CountryName.Add(0x503C00, "立陶宛");
                    _Dic_CountryName.Add(0x4D0000, "卢森堡");
                    _Dic_CountryName.Add(0x6E000, "卢旺达");
                    _Dic_CountryName.Add(0x4A0000, "罗马尼亚 ");
                    _Dic_CountryName.Add(0x54000, "马达加斯加 ");
                    _Dic_CountryName.Add(0x4D2000, "马耳他");
                    _Dic_CountryName.Add(0x5A000, "马尔代夫");
                    _Dic_CountryName.Add(0x58000, "马拉维");
                    _Dic_CountryName.Add(0x750000, "马来西亚");
                    _Dic_CountryName.Add(0x5C000, "马里");
                    _Dic_CountryName.Add(0x900000, "马绍尔群岛");
                    _Dic_CountryName.Add(0x60000, "毛里求斯");
                    _Dic_CountryName.Add(0x5E000, "毛里塔尼亚");
                    _Dic_CountryName.Add(0xA00000, "美国");
                    _Dic_CountryName.Add(0x682000, "蒙古");
                    _Dic_CountryName.Add(0x702000, "孟加拉国");
                    _Dic_CountryName.Add(0xE8C000, "秘鲁");
                    _Dic_CountryName.Add(0x681000, "密克罗尼西亚联邦");
                    _Dic_CountryName.Add(0x704000, "缅甸");
                    _Dic_CountryName.Add(0x20000, "摩洛哥");
                    _Dic_CountryName.Add(0x4D4000, "摩纳哥");
                    _Dic_CountryName.Add(0x6000, "莫桑比克");
                    _Dic_CountryName.Add(0xD0000, "墨西哥 ");
                    _Dic_CountryName.Add(0x201000, "纳米比亚");
                    _Dic_CountryName.Add(0x8000, "南非");
                    _Dic_CountryName.Add(0x70A000, "尼泊尔");
                    _Dic_CountryName.Add(0xC0000, "尼加拉瓜");
                    _Dic_CountryName.Add(0x62000, "尼日尔");
                    _Dic_CountryName.Add(0x64000, "尼日利亚");
                    _Dic_CountryName.Add(0x478000, "挪威");
                    _Dic_CountryName.Add(0x684000, "帕劳 ");
                    _Dic_CountryName.Add(0x490000, "葡萄牙");
                    _Dic_CountryName.Add(0x840000, "日本");
                    _Dic_CountryName.Add(0x4A8000, "瑞典 ");
                    _Dic_CountryName.Add(0x4B0000, "瑞士");
                    _Dic_CountryName.Add(0xB2000, "萨尔瓦多");
                    _Dic_CountryName.Add(0x76000, "塞拉利昂");
                    _Dic_CountryName.Add(0x70000, "塞内加尔");
                    _Dic_CountryName.Add(0x74000, "塞舌尔");
                    _Dic_CountryName.Add(0x710000, "沙特阿拉伯");
                    _Dic_CountryName.Add(0xC8C000, "圣卢西亚");
                    _Dic_CountryName.Add(0x500000, "圣马力诺 ");
                    _Dic_CountryName.Add(0xBC000, "圣文森特和格林纳丁斯");
                    _Dic_CountryName.Add(0x770000, "斯里兰卡");
                    _Dic_CountryName.Add(0x505C00, "斯洛伐克");
                    _Dic_CountryName.Add(0x506C00, "斯洛文尼亚");
                    _Dic_CountryName.Add(0x7A000, "斯威士兰");
                    _Dic_CountryName.Add(0x7C000, "苏丹");
                    _Dic_CountryName.Add(0xC8000, "苏里南 ");
                    _Dic_CountryName.Add(0x78000, "索马里");
                    _Dic_CountryName.Add(0x897000, "所罗门群岛");
                    _Dic_CountryName.Add(0x515000, "塔吉克斯坦");
                    _Dic_CountryName.Add(0x880000, "泰国");
                    _Dic_CountryName.Add(0xC8D000, "汤加");
                    _Dic_CountryName.Add(0xC6000, "特立尼达和多巴哥");
                    _Dic_CountryName.Add(0x28000, "突尼斯 ");
                    _Dic_CountryName.Add(0x4B8000, "土耳其");
                    _Dic_CountryName.Add(0x601800, "土库曼斯坦");
                    _Dic_CountryName.Add(0xC90000, "瓦努阿图");
                    _Dic_CountryName.Add(0xB4000, "危地马拉");
                    _Dic_CountryName.Add(0xD8000, "委内瑞拉");
                    _Dic_CountryName.Add(0x895000, "文莱");
                    _Dic_CountryName.Add(0x68000, "乌干达");
                    _Dic_CountryName.Add(0x508000, "乌克兰");
                    _Dic_CountryName.Add(0xE90000, "乌拉圭");
                    _Dic_CountryName.Add(0x507C00, "乌兹别克斯坦");
                    _Dic_CountryName.Add(0x340000, "西班牙");
                    _Dic_CountryName.Add(0x468000, "希腊");
                    _Dic_CountryName.Add(0x768000, "新加坡");
                    _Dic_CountryName.Add(0xC80000, "新西兰");
                    _Dic_CountryName.Add(0x470000, "匈牙利");
                    _Dic_CountryName.Add(0x778000, "叙利亚 ");
                    _Dic_CountryName.Add(0xBE000, "牙买加");
                    _Dic_CountryName.Add(0x600000, "亚美尼亚");
                    _Dic_CountryName.Add(0x890000, "也门");
                    _Dic_CountryName.Add(0x728000, "伊拉克");
                    _Dic_CountryName.Add(0x730000, "伊朗");
                    _Dic_CountryName.Add(0x738000, "以色列");
                    _Dic_CountryName.Add(0x300000, "意大利 ");
                    _Dic_CountryName.Add(0x800000, "印度 ");
                    _Dic_CountryName.Add(0x8A0000, "印度尼西亚");
                    _Dic_CountryName.Add(0x400000, "英国");
                    _Dic_CountryName.Add(0x740000, "约旦");
                    _Dic_CountryName.Add(0x888000, "越南");
                    _Dic_CountryName.Add(0x8A000, "赞比亚");
                    _Dic_CountryName.Add(0x84000, "乍得");
                    _Dic_CountryName.Add(0xE8000, "智利");
                    _Dic_CountryName.Add(0x6C000, "中非 ");
                    _Dic_CountryName.Add(0x780000, "中国");
                    _Dic_CountryName.Add(0xC88000, "斐济");
                }
                return _Dic_CountryName;
            }
        }
        static int[] countryCode = new int[174] { 0x501000, 0xA0000, 0x700000, 0xE00000, 0x896000, 0x70C000, 0x600800, 0x10000, 0x40000, 0x4CA000, 0x511000, 0x90000, 0xCA000, 0x440000, 0x7C0000, 0xAA000, 0x898000, 0xA8000, 0x760000, 0xE88000, 0x894000, 0xC2000, 0xE40000, 0x510000, 0x450000, 0x94000, 0x448000, 0x4CC000, 0xE94000, 0x513000, 0x488000, 0x30000, 0xAB000, 0x680000, 0x9C000, 0x32000, 0x42000, 0x458000, 0x3C0000, 0x88000, 0xC4000, 0x100000, 0xE84000, 0x202000, 0x380000, 0x758000, 0x460000, 0x96000, 0x9A000, 0x36000, 0xAC000, 0xAE000, 0xCC000, 0x514000, 0xB0000, 0xB6000, 0x683000, 0xB8000, 0x480000, 0xBA000, 0xC8E000, 0x98000, 0x601000, 0x48000, 0xC00000, 0x44000, 0x3E000, 0x70E000, 0x498000, 0x4000, 0x34000, 0x6A000, 0x35000, 0x38000, 0x706000, 0x501C00, 0x4C000, 0x901000, 0x502C00, 0x4A000, 0x708000, 0x748000, 0x50000, 0x18000, 0x503C00, 0x4D0000, 0x6E000, 0x4A0000, 0x54000, 0x4D2000, 0x5A000, 0x58000, 0x750000, 0x5C000, 0x900000, 0x60000, 0x5E000, 0xA00000, 0x682000, 0x702000, 0xE8C000, 0x681000, 0x704000, 0x20000, 0x4D4000, 0x6000, 0xD0000, 0x201000, 0x8000, 0x70A000, 0xC0000, 0x62000, 0x64000, 0x478000, 0x684000, 0x490000, 0x840000, 0x4A8000, 0x4B0000, 0xB2000, 0x76000, 0x70000, 0x74000, 0x710000, 0xC8C000, 0x500000, 0xBC000, 0x770000, 0x505C00, 0x506C00, 0x7A000, 0x7C000, 0xC8000, 0x78000, 0x897000, 0x515000, 0x880000, 0xC8D000, 0xC6000, 0x28000, 0x4B8000, 0x601800, 0xC90000, 0xB4000, 0xD8000, 0x895000, 0x68000, 0x508000, 0xE90000, 0x507C00, 0x340000, 0x468000, 0x768000, 0xC80000, 0x470000, 0x778000, 0xBE000, 0x600000, 0x890000, 0x728000, 0x730000, 0x738000, 0x300000, 0x800000, 0x8A0000, 0x400000, 0x740000, 0x888000, 0x8A000, 0x84000, 0xE8000, 0x6C000, 0x780000, 0xC88000 };
        static int[] countryMode = new int[174] { 0xFFFC00, 0xFF8000, 0xFFF000, 0xFC0000, 0xFFF000, 0xFFFC00, 0xFFFC00, 0xFF8000, 0xFFF000, 0xFFF000, 0xFFFC00, 0xFFF000, 0xFFFC00, 0xFF8000, 0xFC0000, 0xFFFC00, 0xFFF000, 0xFFF000, 0xFF8000, 0xFFF000, 0xFFF000, 0xFFF000, 0xFC0000, 0xFFFC00, 0xFF8000, 0xFFFC00, 0xFF8000, 0xFFF000, 0xFFF000, 0xFFFC00, 0xFF8000, 0xFFFC00, 0xFFFC00, 0xFFFC00, 0xFFF000, 0xFFF000, 0xFFF000, 0xFF8000, 0xFC0000, 0xFFF000, 0xFFF000, 0xF00000, 0xFFF000, 0xFFFC00, 0xFC0000, 0xFF8000, 0xFF8000, 0xFFFC00, 0xFFF000, 0xFFF000, 0xFFF000, 0xFFF000, 0xFFFC00, 0xFFFC00, 0xFFF000, 0xFFF000, 0xFFFC00, 0xFFF000, 0xFF8000, 0xFFF000, 0xFFFC00, 0xFFFC00, 0xFFFC00, 0xFFFC00, 0xFC0000, 0xFFF000, 0xFFF000, 0xFFF000, 0xFF8000, 0xFFFC00, 0xFFF000, 0xFFFC00, 0xFFFC00, 0xFFF000, 0xFFF000, 0xFFFC00, 0xFFF000, 0xFFFC00, 0xFFFC00, 0xFFFC00, 0xFFF000, 0xFF8000, 0xFFF000, 0xFF8000, 0xFFFC00, 0xFFFC00, 0xFFF000, 0xFF8000, 0xFFF000, 0xFFFC00, 0xFFFC00, 0xFFF000, 0xFF8000, 0xFFF000, 0xFFFC00, 0xFFFC00, 0xFFFC00, 0xF00000, 0xFFFC00, 0xFFF000, 0xFFF000, 0xFFFC00, 0xFFF000, 0xFF8000, 0xFFFC00, 0xFFF000, 0xFF8000, 0xFFFC00, 0xFF8000, 0xFFF000, 0xFFF000, 0xFFF000, 0xFFF000, 0xFF8000, 0xFFFC00, 0xFF8000, 0xFC0000, 0xFF8000, 0xFF8000, 0xFFF000, 0xFFFC00, 0xFFF000, 0xFFFC00, 0xFF8000, 0xFFFC00, 0xFFFC00, 0xFFFC00, 0xFF8000, 0xFFFC00, 0xFFFC00, 0xFFFC00, 0xFFF000, 0xFFF000, 0xFFF000, 0xFFFC00, 0xFFFC00, 0xFF8000, 0xFFFC00, 0xFFF000, 0xFF8000, 0xFF8000, 0xFFFC00, 0xFFFC00, 0xFFF000, 0xFF8000, 0xFFFC00, 0xFFF000, 0xFF8000, 0xFFF000, 0xFFFC00, 0xFC0000, 0xFF8000, 0xFF8000, 0xFF8000, 0xFF8000, 0xFF8000, 0xFFF000, 0xFFFC00, 0xFFF000, 0xFF8000, 0xFF8000, 0xFF8000, 0xFC0000, 0xFC0000, 0xFF8000, 0xFC0000, 0xFF8000, 0xFF8000, 0xFFF000, 0xFFF000, 0xFFF000, 0xFFF000, 0xFC0000, 0xFFF000 };
        private static int ADS_GetCountry(SendClass ADS_info)
        {
            int country = 0;
            for (int i = 0; i < countryCode.Count(); i++)
            {
                int tmp = Convert.ToInt32(ADS_info.adICAO) & countryMode[i];
                if ((tmp ^ countryCode[i]) == 0)
                {
                    string str = GetCountryName(countryCode[i]);
                    country = return_country_code(str);
                }
            }
            return country;
        }
        public static int return_country_code(string country_name)
        {
            int countryNum = 0;
            try
            {
                string Path = System.IO.Directory.GetCurrentDirectory() + "\\country.txt";
                string[] ReadText = File.ReadAllLines(Path, Encoding.Default);
                foreach (string item in ReadText)
                {
                    if (item.Contains(country_name))
                    {
                        string num = item.Remove(0, item.Length - 4);
                        countryNum = Convert.ToInt32(num);
                    }
                }
                return countryNum;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        /// <summary>
        /// 判断是否在区域内
        /// </summary>
        /// <param name="AimLat">目标纬度</param>
        /// <param name="AimLon">目标经度</param>
        /// <param name="LocalLat">本地纬度</param>
        /// <param name="LocalLon">本地经度</param>
        /// <returns></returns> true 表明在区域外部
        ///                     false 表明在区域内部
        public static bool CheckRegion(double AimLat, double AimLon, double LocalLat, double LocalLon)
        {
            LineF Lin1 = new LineF();
            Lin1.Start = new PointF();
            Lin1.End = new PointF();
            Lin1.Start.X = 120.9375;
            Lin1.Start.Y = 34.2072;
            Lin1.End.X = 122.5083;
            Lin1.End.Y = 31.4753;

            LineF Lin2 = new LineF();
            Lin2.Start = new PointF();
            Lin2.End = new PointF();
            Lin2.Start.X = 122.5083;
            Lin2.Start.Y = 31.4753;
            Lin2.End.X = 123.4091;
            Lin2.End.Y = 30.7511;

            LineF Lin3 = new LineF();
            Lin3.Start = new PointF();
            Lin3.End = new PointF();
            Lin3.Start.X = 123.4091;
            Lin3.Start.Y = 30.7511;
            Lin3.End.X = 120.2561;
            Lin3.End.Y = 25.5502;


            LineF InputLin = new LineF();
            InputLin.Start = new PointF();
            InputLin.End = new PointF();
            InputLin.Start.X = AimLon;
            InputLin.Start.Y = AimLat;
            InputLin.End.X = LocalLon;
            InputLin.End.Y = LocalLat;

            bool CheckCross = IsIntersect(Lin1.Start, Lin1.End, InputLin.Start, InputLin.End);
            if (CheckCross)
            {
                return true;
            }
            CheckCross = IsIntersect(Lin2.Start, Lin2.End, InputLin.Start, InputLin.End);
            if (CheckCross)
            {
                return true;
            }
            CheckCross = IsIntersect(Lin3.Start, Lin3.End, InputLin.Start, InputLin.End);
            if (CheckCross)
            {
                return true;
            }
            return false;
        }
        public static double Cross(PointF a, PointF b, PointF c)
        {
            return (b.X - a.X) * (c.Y - a.Y) - (b.Y - a.Y) * (c.X - a.X);
        }

        /// <summary>
        /// 线段是否相交
        /// </summary>
        /// <param name="p1">线段P1P2的P1点</param>
        /// <param name="p2">线段P1P2的P2点</param>
        /// <param name="q1">线段Q1Q2的Q1点</param>
        /// <param name="q2">线段Q1Q2的Q2点</param>
        /// <returns></returns>
        public static bool IsIntersect(PointF p1, PointF p2, PointF q1, PointF q2)
        {
            //排斥试验，判断p1p2在q1q2为对角线的矩形区之外
            if (Math.Max(p1.X, p2.X) < Math.Min(q1.X, q2.X))
            {//P1P2中最大的X比Q1Q2中的最小X还要小，说明P1P2在Q1Q2的最左点的左侧，不可能相交。
                return false;
            }

            if (Math.Min(p1.X, p2.X) > Math.Max(q1.X, q2.X))
            {//P1P2中最小的X比Q1Q2中的最大X还要大，说明P1P2在Q1Q2的最右点的右侧，不可能相交。
                return false;
            }

            if (Math.Max(p1.Y, p2.Y) < Math.Min(q1.Y, q2.Y))
            {//P1P2中最大的Y比Q1Q2中的最小Y还要小，说明P1P2在Q1Q2的最低点的下方，不可能相交。
                return false;
            }

            if (Math.Min(p1.Y, p2.Y) > Math.Max(q1.Y, q2.Y))
            {//P1P2中最小的Y比Q1Q2中的最大Y还要大，说明P1P2在Q1Q2的最高点的上方，不可能相交。
                return false;
            }

            //跨立试验
            double crossP1P2Q1 = Cross(p1, p2, q1);
            double crossP1Q2P2 = Cross(p1, p2, q2);
            double crossQ1Q2P1 = Cross(q1, q2, p1);
            double crossQ1P2Q2 = Cross(q1, q2, p2);

            bool isIntersect = (crossP1P2Q1 * crossP1Q2P2 <= 0) && (crossQ1Q2P1 * crossQ1P2Q2 <= 0);
            return isIntersect;
        }
        public static TargetIdentification Get14suo_ADS(byte[] data)
        {
            TargetIdentification target_info = new TargetIdentification();
            target_info = ByteToStructure<TargetIdentification>(data);
            return target_info;
        }

        public static TargetIdentification Send14suo_ADS(SendClass info)
        {
            TargetIdentification target_info = new TargetIdentification();

            //byte[] array = new byte[112];
            target_info.head = 0xa5a5a5a5;
            target_info.length = 112;
            target_info.deviceNum = 3979;
            target_info.PH = 0;
            target_info.produceDate = 0;
            target_info.produceTime = 0;
            target_info.ICAO = new char[6];
            target_info.planeType = 0;
            target_info.flightNumber = new char[8];
            target_info.registrationNum = new char[20];
            target_info.area = 0;
            target_info.flyCode = 0;
            target_info.bak1 = 0;
            target_info.bak2 = 0;
            target_info.bak3 = 0;
            target_info.bak = new char[20];
            byte[] Btmp8 = new byte[info.AirPlaneID.Length];
            if (info.AirPlaneID != "")
            {
                Btmp8 = System.Text.Encoding.ASCII.GetBytes(info.AirPlaneID);
                Array.Copy(Btmp8, 0, target_info.flightNumber, 0, info.AirPlaneID.Length);
            }
            target_info.produceDate = (uint)DateDiff(Convert.ToDateTime("2000-01-01 00:00:00"), info.CreatDate);
            target_info.produceTime = (uint)getMSecond(info.dtDate);
            target_info.ICAO = Convertchar(info.adICAO.ToString());
            if (info.adlongitude != "")
            {
                target_info.bak1 = Math.Round(double.Parse(info.adlongitude), 4);
            }
            if (info.adlatitude != "")
            {
                target_info.bak2 = Math.Round(double.Parse(info.adlatitude), 4);
            }
            target_info.bak3 = Convert.ToDouble(info.Height);
            target_info.PH = (ushort)(info.adICAO & 0xFFFF);
            target_info.area = ADS_GetCountry(info);
            bool iRet = CheckRegion(target_info.bak2, target_info.bak1, 30.71, 122.82);
            if (iRet)
            {
                if (target_info.area == 4117)
                {
                    target_info.IFF = 2;
                }
                else
                {
                    target_info.IFF = 3;
                }
            }
            else
            {
                target_info.IFF = 2;
            }
            //target_info.flightNumber = info.AirPlaneID.ToCharArray();
            //array = StructToBytes(target_info);
            return target_info;

        }

        //结构体转为字节数组
        public static byte[] StructToByte(object structObj)
        {
            int size = Marshal.SizeOf(structObj);
            byte[] dataByte = new byte[size];
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(structObj, structPtr, false);
            Marshal.Copy(structPtr, dataByte, 0, size);
            Marshal.FreeHGlobal(structPtr);
            return dataByte;
        }
        //数组转结构体
        public static T ByteToStructure<T>(byte[] dataBuffer)
        {
            object structure = null;
            int size = Marshal.SizeOf(typeof(T));
            IntPtr allocIntPtr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(dataBuffer, 0, allocIntPtr, size);
                structure = Marshal.PtrToStructure(allocIntPtr, typeof(T));
            }
            finally
            {
                Marshal.FreeHGlobal(allocIntPtr);
            }
            return (T)structure;
        }



    }
}