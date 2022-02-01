using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CPDPortalMVC.DAL;
using CPDPortalMVC.Models;
using CPDPortalMVC.ViewModels;
using CPDPortalMVC.Util;
using System.Configuration;
using System.IO;

using System.Web.Security;
using System.Web.UI;
using OfficeOpenXml;
using System.Drawing;
using System.Threading.Tasks;

namespace CPDPortalMVC.Controllers
{
    public class AdminController : Controller
    {

        public FileResult OpenFile(string ProgramRequestID, string FileType, string FileExt)

        {
            string FileName = String.Empty;
            try

            {
                if (FileType == "Evaluation")
                    FileName = "Evaluation/EvaluationForm." + FileExt;
                else if (FileType == "SignIn")
                    FileName = "SignIn/SignIn." + FileExt;
                else if (FileType == "UserOther")
                    FileName = "UserOther/UserOther." + FileExt;
                else if ((FileType == "SpeakerAgreement"))
                    FileName = "SpeakerAgreement/SpeakerAgreement." + FileExt;

                //File Type: Evaluation,Other,SignIn
                string FileToOpen = Server.MapPath(ConfigurationManager.AppSettings["UserFileUploadPath"] + ProgramRequestID + "/" + FileName);

                if (System.IO.File.Exists(FileToOpen))
                {
                    return File(new FileStream(FileToOpen, FileMode.Open), "application/octetstream", FileToOpen);
                }
                return null;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {

            ProgramRepository repo = new ProgramRepository();
            ViewBag.ProgramList = repo.GetAllProgram();//populate bootstrap modal with all available programs
            return View();
        }
        public ActionResult ManageModules(int ProgramRequestID)
        {

            ProgramRepository repo = new ProgramRepository();
            var model = repo.GetProgramModulesByProgramRequestID(ProgramRequestID);
           
            return View("ManageModules", model);

        }
        [HttpPost]
        public ActionResult ManageModules(EventModule em)
        {
            try
            {
                ProgramRepository repo = new ProgramRepository();
                if (ValidateEventModule(em))
                {
                    repo.UpdateEventModule(em);

                    return Json(new { success = "true" });
                }
                else
                {
                    return Json(new { success = "false" , msg = "Number of Modules selected are greater than the Session Credit (" + em.SessionCredit + ") allowed in the Event Request"});
                }
            }
            catch(Exception e)
            {

                return Json(new { success = "false" });
            }
           // var success = new { success="true", msg = "Program Modules saved sucssessfully" };
            


        }
        public bool ValidateEventModule(EventModule em)
        {
            int ModuleCount = 0;
            if (em.ProgramModule1)
                ModuleCount = ModuleCount + 1;
            if (em.ProgramModule2)
                ModuleCount = ModuleCount + 1;
            if (em.ProgramModule3)
                ModuleCount = ModuleCount+1;
            if (em.ProgramModule4)
                ModuleCount = ModuleCount+1;
            if (em.ProgramModule5)
                ModuleCount = ModuleCount+1;
            if (em.ProgramModule6)
                ModuleCount = ModuleCount++;
            if (em.ProgramModule7)
                ModuleCount = ModuleCount+1;
            if (em.ProgramModule8)
                ModuleCount = ModuleCount+1;
            if (em.ProgramModule9)
                ModuleCount = ModuleCount+1;
            if (em.ProgramModule10)
                ModuleCount = ModuleCount+1;
            if (em.SessionCreditID == 16 && ModuleCount > 3)
                return false;
            else if (em.SessionCreditID == 17 && ModuleCount > 5)
                return false;
            else if (em.SessionCreditID == 18 && ModuleCount > 7)
                return false;
            else if (em.SessionCreditID == 19 && ModuleCount > 10)
                return false;
            else
                return true;


        }
        public ActionResult Edit(int id)
        {
            ProgramRepository repo = new ProgramRepository();
            var model = repo.GetEditProgramRequestByID(id);
            return View("Edit", model);
        }
        //admin to make changes to post session values
        public ActionResult PostSession(int ProgramRequestID)
        {


            PostSessionRepository psr = new PostSessionRepository();
            PostSessionViewModel psvm = new PostSessionViewModel();
            if (ProgramRequestID != 0)
            {
                ProgramRepository pr = new ProgramRepository();
                SessionUpload su = new SessionUpload();
                ViewBag.ProgramRequestID = ProgramRequestID;
                ViewBag.UserID = UserHelper.GetLoggedInUser().UserID;

                su = pr.GetSessionUpload(ProgramRequestID);
                if (su != null)
                    ViewBag.SessionUpload = su;
                else
                {

                    ViewBag.SessionUpload = new SessionUpload()
                    {
                        EvaluationUploaded = false,
                        SignInUploaded=false,
                        UserOtherUploaded=false,
                        SpeakerAgreementUploaded = false
                    };
                }
                //get fileupload information

                //ProgramRequestCancellationVM PReqCancel;
                //ProgramRepository PRepo = new ProgramRepository();
                //PReqCancel = PRepo.GetProgramRequestCancellationbyID(ProgramRequestID);
                //if (PReqCancel != null)
                //{

                //    return View(PReqCancel);
                //}
                //else

            }


            psvm = psr.GetPostSessionByProgramRequestID(ProgramRequestID);
            return View(psvm);
        }
        [HttpPost]
        public JsonResult PostSession(PostSessionViewModel psvm)
        {
            AdminRepository repo = new AdminRepository();
            repo.SavePostSession(psvm);
            var success = new { msg = "PostSession Data saved successfully" };
            return Json(new { success = success });


        }


        //handle the program request status update from the Admin Tool
        [HttpPost]
        public ActionResult UpdateProgramRequestStatus(ProgramRequestViewModel pr)
        {
            ProgramRepository repo = new ProgramRepository();
            AdminRepository Adminrepo = new AdminRepository();
            StatusChangeEmailViewModel sc = new StatusChangeEmailViewModel();

            int ProgramRequestID = pr.ProgramRequestID;
            int StatusId = pr.RequestStatus;

            bool ConfirmSessionDate = Adminrepo.CheckConfirmedSessionDate(ProgramRequestID);
            bool AdminSessionID = Adminrepo.CheckAdminSessionID(ProgramRequestID);
            //AdminSessionID = true;
            //cancelled or under review
            if (StatusId == 6 || StatusId==1)
            {


                repo.UpdateRequestStatusByAdmin(ProgramRequestID, StatusId);

                return Json(new { result = "success" });

            }


            if (ConfirmSessionDate)
            {
                //Active – Regional Ethics Review Pending
                if (StatusId == 2)
                {
                    //Your Event Information has been submitted for Regional Ethics Review
                    repo.UpdateRequestStatusByAdmin(ProgramRequestID, StatusId);
                    sc = Adminrepo.GetStatusChangeByAdminEmail(ProgramRequestID);

                    UserHelper.AdminChangeRequestStatusID2(sc);
                    return Json(new { result = "success" });

                }

                if (StatusId == 5)
                {

                    //Please upload the post-program materials 
                    repo.UpdateRequestStatusByAdmin(ProgramRequestID, StatusId);
                    sc = Adminrepo.GetStatusChangeByAdminEmail(ProgramRequestID);
                    UserHelper.AdminChangeRequestStatusID5(sc);
                    return Json(new { result = "success" });

                }

                if (StatusId == 4)
                {
                    //Completed – Session Closed

                    repo.UpdateRequestStatusByAdmin(ProgramRequestID, StatusId);
                   
                    return Json(new { result = "success" });

                }

               

                if ((StatusId == 3) && (AdminSessionID == true))
                {

                    //Your event has received regional ethics approval  
                    repo.UpdateRequestStatusByAdmin(ProgramRequestID, StatusId);
                    sc = Adminrepo.GetStatusChangeByAdminEmail(ProgramRequestID);
                    UserHelper.AdminChangeRequestStatusID3(sc);
                    return Json(new { result = "success" });
                }
                else
                {

                    return Json(new { error = "AdminSessionID" });

                }


               
            }
            else
            {
                return Json(new { error = "error" });


            }          

           

        }

        



        [HttpPost]
        public ActionResult Edit(ProgramRequestViewModel pr)
        {
            ProgramRepository repo = new ProgramRepository();
            if (ModelState.IsValid)
            {
                repo.EditProgramRequest(pr);
                return Json(new { result = "Success" });

            }

            return Json(new { error = "error" });

        }

        [HttpPost]
        public ActionResult Approved(ProgramRequestViewModel pr)
        {
            ProgramRepository repo = new ProgramRepository();

            repo.ApproveProgramRequest(pr);
            return Json(new { result = "Success" });

        }


        #region Users

        public ActionResult GetAllUsersExceptSpeakers()
        {

            List<UserModel> liUserModel;

            UserRepository ur = new UserRepository();
            liUserModel = ur.GetAllUsersExceptSpeakers();

            return Json(new { data = liUserModel }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult EditUser(int UserID)
        {
            UserModel um;
            UserRepository repo = new UserRepository();

            um = repo.GetUserByUserID(UserID);

            return View(um);
        }

        public ActionResult AddUser()
        {
            return View();
        }
        public ActionResult Reports()
        {
            UserModel user = UserHelper.GetLoggedInUser();
            if (user != null && user.UserType.Equals("7")) //only for admin
            {
                return View();
            }else
            {
                return RedirectToAction("Login", "Account");
            }
                
        }
        public ActionResult ExportToExcel()
        {


            try
            {


                AdminRepository ar = new AdminRepository();
                //need to use nuget manager to install EPPlus
                ExcelPackage pck = new ExcelPackage();
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("ProgramRequestReport");
                ws.Cells["A1"].Value = "Record ID";
                ws.Cells["B1"].Value = "Date Submitted";
                ws.Cells["C1"].Value = "Contact First Name";
                ws.Cells["D1"].Value = "Contact Last Name";
                ws.Cells["E1"].Value = "Program Name";
                ws.Cells["F1"].Value = "Modules Selected";
                ws.Cells["G1"].Value = "Program Status";
                ws.Cells["H1"].Value = "Program Date";              
                ws.Cells["I1"].Value = "Speaker";
                ws.Cells["J1"].Value = "Speaker Honoraria Amount"; 
                ws.Cells["K1"].Value = "Moderator";
                ws.Cells["L1"].Value = "Moderator Honoraria Amount";
                ws.Cells["M1"].Value = "Venue";
                ws.Cells["N1"].Value = "Location";
                ws.Cells["O1"].Value = "City";
                ws.Cells["P1"].Value = "Province";
                ws.Cells["Q1"].Value = "Final Attendance";
                ws.Cells["R1"].Value = "Event Type";
                int rowStart = 2;
                foreach (var item in ar.GetProgramRequestReport())
                {
                   ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                   ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("white")));
                    ws.Cells[string.Format("A{0}", rowStart)].Value = item.ProgramRequestID;
                    ws.Cells[string.Format("B{0}", rowStart)].Value = item.SubmittedDate;
                    ws.Cells[string.Format("C{0}", rowStart)].Value = item.ContactFirstName;
                    ws.Cells[string.Format("D{0}", rowStart)].Value = item.ContactLastName;
                    ws.Cells[string.Format("E{0}", rowStart)].Value = item.ProgramName;
                    ws.Cells[string.Format("F{0}", rowStart)].Value = item.ModulesSelected;
                    ws.Cells[string.Format("G{0}", rowStart)].Value = item.RequestStatus;
                    ws.Cells[string.Format("H{0}", rowStart)].Value = item.ConfirmedProgramDate;
                    ws.Cells[string.Format("I{0}", rowStart)].Value = item.Speaker;
                    ws.Cells[string.Format("J{0}", rowStart)].Value = item.SpeakerHonoraria;
                    ws.Cells[string.Format("K{0}", rowStart)].Value = item.Moderator;
                    ws.Cells[string.Format("L{0}", rowStart)].Value = item.ModeratorHonoraria;
                    ws.Cells[string.Format("M{0}", rowStart)].Value = item.LocationName;
                    ws.Cells[string.Format("N{0}", rowStart)].Value = item.LocationAddress;
                    ws.Cells[string.Format("O{0}", rowStart)].Value = item.LocationCity;
                    ws.Cells[string.Format("P{0}", rowStart)].Value = item.LocationProvince;
                    ws.Cells[string.Format("Q{0}", rowStart)].Value = item.FinalAttendance;
                    ws.Cells[string.Format("R{0}", rowStart)].Value = item.EventType;
                    rowStart = rowStart + 1;
                }
                //  ws.Cells["F1"].Value = string.Format("{0:dd MMMM yyyy} at ")

                ws.Cells["A:AZ"].AutoFitColumns();
                Response.Clear();

                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=ProgramRequestReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(pck.GetAsByteArray());
                Response.Flush();
                Response.End();
                return View("Reports");
            }
            catch (Exception e)
            {
                UserHelper.WriteToLog("Error Message:" + e.Message);
                return View("Reports");
            }

        }
        [HttpPost]
        public ActionResult AddUser(UserViewModel um)
        {

            try
            {
                UserRepository repo = new UserRepository();
                if (!ModelState.IsValid)
                {
                    return Json(new { error = "error" }, JsonRequestBehavior.AllowGet);
                   // return View("AddUser",um);

                }
                else
                {
                    ViewBag.SystemMsg = "";
                    repo.AddUser(um);

                
                    Task.Factory.StartNew(() => {
                        UserHelper.SendRequestActivationEmail(um, this);
                    });

                    return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {
                return Json(new { error = "error" }, JsonRequestBehavior.AllowGet);

                //Console.WriteLine("Error:" + e.Message);
                //ViewBag.ShowSystemMsg = "true";
                //Console.WriteLine("ViewBag.SystemMsg:" + ViewBag.SystemMsg);
                ////  ModelState.AddModelError("", e.Message);
                //// ModelState.AddModelError("", "Duplicate UserName");
                //return View("AddUser", um);
            }
            // return Json(new { result = true }, JsonRequestBehavior.AllowGet);


        }
        public ActionResult Users()
        {


            return View();
        }
        [HttpPost]
        public ActionResult DeleteUser(UserModel um)
        {
            ProgramRepository repo = new ProgramRepository();

            repo.DeleteUser(um);
            return Json(new { result = "Success" });

        }
        #endregion
        #region Speakers
        [HttpPost]
        public ActionResult ApproveSpeaker(SpeakerViewModel spVM)
        {
            UserRepository repo = new UserRepository();
            UserModel UMSpeaker = new UserModel();
            UserModel UMSalesRep = new UserModel();

            repo.ApproveSpeaker(spVM);
            UMSpeaker = repo.GetUserByuserid(spVM.ID);

            //send email to EmailSalesRep_SpeakerApproved

            UMSalesRep = repo.GetUserByUserID(UMSpeaker.UserIDRequestedBy);

            UserHelper.EmailSaleRep_SpeakerApproved(UMSalesRep, UMSpeaker);

            //send email to Speaker. this include activation or optout.
            if (String.IsNullOrEmpty(UMSpeaker.Language) || UMSpeaker.Language == "en")
                UserHelper.EmailSpeaker_SpeakerApproved(UMSpeaker);
            else
                UserHelper.EmailSpeaker_SpeakerApprovedfr(UMSpeaker);
            return Json(new { result = "Success" });

        }

        [HttpPost]
        public ActionResult EditSpeaker(SpeakerViewModel spVM)
        {

            try
            {
                UserRepository repo = new UserRepository();
                if (ModelState.IsValid)
                {
                    repo.EditSpeaker(spVM);
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);

                }
                else
                {

                    return Json(new { error = "error" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {

                Console.WriteLine("Error:" + e.Message);
                return Json(new { error = "error" }, JsonRequestBehavior.AllowGet);
            }
            // return Json(new { result = true }, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        public ActionResult AddSpeaker(SpeakerViewModel spVM)
        {

            try
            {
                UserRepository repo = new UserRepository();
                UserModel um = new UserModel();
                if (ModelState.IsValid)
                {
                    int id = repo.AddSpeaker(spVM);

                    um = repo.GetUserByuserid(id);
                   
                        UserHelper.EmailSpeaker_SpeakerApproved(um);
                  

                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);

                }
                else
                {

                    return Json(new { error = "error" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception e)
            {

                Console.WriteLine("Error:" + e.Message);
                return Json(new { error = "error" }, JsonRequestBehavior.AllowGet);
            }
            // return Json(new { result = true }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult AddSpeaker()
        {

            SpeakerViewModel spVM = new SpeakerViewModel();
            spVM.UserType = 2;//default to speaker

            return View(spVM);

        }
        [HttpGet]
        public ActionResult EditSpeaker(int userid)
        {
            UserModel um;
            UserRepository repo = new UserRepository();

            um = repo.GetUserByuserid(userid);

            return View("EditSpeaker", um);
        }


        [HttpGet]
        public ActionResult EditPayee(int UserID)
        {
            PayeeModel pm;
            AdminRepository repo = new AdminRepository();

            pm = repo.GetPayeeByUserID(UserID);

            return View("EditPayee", pm);
        }



        [HttpPost]
        public ActionResult EditPayee(PayeeModel pm)
        {

            try
            {
                AdminRepository repo = new AdminRepository();

                if (repo.UpdatePayee(pm))

                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { error = "error" }, JsonRequestBehavior.AllowGet);
            }catch(Exception e)
            {

                return Json(new { error = "error" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult GetAllSpeakers()
        {

            List<UserModel> liUserModel;

            UserRepository ur = new UserRepository();
            liUserModel = ur.GetAllSpeakers();

            return Json(new { data = liUserModel }, JsonRequestBehavior.AllowGet);


        }



        public ActionResult Speakers()
        {

            ViewBag.SpeakerCOIURL = ConfigurationManager.AppSettings["SpeakerCOIURL"];
          
            return View();
        }

       

        // GET: COIUpload
        [HttpGet]
        public ActionResult COIForm(int UserID)
        {
            ViewBag.UserID = UserID;
            ViewBag.SpeakerCOIURL = ConfigurationManager.AppSettings["SpeakerCOIURL"];

            AdminRepository ar = new AdminRepository();
            UserRegistration ur =ar.GetUserRegistration(UserID);

            var list = ar.GetProgramList(UserID);
            ViewBag.ProgramList = list;

            if (ur != null)

                return View(ur);
            else
                return View(new UserRegistration());
            //um = repo.GetUserByUserID(UserID);

            //ProgramRepository pr = new ProgramRepository();
            //SessionUpload su = new SessionUpload();

            //su = pr.GetSessionUpload(ProgramRequestID);
            //if (su != null)
            //    return View(su);
            //else
            //{

            //    return View(new SessionUpload());
            //}
            //get fileupload information

            //ProgramRequestCancellationVM PReqCancel;
            //ProgramRepository PRepo = new ProgramRepository();
            //PReqCancel = PRepo.GetProgramRequestCancellationbyID(ProgramRequestID);
            //if (PReqCancel != null)
            //{

            //    return View(PReqCancel);
            //}
            //else

        }



        //handle COI form upload
        public JsonResult COIUpload(int UserID)
        {
           
            string solutiondir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            string SpeakerSite = "CPDPortalSpeaker";
            string COIFormPath = Server.MapPath(solutiondir + "/" + SpeakerSite + ConfigurationManager.AppSettings["UserFileUploadPath"] + UserID + "/COIForm");

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
                        var supportedTypes = new[] { "doc", "docx", "pdf","DOC","DOCX","PDF" };
                        var extension = Path.GetExtension(file).Substring(1);
                        if (!supportedTypes.Contains(extension))
                        {
                            var error = "File Extension Is InValid";
                            return Json(new { error = error });
                        }



                        //check if the folder exists, if not then make one. 
                        //  string FolderPath = Server.MapPath(ConfigurationManager.AppSettings["UserFileUploadPath"] + id + "/ProgramID/" + programId + "/");


                        if (!Directory.Exists(COIFormPath))
                        {
                            Directory.CreateDirectory(COIFormPath);
                        }
                        else
                        {

                            //let's clean up the directory before accept the new file
                            System.IO.DirectoryInfo di = new DirectoryInfo(COIFormPath);
                            foreach (FileInfo ff in di.EnumerateFiles())
                            {
                                ff.Delete();
                            }

                        }
                        // get a stream
                        var stream = fileContent.InputStream;
                        // and optionally write the file to disk
                        var fileName = Path.GetFileName(file);
                        //rename files to prevent user upload unlimited number of different files into the system.
                        string path = COIFormPath + "/" + "COIForm." + extension;
                        //var path = Path.Combine(Server.MapPath(EvaluationPath + "/"), fileName);
                        using (var fileStream = System.IO.File.Create(path))
                        {
                            stream.CopyTo(fileStream);
                        }
                        //save file full path and uploaded flag to db
                        //SessionUpload objSessionUpload = new SessionUpload();
                        //objSessionUpload.ProgramRequestID = Convert.ToInt32(ProgramRequestID);
                        //objSessionUpload.EvaluationFullPath = path;//full path
                        //objSessionUpload.EvaluationUploaded = true;
                        //objSessionUpload.EvaluationFileName = fileName;
                        //objSessionUpload.EvaluationFileExt = extension;
                        AdminRepository pr = new AdminRepository();

                        pr.UpdateCOIForm(UserID, extension);

                        returnPath = COIFormPath + "\\" + "COIForm." + extension;
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
        #endregion Speakers

    }
}