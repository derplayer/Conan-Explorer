using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConanExplorer.Conan.Headers
{
    public class LZSSHeader
    {
        [XmlIgnore]
        public static byte[] Signature { get; } = { 0x4C, 0x5A, 0x53, 0x53, 0x2D, 0x62, 0x75, 0x32 };
        public short Param1 { get; set; }
        public byte Param2 { get; set; }
        public byte Mode { get; set; }
        public uint UncompressedSize { get; set; }

        public LZSSHeader() { }

        public static LZSSHeader Empty(byte mode)
        {
            LZSSHeader result = new LZSSHeader
            {
                Mode = mode,
                UncompressedSize = 0,
                Param1 = 0,
                Param2 = 0
            };
            return result;
        }

        public byte[] GetBytes()
        {
            byte[] result = new byte[16];
            Array.Copy(Signature, 0, result, 0, 8);
            byte[] param1 = BitConverter.GetBytes(Param1);
            Array.Copy(param1, 0, result, 8, 2);
            result[10] = Param2;
            result[11] = Mode;
            byte[] uncompressedSize = BitConverter.GetBytes(UncompressedSize);
            Array.Copy(uncompressedSize, 0, result, 12, 4);
            return result;
        }

        public bool Load(byte[] data)
        {
            if (data.Length != 16) return false;
            byte[] signature = new byte[8];
            Array.Copy(data, 0, signature, 0, 8);
            if (!signature.SequenceEqual(Signature)) return false;

            byte[] param1 = new byte[2];
            Array.Copy(data, 8, param1, 0, 2);
            Param1 = BitConverter.ToInt16(param1, 0);
            Param2 = data[10];
            Mode = data[11];
            byte[] uncompressedSize = new byte[4];
            Array.Copy(data, 12, uncompressedSize, 0, 4);
            UncompressedSize = BitConverter.ToUInt32(uncompressedSize, 0);
            return true;
        }
    }
}
