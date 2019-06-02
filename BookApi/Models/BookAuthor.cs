using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookApi.Models
{
    public class BookAuthor
    {
        // Book Table Linking using Book Id
        public int BookId { get; set; }
        public Book Book { get; set; }

        // Authors are joined with author Id
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }
}