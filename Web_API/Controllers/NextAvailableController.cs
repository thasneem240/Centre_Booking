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

namespace Web_API.Controllers
{
    public class NextAvailableController : ApiController
    {

        private BookingCentreDBEntities2 db = new BookingCentreDBEntities2();

        // POST api/NextAvailable
        public String Post([FromBody]Centre centre)
        {
            String nextDate = null;

            String centreName = centre.CentreName;


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
