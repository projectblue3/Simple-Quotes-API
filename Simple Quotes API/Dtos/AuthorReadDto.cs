using Simple_Quotes_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_Quotes_API.Dtos
{
    public class AuthorReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string DateOfDeath { get; set; }
        public string Occupation { get; set; }
        public string Bio { get; set; }
    }
}
