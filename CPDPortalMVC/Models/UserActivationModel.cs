using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CPDPortalMVC.CustomValidation;

namespace CPDPortalMVC.Models
{
    public class UserActivationModel
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
        public string Province { get; set; }
        [ValidateProvinces]
        public List<ProvinceModel> Provinces { get; set; }
        [ValidateTerritories]
        public string Territories { get; set; }
        [Required]
        [EmailAddress]
        public string Username { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public int UserType { get; set; }
        
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}