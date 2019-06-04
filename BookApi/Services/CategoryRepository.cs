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
            return _Context.Categories.OrderBy(c => c.Id).ToList();
        }

        public ICollection<Category> GetAllCategoriesForABook(int BookId)
        {
            return _Context.BookCategories.Where(b => b.BookId == BookId).Select(c => c.Category).ToList();
        }

        public Category GetCategory(int CategoryId)
        {
            return _Context.Categories.Where(c=> c.Id == CategoryId).FirstOrDefault();
        }

        public bool IsDuplicateCategory(int CategoryId, string CategoryName)
        {
            var Category = _Context.Categories.Where(c => c.Name.Trim().ToUpper() == CategoryName.Trim().ToUpper() 
                                                    && c.Id != CategoryId).FirstOrDefault();
            return Category == null ? false : true;
        }

        public bool CreateCategory(Category category)
        {
            _Context.Add(category); 
            return Save(); 
        }

        public bool UpdateCategory(Category category)
        {
            _Context.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _Context.Remove(category);
            return Save();
        }

        public bool Save()
        {
            var savedChanges = _Context.SaveChanges();
            return savedChanges >= 0 ? true : false; 
        }
    }
}