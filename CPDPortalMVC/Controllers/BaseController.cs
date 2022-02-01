using CPDPortalMVC.DAL;
using CPDPortalMVC.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CPDPortalMVC.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
     
        protected override void Initialize(RequestContext requestContext)
        {
            ProgramRepository pr = new ProgramRepository();
            ProgramRequestStatusCount prsc;
            if (Session["ProgramID"] != null)
            {
              
                int ProgramID = Convert.ToInt32(Session["ProgramID"]);
                prsc = pr.GetProgramRequestStatusCounts(ProgramID);
                if (prsc != null)
                    ViewBag.ProgramRequestStatusCounts = prsc;
                else
                {
                    prsc = new ProgramRequestStatusCount();
                    prsc.Percent_Active = 0;
                    prsc.Percent_Attention = 0;
                    prsc.Percent_Cancelled = 0;
                    prsc.Percent_Completed = 0;

                    ViewBag.ProgramRequestStatusCounts = prsc;
                }
            }
        }
            
    }
}