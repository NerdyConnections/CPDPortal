using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace CPDPortalSpeaker.Models
{
    public class SpeakerActivationModel
    {
        
            public int UserId { get; set; }
            [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
            public string FirstName { get; set; }
            [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
            public string LastName { get; set; }
            [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
            public string Phone { get; set; }
            public string AdditionalPhone { get; set; }
            [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
            public string Speciality { get; set; }
           
             public string Clinic { get; set; }
            [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
             public string Address { get; set; }
            [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
             public string City { get; set; }
            [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
            public string Province { get; set; }
            [Required]
            [EmailAddress]
            public string Username { get; set; }
            [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
            [DataType(DataType.Password)]
            public string Password { get; set; }
            [Compare("Password")]
            public string ConfirmPassword { get; set; }
           

    }
}