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
                    using (XImage img = XImage.FromStream(imageStream))
                    {
                        XGraphics gfx = XGraphics.FromPdfPage(page);
                        gfx.DrawImage(img, 0, 0);
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