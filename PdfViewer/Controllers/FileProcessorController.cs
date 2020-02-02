using PdfViewer.Extensions.Helpers;
using PdfViewer.Extensions.Providers;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace PdfViewer.Controllers
{
    [RoutePrefix("api/file-processor")]
    public class FileProcessorController : ApiController
    {
        [Route("process-image")]
        [HttpPost]
        public async Task<IHttpActionResult> GetPdf()
        {
            try
            {
                if (!Request.Content.IsMimeMultipartContent())
                    return BadRequest("Unsupported media type.");

                var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider());
                if (!provider.Files.Any())
                    return BadRequest("You didn't upload any image.");

                var originalFile = provider.Files[0];
                if (!originalFile.Headers.ContentType.ToString().StartsWith("image"))
                    return BadRequest("You must upload an image.");

                var originalFileName = string.Join(string.Empty, originalFile.Headers.ContentDisposition.FileName.Split(Path.GetInvalidFileNameChars()));
                var fileName = Guid.NewGuid().ToString();
                var extension = Path.GetExtension(originalFileName);
                var pdfPath = $"/Temp/{fileName}.pdf";
                using (var inputStream = await originalFile.ReadAsStreamAsync())
                {
                    ImageResizer.GetImageSize(inputStream, out int width, out int height);
                    if (width > 600 || height > 600)
                    {
                        var resizedImageStream = ImageResizer.ResizeImage(inputStream, 600, 100L);
                        PdfHelper.SaveImageAsPdf(resizedImageStream, pdfPath);
                        if (resizedImageStream != null)
                            resizedImageStream.Dispose();
                    }
                    else
                    {
                        PdfHelper.SaveImageAsPdf(inputStream, pdfPath);
                    }

                    // Save original file
                    using (var image = Image.FromStream(inputStream))
                        image.Save(HttpContext.Current.Server.MapPath($"/Temp/{fileName}{extension}"));
                }

                return Ok(new { url = pdfPath, fileName = $"{fileName}.pdf" });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }            
        }
    }
}