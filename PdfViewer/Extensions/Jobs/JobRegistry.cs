using FluentScheduler;
using System.Web;

namespace PdfViewer.Extensions.Jobs
{
    public class JobRegistry : Registry
    {
        public JobRegistry()
        {
            var directoryPath = HttpContext.Current.Server.MapPath("/Temp");
            Schedule(() => new DirectoryCleanUpJob(directoryPath)).ToRunNow().AndEvery(1).Hours();
        }
    }
}