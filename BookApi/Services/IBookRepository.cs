using System.Collections.Generic;
using BookApi.Models;

namespace BookApi.Services
{
    public interface IBookRepository
    {
        bool BookExists(int bookId);
        bool BookExists(string BookIsbn);
        ICollection<Book> GetBooks();
        Book GetBook(int BookId);
        Book GetBook(string BookIsbn);
        bool IsDuplicateISBN(int bookId, string bookIsbn);
        decimal GetBookRating(int BookId);

        bool CreateBook(List<int> authorsId, List<int> categoriesId, Book book);
        bool UpdateBook(List<int> authorsId, List<int> categoriesId, Book book);
        bool DeleteBook(Book book);
        bool Save();

    }
}