
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


using CPDPortalMVC.App_Data;


namespace CPDPortalMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        public ActionResult Index(bool showPopup = false, bool LoginPopup = false)
        {
            UserRepository ur = new UserRepository();
            List<TherapeuticArea> liTherapeuticArea;

            var CurrentUser = UserHelper.GetLoggedInUser();
            liTherapeuticArea = ur.GetPrivilege(CurrentUser.PrivilegeID);

            //if (UserHelper.IsInRole(Util.Constants.HeadOffice + "," + Util.Constants.RegionalManager))
            //    {
            //        ViewBag.Message = "you are headoffice";

            //    }
            // ViewBag.ImageUrl = UserHelper.GetCPDLogo(Request);
            ViewBag.ShowPopup = showPopup;
            ViewBag.LoginPopup = LoginPopup;
            //ViewBag.loginPopUp = loginPopUp;
            return View(liTherapeuticArea);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult ImplementationDocument()
        {


            var CurrentUser = UserHelper.GetLoggedInUser();
            if (Session["ProgramID"] != null)
            {

                if (CurrentUser != null)
                {


                    ViewBag.UserID = UserHelper.GetLoggedInUser().UserID;

                    ViewBag.ProgramRequestStatusCounts = UserHelper.GetProgramRequestStatusCounts(Convert.ToInt32(Session["ProgramID"]));

                    return View();
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

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ScientificPlanningCommittee()
        {

            var CurrentUser = UserHelper.GetLoggedInUser();
            if (Session["ProgramID"] != null)
            {

                if (CurrentUser != null)
                {


                    ViewBag.UserID = UserHelper.GetLoggedInUser().UserID;

                    ViewBag.ProgramRequestStatusCounts = UserHelper.GetProgramRequestStatusCounts(Convert.ToInt32(Session["ProgramID"]));

                    return View("ScientificCommittee_" + Session["ProgramID"]);
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

        public ActionResult ProgramMaterials()
        {

            if (Session["ProgramID"] != null)
            {
                ViewBag.UserID = UserHelper.GetLoggedInUser().UserID;

                ViewBag.ProgramRequestStatusCounts = UserHelper.GetProgramRequestStatusCounts(Convert.ToInt32(Session["ProgramID"]));
                EvalFormModel efm = new EvalFormModel();
                EvalForm2Model efm2 = new EvalForm2Model();
                EvalForm3Model efm3 = new EvalForm3Model();
                EvalForm5Model efm5 = new EvalForm5Model();
                
                //get all information about this event

                efm.DisplayPDF = false;
                efm2.DisplayPDF = false;
                efm3.DisplayPDF = false;
                efm5.DisplayPDF = false;
                if (Convert.ToInt16(Session["ProgramID"]) == 1)
                    return View("ProgramMaterials_" + Session["ProgramID"], efm);
                else if (Convert.ToInt16(Session["ProgramID"]) == 2)
                    return View("ProgramMaterials_" + Session["ProgramID"], efm2);
                else if (Convert.ToInt16(Session["ProgramID"]) == 3)
                    return View("ProgramMaterials_" + Session["ProgramID"], efm3);
                else if (Convert.ToInt16(Session["ProgramID"]) == 5)
                    return View("ProgramMaterials_" + Session["ProgramID"], efm5);
                else if (Convert.ToInt16(Session["ProgramID"]) == 7)
                    return View("ProgramMaterials_" + Session["ProgramID"], null);
                else if (Convert.ToInt16(Session["ProgramID"]) == 8)
                    return View("ProgramMaterials_" + Session["ProgramID"], null);
                else if (Convert.ToInt16(Session["ProgramID"]) == 9)
                    return View("ProgramMaterials_" + Session["ProgramID"], null);
                else
                    return RedirectToAction("Index", "Home");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }


        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            UserRepository ur = new UserRepository();

            var model = ur.GetAllUsers();
            return PartialView("_GridViewPartial", model);
        }
        [HttpPost]
        public ActionResult EvalForm(EvalFormModel efm)
        {

            bool HasBorder = false;  //set to false when development is completed. 
            System.DateTime ThisMoment = System.DateTime.Now;
            String RandomModifer = ThisMoment.Year + "_" + ThisMoment.Month + "_" + ThisMoment.Day + "_" + ThisMoment.Hour + "_" + ThisMoment.Minute + "_" + ThisMoment.Second + "_" + ThisMoment.Millisecond;
            string path = Server.MapPath("/PDF/EvalForm");
            string imagepath = Server.MapPath("/Images/EvalForm");
            var EvaluationDoc = new Document(PageSize.A4, 18, 18, 18, 18);
            ViewBag.ProgramRequestStatusCounts = UserHelper.GetProgramRequestStatusCounts(Convert.ToInt32(Session["ProgramID"]));
            PdfWriter.GetInstance(EvaluationDoc, new FileStream(path + "/Evaluation" + RandomModifer + ".pdf", FileMode.Create));
            BaseFont HelveticaBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

            Font Helvetica = new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

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
                DateLabel.ScalePercent(30f);
                PdfPCell DateLabelCell = new PdfPCell(DateLabel);
                //cell 2
                //iTextSharp.text.Image SpaceFieldDate = iTextSharp.text.Image.GetInstance(imagepath + "/SpaceField.jpg");
                //DateLabel.ScalePercent(30f);
                //PdfPCell SpaceFieldCellDate = new PdfPCell(SpaceFieldDate);
                PdfPCell DateCell = new PdfPCell(new Phrase(efm.ProgramDate, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                DateCell.HorizontalAlignment = 1;
                DateCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                DateCell.BackgroundColor = new iTextSharp.text.BaseColor(202, 204, 206);
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
                LocationCell.BackgroundColor = new iTextSharp.text.BaseColor(202, 204, 206);

                if (HasBorder == false)
                    DateLabelCell.Border = Rectangle.NO_BORDER;
                if (HasBorder == false)
                    DateCell.Border = Rectangle.NO_BORDER;
                if (HasBorder == false)
                    LocationLabelCell.Border = Rectangle.NO_BORDER;
                if (HasBorder == false)
                    LocationCell.Border = Rectangle.NO_BORDER;


                DateLocation.AddCell(DateLabelCell);
                DateLocation.AddCell(DateCell);
                DateLocation.AddCell(LocationLabelCell);
                DateLocation.AddCell(LocationCell);



                EvaluationDoc.Add(DateLocation);


                //end of set up program date/location

                //set up top image

                iTextSharp.text.Image EvalTop2 = iTextSharp.text.Image.GetInstance(imagepath + "/Demographics.jpg");
                EvalTop2.Alignment = Element.ALIGN_CENTER;

                EvalTop2.ScalePercent(52f);
                EvaluationDoc.Add(EvalTop2);
                //end of set up top image

                //beginning of cases
                if (efm.CaseNewHorizons)
                {
                    iTextSharp.text.Image EvalAF = iTextSharp.text.Image.GetInstance(imagepath + "/CaseNewHorizons.jpg");
                    EvalAF.Alignment = Element.ALIGN_CENTER;
                    EvalAF.ScalePercent(53f);
                    EvaluationDoc.Add(EvalAF);

                }
                if (efm.Case1)
                {
                    iTextSharp.text.Image EvalCase1 = iTextSharp.text.Image.GetInstance(imagepath + "/Case1.jpg");
                    EvalCase1.Alignment = Element.ALIGN_CENTER;
                    EvalCase1.ScalePercent(53f);
                    EvaluationDoc.Add(EvalCase1);
                    EvaluationDoc.NewPage();
                }

                if (efm.Case2)
                {
                    iTextSharp.text.Image EvalCase2 = iTextSharp.text.Image.GetInstance(imagepath + "/Case2.jpg");
                    EvalCase2.ScalePercent(50f);
                    EvaluationDoc.Add(EvalCase2);
                }

                if (efm.Case3)
                {
                    iTextSharp.text.Image EvalCase3 = iTextSharp.text.Image.GetInstance(imagepath + "/Case3.jpg");
                    EvalCase3.ScalePercent(50f);
                    EvaluationDoc.Add(EvalCase3);
                }

                if (efm.Case4)
                {
                    iTextSharp.text.Image EvalCase4 = iTextSharp.text.Image.GetInstance(imagepath + "/Case4.jpg");
                    EvalCase4.ScalePercent(50f);
                    EvaluationDoc.Add(EvalCase4);
                }


                //end of setting up cases
                EvaluationDoc.NewPage();
                //setting up speaker evaluation

                if (!String.IsNullOrEmpty(efm.Speaker1))
                {
                    float[] SpeakerTableWidths = new float[] { 1f, 3f };//speaker column is 2 times wider than evaluation column
                    PdfPTable SpeakerTable = new PdfPTable(2);
                    SpeakerTable.KeepTogether = true;
                    SpeakerTable.TotalWidth = 523f;
                    SpeakerTable.SetWidths(SpeakerTableWidths);
                    SpeakerTable.LockedWidth = true;
                    SpeakerTable.SpacingBefore = 10f;

                    SpeakerTable.SpacingAfter = 10f;
                    SpeakerTable.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right

                    //cell 1
                    iTextSharp.text.Image SpeakerLabel = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerLabel.jpg");
                    SpeakerLabel.ScalePercent(30f);
                    PdfPCell SpeakerLabelCell = new PdfPCell(SpeakerLabel);
                    //cell 2
                    PdfPCell SpeakerCell = new PdfPCell(new Phrase(efm.Speaker1, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    SpeakerCell.HorizontalAlignment = 0;
                    SpeakerCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    SpeakerCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    if (HasBorder == false)
                        SpeakerLabelCell.Border = Rectangle.NO_BORDER;
                    if (HasBorder == false)
                        SpeakerCell.Border = Rectangle.NO_BORDER;

                    SpeakerTable.AddCell(SpeakerLabelCell);
                    SpeakerTable.AddCell(SpeakerCell);
                    EvaluationDoc.Add(SpeakerTable);

                    iTextSharp.text.Image SpeakerEval1 = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerEval1.jpg");

                    SpeakerEval1.Alignment = Element.ALIGN_CENTER;
                    SpeakerEval1.ScalePercent(52f);
                    EvaluationDoc.Add(SpeakerEval1);
                    iTextSharp.text.Image SpeakerEval2 = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerEval2.jpg");

                    SpeakerEval2.Alignment = Element.ALIGN_CENTER;
                    SpeakerEval2.ScalePercent(52f);
                    EvaluationDoc.Add(SpeakerEval2);

                }
               

                //setting up last page
                iTextSharp.text.Image LastPage = iTextSharp.text.Image.GetInstance(imagepath + "/LastPage.jpg");
                LastPage.Alignment = Element.ALIGN_CENTER;
                LastPage.ScalePercent(52f);
                EvaluationDoc.Add(LastPage);

                efm.DisplayPDF = true;

                efm.FileName = "/PDF/EvalForm/Evaluation" + RandomModifer + ".pdf";
                return View("ProgramMaterials_1", efm);
            }
            catch (Exception e)
            {
                efm.DisplayPDF = false;
                return View("ProgramMaterials_1", efm);
            }
            finally
            {
                EvaluationDoc.Close();
            }
        }
        [HttpPost]
        public ActionResult EvalForm_FR(EvalFormModel efm)
        {

            bool HasBorder = false;  //set to false when development is completed. 
            System.DateTime ThisMoment = System.DateTime.Now;
            String RandomModifer = ThisMoment.Year + "_" + ThisMoment.Month + "_" + ThisMoment.Day + "_" + ThisMoment.Hour + "_" + ThisMoment.Minute + "_" + ThisMoment.Second + "_" + ThisMoment.Millisecond;
            string path = Server.MapPath("/PDF/EvalForm_FR");
            string imagepath = Server.MapPath("/Images/EvalForm_FR");
            var EvaluationDoc = new Document(PageSize.A4, 18, 18, 18, 18);
            ViewBag.ProgramRequestStatusCounts = UserHelper.GetProgramRequestStatusCounts(Convert.ToInt32(Session["ProgramID"]));
            PdfWriter.GetInstance(EvaluationDoc, new FileStream(path + "/Evaluation" + RandomModifer + ".pdf", FileMode.Create));
            BaseFont HelveticaBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

            Font Helvetica = new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

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
                DateLabel.ScalePercent(30f);
                PdfPCell DateLabelCell = new PdfPCell(DateLabel);
                //cell 2
                //iTextSharp.text.Image SpaceFieldDate = iTextSharp.text.Image.GetInstance(imagepath + "/SpaceField.jpg");
                //DateLabel.ScalePercent(30f);
                //PdfPCell SpaceFieldCellDate = new PdfPCell(SpaceFieldDate);
                PdfPCell DateCell = new PdfPCell(new Phrase(efm.ProgramDate, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                DateCell.HorizontalAlignment = 1;
                DateCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                DateCell.BackgroundColor = new iTextSharp.text.BaseColor(202, 204, 206);
                //cell 3
                iTextSharp.text.Image LocationLabel = iTextSharp.text.Image.GetInstance(imagepath + "/LocationLabel.jpg");
                LocationLabel.ScalePercent(30f);
                PdfPCell LocationLabelCell = new PdfPCell(LocationLabel);
                LocationLabelCell.HorizontalAlignment = 0; //alignleft
                                                           //cell 4
                                                           //iTextSharp.text.Image SpaceFieldLoc = iTextSharp.text.Image.GetInstance(imagepath + "/SpaceField.jpg");
                                                           //DateLabel.ScalePercent(30f);
                                                           //PdfPCell SpaceFieldCellLocation = new PdfPCell(SpaceFieldLoc);

                PdfPCell LocationCell = new PdfPCell(new Phrase(efm.ProgramLocation,new  iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                LocationCell.HorizontalAlignment = 1;
                LocationCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                LocationCell.BackgroundColor = new iTextSharp.text.BaseColor(202, 204, 206);

                if (HasBorder == false)
                    DateLabelCell.Border = Rectangle.NO_BORDER;
                if (HasBorder == false)
                    DateCell.Border = Rectangle.NO_BORDER;
                if (HasBorder == false)
                    LocationLabelCell.Border = Rectangle.NO_BORDER;
                if (HasBorder == false)
                    LocationCell.Border = Rectangle.NO_BORDER;


                DateLocation.AddCell(DateLabelCell);
                DateLocation.AddCell(DateCell);
                DateLocation.AddCell(LocationLabelCell);
                DateLocation.AddCell(LocationCell);



                EvaluationDoc.Add(DateLocation);


                //end of set up program date/location

                //set up top image

                iTextSharp.text.Image EvalTop2 = iTextSharp.text.Image.GetInstance(imagepath + "/Demographics.jpg");
                EvalTop2.Alignment = Element.ALIGN_CENTER;

                EvalTop2.ScalePercent(52f);
                EvaluationDoc.Add(EvalTop2);
                //end of set up top image

                //beginning of cases
                if (efm.CaseNewHorizons)
                {
                    iTextSharp.text.Image EvalAF = iTextSharp.text.Image.GetInstance(imagepath + "/CaseNewHorizons.jpg");
                    EvalAF.Alignment = Element.ALIGN_CENTER;
                    EvalAF.ScalePercent(53f);
                    EvaluationDoc.Add(EvalAF);

                }
                if (efm.Case1)
                {
                    iTextSharp.text.Image EvalCase1 = iTextSharp.text.Image.GetInstance(imagepath + "/Case1.jpg");
                    EvalCase1.Alignment = Element.ALIGN_CENTER;
                    EvalCase1.ScalePercent(53f);
                    EvaluationDoc.Add(EvalCase1);
                    EvaluationDoc.NewPage();
                }

                if (efm.Case2)
                {
                    iTextSharp.text.Image EvalCase2 = iTextSharp.text.Image.GetInstance(imagepath + "/Case2.jpg");
                    EvalCase2.ScalePercent(50f);
                    EvaluationDoc.Add(EvalCase2);
                }

                if (efm.Case3)
                {
                    iTextSharp.text.Image EvalCase3 = iTextSharp.text.Image.GetInstance(imagepath + "/Case3.jpg");
                    EvalCase3.ScalePercent(50f);
                    EvaluationDoc.Add(EvalCase3);
                }

                if (efm.Case4)
                {
                    iTextSharp.text.Image EvalCase4 = iTextSharp.text.Image.GetInstance(imagepath + "/Case4.jpg");
                    EvalCase4.ScalePercent(50f);
                    EvaluationDoc.Add(EvalCase4);
                }


                //end of setting up cases
                EvaluationDoc.NewPage();
                //setting up speaker evaluation

                if (!String.IsNullOrEmpty(efm.Speaker1))
                {
                    float[] SpeakerTableWidths = new float[] { 1f, 3f };//speaker column is 2 times wider than evaluation column
                    PdfPTable SpeakerTable = new PdfPTable(2);
                    SpeakerTable.KeepTogether = true;
                    SpeakerTable.TotalWidth = 523f;
                    SpeakerTable.SetWidths(SpeakerTableWidths);
                    SpeakerTable.LockedWidth = true;
                    SpeakerTable.SpacingBefore = 10f;

                    SpeakerTable.SpacingAfter = 10f;
                    SpeakerTable.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right

                    //cell 1
                    iTextSharp.text.Image SpeakerLabel = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerLabel.jpg");
                    SpeakerLabel.ScalePercent(30f);
                    PdfPCell SpeakerLabelCell = new PdfPCell(SpeakerLabel);
                    //cell 2
                    PdfPCell SpeakerCell = new PdfPCell(new Phrase(efm.Speaker1, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    SpeakerCell.HorizontalAlignment = 0;
                    SpeakerCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    SpeakerCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    if (HasBorder == false)
                        SpeakerLabelCell.Border = Rectangle.NO_BORDER;
                    if (HasBorder == false)
                        SpeakerCell.Border = Rectangle.NO_BORDER;

                    SpeakerTable.AddCell(SpeakerLabelCell);
                    SpeakerTable.AddCell(SpeakerCell);
                    EvaluationDoc.Add(SpeakerTable);

                    iTextSharp.text.Image SpeakerEval1 = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerEval1.jpg");

                    SpeakerEval1.Alignment = Element.ALIGN_CENTER;
                    SpeakerEval1.ScalePercent(52f);
                    EvaluationDoc.Add(SpeakerEval1);
                    iTextSharp.text.Image SpeakerEval2 = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerEval2.jpg");

                    SpeakerEval2.Alignment = Element.ALIGN_CENTER;
                    SpeakerEval2.ScalePercent(52f);
                    EvaluationDoc.Add(SpeakerEval2);

                }
               

                //setting up last page
                iTextSharp.text.Image LastPage = iTextSharp.text.Image.GetInstance(imagepath + "/LastPage.jpg");
                LastPage.Alignment = Element.ALIGN_CENTER;
                LastPage.ScalePercent(52f);
                EvaluationDoc.Add(LastPage);

                efm.DisplayPDF = true;

                efm.FileName = "/PDF/EvalForm_FR/Evaluation" + RandomModifer + ".pdf";
                return View("ProgramMaterials_1", efm);
            }
            catch (Exception e)
            {
                efm.DisplayPDF = false;
                return View("ProgramMaterials_1", efm);
            }
            finally
            {
                EvaluationDoc.Close();
            }
        }

        public ActionResult EvalForm2(EvalForm2Model efm)
        {

            bool HasBorder = false;  //set to false when development is completed. 
            System.DateTime ThisMoment = System.DateTime.Now;
            String RandomModifer = ThisMoment.Year + "_" + ThisMoment.Month + "_" + ThisMoment.Day + "_" + ThisMoment.Hour + "_" + ThisMoment.Minute + "_" + ThisMoment.Second + "_" + ThisMoment.Millisecond;
            string path = Server.MapPath("/PDF/EvalForm2");
            string imagepath = Server.MapPath("/Images/EvalForm2");
            var EvaluationDoc = new Document(PageSize.A4, 18, 18, 18, 18);
            ViewBag.ProgramRequestStatusCounts = UserHelper.GetProgramRequestStatusCounts(Convert.ToInt32(Session["ProgramID"]));
            PdfWriter.GetInstance(EvaluationDoc, new FileStream(path + "/Evaluation" + RandomModifer + ".pdf", FileMode.Create));
            BaseFont HelveticaBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

            Font Helvetica = new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

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
                DateLabel.ScalePercent(30f);
                PdfPCell DateLabelCell = new PdfPCell(DateLabel);
                //cell 2
                //iTextSharp.text.Image SpaceFieldDate = iTextSharp.text.Image.GetInstance(imagepath + "/SpaceField.jpg");
                //DateLabel.ScalePercent(30f);
                //PdfPCell SpaceFieldCellDate = new PdfPCell(SpaceFieldDate);
                PdfPCell DateCell = new PdfPCell(new Phrase(efm.ProgramDate, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                DateCell.HorizontalAlignment = 1;
                DateCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                DateCell.BackgroundColor = new iTextSharp.text.BaseColor(202, 204, 206);
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
                LocationCell.BackgroundColor = new iTextSharp.text.BaseColor(202, 204, 206);

                if (HasBorder == false)
                    DateLabelCell.Border = Rectangle.NO_BORDER;
                if (HasBorder == false)
                    DateCell.Border = Rectangle.NO_BORDER;
                if (HasBorder == false)
                    LocationLabelCell.Border = Rectangle.NO_BORDER;
                if (HasBorder == false)
                    LocationCell.Border = Rectangle.NO_BORDER;


                DateLocation.AddCell(DateLabelCell);
                DateLocation.AddCell(DateCell);
                DateLocation.AddCell(LocationLabelCell);
                DateLocation.AddCell(LocationCell);



                EvaluationDoc.Add(DateLocation);


                //end of set up program date/location

                //set up top image

                iTextSharp.text.Image EvalTop2 = iTextSharp.text.Image.GetInstance(imagepath + "/Demographics.jpg");
                EvalTop2.Alignment = Element.ALIGN_CENTER;

                EvalTop2.ScalePercent(52f);
                EvaluationDoc.Add(EvalTop2);

                iTextSharp.text.Image LearningObjectives = iTextSharp.text.Image.GetInstance(imagepath + "/LearningObjectives.jpg");
                LearningObjectives.Alignment = Element.ALIGN_CENTER;

                LearningObjectives.ScalePercent(52f);
                EvaluationDoc.Add(LearningObjectives);
                //end of set up top image

                //beginning of cases
                if (efm.CoreDeck)
                {
                    iTextSharp.text.Image EvalCoreDeck = iTextSharp.text.Image.GetInstance(imagepath + "/CoreDeck.jpg");
                    EvalCoreDeck.Alignment = Element.ALIGN_CENTER;
                    EvalCoreDeck.ScalePercent(53f);
                    EvaluationDoc.Add(EvalCoreDeck);

                }

                if (efm.Case1)
                {
                    iTextSharp.text.Image EvalCase1 = iTextSharp.text.Image.GetInstance(imagepath + "/Case1.jpg");
                    EvalCase1.Alignment = Element.ALIGN_CENTER;
                    EvalCase1.ScalePercent(53f);
                    EvaluationDoc.Add(EvalCase1);
                    EvaluationDoc.NewPage();
                }

                if (efm.Case2)
                {
                    iTextSharp.text.Image EvalCase2 = iTextSharp.text.Image.GetInstance(imagepath + "/Case2.jpg");
                    EvalCase2.ScalePercent(50f);
                    EvaluationDoc.Add(EvalCase2);
                }

                if (efm.Case3)
                {
                    iTextSharp.text.Image EvalCase3 = iTextSharp.text.Image.GetInstance(imagepath + "/Case3.jpg");
                    EvalCase3.ScalePercent(50f);
                    EvaluationDoc.Add(EvalCase3);
                }

                if (efm.Case4)
                {
                    iTextSharp.text.Image EvalCase4 = iTextSharp.text.Image.GetInstance(imagepath + "/Case4.jpg");
                    EvalCase4.ScalePercent(50f);
                    EvaluationDoc.Add(EvalCase4);
                }
                if (efm.Case5)
                {
                    iTextSharp.text.Image EvalCase5 = iTextSharp.text.Image.GetInstance(imagepath + "/Case5.jpg");
                    EvalCase5.ScalePercent(50f);
                    EvaluationDoc.Add(EvalCase5);
                }


                //end of setting up cases
                EvaluationDoc.NewPage();  //make sure speaker evaluation starts on a new page
                //setting up speaker evaluation

                if (!String.IsNullOrEmpty(efm.Speaker1))
                {
                    float[] SpeakerTableWidths = new float[] { 1f, 3f };//speaker column is 2 times wider than evaluation column
                    PdfPTable SpeakerTable = new PdfPTable(2);
                    SpeakerTable.KeepTogether = true;
                    SpeakerTable.TotalWidth = 523f;
                    SpeakerTable.SetWidths(SpeakerTableWidths);
                    SpeakerTable.LockedWidth = true;
                    SpeakerTable.SpacingBefore = 10f;

                    SpeakerTable.SpacingAfter = 10f;
                    SpeakerTable.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right

                    //cell 1
                    iTextSharp.text.Image SpeakerLabel = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerLabel.jpg");
                    SpeakerLabel.ScalePercent(30f);
                    PdfPCell SpeakerLabelCell = new PdfPCell(SpeakerLabel);
                    //cell 2
                    PdfPCell SpeakerCell = new PdfPCell(new Phrase(efm.Speaker1, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    SpeakerCell.HorizontalAlignment = 0;
                    SpeakerCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    SpeakerCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    if (HasBorder == false)
                        SpeakerLabelCell.Border = Rectangle.NO_BORDER;
                    if (HasBorder == false)
                        SpeakerCell.Border = Rectangle.NO_BORDER;

                    SpeakerTable.AddCell(SpeakerLabelCell);
                    SpeakerTable.AddCell(SpeakerCell);
                    EvaluationDoc.Add(SpeakerTable);

                    iTextSharp.text.Image SpeakerEval1 = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerEval1.jpg");

                    SpeakerEval1.Alignment = Element.ALIGN_CENTER;
                    SpeakerEval1.ScalePercent(52f);
                    EvaluationDoc.Add(SpeakerEval1);
                    iTextSharp.text.Image SpeakerEval2 = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerEval2.jpg");

                    SpeakerEval2.Alignment = Element.ALIGN_CENTER;
                    SpeakerEval2.ScalePercent(52f);
                    EvaluationDoc.Add(SpeakerEval2);

                }
                if (!String.IsNullOrEmpty(efm.Speaker2))
                {
                    float[] SpeakerTableWidths = new float[] { 1f, 3f };//speaker column is 2 times wider than evaluation column
                    PdfPTable SpeakerTable = new PdfPTable(2);
                    SpeakerTable.KeepTogether = true;
                    SpeakerTable.TotalWidth = 523f;
                    SpeakerTable.SetWidths(SpeakerTableWidths);
                    SpeakerTable.LockedWidth = true;
                    SpeakerTable.SpacingBefore = 10f;

                    SpeakerTable.SpacingAfter = 10f;
                    SpeakerTable.HorizontalAlignment = 1;//0=Left, 1=Centre, 2=Right

                    ////cell 1 missing speaker label need image from Elana
                    iTextSharp.text.Image SpeakerLabel = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerLabel.jpg");
                    SpeakerLabel.ScalePercent(30f);
                    PdfPCell SpeakerLabelCell = new PdfPCell(SpeakerLabel);
                    //cell 2
                    PdfPCell SpeakerCell = new PdfPCell(new Phrase(efm.Speaker2, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    SpeakerCell.HorizontalAlignment = 0;
                    SpeakerCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    SpeakerCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    if (HasBorder == false)
                        SpeakerLabelCell.Border = Rectangle.NO_BORDER;
                    if (HasBorder == false)
                        SpeakerCell.Border = Rectangle.NO_BORDER;

                    SpeakerTable.AddCell(SpeakerLabelCell);
                    SpeakerTable.AddCell(SpeakerCell);
                    EvaluationDoc.Add(SpeakerTable);

                    iTextSharp.text.Image SpeakerEval1 = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerEval1.jpg");

                    SpeakerEval1.Alignment = Element.ALIGN_CENTER;
                    SpeakerEval1.ScalePercent(52f);
                    EvaluationDoc.Add(SpeakerEval1);
                    iTextSharp.text.Image SpeakerEval2 = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerEval2.jpg");

                    SpeakerEval2.Alignment = Element.ALIGN_CENTER;
                    SpeakerEval2.ScalePercent(52f);
                    EvaluationDoc.Add(SpeakerEval2);

                }

                //setting up this program
                iTextSharp.text.Image ThisProgram = iTextSharp.text.Image.GetInstance(imagepath + "/ThisProgram.jpg");
                ThisProgram.Alignment = Element.ALIGN_CENTER;
                ThisProgram.ScalePercent(52f);
                EvaluationDoc.Add(ThisProgram);

                EvaluationDoc.NewPage();

                //setting up last page
                iTextSharp.text.Image LastPage = iTextSharp.text.Image.GetInstance(imagepath + "/LastPage.jpg");
                LastPage.Alignment = Element.ALIGN_CENTER;
                LastPage.ScalePercent(52f);
                EvaluationDoc.Add(LastPage);

                efm.DisplayPDF = true;

                efm.FileName = "/PDF/EvalForm2/Evaluation" + RandomModifer + ".pdf";
                return View("ProgramMaterials_2", efm);
            }
            catch (Exception e)
            {
                efm.DisplayPDF = false;
                return View("ProgramMaterials_2", efm);
            }
            finally
            {
                EvaluationDoc.Close();
            }
        }


        public ActionResult EvalForm3(EvalForm3Model efm)
        {

            bool HasBorder = false;  //set to false when development is completed. 
            System.DateTime ThisMoment = System.DateTime.Now;
            String RandomModifer = ThisMoment.Year + "_" + ThisMoment.Month + "_" + ThisMoment.Day + "_" + ThisMoment.Hour + "_" + ThisMoment.Minute + "_" + ThisMoment.Second + "_" + ThisMoment.Millisecond;
            string path = Server.MapPath("/PDF/EvalForm3");
            string imagepath = Server.MapPath("/Images/EvalForm3");
            var EvaluationDoc = new Document(PageSize.A4, 18, 18, 18, 18);
            ViewBag.ProgramRequestStatusCounts = UserHelper.GetProgramRequestStatusCounts(Convert.ToInt32(Session["ProgramID"]));
            PdfWriter.GetInstance(EvaluationDoc, new FileStream(path + "/Evaluation" + RandomModifer + ".pdf", FileMode.Create));
            BaseFont HelveticaBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

            Font Helvetica = new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

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
                DateLocation.HorizontalAlignment = 0;//0=Left, 1=Centre, 2=Right

                //cell 1
                iTextSharp.text.Image DateLabel = iTextSharp.text.Image.GetInstance(imagepath + "/DateLabel.jpg");
                DateLabel.ScalePercent(30f);
                PdfPCell DateLabelCell = new PdfPCell(DateLabel);
                //cell 2
                //iTextSharp.text.Image SpaceFieldDate = iTextSharp.text.Image.GetInstance(imagepath + "/SpaceField.jpg");
                //DateLabel.ScalePercent(30f);
                //PdfPCell SpaceFieldCellDate = new PdfPCell(SpaceFieldDate);
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
                LocationCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);

                if (HasBorder == false)
                    DateLabelCell.Border = Rectangle.NO_BORDER;
                if (HasBorder == false)
                    DateCell.Border = Rectangle.NO_BORDER;
                if (HasBorder == false)
                    LocationLabelCell.Border = Rectangle.NO_BORDER;
                if (HasBorder == false)
                    LocationCell.Border = Rectangle.NO_BORDER;


                DateLocation.AddCell(DateLabelCell);
                DateLocation.AddCell(DateCell);
                DateLocation.AddCell(LocationLabelCell);
                DateLocation.AddCell(LocationCell);



                EvaluationDoc.Add(DateLocation);


                //end of set up program date/location

                //set up top image

                iTextSharp.text.Image EvalTop2 = iTextSharp.text.Image.GetInstance(imagepath + "/Demographics.jpg");
                EvalTop2.Alignment = 0;

                EvalTop2.ScalePercent(52f);
                EvaluationDoc.Add(EvalTop2);

                iTextSharp.text.Image LearningObjectives = iTextSharp.text.Image.GetInstance(imagepath + "/LearningObjectives.jpg");
                LearningObjectives.Alignment = Element.ALIGN_CENTER;

                LearningObjectives.Alignment = 0;//0=Left, 1=Centre, 2=Right
                LearningObjectives.ScalePercent(52f);
                EvaluationDoc.Add(LearningObjectives);
                //end of set up top image

                //beginning of cases
                //no cases required



                //end of setting up cases
                //  EvaluationDoc.NewPage();  //make sure speaker evaluation starts on a new page
                //setting up speaker evaluation

                if (!String.IsNullOrEmpty(efm.Speaker1))
                {
                    float[] SpeakerTableWidths = new float[] { 1f, 3f };//speaker column is 2 times wider than evaluation column
                    PdfPTable SpeakerTable = new PdfPTable(2);
                    SpeakerTable.KeepTogether = true;
                    SpeakerTable.TotalWidth = 523f;
                    SpeakerTable.SetWidths(SpeakerTableWidths);
                    SpeakerTable.LockedWidth = true;
                    SpeakerTable.SpacingBefore = 10f;

                    SpeakerTable.SpacingAfter = 10f;
                    SpeakerTable.HorizontalAlignment = 0;//0=Left, 1=Centre, 2=Right

                    //cell 1
                    iTextSharp.text.Image SpeakerLabel = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerLabel.jpg");
                    SpeakerLabel.ScalePercent(30f);
                    PdfPCell SpeakerLabelCell = new PdfPCell(SpeakerLabel);
                    //cell 2
                    PdfPCell SpeakerCell = new PdfPCell(new Phrase(efm.Speaker1, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    SpeakerCell.HorizontalAlignment = 0;
                    SpeakerCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    SpeakerCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    if (HasBorder == false)
                        SpeakerLabelCell.Border = Rectangle.NO_BORDER;
                    if (HasBorder == false)
                        SpeakerCell.Border = Rectangle.NO_BORDER;

                    SpeakerTable.AddCell(SpeakerLabelCell);
                    SpeakerTable.AddCell(SpeakerCell);
                    EvaluationDoc.Add(SpeakerTable);

                    iTextSharp.text.Image SpeakerEval1 = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerEval1.jpg");

                    SpeakerEval1.Alignment = 0;
                    SpeakerEval1.ScalePercent(52f);
                    EvaluationDoc.Add(SpeakerEval1);
                    iTextSharp.text.Image SpeakerEval2 = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerEval2.jpg");

                    SpeakerEval2.Alignment = 0;
                    SpeakerEval2.ScalePercent(52f);
                    EvaluationDoc.Add(SpeakerEval2);

                    iTextSharp.text.Image SpeakerEval3 = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerEval3.jpg");

                    SpeakerEval3.Alignment = 0;
                    SpeakerEval3.ScalePercent(52f);
                    EvaluationDoc.Add(SpeakerEval3);

                }
                if (!String.IsNullOrEmpty(efm.Speaker2))
                {
                    float[] SpeakerTableWidths = new float[] { 1f, 3f };//speaker column is 2 times wider than evaluation column
                    PdfPTable SpeakerTable = new PdfPTable(2);
                    SpeakerTable.KeepTogether = true;
                    SpeakerTable.TotalWidth = 523f;
                    SpeakerTable.SetWidths(SpeakerTableWidths);
                    SpeakerTable.LockedWidth = true;
                    SpeakerTable.SpacingBefore = 10f;

                    SpeakerTable.SpacingAfter = 10f;
                    SpeakerTable.HorizontalAlignment = 0;//0=Left, 1=Centre, 2=Right

                    ////cell 1 missing speaker label need image from Elana
                    iTextSharp.text.Image SpeakerLabel = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerLabel.jpg");
                    SpeakerLabel.ScalePercent(30f);
                    PdfPCell SpeakerLabelCell = new PdfPCell(SpeakerLabel);
                    //cell 2
                    PdfPCell SpeakerCell = new PdfPCell(new Phrase(efm.Speaker2, new iTextSharp.text.Font(HelveticaBase, 12f, iTextSharp.text.Font.NORMAL, BaseColor.BLACK)));
                    SpeakerCell.HorizontalAlignment = 0;
                    SpeakerCell.BackgroundColor = new iTextSharp.text.BaseColor(255, 255, 255);
                    SpeakerCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    if (HasBorder == false)
                        SpeakerLabelCell.Border = Rectangle.NO_BORDER;
                    if (HasBorder == false)
                        SpeakerCell.Border = Rectangle.NO_BORDER;

                    SpeakerTable.AddCell(SpeakerLabelCell);
                    SpeakerTable.AddCell(SpeakerCell);
                    EvaluationDoc.Add(SpeakerTable);

                    iTextSharp.text.Image SpeakerEval1 = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerEval1.jpg");

                    SpeakerEval1.Alignment = 0;
                    SpeakerEval1.ScalePercent(52f);
                    EvaluationDoc.Add(SpeakerEval1);
                    iTextSharp.text.Image SpeakerEval2 = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerEval2.jpg");

                    SpeakerEval2.Alignment = 0;
                    SpeakerEval2.ScalePercent(52f);
                    EvaluationDoc.Add(SpeakerEval2);

                    iTextSharp.text.Image SpeakerEval3 = iTextSharp.text.Image.GetInstance(imagepath + "/SpeakerEval3.jpg");

                    SpeakerEval3.Alignment = 0;
                    SpeakerEval3.ScalePercent(52f);
                    EvaluationDoc.Add(SpeakerEval3);

                }



                EvaluationDoc.NewPage();

                //setting up last page
                iTextSharp.text.Image LastPage = iTextSharp.text.Image.GetInstance(imagepath + "/LastPage.jpg");
                LastPage.Alignment = 0;
                LastPage.ScalePercent(52f);
                EvaluationDoc.Add(LastPage);

                efm.DisplayPDF = true;

                efm.FileName = "/PDF/EvalForm3/Evaluation" + RandomModifer + ".pdf";
                return View("ProgramMaterials_3", efm);
            }
            catch (Exception e)
            {
                efm.DisplayPDF = false;
                return View("ProgramMaterials_3", efm);
            }
            finally
            {
                EvaluationDoc.Close();
            }
        }
      

        public ActionResult Reports()
        {
            UserModel user = UserHelper.GetLoggedInUser();
            if (user != null && (user.UserType.Equals("5") || user.UserType.Equals("7"))) //only for admin and head office
            {
                ViewBag.ProgramRequestStatusCounts = UserHelper.GetProgramRequestStatusCounts(Convert.ToInt32(Session["ProgramID"]));
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }

        }
      
    }
}