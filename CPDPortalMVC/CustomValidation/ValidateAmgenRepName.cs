using CPDPortalMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.CustomValidation
{
    public class ValidateAmgenRepName : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ProgramRequest model = (ProgramRequest)validationContext.ObjectInstance;
          
                /*if (model.VenueContacted == "Y" || model.VenueContacted == "N")
                {
                    if (string.IsNullOrEmpty((string)value) && (!string.IsNullOrEmpty(model.MealStartTime) || (((ProgramRequestIIModel)model).MealOption != null && !((ProgramRequestIIModel)model).MealOption.Equals("no"))))
                    {
                        return new ValidationResult(Resources.Resource.RequiredFieldWithAsterisk);
                    }
                }*/
                if (string.IsNullOrEmpty((string)value) && ((ProgramRequestIIModel)model).ProgramCostsSplit.Equals("Y"))
                {
                    return new ValidationResult(Resources.Resource.RequiredFieldWithAsterisk);
                }
            
           

            return ValidationResult.Success;
        }
    }
}