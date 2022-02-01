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
using CPDPortalSpeaker.Controllers;

namespace CPDPortalMVC.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
       
     
        public AccountController()
        {
        }


        [HttpGet]
        public ActionResult LogOff()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();


            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

           // TempData["ImageUrl"] = UserHelper.GetCPDLogo(Request);
            ViewBag.ReturnUrl = returnUrl;
           
            return View();
        }

        
        //
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            
            if (!ModelState.IsValid)
            {
                return View();
            }

            var userRepo = new UserRepository();
            bool IsAuthenticated, IsActivated;
            IsAuthenticated = userRepo.AuthenticateSpeaker(model.Email, Encryptor.Encrypt(model.Password));
            if (IsAuthenticated)
            {
                //the database has the correct credentials but is the account activated yet?
                IsActivated = userRepo.IsActivated(model.Email, Encryptor.Encrypt(model.Password));
                if (IsActivated)
                {
                    HttpCookie AuthorizationCookie = UserHelper.GetAuthorizationCookie(model.Email, userRepo.GetRoles(model.Email)); //roles are pipe delimited
                    Response.Cookies.Add(AuthorizationCookie);
                    string[] userRoles = userRepo.GetRolesAsArray((model.Email));
                    System.Web.HttpContext.Current.User = new GenericPrincipal(System.Web.HttpContext.Current.User.Identity, userRoles);  //set the roles of Current.User.Identity
                    
                    // FormsAuthentication.SetAuthCookie(model.Email, false);


                    //bool result1 = User.IsInRole("SPECIALIST");
                    //bool result2 = User.IsInRole("PCP");

                    UserModel CurrentUser = userRepo.GetUserDetails(model.Email);//cannot user identity.name because it will set only when the auth cookie is passed in in the next request.

                    //making sure nothing in the temppaf table from previous session
                   // Session["LogoUrl"]=UserHelper.GetCPDLogo(Request);

                    UserHelper.SetLoggedInUser(CurrentUser, System.Web.HttpContext.Current.Session);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") & !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                       
                            return RedirectToAction("Index", "Home");
                    }
                }//not yet activate, redirect to activate account screen
                else
                {

                    return RedirectToAction("Activate", "Account");
                }
            }
            else
            {
                ModelState.AddModelError("Email", "Invalid Username or Password");
               
                return View();
            }

 ;
        }


        #region Ali'Code


        // GET: /Account/Register
        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            TempData["ImageUrl"] = UserHelper.GetCPDLogo(Request);

            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        public JsonResult ForgotPassword(string Email)
        {
            var userRepo = new UserRepository();

            string error = string.Empty;

            if (String.IsNullOrEmpty(Email))
            {
                error = "Please Enter Email and click Submit";
                return Json(new { Error = error });
            }

            if (userRepo.CheckIfActivated(Email))
            {
                var model = userRepo.GetUserCredentials(Email);
                UserHelper.SendEmailForgotPassword(model.Email, model.CurrentPassword);
                return Json(new { redirectTo = Url.Action("ForgotPasswordConfirmation", new { email = model.Email }) });
            }

            else
            {
                error = "Please activate your account.No User found";
                return Json(new { Error = error });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation(string email)
        {
            TempData["ImageUrl"] = UserHelper.GetCPDLogo(Request);
            if (String.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login");

            }
            return View();

        }


    

        [HttpGet]
        [AllowAnonymous]
        public ActionResult DateAlreadyConfirmed()
        {
            TempData["ImageUrl"] = UserHelper.GetCPDLogo(Request);

            return View("DateAlreadyConfirmed");
        }
        [HttpGet]
        [AllowAnonymous]
        public ActionResult SpeakerProgramDateUpdate(string ProgramDate, int ProgramRequestID)
        {
            TempData["ImageUrl"] = UserHelper.GetCPDLogo(Request);

            ViewBag.ProgramDate = ProgramDate;
            ViewBag.ProgramRequestID = ProgramRequestID;


            return View("SpeakerProgramDateUpdate");
        }


        #endregion


        #region Othercode
       
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

       
        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

     
      
        #endregion

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

      

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

       
        #endregion
    }
}