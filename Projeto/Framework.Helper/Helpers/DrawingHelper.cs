using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Framework.Helper.Helpers
{
    public static class DrawingHelper
    {
        public static SizeF MeasureString(string text, Font font, int maxWidth)
        {
            Bitmap bitmap = new Bitmap(1, 1);
            Graphics graphics = Graphics.FromImage(bitmap);
            return graphics.MeasureString(text, font, maxWidth);
        }

        public static Bitmap CreateProfileImage(string name, int width, int height, Color backColor, Color foreColor, Font font,
            ContentAlignment fontContentAlignment = ContentAlignment.MiddleCenter)
        {
            name = StringHelper.GetInitials(name);

            Bitmap bitmap = new Bitmap(width, height);

            using (Graphics graphics = CreateGraphics(bitmap))
            {
                graphics.FillRectangle(new SolidBrush(backColor), 0, 0, width, height);

                SizeF textSize = graphics.MeasureString(name, font, SizeF.Empty, StringFormat.GenericDefault);
                float left = 0;
                float top = 0;
                switch (fontContentAlignment)
                {
                    case ContentAlignment.BottomLeft:
                        left = 2;
                        top = height - textSize.Height;
                        break;
                    case ContentAlignment.MiddleCenter:
                        left = (width / (float)2) - (textSize.Width / 2);
                        top = (height / (float)2) - (textSize.Height / 2) + 1;
                        break;

                    default:
                        throw new NotImplementedException();
                }

                graphics.DrawString(name, font, new SolidBrush(foreColor), left, top);
            }

            return bitmap;
        }

        public static Graphics CreateGraphics(Bitmap bitmap)
        {
            Graphics graphics = Graphics.FromImage(bitmap);
            return ConfigureGraphicBestQuality(graphics);
        }

        public static Graphics ConfigureGraphicBestQuality(Graphics graphics)
        {
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            return graphics;
        }

        public static Bitmap CreateRoundedTag(int width, int height, Color color, Color foreColor, string text, Font font, float cornerRadius = 2.5F)
        {
            Bitmap bitmap = new Bitmap(width, height);

            using (Graphics graphics = CreateGraphics(bitmap))
            {
                using (GraphicsPath path = CreateRoundedRectanglePath(new Rectangle(0, 0, width, height), cornerRadius))
                {
                    graphics.FillPath(new SolidBrush(color), path);

                    if (!string.IsNullOrEmpty(text))
                    {
                        SizeF size = graphics.MeasureString(text, font);
                        float left = (width / 2.0F) - (size.Width / 2.0F);
                        float top = (height / 2.0F) - (size.Height / 2.0F);
                        graphics.DrawString(text, font, new SolidBrush(Color.White), new RectangleF(left, top, size.Width + 1, size.Height));
                    }
                }
            }

            return bitmap;
        }

        public static GraphicsPath CreateRoundedRectanglePath(Rectangle bounds, float radius)
        {
            float diameter = radius * 2.0F;
            SizeF size = new SizeF(diameter, diameter);
            PointF point = new PointF(bounds.Location.X, bounds.Location.Y);
            RectangleF arc = new RectangleF(point, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            path.AddArc(arc, 180, 90);

            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        public static void FillRoundedRectangle(Graphics graphics, Brush brush, Rectangle bounds, float cornerRadius)
        {
            if (graphics == null)
            {
                throw new ArgumentNullException("graphics");
            }

            if (brush == null)
            {
                throw new ArgumentNullException("brush");
            }

            using (GraphicsPath path = CreateRoundedRectanglePath(bounds, cornerRadius))
            {
                graphics.FillPath(brush, path);
            }
        }
    }
}
