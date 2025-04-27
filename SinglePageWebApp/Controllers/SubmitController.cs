using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace SinglePageWebApp.Controllers
{
    public class SubmitController : Controller
    {
 
        // GET api/<controller>/5
        public ActionResult Feedback()
        {

            var allFormData = Request.Form; // FormCollection

            foreach (var key in allFormData.AllKeys)
            {
                Debug.WriteLine($"{key}: {allFormData[key]}");
            }

            return View("../Home/Feedback");
        }

 

 
    }
}