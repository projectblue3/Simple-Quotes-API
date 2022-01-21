using Simple_Quotes_API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_Quotes_API.Dtos
{
    public class QuoteCreateDto
    {
        [Required]
        [MaxLength(1000)]
        public string Text { get; set; }

        public bool? IsFeatured { get; set; }

        [Required]
        public int AuthorId { get; set; }
    }
}
