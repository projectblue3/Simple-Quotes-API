using Simple_Quotes_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_Quotes_API.Services
{
    public interface IQuoteRepo
    {
        ICollection<Quote> GetQuotes();
        Quote GetQuote(int quoteId);
        bool QuoteExists(string quoteText);
        bool QuoteExists(int quoteId);
        void CreateQuote(Quote quote);
        bool UpdateQuote(Quote quote);
        bool DeleteQuote(Quote quote);
        bool SaveChanges();
    }
}
