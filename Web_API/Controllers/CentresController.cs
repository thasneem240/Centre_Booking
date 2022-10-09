using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Web_API.Models;

namespace Web_API.Controllers
{
    public class CentresController : ApiController
    {
        private BookingCentreDBEntities db = new BookingCentreDBEntities();

       /* Allowing all users(including the admin) to retrieve the list of centres */

        // GET: api/Centres
        public IQueryable<Centre> GetCentres()
        {
            return db.Centres;
        }

        // GET: api/Centres/5
        [ResponseType(typeof(Centre))]
        public IHttpActionResult GetCentre(string id)
        {
            Centre centre = db.Centres.Find(id);
            if (centre == null)
            {
                return NotFound();
            }

            return Ok(centre);
        }

        // PUT: api/Centres/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCentre(string id, Centre centre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != centre.CentreName)
            {
                return BadRequest();
            }

            db.Entry(centre).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CentreExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /* Allowing admin to add new centre names to its centre lists. */

        // POST: api/Centres
        [ResponseType(typeof(Centre))]
        public IHttpActionResult PostCentre(Centre centre)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Centres.Add(centre);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CentreExists(centre.CentreName))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = centre.CentreName }, centre);
        }

        // DELETE: api/Centres/5
        [ResponseType(typeof(Centre))]
        public IHttpActionResult DeleteCentre(string id)
        {
            Centre centre = db.Centres.Find(id);
            if (centre == null)
            {
                return NotFound();
            }

            db.Centres.Remove(centre);
            db.SaveChanges();

            return Ok(centre);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CentreExists(string id)
        {
            return db.Centres.Count(e => e.CentreName == id) > 0;
        }
    }
}