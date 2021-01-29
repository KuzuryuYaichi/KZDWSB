using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ADS_B_INFO
{
    class code_msg
    {
        public uint icao;
        public uint YZ0;
        public uint XZ0;
        public uint YZ1;
        public uint XZ1;
        public byte odd_sign;
        public byte even_sign;
        public uint odd_time;
        public uint even_time;
        public byte cpr;
    }
}
