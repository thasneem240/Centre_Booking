using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web_API.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Web_API.Controllers
{
    /* providing users with the next available start date for a given centre */

    public class NextAvailableController : ApiController
    {

        private BookingCentreDBEntities2 db = new BookingCentreDBEntities2();
        private BookingCentreDBEntities db2 = new BookingCentreDBEntities();

        // POST api/NextAvailable
        public IHttpActionResult Post([FromBody]Centre centreObj)
        {
            String nextDate = null;

            String centreName = centreObj.CentreName;

            Centre centre = db2.Centres.Find(centreName);
            if (centre == null)
            {
                return NotFound();
            }


            nextDate = getNextDate(centreName);


            return Ok(nextDate);

        }



        private string getNextDate(String centreName) 
        {
            string nextDate = null;

            IQueryable<Booking> bookQuery = db.Bookings;

            List<Booking> bookList = bookQuery.ToList();

            List<Booking> bookListOfCenter = new List<Booking>(); // Get the List of Booking this specefic centre

            foreach (Booking book in bookList)
            {
                if (book.CentreName.Equals(centreName))
                {
                    bookListOfCenter.Add(book);
                }
            }



            if (bookListOfCenter.Count == 0)
            {
                nextDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                DateTime dateTime = bookListOfCenter.ElementAt(0).EndDate;

                foreach (Booking book in bookListOfCenter)
                {
                    if (book.EndDate > dateTime)
                    {
                        dateTime = book.EndDate;
                    }
                }

                DateTime nextDateTime = dateTime.AddDays(1);

                nextDate = nextDateTime.ToString("yyyy-MM-dd");

            }

            return nextDate;
        }

    }
}
