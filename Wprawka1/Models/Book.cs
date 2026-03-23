using System.ComponentModel.DataAnnotations;

namespace Wprawka1.Models
{
    public class Book
    {
        public int Id { get; set; } // Klucz główny

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        public int PublisherId { get; set; }
        public virtual Publisher Publisher { get; set; }

        public ICollection<Author> Authors { get; set; } = new List<Author>();
    }
}
