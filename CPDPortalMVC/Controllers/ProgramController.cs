using CPDPortalMVC.DAL;
using CPDPortalMVC.Models;
using CPDPortalMVC.Util;
using CPDPortalMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;




using System.Web.Mvc;
using System.Web.Security;

namespace CPDPortalMVC.Controllers
{
    public class ProgramController : Controller
    {



        // GET: Program
        public ActionResult Index(int TherapeuticID)
        {



            ProgramRepository pr = new ProgramRepository();
            ViewBag.TherapeuticArea = pr.GetTherapeuticName(TherapeuticID);
            ViewBag.TherapeuticID = TherapeuticID;
            Session["TherapeuticID"] = TherapeuticID;

            TherapeuticRepository tr = new TherapeuticRepository();
            List<Program> liProgram;


            liProgram = tr.GetPrograms(TherapeuticID);

            //if (UserHelper.IsInRole(Util.Constants.HeadOffice + "," + Util.Constants.RegionalManager))
            //    {
            //        ViewBag.Message = "you are headoffice";

            //    }
            // ViewBag.ImageUrl = UserHelper.GetCPDLogo(Request);
            return View(liProgram);

        }
        // GET: SpeakerResp
        public ActionResult Description()
        {
            if (Session["ProgramID"] != null)
            {
                ViewBag.UserID = UserHelper.GetLoggedInUser().UserID;
                ViewBag.ProgramID = Session["ProgramID"].ToString();

                ViewBag.ProgramRequestStatusCounts = UserHelper.GetProgramRequestStatusCounts(Convert.ToInt32(Session["ProgramID"]));
                return View("Description_" + Session["ProgramID"]);

            }
            else
            {
                return RedirectToAction("Index", "Home");

            }






        }

