using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CPDPortalSpeaker.Models;

namespace CPDPortalSpeaker.CustomValidation
{
    public class ValidateProvinces : ValidationAttribute 
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var ActivationModel = (UserActivationModel)validationContext.ObjectInstance;

            bool AtLeastOncChecked = false;

            foreach(var item in ActivationModel.Provinces)
            {
                if (item.Checked == true)
                {
                    AtLeastOncChecked = true;

                }

            }

            if (AtLeastOncChecked == false)

                return new ValidationResult("Please Check at least one Province");
            else
                return ValidationResult.Success;
        }
    }
}