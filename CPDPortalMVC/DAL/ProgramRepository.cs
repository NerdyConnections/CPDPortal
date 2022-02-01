using CPDPortal.Data;
using CPDPortalMVC.Models;
using CPDPortalMVC.Util;
using CPDPortalMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CPDPortalMVC.DAL
{
    public class ProgramRepository : BaseRepository
    {
        #region Andrew Code====================================================================================================================================
        public List<Models.ProgramRequest> GetAllProgramRequests(int? programID)
        {
            List<Models.ProgramRequest> programRequestList = null;

            if (programID == null || programID == 0)
            {
                programRequestList = (from pr in Entities.ProgramRequests

                                          // display all requests for now where pr.RequestStatus == 2 || pr.RequestStatus == 3  //show only submitted requests (either approved 3 or waiting to be approved  2 in the admin tool)
                                      orderby pr.Approved, pr.ContactLastName
                                      select new Models.ProgramRequest()
                                      {
                                          ProgramRequestID = pr.ProgramRequestID,
                                          ProgramID = pr.ProgramID ?? 0,
                                          ContactInformation = pr.ContactFirstName + ", " + pr.ContactLastName + "<br/>" + pr.ContactPhone + "," + pr.ContactEmail,
                                          ContactFirstName = pr.ContactFirstName,
                                          ContactLastName = pr.ContactLastName,
                                          ContactPhone = pr.ContactPhone,
                                          ContactEmail = pr.ContactEmail,
                                          SpeakerStatus = Entities.UserInfoes.Where(x => x.id == pr.ProgramSpeakerID).Select(x => x.FirstName + ", " + x.LastName + ", " + x.Phone + ", " + x.EmailAddress + "<br />" +
                                          (x.UserStatus.UserStatusID.ToString() == "0" ? "Not a speaker or moderator" : "") + (x.UserStatus.UserStatusID.ToString() == "1" ? "Pending Approval" : "") + (x.UserStatus.UserStatusID.ToString() == "2" ? "Approved" : "") + (x.UserStatus.UserStatusID.ToString() == "3" ? "RegisteredNotComplete" : "") + (x.UserStatus.UserStatusID.ToString() == "4" ? "RegisteredCompleted" : "") + (x.UserStatus.UserStatusID.ToString() == "5" ? "Opt-Out" : "") +
                                            "<br />" + pr.SpeakerStatus).FirstOrDefault(),

                                          ModeratorStatus = pr.ProgramModeratorID == null ? "Not Applicable" : Entities.UserInfoes.Where(x => x.id == pr.ProgramModeratorID).Select(x => x.FirstName + "," + x.LastName + "<br />" + x.Phone + "," + x.EmailAddress + "<br />" +
                                          (x.UserStatus.UserStatusID.ToString() == "0" ? "Not a speaker or moderator" : "") + (x.UserStatus.UserStatusID.ToString() == "1" ? "Pending Approval" : "") + (x.UserStatus.UserStatusID.ToString() == "2" ? "Approved" : "") + (x.UserStatus.UserStatusID.ToString() == "3" ? "RegisteredNotComplete" : "") + (x.UserStatus.UserStatusID.ToString() == "4" ? "RegisteredCompleted" : "") + (x.UserStatus.UserStatusID.ToString() == "5" ? "Opt-Out" : "") + "<br />" + pr.ModeratorStatus).FirstOrDefault(),
                                          ConfirmedSessionDate = (pr.ConfirmedSessionDate == null) ? null : SqlFunctions.DateName("year", pr.ConfirmedSessionDate) + "/" + SqlFunctions.DatePart("m", pr.ConfirmedSessionDate) + "/" + SqlFunctions.DateName("day", pr.ConfirmedSessionDate),

                                          ProgramDate1 = (pr.ProgramDate1 == null) ? null : SqlFunctions.DateName("year", pr.ProgramDate1) + "/" + SqlFunctions.DatePart("m", pr.ProgramDate1) + "/" + SqlFunctions.DateName("day", pr.ProgramDate1),
                                          ProgramDate2 = (pr.ProgramDate2 == null) ? null : SqlFunctions.DateName("year", pr.ProgramDate2) + "/" + SqlFunctions.DatePart("m", pr.ProgramDate2) + "/" + SqlFunctions.DateName("day", pr.ProgramDate2),
                                          ProgramDate3 = (pr.ProgramDate3 == null) ? null : SqlFunctions.DateName("year", pr.ProgramDate3) + "/" + SqlFunctions.DatePart("m", pr.ProgramDate3) + "/" + SqlFunctions.DateName("day", pr.ProgramDate3),




                                          MealStartTime = (pr.MealStartTime == null) ? String.Empty : pr.MealStartTime.ToString(),
                                          ProgramStartTime = (pr.ProgramStartTime == null) ? String.Empty : SqlFunctions.DatePart("Hour", pr.ProgramStartTime) + ":" + SqlFunctions.DatePart("Minute", pr.ProgramStartTime),
                                          ProgramEndTime = (pr.ProgramEndTime == null) ? String.Empty : SqlFunctions.DatePart("Hour", pr.ProgramEndTime) + ":" + SqlFunctions.DatePart("Minute", pr.ProgramEndTime),
                                          //   ProgramCredits = pr.ProgramCredits??0,
                                          SubmittedDate = (pr.SubmittedDate == null) ? null : SqlFunctions.DateName("year", pr.SubmittedDate) + "/" + SqlFunctions.DatePart("m", pr.SubmittedDate) + "/" + SqlFunctions.DateName("day", pr.SubmittedDate),

                                          RequestStatusID=pr.RequestStatus??0,
                                          RequestStatus = pr.RequestStatusLookup.RequestStatusDescription,
                                          Approved = pr.Approved ?? false


                                      }).ToList();
            }
            else
            {
                programRequestList = (from pr in Entities.ProgramRequests
                                      where pr.ProgramID == programID
                                      // display all requests for now where pr.RequestStatus == 2 || pr.RequestStatus == 3  //show only submitted requests (either approved 3 or waiting to be approved  2 in the admin tool)
                                      orderby pr.Approved, pr.ContactLastName
                                      select new Models.ProgramRequest()
                                      {
                                          ProgramRequestID = pr.ProgramRequestID,
                                          ProgramID = pr.ProgramID ?? 0,
                                          ContactInformation = pr.ContactFirstName + ", " + pr.ContactLastName + "<br/>" + pr.ContactPhone + "," + pr.ContactEmail,
                                          ContactFirstName = pr.ContactFirstName,
                                          ContactLastName = pr.ContactLastName,
                                          ContactPhone = pr.ContactPhone,
                                          ContactEmail = pr.ContactEmail,

                                          SpeakerStatus = Entities.UserInfoes.Where(x => x.id == pr.ProgramSpeakerID).Select(x => x.FirstName + ", " + x.LastName + ", " + x.Phone + ", " + x.EmailAddress + "<br />" +
                                          (x.UserStatus.UserStatusID.ToString() == "0" ? "Not a speaker or moderator" : "") + (x.UserStatus.UserStatusID.ToString() == "1" ? "Pending Approval" : "") + (x.UserStatus.UserStatusID.ToString() == "2" ? "Approved" : "") + (x.UserStatus.UserStatusID.ToString() == "3" ? "RegisteredNotComplete" : "") + (x.UserStatus.UserStatusID.ToString() == "4" ? "RegisteredCompleted" : "") + (x.UserStatus.UserStatusID.ToString() == "5" ? "Opt-Out" : "") +
                                          "<br />" + pr.SpeakerStatus).FirstOrDefault(),

                                          ModeratorStatus = pr.ProgramModeratorID == null ? "Not Applicable" : Entities.UserInfoes.Where(x => x.id == pr.ProgramModeratorID).Select(x => x.FirstName + "," + x.LastName + "<br />" + x.Phone + "," + x.EmailAddress + "<br />" +
                                          (x.UserStatus.UserStatusID.ToString() == "0" ? "Not a speaker or moderator" : "") + (x.UserStatus.UserStatusID.ToString() == "1" ? "Pending Approval" : "") + (x.UserStatus.UserStatusID.ToString() == "2" ? "Approved" : "") + (x.UserStatus.UserStatusID.ToString() == "3" ? "RegisteredNotComplete" : "") + (x.UserStatus.UserStatusID.ToString() == "4" ? "RegisteredCompleted" : "") + (x.UserStatus.UserStatusID.ToString() == "5" ? "Opt-Out" : "") + "<br />" + pr.ModeratorStatus).FirstOrDefault(),

                                          ConfirmedSessionDate = (pr.ConfirmedSessionDate == null) ? null : SqlFunctions.DateName("year", pr.ConfirmedSessionDate) + "/" + SqlFunctions.DatePart("m", pr.ConfirmedSessionDate) + "/" + SqlFunctions.DateName("day", pr.ConfirmedSessionDate),
                                          ProgramDate1 = (pr.ProgramDate1 == null) ? null : SqlFunctions.DateName("year", pr.ProgramDate1) + "/" + SqlFunctions.DatePart("m", pr.ProgramDate1) + "/" + SqlFunctions.DateName("day", pr.ProgramDate1),
                                          ProgramDate2 = (pr.ProgramDate2 == null) ? null : SqlFunctions.DateName("year", pr.ProgramDate2) + "/" + SqlFunctions.DatePart("m", pr.ProgramDate2) + "/" + SqlFunctions.DateName("day", pr.ProgramDate2),
                                          ProgramDate3 = (pr.ProgramDate3 == null) ? null : SqlFunctions.DateName("year", pr.ProgramDate3) + "/" + SqlFunctions.DatePart("m", pr.ProgramDate3) + "/" + SqlFunctions.DateName("day", pr.ProgramDate3),




                                          MealStartTime = (pr.MealStartTime == null) ? String.Empty : pr.MealStartTime.ToString(),
                                          ProgramStartTime = (pr.ProgramStartTime == null) ? String.Empty : SqlFunctions.DatePart("Hour", pr.ProgramStartTime) + ":" + SqlFunctions.DatePart("Minute", pr.ProgramStartTime),
                                          ProgramEndTime = (pr.ProgramEndTime == null) ? String.Empty : SqlFunctions.DatePart("Hour", pr.ProgramEndTime) + ":" + SqlFunctions.DatePart("Minute", pr.ProgramEndTime),
                                          //   ProgramCredits = pr.ProgramCredits??0,
                                          SubmittedDate = (pr.SubmittedDate == null) ? null : SqlFunctions.DateName("year", pr.SubmittedDate) + "/" + SqlFunctions.DatePart("m", pr.SubmittedDate) + "/" + SqlFunctions.DateName("day", pr.SubmittedDate),

                                          RequestStatusID = pr.RequestStatus ?? 0,
                                          RequestStatus = pr.RequestStatusLookup.RequestStatusDescription,
                                          Approved = pr.Approved ?? false


                                      }).ToList();


            }

            foreach (var programRequest in programRequestList)
            {

                bool ModulesSeleted = Entities.ProgramRequestModules.Where(x => x.ProgramRequestID == programRequest.ProgramRequestID).Any();
                if (ModulesSeleted == true)
                    programRequest.ModulesSelected = true;
                else
                    programRequest.ModulesSelected = false;
            }
            return programRequestList;


        }
        public string GetTherapeuticName(int theraepeuticID)
        {
            string retVal = "";
            var val = Entities.TherapeuticAreas.Where(x => x.TherapeuticID == theraepeuticID).SingleOrDefault();
            if (val != null)
            {
                retVal = val.TherapeuticTitle;

            }
            return retVal;

        }
        public int GetProgramID(int ProgramRequestID)
        {
            try
            {
                int retVal = 0;
                var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).SingleOrDefault();
                if (val != null)
                {
                    retVal = val.ProgramID ?? 0;

                }
                return retVal;
            }catch (Exception e)
            {
                return 0;
            }

        }
        public string GetProgramName(int programID)
        {
            string retVal = "";
            var val = Entities.Programs.Where(x => x.ProgramID == programID).SingleOrDefault();
            if (val != null)
            {
                retVal = val.ProgramName;

            }
            return retVal;

        }
        public int GetEventsCompleted_old(int programID)
        {
            int val = 0;
            int UserID = UserHelper.GetLoggedInUser().UserID;
            string UserRole = UserHelper.GetRoleByUserID(UserID);
            int SponsorID = UserHelper.GetLoggedInUser().SponsorID;

            //head office and sale director see every sessions for a program for a sponsor no need to query for territoryID
            if (UserRole == Util.Constants.HeadOffice || UserRole == Util.Constants.SalesDirector)
            {

                val = Entities.ProgramRequests.Where(x => x.ProgramID == programID && x.RequestStatus == 4 && x.SponsorID == SponsorID).Count();

            }//see all users under the manager's territoryID
            else if (UserRole == Util.Constants.RegionalManager)
            {  //need to retrieve all users with the same territoryID and get all dashboard items with the same territoryID

                //the manager's territoryID ie. 41
                var TerritoryID = (from ut in Entities.UserTerritories where ut.UserID == UserID select ut.TerritoryID).FirstOrDefault();
                //all userids under territoryID 41
                var TerritorialUserIDs = (from ut in Entities.UserTerritories where ut.TerritoryID == TerritoryID select ut.UserID).ToList();
                //dashboarditems of all userids belonging to territory 41

                val = Entities.ProgramRequests.Where(x => x.ProgramID == programID && x.RequestStatus == 4 && TerritorialUserIDs.Contains(x.UserID)).Count();


            }
            else //sale rep only get to see he own session request
            {
                val = Entities.ProgramRequests.Where(x => x.ProgramID == programID && x.RequestStatus == 4 && x.UserID == UserID).Count();
            }


            return val;

        }
        public int GetEventsCompleted(int programID)
        {
            int val=0;
            int UserID = UserHelper.GetLoggedInUser().UserID;
            string UserRole = UserHelper.GetRoleByUserID(UserID);
            int SponsorID = UserHelper.GetLoggedInUser().SponsorID;
           
            //head office and sale director see every sessions for a program for a sponsor no need to query for territoryID
           // if (UserRole == Util.Constants.HeadOffice || UserRole == Util.Constants.SalesDirector)
            //{

                 val = Entities.ProgramRequests.Where(x => x.ProgramID == programID && x.RequestStatus == 4 && x.SponsorID==SponsorID).Count();

            //}//see all users under the manager's territoryID
            //else 
           // {  //need to retrieve all users with the same territoryID and get all dashboard items with the same territoryID

                //the manager's territoryID ie. 41
                //var TerritoryID = (from ut in Entities.UserInfoes where ut.UserID == UserID select ut.TerritoryID).FirstOrDefault();
                //all userids under territoryID 41
               // var TerritorialUserIDs = (from ut in Entities.UserInfoes where ut.TerritoryID == TerritoryID select ut.UserID).ToList();
                //dashboarditems of all userids belonging to territory 41

                 //val = Entities.ProgramRequests.Where(x => x.ProgramID == programID && x.RequestStatus == 4 &&  TerritorialUserIDs.Contains(x.UserID)).Count();

               
            //}
           
           

            return val;

        }
        public List<SelectListItem> GetAllProgram()
        {
            List<SelectListItem> programList = new List<SelectListItem>();
            var programs = Entities.Programs.ToList();
            foreach (CPDPortal.Data.Program p in programs)
            {
                programList.Add(new SelectListItem
                {
                    Text = p.ProgramName,
                    Value = p.ProgramID.ToString()

                });
            }
            return programList;

        }
        //public ProgramRequest GetProgramRequestByID(int ProgramRequestID)
        //{
        //    if (ProgramRequestID != 0)
        //    {
        //        ProgramRequest pr;
        //        pr = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).Select(ui =>
        //                 new ProgramRequest
        //                 {
        //                     ProgramRequestID = ProgramRequestID,
        //                     ContactFirstName = ui.ContactFirstName,
        //                     ContactLastName = ui.ContactLastName,
        //                     ContactEmail = ui.ContactEmail,
        //                     ContactPhone = ui.ContactPhone,
        //                     SessionAgendaUploaded = ui.SessionAgendaUploaded,
        //                     SessionAgendaFileName = ui.SessionAgendaFileName



        //                 }).SingleOrDefault();

        //        return pr;
        //    }
        //    else
        //        return null;
        //}

        //public ProgramRequest InitialProgramRequestForm(int ProgramID)
        //{
        //    var CurrentUser = UserHelper.GetLoggedInUser();

        //    ProgramRequest pr = new ProgramRequest();
        //    //they are all in the hidden field, for saving later.
        //    pr.ContactName = CurrentUser.FirstName + ", " + CurrentUser.LastName;
        //    pr.ContactFirstName = CurrentUser.FirstName;
        //    pr.ContactLastName = CurrentUser.LastName;
        //    pr.ContactEmail = CurrentUser.Username;
        //    pr.ContactPhone = CurrentUser.Phone;
        //    pr.SpeakerChosenProgramDate = false;
        //    pr.ModeratorChosenProgramDate = false;
        //    pr.ContactPhone = CurrentUser.Phone;
        //    pr.ProgramRequestID = GetProgramRequestID();
        //    pr.ProgramID = ProgramID;
        //    pr.SponsorID = CurrentUser.SponsorID;
        //    //get approved speakers
        //    pr.Speakers = Entities.UserInfoes.Where(x => x.UserType == 2 && x.Approved == true).Select(c => new SelectListItem
        //    {
        //        Value = c.id.ToString(),
        //        Text = c.FirstName + " " + c.LastName

        //    });
        //    //get approved moderators only
        //    pr.Moderators = Entities.UserInfoes.Where(x => x.UserType == 3 && x.Approved == true).Select(c => new SelectListItem
        //    {
        //        Value = c.id.ToString(),
        //        Text = c.FirstName + " " + c.LastName

        //    });
        //    return pr;
        //}

        //public int GetProgramRequestID()
        //{
        //    int ProgramRequestID;
        //    try {
                

        //        CPDPortal.Data.ProgramRequest pr = new CPDPortal.Data.ProgramRequest();

        //        Entities.ProgramRequests.Add(pr);
        //        Entities.SaveChanges();

        //        ProgramRequestID = pr.ProgramRequestID;
        //            return ProgramRequestID;
        //     }
        //    catch(Exception e)
        //    {
        //        ProgramRequestID = 0;
        //        return ProgramRequestID;
        //    }

          
        //}

        public int UpdateUploadFileStatus(int ProgramRequestID, bool FileUploadStatus)
        {
            try {
                int FileCount = 0;
                var objProgramRequest = (from v in Entities.ProgramRequests

                                         where v.ProgramRequestID == ProgramRequestID
                                         select v).FirstOrDefault();

                if (objProgramRequest != null)
                {
                    objProgramRequest.SessionAgendaUploaded = FileUploadStatus;
                    Entities.SaveChanges();
                    FileCount = 1;

                }
                return FileCount;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return 0;
            }
          
        }
        public void SaveProgramRequest(Models.ProgramRequest pr)
        {
            var objProgramRequests = Entities.ProgramRequests.Where(x => x.ProgramRequestID == pr.ProgramRequestID).SingleOrDefault();
            if (objProgramRequests != null)
            {
                objProgramRequests.ContactFirstName = pr.ContactFirstName;
                objProgramRequests.ContactLastName = pr.ContactLastName;
                objProgramRequests.ContactPhone = pr.ContactPhone;
                objProgramRequests.ContactEmail = pr.ContactEmail;
                objProgramRequests.ProgramDate1 = DateTime.ParseExact(pr.ProgramDate1, "d", null);
                //d stands for short date pattern and null specifies that the current culture should be used for parsing the string
                 if (!String.IsNullOrEmpty(pr.ProgramDate2))
                objProgramRequests.ProgramDate2 = DateTime.ParseExact(pr.ProgramDate2, "d", null);
                if (!String.IsNullOrEmpty(pr.ProgramDate3))
                    objProgramRequests.ProgramDate3 = DateTime.ParseExact(pr.ProgramDate3, "d", null);
                objProgramRequests.MealStartTime = DateTime.ParseExact(pr.MealStartTime, "g", null);
                objProgramRequests.SubmittedDate = DateTime.Now;

                Entities.SaveChanges();

            }

        }

        public SessionUpload GetSessionUpload(int ProgramRequestID)
        {
            if (ProgramRequestID != 0)
            {
                SessionUpload su;
                su = Entities.ProgramRequestFileUploads.Where(x => x.ProgramRequestID == ProgramRequestID).Select(ui =>
                         new SessionUpload
                         {
                             ProgramRequestID = ProgramRequestID,
                             EvaluationFullPath = ui.EvaluationFullPath,
                             EvaluationUploaded = ui.EvaluationUploaded,
                             EvaluationFileName = ui.EvaluationFileName,
                             EvaluationFileExt = ui.EvaluationFileExt,
                             SignInFullPath = ui.SignInFullPath,
                             SignInUploaded = ui.SignInUploaded,
                             SignInFileName = ui.SignInFileName,
                             SignInFileExt = ui.SignInFileExt,
                             UserOtherFullPath = ui.UserOtherFullPath,
                             UserOtherUploaded = ui.UserOtherUploaded,
                             UserOtherFileName = ui.UserOtherFileName,
                             UserOtherFileExt = ui.UserOtherFileExt,
                             SpeakerAgreementFileExt = ui.SpeakerAgreementFileExt,
                             SpeakerAgreementFileName = ui.SpeakerAgreementFileName,
                             SpeakerAgreementFullPath = ui.SpeakerAgreementFullPath,
                             SpeakerAgreementUploaded = ui.SpeakerAgreementUploaded,



                         }).SingleOrDefault();
              
                // var objProgramRequest = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).SingleOrDefault();
                //  if (objProgramRequest != null)
                //  su.ProgramID = objProgramRequest.ProgramID??0;

                return su;
            }
            else
                return null;
        }
        public void UpdateProgramRequestFileUpload(SessionUpload su)
        {
            var session_upload = (from v in Entities.ProgramRequestFileUploads

                                  where v.ProgramRequestID == su.ProgramRequestID
                                  select v).FirstOrDefault();
            if (session_upload == null)
            {

                CPDPortal.Data.ProgramRequestFileUpload objprful = new CPDPortal.Data.ProgramRequestFileUpload();

                objprful.ProgramRequestID = su.ProgramRequestID;
                objprful.EvaluationFullPath = su.EvaluationFullPath;
                objprful.EvaluationUploaded = su.EvaluationUploaded;
                objprful.EvaluationFileName = su.EvaluationFileName;
                objprful.EvaluationFileExt = su.EvaluationFileExt;
                //signin sheet
                objprful.SignInFullPath = su.SignInFullPath;
                objprful.SignInUploaded = su.SignInUploaded;
                objprful.SignInFileName = su.SignInFileName;
                objprful.SignInFileExt = su.SignInFileExt;
                //speaker agreement
                objprful.SpeakerAgreementFullPath = su.SpeakerAgreementFullPath;
                objprful.SpeakerAgreementUploaded = su.SpeakerAgreementUploaded;
                objprful.SpeakerAgreementFileName = su.SpeakerAgreementFileName;
                objprful.SpeakerAgreementFileExt = su.SpeakerAgreementFileExt;
                //userother
                objprful.UserOtherFullPath = su.UserOtherFullPath;
                objprful.UserOtherUploaded = su.UserOtherUploaded;
                objprful.UserOtherFileName = su.UserOtherFileName;
                objprful.UserOtherFileExt = su.UserOtherFileExt;

                objprful.SpeakerAgreementFileExt = su.SpeakerAgreementFileExt;
                objprful.SpeakerAgreementFileName = su.SpeakerAgreementFileName;
                objprful.SpeakerAgreementFullPath = su.SpeakerAgreementFullPath;
                objprful.SpeakerAgreementUploaded = su.SpeakerAgreementUploaded;






                objprful.LastUpdated = DateTime.Now;
                Entities.ProgramRequestFileUploads.Add(objprful);
                Entities.SaveChanges();
                //PatientID = objTempMAF.PatientID;
            }
            else
            {
                //update patient
                if (su.EvaluationUploaded ?? false)
                {
                    session_upload.ProgramRequestID = su.ProgramRequestID;
                    session_upload.EvaluationFullPath = su.EvaluationFullPath;
                    session_upload.EvaluationUploaded = su.EvaluationUploaded;
                    session_upload.EvaluationFileName = su.EvaluationFileName;
                    session_upload.EvaluationFileExt = su.EvaluationFileExt;
                }
                //signin sheet
                if (su.SignInUploaded ?? false)
                {
                    session_upload.SignInFullPath = su.SignInFullPath;
                    session_upload.SignInUploaded = su.SignInUploaded;
                    session_upload.SignInFileName = su.SignInFileName;
                    session_upload.SignInFileExt = su.SignInFileExt;
                }
                //userother
                if (su.UserOtherUploaded ?? false)
                {
                    session_upload.UserOtherFullPath = su.UserOtherFullPath;
                    session_upload.UserOtherUploaded = su.UserOtherUploaded;
                    session_upload.UserOtherFileName = su.UserOtherFileName;
                    session_upload.UserOtherFileExt = su.UserOtherFileExt;
                }


                if (su.SpeakerAgreementUploaded ?? false)
                {

                    session_upload.SpeakerAgreementFileExt = su.SpeakerAgreementFileExt;
                    session_upload.SpeakerAgreementFileName = su.SpeakerAgreementFileName;
                    session_upload.SpeakerAgreementFullPath = su.SpeakerAgreementFullPath;
                    session_upload.SpeakerAgreementUploaded = su.SpeakerAgreementUploaded;
                }


                session_upload.LastUpdated = DateTime.Now;



                Entities.SaveChanges();

            }
        }
        
        public List<CompletedProgram> GetUpcomingProgramsByuserid(int CompletedProgramCount, int Speakeruserid)
        {
            List<CompletedProgram> liCompletedProgram = null;

            try
            {


                liCompletedProgram = Entities.ProgramRequests.Where(u => u.ProgramSpeakerID == Speakeruserid && (u.RequestStatus == 1 || u.RequestStatus==2 || u.RequestStatus==3)  && u.ConfirmedSessionDate != null).
                Select(u => new CompletedProgram
                {

                    ProgramName = u.Program.ProgramName,
                    LocationName = u.LocationName,

                    ConfirmedSessionDate = u.ConfirmedSessionDate == null ? null : SqlFunctions.DateName("year", u.ConfirmedSessionDate) + "/" + SqlFunctions.DatePart("m", u.ConfirmedSessionDate) + "/" + SqlFunctions.DateName("day", u.ConfirmedSessionDate),


                }).ToList();


                return liCompletedProgram;
            }
            catch (Exception e)
            {
                return liCompletedProgram;
            }
        }
        public List<CompletedProgram> GetCompletedProgramsByuserid(int CompletedProgramCount, int Speakeruserid)
        {
            List<CompletedProgram> liCompletedProgram = null;

            try {

               
                liCompletedProgram = Entities.ProgramRequests.Where(u => u.ProgramSpeakerID == Speakeruserid && u.RequestStatus == 4).
                Select(u => new CompletedProgram
                {

                    ProgramName = u.Program.ProgramName,
                    LocationName = u.LocationName,
                  
                    ConfirmedSessionDate = u.ConfirmedSessionDate==null ? null : SqlFunctions.DateName("year", u.ConfirmedSessionDate) + "/" + SqlFunctions.DatePart("m", u.ConfirmedSessionDate) + "/" + SqlFunctions.DateName("day", u.ConfirmedSessionDate),


                }).ToList();


                return liCompletedProgram;
            }catch(Exception e)
            {
                return liCompletedProgram;
            }
        }

        public  ProgramRequestStatusCount GetProgramRequestStatusCounts(int ProgramID)
        {
            ProgramRequestStatusCount prsc = new ProgramRequestStatusCount();
            if (UserHelper.GetLoggedInUser() != null)
            {
                int UserID = UserHelper.GetLoggedInUser().UserID;
                int SponsorID = UserHelper.GetLoggedInUser().SponsorID;

                string UserRole = UserHelper.GetRoleByUserID(UserID);
                int TotalActive, TotalAttention, TotalCancelled, TotalCompleted;
                int TotalProgramRequest;

              

                    //select all programrequest for a program under a sponsor and status not "Under Review"
                    TotalProgramRequest = Entities.ProgramRequests.Count(t => t.SponsorID == SponsorID && t.ProgramID == ProgramID && t.RequestStatus != 1 && t.UserID==UserID );
                    TotalActive = Entities.ProgramRequests.Count(t => t.SponsorID == SponsorID && t.ProgramID == ProgramID && (t.RequestStatus == 2 || t.RequestStatus == 3) && t.UserID == UserID);
                    TotalAttention = Entities.ProgramRequests.Count(t => t.SponsorID == SponsorID && t.ProgramID == ProgramID && (t.RequestStatus == 5 || t.RequestStatus == 7) && t.UserID == UserID);
                    TotalCancelled = Entities.ProgramRequests.Count(t => t.SponsorID == SponsorID && t.ProgramID == ProgramID && t.RequestStatus == 6 && t.UserID == UserID);
                    TotalCompleted = Entities.ProgramRequests.Count(t => t.SponsorID == SponsorID && t.ProgramID == ProgramID && t.RequestStatus == 4 && t.UserID == UserID);

              
                if (TotalProgramRequest > 0)
                {
                    prsc.Percent_Active = Convert.ToInt32(((decimal)TotalActive / TotalProgramRequest) * 100);
                    prsc.Percent_Attention = Convert.ToInt32(((decimal)TotalAttention / TotalProgramRequest) * 100);
                    prsc.Percent_Cancelled = Convert.ToInt32(((decimal)TotalCancelled / TotalProgramRequest) * 100);
                    prsc.Percent_Completed = Convert.ToInt32(((decimal)TotalCompleted / TotalProgramRequest) * 100);
                }else
                {
                    prsc.Percent_Active = 0;
                    prsc.Percent_Attention = 0;
                    prsc.Percent_Cancelled = 0;
                    prsc.Percent_Completed = 0;

                }
                return prsc;


            }
            else
            {

                return prsc;
            }
        }

        public ProgramSessionPayment GetProgramSessionPayment(int ProgramRequestID)
        {//usertype 2 for speaker
            //usertype 3 for moderator
            try
            {
                if (ProgramRequestID != 0)
                {
                    ProgramSessionPayment psu;
                    psu = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).Select(pr =>
                             new ProgramSessionPayment
                             {

                                 ProgramRequestID = pr.ProgramRequestID,
                                 ProgramSpeakerID = pr.ProgramSpeakerID ?? 0,
                                 ProgramModeratorID = pr.ProgramModeratorID ?? 0,
                                 SpeakerFirstName = pr.SpeakerInfo.FirstName,
                                 SpeakerLastName = pr.SpeakerInfo.LastName,
                                 ModeratorFirstName = pr.ModeratorInfo.FirstName,
                                 ModeratorLastName = pr.ModeratorInfo.LastName,
                                 SpeakerPaymentAmount = pr.AdminSpeakerHonorium.ToString(),
                                 SpeakerPaymentSentDate = (pr.AdminSpeakerPaymentSentDate == null) ? null : SqlFunctions.DateName("year", pr.AdminSpeakerPaymentSentDate) + "/" + SqlFunctions.DatePart("m", pr.AdminSpeakerPaymentSentDate) + "/" + SqlFunctions.DateName("day", pr.AdminSpeakerPaymentSentDate),
                                 ModeratorPaymentAmount = pr.AdminModeratorHonorium.ToString(),
                                 ModeratorPaymentSentDate = (pr.AdminModeratorPaymentSentDate == null) ? null : SqlFunctions.DateName("year", pr.AdminModeratorPaymentSentDate) + "/" + SqlFunctions.DatePart("m", pr.AdminModeratorPaymentSentDate) + "/" + SqlFunctions.DateName("day", pr.AdminModeratorPaymentSentDate),
                                 VenueFees = pr.AdminVenueFees.ToString(),
                                 OtherFees = pr.AdminOtherFees.ToString(),
                                 AVFees = pr.AdminAVFees.ToString(),




                             }).SingleOrDefault();

                    return psu;
                }
                else
                    return null;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
                return null;
            }

        }

        public ProgramRequestCancellationVM GetProgramRequestCancellationbyID(int ProgramRequestId)
        {
            var CurrentUser = UserHelper.GetLoggedInUser();
            ProgramRequestCancellationVM pr = new ProgramRequestCancellationVM();
            if (CurrentUser != null)
            {



                var objPr = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestId).SingleOrDefault();

                if (objPr != null)
                {
                    pr.ContactName = CurrentUser.FirstName + ", " + CurrentUser.LastName;
                    pr.ContactFirstName = CurrentUser.FirstName;
                    pr.ContactLastName = CurrentUser.LastName;
                    pr.ContactEmail = CurrentUser.Username;
                    pr.ContactPhone = CurrentUser.Phone;
                    pr.ProgramRequestID = objPr.ProgramRequestID;
                    pr.ProgramID = objPr.ProgramID ?? 0;


                    pr.ProgramSpeakerID = objPr.ProgramSpeakerID ?? 0;
                    pr.ProgramModeratorID = objPr.ProgramModeratorID ?? 0;


                    pr.Comments = objPr.Comments;

                    pr.LocationAddress = objPr.LocationAddress;
                    pr.LocationCity = objPr.LocationCity;
                    pr.LocationName = objPr.LocationName;
                    pr.LocationPhoneNumber = objPr.LocationPhoneNumber;
                    pr.LocationProvince = objPr.LocationProvince;
                    pr.LocationType = objPr.LocationType;
                    pr.LocationWebsite = objPr.LocationWebsite;



                    /* added to support sessioncancellation */
                    pr.CancellationReason = objPr.CancellationReason;
                    pr.CancellationRequested = objPr.CancellationRequested;
                    pr.ConfirmedSessionDate = objPr.ConfirmedSessionDate.HasValue ? objPr.ConfirmedSessionDate.Value.ToString("yyyy-MM-dd") : "";
                    pr.SpeakerName = objPr.ProgramSpeakerID.HasValue ? objPr.SpeakerInfo.FirstName + "," + objPr.SpeakerInfo.LastName : "";
                    pr.ModeratorName = objPr.ProgramModeratorID.HasValue ? objPr.ModeratorInfo.FirstName + "," + objPr.ModeratorInfo.LastName : "";
                    /*end of session cancellation*/



                    pr.RequestStatus = (objPr.RequestStatus.HasValue ? objPr.RequestStatus.Value : 0).ToString();



                }



            }

            return pr;


        }

        public ProgramRequestModifyVM GetProgramRequestModifybyID(int ProgramRequestId)
        {
            var CurrentUser = UserHelper.GetLoggedInUser();
            ProgramRequestModifyVM pr = new ProgramRequestModifyVM();
            if (CurrentUser != null)
            {



                var objPr = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestId).SingleOrDefault();

                if (objPr != null)
                {
                    pr.ContactName = CurrentUser.FirstName + ", " + CurrentUser.LastName;
                    pr.ContactFirstName = CurrentUser.FirstName;
                    pr.ContactLastName = CurrentUser.LastName;
                    pr.ContactEmail = CurrentUser.Username;
                    pr.ContactPhone = CurrentUser.Phone;
                    pr.ProgramRequestID = objPr.ProgramRequestID;
                    pr.ProgramID = objPr.ProgramID ?? 0;


                    pr.ProgramSpeakerID = objPr.ProgramSpeakerID ?? 0;
                    pr.ProgramModeratorID = objPr.ProgramModeratorID ?? 0;


                    pr.Comments = objPr.Comments;

                    pr.LocationAddress = objPr.LocationAddress;
                    pr.LocationCity = objPr.LocationCity;
                    pr.LocationName = objPr.LocationName;
                    pr.LocationPhoneNumber = objPr.LocationPhoneNumber;
                    pr.LocationProvince = objPr.LocationProvince;
                    pr.LocationType = objPr.LocationType;
                    pr.LocationWebsite = objPr.LocationWebsite;



                    /* added to support sessioncancellation */
                    pr.ModifyReason = objPr.ModifyReason;
                    pr.ModifyRequested = objPr.ModifyRequested;
                    pr.ConfirmedSessionDate = objPr.ConfirmedSessionDate.HasValue ? objPr.ConfirmedSessionDate.Value.ToString("yyyy-MM-dd") : "";
                    pr.SpeakerName = objPr.ProgramSpeakerID.HasValue ? objPr.SpeakerInfo.FirstName + "," + objPr.SpeakerInfo.LastName : "";
                    pr.ModeratorName = objPr.ProgramModeratorID.HasValue ? objPr.ModeratorInfo.FirstName + "," + objPr.ModeratorInfo.LastName : "";
                    /*end of session cancellation*/



                    pr.RequestStatus = (objPr.RequestStatus.HasValue ? objPr.RequestStatus.Value : 0).ToString();



                }



            }

            return pr;


        }
        public void CancelProgramRequest(ProgramRequestCancellationVM pr)
        {

            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == pr.ProgramRequestID).SingleOrDefault();
            if (val != null)
            {

                val.CancellationRequested = true;
                val.CancellationReason = pr.CancellationReason;
                Entities.SaveChanges();

            }
        }
        public void ModifyProgramRequest(ProgramRequestModifyVM pr)
        {

            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == pr.ProgramRequestID).SingleOrDefault();
            if (val != null)
            {

                val.ModifyRequested = true;
                val.ModifyReason = pr.ModifyReason;
                Entities.SaveChanges();

            }
        }
        public void ApproveProgramRequest(ProgramRequestViewModel pr)
        {

            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == pr.ProgramRequestID).SingleOrDefault();
            if (val != null)
            {

                val.Approved = pr.Approved;
                Entities.SaveChanges();

            }
        }
        public void DeleteUser(UserModel um)
        {

            var val = Entities.Users.Where(x => x.UserID == um.UserID).SingleOrDefault();
            if (val != null)
            {

                val.Deleted = um.Deleted;
                Entities.SaveChanges();

            }
        }
        public void UpdateProgramRequestStatus(ProgramRequestViewModel pr)
        {

            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == pr.ProgramRequestID).SingleOrDefault();
            if (val != null)
            {

                val.RequestStatus = pr.RequestStatus;
                Entities.SaveChanges();

            }
        }

        public void EditProgramRequest(ProgramRequestViewModel pr)
        {
            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == pr.ProgramRequestID).SingleOrDefault();
            if (val != null)
            {

                val.ContactFirstName = pr.ContactFirstName;
                val.ContactLastName = pr.ContactLastName;
                val.ContactEmail = pr.ContactEmail;
                val.ContactPhone = pr.ContactPhone;
                val.ProgramDate1 = Convert.ToDateTime(pr.ProgramDate1);
                val.ProgramStartTime = Convert.ToDateTime(pr.ProgramStartTime);
                val.ProgramEndTime = Convert.ToDateTime(pr.ProgramEndTime);
                val.SubmittedDate = Convert.ToDateTime(pr.SubmittedDate);
                val.RequestStatus = Convert.ToInt32(pr.RequestStatus);
                val.Approved = pr.Approved;

                Entities.SaveChanges();

            }

        }

        public ProgramRequestViewModel GetEditProgramRequestByID(int ProgramRequestID)
        {

            if (ProgramRequestID != 0)
            {
                ProgramRequestViewModel pr;
                pr = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).Select(ui =>
                         new ProgramRequestViewModel
                         {
                             ProgramRequestID = ProgramRequestID,
                             ContactFirstName = ui.ContactFirstName,
                             ContactLastName = ui.ContactLastName,
                             ContactEmail = ui.ContactEmail,
                             ContactPhone = ui.ContactPhone,
                             ProgramDate1 = ui.ProgramDate1.HasValue ? ui.ProgramDate1.Value.ToString() : "",
                             ProgramStartTime = ui.ProgramStartTime.HasValue ? ui.ProgramStartTime.Value.ToString() : "",
                             ProgramEndTime = ui.ProgramEndTime.HasValue ? ui.ProgramEndTime.Value.ToString() : "",
                             SubmittedDate = ui.SubmittedDate.HasValue ? ui.SubmittedDate.Value.ToString() : "",
                             RequestStatus = ui.RequestStatus ?? 0,
                             Approved = ui.Approved.HasValue ? ui.Approved.Value : false


                         }).SingleOrDefault();

                return pr;
            }
            else
                return null;
        }
        public EventModule GetProgramModulesByProgramRequestID(int ProgramRequestID)
        {
            EventModule EventModule=null;
            int SessionCreditID = 0;
            int ModuleLimit = 0;
            string SessionCredit="";
            decimal SessionCreditValue = 0.0M;
            if (ProgramRequestID != 0)
            {
                try
                {

                    var prsc = Entities.ProgramRequestSessionCredits.Where(u => u.ProgramRequestID == ProgramRequestID).FirstOrDefault();
                    if (prsc != null)
                    {
                        SessionCreditID = prsc.SessionCreditID ?? 0;
                        if (SessionCreditID == 16)
                            ModuleLimit = 3;
                        if (SessionCreditID == 17)
                            ModuleLimit = 5;
                        if (SessionCreditID == 18)
                            ModuleLimit = 7;
                        if (SessionCreditID == 19)
                            ModuleLimit = 10;
                        var obj = Entities.SessionCreditLookUps.Where(u => u.SessionCreditID == SessionCreditID).FirstOrDefault();
                        SessionCredit = obj.Description;
                        SessionCreditValue = obj.Value??0.0M;

                    }

                    EventModule = Entities.ProgramRequestModules.Where(u => u.ProgramRequestID == ProgramRequestID).
                    Select(u => new EventModule
                    {
                        ProgramRequestID = ProgramRequestID,
                        SessionCreditID = SessionCreditID,
                        SessionCredit=SessionCredit,
                        SessionCreditValue= SessionCreditValue,
                        ModuleLimit=ModuleLimit,
                        ProgramModule1 = u.ProgramModule1 ?? false,
                        ProgramModule2 = u.ProgramModule2 ?? false,
                        ProgramModule3 = u.ProgramModule3 ?? false,
                        ProgramModule4 = u.ProgramModule4 ?? false,
                        ProgramModule5 = u.ProgramModule5 ?? false,
                        ProgramModule6 = u.ProgramModule6 ?? false,
                        ProgramModule7 = u.ProgramModule7 ?? false,
                        ProgramModule8 = u.ProgramModule8 ?? false,
                        ProgramModule9 = u.ProgramModule9 ?? false,
                        ProgramModule10 = u.ProgramModule10 ?? false



                    }).SingleOrDefault();
                    //first time selecting modules
                    if (EventModule == null)
                    {
                        EventModule em = new EventModule();
                        em.ProgramRequestID = ProgramRequestID;
                        em.SessionCreditID = SessionCreditID;
                        em.SessionCredit = SessionCredit;
                        em.SessionCreditValue = SessionCreditValue;
                        em.ModuleLimit = ModuleLimit;
                        return em;
                    }

                    return EventModule;
                }
                catch (Exception e)
                {
                    return EventModule;
                }
            }
            else
                return null;
           
        }
        //public ProgramRequestStatusCount GetProgramRequestStatusCounts(int ProgramID)
        // {
        //     ProgramRequestStatusCount prsc = new ProgramRequestStatusCount();
        //     if (UserHelper.GetLoggedInUser() != null)
        //     {
        //         int UserID = UserHelper.GetLoggedInUser().UserID;
        //         int SponsorID = UserHelper.GetLoggedInUser().SponsorID;

        //         string UserRole = UserHelper.GetRoleByUserID(UserID);
        //         int TotalActive, TotalAttention, TotalCancelled, TotalCompleted;
        //         int TotalProgramRequest;

        //         if (UserRole == Util.Constants.HeadOffice || UserRole == Util.Constants.SalesDirector)//see everything
        //         {

        //             //select all programrequest for a program under a sponsor and status not "Under Review"
        //             TotalProgramRequest = Entities.ProgramRequests.Count(t => t.SponsorID == SponsorID && t.ProgramID == ProgramID && t.RequestStatus != 1);
        //             TotalActive = Entities.ProgramRequests.Count(t => t.SponsorID == SponsorID && t.ProgramID == ProgramID && (t.RequestStatus == 2 || t.RequestStatus == 3));
        //             TotalAttention = Entities.ProgramRequests.Count(t => t.SponsorID == SponsorID && t.ProgramID == ProgramID && (t.RequestStatus == 5 || t.RequestStatus == 7));
        //             TotalCancelled = Entities.ProgramRequests.Count(t => t.SponsorID == SponsorID && t.ProgramID == ProgramID && t.RequestStatus == 6);
        //             TotalCompleted = Entities.ProgramRequests.Count(t => t.SponsorID == SponsorID && t.ProgramID == ProgramID && t.RequestStatus == 4);

        //         }
        //         else if (UserRole == Util.Constants.RegionalManager)//manager can see all user in the same territory
        //         {
        //             var TerritoryID = (from ut in Entities.UserTerritories where ut.UserID == UserID select ut.TerritoryID).FirstOrDefault();
        //             //all userids under territoryID 41
        //             var TerritorialUserIDs = (from ut in Entities.UserTerritories where ut.TerritoryID == TerritoryID select ut.UserID).ToList();

        //             TotalProgramRequest = Entities.ProgramRequests.Count(t => TerritorialUserIDs.Contains(t.UserID) && t.ProgramID == ProgramID && t.RequestStatus != 1);
        //             TotalActive = Entities.ProgramRequests.Count(t => TerritorialUserIDs.Contains(t.UserID) && t.ProgramID == ProgramID && (t.RequestStatus == 2 || t.RequestStatus == 3));
        //             TotalAttention = Entities.ProgramRequests.Count(t => TerritorialUserIDs.Contains(t.UserID) && t.ProgramID == ProgramID && (t.RequestStatus == 5 || t.RequestStatus == 7));
        //             TotalCancelled = Entities.ProgramRequests.Count(t => TerritorialUserIDs.Contains(t.UserID) && t.ProgramID == ProgramID && t.RequestStatus == 6);
        //             TotalCompleted = Entities.ProgramRequests.Count(t => TerritorialUserIDs.Contains(t.UserID) && t.ProgramID == ProgramID && t.RequestStatus == 4);


        //         }
        //         else //sale rep only get to see he own session request
        //         {
        //             TotalProgramRequest = Entities.ProgramRequests.Count(t => t.UserID == UserID && t.ProgramID == ProgramID && t.RequestStatus != 1);
        //             TotalActive = Entities.ProgramRequests.Count(t => t.UserID == UserID && t.ProgramID == ProgramID && (t.RequestStatus == 2 || t.RequestStatus == 3));
        //             TotalAttention = Entities.ProgramRequests.Count(t => t.UserID == UserID && t.ProgramID == ProgramID && (t.RequestStatus == 5 || t.RequestStatus == 7));
        //             TotalCancelled = Entities.ProgramRequests.Count(t => t.UserID == UserID && t.ProgramID == ProgramID && t.RequestStatus == 6);
        //             TotalCompleted = Entities.ProgramRequests.Count(t => t.UserID == UserID && t.ProgramID == ProgramID && t.RequestStatus == 4);

        //         }

        //         prsc.Percent_Active = Convert.ToInt32(((decimal)TotalActive / TotalProgramRequest) * 100);
        //         prsc.Percent_Attention = Convert.ToInt32(((decimal)TotalAttention / TotalProgramRequest) * 100);
        //         prsc.Percent_Cancelled = Convert.ToInt32(((decimal)TotalCancelled / TotalProgramRequest) * 100);
        //         prsc.Percent_Completed = Convert.ToInt32(((decimal)TotalCompleted / TotalProgramRequest) * 100);
        //         return prsc;


        //     }
        //     else
        //     {

        //         return prsc;
        //     }
        // }



        #endregion End of Andrew Code ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region  ali's code ===================================================================================================================================

        public Models.ProgramRequest GetProgramRequestByID(int ProgramRequestID)
        {
            if (ProgramRequestID != 0)
            {
                Models.ProgramRequest pr;
                pr = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).Select(ui =>
                         new Models.ProgramRequest
                         {
                             ProgramRequestID = ProgramRequestID,
                             ContactFirstName = ui.ContactFirstName,
                             ContactLastName = ui.ContactLastName,
                             ContactEmail = ui.ContactEmail,
                             ContactPhone = ui.ContactPhone,
                             SessionAgendaUploaded = ui.SessionAgendaUploaded,
                             SessionAgendaFileName = ui.SessionAgendaFileName



                         }).SingleOrDefault();

                return pr;
            }
            else
                return null;
        }
        public Models.ProgramRequest InitialProgramRequestForm(int ProgramID)
        {
            var CurrentUser = UserHelper.GetLoggedInUser();

            Models.ProgramRequest pr = new Models.ProgramRequest();
            //they are all in the hidden field, for saving later.
            pr.ContactName = CurrentUser.FirstName + ", " + CurrentUser.LastName;
            pr.ContactFirstName = CurrentUser.FirstName;
            pr.ContactLastName = CurrentUser.LastName;
            pr.ContactEmail = CurrentUser.Username;
            pr.ContactPhone = CurrentUser.Phone;
            pr.SpeakerChosenProgramDate = false;
            pr.ModeratorChosenProgramDate = false;
            pr.ContactPhone = CurrentUser.Phone;
            pr.ProgramRequestID = GetProgramRequestID();
            pr.ProgramID = ProgramID;
            pr.SponsorID = CurrentUser.SponsorID;

            //get TherapeuticID 
            int? TherapeuticID = 0;

            var objProgram = Entities.Programs.Where(x => x.ProgramID == ProgramID).FirstOrDefault();
            if (objProgram != null)
            {

                pr.Archive = objProgram.Archive??false;
                pr.DisplayOrder = objProgram.DisplayOrder??0;
            }
                

            var GetTherapeuticID = Entities.TherapeuticPrograms.Where(x => x.ProgramID == ProgramID).FirstOrDefault();

            if(GetTherapeuticID != null)
            {

                TherapeuticID = GetTherapeuticID.TherapeuticID ?? 0;
            }

            if(TherapeuticID == 1)
            {

                pr.Speakers = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 1) || (x.TherapeuticID == 3)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                }).ToList();
                //get approved moderators only
                pr.Moderators = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 1) || (x.TherapeuticID == 3)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                }).ToList();
            }


            if (TherapeuticID == 2)
            {
                pr.Speakers = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 2) || (x.TherapeuticID == 3)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                }).ToList();
                //get approved moderators only
                pr.Moderators = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 2) || (x.TherapeuticID == 3)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                }).ToList();
            }


            if (TherapeuticID == 3)
            {
                pr.Speakers = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 3)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                }).ToList();
                //get approved moderators only
                pr.Moderators = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 3)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                }).ToList();
            }

            //get approved speakers

            return pr;
        }
        public WebCastViewModel InitialWebCastRequest(int ProgramID)
        {
            var CurrentUser = UserHelper.GetLoggedInUser();

            WebCastViewModel wcvm = new WebCastViewModel();
            //they are all in the hidden field, for saving later.
            wcvm.ContactName = CurrentUser.FirstName + ", " + CurrentUser.LastName;
            wcvm.ContactFirstName = CurrentUser.FirstName;
            wcvm.ContactLastName = CurrentUser.LastName;
            wcvm.ContactEmail = CurrentUser.Username;
            wcvm.ContactPhone = CurrentUser.Phone;
            wcvm.SpeakerChosenProgramDate = false;//have the speaker selected a program date yet?

            wcvm.ContactPhone = CurrentUser.Phone;
            wcvm.ProgramRequestID = GetProgramRequestID();
            wcvm.ProgramID = ProgramID;
            wcvm.SponsorID = CurrentUser.SponsorID;

            //get TherapeuticID 
            //int? TherapeuticID = 0;

            //var objProgram = Entities.Programs.Where(x => x.ProgramID == ProgramID).FirstOrDefault();

            //if (objProgram != null)
            //{

            //    TherapeuticID = objProgram.TherapeuticID;
            //}

            if (ProgramID == 1) //if making webcast request from program 1 ie. New Horizons in Dyslipidemia Management in Primary Care 
            {
                //show all speaker with therapeutic id 1 or 3 (CV bone
                //user type 2,3 = speaker, moderator

                //status 2,3,4 = Approved, Registered Not Complete, RegisteredCompleted  - speaker/moderator should not be included if pending approval 1, or optout 5
                wcvm.Speakers = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 1) || (x.TherapeuticID == 3)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                });
                //get approved moderators only
               
            }


            if (ProgramID == 2)
            {
                wcvm.Speakers = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 2) || (x.TherapeuticID == 3)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                });
               
            }

          //  wcvm.FromQueryStringBySalesRep = true;//this webcast is initiate by sale rep by clicking on the left nav.

            //get approved speakers

            return wcvm;
        }
        public int GetProgramRequestID()
        {
            int ProgramRequestID;
            try
            {


                CPDPortal.Data.ProgramRequest pr = new CPDPortal.Data.ProgramRequest();

                Entities.ProgramRequests.Add(pr);
                Entities.SaveChanges();

                ProgramRequestID = pr.ProgramRequestID;
                return ProgramRequestID;
            }
            catch (Exception e)
            {
                ProgramRequestID = 0;
                return ProgramRequestID;
            }


        }
        public void UpdateEventModule(EventModule em)
        {
            var EventModulesToDelete = Entities.ProgramRequestModules.Where(x => x.ProgramRequestID == em.ProgramRequestID).SingleOrDefault();
            //delete all modules for a programrequest
            if (EventModulesToDelete != null)
            {
              

                    Entities.ProgramRequestModules.Remove(EventModulesToDelete);
                
                Entities.SaveChanges();

            }
            //insert all modules for a programrequest selected by the user
            ProgramRequestModule prm = new ProgramRequestModule();
            prm.ProgramRequestID = em.ProgramRequestID;
            prm.ProgramModule1 = em.ProgramModule1;
            prm.ProgramModule2 = em.ProgramModule2;
            prm.ProgramModule3 = em.ProgramModule3;
            prm.ProgramModule4 = em.ProgramModule4;
            prm.ProgramModule5 = em.ProgramModule5;
            prm.ProgramModule6 = em.ProgramModule6;
            prm.ProgramModule7 = em.ProgramModule7;
            prm.ProgramModule8 = em.ProgramModule8;
            prm.ProgramModule9 = em.ProgramModule9;

            prm.ProgramModule10 = em.ProgramModule10;
            Entities.ProgramRequestModules.Add(prm);
            Entities.SaveChanges();

        }
        public void UpdateSession(Models.ProgramRequestIIModel pr)
        {

            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == pr.ProgramRequestID).SingleOrDefault();
            ProgramRequestSessionCredits credits = new ProgramRequestSessionCredits();

            if (val != null)
            {
                DateTime? dt = null;
                val.ProgramID = pr.ProgramID;            
                val.SponsorID = 1;

                val.ContactFirstName = pr.ContactFirstName;
                val.ContactLastName = pr.ContactLastName;
                val.ContactEmail = pr.ContactEmail;
                val.ContactPhone = pr.ContactPhone;
                val.ProgramDate1 = DateTime.ParseExact(pr.ProgramDate1, "yyyy/MM/dd", null);

                val.ProgramDate2 = !(string.IsNullOrEmpty(pr.ProgramDate2)) ? (DateTime.ParseExact(pr.ProgramDate2, "yyyy/MM/dd", null)) : dt;
                val.ProgramDate3 = !(string.IsNullOrEmpty(pr.ProgramDate3)) ? (DateTime.ParseExact(pr.ProgramDate3, "yyyy/MM/dd", null)) : dt;

                DateTime MealStart = DateTime.ParseExact(pr.ProgramDate1, "yyyy/MM/dd", null);
                if (pr.MealStartTime != null)
                {
                    MealStart = MealStart.Add(TimeSpan.Parse(pr.MealStartTime));
                    val.MealStartTime = MealStart;
                }else
                {
                    val.MealStartTime = null;
                }

                DateTime ProgramStart = DateTime.ParseExact(pr.ProgramDate1, "yyyy/MM/dd", null);
                ProgramStart = ProgramStart.Add(TimeSpan.Parse(pr.ProgramStartTime));
                val.ProgramStartTime = ProgramStart;

                DateTime ProgramEnd = DateTime.ParseExact(pr.ProgramDate1, "yyyy/MM/dd", null);
                ProgramEnd = ProgramEnd.Add(TimeSpan.Parse(pr.ProgramEndTime));
                val.ProgramEndTime = ProgramEnd;


                if (pr.IsAdmin == 1)
                {
                    val.AdminUserID = pr.AdminUserID;
                    val.AdminVenueAvailable = pr.AdminVenueConfirmed;
                    val.AdminEditDate = DateTime.Now;
                    val.AdminEdited = true;

                }
                else
                {
                    val.MultiSession = pr.MultiSession;
                    val.SessionAgendaFileName = pr.SessionAgendaFileName;
                    val.SessionAgendaUploaded = pr.SessionAgendaUploaded;
                }

               
                val.CostPerPerson = Convert.ToDecimal(pr.CostPerPerson);
                val.CostByParticipant = Convert.ToDecimal(pr.CostPerparticipants);
                val.ProgramSpeakerID = pr.ProgramSpeakerID;
                val.ProgramModeratorID = pr.ProgramModeratorID;
                val.VenueContacted = pr.VenueContacted;
                val.LocationType = pr.LocationType;
                val.LocationName = pr.LocationName;
                val.LocationAddress = pr.LocationAddress;
                val.LocationCity = pr.LocationCity;
                val.LocationProvince = pr.LocationProvince;
                val.LocationPhoneNumber = pr.LocationPhoneNumber;
                val.LocationWebsite = pr.LocationWebsite;
                val.MealType = pr.MealType;                
                val.AVEquipment = pr.AVEquipment;

                val.ProgramCostsSplit = pr.ProgramCostsSplit;
                val.AmgenRepName = pr.AmgenRepName;

                val.RequestStatus = 1;
                val.ReadOnly = true;
                val.Comments = pr.Comments;
                val.LastUpdatedDate = DateTime.Now;
                

                if(pr.ProgramID == 5 || pr.ProgramID == 7 ||  pr.ProgramID == 8)
                {
                    val.MealOption = ((ProgramRequestIIModel)pr).MealOption;
                    val.LocationTypeOther = ((ProgramRequestIIModel)pr).LocationTypeOther;
                    val.EventType= ((ProgramRequestIIModel)pr).EventType;
                    val.EventTypeQuestion1 = ((ProgramRequestIIModel)pr).EventTypeQuestion1;
                    val.EventTypeQuestion2 = ((ProgramRequestIIModel)pr).EventTypeQuestion2;
                    val.EventTypeQuestion3 = ((ProgramRequestIIModel)pr).EventTypeQuestion3;
                    val.EventTypeQuestion4 = ((ProgramRequestIIModel)pr).EventTypeQuestion4;
                    val.EventTypeQuestion5 = ((ProgramRequestIIModel)pr).EventTypeQuestion5;


                }
                Entities.SaveChanges();
            }

            var SessionCreditsToDelete = Entities.ProgramRequestSessionCredits.Where(x => x.ProgramRequestID == pr.ProgramRequestID).ToList();

            if (SessionCreditsToDelete != null)
            {
                foreach (var row in SessionCreditsToDelete)
                {

                    Entities.ProgramRequestSessionCredits.Remove(row);
                }
                Entities.SaveChanges();

            }

            if (pr.ProgramID == 1)
            {
                if (pr.SessionCredit1)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 1,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (pr.SessionCredit2)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 2,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (pr.SessionCredit3)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 3,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (pr.SessionCredit4)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 4,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (pr.SessionCredit5)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 5,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }
            }
            else if (pr.ProgramID == 2)//the clinical exchange program is different from New Horizon program in CV
            {
                if (pr.SessionCredit1)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 6,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (pr.SessionCredit2)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 7,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (pr.SessionCredit3)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 8,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (pr.SessionCredit4)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 9,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (pr.SessionCredit5)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 10,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }
                if (pr.SessionCredit6)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 11,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

            }
            else if (pr.ProgramID == 3)//bad to bones
            {//update session credits here if new program is added here
                if (pr.SessionCredit1)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 12,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (pr.SessionCredit2)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 13,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }
            }
            else if (pr.ProgramID == 5)
            {
                if (((ProgramRequestIIModel)pr).SessionCredit14)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 14,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (((ProgramRequestIIModel)pr).SessionCredit15)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 15,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }
            }
            else if (pr.ProgramID == 7)
            {
                if (((ProgramRequestIIModel)pr).SessionCredit16)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 16,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (((ProgramRequestIIModel)pr).SessionCredit17)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 17,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (((ProgramRequestIIModel)pr).SessionCredit18)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 18,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (((ProgramRequestIIModel)pr).SessionCredit19)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 19,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }
            }
            else if (pr.ProgramID == 8)
            {
                if (((ProgramRequestIIModel)pr).SessionCredit20)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 20,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (((ProgramRequestIIModel)pr).SessionCredit21)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 21,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }
            }

            Entities.SaveChanges();

        }
       
        public void SaveNewSession(Models.ProgramRequest pr)
        {

            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == pr.ProgramRequestID).SingleOrDefault();
            ProgramRequestSessionCredits credits = new ProgramRequestSessionCredits();

            if (val != null)
            {
                DateTime? dt = null;
                val.ProgramID = pr.ProgramID;
                val.UserID = pr.UserID;
                val.SponsorID = pr.SponsorID;

                val.ContactFirstName = pr.ContactFirstName;
                val.ContactLastName = pr.ContactLastName;
                val.ContactEmail = pr.ContactEmail;
                val.ContactPhone = pr.ContactPhone;
                if (pr.ProgramDate1 != null)
                {
                    val.ProgramDate1 = DateTime.ParseExact(pr.ProgramDate1, "yyyy/MM/dd", null);
                }
                if (pr.ProgramDate2 != null)
                {
                    val.ProgramDate2 = !(string.IsNullOrEmpty(pr.ProgramDate2)) ? (DateTime.ParseExact(pr.ProgramDate2, "yyyy/MM/dd", null)) : dt;
                }
                if (pr.ProgramDate3 != null)
                {
                    val.ProgramDate3 = !(string.IsNullOrEmpty(pr.ProgramDate3)) ? (DateTime.ParseExact(pr.ProgramDate3, "yyyy/MM/dd", null)) : dt;
                }
                if (pr.ProgramDate1 != null)
                {
                    DateTime MealStart = DateTime.ParseExact(pr.ProgramDate1, "yyyy/MM/dd", null);
                    MealStart = MealStart.Add(TimeSpan.Parse(pr.MealStartTime));
                    val.MealStartTime = MealStart;


                    DateTime ProgramStart = DateTime.ParseExact(pr.ProgramDate1, "yyyy/MM/dd", null);
                    ProgramStart = ProgramStart.Add(TimeSpan.Parse(pr.ProgramStartTime));
                    val.ProgramStartTime = ProgramStart;

                    DateTime ProgramEnd = DateTime.ParseExact(pr.ProgramDate1, "yyyy/MM/dd", null);
                    ProgramEnd = ProgramEnd.Add(TimeSpan.Parse(pr.ProgramEndTime));
                    val.ProgramEndTime = ProgramEnd;
                }
                val.SpeakerChosenProgramDate = pr.SpeakerChosenProgramDate;
                val.ModeratorChosenProgramDate = pr.ModeratorChosenProgramDate;

                val.SessionAgendaFileName = pr.SessionAgendaFileName;
                val.SessionAgendaUploaded = pr.SessionAgendaUploaded;


                val.MultiSession = pr.MultiSession;
                val.CostPerPerson = Convert.ToDecimal(pr.CostPerPerson);
                val.CostByParticipant = Convert.ToDecimal(pr.CostPerparticipants);
                val.ProgramSpeakerID = pr.ProgramSpeakerID;
                val.ProgramModeratorID = pr.ProgramModeratorID;
                val.VenueContacted = pr.VenueContacted;
                val.LocationType = pr.LocationType;
                val.LocationName = pr.LocationName;
                val.LocationAddress = pr.LocationAddress;
                val.LocationCity = pr.LocationCity;
                val.LocationProvince = pr.LocationProvince;
                val.LocationPhoneNumber = pr.LocationPhoneNumber;
                val.LocationWebsite = pr.LocationWebsite;
                val.MealType = pr.MealType;
                val.AVEquipment = pr.AVEquipment;
                val.RequestStatus = 1;
                val.ReadOnly = true;
                val.SpeakerStatus = "No Response";

                if (pr.ProgramModeratorID != null)
                {
                    val.ModeratorStatus = "No Response";

                }
                else
                {
                    val.ModeratorStatus = "Not Applicable";
                }
                val.Comments = pr.Comments;
                val.SubmittedDate = DateTime.Now;
                val.LastUpdatedDate = DateTime.Now;

              



                Entities.SaveChanges();

            }

            var SessionCreditsToDelete = Entities.ProgramRequestSessionCredits.Where(x => x.ProgramRequestID == pr.ProgramRequestID).ToList();

            if (SessionCreditsToDelete != null)
            {
                foreach (var row in SessionCreditsToDelete)
                {

                    Entities.ProgramRequestSessionCredits.Remove(row);
                }
                Entities.SaveChanges();

            }
            if (pr.ProgramID == 1)
            {
                if (pr.SessionCredit1)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 1,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (pr.SessionCredit2)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 2,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (pr.SessionCredit3)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 3,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (pr.SessionCredit4)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 4,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (pr.SessionCredit5)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 5,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }
            }
            else if (pr.ProgramID == 2)//the clinical exchange program is different from New Horizon program in CV
            {
                if (pr.SessionCredit1)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 6,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (pr.SessionCredit2)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 7,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (pr.SessionCredit3)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 8,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (pr.SessionCredit4)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 9,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (pr.SessionCredit5)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 10,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }
                if (pr.SessionCredit6)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 11,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

            }
            else if (pr.ProgramID == 3)//bad to the bone
            {
                if (pr.SessionCredit1)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 12,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (pr.SessionCredit2)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 13,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

            }
            else if (pr.ProgramID == 5)
            {
                if (((ProgramRequestIIModel)pr).SessionCredit14)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 14,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (((ProgramRequestIIModel)pr).SessionCredit15)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 15,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

            }
            else if (pr.ProgramID == 7)
            {
                if (((ProgramRequestIIModel)pr).SessionCredit16)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 16,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (((ProgramRequestIIModel)pr).SessionCredit17)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 17,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (((ProgramRequestIIModel)pr).SessionCredit18)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 18,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (((ProgramRequestIIModel)pr).SessionCredit19)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 19,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }
            }
            else if (pr.ProgramID == 8)
            {
                if (((ProgramRequestIIModel)pr).SessionCredit20)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 20,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (((ProgramRequestIIModel)pr).SessionCredit21)
                {
                    Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
                    {
                        SessionCreditID = 21,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }
            }
            Entities.SaveChanges();

        }

        public string GetProgramRequestName(int id)
        {
            string retVal = "";
            var val = Entities.Programs.Where(x => x.ProgramID == id).SingleOrDefault();
            if (val != null)
            {
                retVal = val.ProgramName;

            }
            return retVal;

        }
        public string GetProgramRequestNamefr(int id)
        {
            string retVal = "";
            var val = Entities.Programs.Where(x => x.ProgramID == id).SingleOrDefault();
            if (val != null)
            {
                retVal = val.ProgramNameFr;

            }
            return retVal;

        }

        public Models.ProgramRequest GetProgramRequestForEmail(int ProgramRequestID)
        {

            Models.ProgramRequest pr;
            pr = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).Select(ui =>
                     new Models.ProgramRequest
                     {
                         ProgramRequestID = ProgramRequestID,
                         ContactFirstName = ui.ContactFirstName,
                         ContactLastName = ui.ContactLastName,
                         ContactEmail = ui.ContactEmail,
                         ContactPhone = ui.ContactPhone,
                         LocationName = ui.LocationName,
                         ProgramSpeakerID = ui.ProgramSpeakerID ?? 0,
                         ProgramModeratorID = ui.ProgramModeratorID ?? 0


                     }).SingleOrDefault();

            return pr;
        }

        public string SessionCredit(Models.ProgramRequest pr)
        {
            string val = "";
            int count = 0;
            if (pr.ProgramID == 1)
            {
                if (pr.SessionCredit1)
                {
                    val += "Core New Horizons Deck -  1 Mainpro + Credits (1 hour) ";
                    count++;
                }

                if (pr.SessionCredit2)
                {
                    if (count > 0)
                    {
                        val += ", " + "Case 1: Diabetes & Cardiovascular Disease - 0.5 Mainpro+ Credits (30 minutes) ";

                    }
                    else
                    {
                        val += " Case 1: Diabetes & Cardiovascular Disease - 0.5 Mainpro+ Credits (30 minutes) ";


                    }

                    count++;

                }

                if (pr.SessionCredit3)
                {
                    if (count > 0)
                    {
                        val += ", " + "Case 2: LDL-C: How Low Do You Go? - 0.5 Mainpro+ Credits (30 minutes) ";

                    }
                    else
                    {
                        val += " Case 2: LDL - C: How Low Do You Go ? - 0.5 Mainpro+ Credits (30 minutes) ";

                    }

                    count++;
                }

                if (pr.SessionCredit4)
                {
                    if (count > 0)
                    {
                        val += ", " + "Case 3: ASCVD with Multiple CV Risk Factors - 0.5 Mainpro+ Credits (30 minutes) ";

                    }
                    else
                    {
                        val += " Case 3: ASCVD with Multiple CV Risk Factors - 0.5 Mainpro+ Credits (30 minutes) ";

                    }
                    count++;
                }

                if (pr.SessionCredit5)
                {
                    if (count > 0)
                    {
                        val += ", " + " Case 4: Familiar Hypercholesterolemia - 0.5 Mainpro+ Credits (30 minutes) ";

                    }
                    else
                    {
                        val += " Case 4: Familiar Hypercholesterolemia - 0.5 Mainpro+ Credits (30 minutes) ";

                    }
                    count++;
                }
            }
            else if (pr.ProgramID==2)
            {
                if (pr.SessionCredit1)
                {
                    val += "The Clinical Exchange: Expert Perspectives on Treating and Managing Patients at High Risk for Fracture -  1 Mainpro + Credits (1 hour) ";
                    count++;
                }

                if (pr.SessionCredit2)
                {
                    if (count > 0)
                    {
                        val += ", " + "Case 1: Risk Assessment and Management of Treatment Naïve PMO Patients - 1 Mainpro+ Credits (1 hour) ";

                    }
                    else
                    {
                        val += " Case 1: Risk Assessment and Management of Treatment Naïve PMO Patients - 1 Mainpro+ Credits (1 hour) ";


                    }

                    count++;

                }

                if (pr.SessionCredit3)
                {
                    if (count > 0)
                    {
                        val += ", " + "Case 2: Risk Assessment & Management of Male OP Patients - 1 Mainpro+ Credits (1 hour)";

                    }
                    else
                    {
                        val += " Case 2: Risk Assessment & Management of Male OP Patients - 1 Mainpro+ Credits (1 hour) ";

                    }

                    count++;
                }

                if (pr.SessionCredit4)
                {
                    if (count > 0)
                    {
                        val += ", " + "Case 3: Medications and Comorbidities Associated with Bone Loss";

                    }
                    else
                    {
                        val += "Case 3: Medications and Comorbidities Associated with Bone Loss ";

                    }
                    count++;
                }

                if (pr.SessionCredit5)
                {
                    if (count > 0)
                    {
                        val += ", " + "Case 4: Benefits of Treatment Persistence and Adherence";

                    }
                    else
                    {
                        val += "Case 4: Benefits of Treatment Persistence and Adherence";

                    }
                    count++;
                }
                if (pr.SessionCredit6)
                {
                    if (count > 0)
                    {
                        val += ", " + "Case 5: Optimization of Long-Term Therapy";

                    }
                    else
                    {
                        val += "Case 5: Optimization of Long-Term Therapy";

                    }
                    count++;
                }

            }


            return val;
        }
       
        public string GetSessionCredit(ProgramRequestIIModel model)
        {
            if (model.SessionCredit14)
            {
                return Entities.SessionCreditLookUps.Where(x => x.SessionCreditID == 14).SingleOrDefault().Description;
            }
            else if (model.SessionCredit15)
            {
                return Entities.SessionCreditLookUps.Where(x => x.SessionCreditID == 15).SingleOrDefault().Description;
            }
            else if (model.SessionCredit16)
            {
                return Entities.SessionCreditLookUps.Where(x => x.SessionCreditID == 16).SingleOrDefault().Description;
            }
            else if (model.SessionCredit17)
            {
                return Entities.SessionCreditLookUps.Where(x => x.SessionCreditID == 17).SingleOrDefault().Description;
            }
            else if (model.SessionCredit18)
            {
                return Entities.SessionCreditLookUps.Where(x => x.SessionCreditID == 18).SingleOrDefault().Description;
            }
            else if (model.SessionCredit19)
            {
                return Entities.SessionCreditLookUps.Where(x => x.SessionCreditID == 19).SingleOrDefault().Description;
            }
            else if (model.SessionCredit20)
            {
                return Entities.SessionCreditLookUps.Where(x => x.SessionCreditID == 20).SingleOrDefault().Description;
            }
            else if (model.SessionCredit21)
            {
                return Entities.SessionCreditLookUps.Where(x => x.SessionCreditID == 21).SingleOrDefault().Description;
            }
            else
                return "";
        }
        public string GetSessionCreditfr(ProgramRequestIIModel model)
        {
            if (model.SessionCredit14)
            {
                return Entities.SessionCreditLookUps.Where(x => x.SessionCreditID == 14).SingleOrDefault().Description;
            }
            else if (model.SessionCredit15)
            {
                return Entities.SessionCreditLookUps.Where(x => x.SessionCreditID == 15).SingleOrDefault().Description;
            }
            else if (model.SessionCredit16)
            {
                return Entities.SessionCreditLookUps.Where(x => x.SessionCreditID == 16).SingleOrDefault().Description;
            }
            else if (model.SessionCredit17)
            {
                return Entities.SessionCreditLookUps.Where(x => x.SessionCreditID == 17).SingleOrDefault().Description;
            }
            else if (model.SessionCredit18)
            {
                return Entities.SessionCreditLookUps.Where(x => x.SessionCreditID == 18).SingleOrDefault().Description;
            }
            else if (model.SessionCredit19)
            {
                return Entities.SessionCreditLookUps.Where(x => x.SessionCreditID == 19).SingleOrDefault().Description;
            }//only session 20 and 21 has french
            else if (model.SessionCredit20)
            {
                return Entities.SessionCreditLookUps.Where(x => x.SessionCreditID == 20).SingleOrDefault().Descriptionfr;
            }
            else if (model.SessionCredit21)
            {
                return Entities.SessionCreditLookUps.Where(x => x.SessionCreditID == 21).SingleOrDefault().Descriptionfr;
            }
            else
                return "";
        }

        public void UpdateSpeakerConfirmDate(int ProgramRequestId, string date)
        {
            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestId).SingleOrDefault();
            if (val != null)
            {

                val.SpeakerConfirmedProgramDate = DateTime.ParseExact(date, "yyyy/MM/dd", null);
                val.SpeakerConfirmationDate = DateTime.Now;
                val.SpeakerChosenProgramDate = true;
                val.SpeakerStatus = "Accepted";
                val.SpeakerProgramDateNA = false;
                Entities.SaveChanges();

            }

        }

        public void UpdateModeratorConfirmDate(int ProgramRequestId, string date)
        {
            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestId).SingleOrDefault();
            if (val != null)
            {
                val.ModeratorConfirmedProgramDate = DateTime.ParseExact(date, "yyyy/MM/dd", null);
                val.ModeratorConfirmationDate = DateTime.Now;
                val.ModeratorChosenProgramDate = true;
                val.ModeratorProgramDateNA = false;
                val.ModeratorStatus = "Accepted";
                val.SpeakerStatus = "Accepted";
                val.ConfirmedSessionDate = DateTime.ParseExact(date, "yyyy/MM/dd", null);
                Entities.SaveChanges();

            }

        }

        public void UpdateSpeakerToNotAvailable(int ProgramRequestId)
        {
            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestId).SingleOrDefault();
            if (val != null)
            {

                val.SpeakerProgramDateNA = true;
                val.SpeakerChosenProgramDate = true;
                val.SpeakerStatus = "Not Available";
                val.ReadOnly = false;
                val.RequestStatus = 7;
                Entities.SaveChanges();

            }

        }


        public void UpdateModeratorToNotAvailable(int ProgramRequestId)
        {
            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestId).SingleOrDefault();
            if (val != null)
            {

                val.ModeratorProgramDateNA = true;
                val.ModeratorChosenProgramDate = true;
                val.ModeratorStatus = "Not Available";
                val.ReadOnly = false;
                val.RequestStatus = 7;
                Entities.SaveChanges();

            }

        }

        //check if speaker already confirmed event date
        public bool CheckIfSpeakerConfirmedEmail(int ProgramRequestId)
        {
            bool retVal = false;

            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestId).SingleOrDefault();
            if (val != null)
            {

                if ((val.SpeakerChosenProgramDate == true))
                {
                    retVal = true;
                }

            }
            return retVal;

        }

        public bool CheckIfModeratorConfirmedEmail(int ProgramRequestId)
        {
            bool retVal = false;

            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestId).SingleOrDefault();
            if (val != null)
            {

                if ((val.ModeratorChosenProgramDate == true))
                {
                    retVal = true;
                }

            }
            return retVal;

        }


        public Models.ProgramRequest GetProgramRequestbyQueryString(int ProgramRequestId)
        {
            var CurrentUser = UserHelper.GetLoggedInUser();
            Models.ProgramRequest pr = new Models.ProgramRequest();
            if (CurrentUser != null)
            {

                List<ProgramRequestSessionCredits> SessionList = new List<ProgramRequestSessionCredits>();

                var objPr = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestId).SingleOrDefault();

                if (objPr != null)
                {
                    pr.ContactName = objPr.ContactFirstName + ", " + objPr.ContactLastName;
                    pr.ContactFirstName = objPr.ContactFirstName;
                    pr.ContactLastName = objPr.ContactLastName;
                    pr.ContactEmail = objPr.ContactEmail;
                    pr.ContactPhone = objPr.ContactPhone;
                    pr.ProgramRequestID = objPr.ProgramRequestID;
                    pr.ProgramID = objPr.ProgramID ?? 0;
                    pr.UserID = objPr.UserID ?? 0;
                    pr.SponsorID = objPr.SponsorID ?? 0;

                    int? TherapeuticID = 0;

                    var GetTherapeuticID = Entities.TherapeuticPrograms.Where(x => x.ProgramID == objPr.ProgramID).FirstOrDefault();

                    if (GetTherapeuticID != null)
                    {

                        TherapeuticID = GetTherapeuticID.TherapeuticID ?? 0;
                    }

                    if (TherapeuticID == 1)
                    {

                        pr.Speakers = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 1) || (x.TherapeuticID == 3)))).Select(c => new SelectListItem
                        {
                            Value = c.id.ToString(),
                            Text = c.FirstName + " " + c.LastName

                        }).ToList();
                        //get approved moderators only
                        pr.Moderators = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 1) || (x.TherapeuticID == 3)))).Select(c => new SelectListItem
                        {
                            Value = c.id.ToString(),
                            Text = c.FirstName + " " + c.LastName

                        }).ToList();
                    }


                    if (TherapeuticID == 2)
                    {
                        pr.Speakers = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 2) || (x.TherapeuticID == 3)))).Select(c => new SelectListItem
                        {
                            Value = c.id.ToString(),
                            Text = c.FirstName + " " + c.LastName

                        }).ToList();
                        //get approved moderators only
                        pr.Moderators = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 2) || (x.TherapeuticID == 3)))).Select(c => new SelectListItem
                        {
                            Value = c.id.ToString(),
                            Text = c.FirstName + " " + c.LastName

                        }).ToList();
                    }


                    if (TherapeuticID == 3)
                    {
                        pr.Speakers = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 3)))).Select(c => new SelectListItem
                        {
                            Value = c.id.ToString(),
                            Text = c.FirstName + " " + c.LastName

                        }).ToList();
                        //get approved moderators only
                        pr.Moderators = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 3)))).Select(c => new SelectListItem
                        {
                            Value = c.id.ToString(),
                            Text = c.FirstName + " " + c.LastName

                        }).ToList();
                    }
                    var objProgram = Entities.Programs.Where(x => x.ProgramID == objPr.ProgramID).FirstOrDefault();

                    if (objProgram != null  && objProgram.ProgramID_CHRC != null)
                    {

                        pr.ProgramID_CHRC = objProgram.ProgramID_CHRC.ToString();
                    }
                    pr.ProgramSpeakerID = objPr.ProgramSpeakerID ?? 0;
                    pr.ProgramModeratorID = objPr.ProgramModeratorID ?? 0;

                    pr.AVEquipment = objPr.AVEquipment;
                    pr.Comments = objPr.Comments;
                    pr.CostPerparticipants = objPr.CostByParticipant ?? 0;
                    pr.CostPerPerson = objPr.CostPerPerson ?? 0;
                    pr.LocationAddress = objPr.LocationAddress;
                    pr.LocationCity = objPr.LocationCity;
                    pr.LocationName = objPr.LocationName;
                    pr.LocationPhoneNumber = objPr.LocationPhoneNumber;
                    pr.LocationProvince = objPr.LocationProvince;
                    pr.LocationProvinceFullName = UserHelper.GetProvinceFullName(pr.LocationProvince);
                    pr.LocationType = objPr.LocationType;
                    pr.LocationWebsite = objPr.LocationWebsite;
                    pr.MealStartTime = objPr.MealStartTime.HasValue ? objPr.MealStartTime.Value.ToString("HH:mm") : "";
                    pr.MealType = objPr.MealType;
                    pr.MultiSession = objPr.MultiSession;

                    pr.ProgramDate1 = objPr.ProgramDate1.HasValue ? objPr.ProgramDate1.Value.ToString("yyyy-MM-dd") : "";
                    pr.ProgramDate2 = objPr.ProgramDate2.HasValue ? objPr.ProgramDate2.Value.ToString("yyyy-MM-dd") : "";
                    pr.ProgramDate3 = objPr.ProgramDate3.HasValue ? objPr.ProgramDate3.Value.ToString("yyyy-MM-dd") : "";
                    pr.ProgramEndTime = objPr.ProgramEndTime.HasValue ? objPr.ProgramEndTime.Value.ToString("HH:mm") : "";
                    pr.ProgramStartTime = objPr.ProgramStartTime.HasValue ? objPr.ProgramStartTime.Value.ToString("HH:mm") : "";
                    
                    pr.SessionAgendaFileName = objPr.SessionAgendaFileName;

                    ////update session credits here if new program is added here
                    SessionList = GetSessionCredits(objPr.ProgramRequestID);
                    pr.TotalCredits = 0;
                    foreach (var item in SessionList)
                    {
                        if (item.id==14)//if 1 Mainpro + Credits (1 hour) Core Deck: Dyslipidemia in Primary Care 1 credit
                        {
                            pr.TotalCredits = pr.TotalCredits + 1;

                        }
                        if (item.id == 15)//1 Mainpro + Credits (1 hour) Core Deck: Dyslipidemia in Primary Care1.5 Mainpro + Credits Core Deck: Clinicians' Corner 1.5 credit
                        { 
                            pr.TotalCredits = pr.TotalCredits + 1.5M;

                        }
                        if (item.id == 16)//1 Mainpro + Credits (1 hour) Core Deck: Dyslipidemia in Primary Care1.5 Mainpro + Credits Core Deck: Clinicians' Corner 1.5 credit
                        {
                            pr.TotalCredits = pr.TotalCredits + 1.0M;

                        }
                        if (item.id == 17)//1 Mainpro + Credits (1 hour) Core Deck: Dyslipidemia in Primary Care1.5 Mainpro + Credits Core Deck: Clinicians' Corner 1.5 credit
                        {
                            pr.TotalCredits = pr.TotalCredits + 1.5M;

                        }
                        if (item.id == 18)//1 Mainpro + Credits (1 hour) Core Deck: Dyslipidemia in Primary Care1.5 Mainpro + Credits Core Deck: Clinicians' Corner 1.5 credit
                        {
                            pr.TotalCredits = pr.TotalCredits + 2.0M;

                        }
                        if (item.id == 19)//1 Mainpro + Credits (1 hour) Core Deck: Dyslipidemia in Primary Care1.5 Mainpro + Credits Core Deck: Clinicians' Corner 1.5 credit
                        {
                            pr.TotalCredits = pr.TotalCredits + 2.75M;

                        }
                        if (item.id == 20)//1 Mainpro + Credits (1 hour) Core Deck: Dyslipidemia in Primary Care1.5 Mainpro + Credits Core Deck: Clinicians' Corner 1.5 credit
                        {
                            pr.TotalCredits = pr.TotalCredits + 1.0M;

                        }
                        if (item.id == 21)//1 Mainpro + Credits (1 hour) Core Deck: Dyslipidemia in Primary Care1.5 Mainpro + Credits Core Deck: Clinicians' Corner 1.5 credit
                        {
                            pr.TotalCredits = pr.TotalCredits + 1.5M;

                        }
                        if (item.id == 1 || item.id == 6 || item.id == 12 || item.id == 14)
                        {

                            pr.SessionCredit1 = true;
                        }

                        if (item.id == 2 || item.id == 7 || item.id == 13 || item.id == 15)
                        {

                            pr.SessionCredit2 = true;
                        }

                        if (item.id == 3 || item.id == 8)
                        {

                            pr.SessionCredit3 = true;
                        }

                        if (item.id == 4 || item.id==9)
                        {

                            pr.SessionCredit4 = true;
                        }

                        if (item.id == 5 || item.id==10)
                        {

                            pr.SessionCredit5 = true;
                        }
                        if (item.id==11)
                        {
                            pr.SessionCredit6 = true;
                        }

                    }
                    
                    pr.SessionAgendaUploaded = objPr.SessionAgendaUploaded;
                    pr.RequestStatus = (objPr.RequestStatus.HasValue ? objPr.RequestStatus.Value : 0).ToString();
                    pr.ReadOnly = objPr.ReadOnly.HasValue ? objPr.ReadOnly.Value : false;
                    pr.VenueContacted = objPr.VenueContacted;
                    pr.AdminVenueConfirmed = objPr.AdminVenueAvailable;
                    pr.AdminSessionID = objPr.AdminSessionID;
                    pr.ConfirmedSessionDate= objPr.ConfirmedSessionDate.HasValue ? objPr.ProgramDate1.Value.ToString("yyyy-MM-dd") : "";

                }



            }

            return pr;


        }


        public ProgramRequestIIModel GetProgramRequestII(int ProgramRequestId)
        {
            var CurrentUser = UserHelper.GetLoggedInUser();
            ProgramRequestIIModel pr = new ProgramRequestIIModel();
            if (CurrentUser != null)
            {

                List<ProgramRequestSessionCredits> SessionList = new List<ProgramRequestSessionCredits>();

                var objPr = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestId).SingleOrDefault();

                if (objPr != null)
                {
                    pr.ProgramRequestID = objPr.ProgramRequestID;
                    pr.UserID = objPr.UserID ?? 0;
                    pr.ProgramID = objPr.ProgramID ?? 0;
                    pr.ContactName = objPr.ContactFirstName + " " + objPr.ContactLastName;
                    pr.ContactFirstName = objPr.ContactFirstName;
                    pr.ContactLastName = objPr.ContactLastName;
                    pr.ContactPhone = objPr.ContactPhone;
                    pr.ContactEmail = objPr.ContactEmail;


                    pr.SpeakerStatus = objPr.SpeakerStatus;
                    //pr.Speaker2Status = objPr.Speaker2Status;
                    pr.ModeratorStatus = objPr.ModeratorStatus;
                    pr.SessionAgendaFileName = objPr.SessionAgendaFileName;
                    pr.SessionAgendaUploaded = objPr.SessionAgendaUploaded;

                    pr.ProgramDate1 = objPr.ProgramDate1.HasValue ? objPr.ProgramDate1.Value.ToString("yyyy-MM-dd") : "";
                    pr.ProgramDate2 = objPr.ProgramDate2.HasValue ? objPr.ProgramDate2.Value.ToString("yyyy-MM-dd") : "";
                    pr.ProgramDate3 = objPr.ProgramDate3.HasValue ? objPr.ProgramDate3.Value.ToString("yyyy-MM-dd") : "";
                    pr.MealStartTime = objPr.MealStartTime.HasValue ? objPr.MealStartTime.Value.ToString("HH:mm") : "";
                    pr.ProgramStartTime = objPr.ProgramStartTime.HasValue ? objPr.ProgramStartTime.Value.ToString("HH:mm") : "";
                    pr.ProgramEndTime = objPr.ProgramEndTime.HasValue ? objPr.ProgramEndTime.Value.ToString("HH:mm") : "";

                    pr.ConfirmedSessionDate = objPr.ConfirmedSessionDate.HasValue ? objPr.ConfirmedSessionDate.Value.ToString("yyyy-MM-dd") : "";
                    SessionList = GetSessionCredits(objPr.ProgramRequestID);

                    foreach (var item in SessionList)
                    {
                        switch (item.id)
                        {
                            case 14:
                                pr.SessionCredit14 = true;
                                break;
                            case 15:
                                pr.SessionCredit15 = true;
                                break;
                            case 16:
                                pr.SessionCredit16 = true;
                                break;
                            case 17:
                                pr.SessionCredit17 = true;
                                break;
                            case 18:
                                pr.SessionCredit18 = true;
                                break;
                            case 19:
                                pr.SessionCredit19 = true;
                                break;
                            case 20:
                                pr.SessionCredit20 = true;
                                break;
                            case 21:
                                pr.SessionCredit21 = true;
                                break;
                        }

                    }
                    pr.TotalCredits = 0;
                    foreach (var item in SessionList)
                    {
                        if (item.id == 14)//if 1 Mainpro + Credits (1 hour) Core Deck: Dyslipidemia in Primary Care 1 credit
                        {
                            pr.TotalCredits = pr.TotalCredits + 1;

                        }
                        if (item.id == 15)//1 Mainpro + Credits (1 hour) Core Deck: Dyslipidemia in Primary Care1.5 Mainpro + Credits Core Deck: Clinicians' Corner 1.5 credit
                        {
                            pr.TotalCredits = pr.TotalCredits + 1.5M;

                        }
                        if (item.id == 16)//1 Mainpro + Credits (1 hour) Core Deck: Dyslipidemia in Primary Care1.5 Mainpro + Credits Core Deck: Clinicians' Corner 1.5 credit
                        {
                            pr.TotalCredits = pr.TotalCredits + 1.0M;

                        }
                        if (item.id == 17)//1 Mainpro + Credits (1 hour) Core Deck: Dyslipidemia in Primary Care1.5 Mainpro + Credits Core Deck: Clinicians' Corner 1.5 credit
                        {
                            pr.TotalCredits = pr.TotalCredits + 1.5M;

                        }
                        if (item.id == 18)//1 Mainpro + Credits (1 hour) Core Deck: Dyslipidemia in Primary Care1.5 Mainpro + Credits Core Deck: Clinicians' Corner 1.5 credit
                        {
                            pr.TotalCredits = pr.TotalCredits + 2.0M;

                        }
                        if (item.id == 19)//1 Mainpro + Credits (1 hour) Core Deck: Dyslipidemia in Primary Care1.5 Mainpro + Credits Core Deck: Clinicians' Corner 1.5 credit
                        {
                            pr.TotalCredits = pr.TotalCredits + 2.75M;

                        }
                        if (item.id == 20)//1 Mainpro + Credits (1 hour) Core Deck: Dyslipidemia in Primary Care1.5 Mainpro + Credits Core Deck: Clinicians' Corner 1.5 credit
                        {
                            pr.TotalCredits = pr.TotalCredits + 1.0M;

                        }
                        if (item.id == 21)//1 Mainpro + Credits (1 hour) Core Deck: Dyslipidemia in Primary Care1.5 Mainpro + Credits Core Deck: Clinicians' Corner 1.5 credit
                        {
                            pr.TotalCredits = pr.TotalCredits + 1.5M;

                        }
                        if (item.id == 1 || item.id == 6 || item.id == 12 || item.id == 14)
                        {

                            pr.SessionCredit1 = true;
                        }

                        if (item.id == 2 || item.id == 7 || item.id == 13 || item.id == 15)
                        {

                            pr.SessionCredit2 = true;
                        }

                        if (item.id == 3 || item.id == 8)
                        {

                            pr.SessionCredit3 = true;
                        }

                        if (item.id == 4 || item.id == 9)
                        {

                            pr.SessionCredit4 = true;
                        }

                        if (item.id == 5 || item.id == 10)
                        {

                            pr.SessionCredit5 = true;
                        }
                        if (item.id == 11)
                        {
                            pr.SessionCredit6 = true;
                        }
                    }

                        pr.MultiSession = objPr.MultiSession;
                    pr.ProgramSpeakerID = objPr.ProgramSpeakerID ?? 0;
                    //pr.ProgramSpeaker2ID = objPr.ProgramSpeaker2ID ?? 0;
                    pr.ProgramModeratorID = objPr.ProgramModeratorID ?? 0;

                    pr.Speakers = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)))).OrderBy(x => x.FirstName).Select(c => new SelectListItem
                    {
                        Value = c.id.ToString(),
                        Text = c.FirstName + " " + c.LastName

                    }).ToList();


                    pr.Moderators = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)))).OrderBy(x => x.FirstName).Select(c => new SelectListItem
                    {
                        Value = c.id.ToString(),
                        Text = c.FirstName + " " + c.LastName

                    }).ToList();

                    pr.VenueContacted = objPr.VenueContacted;
                    pr.LocationType = objPr.LocationType;
                    pr.LocationTypeOther = objPr.LocationTypeOther;
                    pr.LocationName = objPr.LocationName;
                    pr.LocationAddress = objPr.LocationAddress;
                    pr.LocationCity = objPr.LocationCity;
                    pr.LocationProvince = objPr.LocationProvince;
                    pr.LocationProvinceFullName = UserHelper.GetProvinceFullName(objPr.LocationProvince);
                    pr.LocationPhoneNumber = objPr.LocationPhoneNumber;
                    pr.LocationWebsite = objPr.LocationWebsite;




                    pr.MealType = objPr.MealType;
                    pr.CostPerPerson = objPr.CostPerPerson ?? 0;
                    pr.CostPerparticipants = objPr.CostByParticipant ?? 0;
                    pr.AVEquipment = objPr.AVEquipment;
                    pr.ProgramCostsSplit=objPr.ProgramCostsSplit;
                    pr.AmgenRepName = objPr.AmgenRepName;
                    pr.Comments = objPr.Comments;
                    pr.RequestStatus = (objPr.RequestStatus.HasValue ? objPr.RequestStatus.Value : 0).ToString();
                    pr.ReadOnly = objPr.ReadOnly.HasValue ? objPr.ReadOnly.Value : false;
                    pr.AdminVenueConfirmed = objPr.AdminVenueAvailable;
                    pr.AdminUserID = objPr.AdminUserID ?? 0;
                    //pr.AdditionalSessionContact = objPr.AdditionalSessionContact;
                    //pr.AdditionalContactName = objPr.AdditionalContactName;
                    //pr.AdditionalContactPhone = objPr.AdditionalContactPhone;
                    //pr.AdditionalContactEmail = objPr.AdditionalContactEmail;
                    pr.EventType = objPr.EventType;
                    pr.EventTypeQuestion1 = objPr.EventTypeQuestion1;
                    pr.EventTypeQuestion2 = objPr.EventTypeQuestion2;
                    pr.EventTypeQuestion3 = objPr.EventTypeQuestion3;
                    pr.EventTypeQuestion4 = objPr.EventTypeQuestion4;
                    pr.EventTypeQuestion5 = objPr.EventTypeQuestion5;
                    pr.RegistrationArrivalTime = objPr.RegistrationArrivalTime.HasValue ? objPr.RegistrationArrivalTime.Value.ToString("HH:mm") : "";

                    pr.MealOption = objPr.MealOption;
                }
            }

            return pr;
        }


        public List<ProgramRequestSessionCredits> GetSessionCredits(int ProgramRequestId)
        {

            //retreieve the SessionCreditID from the ProgramRequestSessionCredits table
            List<ProgramRequestSessionCredits> SessionsList = new List<ProgramRequestSessionCredits>();


            var list = Entities.ProgramRequestSessionCredits.Where(x => x.ProgramRequestID == ProgramRequestId).ToList();

            foreach (var item in list)
            {


                SessionsList.Add(new ProgramRequestSessionCredits
                {
                    id = item.SessionCreditID ?? 0,
                    ProgramRequestID = item.ProgramRequestID ?? 0
                });

            }

            return SessionsList;

        }

        public bool CheckIfProgramRequestExists(int? ProgramRequestID)
        {
            bool val = false;

            val = Entities.ProgramRequests.Any(x => x.ProgramRequestID == ProgramRequestID);

            return val;

        }


        public bool checkifModeratorExist(int ProgramRequestID)
        {
            bool retVal = false;
            int? ModeratorId;

            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).SingleOrDefault();
            if (val != null)
            {
                if (val.ProgramModeratorID == null)
                {
                    retVal = false;
                }
                else
                {
                    ModeratorId = val.ProgramModeratorID;
                    var user = Entities.UserInfoes.Where(x => x.id == ModeratorId).SingleOrDefault();
                    if ((user != null) && (user.id > 0))
                    {
                        retVal = true;
                    }
                }

            }
            return retVal;
        }

        public bool CompareProgramDates(Models.ProgramRequest FromUser)
        {
            DateTime? dt = null;
            bool retval = false;
            var FromDb = Entities.ProgramRequests.Where(x => x.ProgramRequestID == FromUser.ProgramRequestID).SingleOrDefault();
            DateTime ProgramDate1 = DateTime.ParseExact(FromUser.ProgramDate1, "yyyy/MM/dd", null);

            DateTime? ProgramDate2 = !(string.IsNullOrEmpty(FromUser.ProgramDate2)) ? (DateTime.ParseExact(FromUser.ProgramDate2, "yyyy/MM/dd", null)) : dt;
            DateTime? ProgramDate3 = !(string.IsNullOrEmpty(FromUser.ProgramDate3)) ? (DateTime.ParseExact(FromUser.ProgramDate3, "yyyy/MM/dd", null)) : dt;


            if (FromDb != null)
            {

                if ((FromDb.ProgramDate1 != ProgramDate1) || (FromDb.ProgramDate2 != ProgramDate2) || (FromDb.ProgramDate3 != ProgramDate3))
                {
                    retval = true;

                }

            }

            return retval;
        }

        public Models.ProgramRequestIIModel GetProgramRequestForSpeaker(int ProgramRequestId)
        {

            Models.ProgramRequestIIModel pr = new Models.ProgramRequestIIModel();


            List<ProgramRequestSessionCredits> SessionList = new List<ProgramRequestSessionCredits>();

            var objPr = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestId).SingleOrDefault();

            if (objPr != null)
            {
                pr.ContactName = objPr.ContactFirstName + " , " + objPr.ContactLastName;
                pr.ContactFirstName = objPr.ContactFirstName;
                pr.ContactLastName = objPr.ContactLastName;
                pr.ContactEmail = objPr.ContactEmail;
                pr.ContactPhone = objPr.ContactPhone;
                pr.ProgramRequestID = objPr.ProgramRequestID;
                pr.ProgramID = objPr.ProgramID ?? 0;
                pr.UserID = objPr.UserID ?? 0;
                //get approved speakers
                pr.Speakers = Entities.UserInfoes.Where(x => ((x.UserType == 2) || (x.UserType == 3) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                }).ToList();
                //get approved moderators only
                pr.Moderators = Entities.UserInfoes.Where(x => ((x.UserType == 2) || (x.UserType == 3) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                }).ToList();

                pr.ProgramSpeakerID = objPr.ProgramSpeakerID ?? 0;
                pr.ProgramModeratorID = objPr.ProgramModeratorID ?? 0;

                pr.AVEquipment = objPr.AVEquipment;
                pr.Comments = objPr.Comments;
                pr.CostPerparticipants = objPr.CostByParticipant ?? 0;
                pr.CostPerPerson = objPr.CostPerPerson ?? 0;
                pr.LocationAddress = objPr.LocationAddress;
                pr.LocationCity = objPr.LocationCity;
                pr.LocationName = objPr.LocationName;
                pr.LocationPhoneNumber = objPr.LocationPhoneNumber;
                pr.LocationProvince = objPr.LocationProvince;
                pr.LocationType = objPr.LocationType;
                pr.LocationWebsite = objPr.LocationWebsite;
                pr.MealStartTime = objPr.MealStartTime.HasValue ? objPr.MealStartTime.Value.ToString("HH:mm") : "";
                pr.MealType = objPr.MealType;
                pr.MultiSession = objPr.MultiSession;

                pr.ProgramDate1 = objPr.ProgramDate1.HasValue ? objPr.ProgramDate1.Value.ToString("yyyy-MM-dd") : "";
                pr.ProgramDate2 = objPr.ProgramDate2.HasValue ? objPr.ProgramDate2.Value.ToString("yyyy-MM-dd") : "";
                pr.ProgramDate3 = objPr.ProgramDate3.HasValue ? objPr.ProgramDate3.Value.ToString("yyyy-MM-dd") : "";
                pr.ProgramEndTime = objPr.ProgramEndTime.HasValue ? objPr.ProgramEndTime.Value.ToString("HH:mm") : "";
                pr.ProgramStartTime = objPr.ProgramStartTime.HasValue ? objPr.ProgramStartTime.Value.ToString("HH:mm") : "";
                pr.SessionAgendaFileName = objPr.SessionAgendaFileName;

                SessionList = GetSessionCredits(objPr.ProgramRequestID);

                foreach (var item in SessionList)
                {
                    if (item.id == 1)
                    {

                        pr.SessionCredit1 = true;
                    }

                    if (item.id == 2)
                    {

                        pr.SessionCredit2 = true;
                    }

                    if (item.id == 3)
                    {

                        pr.SessionCredit3 = true;
                    }

                    if (item.id == 4)
                    {

                        pr.SessionCredit4 = true;
                    }

                    if (item.id == 5)
                    {

                        pr.SessionCredit5 = true;
                    }
                    if (item.id == 20)
                    {

                        pr.SessionCredit20 = true;
                    }
                    if (item.id == 21)
                    {

                        pr.SessionCredit21 = true;
                    }

                }

                pr.SessionAgendaUploaded = objPr.SessionAgendaUploaded;
                pr.RequestStatus = (objPr.RequestStatus.HasValue ? objPr.RequestStatus.Value : 0).ToString();
                pr.ReadOnly = objPr.ReadOnly.HasValue ? objPr.ReadOnly.Value : false;
                pr.VenueContacted = objPr.VenueContacted;

            }

            return pr;


        }

        public WebCastViewModel PopulateSpeakerDropdown(int ProgramID)
        {

            WebCastViewModel wcvm = new WebCastViewModel();
            int? TherapeuticID = 0;

            var objProgram = Entities.Programs.Where(x => x.ProgramID == ProgramID).FirstOrDefault();

            if (objProgram != null)
            {

                TherapeuticID = objProgram.TherapeuticID ?? 0;
            }

            if (TherapeuticID == 1)
            {

                wcvm.Speakers = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 1) || (x.TherapeuticID == 3)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                });
              
            }


            if (TherapeuticID == 2)
            {
                wcvm.Speakers = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 2) || (x.TherapeuticID == 3)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                });
                //get approved moderators only
              
            }


            if (TherapeuticID == 3)
            {
                wcvm.Speakers = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 3)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                });
               
            }
            return wcvm;


        }
        public Models.ProgramRequest PopulateSpeakerModeratorDropdowns(int ProgramID)
        {

            Models.ProgramRequest pr = new Models.ProgramRequest();
            int? TherapeuticID = 0;

            var GetTherapeuticID = Entities.TherapeuticPrograms.Where(x => x.ProgramID == ProgramID).FirstOrDefault();

            if (GetTherapeuticID != null)
            {

                TherapeuticID = GetTherapeuticID.TherapeuticID ?? 0;
            }

            if (TherapeuticID == 1)
            {

                pr.Speakers = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 1) || (x.TherapeuticID == 3)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                }).ToList();
                //get approved moderators only
                pr.Moderators = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 1) || (x.TherapeuticID == 3)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                }).ToList();
            }


            if (TherapeuticID == 2)
            {
                pr.Speakers = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 2) || (x.TherapeuticID == 3)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                }).ToList();
                //get approved moderators only
                pr.Moderators = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 2) || (x.TherapeuticID == 3)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                }).ToList();
            }


            if (TherapeuticID == 3)
            {
                pr.Speakers = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 3)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                }).ToList();
                //get approved moderators only
                pr.Moderators = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)) && ((x.TherapeuticID == 3)))).Select(c => new SelectListItem
                {
                    Value = c.id.ToString(),
                    Text = c.FirstName + " " + c.LastName

                }).ToList();
            }
            return pr;


        }
        public bool CheckIfModeratorChanges(Models.ProgramRequest FromUser)
        {
            int? ModeratorId = null;
            bool retval = false;
            var FromDb = Entities.ProgramRequests.Where(x => x.ProgramRequestID == FromUser.ProgramRequestID).SingleOrDefault();

            ModeratorId = FromUser.ProgramModeratorID;
            if (FromDb != null)
            {
                if ((FromDb.ProgramModeratorID != ModeratorId))
                {
                    retval = true;
                }
            }
            return retval;
        }
        public bool CheckIfSpeakerChanges(Models.ProgramRequest FromUser)
        {

            bool retval = false;
            var FromDb = Entities.ProgramRequests.Where(x => x.ProgramRequestID == FromUser.ProgramRequestID).SingleOrDefault();

            int SpeakerId = FromUser.ProgramSpeakerID;

            if (FromDb != null)
            {
                if ((FromDb.ProgramSpeakerID != SpeakerId))
                {
                    retval = true;
                }
            }
            return retval;
        }
        public string GetSpeakerConfirmationDate(int ProgramRequestID)
        {
            string Date = "";

            var date = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).FirstOrDefault();
            if (date != null)
            {
                Date = date.SpeakerConfirmedProgramDate.HasValue ? date.SpeakerConfirmedProgramDate.Value.ToString("yyyy/MM/dd") : "";

            }
            return Date;
        }

        public void UpdateSpeakerConfirmDateWhenNoModerator(int ProgramRequestId, string date)
        {
            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestId).SingleOrDefault();
            if (val != null)
            {

                val.SpeakerConfirmedProgramDate = DateTime.ParseExact(date, "yyyy/MM/dd", null);
                val.SpeakerConfirmationDate = DateTime.Now;
                val.SpeakerChosenProgramDate = true;
                val.SpeakerStatus = "Accepted";
                val.ConfirmedSessionDate = DateTime.ParseExact(date, "yyyy/MM/dd", null);
                val.SpeakerProgramDateNA = false;
                Entities.SaveChanges();

            }

        }

        public void UpdateProgramRequestWhenVenueChangedByAdmin(int ProgramRequestID)
        {

            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).SingleOrDefault();

            val.ReadOnly = false;
            val.RequestStatus = 7;

            Entities.SaveChanges();




        }


        public bool IsVenueChangesBySalesRep(Models.ProgramRequest FromUser)
        {

            bool retval = false;
            var FromDb = Entities.ProgramRequests.Where(x => x.ProgramRequestID == FromUser.ProgramRequestID).SingleOrDefault();

           

            if (FromDb != null && FromDb.LocationType != null)
            {
                if(!(FromUser.LocationType.Equals(FromDb.LocationType)) || !(FromUser.LocationAddress.Equals(FromDb.LocationAddress))                     
                    || !(FromUser.LocationName.Equals(FromDb.LocationName)) || !(FromUser.LocationCity.Equals(FromDb.LocationCity))                     
                    || !(FromUser.LocationProvince.Equals(FromDb.LocationProvince))

                    )                   
                  
                {

                    retval = true;

                }
            }
            return retval;
        }



        public bool IsMealTimesChangesBySalesRep(Models.ProgramRequest FromUser)
        {

            bool retval = false;
            var FromDb = Entities.ProgramRequests.Where(x => x.ProgramRequestID == FromUser.ProgramRequestID).SingleOrDefault();
            if(FromDb == null)
            {
                return false;
            }


            string ProgramEndTime = FromDb.ProgramEndTime.HasValue ? FromDb.ProgramEndTime.Value.ToString("HH:mm") : "";
            string  ProgramStartTime = FromDb.ProgramStartTime.HasValue ? FromDb.ProgramStartTime.Value.ToString("HH:mm") : "";

            string MealStartTime = FromDb.MealStartTime.HasValue ? FromDb.MealStartTime.Value.ToString("HH:mm") : "";


            if (FromDb != null)
            {
                if (!(FromUser.ProgramStartTime.Equals(ProgramStartTime)) || !(FromUser.ProgramEndTime.Equals(ProgramEndTime))
                    || (FromUser.MealStartTime != null && !(FromUser.MealStartTime.Equals(MealStartTime))))

                {

                    retval = true;

                }
            }
            return retval;
        }

        public void ResetSpeakerModeratorConfirmDates(int ProgramRequestID)
        {


            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).SingleOrDefault();

            val.SpeakerConfirmedProgramDate = null;
            val.SpeakerDeclined = null;
            val.SpeakerConfirmationDate = null;
            val.SpeakerChosenProgramDate = null;
            val.ConfirmedSessionDate = null;
            val.SpeakerProgramDateNA = null;
            val.ModeratorChosenProgramDate = null;
            val.ModeratorConfirmationDate = null;
            val.ModeratorConfirmedProgramDate = null;
            val.ModeratorDeclined = null;
            val.ModeratorProgramDateNA = null;
           
            Entities.SaveChanges();


        }

        public void ResetModeratorstatusAndConfirmSessionDate(int ProgramRequestID)
        {

            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).SingleOrDefault();

            if (val != null)
            {
                val.ModeratorStatus = "No Response";
                val.ModeratorChosenProgramDate = null;
                val.ModeratorConfirmedProgramDate = null;
                val.ModeratorDeclined = null;
                val.ModeratorConfirmationDate = null;
                val.ModeratorProgramDateNA = null;
                val.ConfirmedSessionDate = null;
                Entities.SaveChanges();

            }


        }

        public void UpdateRequestStatusByAdmin(int ProgramRequestID, int StatusId)
        {

            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).FirstOrDefault();

            if (val != null)
            {
                val.RequestStatus = StatusId;
                Entities.SaveChanges();
            }

        }


        #endregion End of Ali Code ===================================================================================================================================

        public List<SelectListItem> GetSpeakers()
        {
            //speaker who have approved, registered not complete (Speaker chose email/password but not yet upload coi or payeeform) or registercomplete (User chose email/password and upload coi and payeeform)

            return Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)))).Select(c => new SelectListItem
            {
                Value = c.id.ToString(),
                Text = c.FirstName + " " + c.LastName

            }).ToList();
        }

        public List<SelectListItem> GetModerators()
        {
            //get approved moderators only
            return Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4)))).Select(c => new SelectListItem
            {
                Value = c.id.ToString(),
                Text = c.FirstName + " " + c.LastName

            }).ToList();
        }

        public bool SaveNewSession(ProgramRequestIIModel pr)
        {

            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == pr.ProgramRequestID).SingleOrDefault();
            ProgramRequestSessionCredits credits = new ProgramRequestSessionCredits();

            if (val != null)
            {
                DateTime? dt = null;
                val.ProgramID = pr.ProgramID;
                val.UserID = pr.UserID;

                val.SponsorID = pr.SponsorID;

                val.ContactFirstName = pr.ContactFirstName;
                val.ContactLastName = pr.ContactLastName;
                val.ContactEmail = pr.ContactEmail;
                val.ContactPhone = pr.ContactPhone;
                val.ProgramDate1 = DateTime.ParseExact(pr.ProgramDate1, "yyyy/MM/dd", null);

                val.ProgramDate2 = !(string.IsNullOrEmpty(pr.ProgramDate2)) ? (DateTime.ParseExact(pr.ProgramDate2, "yyyy/MM/dd", null)) : dt;
                val.ProgramDate3 = !(string.IsNullOrEmpty(pr.ProgramDate3)) ? (DateTime.ParseExact(pr.ProgramDate3, "yyyy/MM/dd", null)) : dt;


                DateTime startTime = DateTime.ParseExact(pr.ProgramDate1, "yyyy/MM/dd", null);
                if (!string.IsNullOrEmpty(pr.MealStartTime))
                {
                    val.MealStartTime = startTime.Add(TimeSpan.Parse(pr.MealStartTime));
                }
                val.ProgramStartTime = startTime.Add(TimeSpan.Parse(pr.ProgramStartTime));
                val.ProgramEndTime = startTime.Add(TimeSpan.Parse(pr.ProgramEndTime));
                if (!string.IsNullOrEmpty(pr.RegistrationArrivalTime))
                {
                    val.RegistrationArrivalTime = startTime.Add(TimeSpan.Parse(pr.RegistrationArrivalTime));
                }


                val.SpeakerChosenProgramDate = pr.SpeakerChosenProgramDate;
                val.ModeratorChosenProgramDate = pr.ModeratorChosenProgramDate;

               
                val.EventType = pr.EventType;
                val.EventTypeQuestion1 = pr.EventTypeQuestion1;
                val.EventTypeQuestion2 = pr.EventTypeQuestion2;
                val.EventTypeQuestion3 = pr.EventTypeQuestion3;
                val.EventTypeQuestion4 = pr.EventTypeQuestion4;
                val.EventTypeQuestion5 = pr.EventTypeQuestion5;
                val.MealOption = pr.MealOption;





                val.MultiSession = pr.MultiSession;
                if (pr.CostPerPerson != null)
                {
                    val.CostPerPerson = Convert.ToDecimal(pr.CostPerPerson);
                }
                val.CostByParticipant = Convert.ToDecimal(pr.CostPerparticipants);
                val.ProgramSpeakerID = pr.ProgramSpeakerID;
                val.ProgramModeratorID = pr.ProgramModeratorID;
                val.VenueContacted = pr.VenueContacted;
                val.LocationType = pr.LocationType;
                val.LocationTypeOther = pr.LocationTypeOther;
                val.LocationName = pr.LocationName;
                val.LocationAddress = pr.LocationAddress;
                val.LocationCity = pr.LocationCity;
                val.LocationProvince = pr.LocationProvince;
                val.LocationPhoneNumber = pr.LocationPhoneNumber;
                val.LocationWebsite = pr.LocationWebsite;
                val.MealType = pr.MealType;
                val.AVEquipment = pr.AVEquipment;
                val.ProgramCostsSplit = pr.ProgramCostsSplit;
                val.AmgenRepName = pr.AmgenRepName;
                val.RequestStatus = 1;
                val.ReadOnly = true;
                val.SpeakerStatus = "No Response";

                val.SessionAgendaUploaded = pr.SessionAgendaUploaded;
                val.SessionAgendaFileName = pr.SessionAgendaFileName;

              

                if (pr.ProgramModeratorID != null)
                {
                    val.ModeratorStatus = "No Response";
                }
                else
                {
                    val.ModeratorStatus = "Not Applicable";
                }
                val.Comments = pr.Comments;
                val.SubmittedDate = DateTime.Now;
                val.LastUpdatedDate = DateTime.Now;


                try
                {
                    Entities.SaveChanges();
                }
                catch (Exception e)
                {
                    return false;
                }

            }
            /*
                        var SessionCreditsToDelete = Entities.ProgramRequestSessionCredits.Where(x => x.ProgramRequestID == pr.ProgramRequestID).ToList();

                        if (SessionCreditsToDelete != null)
                        {
                            foreach (var row in SessionCreditsToDelete)
                            {

                                Entities.ProgramRequestSessionCredits.Remove(row);
                            }
                            Entities.SaveChanges();

                        }*/
            if (pr.ProgramID == 5)
            {
                if (((ProgramRequestIIModel)pr).SessionCredit14)
                {
                    Entities.ProgramRequestSessionCredits.Add(new ProgramRequestSessionCredit
                    {
                        SessionCreditID = 14,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (((ProgramRequestIIModel)pr).SessionCredit15)
                {
                    Entities.ProgramRequestSessionCredits.Add(new ProgramRequestSessionCredit
                    {
                        SessionCreditID = 15,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }                
            }

            if (pr.ProgramID == 7)
            {
                if (((ProgramRequestIIModel)pr).SessionCredit16)
                {
                    Entities.ProgramRequestSessionCredits.Add(new ProgramRequestSessionCredit
                    {
                        SessionCreditID = 16,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (((ProgramRequestIIModel)pr).SessionCredit17)
                {
                    Entities.ProgramRequestSessionCredits.Add(new ProgramRequestSessionCredit
                    {
                        SessionCreditID = 17,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (((ProgramRequestIIModel)pr).SessionCredit18)
                {
                    Entities.ProgramRequestSessionCredits.Add(new ProgramRequestSessionCredit
                    {
                        SessionCreditID = 18,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (((ProgramRequestIIModel)pr).SessionCredit19)
                {
                    Entities.ProgramRequestSessionCredits.Add(new ProgramRequestSessionCredit
                    {
                        SessionCreditID = 19,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }
            }
            if (pr.ProgramID == 8)
            {
                if (((ProgramRequestIIModel)pr).SessionCredit20)
                {
                    Entities.ProgramRequestSessionCredits.Add(new ProgramRequestSessionCredit
                    {
                        SessionCreditID = 20,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }

                if (((ProgramRequestIIModel)pr).SessionCredit21)
                {
                    Entities.ProgramRequestSessionCredits.Add(new ProgramRequestSessionCredit
                    {
                        SessionCreditID = 21,
                        ProgramRequestID = pr.ProgramRequestID
                    });
                }
            }

            //else if (pr.ProgramID == 2)//the clinical exchange program is different from New Horizon program in CV
            //{
            //    if (pr.SessionCredit1)
            //    {
            //        Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
            //        {
            //            SessionCreditID = 6,
            //            ProgramRequestID = pr.ProgramRequestID
            //        });
            //    }

            //    if (pr.SessionCredit2)
            //    {
            //        Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
            //        {
            //            SessionCreditID = 7,
            //            ProgramRequestID = pr.ProgramRequestID
            //        });
            //    }

            //    if (pr.SessionCredit3)
            //    {
            //        Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
            //        {
            //            SessionCreditID = 8,
            //            ProgramRequestID = pr.ProgramRequestID
            //        });
            //    }

            //    if (pr.SessionCredit4)
            //    {
            //        Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
            //        {
            //            SessionCreditID = 9,
            //            ProgramRequestID = pr.ProgramRequestID
            //        });
            //    }

            //    if (pr.SessionCredit5)
            //    {
            //        Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
            //        {
            //            SessionCreditID = 10,
            //            ProgramRequestID = pr.ProgramRequestID
            //        });
            //    }
            //    if (pr.SessionCredit6)
            //    {
            //        Entities.ProgramRequestSessionCredits.Add(new CPDPortal.Data.ProgramRequestSessionCredit
            //        {
            //            SessionCreditID = 11,
            //            ProgramRequestID = pr.ProgramRequestID
            //        });
            //    }

            //}
            try
            {
                Entities.SaveChanges();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public ProgramRequestIIModel InitialEventRequestForm(int ProgramID)
        {
            var CurrentUser = UserHelper.GetLoggedInUser();

            ProgramRequestIIModel pr = new ProgramRequestIIModel();
            //they are all in the hidden field, for saving later.
            pr.ContactName = CurrentUser.FirstName + " " + CurrentUser.LastName;
            pr.ContactFirstName = CurrentUser.FirstName;
            pr.ContactLastName = CurrentUser.LastName;
            pr.ContactEmail = CurrentUser.Username;
            pr.ContactPhone = CurrentUser.Phone;
            pr.SpeakerChosenProgramDate = false;
            pr.ModeratorChosenProgramDate = false;
            pr.ContactPhone = CurrentUser.Phone;
            pr.ProgramRequestID = GetProgramRequestID();
            pr.ProgramID = ProgramID;
            pr.UserID = CurrentUser.UserID;
            pr.SponsorID = 1;


            pr.Speakers = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4))))
                .OrderBy(x => x.FirstName).Select(c => new SelectListItem
            {
                Value = c.id.ToString(),
                Text = c.FirstName + " " + c.LastName

            }).ToList();
            //get approved moderators only
            pr.Moderators = Entities.UserInfoes.Where(x => (((x.UserType == 2) || (x.UserType == 3)) && ((x.Status == 2) || (x.Status == 3) || (x.Status == 4))))
                .OrderBy(x => x.FirstName).Select(c => new SelectListItem
            {
                Value = c.id.ToString(),
                Text = c.FirstName + " " + c.LastName

            }).ToList();


            var objProgram = Entities.Programs.Where(x => x.ProgramID == ProgramID).FirstOrDefault();
            if (objProgram != null)
            {

                pr.Archive = objProgram.Archive ?? false;
                pr.DisplayOrder = objProgram.DisplayOrder ?? 0;
            }



            //get approved speakers

            return pr;
        }
    }
}