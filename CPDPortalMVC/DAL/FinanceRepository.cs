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
    public class FinanceRepository : BaseRepository
    {
        public List<FinanceModel> GetAllFinances(int UserID, int ProgramID)
        {

         return Entities
            .ProgramRequests
            .Where( x => (x.UserID == UserID) && (x.ProgramID == ProgramID))

            .Select(x =>

            new FinanceModel()
            {


                EventInfo = (x.SubmittedDate == null) ? null : SqlFunctions.DateName("year", x.SubmittedDate) + "/" + SqlFunctions.DatePart("m", x.SubmittedDate) + "/" + SqlFunctions.DateName("day", x.SubmittedDate) + "<br/>" + x.SpeakerInfo.FirstName + "," + x.SpeakerInfo.LastName + (x.ProgramSpeakerID != null ? ("/" + x.ModeratorInfo.FirstName + "," + x.ModeratorInfo.LastName) : "") + "<br/>" + x.LocationName,

                CFPC = (x.AdminCFPCFees.HasValue ? x.AdminCFPCFees.Value : 0.0M) + (x.AdminCFPCImplementationFees.HasValue ? x.AdminCFPCImplementationFees.Value : 0.0M),

                VenueFee = x.AdminVenueFees.HasValue ? x.AdminVenueFees.Value : 0.0M,

                AVFees = x.AdminAVFees.HasValue ? x.AdminAVFees.Value : 0.0M,

                SpeakerFees = (x.AdminSpeakerHonorium.HasValue ? x.AdminSpeakerHonorium.Value : 0.0M) + (x.AdminSpeakerExpense.HasValue ? x.AdminSpeakerExpense.Value : 0.0M),

                ModeratorFees = (x.AdminModeratorHonorium.HasValue ? x.AdminModeratorHonorium.Value : 0.0M) + (x.AdminModeratorExpense.HasValue ? x.AdminModeratorExpense.Value : 0.0M),


                OtherFees = x.AdminOtherFees.HasValue ? x.AdminOtherFees.Value : 0.0M,

                SubTotal = (x.AdminCFPCFees.HasValue ? x.AdminCFPCFees.Value : 0.0M) +
                           (x.AdminCFPCImplementationFees.HasValue ? x.AdminCFPCImplementationFees.Value : 0.0M) +
                           (x.AdminVenueFees.HasValue ? x.AdminVenueFees.Value : 0.0M) +
                           (x.AdminAVFees.HasValue ? x.AdminAVFees.Value : 0.0M) +
                           (x.AdminSpeakerHonorium.HasValue ? x.AdminSpeakerHonorium.Value : 0.0M) + (x.AdminSpeakerExpense.HasValue ? x.AdminSpeakerExpense.Value : 0.0M) +
                           (x.AdminModeratorHonorium.HasValue ? x.AdminModeratorHonorium.Value : 0.0M) + (x.AdminModeratorExpense.HasValue ? x.AdminModeratorExpense.Value : 0.0M) +
                           (x.AdminOtherFees.HasValue ? x.AdminOtherFees.Value : 0.0M),


                TaxesCombined = (x.AdminCFPCFeesTaxes.HasValue ? x.AdminCFPCFeesTaxes.Value : 0.0M) +
                                (x.AdminCFPCImplementationFeesTaxes.HasValue ? x.AdminCFPCImplementationFeesTaxes.Value : 0.0M) +
                                (x.AdminVenueFeesTaxes.HasValue ? x.AdminVenueFeesTaxes.Value : 0.0M) +
                                (x.AdminAVFeesTaxes.HasValue ? +x.AdminAVFeesTaxes.Value : 0.0M) +

                                (x.AdminSpeakerHonoriumTaxes.HasValue ? +x.AdminSpeakerHonoriumTaxes.Value : 0.0M) + (x.AdminSpeakerExpenseTaxes.HasValue ? +x.AdminSpeakerExpenseTaxes.Value : 0.0M) +
                                (x.AdminModeratorHonoriumTaxes.HasValue ? x.AdminModeratorHonoriumTaxes.Value : 0.0M) + (x.AdminModeratorExpenseTaxes.HasValue ? x.AdminModeratorExpenseTaxes.Value : 0.0M) +
                                (x.AdminOtherFeesTaxes.HasValue ? x.AdminOtherFeesTaxes.Value : 0.0M),




                EventTotal = (x.AdminCFPCFees.HasValue ? x.AdminCFPCFees.Value : 0.0M) + (x.AdminCFPCImplementationFees.HasValue ? x.AdminCFPCImplementationFees.Value : 0.0M) +
                               (x.AdminVenueFees.HasValue ? x.AdminVenueFees.Value : 0.0M) +
                               (x.AdminAVFees.HasValue ? x.AdminAVFees.Value : 0.0M) +
                               (x.AdminSpeakerHonorium.HasValue ? x.AdminSpeakerHonorium.Value : 0.0M) + (x.AdminSpeakerExpense.HasValue ? x.AdminSpeakerExpense.Value : 0.0M) +
                               (x.AdminModeratorHonorium.HasValue ? x.AdminModeratorHonorium.Value : 0.0M) + (x.AdminModeratorExpense.HasValue ? x.AdminModeratorExpense.Value : 0.0M)+
                               (x.AdminOtherFees.HasValue ? x.AdminOtherFees.Value : 0.0M) +
                               (x.AdminCFPCFeesTaxes.HasValue ? x.AdminCFPCFeesTaxes.Value : 0.0M) +
                               (x.AdminCFPCImplementationFeesTaxes.HasValue ? x.AdminCFPCImplementationFeesTaxes.Value : 0.0M) +
                               (x.AdminVenueFeesTaxes.HasValue ? x.AdminVenueFeesTaxes.Value : 0.0M) +
                               (x.AdminAVFeesTaxes.HasValue ? +x.AdminAVFeesTaxes.Value : 0.0M) +
                               (x.AdminSpeakerHonoriumTaxes.HasValue ? +x.AdminSpeakerHonoriumTaxes.Value : 0.0M) +
                               (x.AdminSpeakerExpenseTaxes.HasValue ? +x.AdminSpeakerExpenseTaxes.Value : 0.0M) +
                               (x.AdminModeratorHonoriumTaxes.HasValue ? x.AdminModeratorHonoriumTaxes.Value : 0.0M) +
                               (x.AdminModeratorExpenseTaxes.HasValue ? x.AdminModeratorExpenseTaxes.Value : 0.0M) +
                               (x.AdminOtherFeesTaxes.HasValue ? x.AdminOtherFeesTaxes.Value : 0.0M),




            }).ToList();

        }
    }
}