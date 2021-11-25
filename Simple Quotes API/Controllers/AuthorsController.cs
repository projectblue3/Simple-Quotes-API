﻿using Microsoft.AspNetCore.Mvc;
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

        //Controller Constructor
        public AuthorsController(IAuthorRepo authorRepo, IQuoteRepo quoteRepo)
        {
            _authorRepo = authorRepo;
            _quoteRepo = quoteRepo;
        }

        //GET api/authors
        [HttpGet]
        public IActionResult GetAuthors()
        {
            var authorItems = _authorRepo.GetAuthors();

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
                    Name = author.Name
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
                return Ok("No Items");
            }

            var quoteAuthorDto = new List<QuoteAuthorDto>();

            foreach (var quote in quoteItems)
            {
                quoteAuthorDto.Add(new QuoteAuthorDto
                {
                    Id = quote.Id,
                    Text = quote.Text,
                    AuthorName = quote.Author.Name
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
                Name = authorCreateDto.Name.Trim()
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

            _authorRepo.CreateAuthor(authorToCreate);
            _authorRepo.SaveChanges();

            return CreatedAtRoute(nameof(GetAuthorById), new { authorId = authorToCreate.Id }, authorToCreate);
        }

        [HttpDelete]
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

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_authorRepo.DeleteAuthor(authorToDelete))
            {
                ModelState.AddModelError("AuthorDeleteError", $"Something went wrong deleting the author");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
