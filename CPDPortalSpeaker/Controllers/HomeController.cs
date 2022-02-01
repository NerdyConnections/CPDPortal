using CPDPortalSpeaker.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CPDPortalSpeaker.Controllers
{
    
    public class HomeController : BaseController
    {
       
        public ActionResult Index()
        {

            if (UserHelper.GetLoggedInUser() != null)

            {
               // UserHelper.ReloadUser();
                int UserID = UserHelper.GetLoggedInUser().UserID;
                ViewBag.UserID = UserID;
                ViewBag.RegistrationStatus = UserHelper.GetRegistrationStatus(UserID);
                ViewBag.FirstName = UserHelper.GetLoggedInUser().FirstName;
                ViewBag.LastName = UserHelper.GetLoggedInUser().LastName;


                return View();

            }
            else
                return RedirectToAction("Login", "Account");




        }

        public ActionResult ScientificPlanningCommittee(int ProgramID)
        {
            if (UserHelper.GetLoggedInUser() != null)

            {
                // UserHelper.ReloadUser();
                int UserID = UserHelper.GetLoggedInUser().UserID;
                ViewBag.UserID = UserID;
                ViewBag.RegistrationStatus = UserHelper.GetRegistrationStatus(UserID);
                ViewBag.FirstName = UserHelper.GetLoggedInUser().FirstName;
                ViewBag.LastName = UserHelper.GetLoggedInUser().LastName;


                return View("ScientificCommittee_" + ProgramID);

            }
            else
                return RedirectToAction("Login", "Account");

           
            
        }

        public ActionResult SpeakerTraining(int ProgramID)
        {
            if (UserHelper.GetLoggedInUser() != null)

            {
                // UserHelper.ReloadUser();
                int UserID = UserHelper.GetLoggedInUser().UserID;
                ViewBag.UserID = UserID;
                ViewBag.RegistrationStatus = UserHelper.GetRegistrationStatus(UserID);
                ViewBag.FirstName = UserHelper.GetLoggedInUser().FirstName;
                ViewBag.LastName = UserHelper.GetLoggedInUser().LastName;


                return View("SpeakerTraining_" + ProgramID);

            }
            else
                return RedirectToAction("Login", "Account");



        }
    }
}