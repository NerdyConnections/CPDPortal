using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CPDPortalMVC.Models;

namespace CPDPortalMVC.CustomValidation
{
    public class ValidateProgamDate1 : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var pr = (ProgramRequest)validationContext.ObjectInstance;

            DateTime today = DateTime.Now;
            DateTime ProgramDate1 = DateTime.ParseExact(pr.ProgramDate1, "yyyy/MM/dd", null);

            var daycount = (ProgramDate1 - today).TotalDays;

            if (daycount <= 20)
        
                return new ValidationResult("Please Enter Date four weeks from Today");
            else
                return ValidationResult.Success;
        }
    }
}