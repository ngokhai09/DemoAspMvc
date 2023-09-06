using Microsoft.AspNetCore.Mvc;

namespace DemoMVC.Controllers
{
    public class UploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult UploadFile()
        {
            return PartialView();
        }
    }
}
