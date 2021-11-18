using Simple_Quotes_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_Quotes_API.Services
{
    public class AuthorRepo : IAuthorRepo
    {
        private readonly QuotesDbContext _authorContext;
        public AuthorRepo(QuotesDbContext authorContext)
        {
            _authorContext = authorContext;
        }

        public bool AuthorExists(int authorId)
        {
            throw new NotImplementedException();
        }

        public bool CreateAuthor(Author author)
        {
            _authorContext.Authors.Add(author);
            return SaveChanges();
        }

        public bool DeleteAuthor(Author author)
        {
            throw new NotImplementedException();
        }

        public Author GetAuthor(int authorID)
        {
            return _authorContext.Authors.Where(a => a.Id == authorID).FirstOrDefault();
        }

        public ICollection<Author> GetAuthors()
        {
            return _authorContext.Authors.ToList();
        }

        public ICollection<Quote> GetQuotesByAuthor(int authorID)
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            throw new NotImplementedException();
        }

        public bool UpdateAuthor(Author author)
        {
            throw new NotImplementedException();
        }
    }
}
