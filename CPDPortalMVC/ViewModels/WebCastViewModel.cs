using CPDPortalMVC.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CPDPortalMVC.ViewModels
{
    public class WebCastViewModel
    {

        public int UserID { get; set; }
        public int ProgramRequestID { get; set; }
        public int SponsorID { get; set; }
        public int ProgramID { get; set; }
            public string ContactName { get; set; }
            public string ContactFirstName { get; set; }
            public string ContactLastName { get; set; }
            public string ContactPhone { get; set; }
            public string ContactEmail { get; set; }

       
        public string SpeakerStatus { get; set; }
     
        //[ValidateFileSize]
        public HttpPostedFileBase sessionagenda_Uploader { get; set; }
        public string SessionAgendaFileName { get; set; }
        public string SessionAgendaFileExt { get; set; }
        public bool? SessionAgendaUploaded { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public string ConfirmedSessionDate { get; set; }
        [ValidateProgamDate1]
        public string ProgramDate1 { get; set; }
        [ValidateProgramDate2]
        public string ProgramDate2 { get; set; }
        [ValidateProgramDate3]
        public string ProgramDate3 { get; set; }
       
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
        public string ProgramStartTime { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
        public string ProgramEndTime { get; set; }

        public bool SessionCredit1 { get; set; }
        public bool SessionCredit2 { get; set; }
        public bool SessionCredit3 { get; set; }
        public bool SessionCredit4 { get; set; }
        [ValidateSessionCredits]
        public bool SessionCredit5 { get; set; }
        [ValidateVenueContacted]
        public string MultiSession { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
        public int ProgramSpeakerID { get; set; }
        public IEnumerable<SelectListItem> Speakers { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
        public string WebsiteURL { get; set; }
       
        [DataType(DataType.Currency)]
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
        public Decimal CostPerPerson { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
        public Decimal CostPerparticipants { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
        public string AVEquipment { get; set; }
        public string Comments { get; set; }

        public string SubmittedDate { get; set; }
        public int RequestStatusID { get; set; }  //returning both the requeststatus ID and its description

        public string RequestStatus { get; set; }
        public bool ReadOnly { get; set; }

        public bool Approved { get; set; }
        public bool ShowPopup { get; set; }

        public string AdminVenueConfirmed { get; set; }
        public bool AdminVenueNA { get; set; }
        public int AdminUserID { get; set; }
        public int IsAdmin { get; set; }
        public bool SpeakerChosenProgramDate { get; set; }
        public bool FromQueryStringBySalesRep { get; set; }








    }
}