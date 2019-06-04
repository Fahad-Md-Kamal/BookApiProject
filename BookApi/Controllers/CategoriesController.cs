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



        [HttpGet("{CategoryId}", Name = "GetCategory")]
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

        

        
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(Category))]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateCategory([FromBody]Category categoryToBeCreated)
        {
            if(categoryToBeCreated == null)
                return BadRequest(ModelState);

            var category = _CategoryRepository.GetCategories().Where(c => c.Name.Trim().ToUpper() == categoryToBeCreated.Name.Trim().ToUpper()).FirstOrDefault();

            if(category != null)
            {
                ModelState.AddModelError("", $"Category {categoryToBeCreated.Name} already exists");
                return StatusCode(422, ModelState); // Duplicate object Response
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_CategoryRepository.CreateCategory(categoryToBeCreated))
            {
                ModelState.AddModelError("",$"Something went wrong while saving category {categoryToBeCreated.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetCategory", new {categoryId = categoryToBeCreated.Id}, categoryToBeCreated);
        }
 


        
        [HttpPut("{CategoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult UpdateCategory(int CategoryId, [FromBody]Category categoryToBeUpdated)
        {
            if(categoryToBeUpdated == null)
                return BadRequest(ModelState);
            
            if(categoryToBeUpdated.Id != CategoryId)
                return BadRequest(ModelState);
            
            if(!_CategoryRepository.CategoryExists(CategoryId))
                return NotFound();
            
            if(_CategoryRepository.IsDuplicateCategory(CategoryId, categoryToBeUpdated.Name))
            {
                ModelState.AddModelError("",$"Category {categoryToBeUpdated.Name} already exists");
                return StatusCode(422, ModelState); // Duplicate object response
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            if(!_CategoryRepository.UpdateCategory(categoryToBeUpdated))
            {
                ModelState.AddModelError("", $"Something went wrong while creating category {categoryToBeUpdated.Name}");
                return StatusCode(500,ModelState);
            }
            return NoContent();
        }




        [HttpDelete("{CategoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(409)]
        [ProducesResponseType(500)]
        public IActionResult DeleteCategory(int CategoryId)
        {
            if(!_CategoryRepository.CategoryExists(CategoryId))
                return BadRequest(ModelState);
            
            var categoryToBeDeleted = _CategoryRepository.GetCategory(CategoryId);

            if(_CategoryRepository.GetAllBooksForACatagory(CategoryId).Count() > 0)
            {
                ModelState.AddModelError("", $"Category {categoryToBeDeleted.Name} already used by other book(s)");
                return StatusCode(409, ModelState); // conflict object 
            }

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(!_CategoryRepository.DeleteCategory(categoryToBeDeleted))
            {
                ModelState.AddModelError("", $"There were some errors while deleting category '{categoryToBeDeleted.Name}'");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }



    }
}