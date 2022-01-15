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
        ICollection<Quote> GetSearch(string queryTerms);
        Quote GetQuote(int quoteId);
        bool QuoteExists(string quoteText);
        bool QuoteExists(int quoteId);
        bool CreateQuote(Quote quote);
        bool UpdateQuote();
        bool DeleteQuote(Quote quote);
        bool SaveChanges();
    }
}
