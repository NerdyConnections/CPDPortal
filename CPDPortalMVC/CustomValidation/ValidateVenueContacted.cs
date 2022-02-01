using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CPDPortalMVC.Models;

namespace CPDPortalMVC.CustomValidation
{
    public class ValidateVenueContacted : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var pr = (ProgramRequest)validationContext.ObjectInstance;

            if (pr.ProgramID != 5)
            {
                if (pr.IsAdmin == 1)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    if (string.IsNullOrEmpty(pr.VenueContacted))
                    {

                        return new ValidationResult("Please check one radio button");
                    }
                    else
                    {
                        return ValidationResult.Success;
                    }



                }
            }
            return ValidationResult.Success;

        }
    }
}