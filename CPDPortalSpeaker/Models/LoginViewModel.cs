using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace CPDPortalSpeaker.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = null, ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "EMAIL_INVALID")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Resource), ErrorMessageResourceName = "RequiredField")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

}