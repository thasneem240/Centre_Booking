using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Xml.Linq;
using WebGui.Models;

namespace WebGui.Controllers
{
    public class AdminController : Controller
    {
        private static String status = "Not Logged In"; // Admin Login Status
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult GetStatus()
        {
           
            return Ok(status);
        }


        [HttpPost]
        public IActionResult Login([FromBody] Account account)
        {

            String userName = account.UserName;
            String password = account.Password;

            if (String.IsNullOrEmpty(userName) || String.IsNullOrEmpty(password))
            {
                // ModelState.AddModelError("Id", "Invalid user name or password.");
                //return BadRequest(ModelState);

                /*var error = new
                {
                    ErMessage = "Invalid user name or password"
                };*/
                return BadRequest("Please input user Name and Password");
            }
            else 
            {
                try
                {

                    string URL = "http://localhost:2590/";
                    RestClient restClient = new RestClient(URL);


                    RestRequest restRequest = new RestRequest("api/login/admLogin/{userName}/{password}");
                    restRequest.AddUrlSegment("userName", userName);
                    restRequest.AddUrlSegment("password", password);

                    RestResponse restResponse = restClient.Get(restRequest);


                    if (restResponse.StatusCode == HttpStatusCode.OK)
                    {
                        status = "Logged In";

                        return Ok(status);
                    }
                    else
                    {
                        return BadRequest("Invalid user name and Password");
                    }

                }
                catch (HttpRequestException e) 
                {
                    return BadRequest("Invalid user name and Password");
                }
                

            }
            
          
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
        public IActionResult addNewCentre([FromBody]CentreName newCentreName) 
        {
            if (status.Equals("Logged In"))
            {

                if (String.IsNullOrEmpty(newCentreName.Name)) 
                {
                    return BadRequest("Empty Field, Please input the Centre Name");
                }

                try
                {
                    string URL = "http://localhost:2590/";
                    RestClient restClient = new RestClient(URL);

                    RestRequest restRequest = new RestRequest("api/Centres", Method.Post);

                    Centre newCentre = new Centre();

                    newCentre.CentreName = newCentreName.Name;
                    newCentre.BookStatus = 0;

                    //restRequest.AddJsonBody(JsonConvert.SerializeObject(bankDetail));
                    restRequest.AddJsonBody(newCentre);

                    RestResponse restResponse = restClient.Execute(restRequest);


                    if (restResponse.StatusCode == HttpStatusCode.Conflict)
                    {
                        return BadRequest("The entered name is All ready there, Choose different name for new Centre");
                    }
                    else 
                    {
                        return Ok();
                    }
                }
                catch (Exception e)
                {
                    return BadRequest();
                }

            }
            else 
            {
                return BadRequest("Access denied!! Please Login");
            }
        }

    }
}
