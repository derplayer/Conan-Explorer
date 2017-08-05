using ConanExplorer.Conan.Headers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ConanExplorer.Conan.Filetypes
{
    public class TIMFile : BaseFile
    {
        //public BaseFile BitmapFile { get; set; }
        public TIMHeader TIMHeader { get; set; } = new TIMHeader();


        public TIMFile() { }
        public TIMFile(string filePath) : base(filePath)
        {
            using (BinaryReader reader = new BinaryReader(new FileStream(FilePath, FileMode.Open)))
            {
                if (reader.BaseStream.Length == 0) return;
                TIMHeader.Load(reader);
            }
        }

        public Bitmap GetBitmap()
        {
            if (true) //do memorystream check or something if changed
            {
                if (!File.Exists(FilePath)) return null;
                return TIM2BMP();

                //string outputPath = FilePath + ".BMP";
                //using (FileStream fileStream = new FileStream(outputPath, FileMode.Create))
                //{
                //    Bitmap bitmap = TIM2BMP();
                //    bitmap.Save(fileStream, ImageFormat.Bmp); //GDI problem when file is open or already exists?
                //    bitmap.Dispose();
                //}
                //
                //BitmapFile = new BaseFile(outputPath);
            }
            return null;
        }

        public void SetBitmap(Bitmap bitmap, TIMEncodingSettings settings)
        {
            if (TIMHeader.ImageWidthPixels != bitmap.Width || TIMHeader.ImageHeight != bitmap.Height)
            {
                MessageBox.Show("The given bitmap has the wrong size!", "Incorrect bitmap size!");
                return;
            }
            if (TIMHeader.PixelFormat != bitmap.PixelFormat)
            {
                MessageBox.Show("The given bitmap has the wrong pixel format!", "Incorrect bitmap format!");
                return;
            }

            BMP2TIM(bitmap, settings);
        }

        private ushort GetColorIndex(Bitmap bitmap, Color color)
        {
            List<Color> palette = bitmap.Palette.Entries.ToList();
            return (ushort)palette.IndexOf(color);
        }

        private Bitmap TIM2BMP2(string fileName)
        {
            //TOOD better tim2bmp conversion with Bitmap class or something
            return null;
        }

        private Bitmap TIM2BMP()
        {
            uint y, x;
            uint tim_row_off;

            MemoryStream memoryStream = new MemoryStream();
            using (BinaryReader reader = new BinaryReader(new FileStream(FilePath, FileMode.Open)))
            {
                TIMHeader.Load(reader);

                PS_BITMAP bitmapHeader = new PS_BITMAP(TIMHeader.ImageWidthPixels, TIMHeader.ImageHeight, TIMHeader.BPP);
                bitmapHeader.Write(memoryStream);

                if (TIMHeader.HasClut)
                {
                    if (TIMHeader.BPP == 4) // 4bpp
                    {
                        for (x = 0; x < 16; x++)
                        {
                            PS_RGB rgb = new PS_RGB(TIMHeader.ClutData[x]);
                            memoryStream.WriteByte(rgb.B);
                            memoryStream.WriteByte(rgb.G);
                            memoryStream.WriteByte(rgb.R);
                            memoryStream.WriteByte(0);
                        }
                    }
                    else if (TIMHeader.BPP == 8) // 8 bpp
                    {
                        for (x = 0; x < 256; x++)
                        {
                            PS_RGB rgb = new PS_RGB(TIMHeader.ClutData[x]);
                            memoryStream.WriteByte(rgb.B);
                            memoryStream.WriteByte(rgb.G);
                            memoryStream.WriteByte(rgb.R);
                            memoryStream.WriteByte(0);
                        }
                    }
                }
                else
                {
                    if (TIMHeader.BPP == 4)
                    {
                        memoryStream.Position += 64;
                    }
                    else if (TIMHeader.BPP == 8)
                    {
                        memoryStream.Position += 1024;
                    }
                }

                if (TIMHeader.BPP == 16)
                {
                    y = (uint)(TIMHeader.ImageWidthPixels * 24) / 8;
                }
                else
                {
                    y = (uint)(TIMHeader.ImageWidthPixels * TIMHeader.BPP) / 8;
                }

                uint row_round = y;

                if ((row_round & 3) != 0)
                {
                    row_round |= 3;
                    row_round++;
                }

                row_round -= y;

                for (y = 0; y < TIMHeader.ImageHeight; y++)
                {
                    tim_row_off = (uint)(TIMHeader.ImageWidth * 2 * (TIMHeader.ImageHeight - 1 - y));

                    reader.BaseStream.Position = TIMHeader.DataOffset + tim_row_off;

                    for (x = 0; x < TIMHeader.ImageWidth; x++)
                    {
                        ushort c = reader.ReadUInt16();

                        switch (TIMHeader.BPP)
                        {
                            case 4:
                                memoryStream.WriteByte((byte)(((c >> 4) & 0xf) | ((c & 0xf) << 4)));
                                memoryStream.WriteByte((byte)(((c >> 12) & 0xf) | (((c >> 8) & 0xf) << 4)));
                                break;
                            case 8:
                                memoryStream.Write(BitConverter.GetBytes(c), 0, 2);
                                break;
                            case 16:
                                PS_RGB rgb = new PS_RGB(c);
                                memoryStream.WriteByte(rgb.B);
                                memoryStream.WriteByte(rgb.G);
                                memoryStream.WriteByte(rgb.R);
                                break;
                        }
                    }

                    for (x = 0; x < row_round; x++)
                    {
                        memoryStream.WriteByte(0);
                    }
                }
            }
            return new Bitmap(memoryStream);
        }

        private void BMP2TIM(Bitmap bitmap, TIMEncodingSettings settings)
        {
            TIMHeader.GenerateClut(bitmap, settings);
            using (BinaryWriter writer = new BinaryWriter(new FileStream(FilePath, FileMode.OpenOrCreate)))
            {
                TIMHeader.Write(writer.BaseStream);
                switch (TIMHeader.BPP)
                {
                    case 24:
                        for (int y = 0; y < bitmap.Height; y++)
                        {
                            for (int x = 0; x < bitmap.Width; x += 2)
                            {
                                Color color1 = bitmap.GetPixel(x, y);
                                Color color2 = bitmap.GetPixel(x + 1, y);
                                writer.Write((ushort)((color1.G << 8) | color1.R));
                                writer.Write((ushort)((color2.R << 8) | color1.B));
                                writer.Write((ushort)((color2.B << 8) | color2.G));
                            }
                        }
                        break;
                    case 16:
                        for (int y = 0; y < bitmap.Height; y++)
                        {
                            for (int x = 0; x < bitmap.Width; x++)
                            {
                                Color color = bitmap.GetPixel(x, y);
                                PS_RGB rgb = new PS_RGB(color.R, color.G, color.B);
                                writer.Write(rgb.ToRGBPSX(settings));
                            }
                        }
                        break;
                    case 8:
                        for (int y = 0; y < bitmap.Height; y++)
                        {
                            for (int x = 0; x < bitmap.Width; x += 2)
                            {
                                Color color1 = bitmap.GetPixel(x, y);
                                Color color2 = bitmap.GetPixel(x + 1, y);
                                ushort shortbuf = (ushort)((GetColorIndex(bitmap, color2) << 8) | GetColorIndex(bitmap, color1));
                                writer.Write(shortbuf);
                            }
                        }
                        break;
                    case 4:
                        for (int y = 0; y < bitmap.Height; y++)
                        {
                            for (int x = 0; x < bitmap.Width; x += 4)
                            {
                                Color color1 = bitmap.GetPixel(x, y);
                                Color color2 = bitmap.GetPixel(x + 1, y);
                                Color color3 = bitmap.GetPixel(x + 2, y);
                                Color color4 = bitmap.GetPixel(x + 3, y);
                                ushort shortbuf = (ushort)((GetColorIndex(bitmap, color4) << 12) | (GetColorIndex(bitmap, color3) << 8) | (GetColorIndex(bitmap, color2) << 4) | GetColorIndex(bitmap, color1));
                                writer.Write(shortbuf);
                            }
                        }
                        break;
                }
            }
        }

    }

    public class PS_RGB
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }

        public PS_RGB(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public PS_RGB(ushort data)
        {
            R = (byte)((data & 31) << 3);
            G = (byte)(((data >> 5) & 31) << 3);
            B = (byte)(((data >> 10) & 31) << 3);
        }

        public ushort ToRGBPSX(TIMEncodingSettings settings)
        {
            ushort result;
            result = (ushort)(R >> 3);
            result |= (ushort)((G >> 3) << 5);
            result |= (ushort)((B >> 3) << 10);

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

    public class PS_BITMAP
    {
        public int w, h;
        public int depth;
        public byte[] data;

        public PS_BITMAP(int width, int height, int bpp)
        {
            w = width;
            h = height;
            depth = bpp;
        }

        public void Write(MemoryStream memoryStream)
        {
            int x;
            int r = 0;
            int ret;

            if (depth == 16)
                depth = 24;

            memoryStream.WriteByte(Convert.ToByte('B'));
            memoryStream.WriteByte(Convert.ToByte('M'));

            // Calculate and write size of bitmap

            if (depth == 24)
                r = w * 3;
            else if (depth == 8)
                r = w;
            else if (depth == 4)
                r = w / 2;

            ret = r;

            if ((r & 3) == 3)
            {
                r |= 3;
                r++;
            }

            ret = r - ret;

            x = r * h;
            x += 54;

            if (depth == 8)
                x += 1024;
            else if (depth == 4)
                x += 64;

            memoryStream.Write(BitConverter.GetBytes(x), 0, 4);

            // Write bfReserved1 and bfReserved2 as zero
            memoryStream.Write(BitConverter.GetBytes(0), 0, 4);

            // Calculate and write data offset in file

            x = 54;

            if (depth == 8)
                x += 1024;
            else if (depth == 4)
                x += 64;

            memoryStream.Write(BitConverter.GetBytes(x), 0, 4);
            memoryStream.Write(BitConverter.GetBytes(40), 0, 4);
            memoryStream.Write(BitConverter.GetBytes(w), 0, 4); // Width
            memoryStream.Write(BitConverter.GetBytes(h), 0, 4); // Height
            memoryStream.Write(BitConverter.GetBytes(1), 0, 2);
            memoryStream.Write(BitConverter.GetBytes(depth), 0, 2); // Bits Per Pixel
            memoryStream.Write(BitConverter.GetBytes(0), 0, 4);
            memoryStream.Write(BitConverter.GetBytes(r * h), 0, 4); // Image data size
            memoryStream.Write(BitConverter.GetBytes(0), 0, 4);
            memoryStream.Write(BitConverter.GetBytes(0), 0, 4);
            memoryStream.Write(BitConverter.GetBytes(0), 0, 4);
            memoryStream.Write(BitConverter.GetBytes(0), 0, 4);
        }
    }

    public class TIMEncodingSettings
    {
        /// <summary>
        /// True for setting semi transparency bit
        /// </summary>
        public bool SetSemiTransparencyBit { get; set; }
        /// <summary>
        /// True for making black transparent
        /// </summary>
        public bool BlackTransparent { get; set; }
        /// <summary>
        /// True for making magic pink (255, 0, 255) transparent
        /// </summary>
        public bool MagicPinkTransparent { get; set; }
    }
    
}
