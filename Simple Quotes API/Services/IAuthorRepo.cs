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
        Author GetAuthor(string authorName);
        ICollection<Quote> GetQuotesByAuthor(int authorID);
        bool AuthorExists(int authorId);
        bool AuthorExists(string authorName);
        void CreateAuthor(Author author);
        bool UpdateAuthor();
        bool DeleteAuthor(Author author);
        bool SaveChanges();
    }
}
