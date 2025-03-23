using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;
using Owin;
using SinglePageWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results; 

namespace SinglePageWebApp.Controllers {

    [RoutePrefix("api/you")]
    [BasicAuthentication]
    //[System.Web.Http.Authorize]
    public class YouController : ApiController
    {
      

        public YouController()
        {
        }

        // GET api/Me
        [HttpPost]
        [Route("MyFunction")] // Unique route
        public JsonResult<GetViewModel> MyFunction([FromBody] JObject myObject)
        {


            var keysCollection = System.Web.HttpContext.Current.Session.Keys.Count+"";

            return Json(new GetViewModel() { Hometown = (keysCollection) });
             
        }
    }
}