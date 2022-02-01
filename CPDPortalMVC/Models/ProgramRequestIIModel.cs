using CPDPortalMVC.CustomValidation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CPDPortalMVC.Models
{
    public class ProgramRequestIIModel : ProgramRequest
    {/*
        public int UserID { get; set; }
        public int ProgramID { get; set; }
        public string ContactFirstName { get; set; }
        public string ContactLastName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public List<SelectListItem> Speakers { get; set; }
        public List<SelectListItem> Moderators { get; set; }*/



        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredFieldWithAsterisk")]
        public string EventType { get; set; }
        [ValidateEventTypeQuestions]
        public string EventTypeQuestion1 { get; set; }
        [ValidateEventTypeQuestions]
        public string EventTypeQuestion2 { get; set; }
        [ValidateEventTypeQuestions]
        public string EventTypeQuestion3 { get; set; }
        [ValidateEventTypeQuestions]
        public string EventTypeQuestion4 { get; set; }
        [ValidateEventTypeQuestions]
        public string EventTypeQuestion5 { get; set; }


        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredFieldWithAsterisk")]
        public override string ProgramDate1 { get; set; }
        //[Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredFieldWithAsterisk")]
        //public string ProgramStartTime { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredFieldWithAsterisk")]
        [ValidateProgramEndTime]
        public override string ProgramEndTime { get; set; }
        public string RegistrationArrivalTime { get; set; }
        //[ValidateMealStartTime]
        //public override string MealStartTime { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredFieldWithAsterisk")]
        public string MealOption { get; set; }
        //public bool SessionCredit1 { get; set; }
        //public bool SessionCredit2 { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredFieldWithAsterisk")]
        public override string MultiSession { get; set; }
        [VadidateProgramAgendaUpload]
        public override string SessionAgendaFileName { get; set; }       
        //public bool? SessionAgendaUploaded { get; set; }
        //[DataType(DataType.Currency)]
        //[Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredFieldWithAsterisk")]
        //public Decimal CostPerparticipants { get; set; }

        //[Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredFieldWithAsterisk")]
        //public int ProgramSpeakerID { get; set; }
        [ValidateProgramModeratorID]
        public override int? ProgramModeratorID { get; set; }

        


        //[Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredFieldWithAsterisk")]
        //public string VenueContacted { get; set; }
        //[ValidateEventLocationDetails]
        //public override string LocationType { get; set; }
        [ValidateLocationTypeOther]
        public string LocationTypeOther { get; set; }
        /*[ValidateEventLocationDetails]
        public override string LocationName { get; set; }
        [ValidateEventLocationDetails]
        public override string LocationAddress { get; set; }
        [ValidateEventLocationDetails]
        public override string LocationCity { get; set; }
        [ValidateEventLocationDetails]
        public override string LocationProvince { get; set; }*/

        //public string LocationPhoneNumber { get; set; }

        //public string LocationWebsite { get; set; }
        //[ValidateMealType]
        //public override string MealType { get; set; }
        //[DataType(DataType.Currency)]
        //[ValidateEventLocationDetails]
        //public override Decimal CostPerPerson { get; set; }      

        //[ValidateEventLocationDetails]
        //public override string AVEquipment { get; set; }
        //public string Comments { get; set; }
        /*
                public string SubmittedDate { get; set; }

                public string AdminVenueConfirmed { get; set; }
                public bool AdminVenueNA { get; set; }
                public int AdminUserID { get; set; }
                public int IsAdmin { get; set; }

                public int ProgramRequestID { get; set; }*/

        public bool SessionCredit14 { get; set; }
        public bool SessionCredit15 { get; set; }
        public bool SessionCredit16 { get; set; }
        public bool SessionCredit17 { get; set; }
        public bool SessionCredit18 { get; set; }
        public bool SessionCredit19 { get; set; }
        public bool SessionCredit20 { get; set; }
        public bool SessionCredit21 { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
        public string ProgramCostsSplit { get; set; }
        [ValidateAmgenRepName]
        public string AmgenRepName { get; set; }
    }
}