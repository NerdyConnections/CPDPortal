using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using CPDPortalSpeaker.Models;
using CPDPortalSpeaker.Util;
using System.Security.Principal;
using CPDPortalSpeaker.DAL;
using static CPDPortalSpeaker.Util.Constants;
using System.Web.Security;
using System.Collections.Generic;
using System.IO;

namespace CPDPortalSpeaker.Controllers
{
    public class PayeeController : BaseController
    {
        

       // [Authorize]
        public ActionResult Index()
        {

            PayeeRepository repo = new PayeeRepository();
            PayeeModel model = new PayeeModel();
            var CurrentUser = UserHelper.GetLoggedInUser();


            if (CurrentUser != null)
            {

                int UserId = CurrentUser.UserID;
                bool IsSubmitted = repo.IsSubmitted(UserId);
                model = repo.GetPayee(UserId);
                model.IsSubmitted = IsSubmitted;
                model.UserId = UserId;
                return View("Index", model);
            }


            return RedirectToAction("Login", "Account");
        }


        [HttpPost]
        //[Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Index(PayeeModel model)
        {

            PayeeRepository repo = new PayeeRepository();
            var CurrentUser = UserHelper.GetLoggedInUser();

            if ((model.PaymentMethod == "Institution") && (string.IsNullOrEmpty(model.TaxNumber)))
            {

                ModelState.AddModelError("TaxNumber", "*Required");

            }


            if (model.Province == "")
            {

                ModelState.AddModelError("Province", "*");
            }


            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }


            if (CurrentUser != null)
            {


                model.UserId = CurrentUser.UserID;
                repo.AddPayee(model);
                repo.UpdatePayeeInformation(CurrentUser.UserID);
                


                return RedirectToAction("Index", "Home");
                // return RedirectToAction("Payee");

            }
            else
            {

                return RedirectToAction("Login", "Account");

            }


        }

       
    }
}