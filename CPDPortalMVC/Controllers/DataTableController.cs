using CPDPortalMVC.DAL;
using CPDPortalMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CPDPortalMVC.Controllers
{
    public class DataTableController : Controller
    {
        // GET: DataTable
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetUserList()
        {

            List<UserModel> liUserModel;

            UserRepository ur = new UserRepository();

            liUserModel = ur.GetAllUsers();

            return Json(new { data = liUserModel }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult AddOrEdit(int userID = 0)
        {

            UserModel um = null;
            UserRepository ur = new UserRepository();
            ur.GetUserByUserID(userID);
            if (userID == 0)
                return View(new UserModel());
            else
            {
                return View(um);
            }
        }


    }
}