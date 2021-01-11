using System;
using System.IO;
using System.Xml.Serialization;

namespace VDI_VDO_Graf.Serialise
{
    [Serializable]
    public class PLC_IP
    {
        public int IP_192 { get; set; }
        public int IP_168 { get; set; }
        public int IP_1 { get; set; }
        public int IP_17 { get; set; }

        public PLC_IP()
        {

        }
        public PLC_IP(int ip_192, int ip_168, int ip_1, int ip_17)
        {
            IP_192 = ip_192;
            IP_168 = ip_168;
            IP_1 = ip_1;
            IP_17 = ip_17;
        }
    }
}
