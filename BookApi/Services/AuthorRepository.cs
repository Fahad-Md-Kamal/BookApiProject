using System.Collections.Generic;
using System.Linq;
using BookApi.Models;

namespace BookApi.Services
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly BookDbContext _context;
        public AuthorRepository(BookDbContext context)
        {
            _context = context;
        }

        public bool AuthorExists(int AuthorId)
        {
            return _context.Authors.Any(a => a.Id == AuthorId);
        }

        public Author GetAuthor(int AuthorId)
        {
            return _context.Authors.Where(a => a.Id == AuthorId).FirstOrDefault();
        }

        public ICollection<Author> GetAuthors()
        {
            return _context.Authors.OrderBy(a => a.Id).ToList();
        }

        public ICollection<Author> GetAuthorsOfABook(int BookId)
        {
            return _context.BookAuthors.Where(b => b.Book.Id == BookId).Select(a => a.Author).ToList();
        }

        public ICollection<Book> GetBooksByAuthor(int AuthorId)
        {
            return _context.BookAuthors.Where(a => a.Author.Id == AuthorId).Select(b => b.Book).ToList();
        }
    }
}