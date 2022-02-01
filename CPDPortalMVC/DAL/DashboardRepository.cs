using CPDPortalMVC.Models;
using CPDPortalMVC.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.DAL
{
    public class DashboardRepository:BaseRepository
    {
        public List<DashboardItem> GetAllDashboardItems(int UserID, int ProgramID)
        {
            List<DashboardItem> DashboardItemsNonNullSessionDate = null;
            List<DashboardItem> DashboardItemsNullSessionDate = null;
            List<DashboardItem> DashboardItems = null;
                UserRepository UserRepo = new UserRepository();
         
            string UserRole = UserHelper.GetRoleByUserID(UserID);
            int SponsorID = UserHelper.GetLoggedInUser().SponsorID;
            //head office and sale director see every sessions for a program for a sponsor no need to query for territoryID
            if (UserRole == Util.Constants.HeadOffice || UserRole == Util.Constants.SalesDirector)
            {

                DashboardItems = (from pr in Entities.ProgramRequests
                                  where pr.ProgramID == ProgramID && pr.SponsorID == SponsorID
                                  // display all requests for now where pr.RequestStatus == 2 || pr.RequestStatus == 3  //show only submitted requests (either approved 3 or waiting to be approved  2 in the admin tool)
                                  orderby (pr.ConfirmedSessionDate == null) ? DateTime.MaxValue : pr.ConfirmedSessionDate descending
                                  select new DashboardItem()
                                  {
                                      ProgramRequestID = pr.ProgramRequestID,
                                      ProgramID = pr.ProgramID??0,
                                      UserID = UserID,
                                      RequestDate = (pr.SubmittedDate == null) ? null : SqlFunctions.DateName("year", pr.SubmittedDate) + "/" + SqlFunctions.DatePart("m", pr.SubmittedDate) + "/" + SqlFunctions.DateName("day", pr.SubmittedDate),
                                      FullSessionDetails = pr.ReadOnly ?? false, //if true eye image, if false pencil

                                      SessionStatus = pr.RequestStatusLookup.RequestStatusDescription,
                                      FinalAttendance = pr.AdminFinalAttendance == null ? null : pr.AdminFinalAttendance.ToString(),
                                      SessionStatusID = pr.RequestStatus ?? 0,
                                      SessionDate = (pr.ConfirmedSessionDate == null) ? null : SqlFunctions.DateName("year", pr.ConfirmedSessionDate) + "/" + SqlFunctions.DatePart("m", pr.ConfirmedSessionDate) + "/" + SqlFunctions.DateName("day", pr.ConfirmedSessionDate),
                                      Speaker = pr.SpeakerInfo.FirstName + "," + pr.SpeakerInfo.LastName,
                                      Moderator = pr.ModeratorInfo.FirstName + "," + pr.ModeratorInfo.LastName,

                                      SpeakerProgramDateNA = pr.SpeakerProgramDateNA,
                                      ModeratorProgramDateNA = pr.ModeratorProgramDateNA,
                                      SpeakerDeclined = pr.SpeakerDeclined ?? false,
                                      ModeratorDeclined = pr.ModeratorDeclined ?? false,
                                      VenueAvailable = pr.AdminVenueAvailable,

                                      Location = pr.LocationName,
                                      ReadOnly = pr.ReadOnly ?? false




                                  }).ToList();

            }//see all users under the manager's territoryID
            else if (UserRole == Util.Constants.RegionalManager)
            {  //need to retrieve all users with the same territoryID and get all dashboard items with the same territoryID

                //the manager's territoryID ie. 41
                var TerritoryID = (from ut in Entities.UserInfoes where ut.UserID == UserID select ut.TerritoryID).FirstOrDefault();
                //all userids under territoryID 41
                var TerritorialUserIDs = (from ut in Entities.UserInfoes where ut.TerritoryID == TerritoryID select ut.UserID).ToList();
                //dashboarditems of all userids belonging to territory 41
                DashboardItems = (from pr in Entities.ProgramRequests
                                  where pr.ProgramID == ProgramID && TerritorialUserIDs.Contains(pr.UserID)
                                  // display all requests for now where pr.RequestStatus == 2 || pr.RequestStatus == 3  //show only submitted requests (either approved 3 or waiting to be approved  2 in the admin tool)
                                  orderby (pr.ConfirmedSessionDate == null) ? DateTime.MaxValue : pr.ConfirmedSessionDate descending
                                  select new DashboardItem()
                                  {
                                      ProgramRequestID = pr.ProgramRequestID,
                                      ProgramID = pr.ProgramID ?? 0,
                                      UserID = UserID,
                                      RequestDate = (pr.SubmittedDate == null) ? null : SqlFunctions.DateName("year", pr.SubmittedDate) + "/" + SqlFunctions.DatePart("m", pr.SubmittedDate) + "/" + SqlFunctions.DateName("day", pr.SubmittedDate),
                                      FullSessionDetails = pr.ReadOnly ?? false, //if true eye image, if false pencil
                                      SessionStatusID = pr.RequestStatus ?? 0,
                                      SessionStatus = pr.RequestStatusLookup.RequestStatusDescription,
                                      FinalAttendance = pr.AdminFinalAttendance == null ? null : pr.AdminFinalAttendance.ToString(),
                                      SessionDate = (pr.ConfirmedSessionDate == null) ? null : SqlFunctions.DateName("year", pr.ConfirmedSessionDate) + "/" + SqlFunctions.DatePart("m", pr.ConfirmedSessionDate) + "/" + SqlFunctions.DateName("day", pr.ConfirmedSessionDate),
                                      Speaker = pr.SpeakerInfo.FirstName + "," + pr.SpeakerInfo.LastName,
                                      Moderator = pr.ModeratorInfo.FirstName + "," + pr.ModeratorInfo.LastName,


                                      Location = pr.LocationName,

                                      SpeakerProgramDateNA = pr.SpeakerProgramDateNA,
                                      ModeratorProgramDateNA = pr.ModeratorProgramDateNA,
                                      SpeakerDeclined = pr.SpeakerDeclined ?? false,
                                      ModeratorDeclined = pr.ModeratorDeclined ?? false,
                                      VenueAvailable=pr.AdminVenueAvailable,

                                      ReadOnly = pr.ReadOnly ?? false

                                      
                                  }).ToList();
            }
            else //sale rep only get to see he own session request
            {
                //DashboardItemsNullSessionDate = (from pr in Entities.ProgramRequests
                //                  where pr.ProgramID == ProgramID && pr.UserID == UserID && pr.ConfirmedSessionDate == null
                //                  // display all requests for now where pr.RequestStatus == 2 || pr.RequestStatus == 3  //show only submitted requests (either approved 3 or waiting to be approved  2 in the admin tool)
                                
                //                 // orderby (pr.ConfirmedSessionDate == null)? DateTime.MaxValue : pr.ConfirmedSessionDate  descending
                                  
                //                  select new DashboardItem()
                //                  {
                //                      ProgramRequestID = pr.ProgramRequestID,
                //                      ProgramID = pr.ProgramID ?? 0,
                //                      UserID = UserID,
                //                      RequestDate = (pr.SubmittedDate == null) ? null : SqlFunctions.DateName("year", pr.SubmittedDate) + "/" + SqlFunctions.DatePart("m", pr.SubmittedDate) + "/" + SqlFunctions.DateName("day", pr.SubmittedDate),
                //                      FullSessionDetails = pr.ReadOnly ?? false, //if true eye image, if false pencil
                //                      SessionStatusID = pr.RequestStatus ?? 0,
                //                      SessionStatus = pr.RequestStatusLookup.RequestStatusDescription,
                //                      FinalAttendance = pr.AdminFinalAttendance == null ? null : pr.AdminFinalAttendance.ToString(),
                //                      SessionDate = (pr.ConfirmedSessionDate == null) ? null : SqlFunctions.DateName("year", pr.ConfirmedSessionDate) + "/" + SqlFunctions.DatePart("m", pr.ConfirmedSessionDate) + "/" + SqlFunctions.DateName("day", pr.ConfirmedSessionDate),
                //                      Speaker = pr.SpeakerInfo.FirstName + "," + pr.SpeakerInfo.LastName,
                //                      Moderator = pr.ModeratorInfo.FirstName + "," + pr.ModeratorInfo.LastName,
                                    
                //                      Location = pr.LocationName,

                //                      SpeakerProgramDateNA = pr.SpeakerProgramDateNA,
                //                      ModeratorProgramDateNA = pr.ModeratorProgramDateNA,
                //                      SpeakerDeclined = pr.SpeakerDeclined ?? false,
                //                      ModeratorDeclined = pr.ModeratorDeclined ?? false,
                //                      VenueAvailable = pr.AdminVenueAvailable,
                //                      ReadOnly = pr.ReadOnly ?? false






                //                  }).ToList();
                DashboardItems = (from pr in Entities.ProgramRequests
                                                 where pr.ProgramID == ProgramID && pr.UserID == UserID
                                                 // display all requests for now where pr.RequestStatus == 2 || pr.RequestStatus == 3  //show only submitted requests (either approved 3 or waiting to be approved  2 in the admin tool)

                                                  orderby  pr.ConfirmedSessionDate  descending

                                                 select new DashboardItem()
                                                 {
                                                     ProgramRequestID = pr.ProgramRequestID,
                                                     ProgramID = pr.ProgramID ?? 0,
                                                     UserID = UserID,
                                                     RequestDate = (pr.SubmittedDate == null) ? null : SqlFunctions.DateName("year", pr.SubmittedDate) + "/" + SqlFunctions.DatePart("m", pr.SubmittedDate) + "/" + SqlFunctions.DateName("day", pr.SubmittedDate),
                                                     FullSessionDetails = pr.ReadOnly ?? false, //if true eye image, if false pencil
                                                     SessionStatusID = pr.RequestStatus ?? 0,
                                                     SessionStatus = pr.RequestStatusLookup.RequestStatusDescription,
                                                     FinalAttendance = pr.AdminFinalAttendance == null ? null : pr.AdminFinalAttendance.ToString(),
                                                     SessionDate = (pr.ConfirmedSessionDate == null) ? null : SqlFunctions.DateName("year", pr.ConfirmedSessionDate) + "/" + SqlFunctions.DatePart("m", pr.ConfirmedSessionDate) + "/" + SqlFunctions.DateName("day", pr.ConfirmedSessionDate),
                                                     Speaker = pr.SpeakerInfo.FirstName + "," + pr.SpeakerInfo.LastName,
                                                     Moderator = pr.ModeratorInfo.FirstName + "," + pr.ModeratorInfo.LastName,

                                                     Location = pr.LocationName,

                                                     SpeakerProgramDateNA = pr.SpeakerProgramDateNA,
                                                     ModeratorProgramDateNA = pr.ModeratorProgramDateNA,
                                                     SpeakerDeclined = pr.SpeakerDeclined ?? false,
                                                     ModeratorDeclined = pr.ModeratorDeclined ?? false,
                                                     VenueAvailable = pr.AdminVenueAvailable,
                                                     ReadOnly = pr.ReadOnly ?? false






                                                 }).ToList();
                //DashboardItems = DashboardItemsNonNullSessionDate.Concat(DashboardItemsNullSessionDate).ToList();

            }
           
            //setting MyActionItems
            foreach (DashboardItem item in DashboardItems)
            {
                int BusinessDays = 0;
                string Moderator;
                //stores the language of sales rep
                string Language;
                Language=UserRepo.GetUserByUserID(item.UserID).Language;
                //if language is not specified default to english else french
                item.Language = (Language == null) ? "en" : "fr";

                //if moderator does not present return nothing else return a slash and the name
                Moderator = string.IsNullOrEmpty(item.Moderator) ? "" : " / " + item.Moderator;
                item.SessionInfo = item.SessionDate + "<br/>" + item.Speaker + Moderator + "<br/>" + item.Location;
                //if the program is  Osteoporosis Management, check if modules have been selected
                if (item.ProgramID==7)
                {
                    //set status to 8 (modules selected) if modules have been selected and event has not gone to ethics review
                    int modulesSelected = Entities.ProgramRequestModules.Where(p => p.ProgramRequestID == item.ProgramRequestID).Count();

                    if (modulesSelected > 0 &&  item.SessionStatusID < 2)
                    {
                        //if modules selected and request hasn't sent to ethics review
                        //this prompts the display/down national invitation
                        item.SessionStatusID = 8;

                    }

                }
                //calcuate if the session date is at least 2 business days more than today's date

                if (item.SessionDate != null)
                {


                    BusinessDays = BusinessDaysCalc.GetWorkingdays(DateTime.Now, Convert.ToDateTime(item.SessionDate));
                    //cannot cancell if business day less 2 or the session is completed
                    if (BusinessDays >= 2 && item.SessionStatusID != 4 && item.SessionStatusID != 5)
                    {
                        item.CanSessionBeCancelled = true;
                    }
                    else
                    {
                        item.CanSessionBeCancelled = false;
                    }
                }
                else
                {
                    if (item.SessionStatusID != 4 && item.SessionStatusID != 5)
                    
                        item.CanSessionBeCancelled = true;
                     else
                        item.CanSessionBeCancelled = false;

                }
                Console.WriteLine("BusinessDays:" + BusinessDays);
                switch (item.SessionStatusID)
                {
                    case 1://under review
                        item.MyActionItems = Constants.NA;
                        break;
                    case 2://Active – Regional Ethics Review Pending 
                        item.MyActionItems = Constants.NA;
                        break;
                    case 3://Active – Regional Ethics Approved 
                        item.MyActionItems = Constants.NA;
                        break;
                    case 5://Completed – Items Pending 
                        item.MyActionItems = Constants.SubmitPostSessionMaterials + "<br/>";
                        break;
                    case 4://Completed – Session Closed 
                        item.MyActionItems = Constants.NA;
                        break;
                    case 6://Completed – Session Cancelled 
                        item.MyActionItems = Constants.NA;
                        break;
                    case 8://Speaker selected Event Modules
                        item.MyActionItems = Constants.NA;
                        break;


                }
                //Overriding default ActionItems
                string ExistingActionItems = string.Empty;
                // if the following scenarios occur we will be override the default action items
                if (item.SpeakerProgramDateNA ?? false)//when speak select NA in email, it will set the Readonly to false in the programrequestID which change Session Status to Changes Required
                {

                    ExistingActionItems = ExistingActionItems + Constants.SpeakerNA + "<br/>";
                    item.MyActionItems = ExistingActionItems;
                }
                if (item.ModeratorProgramDateNA ?? false)//when speak select NA in email, it will set the Readonly to false in the programrequestID which change Session Status to Changes Required
                {

                    ExistingActionItems = ExistingActionItems + Constants.ModeratorNA + "<br/>";
                    item.MyActionItems = ExistingActionItems;
                }
                if (item.SpeakerDeclined ?? false)//when speak select NA in email, it will set the Readonly to false in the programrequestID which change Session Status to Changes Required
                {

                    ExistingActionItems = ExistingActionItems + " "  + Constants.SpeakerDeclined + "<br/>";
                    item.MyActionItems = ExistingActionItems;
                }
                if (item.ModeratorDeclined ?? false)//when speak select NA in email, it will set the Readonly to false in the programrequestID which change Session Status to Changes Required
                {

                    ExistingActionItems = ExistingActionItems + " " + Constants.ModeratorDeclined + "<br/>";
                    item.MyActionItems = ExistingActionItems;
                }
                if (item.VenueAvailable == "N")//when chrc admin select No to Is Venue Available (under Manage Program Request)
                {

                    ExistingActionItems = ExistingActionItems + " " + ExistingActionItems + Constants.VenueNA + "<br/>";
                    item.MyActionItems = ExistingActionItems;
                }



            }


            return DashboardItems;
        }

        public EvalForm5Model GetEvalForm5Model(int ProgramRequestID)
        {
            EvalForm5Model efm;
            List<ProgramRequestSessionCredits> SessionList = new List<ProgramRequestSessionCredits>();
            ProgramRepository progRepo = new ProgramRepository();

            SessionList = progRepo.GetSessionCredits(ProgramRequestID);

            ProgramRepository prepo = new ProgramRepository();

            efm = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).Select(ui =>
                     new EvalForm5Model
                     {
                         ProgramRequestID = ProgramRequestID,
                         ProgramDate = ui.ConfirmedSessionDate.ToString(),
                         ProgramLocation = ui.LocationName + " " + ui.LocationAddress,
                         Speaker1 = ui.SpeakerInfo.FirstName + ", " + ui.SpeakerInfo.LastName,
                        
                         Moderator = ui.ModeratorInfo.FirstName + ", " + ui.ModeratorInfo.LastName





                     }).SingleOrDefault();
            if (!string.IsNullOrEmpty(efm.ProgramDate))
            {
                efm.ProgramDate = Convert.ToDateTime(efm.ProgramDate).ToString("MMMM dd, yyyy");
            }
            foreach (var item in SessionList)
            {
                if (item.id == 14)
                {

                    efm.SessionCredit1 = true;
                }

                if (item.id == 15)
                {

                    efm.SessionCredit2 = true;
                }

                


            }

            return efm;





        }

        public NationalInvitation7Model GetNationalInvitation7Model(int ProgramRequestID)
        {
            NationalInvitation7Model nim7;
           
            List<ProgramRequestSessionCredits> SessionList = new List<ProgramRequestSessionCredits>();
            ProgramRepository progRepo = new ProgramRepository();

            SessionList = progRepo.GetSessionCredits(ProgramRequestID);

            ProgramRepository prepo = new ProgramRepository();

            nim7 = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).Select(ui =>
                     new NationalInvitation7Model
                     {
                         ProgramRequestID = ProgramRequestID,
                         ContactFirstName = ui.ContactFirstName,
                         ContactLastName = ui.ContactLastName,
                         ContactPhone = ui.ContactPhone,
                         ContactEmail = ui.ContactEmail,
                         // ProgramStartTime = "program start time",
                         ProgramStartTime = (ui.ProgramStartTime == null) ? null : SqlFunctions.DatePart("Hour", ui.ProgramStartTime) + ":" + SqlFunctions.DatePart("Minute", ui.ProgramStartTime),
                         ProgramEndTime = (ui.ProgramEndTime == null) ? null : SqlFunctions.DatePart("Hour", ui.ProgramEndTime) + ":" + SqlFunctions.DatePart("Minute", ui.ProgramEndTime),

                         // ProgramEndTime = ui.ProgramEndTime.HasValue ? ui.ProgramEndTime.Value.ToString("HH:mm") : "",
                         ProgramDate = (ui.ConfirmedSessionDate == null) ? null : SqlFunctions.DateName("year", ui.ConfirmedSessionDate) + "/" + SqlFunctions.DatePart("m", ui.ConfirmedSessionDate) + "/" + SqlFunctions.DateName("day", ui.ConfirmedSessionDate),
                         ProgramLocation = ui.LocationName + " " + ui.LocationAddress,
                         Speaker1 = ui.SpeakerInfo.FirstName + ", " + ui.SpeakerInfo.LastName,
                        
                         Moderator = ui.ModeratorInfo.FirstName + ", " + ui.ModeratorInfo.LastName,
                         ProvinceCode=ui.LocationProvince





                     }).SingleOrDefault();

            nim7.ProgramDate = Convert.ToDateTime(nim7.ProgramDate).ToString("MMMM dd, yyyy");
            nim7.ProvinceName = UserHelper.GetProvinceFullName(nim7.ProvinceCode);
            var obj = Entities.ProgramRequestModules.Where(x => x.ProgramRequestID == ProgramRequestID).SingleOrDefault();
            if (obj != null)
            {
                nim7.ProgramModule1 = obj.ProgramModule1 ?? false;
                nim7.ProgramModule2 = obj.ProgramModule2 ?? false;
                nim7.ProgramModule3 = obj.ProgramModule3 ?? false;
                nim7.ProgramModule4 = obj.ProgramModule4 ?? false;
                nim7.ProgramModule5 = obj.ProgramModule5 ?? false;
                nim7.ProgramModule6 = obj.ProgramModule6 ?? false;
                nim7.ProgramModule7 = obj.ProgramModule7 ?? false;
                nim7.ProgramModule8 = obj.ProgramModule8 ?? false;
                nim7.ProgramModule9 = obj.ProgramModule9 ?? false;
                nim7.ProgramModule10 = obj.ProgramModule10 ?? false;
            }
            foreach (var item in SessionList)
            {
                if (item.id == 16)
                {

                    nim7.SessionCreditID = 16;
                    nim7.SessionCredit = "1.0 Mainpro+ Credits (1 hour)";
                    nim7.SessionCreditValue = 1;
                }

                if (item.id == 17)
                {

                    nim7.SessionCreditID = 17;
                    nim7.SessionCredit = "1.5 Mainpro+ Credits (1 hour and 30 minutes)";
                    nim7.SessionCreditValue = 1.5M;
                }
                if (item.id == 18)
                {

                    nim7.SessionCreditID = 18;
                    nim7.SessionCredit = "2.0 Mainpro+ Credits (2 hours)";
                    nim7.SessionCreditValue = 2;
                }
                if (item.id == 19)
                {

                    nim7.SessionCreditID = 19;
                    nim7.SessionCredit = "2.75 Mainpro + Credits (2 hours and 45 minutes)";
                    nim7.SessionCreditValue = 2.75M;
                }



            }

            return nim7;





        }

        public EvalForm7Model GetEvalForm7Model(int ProgramRequestID)
        {
            EvalForm7Model efm;
            List<ProgramRequestSessionCredits> SessionList = new List<ProgramRequestSessionCredits>();
            ProgramRepository progRepo = new ProgramRepository();

            SessionList = progRepo.GetSessionCredits(ProgramRequestID);

            ProgramRepository prepo = new ProgramRepository();

            efm = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).Select(ui =>
                     new EvalForm7Model
                     {
                         ProgramRequestID = ProgramRequestID,
                         ProgramDate = ui.ConfirmedSessionDate.ToString(),
                         ProgramLocation = ui.LocationName + " " + ui.LocationAddress,
                         Speaker1 = ui.SpeakerInfo.FirstName + ", " + ui.SpeakerInfo.LastName,
                         Moderator = ui.ModeratorInfo.FirstName + ", " + ui.ModeratorInfo.LastName
                     }).SingleOrDefault();
            if (!string.IsNullOrEmpty(efm.ProgramDate))
            {
                efm.ProgramDate = Convert.ToDateTime(efm.ProgramDate).ToString("MMMM dd, yyyy");
            }
            foreach (var item in SessionList)
            {
                if (item.id == 14)
                {

                    efm.SessionCredit1 = true;
                }

                if (item.id == 15)
                {

                    efm.SessionCredit2 = true;
                }
            }

            return efm;
        }

        public EvalForm8Model GetEvalForm8Model(int ProgramRequestID)
        {
            EvalForm8Model efm;
            List<ProgramRequestSessionCredits> SessionList = new List<ProgramRequestSessionCredits>();
            ProgramRepository progRepo = new ProgramRepository();

            SessionList = progRepo.GetSessionCredits(ProgramRequestID);

            ProgramRepository prepo = new ProgramRepository();

            efm = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).Select(ui =>
                     new EvalForm8Model
                     {
                         ProgramRequestID = ProgramRequestID,
                         ProgramDate = ui.ConfirmedSessionDate.ToString(),
                         ProgramLocation = ui.LocationName + " " + ui.LocationAddress,
                         Speaker1 = ui.SpeakerInfo.FirstName + ", " + ui.SpeakerInfo.LastName,
                         Moderator = ui.ModeratorInfo.FirstName + ", " + ui.ModeratorInfo.LastName
                     }).SingleOrDefault();
            if (!string.IsNullOrEmpty(efm.ProgramDate))
            {
                efm.ProgramDate = Convert.ToDateTime(efm.ProgramDate).ToString("MMMM dd, yyyy");
            }
            foreach (var item in SessionList)
            {
                if (item.id == 20)
                {

                    efm.SessionCredit1 = true;
                }

                if (item.id == 21)
                {

                    efm.SessionCredit2 = true;
                }
            }

            return efm;
        }
    }
}
