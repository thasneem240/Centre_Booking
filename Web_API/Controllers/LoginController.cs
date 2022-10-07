using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web_API.Controllers
{
     [RoutePrefix("api/login")]
     public class LoginController : ApiController
     {
         [Route("admLogin/{userName}/{password}")]
         [Route("admLogin")]
         [HttpGet]
         public bool admLogin(String userName, String password) 
         {
           
            if (userName.Equals("admin") && password.Equals("adminpass"))
            {
                return true;
                //return Ok();
            }
            else
            {
                return false;
                //return BadRequest();
            }

        }

    }



}
