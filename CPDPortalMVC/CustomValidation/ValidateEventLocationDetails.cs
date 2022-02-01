using CPDPortalMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.CustomValidation
{
    public class ValidateEventLocationDetails : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ProgramRequest model = (ProgramRequest)validationContext.ObjectInstance;
            /*if (model.ProgramID == 5)
            {
                if (model.VenueContacted == "Y" || model.VenueContacted == "N")
                {
                    if (value == null || value != null && value.ToString() == "")
                    {
                        return new ValidationResult(Resources.Resource.RequiredFieldWithAsterisk);
                    }
                }
            }else
            {*/
                if ((value is string && string.IsNullOrEmpty((string)value)) || value == null)
                {
                    return new ValidationResult(Resources.Resource.RequiredFieldWithAsterisk);
                }
            //}

            return ValidationResult.Success;
        }
    }
}