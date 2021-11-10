using System;
using System.Drawing;
using System.IO;

namespace Framework.Helper.Extension
{
    public static class ImageExtensions
    {        
        public static Image ToImage(this byte[] value)
        {
            using (MemoryStream memoryStream = new MemoryStream(value))
            {
                return Image.FromStream(memoryStream);
            }
        }

        public static Image MaxSize(this Image image, int maxWidth, int maxHeight)
        {
            double ratioX = (double)maxWidth / image.Width;
            double ratioY = (double)maxHeight / image.Height;
            double ratio = Math.Min(ratioX, ratioY);

            int newWidth = (int)(image.Width * ratio);
            int newHeight = (int)(image.Height * ratio);

            Bitmap newImage = new Bitmap(newWidth, newHeight);

            using (Graphics graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            }

            return newImage;
        }

        public static string ToBase64(this Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return string.Format("data:image/png;base64,{0}", Convert.ToBase64String(ms.ToArray()));
            }
        }
    }
}