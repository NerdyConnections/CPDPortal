using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CPDPortalMVC.Models;

namespace CPDPortalMVC.CustomValidation
{
    public class ValidateTerritories : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var ActivationModel = (UserActivationModel)validationContext.ObjectInstance;
            return ValidationResult.Success;
            //if ((ActivationModel.UserType == 5) || (ActivationModel.UserType == 6))
            //{
            //    return ValidationResult.Success;

            //}
            //else
            //{
            //    if (string.IsNullOrEmpty(ActivationModel.Territories))
            //    {

            //        return new ValidationResult("Please Check at least one Territory");
            //    }

            //    else
            //        return ValidationResult.Success;


            //}       


        }
    }
}