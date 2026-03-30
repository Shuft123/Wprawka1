using System.ComponentModel.DataAnnotations;

namespace Wprawka1.Models
{
    public class Publisher
    {
        public int Id { get; set; } // Klucz główny

        [Required] // Odpowiednik NOT NULL w bazie
        [MaxLength(100)] // Ograniczenie długości
        public string? Name { get; set; }
        public virtual ICollection<Book>? Books { get; set; }
    }
}
