using CPDPortal.Data;
using CPDPortalMVC.DAL;
using CPDPortalMVC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CPDPortalMVC.Controllers
{
    public class ArchivedController : Controller
    {
        public ActionResult Index(int TherapeuticID)
        {



            ProgramRepository pr = new ProgramRepository();
            ViewBag.TherapeuticArea = pr.GetTherapeuticName(TherapeuticID);
            ViewBag.TherapeuticID = TherapeuticID;
            Session["TherapeuticID"] = TherapeuticID;

            TherapeuticRepository tr = new TherapeuticRepository();
            List<Models.Program> liProgram;


            liProgram = tr.GetArchivedPrograms(TherapeuticID);

            //if (UserHelper.IsInRole(Util.Constants.HeadOffice + "," + Util.Constants.RegionalManager))
            //    {
            //        ViewBag.Message = "you are headoffice";

            //    }
            // ViewBag.ImageUrl = UserHelper.GetCPDLogo(Request);
            return View(liProgram);

        }
    }
}