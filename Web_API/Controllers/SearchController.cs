using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web_API.Models;

namespace Web_API.Controllers
{
    public class SearchController : ApiController
    {
        private BookingCentreDBEntities db = new BookingCentreDBEntities();

        // POST api/search
        public Centre Post([FromBody] SearchData searchDataObj)
        {
            string searchText = searchDataObj.SearchStr;

            // Search the database and get the Centre Object
            Centre centre = db.Centres.FirstOrDefault(x => x.CentreName.ToUpper().Contains(searchText.ToUpper()));

            return centre;


        }

    }
}
