using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace PdfViewer.Extensions.Helpers
{
    public static class ImageResizer
    {
        private static ImageCodecInfo imageEncoder;

        public static Stream ResizeImage(Stream inStream, double maxDimension, long level)
        {
            var outStream = new MemoryStream();

            using (Image inImage = Image.FromStream(inStream))
            {
                double width;
                double height;

                if (inImage.Height < inImage.Width)
                {
                    width = maxDimension;
                    height = (maxDimension / inImage.Width) * inImage.Height;
                }
                else
                {
                    height = maxDimension;
                    width = (maxDimension / inImage.Height) * inImage.Width;
                }

                using (Bitmap bitmap = new Bitmap((int)width, (int)height))
                {
                    using (Graphics graphics = Graphics.FromImage(bitmap))
                    {
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.DrawImage(inImage, 0, 0, bitmap.Width, bitmap.Height);
                        var ici = ImageCodecInfo.GetImageDecoders().ToList();

                        if(inImage.RawFormat.Guid == ImageFormat.Bmp.Guid)
                            imageEncoder = ici.Find(x => x.FormatID == ImageFormat.Bmp.Guid);
                        else if (inImage.RawFormat.Guid == ImageFormat.Jpeg.Guid)
                            imageEncoder = ici.Find(x => x.FormatID == ImageFormat.Jpeg.Guid);
                        else if (inImage.RawFormat.Guid == ImageFormat.Gif.Guid)
                            imageEncoder = ici.Find(x => x.FormatID == ImageFormat.Gif.Guid);
                        else if (inImage.RawFormat.Guid == ImageFormat.Tiff.Guid)
                            imageEncoder = ici.Find(x => x.FormatID == ImageFormat.Tiff.Guid);
                        else if (inImage.RawFormat.Guid == ImageFormat.Png.Guid)
                            imageEncoder = ici.Find(x => x.FormatID == ImageFormat.Png.Guid);
                        else if(inImage.RawFormat.Guid == ImageFormat.Exif.Guid)
                            imageEncoder = ici.Find(x => x.FormatID == ImageFormat.Exif.Guid);
                        else if(inImage.RawFormat.Guid == ImageFormat.Wmf.Guid)
                            imageEncoder = ici.Find(x => x.FormatID == ImageFormat.Wmf.Guid);
                        else if (inImage.RawFormat.Guid == ImageFormat.Icon.Guid)
                            imageEncoder = ici.Find(x => x.FormatID == ImageFormat.Icon.Guid);

                        if (imageEncoder != null)
                        {
                            EncoderParameters ep = new EncoderParameters(1);
                            ep.Param[0] = new EncoderParameter(Encoder.Quality, level);
                            bitmap.Save(outStream, imageEncoder, ep);
                        }
                        else
                        {
                            graphics.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
                            bitmap.Save(outStream, inImage.RawFormat);
                        }
                    }
                }
            }

            return outStream;
        }

        public static void GetImageSize(Stream stream, out int width, out int height)
        {
            using (Image src_image = Image.FromStream(stream))
            {
                width = src_image.Width;
                height = src_image.Height;
            }
        }
    }
}