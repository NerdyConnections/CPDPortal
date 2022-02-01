using CPDPortalMVC.DAL;
using CPDPortalMVC.Models;
using CPDPortalMVC.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CPDPortalMVC.Controllers
{
    [Authorize]
    public class SpeakerController : Controller
    {
        // GET: Speaker
        public ActionResult Index()
        {
            ViewBag.SpeakerBaseURL = ConfigurationManager.AppSettings["SpeakerBaseURL"];
            if (Session["ProgramID"] != null)
            {

                ViewBag.ProgramRequestStatusCounts = UserHelper.GetProgramRequestStatusCounts(Convert.ToInt32(Session["ProgramID"]));
                ViewBag.ProgramID = Session["ProgramID"];
            }
            else
                return RedirectToAction("Index", "Home");
            return View();
        }

        #region Speakers
        public ActionResult Edit(int user_id)
        {
            SpeakerModel sm;
            SpeakerRepository repo = new SpeakerRepository();

            sm = repo.GetSpeakerByuserid(user_id);

            return View(sm);
        }

        public ActionResult GetPresenterPayments(int userid)
        {
            List< PresenterPayment> liPresenterPayment;
            SpeakerModel sm;
            SpeakerRepository repo = new SpeakerRepository();
          

            sm = repo.GetSpeakerByuserid(userid);
            if (sm.SpeakerHonoraria != null)
            ViewBag.SpeakerHonoraria = "$ " + sm.SpeakerHonoraria;
            if (sm.ModeratorHonoraria != null)
                ViewBag.ModeratorHonoraria = "$ " + sm.ModeratorHonoraria;

            liPresenterPayment = repo.GetPresenterPayments(userid);

            return View(liPresenterPayment);
        }

        public ActionResult GetCOISlides(int UserID)
        {
            ViewBag.SpeakerCOIURL= ConfigurationManager.AppSettings["SpeakerCOIURL"];
            ViewBag.SpeakerBaseURL = ConfigurationManager.AppSettings["SpeakerBaseURL"];
            List<COISlide> liCOISlides;
            SpeakerRepository repo = new SpeakerRepository();
            ProgramRepository programrepo = new ProgramRepository();


            liCOISlides = repo.GetCOISlides(UserID);

            foreach (COISlide slide in liCOISlides)
            {
                slide.ProgramName = programrepo.GetProgramName(slide.ProgramID);
            }

                return View(liCOISlides);
        }


        //public ActionResult Payments(int user_id)
        //{
        //  //  SpeakerModel sm ;
        //  //  SpeakerRepository repo = new SpeakerRepository();

        //  ////  sm = repo.GetSpeakerPaymentsByuserid(user_id);

        //  //  return View(sm);
        //}
        public ActionResult GetAllApprovedSpeakers(int ProgramID)
        {

            List<SpeakerModel> liSpeakerModel;

            SpeakerRepository speakerrepo = new SpeakerRepository();
            liSpeakerModel = speakerrepo.GetAllApprovedSpeakers(ProgramID);

            

            return Json(new { data = liSpeakerModel }, JsonRequestBehavior.AllowGet);


        }
        public ActionResult NewSpeaker()
        {
            //SpeakerModel sm;
            //SpeakerRepository repo = new SpeakerRepository();

            //sm = repo.GetSpeakerByuserid(user_id);

            return View("NewSpeaker");
        }

        [HttpPost]
        public JsonResult NewSpeaker(SpeakerModel model)
        {
            SpeakerModel sm = model;
            SpeakerRepository repo = new SpeakerRepository();
            int index;
            try
            {
                //if (!ModelState.IsValid)
                //{
                //    foreach (var item in ModelState.Values)
                //    {
                //        if (item.Errors.Count > 0)
                //        {
                //            var v = item.Value;
                //            var e = item.Errors;
                //            var key = ModelState.Keys.ElementAt(index);
                //        }
                //        index++;
                //    }

                //    return View("ProgramRequestII", model);
                //}
                if (model.SufficientKnowledge==false)
                {

                    //this is custom validation (because it is a checkbox, not caught by query.validation, so need to catch in the server side
                    var error = "Speaker/Moderator has sufficient knowledge checkbox is not checked";
                    return Json(new { error = error });

                }

                if (repo.SaveNewSpeaker(model))
                {
                    var success = new { msg = "New Speaker saved successfully" };
                    return Json(new { success = success });
                }
                else
                {
                    var error = "Unable to save speaker, duplicate speaker is found";
                    return Json(new { error = error });
                  

                }
            }

            catch (Exception e)
            {
                //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                var error = "Unable to save speaker, database problem, please try again later";
                return Json(new { error = error });

                
            }


        }
        
        #endregion Speakers
    }
}