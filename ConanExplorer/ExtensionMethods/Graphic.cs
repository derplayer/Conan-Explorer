using ConanExplorer.Conan;
using ConanExplorer.Conan.Headers;
using ConanExplorer.Conan.Script;
using ConanExplorer.Conan.Script.Elements;
using ConanExplorer.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConanExplorer.ExtensionMethods
{
    /// <summary>
    /// Collection of static methods for drawing conan message boxes and manipulating images
    /// </summary>
    static class Graphic
    {
        public static Bitmap WindowBitmap = (Bitmap)Resources.ResourceManager.GetObject("WINDOW");

        public static int NearestColor(CLUTEntry[] entries, Color color)
        {
            int nearest = -1;
            int difference = 765;
            for (int i = 0; i < entries.Length; i++)
            {
                CLUTEntry entry = entries[i];
                int diff = ColorDifference(entry.Color, color);
                if (diff < difference)
                {
                    nearest = i;
                    difference = diff;
                }
            }
            return nearest;
        }

        public static int ColorDifference(Color col1, Color col2)
        {
            int diffR = Math.Abs(col1.R - col2.R);
            int diffG = Math.Abs(col1.G - col2.G);
            int diffB = Math.Abs(col1.B - col2.B);

            return diffR + diffG + diffB;
        }

        public static Color[] FontColors = new Color[]
            {
                Color.FromArgb(0, 0, 0, 0),
                Color.FromArgb(255, 255, 255, 255),
                Color.FromArgb(255, 0, 0, 248),
                Color.FromArgb(255, 248, 0, 0),
                Color.FromArgb(255, 0, 248, 248),
                Color.FromArgb(255, 255, 255, 0),
                Color.FromArgb(255, 0, 248, 0),
                Color.FromArgb(255, 0, 0, 80),
                Color.FromArgb(255, 96, 0, 0),
                Color.FromArgb(255, 144, 144, 144),
                Color.FromArgb(255, 0, 0, 160),
                Color.FromArgb(255, 192, 168, 0),
                Color.FromArgb(255, 0, 176, 192),
                Color.FromArgb(255, 192, 192, 0),
                Color.FromArgb(255, 0, 160, 0),
                Color.FromArgb(255, 192, 0, 0)
            };

        public static void DrawDialogueWindow(Graphics graphics, int width, int height, int count = 1)
        {
            int height_offset = 0;
            for (int c = 0; c < count; c++)
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Rectangle rectangle = new Rectangle(j * 16, i * 16 + height_offset, 16, 16);
                        if (i == 0)
                        {
                            if (j == 0)
                            {
                                //corner left
                                graphics.DrawImage(WindowBitmap, rectangle, 160, 64, 16, 16, GraphicsUnit.Pixel);
                            }
                            else if (j == width - 1)
                            {
                                //corner right
                                graphics.DrawImage(WindowBitmap, rectangle, 160, 48, 16, 16, GraphicsUnit.Pixel);
                            }
                            else
                            {
                                //top
                                graphics.DrawImage(WindowBitmap, rectangle, 160, 16, 16, 16, GraphicsUnit.Pixel);
                            }
                        }
                        else if (i == height - 1)
                        {
                            if (j == 0)
                            {
                                //corner left
                                WindowBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                graphics.DrawImage(WindowBitmap, rectangle, 160, 176, 16, 16, GraphicsUnit.Pixel);
                                WindowBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            }
                            else if (j == width - 1)
                            {
                                //corner right
                                WindowBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                graphics.DrawImage(WindowBitmap, rectangle, 160, 192, 16, 16, GraphicsUnit.Pixel);
                                WindowBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            }
                            else
                            {
                                //bottom
                                WindowBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                                graphics.DrawImage(WindowBitmap, rectangle, 160, 224, 16, 16, GraphicsUnit.Pixel);
                                WindowBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            }
                        }
                        else
                        {
                            if (j == 0)
                            {
                                //left
                                graphics.DrawImage(WindowBitmap, rectangle, 160, 32, 16, 16, GraphicsUnit.Pixel);
                            }
                            else if (j == width - 1)
                            {
                                //right
                                WindowBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                                graphics.DrawImage(WindowBitmap, rectangle, 80, 32, 16, 16, GraphicsUnit.Pixel);
                                WindowBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            }
                            else
                            {
                                //middle
                                graphics.DrawImage(WindowBitmap, rectangle, 160, 0, 16, 16, GraphicsUnit.Pixel);
                            }
                        }
                    }
                }
                height_offset += 96;
            }
        }

        public static void DrawSelectionWindow(Graphics graphics, int width, int height, int color = 0)
        {
            int[] offsets = new int[] { 144, 176, 192, 208 };
            int colorOffset = offsets[color];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Rectangle rectangle = new Rectangle(j * 16, i * 16, 16, 16);
                    if (i == 0)
                    {
                        if (j == 0)
                        {
                            //corner top left
                            graphics.DrawImage(WindowBitmap, rectangle, colorOffset, 64, 16, 16, GraphicsUnit.Pixel);
                        }
                        else if (j == width - 1)
                        {
                            //corner top right
                            graphics.DrawImage(WindowBitmap, rectangle, colorOffset, 48, 16, 16, GraphicsUnit.Pixel);
                        }
                        else
                        {
                            //top
                            graphics.DrawImage(WindowBitmap, rectangle, colorOffset, 16, 16, 16, GraphicsUnit.Pixel);
                        }
                    }
                    else if (i == height - 1)
                    {
                        if (j == 0)
                        {
                            //corner bottom left
                            WindowBitmap.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                            graphics.DrawImage(WindowBitmap, rectangle, 240 - colorOffset, 192, 16, 16, GraphicsUnit.Pixel);
                            WindowBitmap.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                        }
                        else if (j == width - 1)
                        {
                            //corner bottom right
                            WindowBitmap.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                            graphics.DrawImage(WindowBitmap, rectangle, 240 - colorOffset, 176, 16, 16, GraphicsUnit.Pixel);
                            WindowBitmap.RotateFlip(RotateFlipType.RotateNoneFlipXY);
                        }
                        else
                        {
                            //bottom
                            WindowBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                            graphics.DrawImage(WindowBitmap, rectangle, colorOffset, 224, 16, 16, GraphicsUnit.Pixel);
                            WindowBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                        }
                    }
                    else
                    {
                        if (j == 0)
                        {
                            //left
                            graphics.DrawImage(WindowBitmap, rectangle, colorOffset, 32, 16, 16, GraphicsUnit.Pixel);
                        }
                        else if (j == width - 1)
                        {
                            //right
                            WindowBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                            graphics.DrawImage(WindowBitmap, rectangle, 240 - colorOffset, 32, 16, 16, GraphicsUnit.Pixel);
                            WindowBitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        }
                        else
                        {
                            //middle
                            graphics.DrawImage(WindowBitmap, rectangle, colorOffset, 0, 16, 16, GraphicsUnit.Pixel);
                        }
                    }
                }
            }
        }

        public static void DrawDialogueText(Graphics graphics, ScriptFile scriptFile, ScriptMessage message)
        {
            Color fontColor = Color.FromArgb(255, 255, 255);
            int line = 0;
            int left = 15;
            int top = 7;
            int windowCount = 1;
            int matchIndex = 0;
            MatchCollection matches = message.Matches;
            for (int i = 0; i < message.Content.Length; i++)
            {
                if (message.Content[i] == '\n' || message.Content[i] == '\r') continue;
                if (matchIndex < matches.Count)
                {
                    if (i == matches[matchIndex].Index + 1)
                    {
                        Match match = matches[matchIndex];
                        if (match.Groups[1].Value == "COL")
                        {
                            int colorIndex = 0;
                            int.TryParse(match.Groups[2].Value, out colorIndex);
                            if (colorIndex > 15)
                            {
                                fontColor = FontColors[0];
                            }
                            else
                            {
                                fontColor = FontColors[colorIndex];
                            }
                        }
                        else if (match.Groups[1].Value == "LF")
                        {
                            line++;
                            top += 16;
                            left = 15;
                        }
                        else if (match.Groups[1].Value == "CLR")
                        {
                            left = 15;
                            top = windowCount * 96 + 7;
                            windowCount++;
                        }
                        matchIndex++;
                        i += match.Length - 2;
                        continue;
                    }
                }

                char character = message.Content[i];
                Font font = scriptFile.Font;
                if (left >= 191) continue;
                if (scriptFile.IsValidChar(character))
                {
                    i++;
                    if (i == message.Content.Length)
                    {
                        FontCharacter fontCharacter = new FontCharacter(new FontSymbol(character), font);
                        Bitmap bitmap = fontCharacter.GetBitmapTransparent(fontColor);
                        graphics.DrawImage(bitmap, new Rectangle(left, top, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel);
                        left += 16;
                    }
                    else if (scriptFile.IsValidChar(message.Content[i]))
                    {
                        FontCharacter fontCharacter = new FontCharacter(new FontSymbol(character, message.Content[i]), font);
                        Bitmap bitmap = fontCharacter.GetBitmapTransparent(fontColor);
                        graphics.DrawImage(bitmap, new Rectangle(left, top, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel);
                        left += 16;
                    }
                    else
                    {
                        FontCharacter fontCharacter = new FontCharacter(new FontSymbol(character), font);
                        Bitmap bitmap = fontCharacter.GetBitmapTransparent(fontColor);
                        graphics.DrawImage(bitmap, new Rectangle(left, top, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel);
                        left += 16;
                    }
                }
                else
                {
                    continue;
                }
            }
        }

        public static void DrawSelectionText(Graphics graphics, ScriptFile scriptFile, ScriptMessage message, bool alternative = false)
        {
            Color fontColor = Color.FromArgb(255, 255, 255);
            Font font = scriptFile.Font;
            int left = 15;
            int top = 7;
            string[] lines = null;
            
            if(alternative)
                lines = ScriptParser.TextToLines(message.ContentAlternative);
            else
                ScriptParser.TextToLines(message.Content);

            bool open = false;
            foreach (string line in lines)
            {
                if (line.StartsWith("%SEL:"))
                {
                    open = true;
                    continue;
                }

                if (line.StartsWith("%END:"))
                {
                    open = false;
                    continue;
                }

                if (open)
                {
                    string[] splitted = line.Split(',');
                    if (splitted.Length < 3) continue;

                    string displayText = splitted[2];

                    for (int i = 0; i < displayText.Length; i++)
                    {
                        char character = displayText[i];

                        if (left >= 223 || top >= 159) continue;
                        if (scriptFile.IsValidChar(character))
                        {
                            i++;
                            if (i == displayText.Length)
                            {
                                FontCharacter fontCharacter = new FontCharacter(new FontSymbol(character), font);
                                Bitmap bitmap = fontCharacter.GetBitmapTransparent(fontColor);
                                graphics.DrawImage(bitmap, new Rectangle(left, top, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel);
                                left += 16;
                            }
                            else if (scriptFile.IsValidChar(displayText[i]))
                            {
                                FontCharacter fontCharacter = new FontCharacter(new FontSymbol(character, displayText[i]), font);
                                Bitmap bitmap = fontCharacter.GetBitmapTransparent(fontColor);
                                graphics.DrawImage(bitmap, new Rectangle(left, top, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel);
                                left += 16;
                            }
                            else
                            {
                                FontCharacter fontCharacter = new FontCharacter(new FontSymbol(character), font);
                                Bitmap bitmap = fontCharacter.GetBitmapTransparent(fontColor);
                                graphics.DrawImage(bitmap, new Rectangle(left, top, 16, 16), 0, 0, 16, 16, GraphicsUnit.Pixel);
                                left += 16;
                            }
                        }
                    }
                }
                top += 16;
                left = 15;
            }
        }

        /// <summary>
        /// Resizes an given image to the given width and height and returns it as a bitmap
        /// </summary>
        /// <param name="image">Image that will be resized</param>
        /// <param name="width">Width of the resized bitmap</param>
        /// <param name="height">Height of the resized bitmap</param>
        /// <returns>Resized bitmap</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var resultRectangle = new Rectangle(0, 0, width, height);
            var resultImage = new Bitmap(width, height);

            resultImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(resultImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, resultRectangle, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return resultImage;
        }

        /// <summary>
        /// Combines a list of bitmaps to a bitmap with the given width
        /// and a needed height to display all bitmaps in the given list
        /// </summary>
        /// <param name="bitmapList">List of bitmaps, each having the same size</param>
        /// <param name="width">The width as how many bitmaps should fit in a row</param>
        /// <returns>Bitmap with the given width and a height that is needed to display all the bitmaps</returns>
        public static Bitmap CombineBitmaps(List<Bitmap> bitmapList, int width)
        {
            if (bitmapList.Count == 0) return new Bitmap(16, 16);

            int bitmapHeight = bitmapList[0].Height;
            int bitmapWidth = bitmapList[0].Width;
            if (bitmapList.Any(bitmap => bitmap.Height != bitmapHeight || bitmap.Width != bitmapWidth))
            {
                throw new ArgumentException("Not all bitmaps in the given list have the same size!");
            }

            int height = (int)Math.Ceiling((double)bitmapList.Count / width);
            Bitmap result = new Bitmap(width * bitmapWidth, height * bitmapHeight);

            using (Graphics graph = Graphics.FromImage(result))
            {
                graph.Clear(Color.Black);

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (i * width + j >= bitmapList.Count) break;
                        graph.DrawImage(bitmapList[i * width + j], new Rectangle(j * bitmapWidth, i * bitmapHeight, bitmapWidth, bitmapHeight));
                    }
                }
            }
            return result;
        }
    }
}
