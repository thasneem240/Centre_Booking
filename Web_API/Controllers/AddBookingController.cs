using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web_API.Models;

namespace Web_API.Controllers
{
    public class AddBookingController : ApiController
    {

        private BookingCentreDBEntities2 db = new BookingCentreDBEntities2();
        private BookingCentreDBEntities db2 = new BookingCentreDBEntities();



        // POST api/AddBooking
        public IHttpActionResult Post([FromBody] Booking bookingObj)
        {
            String nextDate = null;

            String centreName = bookingObj.CentreName;

            Centre centre = db2.Centres.Find(centreName);
            if (centre == null)
            {
                return NotFound();
            }


            nextDate = getNextDate(centreName);

            /* Get the Next Availble dateTime Object*/
            DateTime nextDateTime = DateTime.Parse(nextDate);


            /* Check the Over lapping time */
            if (nextDateTime > bookingObj.StartDate) 
            {
                return BadRequest();
            }



            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Bookings.Add(bookingObj);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (BookingExists(bookingObj.PersonName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = bookingObj.PersonName }, bookingObj);


        }

        private bool BookingExists(string id)
        {
            return db.Bookings.Count(e => e.PersonName == id) > 0;
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
