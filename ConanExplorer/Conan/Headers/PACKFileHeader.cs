using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConanExplorer.Conan.Headers
{
    public class PACKFileHeader
    {
        private static int _mask = 0x1EFFFFE0;

        public byte[] Data { get; set; } = new byte[17];

        [XmlIgnore]
        public int Length
        {
            get
            {
                int buffer = BitConverter.ToInt32(Data, 0);
                return (buffer & _mask) >> 5;
            }
            set
            {
                int buffer = BitConverter.ToInt32(Data, 0);
                buffer = (buffer & ~_mask) | ((value << 5) & _mask);
                Array.Copy(BitConverter.GetBytes(buffer), Data, 4);
            }
        }

        public PACKFileHeader() { }

        public PACKFileHeader(byte[] data)
        {
            Data = data;
        }
    }
}
