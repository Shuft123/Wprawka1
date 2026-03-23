using System.ComponentModel.DataAnnotations;

namespace Wprawka1.Models
{
    public class Author
    {
        public int Id { get; set; } // Klucz główny

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        public ICollection<Book> Books { get; set; } = new List<Book>();
    }
}
