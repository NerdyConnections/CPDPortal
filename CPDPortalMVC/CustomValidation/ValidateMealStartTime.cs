using CPDPortalMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.CustomValidation
{
    public class ValidateMealStartTime : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ProgramRequest model = (ProgramRequest)validationContext.ObjectInstance;
           // if (model.ProgramID == 5 || model.ProgramID == 7)
           // {
                if (string.IsNullOrEmpty(model.MealStartTime) && !string.IsNullOrEmpty(((ProgramRequestIIModel)model).MealOption) && !((ProgramRequestIIModel)model).MealOption.Equals("no"))
                {
                    return new ValidationResult(Resources.Resource.RequiredFieldWithAsterisk);
                }
            //}else
           // {
              //  if (string.IsNullOrEmpty(model.MealStartTime))
               // {
                    //return new ValidationResult(Resources.Resource.RequiredFieldWithAsterisk);
               // }
            //}

            return ValidationResult.Success;
        }
    }
}