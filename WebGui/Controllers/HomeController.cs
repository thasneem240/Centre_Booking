using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using WebGui.Models;

namespace WebGui.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Home"; // we can give anything ViewBag.Abc or anything but it has to matched

            return View();
        }
    }
}
