using System.Collections.Generic;
using BookApi.Dtos;
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
        public AuthorsController(IAuthorRepository AuthorRepository, IBookRepository BookRepository)
        {
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




        [HttpGet("{AuthorId}")]
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

    }
}