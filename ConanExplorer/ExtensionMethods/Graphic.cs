using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConanExplorer.ExtensionMethods
{
    /// <summary>
    /// Collection of static methods for manipulating images
    /// </summary>
    static class Graphic
    {
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
