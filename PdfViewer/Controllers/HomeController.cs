using System.Web.Mvc;

namespace PdfViewer.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Test()
        {
            return View();
        }
    }
}