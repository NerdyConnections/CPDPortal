using CPDPortalMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.CustomValidation
{
    public class ValidateProgramEndTime : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ProgramRequestIIModel model = (ProgramRequestIIModel)validationContext.ObjectInstance;
            if (!string.IsNullOrEmpty(model.ProgramDate1))
            {
                DateTime startTime = DateTime.ParseExact(model.ProgramDate1, "yyyy/MM/dd", null);
                DateTime programStartTime = startTime.Add(TimeSpan.Parse(model.ProgramStartTime));
                DateTime programEndTime = startTime.Add(TimeSpan.Parse(model.ProgramEndTime));
                if (programEndTime <= programStartTime)
                {
                    return new ValidationResult("* Must be greater than start time");
                }
            }

            return ValidationResult.Success;
        }
    }
}