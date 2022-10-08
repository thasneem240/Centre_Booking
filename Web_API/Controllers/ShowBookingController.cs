using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web_API.Models;

namespace Web_API.Controllers
{
    public class ShowBookingController : ApiController
    {
        private BookingCentreDBEntities2 db = new BookingCentreDBEntities2();
        private BookingCentreDBEntities db2 = new BookingCentreDBEntities();

        // POST api/ShowBooking
        public IHttpActionResult Post([FromBody] Centre centreObj)
        {
            
            string centerName = centreObj.CentreName;

            Centre centre = db2.Centres.Find(centerName);
            if (centre == null)
            {
                return NotFound();
            }


            List<Booking> bookingList = db.Bookings.ToList();

            List<Booking> specificCenterBookingList = new List<Booking>();

            foreach (Booking booking in bookingList) 
            {
                if (booking.CentreName.Equals(centerName)) 
                {
                    specificCenterBookingList.Add(booking);
                }
            }

            return Ok(specificCenterBookingList);


        }
    }
}
