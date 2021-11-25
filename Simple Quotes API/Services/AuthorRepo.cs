using Microsoft.EntityFrameworkCore;
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

        public bool AuthorExists(string authorName)
        {
            return _authorContext.Authors.Any(a => a.Name == authorName);
        }

        public bool AuthorExists(int authorID)
        {
            return _authorContext.Authors.Any(a => a.Id == authorID);
        }

        public void CreateAuthor(Author author)
        {
            _authorContext.Authors.Add(author);
        }

        public bool DeleteAuthor(Author author)
        {
            _authorContext.Remove(author);
            return SaveChanges();
        }

        public Author GetAuthor(int authorID)
        {
            return _authorContext.Authors.Where(a => a.Id == authorID).FirstOrDefault();
        }

        public Author GetAuthor(string authorName)
        {
            return _authorContext.Authors.Where(a => a.Name == authorName).FirstOrDefault();
        }

        public ICollection<Author> GetAuthors()
        {
            return _authorContext.Authors.Include(a => a.Quotes).ToList();
        }

        public ICollection<Quote> GetQuotesByAuthor(int authorID)
        {
            return _authorContext.Quotes.Where(a => a.Author.Id == authorID).Include(q => q.Author).ToList();
        }

        public bool SaveChanges()
        {
            return _authorContext.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateAuthor(Author author)
        {
            throw new NotImplementedException();
        }
    }
}
