using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.ViewModels
{
    public class PostSessionViewModel
    {
        public int ProgramRequestID { get; set; }
        public int UserID { get; set; }

        public string VenueAvailable { get; set; }
        public bool CCASent { get; set; }
        public bool VenueReceipt { get; set; }
        public string VenueNotes { get; set; }

        public decimal? VenueFees { get; set; }
        public decimal? VenueFeesTaxes { get; set; }
        public decimal? AVFees { get; set; }
        public decimal? AVFeesTaxes { get; set; }
        public decimal? OtherFees { get; set; }
        public decimal? OtherFeesTaxes { get; set; }
        public decimal? CFPCFees { get; set; }

        public string AdminSessionID { get; set; }
        public decimal? CFPCFeeTaxes { get; set; }
        public decimal? CFPCImplementationFees { get; set; }
        public decimal? CFPCImplementationFeesTaxes { get; set; }
        public decimal? SpeakerExpenses { get; set; }
        public decimal? SpeakerExpensesTaxes { get; set; }
        public decimal? ModeratorExpenses { get; set; }
        public decimal? ModeratorExpensesTaxes { get; set; }

        public string CFPCDateSubmitted { get; set; }
        public string CFPCDateApproved { get; set; }


        public int? FinalAttendance { get; set; }

        public decimal? SpeakerHonorium { get; set; }
        public decimal? SpeakerHonoriumTaxes { get; set; }

        public string SpeakerPaymentMethod { get; set; }
        public string SpeakerPaymentSentDate { get; set; }

        public decimal? ModeratorHonorium { get; set; }
        public decimal? ModeratorHonoriumTaxes { get; set; }
        public string ModeratorPaymentMethod { get; set; }
        public string ModeratorPaymentSentDate { get; set; }

        public DateTime LastUpdatedDate { get; set; }

    }
}