using Microsoft.AspNetCore.Mvc;

namespace WebGui.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
