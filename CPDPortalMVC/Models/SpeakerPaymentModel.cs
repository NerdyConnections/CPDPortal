using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.Models
{
    public class SpeakerPaymentModel
    {
        public int PaymentID { get; set; }
        public int ProgramRequestID { get; set; }
        public int Speakeruserid { get; set; }
        
      
        public decimal? PaymentAmount { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? PaymentDate { get; set; }
    }
}