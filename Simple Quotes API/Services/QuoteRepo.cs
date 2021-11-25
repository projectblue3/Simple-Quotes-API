using Microsoft.EntityFrameworkCore;
using Simple_Quotes_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_Quotes_API.Services
{
    public class QuoteRepo : IQuoteRepo
    {
        private readonly QuotesDbContext _quoteContext;
        public QuoteRepo(QuotesDbContext quoteContext)
        {
            _quoteContext = quoteContext;
        }
        public void CreateQuote(Quote quote)
        {
            _quoteContext.Quotes.Add(quote);
        }

        public bool DeleteQuote(Quote quote)
        {
            throw new NotImplementedException();
        }

        public Quote GetQuote(int quoteId)
        {
            return _quoteContext.Quotes.Where(q => q.Id == quoteId).Include(q => q.Author).FirstOrDefault();
        }

        public ICollection<Quote> GetQuotes()
        {
            return _quoteContext.Quotes.Include(q => q.Author).ToList();
        }

        public bool QuoteExists(int quoteId)
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            return _quoteContext.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateQuote(Quote quote)
        {
            throw new NotImplementedException();
        }
    }
}
