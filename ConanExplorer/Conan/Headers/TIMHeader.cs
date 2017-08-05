using ConanExplorer.Conan.Filetypes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConanExplorer.Conan.Headers
{
    public class TIMHeader
    {
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
        public ushort[] ClutData { get; set; }
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
                    result.AddRange(BitConverter.GetBytes(ClutData[i]));
                }

                result.AddRange(BitConverter.GetBytes(ImageBlockSize));
                result.AddRange(BitConverter.GetBytes(ImageFrameBufferX));
                result.AddRange(BitConverter.GetBytes(ImageFrameBufferY));
                result.AddRange(BitConverter.GetBytes(ImageWidth));
                result.AddRange(BitConverter.GetBytes(ImageHeight));
            }
            return result.ToArray();
        }

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
                    ClutData[i] = RGB24_To_RGBPSX(bitmap.Palette.Entries[i], settings);
                }
            }
            else if (BPP == 8)
            {
                if (bitmap.PixelFormat != PixelFormat.Format8bppIndexed)
                    throw new ArgumentException("The given bitmap has the wrong pixel format! Needed is 8 BPP", "bitmap");

                for (int i = 0; i < 256; i++)
                {
                    ClutData[i] = RGB24_To_RGBPSX(bitmap.Palette.Entries[i], settings);
                }
            }
        }

        public void Write(Stream stream)
        {
            byte[] buffer = GetBytes();
            stream.Write(buffer, 0, buffer.Length);
        }

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

                ClutData = new ushort[ClutWidth * ClutHeight];
                for (int i = 0; i < ClutWidth * ClutHeight; i++)
                {
                    ClutData[i] = reader.ReadUInt16();
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
