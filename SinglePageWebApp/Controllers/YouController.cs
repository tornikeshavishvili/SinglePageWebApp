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
using System.Web.Mvc;

namespace SinglePageWebApp.Controllers {

    [BasicAuthentication]
    //[System.Web.Http.Authorize]
    public class YouController : ApiController
    {
      

        public YouController()
        {
        }

        // GET api/You
        [System.Web.Http.HttpPost]
        public JsonResult<GetViewModel> MyFunction([FromBody] JObject myObject)
        {
            
            return Json(new GetViewModel() { Hometown = ((string)myObject.GetValue("myValue")) });
             
        }
    }
}