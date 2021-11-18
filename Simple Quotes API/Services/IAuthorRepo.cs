using Simple_Quotes_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_Quotes_API.Services
{
    public interface IAuthorRepo
    {
        ICollection<Author> GetAuthors();
        Author GetAuthor(int authorID);
        ICollection<Quote> GetQuotesByAuthor(int authorID);
        bool AuthorExists(int authorId);
        bool CreateAuthor(Author author);
        bool UpdateAuthor(Author author);
        bool DeleteAuthor(Author author);
        bool SaveChanges();
    }
}
