using PdfViewer.Extensions.Helpers;
using PdfViewer.Extensions.Providers;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
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
            if (!Request.Content.IsMimeMultipartContent())
                return BadRequest("Unsupported media type.");

            if (Request.Content.Headers.ContentLength >= 4 * 1024 * 1024)
                return BadRequest("File is bigger than 4MB");

            var provider = await Request.Content.ReadAsMultipartAsync(new InMemoryMultipartFormDataStreamProvider());
            if (!provider.Files.Any())
                return BadRequest("You didn't upload any image.");

            var originalFile = provider.Files[0];
            if (!originalFile.Headers.ContentType.ToString().StartsWith("image"))
                return BadRequest("You must upload an image.");

            var inputStream = await originalFile.ReadAsStreamAsync();
            var pdfPath = $"~/Temp/{Guid.NewGuid()}.pdf";
            PdfHelper.SaveImageAsPdf(inputStream, pdfPath);

            //var fileName = string.Join(string.Empty, originalFile.Headers.ContentDisposition.FileName.Split(Path.GetInvalidFileNameChars()));
            //var image = Image.FromStream(inputStream);
            //var saveImagePath = $"Temp/{Guid.NewGuid()}_{fileName}";
            //image.Save(HttpContext.Current.Server.MapPath(saveImagePath));

            return Ok();
        }
    }
}