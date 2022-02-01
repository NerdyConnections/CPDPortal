using CPDPortal.Data;
using CPDPortalMVC.Models;
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
    public class UserRepository : BaseRepository
    {

        public List<Models.TherapeuticArea> GetPrivilege(int privilegeID)
        {
            //Privilege objPrivilege = new Privilege();

            List<Models.TherapeuticArea> liTherapeuticArea = null;
            liTherapeuticArea = Entities.Privileges.Where(u => u.PrivilegeID == privilegeID).
            Select(u => new Models.TherapeuticArea
            {
                TherapeuticID = u.TherapeuticArea.TherapeuticID,
                TherapeuticName = u.TherapeuticArea.TherapeuticName,
                TherapeuticTitle = u.TherapeuticArea.TherapeuticTitle,
                TherapeuticImageURL = u.TherapeuticArea.ImageUrl,
               // ActivePrograms = GetActiveProgramByTherapeuticID(u.TherapeuticArea.TherapeuticID)

            }).ToList();

            foreach (var item in liTherapeuticArea)
            {

                item.ActivePrograms = GetActiveProgramByTherapeuticID(item.TherapeuticID);
            }

            return liTherapeuticArea;
        }

        public int GetActiveProgramByTherapeuticID(int TherapeuticID)
        {
            int NumberActivePrograms = 0;

            NumberActivePrograms = Entities.TherapeuticPrograms.Where(u => u.TherapeuticID == TherapeuticID && (u.Program.ProgramCompleted > DateTime.Now || u.Program.ProgramCompleted== null) && (u.Program.Archive == false || u.Program.Archive == null)).Count();

            return NumberActivePrograms;

        }
        public bool IsActivated(string userName, string password)
        {


            // return Entities.Users.Count(i => i.Username == userName && i.Password == password) == 1;
            var user = Entities.Users.Where(x => x.Username == userName && x.Password == password).SingleOrDefault();
            if (user != null)
            {//if username and password exist but the delete flag is turned on, then the user is no longer active and should not be authenticated
                if (user.Actviated ?? false)
                {
                    return true;
                }
                else
                {

                    return false;
                }
            }
            return false;

        }
        public string GetRoleByUserID(int UserID)
        {
            var userRole = Entities.UserInfoes.Where(x => x.UserID == UserID).Select(x => x.UserTypeLookUp.Description).SingleOrDefault();
            return userRole;


        }
        public bool Authenticate(string userName, string password)
        {


            // return Entities.Users.Count(i => i.Username == userName && i.Password == password) == 1;
            var user = Entities.Users.Where(x => x.Username == userName && x.Password == password).SingleOrDefault();
            if (user != null)
            {//if username and password exist but the delete flag is turned on, then the user is no longer active and should not be authenticated
                if (user.Deleted ?? false)
                {
                    return false;
                }
                else
                {

                    return true;
                }
            }
            else
                return false;

        }
        public string[] GetRolesAsArray(string userName)
        {
            return Entities.Users.First(u => u.Username == userName).Roles.Select(r => r.RoleName).ToArray();

        }
        public string GetRoles(string userName)
        {
            string retStr = string.Empty;

            StringBuilder sb = new StringBuilder();

            foreach (string str in Entities.Users.First(u => u.Username == userName).Roles.Select(r => r.RoleName).ToList())
            {
                sb.Append(str).Append("|");

            }

            if (sb.Length > 0)
                retStr = sb.ToString().TrimEnd("|".ToCharArray());

            return retStr;
        }

        public void EditSpeaker(UserModel um)
        {
            var objSpeaker = Entities.UserInfoes.Where(x => x.UserID == um.UserID).SingleOrDefault();
            if (objSpeaker != null)
            {

                objSpeaker.FirstName = um.FirstName;
                objSpeaker.LastName = um.LastName;
                objSpeaker.EmailAddress = um.EmailAddress;
                objSpeaker.Address = um.Address;
                objSpeaker.City = um.City;
              //  objSpeaker.Province = um.Province;
                objSpeaker.PostalCode = um.PostalCode;
                objSpeaker.Phone = um.Phone;
                objSpeaker.Fax = um.Fax;
                objSpeaker.SpeakerHonariumRange = um.HonariumRange;

                    Entities.SaveChanges();
                

            }

        }
        public UserModel GetUserByuserid(int userid)
        {
            UserModel um = null;


            um = Entities.UserInfoes.Where(x => x.id == userid).Select(ui =>
                     new UserModel
                     {
                         ID = ui.id,
                         UserID = ui.UserID??0,
                         UserIDRequestedBy = ui.UserIDRequestedBy ?? 0,
                         FirstName = ui.FirstName,
                         LastName = ui.LastName,
                         EmailAddress = ui.EmailAddress,
                         ClinicName = ui.ClinicName,
                         Address = ui.Address,
                         Address2 = ui.Address2,
                         City = ui.City,
                         Province = ui.Province,
                         PostalCode = ui.PostalCode,
                         Phone = ui.Phone,
                         Fax = ui.Fax,
                         Specialty = ui.Specialty,
                         HonariumRange = ui.SpeakerHonariumRange,
                         SpeakerHonariumRange = ui.SpeakerHonariumRange,
                         TherapeuticArea = ui.TherapeuticArea.TherapeuticName,
                         ModeratorHonariumRange = ui.ModeratorHonariumRange,
                         Language=ui.Language


                     }).SingleOrDefault();

            return um;

        }
        public UserModel GetUserByUserID(int UserID)
        {
            UserModel um= null;
            

             um = Entities.UserInfoes.Where(x => x.UserID == UserID).Select(ui =>
                      new UserModel
                      {
                          ID = ui.id,  
                          UserID = UserID,
                          FirstName = ui.FirstName,
                          LastName = ui.LastName,
                          EmailAddress = ui.EmailAddress,
                          ClinicName = ui.ClinicName,
                          Address = ui.Address,
                          Address2 = ui.Address2,
                          City = ui.City,
                          Province = ui.Province,
                          PostalCode = ui.PostalCode,
                          Phone = ui.Phone,
                          Fax = ui.Fax,
                          Specialty = ui.Specialty,
                          HonariumRange = ui.SpeakerHonariumRange,
                          SpeakerHonariumRange = ui.SpeakerHonariumRange,
                          ModeratorHonariumRange = ui.ModeratorHonariumRange,
                          Language=ui.Language
                          

                      }).SingleOrDefault();

            return um;

        }
       
        public List<UserModel> GetAllUsers()
        {
            List<UserModel> userList = null;
            userList = (from ui in Entities.UserInfoes
                        select new UserModel()
                        {
                            UserID=ui.UserID??0,
                            FirstName = ui.FirstName,
                            LastName = ui.LastName,
                            EmailAddress = ui.EmailAddress,
                            ClinicName = ui.ClinicName,
                            Address = ui.Address,
                            Address2 = ui.Address2,
                            City = ui.City,
                            //Province = ui.Province,
                            PostalCode = ui.PostalCode,
                            Phone = ui.Phone,
                            Fax = ui.Fax,
                            Specialty = ui.Specialty,
                            HonariumRange = ui.SpeakerHonariumRange


                        }).ToList();
            return userList;


        }
        public void AddUser(UserViewModel um)
        {

            if (Entities.Users.Any(x=>x.Username==um.EmailAddress))
            {

                throw new Exception();//cannot add a user with duplicated email address

            }
            //User us = new User();
            //int userID;
            //us.Username = um.EmailAddress;
            //us.Password = ConfigurationManager.AppSettings["defaultPassword"].ToString();//default password (password) for a new unactivated user
            //us.Actviated = false;
            //Entities.Users.Add(us);
            //Entities.SaveChanges();
            //userID = us.UserID;

            UserInfo usInfo = new UserInfo();
          //  usInfo.UserID = userID;
            usInfo.UserType = um.UserTypeID;
            usInfo.FirstName = um.FirstName;
            usInfo.LastName = um.LastName;
            usInfo.EmailAddress = um.EmailAddress;
            usInfo.Address = um.Address;
            usInfo.City = um.City;
           // usInfo.Province = um.Province;
            usInfo.SponsorID = um.SponsorID;
            usInfo.PrivilegeID = um.TherapeuticID;//as a salesrep using the therapeuticID to both prvilegeID and TherapeuticID
            usInfo.TherapeuticID = um.TherapeuticID;//even as a salerep, the therapeutic id needs to be populated, when login in it tries to get the therapeutic area base on therapeutic

            usInfo.PostalCode = um.PostalCode;
            usInfo.Phone = um.Phone;
            usInfo.Fax = um.Fax;
            usInfo.SpeakerHonariumRange = um.HonariumRange;
           
            Entities.UserInfoes.Add(usInfo);
            Entities.SaveChanges();







        }

        public int AddSpeaker(SpeakerViewModel spVM)
        {
            //User us = new User();
            //int userID;
            //us.Username = spVM.EmailAddress;
            //us.Password = ConfigurationManager.AppSettings["defaultPassword"].ToString();//default password (password) for a new unactivated user
            //us.Actviated = false;
            //Entities.Users.Add(us);
            //Entities.SaveChanges();
            //userID = us.UserID;

            UserInfo usInfo = new UserInfo();
            
           // usInfo.UserID = userID;
            usInfo.UserType = spVM.UserType;
            usInfo.FirstName = spVM.FirstName;
            usInfo.LastName = spVM.LastName;
            usInfo.EmailAddress = spVM.EmailAddress;
            usInfo.Address = spVM.Address;
            usInfo.City = spVM.City;
            usInfo.Province = spVM.Province;
            usInfo.TherapeuticID = spVM.TherapeuticID;
            usInfo.PostalCode = spVM.PostalCode;
            usInfo.Phone = spVM.Phone;
            usInfo.Fax = spVM.Fax;
            usInfo.SpeakerHonariumRange = spVM.SpeakerHonariumRange;
            usInfo.ModeratorHonariumRange = spVM.ModeratorHonariumRange;
            usInfo.Status = 2;
            usInfo.SubmittedDate = DateTime.Now;
            usInfo.LastUpdated = DateTime.Now;
            Entities.UserInfoes.Add(usInfo);
            Entities.SaveChanges();

            

            return usInfo.id;

            
        }
        public void EditSpeaker(SpeakerViewModel spVM)
        {


            var m_userInfo = (from v in Entities.UserInfoes
                              where v.id==spVM.ID
                            
                             select v).FirstOrDefault();
            if (m_userInfo != null)
            {
                m_userInfo.FirstName = spVM.FirstName;
                m_userInfo.LastName = spVM.LastName;
                m_userInfo.EmailAddress = spVM.EmailAddress;
                m_userInfo.Address = spVM.Address;
                m_userInfo.City = spVM.City;
                m_userInfo.Province = spVM.Province;
                m_userInfo.PostalCode = spVM.PostalCode;
                m_userInfo.Phone = spVM.Phone;
                m_userInfo.Fax = spVM.Fax;
                m_userInfo.SpeakerHonariumRange = spVM.SpeakerHonariumRange;
                m_userInfo.ModeratorHonariumRange = spVM.ModeratorHonariumRange;
                m_userInfo.LastUpdated = DateTime.Now;
                Entities.SaveChanges();//update patient info
            }            
            

        }
        public void ApproveSpeaker(SpeakerViewModel spVM)
        {
            //speaker may not have registered yet, search by id
            var m_user = Entities.UserInfoes.Where(x => x.id == spVM.ID).SingleOrDefault();
            if (m_user != null)
            {

                m_user.Status = 2;//2 is Approved
                Entities.SaveChanges();

            }
        }
        public List<UserModel> GetAllUsersExceptSpeakers()//it actually retrieve both speakers and moderators
        {
            List<UserModel> userList = null;
            List<int> list = new List<int> { 2, 3 };//speakers: usertype=2  moderator: usertype=3
            userList = (from ui in Entities.UserInfoes
                        where !list.Contains(ui.UserType) //get everyone except speakers for admin tool, speakers/moderator are managed in a different tab

                        orderby ui.LastName
                        select new UserModel()
                        {
                            UserID = ui.UserID ?? 0,
                            UserType = ui.UserTypeLookUp.Description,
                            FirstName = ui.FirstName,
                            LastName = ui.LastName,
                            EmailAddress = ui.EmailAddress,
                            ClinicName = ui.ClinicName,
                            Address = ui.Address,
                            Address2 = ui.Address2,
                            City = ui.City,
                            // Province = ui.Province,
                            PostalCode = ui.PostalCode,
                            Phone = ui.Phone,
                            Fax = ui.Fax,
                            Specialty = ui.Specialty,
                            SponsorName = ui.Sponsor.SponsorName,

                            RepID = ui.RepID,
                            BoneWBSCode = ui.BoneWBSCode,
                            CVWBSCode = ui.CVWBSCode,
                            TerritoryID = (ui.TerritoryID == null) ? "" : ui.TerritoryID.ToString(),

                            Deleted = ui.User.Deleted ?? false ? true : false,

                            Activated = ui.User.Actviated??false

                        }).ToList();

            return userList;


        }
        public List<UserModel> GetAllSpeakers()//it actually retrieve both speakers and moderators
        {
            List<UserModel> userList = null;
            userList = (from ui in Entities.UserInfoes
                        where ui.UserType == 2 || ui.UserType == 3 //speakers: usertype=2  moderator: usertype=3
                        orderby ui.Status, ui.LastName  //the un approved should be top of the grid
                        select new UserModel()
                        {
                            ID = ui.id,
                            UserID = ui.UserID ?? 0,
                            UserIDRequestedBy = ui.UserIDRequestedBy ?? 0,
                            UserType = ui.UserTypeLookUp.Description,
                            AssignedRole=ui.AssignedRole,
                            FirstName = ui.FirstName,
                            LastName = ui.LastName,
                            EmailAddress = ui.EmailAddress,
                            ClinicName = ui.ClinicName,
                            Address = ui.Address,
                            Address2 = ui.Address2,
                            City = ui.City,
                            //Province = ui.Province,
                            PostalCode = ui.PostalCode,
                            Phone = ui.Phone,
                            Fax = ui.Fax,
                            Specialty = ui.Specialty,
                            HonariumRange = ui.SpeakerHonariumRange,
                            SubmittedDate = (ui.SubmittedDate == null) ? null : SqlFunctions.DateName("year", ui.SubmittedDate) + "/" + SqlFunctions.DatePart("m", ui.SubmittedDate) + "/" + SqlFunctions.DateName("day", ui.SubmittedDate),
                          
                            StatusString = ui.UserStatus.UserStatusDescription,
                            Status = ui.Status ?? 0,
                            Approved = (ui.Status >= 2) ? true : false,
                            COIForm = Entities.UserRegistrations.Any(x => x.UserID == ui.UserID && x.COIForm == true) ? true : false,
                            COISlides = Entities.COISlidesUploads.Any(x => x.UserID == ui.UserID && x.COISlides == true) ? true : false,


                        }).ToList();

            foreach (var speaker in userList)
            {
                if (speaker.AssignedRole != null)
                {
                    var AssignedRole = Entities.AssignedRoles.Where(x => x.id == speaker.AssignedRole).SingleOrDefault();
                    speaker.AssignedRoleDesc = AssignedRole.RoleDescription;
                }
                var salesRep = Entities.UserInfoes.Where(x => x.UserID == speaker.UserIDRequestedBy).SingleOrDefault();
                if (salesRep != null)
                {
                    speaker.RequestedByFirstName = salesRep.FirstName;
                    speaker.RequestedByLastName = salesRep.LastName;
                    speaker.RequestedBySalesRep = salesRep.FirstName + " " + salesRep.LastName;
                    speaker.StatusString = speaker.StatusString + ", Request By:" + speaker.RequestedBySalesRep;
                }
            }

                return userList;


        }
        public UserModel GetUserDetails(string username)
        {
            var user = Entities.Users.Where(x => x.Username == username).SingleOrDefault();
            int userID = 0;
            if (user != null)
                userID = user.UserID;
            var userInfo = Entities.UserInfoes.Where(x => x.UserID == userID).SingleOrDefault();

            
            if (userInfo != null)
            {
                string therapeuticArea = userInfo.TherapeuticArea.TherapeuticName;

                return new UserModel()
                {
                    UserID = userInfo.UserID ?? 0,
                   // TerritoryID = userInfo.TerritoryID,
                    Username = username,
                    FirstName = userInfo.FirstName,
                    LastName = userInfo.LastName,
                    EmailAddress = userInfo.EmailAddress,
                    ClinicName = userInfo.ClinicName,
                    Address = userInfo.Address,
                    Address2 = userInfo.Address2,
                    City = userInfo.City,
                   // Province = userInfo.Province,
                    PostalCode = userInfo.PostalCode,
                    Phone = userInfo.Phone,
                    Fax = userInfo.Fax,
                    Specialty = userInfo.Specialty,
                    HonariumRange = userInfo.SpeakerHonariumRange,
                    TherapeuticArea = therapeuticArea,
                    TherapeuticID = userInfo.TherapeuticID??0,
                    PrivilegeID = userInfo.PrivilegeID??0,
                    SponsorID = userInfo.SponsorID??0,
                    UserType = userInfo.UserType.ToString(),
                    Language=userInfo.Language




                };//return usermodel object
            }
            else
                return null;
        }
        #region Ali's code
        public bool CheckIfEmailExist(string Email)
        {
            bool ret = false;

            ret = Entities.Users.Any(x => x.Username == Email);

            return ret;
        }

        public bool EmailPasswordMatch(string Email, string Password)
        {
            bool ret = false;

            Password = Util.Encryptor.Encrypt(Password);
            var match = Entities.Users.Where(x => x.Username == Email && x.Password == Password).SingleOrDefault();
            if (match != null)
            {
                ret = true;
            }
            return ret;
        }
        public bool CheckIfActivated(string Email)
        {
            bool ret = false;
            var match = Entities.Users.Where(x => x.Username == Email).SingleOrDefault();
            if (match != null)
            {
                if (match.Actviated == true)
                {
                    ret = true;
                }
            }
            return ret;
        }
        public void NewRegisterUser(string Email, string NewPassword, string oldPassword)
        {
            oldPassword = Util.Encryptor.Encrypt(oldPassword);

            var user = Entities.Users.Where(x => x.Username == Email && x.Password == oldPassword).SingleOrDefault();
            var userinfo = Entities.UserInfoes.Where(x => x.EmailAddress == Email).SingleOrDefault();

            if (user != null && userinfo != null)
            {
                user.Actviated = true;
                user.Username = Email;
                user.Password = Util.Encryptor.Encrypt(NewPassword);
                user.ActivationDate = DateTime.Now;
                userinfo.UserID = user.UserID;
                Entities.SaveChanges();
            }
        }
        public RegisterModel GetUserCredentials(string Email)
        {
            RegisterModel model = new RegisterModel();
            var user = Entities.Users.Where(x => x.Username == Email).SingleOrDefault();

            if (user != null)
            {
                model.Email = user.Username;
                model.CurrentPassword = Util.Encryptor.Decrypt(user.Password);
            }
            return model;
        }
        // sending small Id to get userinfo
        public UserModel GetUserForConfirmEmail(int id)
        {
            UserModel um = null;

            um = Entities.UserInfoes.Where(x => x.id == id).Select(ui =>
                     new UserModel
                     {
                         ID = ui.id,
                         UserID = ui.UserID ?? 0,
                         FirstName = ui.FirstName,
                         LastName = ui.LastName,
                         EmailAddress = ui.EmailAddress,
                         ClinicName = ui.ClinicName,
                         Address = ui.Address,
                         Address2 = ui.Address2,
                         City = ui.City,
                         //Province = ui.Province,
                         PostalCode = ui.PostalCode,
                         Phone = ui.Phone,
                         Fax = ui.Fax,
                         Specialty = ui.Specialty,
                         HonariumRange = ui.SpeakerHonariumRange,
                         SpeakerHonariumRange = ui.SpeakerHonariumRange,
                         ModeratorHonariumRange = ui.ModeratorHonariumRange,
                         Language=ui.Language

                     }).SingleOrDefault();

            return um;

        }
        //Sending big User ID"
        public string CheckUserStatusForEmail(int UserID)
        {
            string val = "";
            var IfUserActivated = Entities.Users.Any(x => x.UserID == UserID);
            var RegisterUser = Entities.UserRegistrations.Where(x => x.UserID == UserID).SingleOrDefault();

            if(RegisterUser != null)
            {
                if ((RegisterUser.COIForm == true) && (RegisterUser.PayeeForm == true))
                {
                    val = "RegisteredCompleted";

                }

                else
                {
                    val = "RegistrationNotComplete";

                }

            }
            else
            {
                if (IfUserActivated)
                {
                    val ="RegistrationNotComplete";

                }
                else
                {
                    val = "NotRegistered";

                }

            }


           
            return val;
           
           
        }



        public List<TerritoryModel> GetUserTerritory(int UserId)
        {
            List<TerritoryModel> Territorylist = Util.AppData.GetTerritories();

            //List<String> Ids = new List<string>();

            //var Terrotories = Entities.UserTerritories.Where(x => x.UserID == UserId).ToList();

            //if (Terrotories != null)
            //{
            //    foreach (var item in Terrotories)
            //    {

            //        Ids.Add(item.TerritoryID);
            //    }
            //    foreach (var item in Ids)
            //    {
            //        foreach (var t in Territorylist)
            //        {
            //            if (t.Id == item)
            //            {
            //                t.Checked = true;

            //            }
            //        }
            //    }

            //}
            return Territorylist;
        }

        public List<ProvinceModel> GetUserProvinces(int UserId)
        {
            List<ProvinceModel> Provincelist = Util.AppData.GetProvinces();

            //List<String> Ids = new List<string>();

            //var Provinces = Entities.UserProvinces.Where(x => x.UserID == UserId).ToList();

            //if (Provinces != null)
            //{
            //    foreach (var item in Provinces)
            //    {

            //        Ids.Add(item.Province);
            //    }
            //    foreach (var item in Ids)
            //    {
            //        foreach (var t in Provincelist)
            //        {
            //            if (t.Name == item)
            //            {
            //                t.Checked = true;

            //            }
            //        }
            //    }

            //}
            return Provincelist;
        }

        public UserModel GetSalesRepForConfirmEmail(int UserID)
        {
            UserModel um = null;

            um = Entities.UserInfoes.Where(x => x.UserID == UserID).Select(ui =>
                     new UserModel
                     {
                         
                         UserID = ui.UserID ?? 0,
                         FirstName = ui.FirstName,
                         LastName = ui.LastName,
                         EmailAddress = ui.EmailAddress,
                         ClinicName = ui.ClinicName,
                         Address = ui.Address,
                         Address2 = ui.Address2,
                         City = ui.City,
                         //Province = ui.Province,
                         PostalCode = ui.PostalCode,
                         Phone = ui.Phone,
                         Fax = ui.Fax,
                         Specialty = ui.Specialty,
                         HonariumRange = ui.SpeakerHonariumRange

                     }).SingleOrDefault();

            return um;

        }




        #endregion
    }
}