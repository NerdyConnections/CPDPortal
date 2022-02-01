using CPDPortal.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Web;
using CPDPortalSpeaker.Models;
using CPDPortalSpeaker.Util;

namespace CPDPortalSpeaker.DAL
{
    public class ActivateRepository : BaseRepository
    {
       

        public void AddActivationSpeaker(SpeakerActivationModel um)
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
            //using small id here (We don't have a big ID at this moment in Userinfo table)
            var UsertoUpdate = Entities.UserInfoes.Where(x => x.id == um.UserId).SingleOrDefault();       

            if (UsertoUpdate != null)
            {
                //update UserID in userinfo table
                UsertoUpdate.UserID = userID;
                UsertoUpdate.FirstName = um.FirstName;
                UsertoUpdate.LastName = um.LastName;
                UsertoUpdate.Phone = um.Phone;
                UsertoUpdate.Address = um.Address;
                UsertoUpdate.ClinicName = um.Clinic;
                UsertoUpdate.Province = um.Province;
                UsertoUpdate.City = um.City;
                UsertoUpdate.EmailAddress = um.Username;
                UsertoUpdate.Specialty = um.Speciality;               

                Entities.SaveChanges();
            }

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


            var userinfo = Entities.UserInfoes.Where(x => x.EmailAddress == email).SingleOrDefault();

            if (userinfo != null)
            {
                am.UserId = userinfo.id;
                am.LastName = userinfo.LastName;
                am.FirstName = userinfo.FirstName;
                am.Phone = userinfo.Phone;
                am.Username = userinfo.EmailAddress;
                am.Province = userinfo.Province;
            }
            return am;
        }


        public SpeakerActivationModel GetActivationSpeakerbyEmail(string email)
        {
            SpeakerActivationModel am = new SpeakerActivationModel();


            var userinfo = Entities.UserInfoes.Where(x => x.EmailAddress == email).FirstOrDefault();

            if (userinfo != null)
            {
                am.UserId = userinfo.id;
                am.LastName = userinfo.LastName;
                am.FirstName = userinfo.FirstName;
                am.Phone = userinfo.Phone;
                am.Username = userinfo.EmailAddress;
                am.Province = userinfo.Province;
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

            }

            return am;


        }

        public SpeakerActivationModel GetActivationSpeakerById(int userid)
        {
            SpeakerActivationModel am = new SpeakerActivationModel();


            var user = Entities.UserInfoes.Where(x => x.id == userid).SingleOrDefault();

            if (user != null)
            {
                am.UserId = user.id;
                am.LastName = user.LastName;
                am.FirstName = user.FirstName;
                am.Phone = user.Phone;                
                am.Username = user.EmailAddress;
                am.Province = user.Province;
                am.Clinic = user.ClinicName;
                am.City = user.City;
                am.Speciality = user.Specialty;
                am.Address = user.Address;


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