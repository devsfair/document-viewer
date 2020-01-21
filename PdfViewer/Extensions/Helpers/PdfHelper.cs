using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.IO;

namespace PdfViewer.Extensions.Helpers
{
    public class PdfHelper
    {
        public PdfHelper()
        {
        }

        public void SaveImageAsPdf(Stream imageStream, string pdfFileName)
        {
            try
            {
                using (var document = new PdfDocument())
                {
                    PdfPage page = document.AddPage();
                    using (XImage img = XImage.FromStream(imageStream))
                    {
                        //// Calculate new height to keep image ratio
                        //var height = (int)(((double)width / (double)img.PixelWidth) * img.PixelHeight);

                        //// Change PDF Page size to match image
                        //page.Width = width;
                        //page.Height = height;

                        XGraphics gfx = XGraphics.FromPdfPage(page);
                        gfx.DrawImage(img, 0, 0);
                    }

                    document.Save(pdfFileName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}