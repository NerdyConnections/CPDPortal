using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CPDPortalSpeaker.DAL;
using CPDPortalSpeaker.Models;

using CPDPortalSpeaker.Util;
using System.Security.Principal;


namespace CPDPortalSpeaker.Controllers
{
    public class ActivateController : BaseController
    {
        // GET: Activate
        //public ActionResult Index()
        //{
        //    TempData["ImageUrl"] = UserHelper.GetCPDLogo(Request);
        //    return View();
        //}

        //// GET: Activate
        //[HttpPost]
        //public ActionResult Index(string Email)
        //{
        //    UserRepository repo = new UserRepository();
        //    ActivateRepository activateReop = new ActivateRepository();
        //    string error = string.Empty;
        //    UserActivationModel am = new UserActivationModel();
        //    //check if email is empty
        //    if (string.IsNullOrEmpty(Email))
        //    {
        //        error = "Required Field";
        //        return Json(new { Error = error });

        //    }
        //    //check if email has proper format
        //    bool IsValidEmail = UserHelper.IsEmailValid(Email);

        //    if (IsValidEmail == false)
        //    {
        //        error = "Invalid Email Format";
        //        return Json(new { Error = error });

        //    }
        //    //check if user exist with this email in our system.
        //    if ((activateReop.CheckActivationEmailExist(Email) == false))
        //    {
        //        error = "Email does not exists";
        //        return Json(new { Error = error });
        //    }

        //    //check if user is not already activited
        //    if(repo.CheckIfActivated(Email) == true)
        //    {

        //        error = "User is Already Activated";
        //        return Json(new { Error = error });
        //    }

        //    am = activateReop.GetActivationUserbyEmail(Email);

        //    if (am != null)
        //    {
                
        //        var redirectUrl = Url.RouteUrl("", new { action = "Account", controller = "Activate" }, Request.Url.Scheme);
        //        redirectUrl += "/" + am.UserId;

        //        return Json(new { Url = redirectUrl });

        //    }

        //    else
        //    {
        //        error = "No User found";
        //        return Json(new { Error = error });

        //    }

        //}
        //[HttpGet]
        ////get small userid
        //public ActionResult Account(int id)
        //{
        //    UserActivationModel um = new UserActivationModel();
        //    UserRepository repo = new UserRepository();
        //    ActivateRepository activateReop = new ActivateRepository();
        //    TempData["ImageUrl"] = UserHelper.GetCPDLogo(Request);

        //    //check if id exists
        //    if (activateReop.CheckifIdExists(id) == false)
        //    {
        //        return RedirectToAction("Login", "Account");

        //    }

        //    //need to see if user is already register 
        //    if (activateReop.GetUserById(id) > 0)
        //    {

        //        return RedirectToAction("Login", "Account");

        //    }

        //    um = activateReop.GetActivationUserById(id);        
            
           


        //    return View("Account", um);
        //}
       

        [HttpGet]
        public ActionResult Index()
        {
            TempData["ImageUrl"] = UserHelper.GetCPDLogo(Request);
            return View();
        }


        [HttpPost]
        public ActionResult Index(string Email)
        {
            UserRepository repo = new UserRepository();
            ActivateRepository activateReop = new ActivateRepository();
            string error = string.Empty;
            SpeakerActivationModel am = new SpeakerActivationModel();
            //check if email is empty
            if (string.IsNullOrEmpty(Email))
            {
                error = "Required Field";
                return Json(new { Error = error });

            }
            //check if email has proper format
            bool IsValidEmail = UserHelper.IsEmailValid(Email);

            if (IsValidEmail == false)
            {
                error = "Invalid Email Format";
                return Json(new { Error = error });

            }
            //check if user exist with this email in our system.
            if ((activateReop.CheckActivationEmailExist(Email) == false))
            {
                error = "Email does not exists";
                return Json(new { Error = error });
            }

            //check if user is not already activited
            if (repo.CheckIfActivated(Email) == true)
            {

                error = "Speaker is Already Activated";
                return Json(new { Error = error });
            }

            am = activateReop.GetActivationSpeakerbyEmail(Email);

            if (am != null)
            {

                var redirectUrl = Url.RouteUrl("", new { action = "Account", controller = "Activate" }, Request.Url.Scheme);
                redirectUrl += "/" + am.UserId;

                return Json(new { Url = redirectUrl });

            }

            else
            {
                error = "No Speaker found";
                return Json(new { Error = error });

            }

        }

        [HttpGet]
        //get small userid
        public ActionResult Account(int id)
        {
            SpeakerActivationModel um = new SpeakerActivationModel();
            UserRepository repo = new UserRepository();
            ActivateRepository activateReop = new ActivateRepository();
            TempData["ImageUrl"] = UserHelper.GetCPDLogo(Request);

            //check if id exists
            if (activateReop.CheckifIdExists(id) == false)
            {
                return RedirectToAction("Login", "Account");

            }

            //need to see if user is already register 
            if (activateReop.GetUserById(id) > 0)
            {

                return RedirectToAction("Login", "Account");

            }

            um = activateReop.GetActivationSpeakerById(id);           


            return View("Account", um);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Account(SpeakerActivationModel vm)
        {
            UserRepository repo = new UserRepository();

            if (!ModelState.IsValid)
            {
                TempData["ImageUrl"] = UserHelper.GetCPDLogo(Request);
               
                return View("Account", vm);
            }
            else
            {
                                          

                ActivateRepository ap = new ActivateRepository();
                //add user to user table and add Userinfo table.
                ap.AddActivationSpeaker(vm);
                //send user email
                UserHelper.SpeakerActivationEmail(vm.FirstName, vm.Username, vm.Password);
                var userRepo = new UserRepository();
                bool IsAuthenticated, IsActivated;
                IsAuthenticated = userRepo.Authenticate(vm.Username, Encryptor.Encrypt(vm.Password));
                if (IsAuthenticated)
                {
                    //the database has the correct credentials but is the account activated yet?
                    IsActivated = userRepo.IsActivated(vm.Username, Encryptor.Encrypt(vm.Password));
                    if (IsActivated)
                    {
                        HttpCookie AuthorizationCookie = UserHelper.GetAuthorizationCookie(vm.Username, userRepo.GetRoles(vm.Username)); //roles are pipe delimited
                        Response.Cookies.Add(AuthorizationCookie);
                        string[] userRoles = userRepo.GetRolesAsArray((vm.Username));
                        System.Web.HttpContext.Current.User = new GenericPrincipal(System.Web.HttpContext.Current.User.Identity, userRoles);  //set the roles of Current.User.Identity


                        UserModel CurrentUser = userRepo.GetUserDetails(vm.Username);//cannot user identity.name because it will set only when the auth cookie is passed in in the next request.

                        //making sure nothing in the temppaf table from previous session
                        Session["LogoUrl"] = UserHelper.GetCPDLogo(Request);
                        UserHelper.SetLoggedInUser(CurrentUser, System.Web.HttpContext.Current.Session);
                        return RedirectToAction("Index", "Home");

                    }

                    else
                    {
                        return RedirectToAction("Login", "Account");

                    }
                }
                else
                {
                    return RedirectToAction("Login", "Account");

                }

               
            }
        }


        




    }

}