        public ActionResult GetProgramSessionPayment(int ProgramRequestID)
        {
            ProgramSessionPayment psu = new ProgramSessionPayment();
            ProgramRepository ur = new ProgramRepository();
            psu = ur.GetProgramSessionPayment(ProgramRequestID);
            return View(psu);
        }
        public ActionResult GetAllProgramRequests(int programID)
        {

            List<ProgramRequest> liProgramRequest;

            ProgramRepository ur = new ProgramRepository();

            liProgramRequest = ur.GetAllProgramRequests(programID);

            return Json(new { data = liProgramRequest }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult ProgramRequest()
        {

            UserModel um = UserHelper.GetLoggedInUser();
            if (um != null)
            {
                int ProgramID = Convert.ToInt32(Session["ProgramID"]);
                ProgramRepository ProgramRepo = new ProgramRepository();

                ProgramRequest pr = ProgramRepo.InitialProgramRequestForm(ProgramID);
                ViewBag.id = um.UserID;
                ViewBag.ProgramName = ProgramRepo.GetProgramName(ProgramID);
                pr.UserID = um.UserID;
                return View(pr);
            }
            else
            {
                Session.Clear();
                Session.Abandon();
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }


        }
        public ActionResult WebCastRequest()
        {

            UserModel um = UserHelper.GetLoggedInUser();
            if (um != null)
            {
                int ProgramID = Convert.ToInt32(Session["ProgramID"]);
                ProgramRepository ProgramRepo = new ProgramRepository();

                WebCastViewModel wcvm = ProgramRepo.InitialWebCastRequest(ProgramID);
                ViewBag.id = um.UserID;
                ViewBag.ProgramName = ProgramRepo.GetProgramName(ProgramID);
                wcvm.UserID = um.UserID;
                return View(wcvm);
            }
            else
            {
                Session.Clear();
                Session.Abandon();
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Home");
            }


        }
        public ActionResult Cancel(int ProgramRequestID)
        {
            //UserModel um;
            //UserRepository repo = new UserRepository();

            //um = repo.GetUserByUserID(UserID);
            if (ProgramRequestID != 0)
            {
                ProgramRequestCancellationVM PReqCancel;
                ProgramRepository PRepo = new ProgramRepository();
                PReqCancel = PRepo.GetProgramRequestCancellationbyID(ProgramRequestID);
                if (PReqCancel != null)
                {

                    return View(PReqCancel);
                }
                else
                    return View();
            }

            return View();
        }

        [HttpPost]
        public ActionResult Cancel(ProgramRequestCancellationVM prcvm)
        {
            ProgramRepository repo = new ProgramRepository();
            //the cancellation must 2 business days before confirmed session date
            //the logic is now switched to server side, if the session cannot be cancelled the button will not show up
            //if (!string.IsNullOrEmpty(prcvm.ConfirmedSessionDate) && (DateTime.Now - Convert.ToDateTime(prcvm.ConfirmedSessionDate)).TotalDays < 5)
            //{
            //    ModelState.AddModelError("", "Session Cancellation must occur at least 2 business prior to the session date");
            //    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            //}
            if (UserHelper.GetLoggedInUser() != null)
            {
                UserHelper.EmailFromSaleRepToAdmin_Cancellation(UserHelper.GetLoggedInUser().EmailAddress, prcvm);

                repo.CancelProgramRequest(prcvm);
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult Modify(int ProgramRequestID)
        {
            //UserModel um;
            //UserRepository repo = new UserRepository();

            //um = repo.GetUserByUserID(UserID);
            if (ProgramRequestID != 0)
            {
                ProgramRequestModifyVM PReqModify;
                ProgramRepository PRepo = new ProgramRepository();
                PReqModify = PRepo.GetProgramRequestModifybyID(ProgramRequestID);
                if (PReqModify != null)
                {

                    return View(PReqModify);
                }
                else
                    return View();
            }

            return View();
        }

        [HttpPost]
        public ActionResult Modify(ProgramRequestModifyVM prmvm)
        {
            ProgramRepository repo = new ProgramRepository();
            //the cancellation must 2 business days before confirmed session date
            //the logic is now switched to server side, if the session cannot be cancelled the button will not show up
            //if (!string.IsNullOrEmpty(prcvm.ConfirmedSessionDate) && (DateTime.Now - Convert.ToDateTime(prcvm.ConfirmedSessionDate)).TotalDays < 5)
            //{
            //    ModelState.AddModelError("", "Session Cancellation must occur at least 2 business prior to the session date");
            //    return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            //}
            if (UserHelper.GetLoggedInUser() != null)
            {
                UserHelper.EmailFromSaleRepToAdmin_Modify(UserHelper.GetLoggedInUser().EmailAddress, prmvm);

                repo.ModifyProgramRequest(prmvm);
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult GetProgramRequest(int ProgramRequestID, int? IsAdmin)
        {
            var user = UserHelper.GetLoggedInUser();

            if (user != null)
            {
                ViewBag.id = user.UserID;

                if (ProgramRequestID != 0)
                {
                    ProgramRequest PReq;
                    ProgramRepository PRepo = new ProgramRepository();


                    PReq = PRepo.GetProgramRequestbyQueryString(ProgramRequestID);
                    ViewBag.ProgramName = PRepo.GetProgramName(PReq.ProgramID);

                    if (PReq != null)
                    {
                        if (IsAdmin == 1)
                        {
                            ViewBag.IsAdmin = IsAdmin;
                            int AdminID = user.UserID;

                            PReq.AdminUserID = AdminID;
                            PReq.IsAdmin = 1;

                        }
                        else
                        {

                            PReq.FromQueryStringBySalesRep = true;

                        }
                        return View("ProgramRequest", PReq);
                    }
                    else
                        return View("ProgramRequest");
                }
                else
                    return View("ProgramRequest");
            }
            return RedirectToAction("Login", "Account");
        }

        public ActionResult GetProgramRequestII(int ProgramRequestID, int? IsAdmin)
        {
            var user = UserHelper.GetLoggedInUser();

            if (user != null)
            {
                ViewBag.id = user.UserID;

                if (ProgramRequestID != 0)
                {
                    ProgramRequestIIModel PReq;
                    ProgramRepository PRepo = new ProgramRepository();


                    PReq = PRepo.GetProgramRequestII(ProgramRequestID);
                    ViewBag.ProgramName = PRepo.GetProgramName(PReq.ProgramID);

                    if (PReq != null)
                    {
                        if (IsAdmin == 1)
                        {
                            //ViewBag.IsAdmin = IsAdmin;
                            int AdminID = user.UserID;

                            PReq.AdminUserID = AdminID;
                            PReq.IsAdmin = 1;

                        }
                        else
                        {
                            PReq.IsAdmin = 0;
                            PReq.FromQueryStringBySalesRep = true;

                        }
                        return View("ProgramRequestII", PReq);
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
                else
                    return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Login", "Account");
        }

        private bool IsValidProgramDate(string strProgramDate)
        {
            DateTime today = DateTime.Now;

            if (!String.IsNullOrEmpty(strProgramDate))
            {
                DateTime ProgramDate = DateTime.ParseExact(strProgramDate, "yyyy/MM/dd", null);
                var daycount = (ProgramDate - today).TotalDays;

                if (daycount <= 28)//if the requested program date is less than 4 week it is not valid
                    return false;
                else
                    return true;
            }
            else
                return true;  //             
        }
        [HttpPost]
        //public ActionResult SaveProgramRequest(ProgramRequest pr)
        //{
        //    ProgramRepository PRepo = new ProgramRepository();

        //    //string destinationPath = Server.MapPath("~/App_Data/MultSessionAgenda.pdf");

        //    var CurrentUser = UserHelper.GetLoggedInUser();
        //    if (CurrentUser != null)//let make sure session is still active before saving
        //    {
        //        ProgramRequest val = PRepo.PopulateSpeakerModeratorDropdowns(pr.ProgramID);
        //        string ProgramName = PRepo.GetProgramRequestName(pr.ProgramID);
        //        UserRepository userRepo = new UserRepository();

        //        //can only validate program when the program request is first save, cannot validate date when a program request is updated because by then the program date will not be 4 weeks from today's date for sure
        //        if (!IsValidProgramDate(pr.ProgramDate1))
        //        {

        //            ModelState.AddModelError("ProgramDate1", "* Must be 4 weeks from today's date");
        //        }
        //        if (!IsValidProgramDate(pr.ProgramDate2))
        //        {

        //            ModelState.AddModelError("ProgramDate2", "* Must be 4 weeks from today's date");
        //        }
        //        if (!IsValidProgramDate(pr.ProgramDate3))
        //        {

        //            ModelState.AddModelError("ProgramDate3", "* Must be 4 weeks from today's date");
        //        }

        //        if (!ModelState.IsValid)
        //        {
        //            pr.Speakers = val.Speakers;
        //            pr.Moderators = val.Moderators;
        //            //ViewBag.id = CurrentUser.UserID;
        //            // ViewBag.IsAdmin = pr.IsAdmin;


        //            return View("ProgramRequest", pr);
        //        }
        //        else
        //        {
        //            if (pr.IsAdmin != 1)//this is an end  user
        //            {
        //                //if first time or dates are changed
        //                bool TimechangesBySalesRep = PRepo.IsMealTimesChangesBySalesRep(pr);
        //                bool VenueChangeBySalesRep = PRepo.IsVenueChangesBySalesRep(pr);

        //                if (PRepo.CheckIfSpeakerChanges(pr) || PRepo.CompareProgramDates(pr) || TimechangesBySalesRep || VenueChangeBySalesRep)
        //                {
        //                    //save event request
        //                    PRepo.SaveNewSession(pr);
        //                    PRepo.ResetSpeakerModeratorConfirmDates(pr.ProgramRequestID);


        //                    var SpeakerList = val.Speakers.ToList();
        //                    var speakername = SpeakerList.Find(x => x.Value == pr.ProgramSpeakerID.ToString()).Text;
        //                    var ModeratorList = val.Moderators.ToList();
        //                    var ModeratorName = "";
        //                    if (!string.IsNullOrEmpty(pr.ProgramModeratorID.ToString()))
        //                    {
        //                        ModeratorName = ModeratorList.Find(x => x.Value == pr.ProgramModeratorID.ToString()).Text;
        //                    }

        //                    string SessionCredit = PRepo.SessionCredit(pr);

        //                    UserModel um = new UserModel();
        //                    um = userRepo.GetUserForConfirmEmail(pr.ProgramSpeakerID);

        //                    UserHelper.SendAdminProgramRequest(pr, speakername, ModeratorName, SessionCredit, ProgramName, this);
        //                    UserHelper.SelectInvitationOption(pr, ModeratorName, SessionCredit, this);
        //                    return RedirectToAction("Index", "Home", new { showPopup = true });

        //                }

        //                else
        //                {
        //                    //this part will not run for the first run
        //                    if (PRepo.CheckIfModeratorChanges(pr))
        //                    {
        //                        if (pr.ProgramModeratorID != null)
        //                        {
        //                            var SpeakerList = val.Speakers.ToList();
        //                            var speakername = SpeakerList.Find(x => x.Value == pr.ProgramSpeakerID.ToString()).Text;
        //                            var ModeratorList = val.Moderators.ToList();
        //                            var ModeratorName = "";
        //                            if (!string.IsNullOrEmpty(pr.ProgramModeratorID.ToString()))
        //                            {
        //                                ModeratorName = ModeratorList.Find(x => x.Value == pr.ProgramModeratorID.ToString()).Text;
        //                            }

        //                            string SessionCredit = PRepo.SessionCredit(pr);
        //                            //get date from speaker confirmation date
        //                            var chosendate = PRepo.GetSpeakerConfirmationDate(pr.ProgramRequestID);

        //                            PRepo.UpdateSession(pr);
        //                            PRepo.ResetModeratorstatusAndConfirmSessionDate(pr.ProgramRequestID);


        //                            //send email to moderator only this time

        //                            UserModel um = new UserModel();
        //                            um = userRepo.GetUserForConfirmEmail(pr.ProgramModeratorID ?? 0);
        //                            UserHelper.FromSpeakerToModerator(pr, um, ProgramName, chosendate, SessionCredit);
        //                        }

        //                    }

        //                    PRepo.UpdateSession(pr);
        //                    return RedirectToAction("Index", "Home");
        //                }

        //            }
        //            else //this is an admin
        //            {
        //                bool TimechangesBySalesRep = PRepo.IsMealTimesChangesBySalesRep(pr);
        //                bool VenueChangeBySalesRep = PRepo.IsVenueChangesBySalesRep(pr);

        //                if (PRepo.CheckIfSpeakerChanges(pr) || PRepo.CompareProgramDates(pr) || TimechangesBySalesRep || VenueChangeBySalesRep)
        //                {

        //                    PRepo.SaveNewSession(pr);
        //                    PRepo.ResetSpeakerModeratorConfirmDates(pr.ProgramRequestID);


        //                    var SpeakerList = val.Speakers.ToList();
        //                    var speakername = SpeakerList.Find(x => x.Value == pr.ProgramSpeakerID.ToString()).Text;
        //                    var ModeratorList = val.Moderators.ToList();
        //                    var ModeratorName = "";
        //                    if (!string.IsNullOrEmpty(pr.ProgramModeratorID.ToString()))
        //                    {
        //                        ModeratorName = ModeratorList.Find(x => x.Value == pr.ProgramModeratorID.ToString()).Text;
        //                    }

        //                    string SessionCredit = PRepo.SessionCredit(pr);

        //                    UserModel um = new UserModel();
        //                    UserModel SalesRep = new UserModel();

        //                    um = userRepo.GetUserForConfirmEmail(pr.ProgramSpeakerID);
        //                    //Get the Sales Rep from Userinfo table 
        //                    SalesRep = userRepo.GetUserByUserID(pr.UserID);

        //                    UserHelper.SendAdminProgramRequest(pr, speakername, ModeratorName, SessionCredit, ProgramName, this);

        //                    //ToDo:  send email to salesrep

        //                    UserHelper.EmailSalesRepWhenAdminMakeChanges(pr, SalesRep, ModeratorName, SessionCredit, ProgramName);
        //                    UserHelper.SelectInvitationOption(pr, ModeratorName, SessionCredit, this);
        //                    return RedirectToAction("Index", "Admin");
        //                }


        //                if (PRepo.CheckIfModeratorChanges(pr))
        //                {
        //                    if (pr.ProgramModeratorID != null)
        //                    {
        //                        var SpeakerList = val.Speakers.ToList();
        //                        var speakername = SpeakerList.Find(x => x.Value == pr.ProgramSpeakerID.ToString()).Text;
        //                        var ModeratorList = val.Moderators.ToList();
        //                        var ModeratorName = "";
        //                        if (!string.IsNullOrEmpty(pr.ProgramModeratorID.ToString()))
        //                        {
        //                            ModeratorName = ModeratorList.Find(x => x.Value == pr.ProgramModeratorID.ToString()).Text;
        //                        }

        //                        string SessionCredit = PRepo.SessionCredit(pr);
        //                        //get date from speaker confirmation date
        //                        var chosendate = PRepo.GetSpeakerConfirmationDate(pr.ProgramRequestID);

        //                        PRepo.UpdateSession(pr);
        //                        PRepo.ResetModeratorstatusAndConfirmSessionDate(pr.ProgramRequestID);
        //                        //send email to moderator only this time

        //                        UserModel um = new UserModel();
        //                        um = userRepo.GetUserForConfirmEmail(pr.ProgramModeratorID ?? 0);
        //                        UserHelper.FromSpeakerToModerator(pr, um, ProgramName, chosendate, SessionCredit);
        //                        return RedirectToAction("Index", "Admin");
        //                    }

        //                }

        //                if ((pr.AdminVenueConfirmed).Equals("N"))
        //                {

        //                    //speaker has alrady chosen a program date by click in email 
        //                    var chosendate = PRepo.GetSpeakerConfirmationDate(pr.ProgramRequestID);
        //                    //send email to SalesRep
        //                    //get the sales rep data from userinfo data. 
        //                    UserModel um = new UserModel();
        //                    um = userRepo.GetSalesRepForConfirmEmail(pr.UserID);
        //                    //email to sales rep venue is not available
        //                    UserHelper.AdminToSalesRep(pr.LocationName, um, ProgramName, chosendate);
        //                    PRepo.UpdateSession(pr);
        //                    //make the program request "change required" so that sales rep can pick another venue
        //                    PRepo.UpdateProgramRequestWhenVenueChangedByAdmin(pr.ProgramRequestID);
        //                    return RedirectToAction("Index", "Admin");

        //                }

        //                else
        //                {
        //                    PRepo.UpdateSession(pr);
        //                    return RedirectToAction("Index", "Admin");


        //                }

        //            }

        //        }


        //    }
        //    else
        //    {
        //        Session.Clear();
        //        Session.Abandon();
        //        FormsAuthentication.SignOut();
        //        return RedirectToAction("Home");

        //    }

        //}

     
        //public ActionResult SaveWebCastRequest(WebCastViewModel wcvm)
        //{
        //    ProgramRepository PRepo = new ProgramRepository();

        //    //string destinationPath = Server.MapPath("~/App_Data/MultSessionAgenda.pdf");

        //    var CurrentUser = UserHelper.GetLoggedInUser();
        //    if (CurrentUser != null)//let make sure session is still active before saving
        //    {
        //        WebCastViewModel val = PRepo.PopulateSpeakerDropdown(wcvm.ProgramID);
        //        string ProgramName = PRepo.GetProgramRequestName(wcvm.ProgramID);
        //        UserRepository userRepo = new UserRepository();


        //        if (!ModelState.IsValid)
        //        {
        //            wcvm.Speakers = val.Speakers;

        //            //ViewBag.id = CurrentUser.UserID;
        //            // ViewBag.IsAdmin = pr.IsAdmin;


        //            return View("WebCastRequest", wcvm);
        //        }
        //        else
        //        {
        //            if (wcvm.IsAdmin != 1)//this is an end  user
        //            {
        //                //if first time or dates are changed
        //                bool TimechangesBySalesRep = PRepo.IsMealTimesChangesBySalesRep(pr);
        //                bool VenueChangeBySalesRep = PRepo.IsVenueChangesBySalesRep(pr);

        //                if (PRepo.CheckIfSpeakerChanges(pr) || PRepo.CompareProgramDates(pr) || TimechangesBySalesRep || VenueChangeBySalesRep)
        //                {

        //                    PRepo.SaveNewSession(pr);
        //                    PRepo.ResetSpeakerModeratorConfirmDates(pr.ProgramRequestID);


        //                    var SpeakerList = val.Speakers.ToList();
        //                    var speakername = SpeakerList.Find(x => x.Value == pr.ProgramSpeakerID.ToString()).Text;
        //                    var ModeratorList = val.Moderators.ToList();
        //                    var ModeratorName = "";
        //                    if (!string.IsNullOrEmpty(pr.ProgramModeratorID.ToString()))
        //                    {
        //                        ModeratorName = ModeratorList.Find(x => x.Value == pr.ProgramModeratorID.ToString()).Text;
        //                    }

        //                    string SessionCredit = PRepo.SessionCredit(pr);

        //                    UserModel um = new UserModel();
        //                    um = userRepo.GetUserForConfirmEmail(pr.ProgramSpeakerID);

        //                    UserHelper.SendAdminProgramRequest(pr, speakername, ModeratorName, SessionCredit, ProgramName);
        //                    UserHelper.SelectInvitationOption(pr, ModeratorName, SessionCredit);
        //                    return RedirectToAction("Index", "Home", new { showPopup = true });

        //                }

        //                else
        //                {
        //                    //this part will not run for the first run
        //                    if (PRepo.CheckIfModeratorChanges(pr))
        //                    {
        //                        if (pr.ProgramModeratorID != null)
        //                        {
        //                            var SpeakerList = val.Speakers.ToList();
        //                            var speakername = SpeakerList.Find(x => x.Value == pr.ProgramSpeakerID.ToString()).Text;
        //                            var ModeratorList = val.Moderators.ToList();
        //                            var ModeratorName = "";
        //                            if (!string.IsNullOrEmpty(pr.ProgramModeratorID.ToString()))
        //                            {
        //                                ModeratorName = ModeratorList.Find(x => x.Value == pr.ProgramModeratorID.ToString()).Text;
        //                            }

        //                            string SessionCredit = PRepo.SessionCredit(pr);
        //                            //get date from speaker confirmation date
        //                            var chosendate = PRepo.GetSpeakerConfirmationDate(pr.ProgramRequestID);

        //                            PRepo.UpdateSession(pr);
        //                            PRepo.ResetModeratorstatusAndConfirmSessionDate(pr.ProgramRequestID);


        //                            //send email to moderator only this time

        //                            UserModel um = new UserModel();
        //                            um = userRepo.GetUserForConfirmEmail(pr.ProgramModeratorID ?? 0);
        //                            UserHelper.FromSpeakerToModerator(pr, um, ProgramName, chosendate, SessionCredit);
        //                        }

        //                    }

        //                    PRepo.UpdateSession(pr);
        //                    return RedirectToAction("Index", "Home");
        //                }

        //            }
        //            else //this is an admin
        //            {
        //                bool TimechangesBySalesRep = PRepo.IsMealTimesChangesBySalesRep(pr);
        //                bool VenueChangeBySalesRep = PRepo.IsVenueChangesBySalesRep(pr);

        //                if (PRepo.CheckIfSpeakerChanges(pr) || PRepo.CompareProgramDates(pr) || TimechangesBySalesRep || VenueChangeBySalesRep)
        //                {

        //                    PRepo.SaveNewSession(pr);
        //                    PRepo.ResetSpeakerModeratorConfirmDates(pr.ProgramRequestID);


        //                    var SpeakerList = val.Speakers.ToList();
        //                    var speakername = SpeakerList.Find(x => x.Value == pr.ProgramSpeakerID.ToString()).Text;
        //                    var ModeratorList = val.Moderators.ToList();
        //                    var ModeratorName = "";
        //                    if (!string.IsNullOrEmpty(pr.ProgramModeratorID.ToString()))
        //                    {
        //                        ModeratorName = ModeratorList.Find(x => x.Value == pr.ProgramModeratorID.ToString()).Text;
        //                    }

        //                    string SessionCredit = PRepo.SessionCredit(pr);

        //                    UserModel um = new UserModel();
        //                    UserModel SalesRep = new UserModel();

        //                    um = userRepo.GetUserForConfirmEmail(pr.ProgramSpeakerID);
        //                    //Get the Sales Rep from Userinfo table 
        //                    SalesRep = userRepo.GetUserByUserID(pr.UserID);

        //                    UserHelper.SendAdminProgramRequest(pr, speakername, ModeratorName, SessionCredit, ProgramName);

        //                    //ToDo:  send email to salesrep

        //                    UserHelper.EmailSalesRepWhenAdminMakeChanges(pr, SalesRep, ModeratorName, SessionCredit, ProgramName);
        //                    UserHelper.SelectInvitationOption(pr, ModeratorName, SessionCredit);
        //                    return RedirectToAction("Index", "Admin");
        //                }


        //                if (PRepo.CheckIfModeratorChanges(pr))
        //                {
        //                    if (pr.ProgramModeratorID != null)
        //                    {
        //                        var SpeakerList = val.Speakers.ToList();
        //                        var speakername = SpeakerList.Find(x => x.Value == pr.ProgramSpeakerID.ToString()).Text;
        //                        var ModeratorList = val.Moderators.ToList();
        //                        var ModeratorName = "";
        //                        if (!string.IsNullOrEmpty(pr.ProgramModeratorID.ToString()))
        //                        {
        //                            ModeratorName = ModeratorList.Find(x => x.Value == pr.ProgramModeratorID.ToString()).Text;
        //                        }

        //                        string SessionCredit = PRepo.SessionCredit(pr);
        //                        //get date from speaker confirmation date
        //                        var chosendate = PRepo.GetSpeakerConfirmationDate(pr.ProgramRequestID);

        //                        PRepo.UpdateSession(pr);
        //                        PRepo.ResetModeratorstatusAndConfirmSessionDate(pr.ProgramRequestID);
        //                        //send email to moderator only this time

        //                        UserModel um = new UserModel();
        //                        um = userRepo.GetUserForConfirmEmail(pr.ProgramModeratorID ?? 0);
        //                        UserHelper.FromSpeakerToModerator(pr, um, ProgramName, chosendate, SessionCredit);
        //                        return RedirectToAction("Index", "Admin");
        //                    }

        //                }

        //                if ((pr.AdminVenueConfirmed).Equals("N"))
        //                {

        //                    //speaker has alrady chosen a program date by click in email 
        //                    var chosendate = PRepo.GetSpeakerConfirmationDate(pr.ProgramRequestID);
        //                    //send email to SalesRep
        //                    //get the sales rep data from userinfo data. 
        //                    UserModel um = new UserModel();
        //                    um = userRepo.GetSalesRepForConfirmEmail(pr.UserID);
        //                    //email to sales rep venue is not available
        //                    UserHelper.AdminToSalesRep(pr.LocationName, um, ProgramName, chosendate);
        //                    PRepo.UpdateSession(pr);
        //                    //make the program request "change required" so that sales rep can pick another venue
        //                    PRepo.UpdateProgramRequestWhenVenueChangedByAdmin(pr.ProgramRequestID);
        //                    return RedirectToAction("Index", "Admin");

        //                }

        //                else
        //                {
        //                    PRepo.UpdateSession(pr);
        //                    return RedirectToAction("Index", "Admin");


        //                }

        //            }

        //        }


        //    }
        //    else
        //    {
        //        Session.Clear();
        //        Session.Abandon();
        //        FormsAuthentication.SignOut();
        //        return RedirectToAction("Home");

        //    }

        //}




        public FileResult OpenFile(string fileName)

        {
            try

            {
                return File(new FileStream(Server.MapPath("~/App_Data/" + fileName), FileMode.Open), "application/octetstream", fileName);

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]

        public ActionResult RemoveUploadFile(string fileName, int ProgramRequestID)

        {

            int retProgramRequestID = 0;

            // ((List<FileUploadModel>)Session["fileUploader"]).RemoveAll(x => x.FileName == fileName);

            //  sessionFileCount = ((List<FileUploadModel>)Session["fileUploader"]).Count;

            if (fileName != null || fileName != string.Empty)

            {

                FileInfo file = new FileInfo(Server.MapPath("~/App_Data/" + fileName));

                if (file.Exists)

                {

                    ProgramRepository PRepo = new ProgramRepository();
                    PRepo.UpdateUploadFileStatus(ProgramRequestID, false);
                    retProgramRequestID = ProgramRequestID;
                    file.Delete();

                }

            }
            return Json(retProgramRequestID, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UploadFile(string ProgramRequestID)
        {
            string returnPath = "";
            string UploadedFileName = "";
            try
            {
                foreach (string file in Request.Files)
                {
                    var fileContent = Request.Files[file];
                    if (fileContent != null && fileContent.ContentLength > 0)
                    {

                        //check if file is supported
                        var supportedTypes = new[] { "doc", "docx", "pdf" };
                        var extension = Path.GetExtension(file).Substring(1);
                        if (!supportedTypes.Contains(extension))
                        {
                            var error = "File Extension Is InValid";
                            return Json(new { error = error });
                        }



                        //check if the folder exists, if not then make one. 
                        string FolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserFileUploadPath"] + ProgramRequestID + "/Agenda/");


                        if (!Directory.Exists(FolderPath))
                        {
                            Directory.CreateDirectory(FolderPath);
                        }
                        // get a stream
                        var stream = fileContent.InputStream;
                        // and optionally write the file to disk
                        //var fileName = Path.GetFileName(file);
                        string fileName = "Agenda." + extension;

                        var path = Path.Combine(Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UserFileUploadPath"] + ProgramRequestID + "/Agenda/"), fileName);


                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        returnPath = FolderPath + "\\" + fileName;
                        UploadedFileName = fileName;
                    }
                }
            }
            catch (Exception e)
            {
                //Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Upload failed");
            }

            var success = new { msg = "File uploaded successfully", returnFileName = returnPath, uploadedFilename = UploadedFileName };
            return Json(new { success = success });
        }

        public ActionResult ProgramRequestII()
        {
            if (Session["ProgramID"] != null)
            {
                UserModel um = UserHelper.GetLoggedInUser();
                if (um != null)
                {
                    int programID = Convert.ToInt32(Session["ProgramID"]);
                    ProgramRepository ProgramRepo = new ProgramRepository();
                    ProgramRequestIIModel model = ProgramRepo.InitialEventRequestForm(programID);

                    ViewBag.ProgramName = ProgramRepo.GetProgramRequestName(programID);

                    return View("ProgramRequestII", model);
                }
                else
                {

                    return RedirectToAction("Login", "Account");
                }

            }
            else
            {
                return RedirectToAction("Index", "Home");

            }

        }

        [HttpPost]
        public ActionResult SaveProgramRequestII(ProgramRequestIIModel erm)
        {


            var CurrentUser = UserHelper.GetLoggedInUser();
            if (CurrentUser == null)
            {
                return RedirectToAction("Index", "Home");
            }


            //if (!IsValidProgramDate(erm.ProgramDate1))
            //{

              //  ModelState.AddModelError("ProgramDate1", "* Must be 4 weeks from today's date");
            //}

            ProgramRepository PRepo = new ProgramRepository();
            string programName = PRepo.GetProgramRequestName(erm.ProgramID);
            ViewBag.ProgramName = programName;

            int index = 0;
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState.Values)
                {
                    if (item.Errors.Count > 0)
                    {
                        var v = item.Value;
                        var e = item.Errors;
                        var key = ModelState.Keys.ElementAt(index);
                    }
                    index++;
                }

                return View("ProgramRequestII", erm);
            }
            /*
                        if (!erm.SessionCredit14 && !erm.SessionCredit15)
                        {
                            ViewBag.ShowSelectCreditMsg = true;
                            return View("ProgramRequestII", erm);
                        }*/

            
            UserRepository userRepo = new UserRepository();


            if (erm.IsAdmin != 1)//this is an end  user
            {
                //bool TimechangesBySalesRep = PRepo.IsMealTimesChangesBySalesRep(erm);
                //bool VenueChangeBySalesRep = PRepo.IsVenueChangesBySalesRep(erm);
                //if either speaker changed or program date/time/venue changed
                //if (PRepo.CheckIfSpeakerChanges(erm) || PRepo.CompareProgramDates(erm) || TimechangesBySalesRep || VenueChangeBySalesRep)
               // {
               //if there is a speaker change send email to speaker again otherwise don't do anything.
               if (PRepo.CheckIfSpeakerChanges(erm))
                { 
                    //save event request
                    ViewBag.IsSuccessful = PRepo.SaveNewSession(erm);

                    //since something important has changed, hence need to resend invitation emails to speaker/moderator
                    //PRepo.ResetSpeakerModeratorConfirmDates(erm.ProgramRequestID);


                    var SpeakerList = erm.Speakers;
                    var speakername = SpeakerList.Find(x => x.Value == erm.ProgramSpeakerID.ToString()).Text;
                    var ModeratorList = erm.Moderators;
                    var moderatorName = "";
                    if (!string.IsNullOrEmpty(erm.ProgramModeratorID.ToString()))
                    {
                        moderatorName = ModeratorList.Find(x => x.Value == erm.ProgramModeratorID.ToString()).Text;
                    }
                    //a string containing all session credits
                    string SessionCredit = PRepo.GetSessionCredit(erm);
                    string SessionCreditfr = PRepo.GetSessionCreditfr(erm);
                    UserModel um = new UserModel();
                    um = userRepo.GetUserForConfirmEmail(erm.ProgramSpeakerID);

                    Task.Factory.StartNew(() => {
                        UserHelper.SendAdminProgramRequest(erm, speakername, moderatorName, SessionCredit, programName, this);
                        if (erm.ProgramID == 7 )
                        {
                            UserHelper.SendEmailToSpeaker(erm, moderatorName, SessionCredit, this);
                        }
                        else
                        {//after program request is submitted , send email to speaker
                            UserHelper.SelectInvitationOption(erm, moderatorName, SessionCredit, SessionCreditfr,this);
                        }
                    });


                    ViewBag.IsHttpPost = true;

                    return View("ProgramRequestII", erm);
                }

                else  //no  changes in time/venue speakers
                {
                    //if no changes in time/venue/speaker but moderator changed
                    if (PRepo.CheckIfModeratorChanges(erm))
                    {
                        //if there is a moderator
                        if (erm.ProgramModeratorID != null)
                        {
                            var SpeakerList = erm.Speakers;
                            var speakername = SpeakerList.Find(x => x.Value == erm.ProgramSpeakerID.ToString()).Text;
                            var ModeratorList = erm.Moderators;
                            var ModeratorName = "";
                            if (!string.IsNullOrEmpty(erm.ProgramModeratorID.ToString()))
                            {
                                ModeratorName = ModeratorList.Find(x => x.Value == erm.ProgramModeratorID.ToString()).Text;
                            }

                            string SessionCredit = PRepo.SessionCredit(erm);
                            //get date from speaker confirmation date
                            var chosendate = PRepo.GetSpeakerConfirmationDate(erm.ProgramRequestID);

                            PRepo.UpdateSession(erm);
                            PRepo.ResetModeratorstatusAndConfirmSessionDate(erm.ProgramRequestID);


                            //send email to moderator only this time  because moderator has been updated

                            UserModel um = new UserModel();
                            um = userRepo.GetUserForConfirmEmail(erm.ProgramModeratorID ?? 0);
                            UserHelper.FromSpeakerToModerator(erm, um, programName, chosendate, SessionCredit);
                        }
                    }
                }

                PRepo.UpdateSession(erm);
                return RedirectToAction("Index", "Home");
            }


            else //this is an admin updating event request.
            {


                if (erm.AdminVenueConfirmed != null && (erm.AdminVenueConfirmed).Equals("N"))
                {
                    //if admin cannot reserve the venue need to inform sale rep

                    //speaker has alrady chosen a program date by click in email 
                    var chosendate = PRepo.GetSpeakerConfirmationDate(erm.ProgramRequestID);
                    //send email to SalesRep
                    //get the sales rep data from userinfo data. 
                    UserModel um = new UserModel();
                    um = userRepo.GetSalesRepForConfirmEmail(erm.UserID);
                    //email to sales rep venue is not available
                    UserHelper.AdminToSalesRep(erm.LocationName, um, programName, chosendate);
                    PRepo.UpdateSession(erm);
                    //make the program request "change required" so that sales rep can pick another venue
                    PRepo.UpdateProgramRequestWhenVenueChangedByAdmin(erm.ProgramRequestID);
                    return RedirectToAction("Index", "Admin");

                }

                else
                {
                    PRepo.UpdateSession(erm);
                    return RedirectToAction("Index", "Admin");


                }

            }

        }
    }
}