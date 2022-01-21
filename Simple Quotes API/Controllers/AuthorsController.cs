using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
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
    public class AuthorsController : ControllerBase
    {
        //Repo fields
        private readonly IAuthorRepo _authorRepo;
        private readonly IQuoteRepo _quoteRepo;
        private readonly IMapper _mapper;

        //Controller Constructor
        public AuthorsController(IAuthorRepo authorRepo, IQuoteRepo quoteRepo, IMapper mapper)
        {
            _authorRepo = authorRepo;
            _quoteRepo = quoteRepo;
            _mapper = mapper;
        }

        //GET api/authors
        [HttpGet]
        public IActionResult GetAuthors([FromQuery] string searchTerms)
        {
            ICollection<Author> authorItems = new List<Author>();

            if (searchTerms == "isFeatured")
            {
                authorItems = _authorRepo.GetFeaturedAuthors();
            }
            else
            {
                authorItems = _authorRepo.GetAuthors();
            }

            if (authorItems.Count == 0)
            {
                return Ok("No Items");
            }

            var authorReadDtos = new List<AuthorReadDto>();

            foreach (var author in authorItems)
            {
                authorReadDtos.Add(new AuthorReadDto
                {
                    Id = author.Id,
                    Name = author.Name,
                    DateOfBirth = author.DateOfBirth,
                    DateOfDeath = author.DateOfDeath,
                    Occupation = author.Occupation,
                    Bio = author.Bio,
                    IsFeatured = author.IsFeatured
                });
            }

            return Ok(authorReadDtos);
        }

        //GET api/authors/id
        [HttpGet("{authorId}", Name="GetAuthorById")]
        public IActionResult GetAuthorById(int authorId)
        {
            var authorItem = _authorRepo.GetAuthor(authorId);

            if(authorItem == null)
            {
                return NotFound();
            }

            var authorReadDto = new AuthorReadDto()
            {
                Id = authorItem.Id,
                Name = authorItem.Name,
                DateOfBirth = authorItem.DateOfBirth,
                DateOfDeath = authorItem.DateOfDeath,
                Occupation = authorItem.Occupation,
                Bio = authorItem.Bio,
                IsFeatured = authorItem.IsFeatured
            };

            return Ok(authorReadDto);
        }

        //GET api/authors/id/quotes
        [HttpGet("{authorId}/quotes")]
        public IActionResult GetQuotesOfAuthor(int authorId)
        {
            if (!_authorRepo.AuthorExists(authorId))
            {
                return NotFound();
            }

            var quoteItems = _authorRepo.GetQuotesByAuthor(authorId);

            if (quoteItems.Count == 0)
            {
                List<string> noItems = new List<string>();
                return Ok(noItems);
            }

            var quoteAuthorDto = new List<QuoteAuthorDto>();

            foreach (var quote in quoteItems)
            {
                quoteAuthorDto.Add(new QuoteAuthorDto
                {
                    Id = quote.Id,
                    Text = quote.Text,
                    AuthorName = quote.Author.Name,
                    IsFeatured = quote.IsFeatured
                });
            }

            return Ok(quoteAuthorDto);
        }

        //POST api/authors
        [HttpPost]
        public IActionResult CreateAuthor(AuthorCreateDto authorCreateDto)
        {
            Author authorToCreate = new Author()
            {
                Name = authorCreateDto.Name.Trim(),
                DateOfBirth = authorCreateDto.DateOfBirth.Trim(),
                DateOfDeath = authorCreateDto.DateOfDeath.Trim(),
                Occupation = authorCreateDto.Occupation.Trim(),
                Bio = authorCreateDto.Bio.Trim(),
                IsFeatured = authorCreateDto.IsFeatured
            };

            if (authorToCreate == null)
            {
                return BadRequest(ModelState);
            }

            if (_authorRepo.AuthorExists(authorToCreate.Name))
            {
                ModelState.AddModelError("AuthorExists", "This author already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_authorRepo.CreateAuthor(authorToCreate))
            {
                ModelState.AddModelError("AuthorCreationError", $"Something went wrong creating the author");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute(nameof(GetAuthorById), new { authorId = authorToCreate.Id }, authorToCreate);
        }

        //DELETE api/authors/id
        [HttpDelete("{authorId}")]
        public IActionResult DeleteAuthor(int authorId)
        {
            var authorToDelete = _authorRepo.GetAuthor(authorId);

            if (authorToDelete == null)
            {
                return NotFound();
            }

            if (_authorRepo.GetQuotesByAuthor(authorId).Count() > 0)
            {
                ModelState.AddModelError("AuthorDeleteError","Cannot delete an author who has quotes");
                return StatusCode(409, ModelState);
            }

            if (!_authorRepo.DeleteAuthor(authorToDelete))
            {
                ModelState.AddModelError("AuthorDeleteError", $"Something went wrong deleting the author");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        //PATCH api/authors/id
        [HttpPatch("{authorId}")]
        public IActionResult PatchAuthor(int authorId, [FromBody] JsonPatchDocument<AuthorUpdateDto> authorPatch)
        {
            var repoAuthor = _authorRepo.GetAuthor(authorId);

            if (repoAuthor == null)
            {
                return NotFound();
            }

            var authorToUpdate = _mapper.Map<AuthorUpdateDto>(repoAuthor);

            authorPatch.ApplyTo(authorToUpdate, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!TryValidateModel(authorToUpdate))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(authorToUpdate, repoAuthor);

            if (!_authorRepo.UpdateAuthor())
            {
                ModelState.AddModelError("AuthorUpdateError", $"Something went wrong updating the author");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
