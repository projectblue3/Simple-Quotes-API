using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_Quotes_API.Dtos
{
    public class AuthorUpdateDto
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
