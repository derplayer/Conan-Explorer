using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConanExplorer.Conan.Headers
{
    /// <summary>
    /// PB file sub-header for each file entry.
    /// </summary>
    public class PBFileHeader
    {
        private static int _mask = 0x1EFFFFE0;

        /// <summary>
        /// Data of the sub-header.
        /// </summary>
        public byte[] Data { get; set; } = new byte[17];

        /// <summary>
        /// Length of the next file.
        /// </summary>
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

        public PBFileHeader() { }

        public PBFileHeader(byte[] data)
        {
            Data = data;
        }
    }
}
