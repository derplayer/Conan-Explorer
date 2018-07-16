using ConanExplorer.Conan.Filetypes;
using ConanExplorer.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ConanExplorer.Conan.Headers
{
    /// <summary>
    /// TIM header class.
    /// </summary>
    [Serializable]
    public class TIMHeader
    {
        /// <summary>
        /// Header signature.
        /// </summary>
        public static byte[] Signature = new byte[] { 0x10, 0x00, 0x00, 0x00 };

        /// <summary>
        /// Mode consists of the flag for having a CLUT and the BPP mode.
        /// </summary>
        public uint Mode
        {
            get
            {
                uint mode = HasClut ? 8u : 0u;
                if (BPP > 4) mode |= (ushort)(BPP / 8);
                return mode;
            }
            set
            {
                HasClut = (value & 8) != 0;
                uint bpp = value &= 7;

                if (bpp == 0)
                {
                    BPP = 4;
                }
                else if (bpp <= 3)
                {
                    BPP = (byte)(bpp * 8);
                }
                else
                {
                    throw new IndexOutOfRangeException("The BPP mode is out of range! Only allowed modes are 0 - 3.");
                }
            }
        }
        /// <summary>
        /// Available depths: 4 BPP (16-color), 8 BPP (256-color), 16 BPP (RGB555), 24 BPP (RGB888)
        /// </summary>
        public byte BPP { get; set; }
        /// <summary>
        /// Only 4 BPP and 8 BPP should have a CLUT
        /// </summary>
        public bool HasClut{ get; set; }
        public uint ClutBlockSize
        {
            get
            {
                if (BPP == 4) return 44;
                else if (BPP == 8) return 524;
                return 0;
            }
        }
        public ushort ClutX { get; set; }
        public ushort ClutY { get; set; }
        public ushort ClutWidth { get; set; }
        public ushort ClutHeight { get; set; }
        public CLUTEntry[] ClutEntries { get; set; }

        public uint ImageBlockSize
        {
            get
            {
                switch (BPP)
                {
                    case 4:
                        return 12 + (((uint)ImageWidthPixels * ImageHeight) / 2);
                    case 8:
                        return 12 + ((uint)ImageWidthPixels * ImageHeight);
                    case 16:
                        return 12 + (((uint)ImageWidthPixels * ImageHeight) * 2);
                    case 24:
                        return 12 + (((uint)ImageWidthPixels * ImageHeight) * 3);
                    default:
                        return 0;
                }
            }
        }
        public ushort ImageFrameBufferX { get; set; }
        public ushort ImageFrameBufferY { get; set; }
        /// <summary>
        /// Width in 16-bit unit pixels
        /// </summary>
        public ushort ImageWidth { get; set; }
        /// <summary>
        /// Width in pixels
        /// </summary>
        public ushort ImageWidthPixels
        {
            get
            {
                switch (BPP)
                {
                    case 4:
                        return (ushort)(ImageWidth * 4);
                    case 8:
                        return (ushort)(ImageWidth * 2);
                    case 16:
                        return ImageWidth;
                    case 24:
                        return (ushort)(ImageWidth / 1.5);
                    default:
                        return 0;
                }
            }
            set
            {
                switch (BPP)
                {
                    case 4:
                        ImageWidth = (ushort)(value / 4);
                        break;
                    case 8:
                        ImageWidth = (ushort)(value / 2);
                        break;
                    case 16:
                        ImageWidth = ImageWidth;
                        break;
                    case 24:
                        ImageWidth = (ushort)(value * 1.5);
                        break;
                    default:
                        ImageWidth = 0;
                        break;
                }
            }
        }
        /// <summary>
        /// Height in pixels
        /// </summary>
        public ushort ImageHeight { get; set; }
        public uint DataOffset { get; set; }
        public PixelFormat PixelFormat
        {
            get
            {
                switch (BPP)
                {
                    case 4:
                        return PixelFormat.Format4bppIndexed;
                    case 8:
                        return PixelFormat.Format8bppIndexed;
                    case 16:
                        return PixelFormat.Format16bppRgb555;
                    case 24:
                        return PixelFormat.Format24bppRgb;
                    default:
                        return PixelFormat.Undefined;
                }
            }
        }

        public TIMHeader() { }

        public TIMHeader DeepClone()
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, this);
                ms.Position = 0;

                return (TIMHeader)formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// Gets the bytes for writing the TIM header.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            List<byte> result = new List<byte>();
            result.AddRange(Signature);

            result.AddRange(BitConverter.GetBytes(Mode));

            if (HasClut)
            {
                result.AddRange(BitConverter.GetBytes(ClutBlockSize));
                result.AddRange(BitConverter.GetBytes(ClutX));
                result.AddRange(BitConverter.GetBytes(ClutY));
                result.AddRange(BitConverter.GetBytes(ClutWidth));
                result.AddRange(BitConverter.GetBytes(ClutHeight));

                for (int i = 0; i < ClutWidth * ClutHeight; i++)
                {
                    result.AddRange(BitConverter.GetBytes(ClutEntries[i].Data));
                }

                result.AddRange(BitConverter.GetBytes(ImageBlockSize));
                result.AddRange(BitConverter.GetBytes(ImageFrameBufferX));
                result.AddRange(BitConverter.GetBytes(ImageFrameBufferY));
                result.AddRange(BitConverter.GetBytes(ImageWidth));
                result.AddRange(BitConverter.GetBytes(ImageHeight));
            }
            return result.ToArray();
        }

        /// <summary>
        /// Writes the header to a stream.
        /// </summary>
        /// <param name="stream"></param>
        public void Write(Stream stream)
        {
            byte[] buffer = GetBytes();
            stream.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Loads the header from a stream.
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public bool Load(BinaryReader reader)
        {
            byte[] signature = reader.ReadBytes(4);
            if (!signature.SequenceEqual(Signature)) return false;

            Mode = reader.ReadUInt32();

            if (HasClut)
            {
                //Skipping ClutBlockSize cause it will be generated
                reader.ReadUInt32(); 

                ClutX = reader.ReadUInt16();
                ClutY = reader.ReadUInt16();
                ClutWidth = reader.ReadUInt16();
                ClutHeight = reader.ReadUInt16();

                ClutEntries = new CLUTEntry[ClutWidth * ClutHeight];
                for (int i = 0; i < ClutWidth * ClutHeight; i++)
                {
                    ClutEntries[i] = new CLUTEntry(reader.ReadUInt16());
                }
            }
            //Skipping ImageBlockSize cause it will be generated
            reader.ReadUInt32();

            ImageFrameBufferX = reader.ReadUInt16();
            ImageFrameBufferY = reader.ReadUInt16();
            ImageWidth = reader.ReadUInt16();
            ImageHeight = reader.ReadUInt16();

            DataOffset = (uint)reader.BaseStream.Position;
            return true;
        }

        public void SetSemiTransparentBits(CLUTEntry[] clutEntries)
        {
            foreach (CLUTEntry entry in ClutEntries)
            {
                foreach (CLUTEntry e in clutEntries)
                {
                    if (entry.Color == e.Color)
                    {
                        entry.SemiTransparentBit = e.SemiTransparentBit;
                    }
                }
            }
        }

        public void SetOriginalColor(CLUTEntry[] clutEntries)
        {
            foreach (CLUTEntry entry in ClutEntries)
            {
                CLUTEntry nearest = new CLUTEntry();
                int difference = 765;
                foreach(CLUTEntry e in clutEntries)
                {
                    int diff = Graphic.ColorDifference(entry.Color, e.Color);
                    if (diff < difference)
                    {
                        nearest = e;
                        difference = diff;
                    }
                }
                entry.Color = nearest.Color;
                entry.SemiTransparentBit = nearest.SemiTransparentBit;
            }
        }

        

        /// <summary>
        /// Generate CLUT from given bitmap and encoding settings
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="settings">Encoding Settings</param>
        public void GenerateClut(Bitmap bitmap, TIMEncodingSettings settings)
        {
            if (BPP > 8) return;
            if (bitmap.Width != ImageWidthPixels || bitmap.Height != ImageHeight)
                throw new ArgumentOutOfRangeException("bitmap", "The given bitmap has not the correct width or height!");

            if (BPP == 4)
            {
                if (bitmap.PixelFormat != PixelFormat.Format4bppIndexed)
                    throw new ArgumentException("The given bitmap has the wrong pixel format! Needed is 4 BPP.", "bitmap");

                Color[] palette = bitmap.Palette.Entries;

                for (int i = 0; i < 16; i++)
                {
                    ClutEntries[i] = new CLUTEntry(bitmap.Palette.Entries[i], settings);
                }
            }
            else if (BPP == 8)
            {
                if (bitmap.PixelFormat != PixelFormat.Format8bppIndexed)
                    throw new ArgumentException("The given bitmap has the wrong pixel format! Needed is 8 BPP", "bitmap");

                for (int i = 0; i < 256; i++)
                {
                    ClutEntries[i] = new CLUTEntry(bitmap.Palette.Entries[i], settings);
                }
            }
        }

        private ushort RGB24_To_RGBPSX(Color color, TIMEncodingSettings settings)
        {
            ushort result;

            result = (ushort)(color.R >> 3);
            result |= (ushort)((color.G >> 3) << 5);
            result |= (ushort)((color.B >> 3) << 10);

            if (result == 0 && settings.BlackTransparent) result |= 0x8000;
            if (result == ((31) | (31 << 10)) && settings.MagicPinkTransparent) result = 0;

            if (settings.SetSemiTransparencyBit)
            {
                if (settings.BlackTransparent && result == 0) return result;
                if (settings.MagicPinkTransparent && result == ((31) | (31 << 10))) return result;
                result |= 0x8000;
            }
            return result;
        }

    }

    [Serializable]
    public class CLUTEntry
    {
        public ushort Data { get; set; }

        [XmlIgnore]
        public bool SemiTransparentBit
        {
            get
            {
                return (Data & 0x8000) == 0x8000;
            }
            set
            {
                if (value)
                {
                    Data |= 0x8000; 
                }
                else
                {
                    Data &= 0x7FFF;
                }
            }
        }

        [XmlIgnore]
        public Color Color
        {
            get
            {
                byte R = (byte)((Data & 31) << 3);
                byte G = (byte)(((Data >> 5) & 31) << 3);
                byte B = (byte)(((Data >> 10) & 31) << 3);
                return Color.FromArgb(255, R, G, B);
            }
            set
            {
                bool transparent = SemiTransparentBit;
                ushort result = (ushort)(value.R >> 3);
                result |= (ushort)((value.G >> 3) << 5);
                result |= (ushort)((value.B >> 3) << 10);
                Data = result;
                SemiTransparentBit = transparent;
            }
        }

        [XmlIgnore]
        public System.Windows.Media.Color MediaColor
        {
            get
            {
                byte R = (byte)((Data & 31) << 3);
                byte G = (byte)(((Data >> 5) & 31) << 3);
                byte B = (byte)(((Data >> 10) & 31) << 3);
                return System.Windows.Media.Color.FromArgb(255, R, G, B);
            }
            set
            {
                bool transparent = SemiTransparentBit;
                ushort result = (ushort)(value.R >> 3);
                result |= (ushort)((value.G >> 3) << 5);
                result |= (ushort)((value.B >> 3) << 10);
                Data = result;
                SemiTransparentBit = transparent;
            }
        }

        public CLUTEntry(ushort data)
        {
            Data = data;
        }

        public CLUTEntry(Color color, TIMEncodingSettings settings)
        {
            Data = RGB24_To_RGBPSX(color, settings);
        }

        public CLUTEntry()
        {

        }

        private ushort RGB24_To_RGBPSX(Color color, TIMEncodingSettings settings)
        {
            ushort result;

            result = (ushort)(color.R >> 3);
            result |= (ushort)((color.G >> 3) << 5);
            result |= (ushort)((color.B >> 3) << 10);

            if (result == 0 && settings.BlackTransparent) result |= 0x8000;
            if (result == ((31) | (31 << 10)) && settings.MagicPinkTransparent) result = 0;

            if (settings.SetSemiTransparencyBit)
            {
                if (settings.BlackTransparent && result == 0) return result;
                if (settings.MagicPinkTransparent && result == ((31) | (31 << 10))) return result;
                result |= 0x8000;
            }
            return result;
        }

    }
}
