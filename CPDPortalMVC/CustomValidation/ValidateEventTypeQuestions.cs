using CPDPortalMVC.Models;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CPDPortalMVC.CustomValidation
{
    public class ValidateEventTypeQuestions : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ProgramRequestIIModel model = (ProgramRequestIIModel)validationContext.ObjectInstance;
            if (model.EventType == "Webcast")
            {
                if (string.IsNullOrEmpty((string)value))
                {
                    return new ValidationResult(Resources.Resource.RequiredFieldWithAsterisk);
                }
            }

            return ValidationResult.Success;
        }
    }
}