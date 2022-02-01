using CPDPortalMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.CustomValidation
{
    public class ValidateFileSize : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var pr = (ProgramRequest)validationContext.ObjectInstance;

            if (pr.sessionagenda_Uploader.ContentLength > 20000)
                return new ValidationResult("Exceeded File Size Limit of 20 MB");
            else
                return ValidationResult.Success;
        }
    }
}