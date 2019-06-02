using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookApi.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        [Required]
        [MaxLength(50, ErrorMessage="Category name must be in 50 characters in length")]
        public string Name { get; set; }
        public virtual ICollection<BookCategory> BookCategories { get; set; }
    }
}