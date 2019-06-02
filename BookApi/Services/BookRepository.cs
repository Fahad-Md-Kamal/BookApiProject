using System.Collections.Generic;
using System.Linq;
using BookApi.Models;

namespace BookApi.Services
{
    public class BookRepository : IBookRepository
    {
        private readonly BookDbContext _context;

        public BookRepository(BookDbContext context)
        {
            _context = context;
        }

        public bool BookExists(int bookId)
        {
            return _context.Books.Any(bk => bk.Id == bookId);
        }

        public bool BookExists(string BookIsbn)
        {
            throw new System.NotImplementedException();
        }

        public Book GetBook(int BookId)
        {
            throw new System.NotImplementedException();
        }

        public Book GetBook(string BookIsbn)
        {
            throw new System.NotImplementedException();
        }

        public decimal GetBookRating(int BookId)
        {
            throw new System.NotImplementedException();
        }

        public ICollection<Book> GetBooks()
        {
            throw new System.NotImplementedException();
        }

        public bool IsDuplicateISBN(int bookId, string bookIsbn)
        {
            throw new System.NotImplementedException();
        }
    }
}