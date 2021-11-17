using Microsoft.EntityFrameworkCore;
using Simple_Quotes_API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_Quotes_API.Services
{
    public class QuotesDbContext : DbContext
    {
        public QuotesDbContext(DbContextOptions<QuotesDbContext> options) : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quote>()
                .HasOne<Author>(q => q.Author)
                .WithMany(a => a.Quotes)
                .HasForeignKey(q => q.QuoteAuthorId);
        }

        public virtual DbSet<Quote> Quotes { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
    }
}
