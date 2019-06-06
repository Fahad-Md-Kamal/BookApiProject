using System.Collections.Generic;
using System.Linq;
using BookApi.Dtos;
using BookApi.Models;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : Controller
    {
        private readonly IAuthorRepository _AuthorRepository;
        private readonly IBookRepository _BookRepository;
        private readonly ICountryRepository _countryRepository;
        public AuthorsController(IAuthorRepository AuthorRepository, IBookRepository BookRepository, ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
            _BookRepository = BookRepository;
            _AuthorRepository = AuthorRepository;
        }



        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetAuthors()
        {
            var authors = _AuthorRepository.GetAuthors();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorsDto = new List<AuthorDto>();

            foreach (var author in authors)
            {
                authorsDto.Add(new AuthorDto
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                });
            }
            return Ok(authorsDto);
        }




        [HttpGet("{AuthorId}", Name= "GetAuthor")]
        [ProducesResponseType(200, Type = typeof(AuthorDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetAuthor(int AuthorId)
        {
            if (!_AuthorRepository.AuthorExists(AuthorId))
                return NotFound();

            var author = _AuthorRepository.GetAuthor(AuthorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorDto = new AuthorDto
            {
                Id = author.Id,
                FirstName = author.FirstName,
                LastName = author.LastName
            };
            return Ok(authorDto);
        }




        [HttpGet("Book/{BookId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<AuthorDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetAuthorsOfABook(int BookId)
        {
            if (!_BookRepository.BookExists(BookId))
                return NotFound();

            var authors = _AuthorRepository.GetAuthorsOfABook(BookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var authorsDto = new List<AuthorDto>();

            foreach (var author in authors)
            {
                authorsDto.Add(new AuthorDto
                {
                    Id = author.Id,
                    FirstName = author.FirstName,
                    LastName = author.LastName
                });
            }
            return Ok(authorsDto);
        }




        [HttpGet("{AuthorId}/Books")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetBooksByAuthor(int AuthorId)
        {
            if (!_AuthorRepository.AuthorExists(AuthorId))
                return NotFound();

            var books = _AuthorRepository.GetBooksByAuthor(AuthorId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var booksDto = new List<BookDto>();

            foreach (var Book in books)
            {
                booksDto.Add(new BookDto
                {
                    Id = Book.Id,
                    Title = Book.Title,
                    Isbn = Book.Isbn,
                    DatePublished = Book.DatePublished
                });
            }
            return Ok(booksDto);
        }





        [HttpPost]
        [ProducesResponseType(201,Type=typeof(Author))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateAuthor([FromBody]Author authorToCreate)
        {
            if (authorToCreate == null)
                return BadRequest(ModelState);
            
            if(!_countryRepository.CountryExists(authorToCreate.Country.Id))
            {
                ModelState.AddModelError("","Country doesn't exist!");
                return StatusCode(404, ModelState);
            }
            authorToCreate.Country = _countryRepository.GetCountry(authorToCreate.Country.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_AuthorRepository.CreateAuthor(authorToCreate))
            {
                ModelState.AddModelError("", $"Somthing went wrong while creating author {authorToCreate.FirstName} {authorToCreate.LastName}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetAuthor", new {authorId = authorToCreate.Id}, authorToCreate);

        }





        [HttpPut("{authorId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult UpdateAuthor(int authorId, [FromBody]Author authorToUpdate)
        {
            if (authorToUpdate == null)
                return BadRequest(ModelState);

            if(authorId != authorToUpdate.Id)
                return BadRequest(ModelState);

            if(!_AuthorRepository.AuthorExists(authorId))
                ModelState.AddModelError("","Author doesn't exist");
            
            if(!_countryRepository.CountryExists(authorToUpdate.Country.Id))
                ModelState.AddModelError("","Country doesn't exist!");
            
             if (!ModelState.IsValid)
                return StatusCode(404,ModelState);           
            
            authorToUpdate.Country = _countryRepository.GetCountry(authorToUpdate.Country.Id);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_AuthorRepository.UpdateAuthor(authorToUpdate))
            {
                ModelState.AddModelError("", $"Somthing went wrong while updating author {authorToUpdate.FirstName} {authorToUpdate.LastName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }



        [HttpDelete("{AuthorId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult DeleteAuthor(int AuthorId)
        {
            if(!_AuthorRepository.AuthorExists(AuthorId))
                return NotFound();
            
            var authorToDelete = _AuthorRepository.GetAuthor(AuthorId);

            if(_AuthorRepository.GetBooksByAuthor(AuthorId).Count() > 0)
            {
                ModelState.AddModelError("", $"Author {authorToDelete.FirstName} {authorToDelete.LastName} is associated with book(s)");
                return StatusCode(409, ModelState);
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if(!_AuthorRepository.DeleteAuthor(authorToDelete))
            {
                ModelState.AddModelError("",$"Something went wrong while deleteing author {authorToDelete.FirstName} {authorToDelete.LastName}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

    }
}