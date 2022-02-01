using CPDPortal.Data;
using CPDPortalMVC.Models;
using CPDPortalMVC.Util;
using CPDPortalMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Web;

namespace CPDPortalMVC.DAL
{
    public class ActivateRepository : BaseRepository
    {
        public void AddActivationUser(UserActivationModel um)
        {
            UserRepository userRepo = new UserRepository();

            if (Entities.Users.Any(x => x.Username == um.Username))
            {

                throw new Exception();//cannot add a user with duplicated email address

            }
            User us = new User();
            int userID;
            us.Username = um.Username;
            us.Password = Encryptor.Encrypt(um.Password);
            us.Actviated = true;
            us.ActivationDate = DateTime.Now;
            Entities.Users.Add(us);
            Entities.SaveChanges();
            userID = us.UserID;

            UserInfo usInfo = new UserInfo();
            //using small id here
            var UsertoUpdate = Entities.UserInfoes.Where(x => x.id == um.UserId).SingleOrDefault();

            //update the email address the user enter in activation form.

            UsertoUpdate.EmailAddress = um.Username;
            UsertoUpdate.UserID = userID;
            UsertoUpdate.FirstName = um.FirstName;
            UsertoUpdate.LastName = um.LastName;
            UsertoUpdate.Phone = um.Phone;           
            UsertoUpdate.Province = um.Province;

            //only update provinces and territory if usertype is not 5 or 6
            if (UsertoUpdate.UserType != 5 && UsertoUpdate.UserType != 6)
            {

                var ProvinceAlreadyExist = userRepo.GetUserProvinces(um.UserId).Where(x => x.Checked == true).ToList();
                var TerritoryListAlreadyExists = userRepo.GetUserTerritory(um.UserId).Where(x => x.Checked == true).ToList();
                var differenceProvinces = um.Provinces.Except(ProvinceAlreadyExist, new Util.ProvinceComparer()).ToList();


                if (UsertoUpdate != null)
                {

                    foreach (var item in differenceProvinces)
                    {
                        Entities.UserProvinces.Add(new CPDPortal.Data.UserProvince
                        {
                            UserID = UsertoUpdate.UserID,
                            Province = item.Name
                        });
                    }

                    //We no longer as sale rep/head off/director for territoryID, it is populated by the spreadsheet.

                    //Entities.UserTerritories.Add(new CPDPortal.Data.UserTerritory
                    //{
                    //    UserID = UsertoUpdate.UserID,
                    //    TerritoryID = um.Territories
                    //});


                }

            }

              
                Entities.SaveChanges();
        }

        

        public bool CheckActivationEmailExist(string Email)
        {
            bool ret = false;

            ret = Entities.UserInfoes.Any(x => x.EmailAddress == Email);


            return ret;
        }

        public UserActivationModel GetActivationUserbyEmail(string email)
        {
            UserActivationModel am = new UserActivationModel();


            var userinfo = Entities.UserInfoes.Where(x => x.EmailAddress == email).FirstOrDefault();

            if (userinfo != null)
            {
                am.UserId = userinfo.id;
                am.LastName = userinfo.LastName;
                am.FirstName = userinfo.FirstName;
                am.Phone = userinfo.Phone;
                am.Username = userinfo.EmailAddress;
                am.Province = userinfo.Province;
                am.UserType = userinfo.UserType;

            }
            return am;
        }
        //this function takes small user id 
        public UserActivationModel GetActivationUserById(int userid)
        {
            UserActivationModel am = new UserActivationModel();


            var user = Entities.UserInfoes.Where(x => x.id == userid).SingleOrDefault();

            if (user != null)
            {
                am.UserId = user.id;
                am.LastName = user.LastName;
                am.FirstName = user.FirstName;
                am.Phone = user.Phone;
                am.Username = user.EmailAddress;
                am.Province = user.Province;
                am.UserType = user.UserType;

            }

            return am;


        }

        public int GetUserById(int userid)
        {
            int UserID = 0; 

            var user = Entities.UserInfoes.Where(x => x.id == userid).SingleOrDefault();

            if(user != null){

                UserID = user.UserID ?? 0;

            }

            return UserID;


        }

        public bool CheckifIdExists(int id)
        {
            bool ret = false;

            ret = Entities.UserInfoes.Any(x => x.id == id);


            return ret;
        }

    }
}