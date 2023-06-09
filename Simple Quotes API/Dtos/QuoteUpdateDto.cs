using Simple_Quotes_API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_Quotes_API.Dtos
{
    public class QuoteUpdateDto
    {
        [Required]
        [MaxLength(1000)]
        //[ValidateQuoteExistence(ErrorMessage = "This quote already exists")]
        public string Text { get; set; }

        public bool? IsFeatured { get; set; }
    }
}
