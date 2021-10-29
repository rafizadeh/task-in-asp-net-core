using Microsoft.AspNetCore.Http;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Common.Extensions
{
    public static class FileExtension
    {
        public const int ImageMinimumBytes = 512;

        public static async Task<string> SaveAsAsync(this IFormFile file, string path)
        {
            try
            {
                string fileName = GetNewFileName() + file.GetExtension();

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fullPath = Path.Combine(path, fileName);

                using (FileStream stream = new FileStream(fullPath, FileMode.Create)) await file.CopyToAsync(stream);

                return fileName;
            }
            catch { return null; }
        }

        public static async Task<string> SaveAsAsync(this IFormFile file, string path, string filename)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fullPath = Path.Combine(path, filename);

                using (FileStream stream = new FileStream(fullPath, FileMode.Create)) await file.CopyToAsync(stream);

                return filename;
            }
            catch
            {
                throw;
            }
        }

        public static void ImageResize(this IFormFile file, string path, Size size, int rotationAngle, long quality = 100, int fontEmSize = 15)
        {
            try
            {
                //Thumbnain yaradaq, şəkil təmizliyi maksimum olacaq.
                using Bitmap bitmap = new Bitmap(file.OpenReadStream());

                Size originalSize = new Size(bitmap.Width, bitmap.Height);

                Size newSize = originalSize.GetAppropriateSize(size); // Thumbnail ölçüsü (width = xxx) (height = xxx)

                using Bitmap thumb = new Bitmap(bitmap, newSize);

                using Graphics graphics = Graphics.FromImage(thumb);

                #region Təmizlik paramterləri
                graphics.Cleaning();
                #endregion

                //Şəkil Codec yaradırıq. Sondakı 1 index dəyəridir.
                ImageCodecInfo imageCodecInfo = ImageCodecInfo.GetImageEncoders()[1];

                //Encoding paramter maksimum 100L gedir o zaman şəkilin həcmi çox olur.
                EncoderParameters encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                //Şəkili yaradırıq və dəyərlərini üstəki kimi daxil edirik.
                graphics.DrawImage(bitmap, new Rectangle(0, 0, thumb.Width, thumb.Height));

                thumb.Save(path, imageCodecInfo, encoderParameters);
            }
            catch
            {
                throw;
            }
        }

        public static Bitmap WriteText(this IFormFile file, string text, int fontEmSize)
        {
            using Bitmap bitmap = new Bitmap(file.OpenReadStream());
            bitmap.WriteText(text, fontEmSize);
            return bitmap;
        }

        public static string WriteTextAndSave(this IFormFile file, string path, string text, int fontEmSize)
        {
            try
            {
                string fileName = GetNewFileName() + file.GetExtension();

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fullPath = Path.Combine(path, fileName);

                using Bitmap bitmap = new Bitmap(file.OpenReadStream());

                bitmap.WriteTextAndSave(fullPath, text, fontEmSize);

                return fileName;
            }
            catch { return null; }
        }

        public static void WriteText(this Bitmap bitmap, string text, int fontEmSize)
        {
            using Graphics graphics = Graphics.FromImage(bitmap);
            graphics.WriteText(text, fontEmSize, bitmap.Size);
        }

        public static void WriteTextAndSave(this Bitmap bitmap, string path, string text, int fontEmSize)
        {
            bitmap.WriteText(text, fontEmSize);
            bitmap.Save(path);
        }

        public static void WriteText(this Graphics graphics, string text, int fontEmSize, Size size)
        {
            Brush brush = new SolidBrush(Color.FromArgb(128, Color.Black));
            Font font = new Font("Visby CF - Bold", fontEmSize, FontStyle.Bold, GraphicsUnit.Pixel);
            SizeF textSize = graphics.MeasureString(text, font);
            graphics.TranslateTransform(size.Width / 2, size.Height / 2);
            graphics.RotateTransform(45);
            graphics.DrawString(text, font, brush, -(textSize.Width / 2), -(textSize.Height / 2));
        }

        public static void Cleaning(this Graphics graphics)
        {
            #region Təmizlik paramterləri
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;
            //graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            //graphics.SmoothingMode = SmoothingMode.HighQuality;
            #endregion
        }

        public static bool IsImage(this IFormFile file, bool nullCheck = true)
        {
            if (nullCheck && file == null) return true;

            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (!string.Equals(file.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/pjpeg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/x-png", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "image/png", StringComparison.OrdinalIgnoreCase)) return false;

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            var fileExtension = Path.GetExtension(file.FileName);
            if (!string.Equals(fileExtension, ".jpg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(fileExtension, ".png", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(fileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase)) return false;

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!file.OpenReadStream().CanRead) return false;
                //------------------------------------------
                //   Check whether the image size exceeding the limit or not
                //------------------------------------------ 
                if (file.Length < ImageMinimumBytes) return false;

                byte[] buffer = new byte[ImageMinimumBytes];
                file.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                string content = Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline)) return false;

            }
            catch { return false; }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            try
            { using Bitmap bitmap = new Bitmap(file.OpenReadStream()); }
            catch { return false; }
            finally { file.OpenReadStream().Position = 0; }

            return true;
        }

        public static bool IsVideo(this IFormFile file)
        {
            if (file == null) return false;

            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (!string.Equals(file.ContentType, "video/mp4", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "video/mov", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "video/avi", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "video/wmv", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(file.ContentType, "video/flv", StringComparison.OrdinalIgnoreCase)) return false;

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            var fileExtension = Path.GetExtension(file.FileName);
            if (!string.Equals(fileExtension, ".mp4", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(fileExtension, ".mov", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(fileExtension, ".flv", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(fileExtension, ".avi", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(fileExtension, ".wmv", StringComparison.OrdinalIgnoreCase)) return false;

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!file.OpenReadStream().CanRead) return false;
                //------------------------------------------
                //   Check whether the image size exceeding the limit or not
                //------------------------------------------ 
                if (file.Length < ImageMinimumBytes) return false;

                byte[] buffer = new byte[ImageMinimumBytes];
                file.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                string content = Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline)) return false;

            }
            catch { return false; }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            return true;
        }

        public static Size GetAppropriateSize(this Size originalSize, Size size)
        {
            if (size.Width == -1)
            {
                size.Width = (originalSize.Width * (size.Height * 100) / originalSize.Height) / 100;
            }
            else if (size.Height == -1)
            {
                size.Height = (originalSize.Height * (size.Width * 100) / originalSize.Width) / 100;
            }
            return size;
        }

        public static string GetExtension(this IFormFile file)
        {
            return Path.GetExtension(file.FileName).ToLower();
        }

        public static string GetNewFileName()
        {
            return $"{DateTime.Now:dd-MM-yyyy-HH-mm-ss}-{Guid.NewGuid().ToString().Substring(0, 6).ToUpper()}";
        }
    }
}