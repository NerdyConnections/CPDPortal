using CPDPortalMVC.DAL;
using CPDPortalMVC.Models;
using CPDPortalMVC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CPDPortalMVC.Controllers
{
    public class ProgramDetailController : Controller
    {
      

        // GET: ProgramDetail
        public ActionResult Index(int TherapeuticID, int ProgramID)
        {
            ProgramRepository pr = new ProgramRepository();

            ViewBag.TherapeuticArea = pr.GetTherapeuticName(TherapeuticID);
            Session["ProgramName"] = pr.GetProgramName(ProgramID);

            Session["ProgramID"] = ProgramID;

            if (Session["ProgramID"] != null && Session["TherapeuticID"] != null)
                ViewBag.ProgramRequestStatusCounts = UserHelper.GetProgramRequestStatusCounts(Convert.ToInt32(Session["ProgramID"]));
            else
                return RedirectToAction("Index", "Home");

            return View();
        }
    }
}