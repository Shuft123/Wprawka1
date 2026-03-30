using System.ComponentModel.DataAnnotations;

namespace Wprawka1.Models
{
    public class Book
    {
        public int Id { get; set; } // Klucz główny
        [Display(Name = "Tytuł Książki")]
        [Required(ErrorMessage="Pole Tytuł Książki jest wymagane")]
        [MaxLength(50, ErrorMessage ="Tytuł książki nie może być dłuższy niż 50 znaków")]
        public string? Title { get; set; }
        [Display(Name ="Wydawnictwo")]
        public int PublisherId { get; set; } // Klucz obcy
        [Display(Name ="Wydawnictwo")]
        public virtual Publisher? Publisher { get; set; }
        [Display(Name ="Autorzy")]
        public ICollection<Author>? Authors { get; set; } = new List<Author>();
    }
}
