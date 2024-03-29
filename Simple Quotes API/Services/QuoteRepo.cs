﻿using Microsoft.EntityFrameworkCore;
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
        public bool CreateQuote(Quote quote)
        {
            _quoteContext.Quotes.Add(quote);
            return SaveChanges();
        }

        public bool DeleteQuote(Quote quote)
        {
            _quoteContext.Remove(quote);
            return SaveChanges();
        }

        public ICollection<Quote> GetFeaturedQuotes()
        {
            return _quoteContext.Quotes.Where(q => q.IsFeatured == true).Include(q => q.Author).ToList();
        }

        public Quote GetQuote(int quoteId)
        {
            return _quoteContext.Quotes.Where(q => q.Id == quoteId).Include(q => q.Author).FirstOrDefault();
        }

        public ICollection<Quote> GetQuotes()
        {
            return _quoteContext.Quotes.Include(q => q.Author).ToList();
        }

        public ICollection<Quote> GetSearch(string searchTerms)
        {
            return _quoteContext.Quotes.Where(q => q.Text.Contains(searchTerms) || q.Author.Name.Contains(searchTerms)).Include(q => q.Author).ToList();
        }

        public bool QuoteExists(string quoteText)
        {
            return _quoteContext.Quotes.Any(q => q.Text == quoteText);
        }

        public bool QuoteExists(int quoteId)
        {
            return _quoteContext.Quotes.Any(q => q.Id == quoteId);
        }

        public bool SaveChanges()
        {
            return _quoteContext.SaveChanges() >= 0 ? true : false;
        }

        public bool UpdateQuote()
        {
            return SaveChanges();
        }
    }
}
