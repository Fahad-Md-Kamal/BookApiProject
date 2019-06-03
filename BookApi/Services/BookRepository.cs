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
            return _context.Books.Any(bk => bk.Isbn.Trim().ToUpper() == BookIsbn.Trim().ToUpper());
        }

        public Book GetBook(int BookId)
        {
            return _context.Books.Where(bk => bk.Id == BookId).FirstOrDefault();
        }

        public Book GetBook(string BookIsbn)
        {
            return _context.Books.Where(bk => bk.Isbn == BookIsbn).FirstOrDefault();
        }

        public decimal GetBookRating(int BookId)
        {
            var reviews = _context.Reviews.Where(rv => rv.Book.Id == BookId);

            if(reviews.Count() <= 0)
                return 0;
            
            return (decimal)reviews.Sum(r => r.Rating)/reviews.Count();
        }

        public ICollection<Book> GetBooks()
        {
            return _context.Books.OrderBy(b => b.Id).ToList();
        }

        public bool IsDuplicateISBN(int bookId, string bookIsbn)
        {
            var book = _context.Books.Where(bk => bk.Isbn.Trim().ToUpper() == bookIsbn.Trim().ToUpper() 
                                            && bk.Id != bookId)
                                            .FirstOrDefault();
            return book == null ? false : true;
        }
    }
}