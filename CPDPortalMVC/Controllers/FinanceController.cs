using CPDPortalMVC.DAL;
using CPDPortalMVC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CPDPortalMVC.Controllers
{
    public class FinanceController : Controller
    {
        
        public ActionResult Index()
        {



            if (Session["ProgramID"] != null)
            {
                ViewBag.UserID = UserHelper.GetLoggedInUser().UserID;
                ViewBag.ProgramID = Session["ProgramID"].ToString();

                int UserId = Convert.ToInt32(UserHelper.GetLoggedInUser().UserID);
                int ProgramId = Convert.ToInt32(Session["ProgramID"]);

                ViewBag.ProgramRequestStatusCounts = UserHelper.GetProgramRequestStatusCounts(Convert.ToInt32(Session["ProgramID"]));

                var lst = (new FinanceRepository()).GetAllFinances(UserId, ProgramId);

                ViewBag.SumSubtotal = lst.Sum(item => item.SubTotal).ToString();
                ViewBag.SumTaxes = lst.Sum(item => item.TaxesCombined).ToString(); 
                ViewBag.SumEventTotal = lst.Sum(item => item.EventTotal).ToString(); 

                //var lst = (new FinanceRepository()).GetAllFinances(UserId, ProgramId);
                //return Json(new { data = lst }, JsonRequestBehavior.AllowGet);

            }
            
                //{
                //    return RedirectToAction("Index", "Home");

                //}
                // ViewBag.ProgramList = repo.GetAllDashboardItems();//populate bootstrap modal with all available programs
                return View();        
        }

       

        public ActionResult GetFinanceInfo(string UserID, string ProgramID)
        {   
            
                //ViewBag.UserID = UserHelper.GetLoggedInUser().UserID;
                //ViewBag.ProgramID = Session["ProgramID"].ToString();

                int UserId = Convert.ToInt32(UserID);
                int ProgramId = Convert.ToInt32(ProgramID);

                ViewBag.ProgramRequestStatusCounts = UserHelper.GetProgramRequestStatusCounts(Convert.ToInt32(Session["ProgramID"]));
                var lst = (new FinanceRepository()).GetAllFinances(UserId, ProgramId);
                return Json(new { data = lst }, JsonRequestBehavior.AllowGet);
                       
          

        }



    }
}