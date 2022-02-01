using CPDPortalSpeaker.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CPDPortalSpeaker.Controllers
{
    public class CultureController : Controller
    {
       
            [HttpGet]
            [AllowAnonymousAttribute]
            public ActionResult ToggleCulture(string OriginalUrl)
            {
                if (HttpContext.Session[Constants.CULTURE] == null)
                    HttpContext.Session[Constants.CULTURE] = Constants.ENGLISH;

                string currCulture = HttpContext.Session[Constants.CULTURE].ToString();

                if (currCulture == Constants.ENGLISH)
                    currCulture = Constants.FRENCH;
                else
                    currCulture = Constants.ENGLISH;

                HttpContext.Session[Constants.CULTURE] = currCulture;
                //return RedirectToAction("Index", "Home");

                 return Redirect(OriginalUrl);
            }
        
    }
}