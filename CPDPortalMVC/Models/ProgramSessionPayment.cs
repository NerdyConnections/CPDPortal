using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.Models
{
    public class ProgramSessionPayment
    {
       
        public int ProgramRequestID { get; set; }
        public int userid { get; set; }
        public int ProgramSpeakerID { get; set; }
        public int ProgramModeratorID { get; set; }
        public string SpeakerFirstName { get; set; }
        public string SpeakerLastName { get; set; }
        public string SpeakerPaymentAmount { get; set; }
        public string SpeakerPaymentSentDate { get; set; }
        public string ModeratorFirstName { get; set; }
        public string ModeratorLastName { get; set; }
        public string ModeratorPaymentAmount { get; set; }
        public string ModeratorPaymentSentDate { get; set; }
        public string VenueFees { get; set; }
        public string OtherFees { get; set; }
        public string AVFees { get; set; }

        public string PaymentSentDate { get; set; }
    }
}