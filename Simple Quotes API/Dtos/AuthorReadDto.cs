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
    }
}
