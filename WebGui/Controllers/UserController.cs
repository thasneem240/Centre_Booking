using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
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


        [HttpPost]
        public IActionResult search([FromBody] SearchData searchString)
        {
            if (String.IsNullOrEmpty(searchString.SearchStr))
            {
                return BadRequest("Empty Field!!, Please input the Search centre name");
            }


            string URL = "http://localhost:2590/";

            RestClient restClient = new RestClient(URL);

            //Build a request with the json in the body
            RestRequest restRequest = new RestRequest("api/search", Method.Post);

            //restRequest.AddJsonBody(JsonConvert.SerializeObject(searchString));

            restRequest.AddJsonBody(searchString);

            RestResponse restResponse = restClient.Execute(restRequest);

            Centre centre = JsonConvert.DeserializeObject<Centre>(restResponse.Content);

            if (centre != null)
            {
                return Ok(restResponse.Content);
            }
            else
            {
                return NotFound("Not Found");
            }
        }



        [HttpPost]
        public IActionResult showNextAvailableDate([FromBody] Centre centreObj)
        {
            if (String.IsNullOrEmpty(centreObj.CentreName))
            {
                return BadRequest("Empty Field!!, Please input the Centre name");
            }

            string URL = "http://localhost:2590/";

            RestClient restClient = new RestClient(URL);

            //Build a request with the json in the body
            RestRequest restRequest = new RestRequest("api/NextAvailable", Method.Post);

            //restRequest.AddJsonBody(JsonConvert.SerializeObject(searchString));

            restRequest.AddJsonBody(centreObj);

            RestResponse restResponse = restClient.Execute(restRequest);

            
            if (restResponse.StatusCode == HttpStatusCode.OK)
            {
                String nextDate = restResponse.Content;

                return Ok(nextDate);
            }
            else
            {
                return NotFound("Centre Not Found");
            }
        }

    }
}
