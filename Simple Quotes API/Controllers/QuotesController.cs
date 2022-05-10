using AutoMapper;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        private readonly IMapper _mapper;

        //Controller Constructor
        public QuotesController(IAuthorRepo authorRepo, IQuoteRepo quoteRepo, IMapper mapper)
        {
            _authorRepo = authorRepo;
            _quoteRepo = quoteRepo;
            _mapper = mapper;
        }

        //GET api/quotes
        [HttpGet]
        public IActionResult GetQuotes([FromQuery]string searchTerms)
        {
            ICollection<Quote> quoteItems = new List<Quote>();

            if (searchTerms != null && searchTerms != "isFeatured")
            {
                quoteItems = _quoteRepo.GetSearch(searchTerms);
            }
            else if (searchTerms != null && searchTerms == "isFeatured")
            {
                quoteItems = _quoteRepo.GetFeaturedQuotes();
            }
            else
            {
                quoteItems = _quoteRepo.GetQuotes();
            } 

            if (quoteItems.Count == 0)
            {
                List<string> noItems = new List<string>();
                
                return Ok(noItems);
            }

            ICollection<QuoteReadDto> quoteReadDtos = new List<QuoteReadDto>();

            foreach(var quote in quoteItems)
            {
                quoteReadDtos.Add(new QuoteReadDto
                {
                    Id = quote.Id,
                    Text = quote.Text,
                    AuthorId = quote.Author.Id,
                    AuthorName = quote.Author.Name,
                    IsFeatured = quote.IsFeatured
                });
            }

            return Ok(quoteReadDtos.OrderBy(q => q.AuthorName).ToList());
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
                AuthorName = quoteItem.Author.Name,
                IsFeatured = quoteItem.IsFeatured
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
                AuthorId = quoteCreateDto.AuthorId,
                IsFeatured = quoteCreateDto.IsFeatured
            };

            var authorItem = _authorRepo.GetAuthor(quoteCreateDto.AuthorId);

            if (quoteToCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (authorItem == null)
            {
                ModelState.AddModelError("AuthorNotFoundError", "Author cannot be found");
                return StatusCode(409, new { messages = new List<string>() { "Author cannot be found" } });
            }

            if (!_quoteRepo.CreateQuote(quoteToCreate))
            {
                ModelState.AddModelError("QuoteCreationError", $"Something went wrong creating the quote");
                throw new CreationException("Something went wrong creating the quote");
            }

            return CreatedAtRoute(nameof(GetQuoteById), new { quoteId = quoteToCreate.Id }, quoteToCreate);
        }

        //DELETE api/quotes/id
        [HttpDelete("{quoteId}")]
        public IActionResult DeleteQuote(int quoteId)
        {
            var quoteToDelete = _quoteRepo.GetQuote(quoteId);

            if (quoteToDelete == null)
            {
                return NotFound();
            }

            if (!_quoteRepo.DeleteQuote(quoteToDelete))
            {
                ModelState.AddModelError("QuoteDeleteError", $"Something went wrong deleting quote");
                throw new DeletionException("Something went wrong deleting the quote");
            }

            return NoContent();
        }

        //PATCH api/quotes/id
        [HttpPatch("{quoteId}")]
        public IActionResult PatchQuote(int quoteId, [FromBody] JsonPatchDocument<QuoteUpdateDto> quotePatch)
        {
            var repoQuote = _quoteRepo.GetQuote(quoteId);

            if (repoQuote == null)
            {
                return NotFound();
            }

            var quoteToUpdate = _mapper.Map<QuoteUpdateDto>(repoQuote);

            quotePatch.ApplyTo(quoteToUpdate, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryValidateModel(quoteToUpdate))
            {
                return BadRequest(new
                {
                    Messages = ModelState.Values.SelectMany(x => x.Errors)
                            .Select(x => x.ErrorMessage)
                });
            }

            _mapper.Map(quoteToUpdate, repoQuote);

            if (!_quoteRepo.UpdateQuote())
            {
                ModelState.AddModelError("QuoteUpdateError", $"Something went wrong updating quote");
                throw new UpdateException("Something went wrong updating the quote");
            }

            return NoContent();
        }
    }
}
