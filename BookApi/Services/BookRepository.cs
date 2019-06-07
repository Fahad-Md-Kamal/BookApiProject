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

        public bool CreateBook(List<int> authorsId, List<int> categoriesId, Book book)
        {
            var authors = _context.Authors.Where( a => authorsId.Contains(a.Id)).ToList();
            var categories = _context.Categories.Where( c => categoriesId.Contains(c.Id)).ToList();

            foreach(var author in authors)
            {
                var bookAuthor = new BookAuthor()
                {
                    Author = author,
                    Book = book
                };
                _context.Add(bookAuthor);
            }


            // Creating a category list first and then add it to the 
            var bookCategories = new List<BookCategory>();

            foreach(var category in categories)
            {
                bookCategories.Add(new BookCategory()
                {
                    Category = category,
                    Book = book
                });
            }
            
            _context.AddRange(bookCategories);
            _context.Add(book);
            return Save();
        }

        public bool DeleteBook(Book book)
        {
            _context.Remove(book);
            return Save();
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
            var book = _context.Books.Where(bk => bk.Isbn.Trim().ToUpper() == bookIsbn.Trim().ToUpper()&& bk.Id != bookId).FirstOrDefault();
            return book == null ? false : true;
        }

        public bool Save()
        {
            var SaveItem = _context.SaveChanges();
            return SaveItem >= 0 ? true : false;
        }

        public bool UpdateBook(List<int> authorsId, List<int> categoriesId, Book book)
        {
            
            var authors = _context.Authors.Where( a => authorsId.Contains(a.Id)).ToList();
            var categories = _context.Categories.Where( c => categoriesId.Contains(c.Id)).ToList();

            var bookAuthorsToDelete = _context.BookAuthors.Where(b=> b.BookId == book.Id);
            var bookCategoriesToDelete = _context.BookCategories.Where(c=> c.BookId == book.Id);

            _context.RemoveRange(bookAuthorsToDelete);
            _context.RemoveRange(bookCategoriesToDelete);

            foreach(var author in authors)
            {
                var bookAuthor = new BookAuthor()
                {
                    Author = author,
                    Book = book
                };
                _context.Add(bookAuthor);
            }


            // Creating a category list first and then add it to the 
            var bookCategories = new List<BookCategory>();

            foreach(var category in categories)
            {
                bookCategories.Add(new BookCategory()
                {
                    Category = category,
                    Book = book
                });
            }
            
            _context.AddRange(bookCategories);
            _context.Update(book);
            return Save();
        }
        
    }
}