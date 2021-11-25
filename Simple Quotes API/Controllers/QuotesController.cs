using Microsoft.AspNetCore.Mvc;
using Simple_Quotes_API.Dtos;
using Simple_Quotes_API.Models;
using Simple_Quotes_API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simple_Quotes_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        //Repo fields
        private readonly IAuthorRepo _authorRepo;
        private readonly IQuoteRepo _quoteRepo;

        //Controller Constructor
        public QuotesController(IAuthorRepo authorRepo, IQuoteRepo quoteRepo)
        {
            _authorRepo = authorRepo;
            _quoteRepo = quoteRepo;
        }

        //GET api/quotes
        [HttpGet]
        public IActionResult GetQuotes()
        {
            var quoteItems = _quoteRepo.GetQuotes();

            if (quoteItems.Count == 0)
            {
                return Ok("No Items");
            }

            var quoteReadDtos = new List<QuoteReadDto>();

            foreach(var quote in quoteItems)
            {
                quoteReadDtos.Add(new QuoteReadDto
                {
                    Id = quote.Id,
                    Text = quote.Text,
                    AuthorId = quote.Author.Id,
                    AuthorName = quote.Author.Name
                });
            }

            return Ok(quoteReadDtos);
        }

        //GET api/quotes/id
        [HttpGet("{quoteId}", Name = "GetQuoteById")]
        public IActionResult GetQuoteById(int quoteId)
        {
            var quoteItem = _quoteRepo.GetQuote(quoteId);

            if (quoteItem == null)
            {
                return NotFound();
            }

            var quoteReadDto = new QuoteReadDto()
            {
                Id = quoteItem.Id,
                Text = quoteItem.Text,
                AuthorId = quoteItem.Author.Id,
                AuthorName = quoteItem.Author.Name
            };

            return Ok(quoteReadDto);
        }

        //POST api/quotes
        [HttpPost]
        public IActionResult CreateQuote(QuoteCreateDto quoteCreateDto)
        {

            Quote quoteToCreate = new Quote()
            {
                Text = quoteCreateDto.Text.Trim(),
                AuthorId = quoteCreateDto.AuthorId
            };

            if (quoteToCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (_quoteRepo.QuoteExists(quoteToCreate.Text))
            {
                ModelState.AddModelError("QuoteExists", "This quote already exists");
                return StatusCode(422, ModelState);
            }

            _quoteRepo.CreateQuote(quoteToCreate);
            _quoteRepo.SaveChanges();

            return CreatedAtRoute(nameof(GetQuoteById), new { quoteId = quoteToCreate.Id }, quoteToCreate);
        }

        //DELETE api/quotes/id
        [HttpDelete]
        public IActionResult DeleteQuote(int quoteId)
        {
            var quoteToDelete = _quoteRepo.GetQuote(quoteId);

            if (quoteToDelete == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_quoteRepo.DeleteQuote(quoteToDelete))
            {
                ModelState.AddModelError("QuoteDeleteError", $"Something went wrong deleting quote");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
