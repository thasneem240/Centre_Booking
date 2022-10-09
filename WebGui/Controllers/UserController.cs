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

        /* View for non-admin users to show all centres */

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

        /* View for non-admin users to search centres names
              (partial string match should produce a result) */

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


        /* View for non-admin to select a centre and show the next available start date */

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



        /* View for non-admin to select a centre and book it 
            from a start date to an end date (if available). */

        [HttpPost]
        public IActionResult addBooking([FromBody] BookingStr bookingStrObj)
        {

            String personName = bookingStrObj.PersonName;
            String centreName = bookingStrObj.CentreName;
            String startDate = bookingStrObj.StartDate;
            String endDate = bookingStrObj.EndDate;

            /* Check Empty */

            if (String.IsNullOrEmpty(personName) || String.IsNullOrEmpty(centreName) 
                || String.IsNullOrEmpty(startDate) || String.IsNullOrEmpty(endDate))
            {
                return BadRequest(" Contains Empty Field!! Please input all the relevant fields");
            }

            /* Validate the Dates */

            if (!IsValidDate(startDate)) 
            {
                return BadRequest(" Invalid Start Date!! Date Format Error");
            }


            if (!IsValidDate(endDate))
            {
                return BadRequest(" Invalid End Date!! Date Format Error");
            }

            String currDateTime = DateTime.Now.ToString("yyyy-MM-dd");

            DateTime stDate = DateTime.Parse(startDate);
            DateTime enDate = DateTime.Parse(endDate);
            DateTime currDate = DateTime.Parse(currDateTime);
            


            /* Check the Start date */

            if (stDate < currDate) 
            {
                return BadRequest(" Error!! start date should be greater and equal to the current date");
            }

            /* Check the Start date and end date */

            if (stDate >= enDate)
            {
                return BadRequest(" Error!! End date Should be Greater than Start date");
            }


            string URL = "http://localhost:2590/";
            RestClient restClient = new RestClient(URL);

            RestRequest restRequest = new RestRequest("api/AddBooking", Method.Post);
            
            /* Create a Booking Object */
            Booking booking = new Booking();

            booking.PersonName = personName;
            booking.CentreName = centreName;
            booking.StartDate = stDate;
            booking.EndDate = enDate;

            restRequest.AddJsonBody(booking);

            RestResponse restResponse = restClient.Execute(restRequest);

            if (restResponse.StatusCode == HttpStatusCode.NotFound) 
            {
                return NotFound("Centre Not Found");
            }

            if (restResponse.StatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(" Error!! Dates are Overlapping. please put the start date from next avilable date");
            }

            if (restResponse.StatusCode == HttpStatusCode.Conflict) 
            {
                return Conflict("Person Name already exist please enter different person name");
            }

           
            return Ok();

        }


        /* To check the Date */
        private static bool IsValidDate(string date) 
        {
            DateTime tempDateTimeObj;

            return DateTime.TryParse(date, out tempDateTimeObj);
        }


    }
}
