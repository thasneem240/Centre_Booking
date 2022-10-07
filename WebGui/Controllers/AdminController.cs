using Microsoft.AspNetCore.Mvc;

namespace WebGui.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
