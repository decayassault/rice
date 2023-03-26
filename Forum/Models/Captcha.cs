using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.ComponentModel;
using System.Threading;
using Forum.Data;

namespace Forum.Models
{
    internal sealed class Captcha
    {

        internal async static Task<Res> GetImageCode()
        {
            var res = await GenerateImage();
            res.image = "<img src='data:image/gif;base64," + res.image
                + "' />";

            return res;
        }

        internal static void RefreshLogRegPages()
        {
            while (MvcApplication.True)
            {                
                LoginData.InitPage();
                RegistrationData.InitPage();
                Thread.Sleep(1000);
            }
        }
        
        private static Task<ImageCodecInfo> 
                        GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = MvcApplication.Zero; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return Task.FromResult(encoders[j]);
            }
            return null;
        }

        internal struct Res
        {
            public string message;
            public string image;
        }

        private async static Task<Res> GenerateImage()
        {
            // Создаём новое 32-битное битмап-изображение.
            
            Bitmap bitmap = new Bitmap(
              150,
              30);

            // Создаём графический объект для рисования.

            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            Rectangle rect = new Rectangle
                (MvcApplication.Zero, MvcApplication.Zero, 150, 30);

            // Заполняем фон.

            HatchBrush hatchBrush = new HatchBrush(
              HatchStyle.SmallConfetti,
              Color.LightGray,
              Color.White);
            g.FillRectangle(hatchBrush, rect);

            // Устанавливаем шрифт текста.

            SizeF size;
            float fontSize = rect.Height + MvcApplication.One;
            Font font;
            var random = new Random();
            // Настраиваем размер шрифта, чтобы текст вписался в картинку.
            string text = random.Next(10000,99999).ToString();
            var res=new Res{message=text};
           
            do
            {
                fontSize--;
                font = new Font(
                  "Buxton Sketch",
                  fontSize,
                  FontStyle.Bold);
                size = g.MeasureString(text, font);
            } while (size.Width > rect.Width);

            // Устанавливаем формат текста.

            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            format.LineAlignment = StringAlignment.Center;

            // Создаём путь используя текст и вписываем его рандомно.
            
            GraphicsPath path = new GraphicsPath();
            path.AddString(
              text,
              font.FontFamily,
              (int)font.Style,
              font.Size, rect,
              format);
            float v = 4F;
            PointF[] points =
      {
        new PointF(
          random.Next(rect.Width) / v,
          random.Next(rect.Height) / v),
        new PointF(
          rect.Width - random.Next(rect.Width) / v,
          random.Next(rect.Height) / v),
        new PointF(
          random.Next(rect.Width) / v,
          rect.Height - random.Next(rect.Height) / v),
        new PointF(
          rect.Width - random.Next(rect.Width) / v,
          rect.Height - random.Next(rect.Height) / v)
      };
            Matrix matrix = new Matrix();
            matrix.Translate(0F, 0F);
            path.Warp(points, rect, matrix, WarpMode.Perspective, 0F);

            // Рисуем текст.

            hatchBrush = new HatchBrush(
              HatchStyle.LargeConfetti,
              Color.LightGray,
              Color.DarkGray);
            g.FillPath(hatchBrush, path);

            // Добавляем случайный шум.

            int m = Math.Max(rect.Width, rect.Height);
            for (int i = MvcApplication.Zero;
                    i < (int)(rect.Width * rect.Height / 30F); i++)
            {
                int x = random.Next(rect.Width);
                int y = random.Next(rect.Height);
                int w = random.Next(m / 50);
                int h = random.Next(m / 50);
                g.FillEllipse(hatchBrush, x, y, w, h);
            }

            // Освобождаем ресурсы.

            font.Dispose();
            hatchBrush.Dispose();
            g.Dispose();            

            ImageCodecInfo myImageCodecInfo = await GetEncoderInfo("image/gif"); ;
            EncoderParameter encCompressionrParameter = new EncoderParameter
                (System.Drawing.Imaging.Encoder.Compression, (long)EncoderValue.CompressionLZW); ;
            EncoderParameter encQualityParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 0L);
            EncoderParameters myEncoderParameters = new EncoderParameters(2);
            myEncoderParameters.Param[MvcApplication.Zero] = encCompressionrParameter;
            myEncoderParameters.Param[MvcApplication.One] = encQualityParameter;
            byte[] bytes = (byte[])TypeDescriptor.GetConverter(bitmap)
                .ConvertTo(bitmap, typeof(byte[]));
            res.image = Convert.ToBase64String(bytes);
            
            return res;
        }
    }
}