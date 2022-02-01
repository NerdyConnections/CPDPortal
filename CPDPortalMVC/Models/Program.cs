using CPDPortalMVC.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace CPDPortalMVC.Models
{
    public class Program
    {


        public int ProgramID { get; set; }
        public int? ProgramID_CHRC { get; set; }
        public string ProgramName { get; set; }
        public string DevelopedBy { get; set; }
        public string CertifiedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public string ExpirationDate { get; set; }
        public string TargetAudience { get; set; }
        public decimal? CreditHours { get; set; }
        public int EventsCompleted { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public DateTime? ProgramCompleted { get; set; }
        public bool? Archived { get; set; }



    }

  
}