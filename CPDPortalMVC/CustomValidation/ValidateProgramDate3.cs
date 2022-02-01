using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using CPDPortalMVC.Models;

namespace CPDPortalMVC.CustomValidation
{
    public class ValidateProgramDate3 : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var pr = (ProgramRequest)validationContext.ObjectInstance;
           
           
            DateTime ProgramDate1 = DateTime.ParseExact(pr.ProgramDate1, "yyyy/MM/dd", null);
            DateTime today = DateTime.Now;

            if (!string.IsNullOrEmpty(pr.ProgramDate3))
            {
                DateTime ProgramDate3 = DateTime.ParseExact(pr.ProgramDate3, "yyyy/MM/dd", null);
                var daycount = (ProgramDate3 - today).TotalDays;

                if (daycount <= 20)
                {
                    return new ValidationResult("Please Enter Date four weeks from Today");
                }


                if ((ProgramDate3 == ProgramDate1)) 
                {
                    return new ValidationResult("Dates cannot be same");

                }

                if (!string.IsNullOrEmpty(pr.ProgramDate2))
                {
                    DateTime ProgramDate2 = DateTime.ParseExact(pr.ProgramDate2, "yyyy/MM/dd", null);

                    if ((ProgramDate3 == ProgramDate2)) {

                        return new ValidationResult("Dates cannot be same");
                    }

                    return ValidationResult.Success;
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