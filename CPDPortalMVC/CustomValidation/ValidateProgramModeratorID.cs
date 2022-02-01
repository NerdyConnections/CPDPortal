using CPDPortalMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.CustomValidation
{
    public class ValidateProgramModeratorID : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ProgramRequestIIModel model = (ProgramRequestIIModel)validationContext.ObjectInstance;
            if (model.ProgramModeratorID != null)
            {
                if (model.ProgramModeratorID == model.ProgramSpeakerID)
                {
                    return new ValidationResult("* Cannot be the same as speaker");
                }
            }

            return ValidationResult.Success;
        }
    }
}