using CPDPortalMVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.DAL
{
    public class PostSessionRepository : BaseRepository
    {

        public PostSessionViewModel GetPostSessionByProgramRequestID(int ProgramRequestID)
        {
            int UserID = Util.UserHelper.GetLoggedInUser().UserID;
            PostSessionViewModel psvm = new PostSessionViewModel();

            var objPr = Entities.ProgramRequests.Where(x => x.ProgramRequestID == ProgramRequestID).SingleOrDefault();

            if (objPr != null)
            {
                psvm.ProgramRequestID = ProgramRequestID;
                psvm.UserID = UserID;
                psvm.CCASent = objPr.AdminCCASent ?? false;
                psvm.VenueReceipt = objPr.AdminVenueReceipt ?? false;
                psvm.VenueNotes = objPr.AdminVenueNotes;
                psvm.CFPCDateApproved = objPr.AdminCFPCDateApproved.HasValue ? objPr.AdminCFPCDateApproved.Value.ToString("dd-MM-yyyy") : "";
                psvm.CFPCDateSubmitted = objPr.AdminCFPCDateSubmitted.HasValue ? objPr.AdminCFPCDateSubmitted.Value.ToString("dd-MM-yyyy") : "";
                psvm.CFPCFees = objPr.AdminCFPCFees;
                psvm.VenueFees = objPr.AdminVenueFees;
            
                psvm.AdminSessionID = objPr.AdminSessionID;
                psvm.CFPCFeeTaxes = objPr.AdminCFPCFeesTaxes;
                psvm.CFPCImplementationFees = objPr.AdminCFPCImplementationFees;
                psvm.CFPCImplementationFeesTaxes = objPr.AdminCFPCImplementationFeesTaxes;
                psvm.SpeakerExpenses = objPr.AdminSpeakerExpense;
                psvm.SpeakerExpensesTaxes = objPr.AdminSpeakerExpenseTaxes;
                psvm.ModeratorExpenses = objPr.AdminModeratorExpense;
                psvm.ModeratorExpensesTaxes = objPr.AdminModeratorExpenseTaxes;


                psvm.AVFees = objPr.AdminAVFees;
                psvm.OtherFees = objPr.AdminOtherFees;

                psvm.VenueFeesTaxes = objPr.AdminVenueFeesTaxes;
                psvm.AVFeesTaxes = objPr.AdminAVFeesTaxes;
                psvm.OtherFeesTaxes = objPr.AdminOtherFeesTaxes;

                psvm.FinalAttendance = objPr.AdminFinalAttendance;
                psvm.SpeakerHonorium = objPr.AdminSpeakerHonorium;
                psvm.SpeakerHonoriumTaxes = objPr.AdminSpeakerHonoriumTaxes;
                psvm.SpeakerPaymentMethod = objPr.AdminSpeakerPaymentMethod;
                psvm.SpeakerPaymentSentDate = objPr.AdminSpeakerPaymentSentDate.HasValue ? objPr.AdminSpeakerPaymentSentDate.Value.ToString("dd-MM-yyyy") : "";
                psvm.ModeratorHonorium = objPr.AdminModeratorHonorium;
                psvm.ModeratorHonoriumTaxes = objPr.AdminModeratorHonoriumTaxes;
                psvm.ModeratorPaymentMethod = objPr.AdminModeratorPaymentMethod;
                psvm.ModeratorPaymentSentDate = objPr.AdminModeratorPaymentSentDate.HasValue ? objPr.AdminModeratorPaymentSentDate.Value.ToString("dd-MM-yyyy") : "";


            }
            return psvm;

        }
    }
}