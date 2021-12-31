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

        [Required]
        [MaxLength(64)]
        public string DateOfBirth { get; set; }

        [MaxLength(64)]
        public string DateOfDeath { get; set; }

        [Required]
        [MaxLength(64)]
        public string Occupation { get; set; }

        [Required]
        [MaxLength(512)]
        public string Bio { get; set; }
    }
}
