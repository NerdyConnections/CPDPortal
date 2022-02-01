using CPDPortalMVC.App_Data;
using CPDPortalMVC.DAL;
using CPDPortalMVC.Models;
using CPDPortalMVC.Util;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CPDPortalMVC.Controllers
{
    public class DynamicPDFController : Controller
    {
      
        public ActionResult COA(int ProgramRequestID)
        {
            try
            {
                System.DateTime ThisMoment = System.DateTime.Now;
                String RandomModifer = ThisMoment.Year + "_" + ThisMoment.Month + "_" + ThisMoment.Day + "_" + ThisMoment.Hour + "_" + ThisMoment.Minute + "_" + ThisMoment.Second + "_" + ThisMoment.Millisecond;

                UserModel user = UserHelper.GetLoggedInUser();
                if (user != null)
                {
                    ProgramRepository programRepo = new ProgramRepository();
                    ProgramRequest pr = programRepo.GetProgramRequestbyQueryString(ProgramRequestID);

                    byte[] bytes = GetCOAPdf(pr);
                    PDFResult pdfResult = new PDFResult(bytes);
                    return pdfResult;
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }

                // efm.DisplayPDF = true;

                // efm.FileName = "/PDF/EvalForm5/Evaluation" + RandomModifer + ".pdf";


                // return View("ProgramMaterials_5", efm);
            }

            catch (Exception e)
            {
                return null;
                // efm.DisplayPDF = false;
                // return View("ProgramMaterials_5", efm);
            }

        }
        public ActionResult SignInSheet(int ProgramRequestID)
        {
            try
            {
                System.DateTime ThisMoment = System.DateTime.Now;
                String RandomModifer = ThisMoment.Year + "_" + ThisMoment.Month + "_" + ThisMoment.Day + "_" + ThisMoment.Hour + "_" + ThisMoment.Minute + "_" + ThisMoment.Second + "_" + ThisMoment.Millisecond;

                UserModel user = UserHelper.GetLoggedInUser();
                if (user != null)
                {
                    ProgramRepository programRepo = new ProgramRepository();
                    ProgramRequest pr = programRepo.GetProgramRequestbyQueryString(ProgramRequestID);

                    byte[] bytes = GetSignInSheetPdf(pr);
                    PDFResult pdfResult = new PDFResult(bytes);
                    return pdfResult;
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }

                // efm.DisplayPDF = true;

                // efm.FileName = "/PDF/EvalForm5/Evaluation" + RandomModifer + ".pdf";


                // return View("ProgramMaterials_5", efm);
            }

            catch (Exception e)
            {
                return null;
                // efm.DisplayPDF = false;
                // return View("ProgramMaterials_5", efm);
            }

        }
        public ActionResult NationalInvitation(int ProgramRequestID)
        {
            try
            {
                System.DateTime ThisMoment = System.DateTime.Now;
                String RandomModifer = ThisMoment.Year + "_" + ThisMoment.Month + "_" + ThisMoment.Day + "_" + ThisMoment.Hour + "_" + ThisMoment.Minute + "_" + ThisMoment.Second + "_" + ThisMoment.Millisecond;

                UserModel user = UserHelper.GetLoggedInUser();
                if (user != null)
                {
                    ProgramRepository programRepo = new ProgramRepository();
                   // ProgramRequest pr = programRepo.GetProgramRequestbyQueryString(ProgramRequestID);
                   ProgramRequestIIModel pr = programRepo.GetProgramRequestII(ProgramRequestID);

                   
                    byte[] bytes = GetNationalInvitationPdf(pr);
                    PDFResult pdfResult = new PDFResult(bytes);
                    return pdfResult;
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }

                // efm.DisplayPDF = true;

                // efm.FileName = "/PDF/EvalForm5/Evaluation" + RandomModifer + ".pdf";


                // return View("ProgramMaterials_5", efm);
            }

            catch (Exception e)
            {
                return null;
                // efm.DisplayPDF = false;
                // return View("ProgramMaterials_5", efm);
            }

        }
        public ActionResult RegionalInvitation(int ProgramRequestID)
        {
            try
            {
                System.DateTime ThisMoment = System.DateTime.Now;
                String RandomModifer = ThisMoment.Year + "_" + ThisMoment.Month + "_" + ThisMoment.Day + "_" + ThisMoment.Hour + "_" + ThisMoment.Minute + "_" + ThisMoment.Second + "_" + ThisMoment.Millisecond;

                UserModel user = UserHelper.GetLoggedInUser();
                if (user != null)
                {
                    ProgramRepository programRepo = new ProgramRepository();
                   // ProgramRequest pr = programRepo.GetProgramRequestbyQueryString(ProgramRequestID);
                    ProgramRequestIIModel pr = programRepo.GetProgramRequestII(ProgramRequestID);
                    byte[] bytes = GetRegionalInvitationPdf(pr);
                    PDFResult pdfResult = new PDFResult(bytes);
                    return pdfResult;
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }

                // efm.DisplayPDF = true;

                // efm.FileName = "/PDF/EvalForm5/Evaluation" + RandomModifer + ".pdf";


                // return View("ProgramMaterials_5", efm);
            }

            catch (Exception e)
            {
                return null;
                // efm.DisplayPDF = false;
                // return View("ProgramMaterials_5", efm);
            }

        }
        private byte[] GetEvalFormPdf(ProgramRequest pr)
        {

            string speakerModerator = string.Empty;
            SpeakerRepository speakerRepo = new SpeakerRepository();
            SpeakerModel Speaker1 = speakerRepo.GetSpeakerByuserid(pr.ProgramSpeakerID);
            SpeakerModel Moderator = null;
            if (pr.ProgramModeratorID != null)
            {
                Moderator = speakerRepo.GetSpeakerByuserid(pr.ProgramModeratorID ?? 0);

            }
            //first add speaker
            speakerModerator = Speaker1.FirstName + " " + Speaker1.LastName;
            //if moderator exists, add moderator too
            if (pr.ProgramModeratorID != null && Moderator != null)
            {
                speakerModerator = ", " + Moderator.FirstName + " " + Moderator.LastName;
            }


            GenPdf genpdf = new GenPdf(Server.MapPath("~/Template/" + pr.ProgramID + "/"));
            genpdf.Create();


            genpdf.AddForm("EvalForm.pdf");//for evaluation form template
            genpdf.AddField("Event Date", Convert.ToString(pr.ConfirmedSessionDate), 0);
            genpdf.AddField("Event Location", pr.LocationName, 0);
            genpdf.AddField("Speaker / Moderator Name", speakerModerator, 0);

            genpdf.FinalizeForm(false);

            return genpdf.Close();

        }
        private byte[] GetCOAPdf(ProgramRequest pr)
        {

            string speakerModerator = string.Empty;
            SpeakerRepository speakerRepo = new SpeakerRepository();
            SpeakerModel Speaker1 = speakerRepo.GetSpeakerByuserid(pr.ProgramSpeakerID);
            SpeakerModel Moderator = null;
            if (pr.ProgramModeratorID != null)
            {
                Moderator = speakerRepo.GetSpeakerByuserid(pr.ProgramModeratorID ?? 0);

            }
            //first add speaker
            speakerModerator = Speaker1.FirstName + " " + Speaker1.LastName;
            //if moderator exists, add moderator too
            if (pr.ProgramModeratorID != null && Moderator != null)
            {
                speakerModerator = ", " + Moderator.FirstName + " " + Moderator.LastName;
            }


            GenPdf genpdf = new GenPdf(Server.MapPath("~/Template/" + pr.ProgramID + "/"));
            genpdf.Create();


            genpdf.AddForm("COA.pdf");//for evaluation form template
                                      //genpdf.AddField("Participant's Name", pr.ContactFirstName + " " + pr.ContactLastName, 0);  physican's name filled out by sales rep
                                      // genpdf.AddField("Session ID", pr.AdminSessionID, 0);  already hardcoded in template
                                      // genpdf.AddField("Provincial Chapter", "ON", 0); user select from dropdown
            // genpdf.AddField("Credits", "1.5", 0); user select from dropdown
            genpdf.AddField("Event Date", Convert.ToDateTime(pr.ConfirmedSessionDate).ToString("MMMM dd, yyyy"), 0);
            genpdf.AddField("Event City and Province", pr.LocationCity + ", " + pr.LocationProvince, 0);

            genpdf.AddField("Session ID", pr.AdminSessionID, 0);
            genpdf.AddField("Provincial Chapter", pr.LocationProvinceFullName + " Chapter", 0);
            genpdf.AddField("Credits", pr.TotalCredits.ToString(), 0);
            genpdf.FinalizeForm(false);

            return genpdf.Close();

        }
        private byte[] GetSignInSheetPdf(ProgramRequest pr)
        {

            string speakerModerator = string.Empty;
            SpeakerRepository speakerRepo = new SpeakerRepository();
            SpeakerModel Speaker1 = speakerRepo.GetSpeakerByuserid(pr.ProgramSpeakerID);
            SpeakerModel Moderator = null;
            if (pr.ProgramModeratorID != null)
            {
                Moderator = speakerRepo.GetSpeakerByuserid(pr.ProgramModeratorID ?? 0);

            }
            //first add speaker
            speakerModerator = Speaker1.FirstName + " " + Speaker1.LastName;
            //if moderator exists, add moderator too
            if (pr.ProgramModeratorID != null && Moderator != null)
            {
                speakerModerator = ", " + Moderator.FirstName + " " + Moderator.LastName;
            }


            GenPdf genpdf = new GenPdf(Server.MapPath("~/Template/" + pr.ProgramID + "/"));
            genpdf.Create();


            genpdf.AddForm("SignInSheet.pdf");//for evaluation form template
                                      //genpdf.AddField("Participant's Name", pr.ContactFirstName + " " + pr.ContactLastName, 0);  physican's name filled out by sales rep
                                      // genpdf.AddField("Session ID", pr.AdminSessionID, 0);  already hardcoded in template
                                      // genpdf.AddField("Provincial Chapter", "ON", 0); user select from dropdown
                                      // genpdf.AddField("Credits", "1.5", 0); user select from dropdown
            genpdf.AddField("Program Date", Convert.ToDateTime(pr.ConfirmedSessionDate).ToString("MMMM dd, yyyy"), 0);
            
                 genpdf.AddField("Session ID", Convert.ToString(pr.AdminSessionID), 0);
            genpdf.AddField("City & Province", pr.LocationCity + ", " + pr.LocationProvince, 0);
            genpdf.AddField("Speaker 1", Speaker1.FirstName + " " + Speaker1.LastName, 0);
            if (Moderator != null)
            {
                genpdf.AddField("Moderator", Moderator.FirstName + ", " + Moderator.LastName, 0);

            }

            genpdf.FinalizeForm(false);

            return genpdf.Close();

        }

        private byte[] GetNationalInvitationPdf(ProgramRequestIIModel pr)
        {

            string speakerModerator = string.Empty;
            SpeakerRepository speakerRepo = new SpeakerRepository();
            SpeakerModel Speaker1 = speakerRepo.GetSpeakerByuserid(pr.ProgramSpeakerID);
            SpeakerModel Moderator = null;
            try
            {
                if (pr.ProgramModeratorID != null)
                {
                    Moderator = speakerRepo.GetSpeakerByuserid(pr.ProgramModeratorID ?? 0);

                }
                //first add speaker
                speakerModerator = Speaker1.FirstName + " " + Speaker1.LastName;
                //if moderator exists, add moderator too
                if (pr.ProgramModeratorID != null && Moderator != null)
                {
                    speakerModerator = ", " + Moderator.FirstName + " " + Moderator.LastName;
                }


                GenPdf genpdf = new GenPdf(Server.MapPath("~/Template/" + pr.ProgramID + "/"));
                genpdf.Create();
                string TemplateFormName = "";
                if (pr.EventType == "Webcast")
                    TemplateFormName = "NationalInvitationWeb.pdf";
                else
                    TemplateFormName = "NationalInvitationLive.pdf";
                genpdf.AddForm(TemplateFormName);//for evaluation form template
                                                 //genpdf.AddField("Participant's Name", pr.ContactFirstName + " " + pr.ContactLastName, 0);  physican's name filled out by sales rep
                                                 // genpdf.AddField("Session ID", pr.AdminSessionID, 0);  already hardcoded in template
                                                 // genpdf.AddField("Provincial Chapter", "ON", 0); user select from dropdown
                                                 // genpdf.AddField("Credits", "1.5", 0); user select from dropdown
                if (!String.IsNullOrEmpty(pr.ConfirmedSessionDate))//in case there no confirmed session date it won't throw exception
                genpdf.AddField("Event Date", Convert.ToDateTime(pr.ConfirmedSessionDate).ToString("MMMM dd, yyyy"), 0);

                genpdf.AddField("Location", pr.LocationName + " " + pr.LocationAddress + " " + pr.LocationCity + " " + pr.LocationProvince, 0);

                genpdf.AddField("Start Time", pr.ProgramStartTime, 0);
                genpdf.AddField("End Time", pr.ProgramEndTime, 0);
                genpdf.AddField("RSVP Information", pr.ContactFirstName + " " + pr.ContactLastName + " " + pr.ContactPhone + " " + pr.ContactEmail, 0);

                genpdf.AddField("Speaker 1", Speaker1.FirstName + " " + Speaker1.LastName, 0);
                if (Moderator != null)
                {
                    genpdf.AddField("Moderator", Moderator.FirstName + ", " + Moderator.LastName, 0);

                }
                genpdf.AddField("Provincial Chapter", pr.LocationProvinceFullName + " Chapter", 0);
                genpdf.FinalizeForm(false);

                return genpdf.Close();
            }catch (Exception e)
            {
                return null;
            }

        }

        private byte[] GetRegionalInvitationPdf(ProgramRequestIIModel pr)
        {

            string speakerModerator = string.Empty;
            SpeakerRepository speakerRepo = new SpeakerRepository();
            SpeakerModel Speaker1 = speakerRepo.GetSpeakerByuserid(pr.ProgramSpeakerID);
            SpeakerModel Moderator = null;
            if (pr.ProgramModeratorID != null)
            {
                Moderator = speakerRepo.GetSpeakerByuserid(pr.ProgramModeratorID ?? 0);

            }
            try
            {
                //first add speaker
                speakerModerator = Speaker1.FirstName + " " + Speaker1.LastName;
                //if moderator exists, add moderator too
                if (pr.ProgramModeratorID != null && Moderator != null)
                {
                    speakerModerator = ", " + Moderator.FirstName + " " + Moderator.LastName;
                }


                GenPdf genpdf = new GenPdf(Server.MapPath("~/Template/" + pr.ProgramID + "/"));
                genpdf.Create();
                string TemplateFormName;
                if (pr.EventType == "Webcast")
                    TemplateFormName = "RegionalInvitationWeb.pdf";
                else
                    TemplateFormName = "RegionalInvitationLive.pdf";
                genpdf.AddForm(TemplateFormName);//for evaluation form template
                                                 //genpdf.AddField("Participant's Name", pr.ContactFirstName + " " + pr.ContactLastName, 0);  physican's name filled out by sales rep
                                                 // genpdf.AddField("Session ID", pr.AdminSessionID, 0);  already hardcoded in template
                                                 // genpdf.AddField("Provincial Chapter", "ON", 0); user select from dropdown
                                                 // genpdf.AddField("Credits", "1.5", 0); user select from dropdown
                if (!String.IsNullOrEmpty(pr.ConfirmedSessionDate))//in case there no confirmed session date it won't throw exception

                    genpdf.AddField("Event Date", Convert.ToDateTime(pr.ConfirmedSessionDate).ToString("MMMM dd, yyyy"), 0);

                genpdf.AddField("Location", pr.LocationName + " " + pr.LocationAddress + " " + pr.LocationCity + " " + pr.LocationProvince, 0);

                genpdf.AddField("Start Time", pr.ProgramStartTime, 0);
                genpdf.AddField("End Time", pr.ProgramEndTime, 0);
                genpdf.AddField("RSVP Information", pr.ContactFirstName + " " + pr.ContactLastName + " " + pr.ContactPhone + " " + pr.ContactEmail, 0);
                genpdf.AddField("Credits", pr.TotalCredits.ToString(), 0);
                genpdf.AddField("Provincial Chapter", pr.LocationProvinceFullName + " Chapter", 0);

                genpdf.AddField("Speaker 1", Speaker1.FirstName + " " + Speaker1.LastName, 0);
                if (Moderator != null)
                {
                    genpdf.AddField("Moderator", Moderator.FirstName + ", " + Moderator.LastName, 0);

                }

                genpdf.FinalizeForm(false);

                return genpdf.Close();
            }
            catch (Exception e)
            {
                return null;
            }

        }
        [HttpGet]
        public ActionResult EvalForm5(int ProgramRequestID)
        {
            //get the evaluation form for the new program
            int CaseCounter = 1;
            string fileName = string.Empty; //contains the file name of the resultant pdf
            bool HasBorder = false;  //set to false when development is completed. 
            System.DateTime ThisMoment = System.DateTime.Now;
            String RandomModifer = ThisMoment.Year + "_" + ThisMoment.Month + "_" + ThisMoment.Day + "_" + ThisMoment.Hour + "_" + ThisMoment.Minute + "_" + ThisMoment.Second + "_" + ThisMoment.Millisecond;
            string path = Server.MapPath("/PDF/EvalForm5");
            string imagepath = Server.MapPath("/Images/EvalForm5");
            var EvaluationDoc = new Document(PageSize.A4, 18, 18, 18, 18);
            //ViewBag.ProgramRequestStatusCounts = UserHelper.GetProgramRequestStatusCounts(Convert.ToInt32(Session["ProgramID"]));
            PdfWriter.GetInstance(EvaluationDoc, new FileStream(path + "/Evaluation" + RandomModifer + ".pdf", FileMode.Create));
            BaseFont HelveticaBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            DashboardRepository DBRepo = new DashboardRepository();
            EvalForm5Model efm = DBRepo.GetEvalForm5Model(ProgramRequestID);
            iTextSharp.text.Font Helvetica = new iTextSharp.text.Font(HelveticaBase, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


            try
            {
                //set up top image
                //set up top image
                EvaluationDoc.Open();
                EvaluationDoc.NewPage();
                iTextSharp.text.Image EvalTop1 = iTextSharp.text.Image.GetInstance(imagepath + "/MainHeader.jpg");

                EvalTop1.ScalePercent(54f);
                EvaluationDoc.Add(EvalTop1);
                //end of set up top image

                float[] widths = new float[] { 1f, 2f, 1f, 2f };//speaker column is 2 times wider than evaluation column
                PdfPTable DateLocation = new PdfPTable(4);
                DateLocation.KeepTogether = true;
                DateLocation.TotalWidth = 523f;
                DateLocation.SetWidths(widths);
                DateLocation.LockedWidth = true;
                DateLocation.SpacingBefore = 10f;


                DateLocation.SpacingAfter = 10f;
                DateLocation.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right

                //cell 1
                iTextSharp.text.Image DateLabel = iTextSharp.text.Image.GetInstance(imagepath + "/DateLabel.jpg");
                DateLabel.ScalePercent(50f);
                PdfPCell DateLabelCell = new PdfPCell(DateLabel);
                //cell 2
                PdfPCell DateCell = new PdfPCell(new Phrase(efm.ProgramDate, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));


                DateCell.HorizontalAlignment = 1;
                DateCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                DateCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255,255);
                //cell 3
                iTextSharp.text.Image LocationLabel = iTextSharp.text.Image.GetInstance(imagepath + "/LocationLabel.jpg");
                LocationLabel.ScalePercent(50f);
                PdfPCell LocationLabelCell = new PdfPCell(LocationLabel);
                LocationLabelCell.HorizontalAlignment = 0; //alignleft
                                                           //cell 4
                                                           //iTextSharp.text.Image SpaceFieldLoc = iTextSharp.text.Image.GetInstance(imagepath + "/SpaceField.jpg");
                                                           //DateLabel.ScalePercent(30f);
                                                           //PdfPCell SpaceFieldCellLocation = new PdfPCell(SpaceFieldLoc);

                PdfPCell LocationCell = new PdfPCell(new Phrase(efm.ProgramLocation, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                LocationCell.HorizontalAlignment = 1;
                LocationCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                LocationCell.BackgroundColor = new BaseColor(255, 255, 255);

                if (HasBorder == false)
                    DateLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    DateCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    LocationLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    LocationCell.Border = iTextSharp.text.Rectangle.NO_BORDER;


                DateLocation.AddCell(DateLabelCell);
                DateLocation.AddCell(DateCell);
                DateLocation.AddCell(LocationLabelCell);
                DateLocation.AddCell(LocationCell);



                EvaluationDoc.Add(DateLocation);


                //end of set up program date/location

                //set up top image
                iTextSharp.text.Image EvalTop2 = iTextSharp.text.Image.GetInstance(imagepath + "/AfterDateLocation.jpg");
                EvalTop2.Alignment = Element.ALIGN_CENTER;

                EvalTop2.ScalePercent(52f);
                EvaluationDoc.Add(EvalTop2);
               

                
                if (CaseCounter >= 3)
                {
                    EvaluationDoc.NewPage();
                    CaseCounter = 0;
                }






                //make sure speaker evaluation starts on a new page
                //setting up speaker evaluation
               
                if (!String.IsNullOrEmpty(efm.Speaker1))
                {
                    iTextSharp.text.Image SpeakerTitle = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerTitle.jpg");
                    SpeakerTitle.ScalePercent(50f);
                    EvaluationDoc.Add(SpeakerTitle);

                    float[] SpeakerTableWidths = new float[] { 1f };//speaker column is 2 times wider than evaluation column
                    PdfPTable SpeakerTable = new PdfPTable(1);
                    SpeakerTable.KeepTogether = true;
                    SpeakerTable.TotalWidth = 523f;
                    SpeakerTable.SetWidths(SpeakerTableWidths);
                    SpeakerTable.LockedWidth = true;
                    SpeakerTable.SpacingBefore = 10f;

                    SpeakerTable.SpacingAfter = 10f;
                    SpeakerTable.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right


                    //cell 1
                    PdfPCell SpeakerCell = new PdfPCell(new Phrase(efm.Speaker1, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.DARK_GRAY)));
                    SpeakerCell.HorizontalAlignment = 0;
                    SpeakerCell.BackgroundColor = new BaseColor(255, 255, 255);
                    SpeakerCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    //speaker form cell
                    iTextSharp.text.Image SpeakerForm = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerForm.jpg");
                    SpeakerForm.ScalePercent(50f);
                   


                    if (HasBorder == false)
                        SpeakerForm.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    if (HasBorder == false)
                        SpeakerCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    SpeakerTable.AddCell(SpeakerCell);
                    EvaluationDoc.Add(SpeakerTable);
                    EvaluationDoc.Add(SpeakerForm);



                }
                string[] Moderator2FirstLastName = efm.Moderator.Split(',');
                if (!String.IsNullOrEmpty(Moderator2FirstLastName[0]))
                {
                    float[] ModeratorTableWidths = new float[] { 1f };//speaker column is 2 times wider than evaluation column
                    PdfPTable ModeratorTable = new PdfPTable(1);
                    ModeratorTable.KeepTogether = true;
                    ModeratorTable.TotalWidth = 523f;
                    ModeratorTable.SetWidths(ModeratorTableWidths);
                    ModeratorTable.LockedWidth = true;
                    ModeratorTable.SpacingBefore = 10f;

                    ModeratorTable.SpacingAfter = 10f;
                    ModeratorTable.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right
                    iTextSharp.text.Image SpeakerTitle = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerTitle.jpg");
                    SpeakerTitle.ScalePercent(50f);
                    EvaluationDoc.Add(SpeakerTitle);

                    float[] SpeakerTableWidths = new float[] { 1f };//speaker column is 2 times wider than evaluation column

                   


                    //cell 1
                    PdfPCell ModeratorCell = new PdfPCell(new Phrase(efm.Moderator, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.DARK_GRAY)));
                    ModeratorCell.HorizontalAlignment = 0;
                    ModeratorCell.BackgroundColor = new BaseColor(255, 255, 255);
                    ModeratorCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    //speaker form cell
                    iTextSharp.text.Image SpeakerForm = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerForm.jpg");
                    SpeakerForm.ScalePercent(50f);
                    EvaluationDoc.Add(SpeakerForm);


                    if (HasBorder == false)
                        SpeakerForm.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    if (HasBorder == false)
                        ModeratorCell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                    ModeratorTable.AddCell(ModeratorCell);
                    EvaluationDoc.Add(ModeratorTable);



                }
                if (CaseCounter >= 3)
                {
                    EvaluationDoc.NewPage();
                    CaseCounter = 0;
                }









                
                if (CaseCounter >= 3)
                {
                    EvaluationDoc.NewPage();
                    CaseCounter = 0;
                }
                string[] moderatorFirstLastName = efm.Moderator.Split(',');
                

                EvaluationDoc.NewPage();
                CaseCounter = 0;





                //setting up this program
                iTextSharp.text.Image LastPage = iTextSharp.text.Image.GetInstance(imagepath + "/LastPage.jpg");
                LastPage.Alignment = Element.ALIGN_CENTER;
                LastPage.ScalePercent(52f);
                EvaluationDoc.Add(LastPage);
                efm.DisplayPDF = true;

                fileName = "/PDF/EvalForm5/Evaluation" + RandomModifer + ".pdf";
                //  return File(new FileStream(Server.MapPath("~/App_Data/" + fileName), FileMode.Open), "application/octetstream", fileName);
                return Redirect(fileName);
            }
            catch (Exception e)
            {
                return Redirect(fileName);
                // return View("ProgramMaterials", efm);
            }
            finally
            {
                EvaluationDoc.Close();
            }
        }
        [HttpGet]
        public ActionResult GetNationalInvitationPdf7(int ProgramRequestID)
        {
            //get the evaluation form for the new program
            int ModuleCounter = 0;
            string fileName = string.Empty; //contains the file name of the resultant pdf
            bool HasBorder = false;  //set to false when development is completed. 
            System.DateTime ThisMoment = System.DateTime.Now;
            String RandomModifer = ThisMoment.Year + "_" + ThisMoment.Month + "_" + ThisMoment.Day + "_" + ThisMoment.Hour + "_" + ThisMoment.Minute + "_" + ThisMoment.Second + "_" + ThisMoment.Millisecond;
            string path = Server.MapPath("/PDF/NationalInvitation");
            string imagepath = Server.MapPath("/Images/NationalInvitation7");
            var EvaluationDoc = new Document(PageSize.A4, 18, 18, 18, 18);
            //ViewBag.ProgramRequestStatusCounts = UserHelper.GetProgramRequestStatusCounts(Convert.ToInt32(Session["ProgramID"]));
            PdfWriter writer= PdfWriter.GetInstance(EvaluationDoc, new FileStream(path + "/NationalInvitation7" + RandomModifer + ".pdf", FileMode.Create));
            BaseFont HelveticaBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            BaseFont HelveticaBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);


            DashboardRepository DBRepo = new DashboardRepository();
            NationalInvitation7Model nim7 = DBRepo.GetNationalInvitation7Model(ProgramRequestID);
            iTextSharp.text.Font Helvetica = new iTextSharp.text.Font(HelveticaBase, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


            try
            {
                //set up top image
                //set up top image
                EvaluationDoc.Open();
                EvaluationDoc.NewPage();
                iTextSharp.text.Image EvalTop1 = iTextSharp.text.Image.GetInstance(imagepath + "/MainHeader.jpg");

                EvalTop1.ScalePercent(54f);
                EvaluationDoc.Add(EvalTop1);
                //end of set up top image

                float[] widths = new float[] { 1f, 2f, 1f, 2f };//speaker column is 2 times wider than evaluation column
                PdfPTable DateLocation = new PdfPTable(4);
                DateLocation.KeepTogether = true;
                DateLocation.TotalWidth = 523f;
                DateLocation.SetWidths(widths);
                DateLocation.LockedWidth = true;
                DateLocation.SpacingBefore = 10f;


                DateLocation.SpacingAfter = 10f;
                DateLocation.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right

                //cell 1
                iTextSharp.text.Image DateLabel = iTextSharp.text.Image.GetInstance(imagepath + "/DateLabel.jpg");
                DateLabel.ScalePercent(50f);
                PdfPCell DateLabelCell = new PdfPCell(DateLabel);
                //cell 2
                PdfPCell DateCell = new PdfPCell(new Phrase(nim7.ProgramDate, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));


                DateCell.HorizontalAlignment = 1;
                DateCell.VerticalAlignment = Element.ALIGN_TOP;
                DateCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                //cell 3
                iTextSharp.text.Image LocationLabel = iTextSharp.text.Image.GetInstance(imagepath + "/LocationLabel.jpg");
                LocationLabel.ScalePercent(50f);
                PdfPCell LocationLabelCell = new PdfPCell(LocationLabel);
                LocationLabelCell.HorizontalAlignment = 0; //alignleft
                                                           //cell 4
                                                           //iTextSharp.text.Image SpaceFieldLoc = iTextSharp.text.Image.GetInstance(imagepath + "/SpaceField.jpg");
                                                           //DateLabel.ScalePercent(30f);
                                                           //PdfPCell SpaceFieldCellLocation = new PdfPCell(SpaceFieldLoc);

                PdfPCell LocationCell = new PdfPCell(new Phrase(nim7.ProgramLocation, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                LocationCell.HorizontalAlignment = 1;
                LocationCell.VerticalAlignment = Element.ALIGN_TOP;
                LocationCell.BackgroundColor = new BaseColor(255, 255, 255);

                if (HasBorder == false)
                    DateLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    DateCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    LocationLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    LocationCell.Border = iTextSharp.text.Rectangle.NO_BORDER;


                DateLocation.AddCell(DateLabelCell);
                DateLocation.AddCell(DateCell);
                DateLocation.AddCell(LocationLabelCell);
                DateLocation.AddCell(LocationCell);



                EvaluationDoc.Add(DateLocation);


                //end of set up program date/location

                //set up Program Start Time /End Time
                float[] ProgramWidths = new float[] { 1f, 1f, 1f, 1f };//speaker column is 2 times wider than evaluation column
                PdfPTable ProgramTime = new PdfPTable(4);
                ProgramTime.KeepTogether = true;
                ProgramTime.TotalWidth = 523f;
                ProgramTime.SetWidths(ProgramWidths);
                ProgramTime.LockedWidth = true;
                ProgramTime.SpacingBefore = 10f;


                ProgramTime.SpacingAfter = 10f;
                ProgramTime.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right

                //cell 1
                iTextSharp.text.Image ProgramStartLabel = iTextSharp.text.Image.GetInstance(imagepath + "/ProgramStartTime.jpg");
                ProgramStartLabel.ScalePercent(50f);
                PdfPCell ProgramStartLabelCell = new PdfPCell(ProgramStartLabel);
                //cell 2
                PdfPCell ProgramStartCell = new PdfPCell(new Phrase(nim7.ProgramStartTime, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                ProgramStartCell.HorizontalAlignment = 1;
                ProgramStartCell.VerticalAlignment = Element.ALIGN_TOP;
                ProgramStartCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                //cell 3
                iTextSharp.text.Image ProgramEndLabel = iTextSharp.text.Image.GetInstance(imagepath + "/ProgramEndTime.jpg");
                ProgramEndLabel.ScalePercent(50f);
                PdfPCell ProgramEndLabelCell = new PdfPCell(ProgramEndLabel);

                //cell 4
                //iTextSharp.text.Image SpaceFieldLoc = iTextSharp.text.Image.GetInstance(imagepath + "/SpaceField.jpg");
                //DateLabel.ScalePercent(30f);
                //PdfPCell SpaceFieldCellLocation = new PdfPCell(SpaceFieldLoc);

                PdfPCell ProgramEndCell = new PdfPCell(new Phrase(nim7.ProgramEndTime, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                ProgramEndCell.HorizontalAlignment = 1;
                ProgramEndCell.VerticalAlignment = Element.ALIGN_TOP;
                ProgramEndCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);

                if (HasBorder == false)
                    ProgramStartLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    ProgramStartCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    ProgramEndLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    ProgramEndCell.Border = iTextSharp.text.Rectangle.NO_BORDER;


                ProgramTime.AddCell(ProgramStartLabelCell);
                ProgramTime.AddCell(ProgramStartCell);
                ProgramTime.AddCell(ProgramEndLabelCell);
                ProgramTime.AddCell(ProgramEndCell);



                EvaluationDoc.Add(ProgramTime);

                //end of program start time/end time

                //speaker(s) row

                float[] SpeakerWidths = new float[] { 1f, 3f};//speaker column is 2 times wider than evaluation column
                PdfPTable SpeakerTable = new PdfPTable(2);
                SpeakerTable.KeepTogether = true;
                SpeakerTable.TotalWidth = 523f;
                SpeakerTable.SetWidths(SpeakerWidths);
                SpeakerTable.LockedWidth = true;
                SpeakerTable.SpacingBefore = 10f;


                SpeakerTable.SpacingAfter = 10f;
                SpeakerTable.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right

                //cell 1
                iTextSharp.text.Image SpeakerLabel = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerLabel.jpg");
                SpeakerLabel.ScalePercent(50f);
                PdfPCell SpeakerLabelCell = new PdfPCell(SpeakerLabel);
                //cell 2
                string speakers = nim7.Speaker1 + " " +  nim7.Moderator;
                PdfPCell SpeakerCell = new PdfPCell(new Phrase(speakers, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                SpeakerCell.HorizontalAlignment = 1;
                SpeakerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                SpeakerCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                if (HasBorder == false)
                    SpeakerLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    SpeakerCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SpeakerTable.AddCell(SpeakerLabelCell);
                SpeakerTable.AddCell(SpeakerCell);



                EvaluationDoc.Add(SpeakerTable);
                //end of speaker(s) row

                //RSVP(s) row

                float[] RSVPWidths = new float[] { 1f, 3f };//speaker column is 2 times wider than evaluation column
                PdfPTable RSVPTable = new PdfPTable(2);
                RSVPTable.KeepTogether = true;
                RSVPTable.TotalWidth = 523f;
                RSVPTable.SetWidths(RSVPWidths);
                RSVPTable.LockedWidth = true;
                RSVPTable.SpacingBefore = 10f;


                RSVPTable.SpacingAfter = 10f;
                RSVPTable.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right

                //cell 1
                iTextSharp.text.Image RSVPLabel = iTextSharp.text.Image.GetInstance(imagepath + "/RSVPLabel.jpg");
                RSVPLabel.ScalePercent(50f);
                PdfPCell RSVPLabelCell = new PdfPCell(RSVPLabel);
                //cell 2
                string RSVPContents = nim7.ContactFirstName + " " + nim7.ContactLastName + " " + nim7.ContactEmail + " " + nim7.ContactPhone;
                PdfPCell RSVPContentsCell = new PdfPCell(new Phrase(RSVPContents, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                RSVPContentsCell.HorizontalAlignment = 1;
                RSVPContentsCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                RSVPContentsCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                if (HasBorder == false)
                    RSVPLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    RSVPContentsCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                RSVPTable.AddCell(RSVPLabelCell);
                RSVPTable.AddCell(RSVPContentsCell);



                EvaluationDoc.Add(RSVPTable);
                //end of RSVP row

                //set up image "AGENDA & LEARNING OBJECTIVES
                iTextSharp.text.Image EvalTop2 = iTextSharp.text.Image.GetInstance(imagepath + "/AfterDateLocation.jpg");
                EvalTop2.Alignment = Element.ALIGN_CENTER;

                EvalTop2.ScalePercent(52f);
                EvaluationDoc.Add(EvalTop2);
                //adding image "The following Clinical Questions will be addressed during the session
                iTextSharp.text.Image EvalTop3 = iTextSharp.text.Image.GetInstance(imagepath + "/AfterDateLocation2.jpg");
                EvalTop3.Alignment = Element.ALIGN_CENTER;

                EvalTop3.ScalePercent(52f);
                EvaluationDoc.Add(EvalTop3);

                //setting up learning objectives table
               

                //start of learning objectives3
                if (nim7.ProgramModule1)
                {
                    //module 1
                    iTextSharp.text.Image m1 = iTextSharp.text.Image.GetInstance(imagepath + "/m1.jpg");
                    m1.ScalePercent(50f);
                    m1.Alignment = Element.ALIGN_LEFT;


                    EvaluationDoc.Add(m1);
                    ModuleCounter = ModuleCounter + 1;

                }
                if (nim7.ProgramModule2)
                {
                    //module 2
                    iTextSharp.text.Image m2 = iTextSharp.text.Image.GetInstance(imagepath + "/m2.jpg");
                    m2.ScalePercent(50f);
                    m2.Alignment = Element.ALIGN_LEFT;


                    EvaluationDoc.Add(m2);
                    ModuleCounter = ModuleCounter + 1;

                }
                if (nim7.ProgramModule3)
                {
                    //module 3
                    iTextSharp.text.Image m3 = iTextSharp.text.Image.GetInstance(imagepath + "/m3.jpg");
                    m3.ScalePercent(50f);
                    m3.Alignment = Element.ALIGN_LEFT;


                    EvaluationDoc.Add(m3);
                    ModuleCounter = ModuleCounter + 1;

                }

                if (ModuleCounter > 5)

                {
                    EvaluationDoc.NewPage();
                    ModuleCounter = 0;

                }
                if (nim7.ProgramModule4)
                {
                    //module 4
                    iTextSharp.text.Image m4 = iTextSharp.text.Image.GetInstance(imagepath + "/m4.jpg");
                    m4.ScalePercent(50f);
                    m4.Alignment = Element.ALIGN_LEFT;


                    EvaluationDoc.Add(m4);
                    ModuleCounter = ModuleCounter + 1;

                }
                if (ModuleCounter > 5)

                {
                    EvaluationDoc.NewPage();
                    ModuleCounter = 0;

                }
                if (nim7.ProgramModule5)
                {
                    //module 5
                    iTextSharp.text.Image m5 = iTextSharp.text.Image.GetInstance(imagepath + "/m5.jpg");
                    m5.ScalePercent(50f);
                    m5.Alignment = Element.ALIGN_LEFT;


                    EvaluationDoc.Add(m5);
                    ModuleCounter = ModuleCounter + 1;

                }
                if (ModuleCounter > 5)

                {
                    EvaluationDoc.NewPage();
                    ModuleCounter = 0;

                }
                if (nim7.ProgramModule6)
                {
                    //module 6
                    iTextSharp.text.Image m6 = iTextSharp.text.Image.GetInstance(imagepath + "/m6.jpg");
                    m6.ScalePercent(50f);
                    m6.Alignment = Element.ALIGN_LEFT;


                    EvaluationDoc.Add(m6);
                    ModuleCounter = ModuleCounter + 1;

                }
                if (ModuleCounter > 5)

                {
                    EvaluationDoc.NewPage();
                    ModuleCounter = 0;

                }
                if (nim7.ProgramModule7)
                {
                    //module 7
                    iTextSharp.text.Image m7 = iTextSharp.text.Image.GetInstance(imagepath + "/m7.jpg");
                    m7.ScalePercent(50f);
                    m7.Alignment = Element.ALIGN_LEFT;
                    EvaluationDoc.Add(m7);
                    ModuleCounter = ModuleCounter + 1;

                }
                if (ModuleCounter >5)

                {
                    EvaluationDoc.NewPage();
                    ModuleCounter = 0;

                }
                if (nim7.ProgramModule8)
                {
                    //module 8
                    iTextSharp.text.Image m8 = iTextSharp.text.Image.GetInstance(imagepath + "/m8.jpg");
                    m8.ScalePercent(50f);
                    m8.Alignment = Element.ALIGN_LEFT;


                    EvaluationDoc.Add(m8);
                    ModuleCounter = ModuleCounter + 1;

                }
                if (ModuleCounter > 5)

                {
                    EvaluationDoc.NewPage();
                    ModuleCounter = 0;

                }
                if (nim7.ProgramModule9)
                {
                    //module 9
                    iTextSharp.text.Image m9 = iTextSharp.text.Image.GetInstance(imagepath + "/m9.jpg");
                    m9.ScalePercent(50f);
                    m9.Alignment = Element.ALIGN_LEFT;


                    EvaluationDoc.Add(m9);
                    ModuleCounter = ModuleCounter + 1;

                }
                if (ModuleCounter > 5)

                {
                    EvaluationDoc.NewPage();
                    ModuleCounter = 0;

                }
                if (nim7.ProgramModule10)
                {
                    //module 10
                    iTextSharp.text.Image m10 = iTextSharp.text.Image.GetInstance(imagepath + "/m10.jpg");
                    m10.ScalePercent(50f);
                    m10.Alignment = Element.ALIGN_LEFT;


                    EvaluationDoc.Add(m10);
                    ModuleCounter = ModuleCounter + 1;

                }
              
                //end of learning objectives table

              


                //RSVP(s) row

                float[] Footerwidths = new float[] { 4f, 1f };//equal width for now
                PdfPTable FooterTable = new PdfPTable(2);
                FooterTable.KeepTogether = true;
               
                FooterTable.TotalWidth = 628f;
                FooterTable.WidthPercentage = 100f;
                FooterTable.SetWidths(Footerwidths);
                FooterTable.LockedWidth = true;
                FooterTable.SpacingBefore = 50f;
                FooterTable.SpacingAfter = 10f;
                var chunk1 = new Chunk("\n    This Group Learning program has been reviewed by the College of Family Physicians of Canada and is awaiting \n final certification by the College's " + nim7.ProvinceName + " Chapter \n ", new iTextSharp.text.Font(HelveticaBase, 9f, iTextSharp.text.Font.BOLD, BaseColor.WHITE));
                var chunk2 = new Chunk("\n This program has received an educational grant and in-kind support from Amgen Canada\n ", new iTextSharp.text.Font(HelveticaBase, 9f, iTextSharp.text.Font.NORMAL, BaseColor.WHITE));
                var phase1 = new Phrase();
                phase1.Add(chunk1);
                phase1.Add(chunk2);
                PdfPCell Footer1Cell = new PdfPCell(phase1);
               // PdfPCell Footer1Cell = new PdfPCell(new Phrase("This Group Learning program has been reviewed by the College of Family Physicians of Canada and is awaiting final certification by the College's " , new iTextSharp.Font(HelveticaBase,9f,iTextSharp.text.Font.BOLD,BaseColor.WHITE));
                Footer1Cell.BackgroundColor = new iTextSharp.text.BaseColor(119, 114, 114);
                Footer1Cell.HorizontalAlignment = Element.ALIGN_CENTER;
                Footer1Cell.BorderColor = new iTextSharp.text.BaseColor(119, 114, 114);
                Footer1Cell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                // Footer1Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;

               // PdfPCell Footer2Cell = new PdfPCell(new Phrase("\n This program has received an educational grant and in-kind support from Amgen Canada\n ", new iTextSharp.text.Font(HelveticaBase, 9f, iTextSharp.text.Font.NORMAL, BaseColor.WHITE)));
               ////PdfPCell Footer2Cell = new PdfPCell(new Phrase("\n This program has received an educational grant and in-kind support from Amgen Canada"));
               // Footer2Cell.BackgroundColor = new iTextSharp.text.BaseColor(119, 114, 114);
               // Footer2Cell.HorizontalAlignment = Element.ALIGN_CENTER;
               // Footer2Cell.BorderColor = new iTextSharp.text.BaseColor(119, 114, 114);
               // Footer2Cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;


                //PdfPCell CHRCLogoCell = new PdfPCell(new Phrase("\n CANADIAN CENTRE FOR\n PROFESSIONAL DEVELOPMENT\n IN HEALTH AND MEDICINE \n A Division of the Canadian Heart Research Centre", new iTextSharp.text.Font(HelveticaBase, 9f, iTextSharp.text.Font.NORMAL, BaseColor.WHITE)));

                //CHRCLogoCell.BackgroundColor = new iTextSharp.text.BaseColor(119, 114, 114);
                //CHRCLogoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //CHRCLogoCell.BorderColor = new iTextSharp.text.BaseColor(119, 114, 114);
                //CHRCLogoCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;

                iTextSharp.text.Image ImgCHRCLogo = iTextSharp.text.Image.GetInstance(imagepath + "/NationalFooterR1.jpg");
                ImgCHRCLogo.ScalePercent(50f);
                ImgCHRCLogo.Alignment = Element.ALIGN_LEFT;
                PdfPCell CHRCLogoCell = new PdfPCell(ImgCHRCLogo);


                //FooterTable.WriteSelectedRows do not work if there is a cell that has rowspan


                //CHRCLogoCell.Rowspan = 2;

                FooterTable.AddCell(Footer1Cell);
                FooterTable.AddCell(CHRCLogoCell);
                FooterTable.DefaultCell.Border = PdfPCell.NO_BORDER;
               // FooterTable.AddCell(Footer2Cell);


                //display all table row 0,-1
                //x y cooridinates 0,65
                FooterTable.WriteSelectedRows(0, -1, 0, 52, writer.DirectContent);

                //EvaluationDoc.Add(FooterTable);

                //end of footer

                //footer
                //iTextSharp.text.Image FooterNational = iTextSharp.text.Image.GetInstance(imagepath + "/FooterNational.jpg");
                //FooterNational.Alignment = Element.ALIGN_CENTER;
                //FooterNational.ScalePercent(52f);
                //EvaluationDoc.Add(FooterNational);
                nim7.DisplayPDF = true;

                fileName = "/PDF/NationalInvitation/NationalInvitation7" + RandomModifer + ".pdf";
                //  return File(new FileStream(Server.MapPath("~/App_Data/" + fileName), FileMode.Open), "application/octetstream", fileName);
                return Redirect(fileName);
            }
            catch (Exception e)
            {
                return Redirect(fileName);
                // return View("ProgramMaterials", efm);
            }
            finally
            {
                EvaluationDoc.Close();
            }
        }
        [HttpGet]
        public ActionResult GetRegionalInvitationPdf7(int ProgramRequestID)
        {
            //get the evaluation form for the new program
            int ModuleCounter = 0;
            string fileName = string.Empty; //contains the file name of the resultant pdf
            bool HasBorder = false;  //set to false when development is completed. 
            System.DateTime ThisMoment = System.DateTime.Now;
            String RandomModifer = ThisMoment.Year + "_" + ThisMoment.Month + "_" + ThisMoment.Day + "_" + ThisMoment.Hour + "_" + ThisMoment.Minute + "_" + ThisMoment.Second + "_" + ThisMoment.Millisecond;
            string path = Server.MapPath("/PDF/RegionalInvitation");
            string imagepath = Server.MapPath("/Images/NationalInvitation7");
            var EvaluationDoc = new Document(PageSize.A4, 18, 18, 18, 18);
            //ViewBag.ProgramRequestStatusCounts = UserHelper.GetProgramRequestStatusCounts(Convert.ToInt32(Session["ProgramID"]));
            PdfWriter writer = PdfWriter.GetInstance(EvaluationDoc, new FileStream(path + "/RegionalInvitation7" + RandomModifer + ".pdf", FileMode.Create));
            BaseFont HelveticaBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            BaseFont HelveticaBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, false);


            DashboardRepository DBRepo = new DashboardRepository();
            NationalInvitation7Model nim7 = DBRepo.GetNationalInvitation7Model(ProgramRequestID);
            iTextSharp.text.Font Helvetica = new iTextSharp.text.Font(HelveticaBase, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


            try
            {
                EvaluationDoc.Open();
                EvaluationDoc.NewPage();
                iTextSharp.text.Image EvalTop1 = iTextSharp.text.Image.GetInstance(imagepath + "/MainHeader.jpg");

                EvalTop1.ScalePercent(54f);
                EvaluationDoc.Add(EvalTop1);
                

                float[] widths = new float[] { 0.7f, 1f, 0.7f, 2f };
                PdfPTable DateLocation = new PdfPTable(4);
                DateLocation.KeepTogether = true;
                DateLocation.TotalWidth = 523f;
                DateLocation.SetWidths(widths);
                DateLocation.LockedWidth = true;
                DateLocation.SpacingBefore = 10f;


                DateLocation.SpacingAfter = 10f;
                DateLocation.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right

                //cell 1
                iTextSharp.text.Image DateLabel = iTextSharp.text.Image.GetInstance(imagepath + "/DateLabel.jpg");
                DateLabel.ScalePercent(50f);
                PdfPCell DateLabelCell = new PdfPCell(DateLabel);
                //cell 2
                PdfPCell DateCell = new PdfPCell(new Phrase(nim7.ProgramDate, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));


                DateCell.HorizontalAlignment = 1;
                DateCell.VerticalAlignment = Element.ALIGN_TOP;
                DateCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                //cell 3
                iTextSharp.text.Image LocationLabel = iTextSharp.text.Image.GetInstance(imagepath + "/LocationLabel.jpg");
                LocationLabel.ScalePercent(50f);
                PdfPCell LocationLabelCell = new PdfPCell(LocationLabel);
                LocationLabelCell.HorizontalAlignment = 0; //alignleft
                                                           //cell 4
                                                           //iTextSharp.text.Image SpaceFieldLoc = iTextSharp.text.Image.GetInstance(imagepath + "/SpaceField.jpg");
                                                           //DateLabel.ScalePercent(30f);
                                                           //PdfPCell SpaceFieldCellLocation = new PdfPCell(SpaceFieldLoc);

                PdfPCell LocationCell = new PdfPCell(new Phrase(nim7.ProgramLocation, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                LocationCell.HorizontalAlignment = 1;
                LocationCell.VerticalAlignment = Element.ALIGN_TOP;
                LocationCell.BackgroundColor = new BaseColor(255, 255, 255);

                if (HasBorder == false)
                    DateLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    DateCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    LocationLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    LocationCell.Border = iTextSharp.text.Rectangle.NO_BORDER;


                DateLocation.AddCell(DateLabelCell);
                DateLocation.AddCell(DateCell);
                DateLocation.AddCell(LocationLabelCell);
                DateLocation.AddCell(LocationCell);



                EvaluationDoc.Add(DateLocation);


                //end of set up program date/location

                //set up Program Start Time /End Time
                float[] ProgramWidths = new float[] { 1f, 1f, 1f, 1f };//speaker column is 2 times wider than evaluation column
                PdfPTable ProgramTime = new PdfPTable(4);
                ProgramTime.KeepTogether = true;
                ProgramTime.TotalWidth = 523f;
                ProgramTime.SetWidths(ProgramWidths);
                ProgramTime.LockedWidth = true;
                ProgramTime.SpacingBefore = 10f;


                ProgramTime.SpacingAfter = 10f;
                ProgramTime.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right

                //cell 1
                iTextSharp.text.Image ProgramStartLabel = iTextSharp.text.Image.GetInstance(imagepath + "/ProgramStartTime.jpg");
                ProgramStartLabel.ScalePercent(50f);
                PdfPCell ProgramStartLabelCell = new PdfPCell(ProgramStartLabel);
                //cell 2
                PdfPCell ProgramStartCell = new PdfPCell(new Phrase(nim7.ProgramStartTime, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                ProgramStartCell.HorizontalAlignment = 1;
                ProgramStartCell.VerticalAlignment = Element.ALIGN_TOP;
                ProgramStartCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                //cell 3
                iTextSharp.text.Image ProgramEndLabel = iTextSharp.text.Image.GetInstance(imagepath + "/ProgramEndTime.jpg");
                ProgramEndLabel.ScalePercent(50f);
                PdfPCell ProgramEndLabelCell = new PdfPCell(ProgramEndLabel);

                //cell 4
                //iTextSharp.text.Image SpaceFieldLoc = iTextSharp.text.Image.GetInstance(imagepath + "/SpaceField.jpg");
                //DateLabel.ScalePercent(30f);
                //PdfPCell SpaceFieldCellLocation = new PdfPCell(SpaceFieldLoc);

                PdfPCell ProgramEndCell = new PdfPCell(new Phrase(nim7.ProgramEndTime, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                ProgramEndCell.HorizontalAlignment = 1;
                ProgramEndCell.VerticalAlignment = Element.ALIGN_TOP;
                ProgramEndCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);

                if (HasBorder == false)
                    ProgramStartLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    ProgramStartCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    ProgramEndLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    ProgramEndCell.Border = iTextSharp.text.Rectangle.NO_BORDER;


                ProgramTime.AddCell(ProgramStartLabelCell);
                ProgramTime.AddCell(ProgramStartCell);
                ProgramTime.AddCell(ProgramEndLabelCell);
                ProgramTime.AddCell(ProgramEndCell);



                EvaluationDoc.Add(ProgramTime);

                //end of program start time/end time

                //speaker(s) row

                float[] SpeakerWidths = new float[] { 1f, 3f };//speaker column is 2 times wider than evaluation column
                PdfPTable SpeakerTable = new PdfPTable(2);
                SpeakerTable.KeepTogether = true;
                SpeakerTable.TotalWidth = 523f;
                SpeakerTable.SetWidths(SpeakerWidths);
                SpeakerTable.LockedWidth = true;
                SpeakerTable.SpacingBefore = 10f;


                SpeakerTable.SpacingAfter = 10f;
                SpeakerTable.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right

                //cell 1
                iTextSharp.text.Image SpeakerLabel = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerLabel.jpg");
                SpeakerLabel.ScalePercent(50f);
                PdfPCell SpeakerLabelCell = new PdfPCell(SpeakerLabel);
                //cell 2
                string speakers = nim7.Speaker1 + " " + nim7.Moderator;
                PdfPCell SpeakerCell = new PdfPCell(new Phrase(speakers, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                SpeakerCell.HorizontalAlignment = 1;
                SpeakerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                SpeakerCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                if (HasBorder == false)
                    SpeakerLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    SpeakerCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                SpeakerTable.AddCell(SpeakerLabelCell);
                SpeakerTable.AddCell(SpeakerCell);



                EvaluationDoc.Add(SpeakerTable);
                //end of speaker(s) row

                //RSVP(s) row

                float[] RSVPWidths = new float[] { 1f, 3f };//speaker column is 2 times wider than evaluation column
                PdfPTable RSVPTable = new PdfPTable(2);
                RSVPTable.KeepTogether = true;
                RSVPTable.TotalWidth = 523f;
                RSVPTable.SetWidths(RSVPWidths);
                RSVPTable.LockedWidth = true;
                RSVPTable.SpacingBefore = 10f;


                RSVPTable.SpacingAfter = 10f;
                RSVPTable.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right

                //cell 1
                iTextSharp.text.Image RSVPLabel = iTextSharp.text.Image.GetInstance(imagepath + "/RSVPLabel.jpg");
                RSVPLabel.ScalePercent(50f);
                PdfPCell RSVPLabelCell = new PdfPCell(RSVPLabel);
                //cell 2
                string RSVPContents = nim7.ContactFirstName + " " + nim7.ContactLastName + " " + nim7.ContactEmail + " " + nim7.ContactPhone;
                PdfPCell RSVPContentsCell = new PdfPCell(new Phrase(RSVPContents, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                RSVPContentsCell.HorizontalAlignment = 1;
                RSVPContentsCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                RSVPContentsCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                if (HasBorder == false)
                    RSVPLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    RSVPContentsCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                RSVPTable.AddCell(RSVPLabelCell);
                RSVPTable.AddCell(RSVPContentsCell);



                EvaluationDoc.Add(RSVPTable);
                //end of RSVP row

                //set up image "AGENDA & LEARNING OBJECTIVES
                iTextSharp.text.Image EvalTop2 = iTextSharp.text.Image.GetInstance(imagepath + "/AfterDateLocation.jpg");
                EvalTop2.Alignment = Element.ALIGN_CENTER;

                EvalTop2.ScalePercent(52f);
                EvaluationDoc.Add(EvalTop2);
                //adding image "The following Clinical Questions will be addressed during the session
                iTextSharp.text.Image EvalTop3 = iTextSharp.text.Image.GetInstance(imagepath + "/AfterDateLocation2.jpg");
                EvalTop3.Alignment = Element.ALIGN_CENTER;

                EvalTop3.ScalePercent(52f);
                EvaluationDoc.Add(EvalTop3);

                //setting up learning objectives table


                //start of learning objectives3
                if (nim7.ProgramModule1)
                {
                    //module 1
                    iTextSharp.text.Image m1 = iTextSharp.text.Image.GetInstance(imagepath + "/m1.jpg");
                    m1.ScalePercent(50f);
                    m1.Alignment = Element.ALIGN_LEFT;


                    EvaluationDoc.Add(m1);
                    ModuleCounter = ModuleCounter + 1;

                }
                if (nim7.ProgramModule2)
                {
                    //module 2
                    iTextSharp.text.Image m2 = iTextSharp.text.Image.GetInstance(imagepath + "/m2.jpg");
                    m2.ScalePercent(50f);
                    m2.Alignment = Element.ALIGN_LEFT;


                    EvaluationDoc.Add(m2);
                    ModuleCounter = ModuleCounter + 1;

                }
                if (nim7.ProgramModule3)
                {
                    //module 3
                    iTextSharp.text.Image m3 = iTextSharp.text.Image.GetInstance(imagepath + "/m3.jpg");
                    m3.ScalePercent(50f);
                    m3.Alignment = Element.ALIGN_LEFT;


                    EvaluationDoc.Add(m3);
                    ModuleCounter = ModuleCounter + 1;

                }

                if (ModuleCounter > 5)

                {
                    EvaluationDoc.NewPage();
                    ModuleCounter = 0;

                }
                if (nim7.ProgramModule4)
                {
                    //module 4
                    iTextSharp.text.Image m4 = iTextSharp.text.Image.GetInstance(imagepath + "/m4.jpg");
                    m4.ScalePercent(50f);
                    m4.Alignment = Element.ALIGN_LEFT;


                    EvaluationDoc.Add(m4);
                    ModuleCounter = ModuleCounter + 1;

                }
                if (ModuleCounter > 5)

                {
                    EvaluationDoc.NewPage();
                    ModuleCounter = 0;

                }
                if (nim7.ProgramModule5)
                {
                    //module 5
                    iTextSharp.text.Image m5 = iTextSharp.text.Image.GetInstance(imagepath + "/m5.jpg");
                    m5.ScalePercent(50f);
                    m5.Alignment = Element.ALIGN_LEFT;


                    EvaluationDoc.Add(m5);
                    ModuleCounter = ModuleCounter + 1;

                }
                if (ModuleCounter > 5)

                {
                    EvaluationDoc.NewPage();
                    ModuleCounter = 0;

                }
                if (nim7.ProgramModule6)
                {
                    //module 6
                    iTextSharp.text.Image m6 = iTextSharp.text.Image.GetInstance(imagepath + "/m6.jpg");
                    m6.ScalePercent(50f);
                    m6.Alignment = Element.ALIGN_LEFT;


                    EvaluationDoc.Add(m6);
                    ModuleCounter = ModuleCounter + 1;

                }
                if (ModuleCounter > 5)

                {
                    EvaluationDoc.NewPage();
                    ModuleCounter = 0;

                }
                if (nim7.ProgramModule7)
                {
                    //module 7
                    iTextSharp.text.Image m7 = iTextSharp.text.Image.GetInstance(imagepath + "/m7.jpg");
                    m7.ScalePercent(50f);
                    m7.Alignment = Element.ALIGN_LEFT;
                    EvaluationDoc.Add(m7);
                    ModuleCounter = ModuleCounter + 1;

                }
                if (ModuleCounter > 5)

                {
                    EvaluationDoc.NewPage();
                    ModuleCounter = 0;

                }
                if (nim7.ProgramModule8)
                {
                    //module 8
                    iTextSharp.text.Image m8 = iTextSharp.text.Image.GetInstance(imagepath + "/m8.jpg");
                    m8.ScalePercent(50f);
                    m8.Alignment = Element.ALIGN_LEFT;


                    EvaluationDoc.Add(m8);
                    ModuleCounter = ModuleCounter + 1;

                }
                if (ModuleCounter > 5)

                {
                    EvaluationDoc.NewPage();
                    ModuleCounter = 0;

                }
                if (nim7.ProgramModule9)
                {
                    //module 9
                    iTextSharp.text.Image m9 = iTextSharp.text.Image.GetInstance(imagepath + "/m9.jpg");
                    m9.ScalePercent(50f);
                    m9.Alignment = Element.ALIGN_LEFT;


                    EvaluationDoc.Add(m9);
                    ModuleCounter = ModuleCounter + 1;

                }
                if (ModuleCounter > 5)

                {
                    EvaluationDoc.NewPage();
                    ModuleCounter = 0;

                }
                if (nim7.ProgramModule10)
                {
                    //module 10
                    iTextSharp.text.Image m10 = iTextSharp.text.Image.GetInstance(imagepath + "/m10.jpg");
                    m10.ScalePercent(50f);
                    m10.Alignment = Element.ALIGN_LEFT;


                    EvaluationDoc.Add(m10);
                    ModuleCounter = ModuleCounter + 1;

                }

                //end of learning objectives table




                //RSVP(s) row

                float[] Footerwidths = new float[] { 4f, 1f };//equal width for now
                PdfPTable FooterTable = new PdfPTable(2);
                FooterTable.KeepTogether = true;

                FooterTable.TotalWidth = 628f;
                FooterTable.WidthPercentage = 100f;
                FooterTable.SetWidths(Footerwidths);
                FooterTable.LockedWidth = true;
                FooterTable.SpacingBefore = 50f;
                FooterTable.SpacingAfter = 10f;
                var chunk1 = new Chunk("\n    This one-credit-per-hour Group Learning program has been certifed by the College of Family Physicians of Canada \n and the Chapter for up to " + nim7.SessionCreditValue + " Mainpro+ credits. \n ", new iTextSharp.text.Font(HelveticaBase, 9f, iTextSharp.text.Font.BOLD, BaseColor.WHITE));
                var chunk2 = new Chunk("\n This program has received an educational grant and in-kind support from Amgen Canada\n ", new iTextSharp.text.Font(HelveticaBase, 9f, iTextSharp.text.Font.NORMAL, BaseColor.WHITE));
                var phase1 = new Phrase();
                phase1.Add(chunk1);
                phase1.Add(chunk2);
                PdfPCell Footer1Cell = new PdfPCell(phase1);
                // PdfPCell Footer1Cell = new PdfPCell(new Phrase("This Group Learning program has been reviewed by the College of Family Physicians of Canada and is awaiting final certification by the College's " , new iTextSharp.Font(HelveticaBase,9f,iTextSharp.text.Font.BOLD,BaseColor.WHITE));
                Footer1Cell.BackgroundColor = new iTextSharp.text.BaseColor(119, 114, 114);
                Footer1Cell.HorizontalAlignment = Element.ALIGN_CENTER;
                Footer1Cell.BorderColor = new iTextSharp.text.BaseColor(119, 114, 114);
                Footer1Cell.Border = iTextSharp.text.Rectangle.TOP_BORDER;
                // Footer1Cell.Border = iTextSharp.text.Rectangle.NO_BORDER;

                // PdfPCell Footer2Cell = new PdfPCell(new Phrase("\n This program has received an educational grant and in-kind support from Amgen Canada\n ", new iTextSharp.text.Font(HelveticaBase, 9f, iTextSharp.text.Font.NORMAL, BaseColor.WHITE)));
                ////PdfPCell Footer2Cell = new PdfPCell(new Phrase("\n This program has received an educational grant and in-kind support from Amgen Canada"));
                // Footer2Cell.BackgroundColor = new iTextSharp.text.BaseColor(119, 114, 114);
                // Footer2Cell.HorizontalAlignment = Element.ALIGN_CENTER;
                // Footer2Cell.BorderColor = new iTextSharp.text.BaseColor(119, 114, 114);
                // Footer2Cell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;


                //PdfPCell CHRCLogoCell = new PdfPCell(new Phrase("\n CANADIAN CENTRE FOR\n PROFESSIONAL DEVELOPMENT\n IN HEALTH AND MEDICINE \n A Division of the Canadian Heart Research Centre", new iTextSharp.text.Font(HelveticaBase, 9f, iTextSharp.text.Font.NORMAL, BaseColor.WHITE)));

                //CHRCLogoCell.BackgroundColor = new iTextSharp.text.BaseColor(119, 114, 114);
                //CHRCLogoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                //CHRCLogoCell.BorderColor = new iTextSharp.text.BaseColor(119, 114, 114);
                //CHRCLogoCell.Border = iTextSharp.text.Rectangle.BOTTOM_BORDER;

                iTextSharp.text.Image ImgCHRCLogo = iTextSharp.text.Image.GetInstance(imagepath + "/NationalFooterR1.jpg");
                ImgCHRCLogo.ScalePercent(50f);
                ImgCHRCLogo.Alignment = Element.ALIGN_LEFT;
                PdfPCell CHRCLogoCell = new PdfPCell(ImgCHRCLogo);


                //FooterTable.WriteSelectedRows do not work if there is a cell that has rowspan


                //CHRCLogoCell.Rowspan = 2;

                FooterTable.AddCell(Footer1Cell);
                FooterTable.AddCell(CHRCLogoCell);
                FooterTable.DefaultCell.Border = PdfPCell.NO_BORDER;
                // FooterTable.AddCell(Footer2Cell);


                //display all table row 0,-1
                //x y cooridinates 0,65
                FooterTable.WriteSelectedRows(0, -1, 0, 52, writer.DirectContent);

                //EvaluationDoc.Add(FooterTable);

                //end of footer

                //footer
                //iTextSharp.text.Image FooterNational = iTextSharp.text.Image.GetInstance(imagepath + "/FooterNational.jpg");
                //FooterNational.Alignment = Element.ALIGN_CENTER;
                //FooterNational.ScalePercent(52f);
                //EvaluationDoc.Add(FooterNational);
                nim7.DisplayPDF = true;

                fileName = "/PDF/RegionalInvitation/RegionalInvitation7" + RandomModifer + ".pdf";
                //  return File(new FileStream(Server.MapPath("~/App_Data/" + fileName), FileMode.Open), "application/octetstream", fileName);
                return Redirect(fileName);
            }
            catch (Exception e)
            {
                return Redirect(fileName);
                // return View("ProgramMaterials", efm);
            }
            finally
            {
                EvaluationDoc.Close();
            }
        }



        public ActionResult EvalForm7(int ProgramRequestID)
        {

            string fileName = string.Empty;
            bool HasBorder = false;
            System.DateTime ThisMoment = System.DateTime.Now;
            String RandomModifer = ThisMoment.Year + "_" + ThisMoment.Month + "_" + ThisMoment.Day + "_" + ThisMoment.Hour + "_" + ThisMoment.Minute + "_" + ThisMoment.Second + "_" + ThisMoment.Millisecond;
            string path = Server.MapPath("/PDF/EvalForm7");
            string imagepath = Server.MapPath("/Images/EvalForm7");
            var EvaluationDoc = new Document(PageSize.A4, 18, 18, 18, 18);
            var writer = PdfWriter.GetInstance(EvaluationDoc, new FileStream(path + "/Evaluation" + RandomModifer + ".pdf", FileMode.Create));
            writer.StrictImageSequence = true;
            BaseFont HelveticaBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            DashboardRepository DBRepo = new DashboardRepository();
            EvalForm7Model efm = DBRepo.GetEvalForm7Model(ProgramRequestID);
            iTextSharp.text.Font Helvetica = new iTextSharp.text.Font(HelveticaBase, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


            try
            {
                EvaluationDoc.Open();
                EvaluationDoc.NewPage();
                iTextSharp.text.Image EvalTop1 = iTextSharp.text.Image.GetInstance(imagepath + "/MainHeader.jpg");

                EvalTop1.ScalePercent(54f);
                EvaluationDoc.Add(EvalTop1);


                float[] widths = new float[] { 0.7f, 1f, 0.7f, 2f };
                PdfPTable DateLocation = new PdfPTable(4);
                DateLocation.KeepTogether = true;
                DateLocation.TotalWidth = 523f;
                DateLocation.SetWidths(widths);
                DateLocation.LockedWidth = true;
                DateLocation.SpacingBefore = 10f;


                DateLocation.SpacingAfter = 10f;
                DateLocation.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right

                //cell 1
                iTextSharp.text.Image DateLabel = iTextSharp.text.Image.GetInstance(imagepath + "/DateLabel.jpg");
                DateLabel.ScalePercent(30f);
                PdfPCell DateLabelCell = new PdfPCell(DateLabel);
                //cell 2
                PdfPCell DateCell = new PdfPCell(new Phrase(efm.ProgramDate, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));


                DateCell.HorizontalAlignment = 1;
                DateCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                DateCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                //cell 3
                iTextSharp.text.Image LocationLabel = iTextSharp.text.Image.GetInstance(imagepath + "/LocationLabel.jpg");
                LocationLabel.ScalePercent(30f);
                PdfPCell LocationLabelCell = new PdfPCell(LocationLabel);
                LocationLabelCell.HorizontalAlignment = 0; //alignleft
                                                           //cell 4
                                                           //iTextSharp.text.Image SpaceFieldLoc = iTextSharp.text.Image.GetInstance(imagepath + "/SpaceField.jpg");
                                                           //DateLabel.ScalePercent(30f);
                                                           //PdfPCell SpaceFieldCellLocation = new PdfPCell(SpaceFieldLoc);

                PdfPCell LocationCell = new PdfPCell(new Phrase(efm.ProgramLocation, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                LocationCell.HorizontalAlignment = 1;
                LocationCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                LocationCell.BackgroundColor = new BaseColor(255, 255, 255);

                if (HasBorder == false)
                    DateLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    DateCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    LocationLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    LocationCell.Border = iTextSharp.text.Rectangle.NO_BORDER;


                DateLocation.AddCell(DateLabelCell);
                DateLocation.AddCell(DateCell);
                DateLocation.AddCell(LocationLabelCell);
                DateLocation.AddCell(LocationCell);



                EvaluationDoc.Add(DateLocation);


                iTextSharp.text.Image EvalTop2 = iTextSharp.text.Image.GetInstance(imagepath + "/Demographics.jpg");
                EvalTop2.Alignment = Element.ALIGN_CENTER;

                EvalTop2.ScalePercent(52f);
                EvaluationDoc.Add(EvalTop2);

                EvaluationDoc.Add(new Paragraph("\n"));

                EvaluationDoc.Add(new Paragraph("\n\n\n\n\n\n\n\n\n\n\n\n")); //add space
               
                //speaker
                if (!String.IsNullOrEmpty(efm.Speaker1))
                {
                    float[] SpeakerTableWidths = new float[] { 2f, 8f };
                    PdfPTable SpeakerTable = new PdfPTable(2);
                    SpeakerTable.KeepTogether = true;
                    SpeakerTable.TotalWidth = 523f;
                    SpeakerTable.SetWidths(SpeakerTableWidths);
                    SpeakerTable.LockedWidth = true;
                    SpeakerTable.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right


                    iTextSharp.text.Image speakerTitle = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerTitle.jpg");
                    speakerTitle.ScalePercent(30f);
                    PdfPCell speakerTitleCell = new PdfPCell(speakerTitle);
                    speakerTitleCell.HorizontalAlignment = 0;
                    speakerTitleCell.VerticalAlignment = Element.ALIGN_MIDDLE;


                    PdfPCell SpeakerCell = new PdfPCell(new Phrase(efm.Speaker1, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.DARK_GRAY)));
                    SpeakerCell.HorizontalAlignment = 0;
                    SpeakerCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    if (HasBorder == false)
                    {
                        speakerTitleCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        SpeakerCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    }
                    SpeakerTable.AddCell(speakerTitleCell);
                    SpeakerTable.AddCell(SpeakerCell);
                    EvaluationDoc.Add(SpeakerTable);


                    //speaker form cell
                    iTextSharp.text.Image SpeakerForm = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerForm.jpg");
                    SpeakerForm.ScalePercent(55f);
                    EvaluationDoc.Add(SpeakerForm);
                }


                //moderator
                string[] ModeratorFirstLastName = efm.Moderator.Split(',');
                if (!String.IsNullOrEmpty(ModeratorFirstLastName[0]))
                {
                    float[] SpeakerTableWidths = new float[] { 2f, 8f };
                    PdfPTable SpeakerTable = new PdfPTable(2);
                    SpeakerTable.KeepTogether = true;
                    SpeakerTable.TotalWidth = 523f;
                    SpeakerTable.SetWidths(SpeakerTableWidths);
                    SpeakerTable.LockedWidth = true;
                    SpeakerTable.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right


                    iTextSharp.text.Image speakerTitle = iTextSharp.text.Image.GetInstance(imagepath + "/ModeratorTitle.jpg");
                    speakerTitle.ScalePercent(30f);
                    PdfPCell speakerTitleCell = new PdfPCell(speakerTitle);
                    speakerTitleCell.HorizontalAlignment = 0;
                    speakerTitleCell.VerticalAlignment = Element.ALIGN_MIDDLE;


                    PdfPCell SpeakerCell = new PdfPCell(new Phrase(efm.Moderator, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.DARK_GRAY)));
                    SpeakerCell.HorizontalAlignment = 0;
                    SpeakerCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    if (HasBorder == false)
                    {
                        speakerTitleCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        SpeakerCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    }
                    SpeakerTable.AddCell(speakerTitleCell);
                    SpeakerTable.AddCell(SpeakerCell);
                    EvaluationDoc.Add(SpeakerTable);


                    iTextSharp.text.Image SpeakerForm = iTextSharp.text.Image.GetInstance(imagepath + "/ModeratorForm.jpg");
                    SpeakerForm.ScalePercent(55f);
                    EvaluationDoc.Add(SpeakerForm);
                }


                EvaluationDoc.NewPage();


                iTextSharp.text.Image LastPage = iTextSharp.text.Image.GetInstance(imagepath + "/LastPage.jpg");
                LastPage.Alignment = Element.ALIGN_CENTER;
                LastPage.ScalePercent(58f);
                EvaluationDoc.Add(LastPage);
                efm.DisplayPDF = true;

                fileName = "/PDF/EvalForm7/Evaluation" + RandomModifer + ".pdf";

                return Redirect(fileName);
            }
            catch (Exception e)
            {
                return Redirect(fileName);
            }
            finally
            {
                EvaluationDoc.Close();
            }
        }

        public ActionResult EvalForm8(int ProgramRequestID)
        {

            string fileName = string.Empty;
            bool HasBorder = false;
            System.DateTime ThisMoment = System.DateTime.Now;
            String RandomModifer = ThisMoment.Year + "_" + ThisMoment.Month + "_" + ThisMoment.Day + "_" + ThisMoment.Hour + "_" + ThisMoment.Minute + "_" + ThisMoment.Second + "_" + ThisMoment.Millisecond;
            string path = Server.MapPath("/PDF/EvalForm8");
            string imagepath = Server.MapPath("/Images/EvalForm8");
            var EvaluationDoc = new Document(PageSize.A4, 18, 18, 18, 18);
            var writer = PdfWriter.GetInstance(EvaluationDoc, new FileStream(path + "/Evaluation" + RandomModifer + ".pdf", FileMode.Create));
            writer.StrictImageSequence = true;
            BaseFont HelveticaBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);
            DashboardRepository DBRepo = new DashboardRepository();
            EvalForm8Model efm = DBRepo.GetEvalForm8Model(ProgramRequestID);
            iTextSharp.text.Font Helvetica = new iTextSharp.text.Font(HelveticaBase, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);


            try
            {
                EvaluationDoc.Open();
                EvaluationDoc.NewPage();
                iTextSharp.text.Image EvalTop1 = iTextSharp.text.Image.GetInstance(imagepath + "/MainHeader.jpg");

                EvalTop1.ScalePercent(58f);
                EvaluationDoc.Add(EvalTop1);


                float[] widths = new float[] { 0.4f, 1f, 0.7f, 2.3f };
                PdfPTable DateLocation = new PdfPTable(4);
                DateLocation.KeepTogether = true;
                DateLocation.TotalWidth = 525f;
                DateLocation.SetWidths(widths);
                DateLocation.LockedWidth = true;
                DateLocation.SpacingBefore = 10f;


                DateLocation.SpacingAfter = 0f;
                DateLocation.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right

                //cell 1
                iTextSharp.text.Image DateLabel = iTextSharp.text.Image.GetInstance(imagepath + "/DateLabel.jpg");
                DateLabel.ScalePercent(30f);
                PdfPCell DateLabelCell = new PdfPCell(DateLabel);
                //cell 2
                PdfPCell DateCell = new PdfPCell(new Phrase(efm.ProgramDate, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));


                DateCell.HorizontalAlignment = 0;
                DateCell.VerticalAlignment = Element.ALIGN_TOP;
                DateCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                //cell 3
                iTextSharp.text.Image LocationLabel = iTextSharp.text.Image.GetInstance(imagepath + "/LocationLabel.jpg");
                LocationLabel.ScalePercent(30f);
                PdfPCell LocationLabelCell = new PdfPCell(LocationLabel);
                LocationLabelCell.HorizontalAlignment = 0;

                PdfPCell LocationCell = new PdfPCell(new Phrase(efm.ProgramLocation, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                LocationCell.HorizontalAlignment = 0;
                LocationCell.VerticalAlignment = Element.ALIGN_TOP;
                LocationCell.BackgroundColor = new BaseColor(255, 255, 255);

                if (HasBorder == false)
                    DateLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    DateCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    LocationLabelCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                if (HasBorder == false)
                    LocationCell.Border = iTextSharp.text.Rectangle.NO_BORDER;


                DateLocation.AddCell(DateLabelCell);
                DateLocation.AddCell(DateCell);
                DateLocation.AddCell(LocationLabelCell);
                DateLocation.AddCell(LocationCell);



                EvaluationDoc.Add(DateLocation);


                iTextSharp.text.Image EvalTop2 = iTextSharp.text.Image.GetInstance(imagepath + "/AfterDateLocation.jpg");
                EvalTop2.Alignment = Element.ALIGN_CENTER;

                EvalTop2.ScalePercent(58f);
                EvaluationDoc.Add(EvalTop2);

               // EvaluationDoc.Add(new Paragraph("\n"));

                //Modules
                ProgramRepository repo = new ProgramRepository();
               
              //  EvaluationDoc.Add(new Paragraph("\n"));


                //speaker
                if (!String.IsNullOrEmpty(efm.Speaker1))
                {
                    iTextSharp.text.Image SpeakerTitle = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerTitle.jpg");

                    SpeakerTitle.Alignment = Element.ALIGN_CENTER;

                    SpeakerTitle.ScalePercent(58f);
                    EvaluationDoc.Add(SpeakerTitle);

                    float[] SpeakerTableWidths = new float[] { 10f };
                    PdfPTable SpeakerTable = new PdfPTable(1);
                    SpeakerTable.KeepTogether = true;
                    SpeakerTable.TotalWidth = 523f;
                    SpeakerTable.SetWidths(SpeakerTableWidths);
                    SpeakerTable.LockedWidth = true;
                    SpeakerTable.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right
                    PdfPCell SpeakerCell = new PdfPCell(new Phrase(efm.Speaker1, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.DARK_GRAY)));
                    SpeakerCell.HorizontalAlignment = 0;
                    SpeakerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    SpeakerCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    SpeakerTable.AddCell(SpeakerCell);
                    EvaluationDoc.Add(SpeakerTable);


                    iTextSharp.text.Image SpeakerForm = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerForm.jpg");
                    SpeakerForm.ScalePercent(58f);
                    EvaluationDoc.Add(SpeakerForm);
                    /*
                    float[] SpeakerTableWidths = new float[] { 2f, 8f };
                    PdfPTable SpeakerTable = new PdfPTable(2);
                    SpeakerTable.KeepTogether = true;
                    SpeakerTable.TotalWidth = 523f;
                    SpeakerTable.SetWidths(SpeakerTableWidths);
                    SpeakerTable.LockedWidth = true;
                    SpeakerTable.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right


                    iTextSharp.text.Image speakerTitle = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerTitle.jpg");
                    speakerTitle.ScalePercent(30f);
                    PdfPCell speakerTitleCell = new PdfPCell(speakerTitle);
                    speakerTitleCell.HorizontalAlignment = 0;
                    speakerTitleCell.VerticalAlignment = Element.ALIGN_MIDDLE;


                    PdfPCell SpeakerCell = new PdfPCell(new Phrase(efm.Speaker1, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.DARK_GRAY)));
                    SpeakerCell.HorizontalAlignment = 0;
                    SpeakerCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    if (HasBorder == false)
                    {
                        speakerTitleCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                        SpeakerCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    }
                    SpeakerTable.AddCell(speakerTitleCell);
                    SpeakerTable.AddCell(SpeakerCell);
                    EvaluationDoc.Add(SpeakerTable);


                    //speaker form cell
                    iTextSharp.text.Image SpeakerForm = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerForm.jpg");
                    SpeakerForm.ScalePercent(55f);
                    EvaluationDoc.Add(SpeakerForm);
                    */
                }


                //moderator
                string[] ModeratorFirstLastName = efm.Moderator.Split(',');
                if (!String.IsNullOrEmpty(ModeratorFirstLastName[0]))
                {
                    float[] SpeakerTableWidths = new float[] { 10f };
                    PdfPTable SpeakerTable = new PdfPTable(1);
                    SpeakerTable.KeepTogether = true;
                    SpeakerTable.TotalWidth = 523f;
                    SpeakerTable.SetWidths(SpeakerTableWidths);
                    SpeakerTable.LockedWidth = true;
                    SpeakerTable.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right

                    iTextSharp.text.Image SpeakerTitle = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerTitle.jpg");

                    SpeakerTitle.Alignment = Element.ALIGN_CENTER;

                    SpeakerTitle.ScalePercent(58f);
                    EvaluationDoc.Add(SpeakerTitle);

                    PdfPCell SpeakerCell = new PdfPCell(new Phrase(efm.Moderator, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.DARK_GRAY)));
                    SpeakerCell.HorizontalAlignment = 0;
                    SpeakerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    SpeakerCell.Border = iTextSharp.text.Rectangle.NO_BORDER;
                    SpeakerTable.AddCell(SpeakerCell);
                    EvaluationDoc.Add(SpeakerTable);


                    iTextSharp.text.Image SpeakerForm = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerForm.jpg");
                    SpeakerForm.ScalePercent(58f);
                    EvaluationDoc.Add(SpeakerForm);

                   

                  

                  
                   
                  


                  
                    iTextSharp.text.Image LastPage = iTextSharp.text.Image.GetInstance(imagepath + "/LastPage.jpg");
                    LastPage.Alignment = Element.ALIGN_CENTER;
                    LastPage.ScalePercent(58f);
                    EvaluationDoc.Add(LastPage);
                    efm.DisplayPDF = true;
                }
                else
                {

                    EvaluationDoc.NewPage();


                    iTextSharp.text.Image LastPage = iTextSharp.text.Image.GetInstance(imagepath + "/LastPage.jpg");
                    LastPage.Alignment = Element.ALIGN_CENTER;
                    LastPage.ScalePercent(58f);
                    EvaluationDoc.Add(LastPage);
                    efm.DisplayPDF = true;
                }
                fileName = "/PDF/EvalForm8/Evaluation" + RandomModifer + ".pdf";

                return Redirect(fileName);
            }
            catch (Exception e)
            {
                return Redirect(fileName);
            }
            finally
            {
                EvaluationDoc.Close();
            }
        }
    }
}
