using System.Collections.Generic;
using System.Linq;
using BookApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Services
{
    public class CategoryRepository : ICategoryRepository
    {
        public BookDbContext _Context;

        public CategoryRepository(BookDbContext context)
        {
            _Context = context;

        }

        public bool CategoryExists(int CategoryId)
        {
            return _Context.Categories.Any(c => c.Id == CategoryId);
        }

        public ICollection<Book> GetAllBooksForACatagory(int CategoryId)
        {
            return _Context.BookCategories.Where(c => c.CategoryId == CategoryId).Select(b => b.Book).ToList();
        }

        public ICollection<Category> GetCategories()
        {
            return _Context.Categories.OrderBy(c => c.Name).ToList();
        }

        public ICollection<Category> GetAllCategoriesForABook(int BookId)
        {
            return _Context.BookCategories.Where(b => b.BookId == BookId).Select(c => c.Category).ToList();
        }

        public Category GetCategory(int CategoryId)
        {
            return _Context.Categories.Where(c=> c.Id == CategoryId).FirstOrDefault();
        }
 
    }
}