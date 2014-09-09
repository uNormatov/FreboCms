using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using FCore.Helper;


namespace FModules
{
    public class ImageScaleHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {

            string path = ValidationHelper.GetString(context.Request.QueryString["path"], string.Empty);
            int width = ValidationHelper.GetInteger(context.Request.QueryString["width"], 50);
            int height = ValidationHelper.GetInteger(context.Request.QueryString["height"], 50);
            path = HttpContext.Current.Server.MapPath(path);

            if (File.Exists(path))
            {
                byte[] file = GenerateThumbnail(path, width, height);
                context.Response.Clear();
                context.Response.ContentType = "image/jpeg";
                context.Response.BinaryWrite(file);
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get { return true; }
        }

        private byte[] GenerateThumbnail(string filename, int width, int height)
        {

            try
            {
                Image image = new Bitmap(filename);
                Image newImage = Resize(image, width, height, RotateFlipType.RotateNoneFlipNone);
                MemoryStream imageStream = new MemoryStream();
                newImage.Save(imageStream, System.Drawing.Imaging.ImageFormat.Jpeg);

                byte[] imageContent = new Byte[imageStream.Length];
                imageStream.Position = 0;
                imageStream.Read(imageContent, 0, (int)imageStream.Length);
                return imageContent;
            }
            catch (Exception ex)
            {

            }
            return null;
        }

        public Image Resize(Image image, int width, int height, RotateFlipType rotateFlipType)
        {
            var rotatedImage = image.Clone() as Image;
            rotatedImage.RotateFlip(rotateFlipType);
            var newSize = CalculateResizedDimensions(rotatedImage, width, height);

            var resizedImage = new Bitmap(newSize.Width, newSize.Height, PixelFormat.Format32bppArgb);
            resizedImage.SetResolution(72, 72);

            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var attribute = new ImageAttributes())
                {
                    attribute.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(rotatedImage, new Rectangle(new Point(0, 0), newSize), 0, 0, rotatedImage.Width, rotatedImage.Height, GraphicsUnit.Pixel, attribute);
                }
            }

            return resizedImage;
        }

        /// <summary>
        /// Calculates resized dimensions for an image, preserving the aspect ratio.
        /// </summary>
        /// <param name="image">Image instance</param>
        /// <param name="desiredWidth">desired width</param>
        /// <param name="desiredHeight">desired height</param>
        /// <returns>Size instance with the resized dimensions</returns>
        private static Size CalculateResizedDimensions(Image image, int desiredWidth, int desiredHeight)
        {
            var widthScale = (double)desiredWidth / image.Width;
            var heightScale = (double)desiredHeight / image.Height;
            var scale = widthScale < heightScale ? widthScale : heightScale;
            return new Size
            {
                Width = (int)(scale * image.Width),
                Height = (int)(scale * image.Height)
            };
        }
    }
}
