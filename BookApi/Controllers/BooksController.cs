using System.Collections.Generic;
using BookApi.Dtos;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class BooksController : Controller
    {
        private readonly IBookRepository _BookRepository;
        public BooksController(IBookRepository BookRepository)
        {
            _BookRepository = BookRepository;
        }







        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetBooks()
        {
            var books = _BookRepository.GetBooks();

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var booksDto = new List<BookDto>();

            foreach (var book in books)
            {
                booksDto.Add(new BookDto
                {
                    Id = book.Id,
                    Title = book.Title,
                    Isbn = book.Isbn,
                    DatePublished = book.DatePublished
                });
            }
            return Ok(booksDto);
        }





        [HttpGet("{BookId}")]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetBook(int BookId)
        {
            if(!_BookRepository.BookExists(BookId))
                return NotFound();

            var Book = _BookRepository.GetBook(BookId);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookDto = new BookDto
            {
                    Id = Book.Id,
                    Title = Book.Title,
                    Isbn = Book.Isbn,
                    DatePublished = Book.DatePublished                
            };

            return Ok(bookDto);
        }





        [HttpGet("ISBN/{BookIsbn}")]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetBook(string BookIsbn)
        {
            if(!_BookRepository.BookExists(BookIsbn))
                return NotFound();

            var Book = _BookRepository.GetBook(BookIsbn);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            var bookDto = new BookDto
            {
                    Id = Book.Id,
                    Title = Book.Title,
                    Isbn = Book.Isbn,
                    DatePublished = Book.DatePublished                
            };

            return Ok(bookDto);
        }





        [HttpGet("{BookId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetBookRating(int BookId)
        {
            if(!_BookRepository.BookExists(BookId))
                return NotFound();

            var rating = _BookRepository.GetBookRating(BookId);

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rating);
        }







    }
}