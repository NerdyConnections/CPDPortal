using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CPDPortalSpeaker.Util;
using CPDPortalSpeaker.DAL;
using CPDPortalSpeaker.Models;

namespace CPDPortalSpeaker.Controllers
{
   
    public class OptOutController : Controller
    {
        // GET: optout
        public ActionResult Index(string userid)
        {
            if (!String.IsNullOrEmpty(userid))
            {
                var UserRepo = new UserRepository();
                UserModel um = new UserModel();


                int id = Convert.ToInt32(userid);
                um = UserRepo.GetUserForConfirmEmail(id);

                ViewBag.Email = um.EmailAddress;
                ViewBag.id = userid;


            }
          
            

            TempData["ImageUrl"] = UserHelper.GetCPDLogo(Request);


            return View();
        }

      


        [HttpPost]
        [AllowAnonymous]
        public JsonResult Index(string userid, string Email, string FullName)
        {
            TempData["ImageUrl"] = UserHelper.GetCPDLogo(Request);
            

            var userRepo = new UserRepository();
            int SpeakerOrModeratorId;

            string error = string.Empty;

            if (String.IsNullOrEmpty(Email))
            {
                error = "Please Enter Email";
                return Json(new { Error = error });
            }

            if (userRepo.CheckEmailInUserinfo(Email))
            {
                if(string.IsNullOrEmpty(userid))
                {
                    SpeakerOrModeratorId = userRepo.GetUserIdFromEmail(Email);

                }
                else
                {
                    SpeakerOrModeratorId = Convert.ToInt32(userid);

                }

               
                userRepo.UpdateOptOutStatusAndProgramRequest(SpeakerOrModeratorId);
                return Json(new { redirectTo = Url.Action("OptOutConfirmation") });

            }
            else
            {
                error = "*Please Contact Admin - Email not valid";
                return Json(new { Error = error });

            }

        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult OptOutConfirmation()
        {
            TempData["ImageUrl"] = UserHelper.GetCPDLogo(Request);
            return View();
        }
    }
}