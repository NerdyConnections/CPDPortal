using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CPDPortalMVC.Models;

namespace CPDPortalMVC.CustomValidation
{
    public class ValidateProgramDate2 : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var pr = (ProgramRequest)validationContext.ObjectInstance;

           if(!string.IsNullOrEmpty(pr.ProgramDate2))
            {
                DateTime ProgramDate2 = DateTime.ParseExact(pr.ProgramDate2, "yyyy/MM/dd", null);
                
                DateTime ProgramDate1 = DateTime.ParseExact(pr.ProgramDate1, "yyyy/MM/dd", null);
                DateTime today = DateTime.Now;

                var daycount = (ProgramDate2 - today).TotalDays;

                if (daycount <= 20)
                {
                    return new ValidationResult("Please Enter Date four weeks from Today");
                }

                    
                if(ProgramDate2 == ProgramDate1)
                {
                    return new ValidationResult("Dates cannot be same");

                }
                else
                    return ValidationResult.Success;

            }
           else
            {
                return ValidationResult.Success;

            }      
           
           
        }
    }
}