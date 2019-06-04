using System.Collections.Generic;
using BookApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookApi.Services
{
    public interface ICategoryRepository
    {
         ICollection<Category> GetCategories();
         Category GetCategory(int CategoryId);
         ICollection<Category> GetAllCategoriesForABook(int BookId);
         ICollection<Book> GetAllBooksForACatagory(int categoryId);
         bool CategoryExists(int CategoryId);
         bool IsDuplicateCategory(int CategoryId, string CategoryName);

         bool CreateCategory(Category category);
         bool UpdateCategory(Category category);
         bool DeleteCategory(Category category);
         bool Save();

    }
}