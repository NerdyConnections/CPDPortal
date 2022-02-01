using CPDPortalMVC.Models;
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
    public class EvalFormController : Controller
    {
        //Get
        public ActionResult Index()
        {
            EvalFormModel efm= new EvalFormModel();
            efm.DisplayPDF = false;
            return View(efm);


        }
        // GET: EvalForm
        [HttpPost]
        public ActionResult Index(EvalFormModel efm)
        {

            bool HasBorder = false;  //set to false when development is completed. 
            System.DateTime ThisMoment = System.DateTime.Now;
            String RandomModifer = ThisMoment.Year + "_" + ThisMoment.Month + "_" + ThisMoment.Day + "_" + ThisMoment.Hour + "_" + ThisMoment.Minute + "_" + ThisMoment.Second + "_" + ThisMoment.Millisecond;
            string path = Server.MapPath("PDF/EvalForm");
            string imagepath = Server.MapPath("Images/EvalForm");
            var EvaluationDoc = new Document(PageSize.A4, 18, 18, 18, 18);

            PdfWriter.GetInstance(EvaluationDoc, new FileStream(path + "/Evaluation" + RandomModifer + ".pdf", FileMode.Create));
            BaseFont HelveticaBase = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, false);

            iTextSharp.text.Font Helvetica = new iTextSharp.text.Font(HelveticaBase, 12, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

            try
            {
                //set up top image
                EvaluationDoc.Open();
                EvaluationDoc.NewPage();
                iTextSharp.text.Image EvalTop1 = iTextSharp.text.Image.GetInstance(imagepath + "/EvalTop1.jpg");
                EvalTop1.ScalePercent(50f);
                EvaluationDoc.Add(EvalTop1);
                //end of set up top image

                //set up program date/location
                PdfPTable tableHeader = new PdfPTable(2);
                tableHeader.TotalWidth = 500f;
                tableHeader.LockedWidth = true;
                tableHeader.SpacingBefore = 20f;

                tableHeader.SpacingAfter = 30f;
                tableHeader.HorizontalAlignment = 1;

                PdfPCell cell1 = new PdfPCell(new Phrase("Program Date: " + efm.ProgramDate, Helvetica));
                cell1.Border = HasBorder ? Rectangle.BOTTOM_BORDER : Rectangle.NO_BORDER;
                PdfPCell cell2 = new PdfPCell(new Phrase("Program Location: " + efm.ProgramLocation, Helvetica));
                cell2.Border = HasBorder ? Rectangle.BOTTOM_BORDER : Rectangle.NO_BORDER;


                tableHeader.AddCell(cell1);
                tableHeader.AddCell(cell2);


                EvaluationDoc.Add(tableHeader);


                //end of set up program date/location

                //set up top image

                iTextSharp.text.Image EvalTop2 = iTextSharp.text.Image.GetInstance(imagepath + "/EvalTop2.jpg");
                EvalTop2.ScalePercent(50f);
                EvaluationDoc.Add(EvalTop2);
                //end of set up top image

                //beginning of cases
                if (efm.CaseNewHorizons)
                {
                    iTextSharp.text.Image EvalAF = iTextSharp.text.Image.GetInstance(imagepath + "/EvalAF.jpg");
                    EvalAF.Alignment = Element.ALIGN_CENTER;
                    EvalAF.ScalePercent(53f);
                    EvaluationDoc.Add(EvalAF);

                }
                if (efm.Case1)
                {
                    iTextSharp.text.Image EvalCase1 = iTextSharp.text.Image.GetInstance(imagepath + "/EvalCase1.jpg");
                    EvalCase1.Alignment = Element.ALIGN_CENTER;
                    EvalCase1.ScalePercent(53f);
                    EvaluationDoc.Add(EvalCase1);
                }

                if (efm.Case2)
                {
                    iTextSharp.text.Image EvalCase2 = iTextSharp.text.Image.GetInstance(imagepath + "/EvalCase2.jpg");
                    EvalCase2.ScalePercent(50f);
                    EvaluationDoc.Add(EvalCase2);
                }

                if (efm.Case3)
                {
                    iTextSharp.text.Image EvalCase3 = iTextSharp.text.Image.GetInstance(imagepath + "/EvalCase3.jpg");
                    EvalCase3.ScalePercent(50f);
                    EvaluationDoc.Add(EvalCase3);
                }

                if (efm.Case4)
                {
                    iTextSharp.text.Image EvalCase4 = iTextSharp.text.Image.GetInstance(imagepath + "/EvalCase4.jpg");
                    EvalCase4.ScalePercent(50f);
                    EvaluationDoc.Add(EvalCase4);
                }


                //end of setting up cases

                iTextSharp.text.Image EvalAgreement = iTextSharp.text.Image.GetInstance(imagepath + "/EvalAgreement.jpg");
                EvalAgreement.ScalePercent(50f);
                EvaluationDoc.Add(EvalAgreement);
                efm.DisplayPDF = true;
                return View(efm);
            }
            catch (Exception e)
            {
                efm.DisplayPDF = false;
                return View(efm);
            }
        }
    }
}