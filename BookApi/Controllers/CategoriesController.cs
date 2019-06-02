using System.Collections.Generic;
using System.Linq;
using BookApi.Dtos;
using BookApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly ICategoryRepository _CategoryRepository;
        private readonly IBookRepository _BookRepository;
        public CategoriesController(ICategoryRepository CategoryRepository, IBookRepository BookRepository)
        {
            _BookRepository = BookRepository;
            _CategoryRepository = CategoryRepository;
        }



        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        public IActionResult GetCategories()
        {
            var Categories = _CategoryRepository.GetCategories();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var CategoriesDto = new List<CategoryDto>();
            foreach (var Category in Categories)
            {
                CategoriesDto.Add(new CategoryDto
                {
                    Id = Category.Id,
                    Name = Category.Name
                });
            }

            return Ok(CategoriesDto);
        }



        [HttpGet("{CategoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        public IActionResult GetCategory(int CategoryId)
        {

            if (!_CategoryRepository.CategoryExists(CategoryId))
                return NotFound(CategoryId);

            var Category = _CategoryRepository.GetCategory(CategoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var CategoryDto = new CategoryDto()
            {
                Id = Category.Id,
                Name = Category.Name
            };
            return Ok(CategoryDto);
        }



// TODO - Text this After Book repository is solved
        [HttpGet("books/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        public IActionResult GetAllCategoriesForABook(int bookId)
        {

            if(!_BookRepository.BookExists(bookId))
                return NotFound();

            var Categories = _CategoryRepository.GetAllCategoriesForABook(bookId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var CategoryDto = new List<CategoryDto>();
            foreach (var category in Categories)
            {
                CategoryDto.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                });

            }
            return Ok(CategoryDto);

        }



        [HttpGet("{CategoryId}/books")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<BookDto>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetAllBooksForACategory(int CategoryId)
        {
            if (!_CategoryRepository.CategoryExists(CategoryId))
                return NotFound();

            var books = _CategoryRepository.GetAllBooksForACatagory(CategoryId).ToList();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var BooksDto = new List<BookDto>();

            foreach (var Book in books)
            {
                BooksDto.Add(new BookDto()
                {
                    Id = Book.Id,
                    Isbn = Book.Isbn,
                    Title = Book.Title,
                    DatePublished = Book.DatePublished
                });
            }

            return Ok(BooksDto);

        }

    }
}