using CPDPortalMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.CustomValidation
{
    public class ValidateSufficientKnowledge:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var gModel = (SpeakerModel)validationContext.ObjectInstance;

            if ((gModel.SufficientKnowledge == false))
            {
                return new ValidationResult("This is required");
            }

            else
                return ValidationResult.Success;
        }
    }
}