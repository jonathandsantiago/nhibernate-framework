using Framework.Helper.Extension;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;

namespace Framework.Helper.Helpers
{
    public static class ImageHelper
    {
        public static byte[] Resize(byte[] array, int newWidth, int maxHeight)
        {
            Image image = Resize(array.ToImage(), newWidth, maxHeight);

            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, image.RawFormat);
                return ms.ToArray();
            }
        }

        public static Image Resize(Image image, int newWidth, int maxHeight)
        {
            int newHeight = image.Height * newWidth / image.Width;

            if (newHeight > maxHeight)
            {
                newWidth = image.Width * maxHeight / image.Height;
                newHeight = maxHeight;
            }

            Bitmap bitmap = new Bitmap(newWidth, newHeight);

            using (Graphics graphic = Graphics.FromImage(bitmap))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return bitmap;
        }
    }
}
