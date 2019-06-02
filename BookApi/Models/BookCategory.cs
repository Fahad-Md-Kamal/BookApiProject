using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookApi.Models
{
    public class BookCategory
    {
        public int BookId { get; set; }
        public Book Book { get; set; }

        // Category table is joined using Category Id and to notify
        // about the category class and to called it from here the class is introduced.
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}