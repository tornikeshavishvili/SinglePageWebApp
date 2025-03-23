using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Owin;
using SinglePageWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace SinglePageWebApp.Controllers
{

    [RoutePrefix("api/me")]
    [BasicAuthentication]
    //[Authorize]
    public class MeController : ApiController
    {
        private ApplicationUserManager _userManager;

        public MeController()
        {
        }

        public MeController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET api/Me
        [HttpGet]
        [Route("Get")] // Unique route
        public GetViewModel Get()
        {
            var keysCollection = System.Web.HttpContext.Current.Session.Keys.Count + "";
            var user = UserManager.FindByName(User.Identity.Name);
            return new GetViewModel() { Hometown = user.Hometown };
        }


        [HttpGet]
        [Route("listQueryString")]
        public JsonResult<Dictionary<string, string>> listQueryString()
        {

            // Retrieve all query string parameters as an IEnumerable<KeyValuePair<string, string>>
            var queryParameters = Request.GetQueryNameValuePairs();

            // Optionally convert them to a dictionary for easier use
            var queryDict = queryParameters.ToDictionary(param => param.Key, param => param.Value);
 
            return Json(queryDict);
        }


        [HttpGet]
        [Route("listCookies")]
        public JsonResult<GetViewModel> listCookies()
        {

            //System.Diagnostics.Debug.WriteLine("DDDDDDDDDDDDDDDDDD"); 

            string cookieStr = "";

            var cookies = System.Web.HttpContext.Current.Request.Cookies;
            foreach (string cookieName in cookies.AllKeys)
            {
                var cookie = cookies[cookieName];
                // For example, you can log or display the cookie name and value:
                cookieStr += $"Cookie Name: {cookieName}, Value: {cookie.Value}";
            }
            //var keysCollection = System.Web.HttpContext.Current.Session.Keys.Count + "";
            //var user = UserManager.FindByName(User.Identity.Name);
            return Json(new GetViewModel() { Hometown = cookieStr });
        }


        [HttpGet]
        [Route("listSessionData")]
        public JsonResult<GetViewModel> listSessionData()
        {
            try
            {
                string sessionData = "";
                var session = HttpContext.Current.Session;

                if (session == null)
                {
                    return Json(new GetViewModel() { Hometown = "Session is null" });
                }

                if (session.Count == 0)
                {
                    return Json(new GetViewModel() { Hometown = "Session is empty" });
                }

                foreach (string key in session.Keys)
                {
                    sessionData += $"Key: {key}, Value: {session[key]}; ";
                }

                return Json(new GetViewModel() { Hometown = sessionData });
            }
            catch (Exception ex)
            {
                return Json(new GetViewModel() { Hometown = "Error: " + ex.Message });
            }
        }
    }
}