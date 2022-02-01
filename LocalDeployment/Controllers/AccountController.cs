using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using CPDPortalMVC.Models;
using CPDPortalMVC.Util;
using System.Security.Principal;
using CPDPortalMVC.DAL;
using static CPDPortalMVC.Util.Constants;
using System.Web.Security;

namespace CPDPortalMVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        //added some comments
        #region LoginCode
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountController()
        {
        }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }
       

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {

            TempData["ImageUrl"] = UserHelper.GetCPDLogo(Request);
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
                TempData["ImageUrl"] = UserHelper.GetCPDLogo(Request);
                return View();
            }

            var userRepo = new UserRepository();
            bool IsAuthenticated, IsActivated;
            IsAuthenticated = userRepo.Authenticate(model.Email, Encryptor.Encrypt(model.Password));
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
                    Session["LogoUrl"]=UserHelper.GetCPDLogo(Request);

                    UserHelper.SetLoggedInUser(CurrentUser, System.Web.HttpContext.Current.Session);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/") & !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        if (User.IsInRole(Util.Constants.Admin))
                            return RedirectToAction("Index", "Admin");
                        else
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
                TempData["ImageUrl"] = UserHelper.GetCPDLogo(Request);
                return View();
            }

 ;
        }
        //
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Error");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }
        #endregion

        #region Ali'Code


        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            RegisterModel rm = new RegisterModel();

            return View("Register", rm);
        }

        [HttpPost]

        [AllowAnonymous]
        public JsonResult Register(RegisterModel vm)
        {
            var userRepo = new UserRepository();
            string error = string.Empty;

            if (!ModelState.IsValid)
            {
                error = "Please Fill all field and click Submit";
                return Json(new { Error = error });
            }

            //check if email is valid
            if ((userRepo.CheckIfEmailExist(vm.Email) == false))
            {
                error = "Email does not exists";
                return Json(new { Error = error });
            }
            if ((userRepo.EmailPasswordMatch(vm.Email, vm.CurrentPassword) == false))
            {
                error = "Email or Current Password Does not match";
                return Json(new { Error = error });
            }

            if (userRepo.CheckIfActivated(vm.Email) == true)
            {
                error = "Please Log in with your new password";
                return Json(new { Error = error });
            }

            userRepo.NewRegisterUser(vm.Email, vm.NewPassword, vm.CurrentPassword);

            bool IsAuthenticated, IsActivated;
            IsAuthenticated = userRepo.Authenticate(vm.Email, Encryptor.Encrypt(vm.NewPassword));
            if (IsAuthenticated)
            {
                //the database has the correct credentials but is the account activated yet?
                IsActivated = userRepo.IsActivated(vm.Email, Encryptor.Encrypt(vm.NewPassword));
                if (IsActivated)
                {
                    HttpCookie AuthorizationCookie = UserHelper.GetAuthorizationCookie(vm.Email, userRepo.GetRoles(vm.Email)); //roles are pipe delimited
                    Response.Cookies.Add(AuthorizationCookie);
                    string[] userRoles = userRepo.GetRolesAsArray((vm.Email));
                    System.Web.HttpContext.Current.User = new GenericPrincipal(System.Web.HttpContext.Current.User.Identity, userRoles);  //set the roles of Current.User.Identity


                    UserModel CurrentUser = userRepo.GetUserDetails(vm.Email);//cannot user identity.name because it will set only when the auth cookie is passed in in the next request.

                    //making sure nothing in the temppaf table from previous session
                    Session["LogoUrl"] = UserHelper.GetCPDLogo(Request);
                    UserHelper.SetLoggedInUser(CurrentUser, System.Web.HttpContext.Current.Session);

                    var redirectUrl = Url.RouteUrl("", new { action = "Index", controller = "Home" }, Request.Url.Scheme);
                    redirectUrl += "Home/Index";

                    return Json(new { Url = redirectUrl });

                }
                else
                {
                    error = "User is not Activated";
                    return Json(new { Error = error });
                }
            }
            else
            {
                error = "User is not Authenticated";
                return Json(new { Error = error });
            }
        }

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
        public ActionResult ConfirmSpeakerEmail(string ProgramRequestID, string ProgramDate)
        {
            ProgramRepository repo = new ProgramRepository();
            UserRepository userRepo = new UserRepository();
            ProgramRequestIIModel pr = new ProgramRequestIIModel();
            UserModel um = new UserModel();


            if (string.IsNullOrEmpty(ProgramRequestID) || string.IsNullOrEmpty(ProgramRequestID))
            {
                return RedirectToAction("Login");

            }
            //check to see if user already confirmed the date
            int programRequestID = int.Parse(ProgramRequestID);
          
            string ChoosenDate = ProgramDate;
            pr = repo.GetProgramRequestForSpeaker(programRequestID);
            int ProgramID = pr.ProgramID;
            if (repo.CheckIfSpeakerConfirmedEmail(programRequestID))
            {
                return RedirectToAction("SpeakerProgramDateUpdate", "Account", new { ProgramDate = "AlreadyExists", ProgramRequestID = programRequestID,ProgramID=ProgramID });
            }

            
            //get the programid, pass to webpage for determining if 
            

            string ProgramName = repo.GetProgramRequestName(pr.ProgramID);
            string ProgramNamefr = repo.GetProgramRequestNamefr(pr.ProgramID);
            //get the user object from userinfo table



            if (ProgramDate.Equals("NotAvailable"))
            {
                //change Speakerstatus to Not  Available
                repo.UpdateSpeakerToNotAvailable(programRequestID);
                um = userRepo.GetUserForConfirmEmail(pr.ProgramSpeakerID);
                UserHelper.FromSpeakerToAdmin(pr, um, ProgramName, ProgramDate);


                //send email to salerep when speaker is not available 
                UserHelper.FromSpeakerToSalesRep(pr, um, ProgramName, ProgramDate);


                return RedirectToAction("SpeakerProgramDateUpdate", "Account", new { ProgramDate = ProgramDate, ProgramRequestID = programRequestID });
            }

            //check to see if moderator exists
            if (repo.checkifModeratorExist(programRequestID))
            {
                //if there is a moderator
                string SessionCredit = repo.SessionCredit(pr);
                string SessionCreditfr = repo.GetSessionCreditfr(pr);
                repo.UpdateSpeakerConfirmDate(programRequestID, ProgramDate);
                um = userRepo.GetUserForConfirmEmail(pr.ProgramModeratorID ?? 0);
                if (String.IsNullOrEmpty(um.Language) || um.Language.Equals("en"))
                    UserHelper.FromSpeakerToModerator(pr, um, ProgramName, ChoosenDate, SessionCredit);
                else//moderator speaks french
                {
                    
                    UserHelper.FromSpeakerToModeratorfr(pr, um, ProgramNamefr, ChoosenDate, SessionCreditfr);
                }

            }
            else
            {

               //if no moderator
                repo.UpdateSpeakerConfirmDateWhenNoModerator(programRequestID, ProgramDate);
                um = userRepo.GetUserForConfirmEmail(pr.ProgramSpeakerID);
                UserHelper.FromSpeakerToAdmin(pr, um, ProgramName, ProgramDate);
            }

            return RedirectToAction("SpeakerProgramDateUpdate", "Account", new { ProgramDate = ProgramDate, ProgramRequestID = programRequestID });
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ConfirmModeratorEmail(string ProgramRequestID, string ProgramDate)
        {
            ProgramRepository repo = new ProgramRepository();
            UserRepository userRepo = new UserRepository();
            ProgramRequest pr = new ProgramRequest();
            UserModel um = new UserModel();


            if (string.IsNullOrEmpty(ProgramDate) || string.IsNullOrEmpty(ProgramRequestID))
            {
                return RedirectToAction("Login");

            }
            //check to see if user already confirmed the date
            int programRequestID = int.Parse(ProgramRequestID);


            if (repo.CheckIfModeratorConfirmedEmail(programRequestID))
            {
                return RedirectToAction("SpeakerProgramDateUpdate", "Account", new { ProgramDate = "AlreadyExists", ProgramRequestID = programRequestID });

            }

            pr = repo.GetProgramRequestForSpeaker(programRequestID);
            string ProgramName = repo.GetProgramRequestName(pr.ProgramID);
            //get the user object from userinfo table
            um = userRepo.GetUserForConfirmEmail(pr.ProgramModeratorID ?? 0);

            if (ProgramDate.Equals("NotAvailable"))
            {

                repo.UpdateModeratorToNotAvailable(programRequestID);
                //send email to admin
                UserHelper.FromModeratorToAdmin(pr, um, ProgramName, ProgramDate);
                //send email to SalesRep
                UserHelper.FromModeratorToSalesRep(pr, um, ProgramName, ProgramDate);

                return RedirectToAction("SpeakerProgramDateUpdate", "Account", new { ProgramDate = ProgramDate, ProgramRequestID = programRequestID });
            }
            else
            {
                repo.UpdateModeratorConfirmDate(programRequestID, ProgramDate);
                UserHelper.FromModeratorToAdmin(pr, um, ProgramName, ProgramDate);

            }

            return RedirectToAction("SpeakerProgramDateUpdate", "Account", new { ProgramDate = ProgramDate, ProgramRequestID = programRequestID });
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
        public ActionResult SpeakerProgramDateUpdate(string ProgramDate, int ProgramRequestID, int? ProgramID)
        {
            TempData["ImageUrl"] = UserHelper.GetCPDLogo(Request);

            ViewBag.ProgramDate = ProgramDate;
            ViewBag.ProgramRequestID = ProgramRequestID;
            ViewBag.ProgramID = ProgramID;

            return View("SpeakerProgramDateUpdate");
        }


        #endregion


        #region Othercode
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(model.Provider, model.Code, isPersistent:  model.RememberMe, rememberBrowser: model.RememberBrowser);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }
        
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }

        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            var result = await UserManager.ResetPasswordAsync(user.Id, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);
            var factorOptions = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            return View(new SendCodeViewModel { Providers = factorOptions, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = false });
                case SignInStatus.Failure:
                default:
                    // If the user does not have an account, then prompt the user to create an account
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return View("ExternalLoginFailure");
                }
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToLocal(returnUrl);
                    }
                }
                AddErrors(result);
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }

        //
        // GET: /Account/LogOff

        [HttpGet]
        public ActionResult LogOff()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();


            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/ExternalLoginFailure
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
        #endregion

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}