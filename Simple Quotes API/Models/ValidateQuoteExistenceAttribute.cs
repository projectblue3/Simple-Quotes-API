using Simple_Quotes_API.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_Quotes_API.Models
{
    public class ValidateQuoteExistenceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var quoteRepo = (IQuoteRepo)validationContext.GetService(typeof(IQuoteRepo));

            if (quoteRepo.QuoteExists(value.ToString()))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
