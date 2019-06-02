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

    }
}