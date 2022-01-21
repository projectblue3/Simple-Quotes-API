using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_Quotes_API.Dtos
{
    public class QuoteAuthorDto
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public string AuthorName { get; set; }
        public bool? IsFeatured { get; set; }
    }
}
