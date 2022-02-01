using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CPDPortalMVC.Models;

namespace CPDPortalMVC.CustomValidation
{
    public class ValidateSessionCredits : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var pr = (ProgramRequest)validationContext.ObjectInstance;

            if (pr.ProgramID != 5 && pr.ProgramID != 7 && pr.ProgramID != 8)
            {
                if ((pr.SessionCredit1 == false) && (pr.SessionCredit2 == false) && (pr.SessionCredit3 == false) && (pr.SessionCredit4 == false) && (pr.SessionCredit5 == false))
                {
                    return new ValidationResult("Please check one of the check boxes");
                }
            }

            return ValidationResult.Success;
        }
    }
}