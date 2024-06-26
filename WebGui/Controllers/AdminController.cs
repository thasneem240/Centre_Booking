﻿using Microsoft.AspNetCore.Mvc;
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


        /* Log in for admin */

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


        /* View for admin to show all centres */

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


        /* View for admin to add a new centre */

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


        /* View for admin to select a centre and show all bookings */

        [HttpPost]
        public IActionResult showAllBookings([FromBody] Centre centre)
        {
            if (status.Equals("Logged In"))
            {

                if (String.IsNullOrEmpty(centre.CentreName))
                {
                    return BadRequest("Empty Field, Please input the Centre Name");
                }

                string URL = "http://localhost:2590/";
                RestClient restClient = new RestClient(URL);

                RestRequest restRequest = new RestRequest("api/ShowBooking", Method.Post);

                restRequest.AddJsonBody(centre);

                RestResponse restResponse = restClient.Execute(restRequest);


                if (restResponse.StatusCode == HttpStatusCode.OK)
                {
                    List<Booking> bookingList = JsonConvert.DeserializeObject<List<Booking>>(restResponse.Content);

                    return Ok(bookingList);
                }
                else 
                {
                    return BadRequest("Centre Not Found");
                }

            }
            else
            {
                return BadRequest("Access denied!! Please Login");
            }
        }


    }


}
