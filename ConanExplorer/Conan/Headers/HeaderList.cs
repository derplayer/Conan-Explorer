using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ConanExplorer.Conan.Filetypes;

namespace ConanExplorer.Conan.Headers
{
    /// <summary>
    /// List of all the known Meitantei Conan(SLPS_01690) file headers.
    /// </summary>
    public static class HeaderList
    {
        public static List<Header> Headers
        {
            get
            {
                return new List<Header>()
                {
                    BG,
                    LZB,
                    PACK,
                    SEQ,
                    STR,
                    TIM,
                    VB,
                    VH,
                    XA
                };
            }
        }

        /// <summary>
        /// Proprietary Package header.
        /// </summary>
        public static Header BG
        {
            get
            {
                Header header = new Header();
                header.Name = "BU-MAP0";
                header.Description = "Packs multiple files without unnecessary blank space between files. (No Compression)";
                header.Extension = "BG";
                header.Signature = new byte[] { 0x42, 0x55, 0x2D, 0x4D, 0x41, 0x50, 0x30, 0x00 };
                header.FileType = typeof(BGFile);
                return header;
            }
        }

        /// <summary>
        /// Proprietary LZSS compressed file header.
        /// </summary>
        public static Header LZB 
        {
            get
            {
                Header header = new Header();
                header.Name = "LZSS-bu2";
                header.Description = "Proprietary LZSS compressed file.";
                header.Extension = "LZB";
                header.Signature = new byte[] { 0x4C, 0x5A, 0x53, 0x53, 0x2D, 0x62, 0x75, 0x32 };
                header.FileType = typeof(LZBFile);
                return header;
            }
        }

        /// <summary>
        /// Proprietary Package header.
        /// </summary>
        public static Header PACK
        {
            get
            {
                Header header = new Header();
                header.Name = "PACK-bu2";
                header.Description = "Packs multiple files without unnecessary blank space between files. (No Compression)";
                header.Extension = "PACK";
                header.Signature = new byte[] { 0x50, 0x41, 0x43, 0x4B, 0x2D, 0x62, 0x75, 0x32 };
                header.FileType = typeof(PBFile);
                return header;
            }
        }

        public static Header SEQ
        {
            get
            {
                Header header = new Header();
                header.Name = "SEQ";
                header.Description = "Audio";
                header.Extension = "SEQ";
                header.Signature = new byte[] { 0x70, 0x51, 0x45, 0x53, 0x00, 0x00, 0x00, 0x01 };
                header.FileType = typeof(SEQFile);
                return header;
            }
        }

        public static Header STR
        {
            get
            {
                Header header = new Header();
                header.Name = "STR (MDEC)";
                header.Description = "Video + Audio";
                header.Extension = "STR";
                header.Signature = new byte[] { 0x01, 0x01, 0x48, 0x00, 0x01, 0x01, 0x48, 0x00 };
                header.FileType = typeof(STRFile);
                return header;
            }
        }

        /// <summary>
        /// TIM Graphic Format header.
        /// </summary>
        public static Header TIM
        {
            get
            {
                Header header = new Header();
                header.Name = "TIM Graphic Format";
                header.Description = "PSX 2D Graphics file.";
                header.Extension = "TIM";
                header.Signature = new byte[] { 0x10, 0x00, 0x00, 0x00 };
                header.FileType = typeof(TIMFile);
                return header;
            }
        }

        public static Header VB
        {
            get
            {
                Header header = new Header();
                header.Name = "VB";
                header.Description = "Audio";
                header.Extension = "VB";
                header.Signature = new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                header.FileType = typeof(SEQFile);
                return header;
            }
        }

        public static Header VH
        {
            get
            {
                Header header = new Header();
                header.Name = "VH";
                header.Description = "Audio";
                header.Extension = "VH";
                header.Signature = new byte[] { 0x70, 0x42, 0x41, 0x56, 0x07, 0x00, 0x00, 0x00 };
                header.FileType = typeof(SEQFile);
                return header;
            }
        }

        public static Header XA
        {
            get
            {
                Header header = new Header();
                header.Name = "XA";
                header.Description = "Audio";
                header.Extension = "XA";
                header.Signature = new byte[] { 0x01, 0x00, 0x64, 0x04, 0x01, 0x00, 0x64, 0x04 };
                header.FileType = typeof(XAFile);
                return header;
            }
        }


        public static string GetExtensionFromBuffer(byte[] buffer)
        {
            if (BG.Compare(buffer))
            {
                return "BG";
            }
            if (LZB.Compare(buffer))
            {
                return "LZB";
            }
            if (PACK.Compare(buffer))
            {
                return "PB";
            }
            if (SEQ.Compare(buffer))
            {
                return "SEQ";
            }
            if (STR.Compare(buffer))
            {
                return "STR";
            }
            if (TIM.Compare(buffer))
            {
                return "TIM";
            }
            if (VB.Compare(buffer))
            {
                return "VB";
            }
            if (VH.Compare(buffer))
            {
                return "VH";
            }
            if (XA.Compare(buffer))
            {
                return "XA";
            }
            return "RAW";
        }

        public static BaseFile GetTypeFromFile(string filePath)
        {
            byte[] buffer;
            using (BinaryReader reader = new BinaryReader(new FileStream(filePath, FileMode.Open)))
            {
                if (reader.BaseStream.Length < 256)
                {
                    buffer = new byte[reader.BaseStream.Length];
                    reader.Read(buffer, 0, buffer.Length);
                }
                else
                {
                    buffer = new byte[256];
                    reader.Read(buffer, 0, 256);
                }
            }

            if (BG.Compare(buffer))
            {
                return new BGFile(filePath);
            }
            if (LZB.Compare(buffer))
            {
                return new LZBFile(filePath);
            }
            if (PACK.Compare(buffer))
            {
                return new PBFile(filePath);
            }
            if (SEQ.Compare(buffer))
            {
                return new SEQFile(filePath);
            }
            if (STR.Compare(filePath))
            {
                return new STRFile(filePath);
            }
            if (TIM.Compare(buffer))
            {
                return new TIMFile(filePath);
            }
            if (VB.Compare(buffer))
            {
                return new VBFile(filePath);
            }
            if (VH.Compare(buffer))
            {
                return new VHFile(filePath);
            }
            if (XA.Compare(filePath))
            {
                return new XAFile(filePath);
            }
            return new BaseFile(filePath);
        }
    }
}
