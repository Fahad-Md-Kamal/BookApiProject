using System.Collections.Generic;
using System.Linq;
using BookApi.Dtos;
using BookApi.Models;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class BooksController : Controller
    {
        private readonly IBookRepository _BookRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAuthorRepository _authorRepository;

        public IReviewRepository _reviewRepository;

        public BooksController(IBookRepository BookRepository, 
                                IAuthorRepository authorRepository, 
                                ICategoryRepository categoryRepository, 
                                IReviewRepository reviewRepository)
        {
            _authorRepository = authorRepository;
            _categoryRepository = categoryRepository;
            _reviewRepository = reviewRepository;
            _BookRepository = BookRepository;
        }







        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        [ProducesResponseType(400)]
        public IActionResult GetBooks()
        {
            var books = _BookRepository.GetBooks();

            if (!ModelState.IsValid)
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





        [HttpGet("{BookId}", Name = "GetBook")]
        [ProducesResponseType(200, Type = typeof(BookDto))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetBook(int BookId)
        {
            if (!_BookRepository.BookExists(BookId))
                return NotFound();

            var Book = _BookRepository.GetBook(BookId);

            if (!ModelState.IsValid)
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
            if (!_BookRepository.BookExists(BookIsbn))
                return NotFound();

            var Book = _BookRepository.GetBook(BookIsbn);

            if (!ModelState.IsValid)
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
            if (!_BookRepository.BookExists(BookId))
                return NotFound();

            var rating = _BookRepository.GetBookRating(BookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(rating);
        }



        [HttpPost]
        [ProducesResponseType(201,Type=typeof(Book))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateBook([FromQuery]List<int> authId,
                                        [FromQuery]List<int> catId, 
                                        [FromBody]Book bookToCreate)
        {
            var statusCode = ValidateBook(authId, catId, bookToCreate);

            if(!ModelState.IsValid)
                return StatusCode(statusCode.StatusCode);
            
            if(!_BookRepository.CreateBook(authId,catId, bookToCreate))
            {
                ModelState.AddModelError("", $"Something went wrong saving the book {bookToCreate.Title}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetBook", new {bookId = bookToCreate.Id}, bookToCreate);
        }



        [HttpPut("{bookId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateBook(int bookId, 
                                        [FromQuery]List<int> authId,
                                        [FromQuery]List<int> catId, 
                                        [FromBody]Book bookToUpdate)
        {
           
            var statusCode = ValidateBook(authId, catId, bookToUpdate);
            
            if(bookId != bookToUpdate.Id)
            return BadRequest();

            if(!_BookRepository.BookExists(bookId))
                return NotFound();

            if(!ModelState.IsValid)
                return StatusCode(statusCode.StatusCode);
            
            if(!_BookRepository.UpdateBook(authId,catId, bookToUpdate))
            {
                ModelState.AddModelError("", $"Something went wrong Updating the book {bookToUpdate.Title}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }



        [HttpDelete("{bookId}")]
        [ProducesResponseType(204)] // no content
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public IActionResult DeleteReviewer(int bookId)
        {
            if(!_BookRepository.BookExists(bookId))
                return NotFound();
            
            var reviewsToDelete = _reviewRepository.GetReviewsOfABook(bookId);
            var bookToDelete = _BookRepository.GetBook(bookId);

            if(!_reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
            {
                ModelState.AddModelError("",$"Something went wrong while deleting reviews of the book {bookToDelete.Title}");
                return StatusCode(500, ModelState);
            }

            if(!_BookRepository.DeleteBook(bookToDelete))
            {
                ModelState.AddModelError("",$"Something went wrong while deleting the book {bookToDelete.Title}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }







        private StatusCodeResult ValidateBook(List<int> authId, List<int> catId, Book book)
        {
            if(book == null || authId.Count() <= 0 || catId.Count() <= 0)
            {
                ModelState.AddModelError("", "Missing Book, author, or category");
                return BadRequest();
            }

            if(_BookRepository.IsDuplicateISBN(book.Id, book.Isbn))
            {
                ModelState.AddModelError("", "Duplicate ISBN");
                return StatusCode(422);
            }

            foreach(var id in authId)
            {
                if(!_authorRepository.AuthorExists(id))
                {
                    ModelState.AddModelError("", $"Author not found");
                    return StatusCode(404);
                }
            }

            foreach(var id in catId)
            {
                if(!_categoryRepository.CategoryExists(id))
                {
                    ModelState.AddModelError("", $"Category not found");
                    return StatusCode(404);
                }
            }

            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("",$"Crititcal Error");
                return BadRequest();
            }

            return NoContent();
        }



    }
}