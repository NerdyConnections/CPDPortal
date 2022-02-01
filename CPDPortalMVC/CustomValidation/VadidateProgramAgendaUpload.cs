using CPDPortalMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.CustomValidation
{
    public class VadidateProgramAgendaUpload : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ProgramRequestIIModel model = (ProgramRequestIIModel)validationContext.ObjectInstance;
            if (model.MultiSession == "Y")
            {
                if (string.IsNullOrEmpty((string)value))
                {
                    return new ValidationResult(Resources.Resource.RequiredFieldWithAsterisk);
                }
            }

            return ValidationResult.Success;
        }
    }
}