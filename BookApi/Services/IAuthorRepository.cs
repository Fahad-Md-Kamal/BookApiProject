using System.Collections.Generic;
using BookApi.Models;

namespace BookApi.Services
{
    public interface IAuthorRepository
    {
         bool AuthorExists(int AuthorId);
         ICollection<Author> GetAuthors();
         Author GetAuthor(int AuthorId);
         ICollection<Author> GetAuthorsOfABook(int BookId);
         ICollection<Book> GetBooksByAuthor(int AuthorId); 
    }
}