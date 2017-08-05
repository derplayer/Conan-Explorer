using System;
using System.Drawing;
using ConanExplorer.ExtensionMethods;
using System.Drawing.Drawing2D;
using System.Windows;

namespace ConanExplorer.Conan
{
    public class FontCharacter
    {
        public byte[] Data { get; set; } = new byte[32];
        public short Index { get; set; } = -1;
        public string Symbol { get; set; } = "";


        public FontCharacter() { }

        public FontCharacter(FontSymbol symbol, Font font)
        {
            Symbol = symbol.Symbol;
            SetSymbol(symbol, font);
        }

        public FontCharacter(short index)
        {
            Index = index;
        }

        public FontCharacter(byte[] data, short index, string symbol)
        {
            Data = data;
            Index = index;
            Symbol = symbol;
        }


        public void SetSymbol(FontSymbol symbol, Font font)
        {
            Bitmap bitmap = new Bitmap(16, 16);
            using (Graphics graphic = Graphics.FromImage(bitmap))
            {
                RectangleF firstChar = new RectangleF(-2, 0, 16, 16);
                RectangleF secondChar = new RectangleF(6, 0, 16, 16);

                graphic.Clear(Color.Black);
                graphic.SmoothingMode = SmoothingMode.None;
                graphic.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphic.PixelOffsetMode = PixelOffsetMode.None;

                if (symbol.Splitted)
                {
                    graphic.DrawString(symbol.Character1.ToString(), font, Brushes.White, firstChar);
                    graphic.DrawString(symbol.Character2.ToString(), font, Brushes.White, secondChar);
                }
                else
                {
                    //TODO make non splitted characters bigger or in center? if bigger need two fonts/sizes
                    graphic.DrawString(symbol.Character1.ToString(), font, Brushes.White, firstChar);
                }
            }
            SetBitmap(bitmap);
        }

        public Bitmap GetBitmap()
        {
            Bitmap result = new Bitmap(16, 16);
            byte[] table = new byte[256];

            for (int i = 0; i < 32; i++)
            {
                byte currentByte = Data[i];
                for (int j = 7; j >= 0; j--)
                {
                    table[i * 8 + (7 - j)] = (byte)(currentByte & (1 << j));
                }
            }

            using (Graphics graph = Graphics.FromImage(result))
            {
                Rectangle imageSize = new Rectangle(0, 0, 16, 16);
                graph.FillRectangle(Brushes.White, imageSize);

                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        Color color = table[i * 16 + j] == 0 ? Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 255, 255);
                        result.SetPixel(j, i, color);
                    }
                }
                return result;
            }
        }

        public Bitmap GetBitmapTransparent(Color fontColor)
        {
            Bitmap result = new Bitmap(16, 16);
            byte[] table = new byte[256];

            for (int i = 0; i < 32; i++)
            {
                byte currentByte = Data[i];
                for (int j = 7; j >= 0; j--)
                {
                    table[i * 8 + (7 - j)] = (byte)(currentByte & (1 << j));
                }
            }

            using (Graphics graph = Graphics.FromImage(result))
            {
                Rectangle imageSize = new Rectangle(0, 0, 16, 16);
                graph.FillRectangle(Brushes.White, imageSize);

                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        Color color = table[i * 16 + j] == 0 ? Color.FromArgb(0, 0, 0, 0) : fontColor;
                        result.SetPixel(j, i, color);
                    }
                }
                return result;
            }
        }

        public void SetBitmap(Bitmap bitmap)
        {
            if (bitmap.Height != 16 || bitmap.Width != 16) throw new ArgumentException("The given bitmap is not 16 x 16 pixels in size.");

            byte[] table = new byte[256];
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    Color color = bitmap.GetPixel(j, i);                   
                    if (color.R > 128 && color.G > 128 && color.B > 128)
                    {
                        table[i*16 + j] = 1;
                    }
                }
            }

            for (int i = 0; i < 32; i++)
            {
                Data[i] = 0;
                for (int j = 7; j >= 0; j--)
                {
                    Data[i] |= (byte)(table[i*8 + (7 - j)] << j);
                }
            }
        }

    }
}
