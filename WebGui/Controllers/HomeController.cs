using Microsoft.AspNetCore.Mvc;

namespace WebGui.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Home";
            return View();
        }
    }
}
