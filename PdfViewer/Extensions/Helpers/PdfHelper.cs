using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.IO;
using System.Web;

namespace PdfViewer.Extensions.Helpers
{
    public static class PdfHelper
    {
        public static void SaveImageAsPdf(Stream imageStream, string fileName)
        {
            try
            {
                using (var document = new PdfDocument())
                {
                    PdfPage page = document.AddPage();
                    using (XImage image = XImage.FromStream(imageStream))
                    {
                        double imageWidth = image.PointWidth;
                        double imageHeight = image.PointHeight;
                        double pageWidth = page.Width;
                        double pageHeight = page.Height;

                        XGraphics gfx = XGraphics.FromPdfPage(page);
                        gfx.DrawImage(image, (pageWidth-imageWidth)/2, (pageHeight-imageHeight)/2);
                    }

                    var physicalPath = HttpContext.Current.Server.MapPath(fileName);
                    document.Save(physicalPath);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}