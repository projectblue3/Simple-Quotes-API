using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_Quotes_API.Models
{
    public class Quote
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Text { get; set; }

        [Required]
        public virtual Author Author { get; set; }
    }
}
