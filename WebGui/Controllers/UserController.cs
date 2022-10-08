using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using WebGui.Models;

namespace WebGui.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult showAllCentres()
        {
            string URL = "http://localhost:2590/";
            RestClient restClient = new RestClient(URL);
            RestRequest restRequest = new RestRequest("api/Centres", Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);

            List<Centre> centreList = JsonConvert.DeserializeObject<List<Centre>>(restResponse.Content);

            if (centreList != null)
            {
                return Ok(centreList);
            }
            else
            {
                return BadRequest();
            }


        }

    }
}
