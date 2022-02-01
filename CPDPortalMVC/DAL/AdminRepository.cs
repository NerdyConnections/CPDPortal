using CPDPortal.Data;
using CPDPortalMVC.Models;
using CPDPortalMVC.Util;
using CPDPortalMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.DAL
{
    public class AdminRepository:BaseRepository
    {
        public CPDPortalMVC.Models.UserRegistration GetUserRegistration(int UserID)
        {//usertype 2 for speaker
         //usertype 3 for moderator

           
                var val = Entities.UserRegistrations.Where(x => x.UserID == UserID).Select(pr =>
                         new Models.UserRegistration
                         {

                             UserID = UserID,
                             COIForm = pr.COIForm??false,
                             PayeeForm = pr.PayeeForm??false,
                             COIFormExt = pr.COIFormExt,
                           





                         }).SingleOrDefault();

            if (val == null)
            {

                val = new Models.UserRegistration()
                {
                    UserID = UserID,
                    COIForm = false,
                    PayeeForm = false,
                    COISlides = false
                };
            }

            return val;

           

        }
        public bool UpdateCOIForm(int UserID, string COIFormExt)
        {

            try {
                var val = Entities.UserRegistrations.Where(x => x.UserID == UserID).SingleOrDefault();
                if (val != null)
                {

                    val.COIForm = true;
                    val.COIFormExt = COIFormExt;
                    Entities.SaveChanges();
                    return true;
                }
                else
                {
                    CPDPortal.Data.UserRegistration objUserRegistration = new CPDPortal.Data.UserRegistration();
                    objUserRegistration.UserID = UserID;
                    objUserRegistration.COIForm = true;
                    objUserRegistration.COIFormExt = COIFormExt;
                    Entities.UserRegistrations.Add(objUserRegistration);
                    Entities.SaveChanges();
                    return true;



                }
            }catch(Exception e)
            {

                return false;
            }
               

        }
        public void SavePostSession(PostSessionViewModel psvm)
        {
            DateTime? dt = null;

            var objpr = (from v in Entities.ProgramRequests

                                  where v.ProgramRequestID == psvm.ProgramRequestID
                                  select v).FirstOrDefault();
            if (objpr != null)
            {



                objpr.ProgramRequestID = psvm.ProgramRequestID;

                objpr.AdminCCASent = psvm.CCASent;
                objpr.AdminVenueReceipt = psvm.VenueReceipt;
                objpr.AdminCFPCDateApproved = !(string.IsNullOrEmpty(psvm.CFPCDateApproved)) ? (DateTime.ParseExact(psvm.CFPCDateApproved, "dd/MM/yyyy", null)) : dt;
                objpr.AdminCFPCDateSubmitted = !(string.IsNullOrEmpty(psvm.CFPCDateSubmitted)) ? (DateTime.ParseExact(psvm.CFPCDateSubmitted, "dd/MM/yyyy", null)) : dt;
                objpr.AdminVenueNotes = psvm.VenueNotes;

                objpr.AdminCFPCFees = psvm.CFPCFees;
                objpr.AdminCFPCFeesTaxes = psvm.CFPCFeeTaxes;
                objpr.AdminCFPCImplementationFees = psvm.CFPCImplementationFees;
                objpr.AdminCFPCImplementationFeesTaxes = psvm.CFPCImplementationFeesTaxes;

                objpr.AdminVenueFees = psvm.VenueFees;
                objpr.AdminVenueFeesTaxes = psvm.VenueFeesTaxes;
                objpr.AdminAVFeesTaxes = psvm.AVFeesTaxes;
                objpr.AdminOtherFeesTaxes = psvm.OtherFeesTaxes;
         
                objpr.AdminAVFees = psvm.AVFees;
                objpr.AdminOtherFees = psvm.OtherFees;
                objpr.AdminFinalAttendance = psvm.FinalAttendance;
                //signin sheet
                objpr.AdminSessionID = psvm.AdminSessionID;
                objpr.AdminSpeakerHonorium = psvm.SpeakerHonorium;
                objpr.AdminSpeakerHonoriumTaxes = psvm.SpeakerHonoriumTaxes;
                objpr.AdminSpeakerExpense = psvm.SpeakerExpenses;
                objpr.AdminSpeakerExpenseTaxes = psvm.SpeakerExpensesTaxes;
                objpr.AdminModeratorExpense = psvm.ModeratorExpenses;
                objpr.AdminModeratorExpenseTaxes = psvm.ModeratorExpensesTaxes;


                objpr.AdminSpeakerPaymentMethod = "Cheque";
                objpr.AdminSpeakerPaymentSentDate = !(string.IsNullOrEmpty(psvm.SpeakerPaymentSentDate)) ? DateTime.ParseExact(psvm.SpeakerPaymentSentDate, "dd/MM/yyyy", null) : dt;

                objpr.AdminModeratorHonorium = psvm.ModeratorHonorium;
                objpr.AdminModeratorHonoriumTaxes = psvm.ModeratorHonoriumTaxes;


                objpr.AdminModeratorPaymentMethod = "Cheque";
                objpr.AdminModeratorPaymentSentDate = !(string.IsNullOrEmpty(psvm.ModeratorPaymentSentDate)) ? DateTime.ParseExact(psvm.ModeratorPaymentSentDate, "dd/MM/yyyy", null) : dt;




                objpr.AdminEditDate = DateTime.Now;
                objpr.AdminUserID = psvm.UserID;

                Entities.SaveChanges();
                //PatientID = objTempMAF.PatientID;
            }

        }

        public PayeeModel GetPayeeByUserID(int UserID)
        {

            PayeeModel payee = new PayeeModel();
            var query = Entities.PayeeInfoes.Where(p => p.UserID == UserID).SingleOrDefault();


            if (query != null)
            {

                payee.UserId = query.UserID;
                payee.PaymentMethod = query.PaymentMethod;
                payee.PayableTo = query.ChequePayableTo;
                payee.IRN = query.InternalRefNum;
                payee.MailingAddress1 = query.MailingAddr1;
                payee.MailingAddress2 = query.MailingAddr2;
                payee.AttentionTo = query.AttentionTo;
                payee.City = query.City;
                payee.Province = query.ProvinceID;
                payee.PostalCode = query.PostalCode;
                payee.TaxNumber = query.TaxNumber;
                payee.Instructions = query.AdditionalInstructions;

            }else
            {
                payee.UserId = UserID;
                
            }

            return payee;
        }
        public void UpdatePayeeInformation(int userid)
        {
            CPDPortal.Data.UserRegistration UserReg = new CPDPortal.Data.UserRegistration();

            var user = Entities.UserRegistrations.Where(x => x.UserID == userid).SingleOrDefault();

            if (user != null)
            {
                user.PayeeForm = true;
                Entities.SaveChanges();

            }

            else
            {
                UserReg.UserID = userid;
                UserReg.PayeeForm = true;
                Entities.UserRegistrations.Add(UserReg);
                Entities.SaveChanges();

            }

        }
        public  bool UpdatePayee(PayeeModel payee)
        {
            

            var payeeInfo = Entities.PayeeInfoes.Where(p => p.UserID == payee.UserId).SingleOrDefault();

            try
            {
                if (payeeInfo != null)
                {

                    payeeInfo.UserID = payee.UserId.Value;
                    payeeInfo.PaymentMethod = payee.PaymentMethod;
                    payeeInfo.ChequePayableTo = payee.PayableTo;
                    payeeInfo.InternalRefNum = payee.IRN;
                    payeeInfo.MailingAddr1 = payee.MailingAddress1;
                    payeeInfo.MailingAddr2 = payee.MailingAddress2;
                    payeeInfo.AttentionTo = payee.AttentionTo;
                    payeeInfo.City = payee.City;
                    payeeInfo.ProvinceID = payee.Province;
                    payeeInfo.PostalCode = payee.PostalCode;
                    payeeInfo.TaxNumber = payee.TaxNumber;
                    payeeInfo.AdditionalInstructions = payee.Instructions;
                    payeeInfo.LastUpdated = DateTime.Now;

                    Entities.SaveChanges();
                    return true;
                }
                else
                {
                    CPDPortal.Data.PayeeInfo objPayee = new CPDPortal.Data.PayeeInfo();
                    objPayee.UserID = payee.UserId ?? 0;
                    objPayee.PaymentMethod = payee.PaymentMethod;
                    objPayee.ChequePayableTo = payee.PayableTo;
                    objPayee.InternalRefNum = payee.IRN;
                    objPayee.MailingAddr1 = payee.MailingAddress1;
                    objPayee.MailingAddr2 = payee.MailingAddress2;
                    objPayee.AttentionTo = payee.AttentionTo;
                    objPayee.City = payee.City;
                    objPayee.ProvinceID = payee.Province;
                    objPayee.PostalCode = payee.PostalCode;
                    objPayee.TaxNumber = payee.TaxNumber;
                    objPayee.AdditionalInstructions = payee.Instructions;
                    objPayee.LastUpdated = DateTime.Now;
                    Entities.PayeeInfoes.Add(objPayee);
                    UpdatePayeeInformation(payee.UserId??0);//update UserRegistration table
                    Entities.SaveChanges();
                    return true;
                }
            }catch (Exception e)
            {

                return false;
            }

        }

        public StatusChangeEmailViewModel GetStatusChangeByAdminEmail(int ProgramRequestID)
        {

            StatusChangeEmailViewModel sc = new StatusChangeEmailViewModel();

            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).FirstOrDefault();

            if (val != null)
            {

                sc.FirstName = val.ContactFirstName;
                sc.ProgramDate = val.ConfirmedSessionDate.HasValue ? val.ConfirmedSessionDate.Value.ToString("MMMM dd, yyyy") : "";
                sc.ProgramName = Entities.Programs.Where(x => x.ProgramID == val.ProgramID).Select(x => x.ProgramName).SingleOrDefault();
                sc.EventID = val.AdminSessionID;
                sc.Email = Entities.UserInfoes.Where(x => x.UserID == val.UserID).Select(x => x.EmailAddress).SingleOrDefault();

            }

            return sc;


        }


        public bool CheckConfirmedSessionDate(int ProgramRequestID)
        {
            bool retVal = false;

            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).FirstOrDefault();

            if (val != null)
            {

                if (val.ConfirmedSessionDate.HasValue)
                {
                    retVal = true;

                }

            }


            return retVal;
        }

        public bool CheckAdminSessionID(int ProgramRequestID)
        {
            bool retVal = false;

            var val = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).FirstOrDefault();

            if (val != null)
            {

                if (!(string.IsNullOrEmpty(val.AdminSessionID)))
                {
                    retVal = true;

                }

            }


            return retVal;
        }

        public List<ProgramListViewModel> GetProgramList(int UserID)
        {

            List<ProgramListViewModel> list = new List<ProgramListViewModel>();

            var TherapeuticId = Entities.UserInfoes.Where(x => x.UserID == UserID).Select(x => x.TherapeuticID).SingleOrDefault();

            var ProgramList = Entities.TherapeuticPrograms.Where(x => x.TherapeuticID == TherapeuticId).ToList();


            foreach (var item in ProgramList)
            {

                list.Add(new ProgramListViewModel()
                {
                    Id = item.ProgramID ?? 0,
                    Name = Entities.Programs.Where(x => x.ProgramID == item.ProgramID).Select(x => x.ProgramName).FirstOrDefault()

                });

            }



            return list;


        }
        public List<ProgramRequestReportItem> GetProgramRequestReport()
        {

            List<ProgramRequestReportItem> list = new List<ProgramRequestReportItem>();

            //list = (from item in Entities.ProgramRequests
            //           select new ProgramRequestReportItem
            //           {
            //               ProgramRequestID = item.ProgramRequestID,
            //               SubmittedDate = item.SubmittedDate.HasValue ? item.ConfirmedSessionDate.Value.ToString("MMMM dd, yyyy") : "",
            //               ContactFirstName = item.ContactFirstName,
            //               ContactLastName = item.ContactLastName,
            //               RequestStatus = item.RequestStatusLookup.RequestStatusDescription,
            //               ConfirmedProgramDate = item.ConfirmedSessionDate.HasValue ? item.ConfirmedSessionDate.Value.ToString("MMMM dd, yyyy") : "",

            //               LocationName = item.LocationName,
            //               LocationAddress = item.LocationAddress,
            //               LocationCity = item.LocationCity,
            //               LocationProvince = item.LocationProvince,
            //               FinalAttendance = item.AdminFinalAttendance ?? 0

            //           }).ToList();
            try
            {
                
                var ProgramRequestList = Entities.ProgramRequests.Where(x => x.ProgramID != null && x.UserID != null && x.UserID > 169).ToList();
                
                foreach (var item in ProgramRequestList)
                {
                    string programName = Entities.Programs.Where(x => x.ProgramID == item.ProgramID).FirstOrDefault().ProgramName;
                    var result = from p in Entities.ProgramRequestSessionCredits.Where(x => x.ProgramRequestID == item.ProgramRequestID).ToList()
                                            join s in Entities.SessionCreditLookUps on p.SessionCreditID equals s.SessionCreditID
                                            select new
                                            {
                                                s.Description
                                            };
                    var sessionCreditList = result.ToList();
                    string moduleSelected = "";
                    for(int i = 0; i < sessionCreditList.Count; i++)
                    {
                        if (i < sessionCreditList.Count - 1)
                        {
                            moduleSelected += sessionCreditList[i].Description + ", ";
                        }else {
                            moduleSelected += sessionCreditList[i].Description;
                        }
                    }

                    list.Add(new ProgramRequestReportItem()
                    {
                        ProgramRequestID = item.ProgramRequestID,
                        SubmittedDate = item.ConfirmedSessionDate != null ? item.ConfirmedSessionDate.Value.ToString("MM/dd/yyyy") : "",
                        ContactFirstName = item.ContactFirstName,
                        ContactLastName = item.ContactLastName,
                        ProgramName = programName,
                        ModulesSelected = moduleSelected,
                        RequestStatus = item.RequestStatus.HasValue ? item.RequestStatusLookup.RequestStatusDescription : "",
                        ConfirmedProgramDate = (item.ConfirmedSessionDate != null) ? item.ConfirmedSessionDate.Value.ToString("MMMM dd, yyyy") : "",
                        Speaker = (item.SpeakerInfo != null) ? item.SpeakerInfo.FirstName + "," + item.SpeakerInfo.LastName : "",
                        SpeakerHonoraria = item.SpeakerInfo != null ? item.SpeakerInfo.SpeakerHonariumRange : "",
                        Moderator = (item.ModeratorInfo != null) ? item.ModeratorInfo.FirstName + "," + item.ModeratorInfo.LastName : "",
                        ModeratorHonoraria = item.ModeratorInfo != null ? item.ModeratorInfo.ModeratorHonariumRange : "",
                        LocationName = item.LocationName,
                        LocationAddress = item.LocationAddress,
                        
                        
                        LocationCity = item.LocationCity,
                        LocationProvince = item.LocationProvince,
                        FinalAttendance = item.AdminFinalAttendance ?? 0,
                        EventType=item.EventType
                        
                        
                        
                        

                    });

                }



                return list;
            }catch (Exception e)
            {
                UserHelper.WriteToLog("Error Message:" + e.Message);
                var st = new StackTrace(e, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();

                UserHelper.WriteToLog("Error Location:" + line);
                return list;
            }


        }




    }




}