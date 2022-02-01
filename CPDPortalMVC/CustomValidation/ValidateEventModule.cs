using CPDPortalMVC.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.CustomValidation
{
    public class ValidateEventModule : ValidationAttribute
    {
      
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var em = (EventModule)validationContext.ObjectInstance;

                int ModuleCount = 0;
            if (em.ProgramModule1)
                ModuleCount = ModuleCount++;
            if (em.ProgramModule2)
                ModuleCount = ModuleCount++;
            if (em.ProgramModule3)
                ModuleCount = ModuleCount++;
            if (em.ProgramModule4)
                ModuleCount = ModuleCount++;
            if (em.ProgramModule5)
                ModuleCount = ModuleCount++;
            if (em.ProgramModule6)
                ModuleCount = ModuleCount++;
            if (em.ProgramModule7)
                ModuleCount = ModuleCount++;
            if (em.ProgramModule8)
                ModuleCount = ModuleCount++;
            if (em.ProgramModule9)
                ModuleCount = ModuleCount++;
            if (em.ProgramModule10)
                ModuleCount = ModuleCount++;
            if (em.SessionCreditID==16  && ModuleCount > 3)
                return new ValidationResult("Modules selected are greater than Session Credit allowed 3 (1.0 Mainpro + Credits (1 hour))");
            else if (em.SessionCreditID == 17 && ModuleCount > 5)
                return new ValidationResult("Modules selected are greater than Session Credit allowed 5 (1.5 Mainpro + Credits (1.5 hour))");
            else if (em.SessionCreditID == 18 && ModuleCount > 7)
                return new ValidationResult("Modules selected are greater than Session Credit allowed 7 (2 Mainpro + Credits (2 hour))");
            else if (em.SessionCreditID == 19 && ModuleCount > 10)
                return new ValidationResult("Modules selected are greater than Session Credit allowed 10 (2.75 Mainpro + Credits (2 hour 45 min))");

            else
                return ValidationResult.Success;

            //if (pr.ProgramID != 5 && pr.ProgramID != 7)
            //    {
            //        if ((pr.SessionCredit1 == false) && (pr.SessionCredit2 == false) && (pr.SessionCredit3 == false) && (pr.SessionCredit4 == false) && (pr.SessionCredit5 == false))
            //        {
            //            return new ValidationResult("Please check one of the check boxes");
            //        }
            //    }


        }
        
    }
}