using CPDPortalMVC.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CPDPortalMVC.Models
{

    public class ProgramRequest
    {

        public int ProgramRequestID { get; set; }
        public int UserID { get; set; }
        public int SponsorID { get; set; }
        public int ProgramID { get; set; }
        public string ContactInformation { get; set; }
        public string ContactName { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string SpeakerStatus { get; set; }
        public string ModeratorStatus { get; set; }
        //[ValidateFileSize]
        public HttpPostedFileBase sessionagenda_Uploader { get; set; }
        public virtual string SessionAgendaFileName { get; set; }
        public string SessionAgendaFileExt { get; set; }
        public bool? SessionAgendaUploaded { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
        public string ConfirmedSessionDate { get; set; }

        public virtual string ProgramDate1 { get; set; }

        public string ProgramDate2 { get; set; }

        public string ProgramDate3 { get; set; }
        [ValidateMealStartTime]
        public string MealStartTime { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
        public string ProgramStartTime { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
        public virtual string ProgramEndTime { get; set; }

        public bool SessionCredit1 { get; set; }
        public bool SessionCredit2 { get; set; }
        public bool SessionCredit3 { get; set; }
        public bool SessionCredit4 { get; set; }
        //for program id 7, speaker needs to select modules for the event
        public bool ModulesSelected { get; set; }
        [ValidateSessionCredits]
        public bool SessionCredit5 { get; set; }
        [ValidateSessionCredits]
        public bool SessionCredit6 { get; set; }
        [ValidateVenueContacted]
        public virtual string MultiSession { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
        public int ProgramSpeakerID { get; set; }
        public List<SelectListItem> Speakers { get; set; }

        public virtual int? ProgramModeratorID { get; set; }
        public List<SelectListItem> Moderators { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
        public string VenueContacted { get; set; }
        [ValidateEventLocationDetails]
        public string LocationType { get; set; }
        [ValidateEventLocationDetails]
        public string LocationName { get; set; }
        [ValidateEventLocationDetails]
        public string LocationAddress { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
        public string LocationCity { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
        public string LocationProvince { get; set; }
        public string LocationProvinceFullName { get; set; }
        public string LocationPhoneNumber { get; set; }

        public string LocationWebsite { get; set; }
        [ValidateMealType]
        public string MealType { get; set; }
        [DataType(DataType.Currency)]
        [ValidateEventLocationDetails]
        public Decimal? CostPerPerson { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
        public Decimal CostPerparticipants { get; set; }

        [ValidateEventLocationDetails]
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
        public bool ModeratorChosenProgramDate { get; set; }
        public bool FromQueryStringBySalesRep { get; set; }
        public HttpPostedFileBase File { get; set; }
        public string message { get; set; }
        public string AdminSessionID { get; set; }
        public decimal TotalCredits { get; set; }
        public string ProgramID_CHRC { get; set; }
        public bool Archive { get; set; }
            public int DisplayOrder{get; set;}
        
    }
}