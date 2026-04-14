using System.ComponentModel.DataAnnotations;

namespace Wprawka1.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Login jest wymagany")]
        [Display(Name = "Login")]
        public string Login { get; set; }

        //[Required(ErrorMessage = "Hasło jest wymagane")]
        [Display(Name = "Hasło")]
        public string? PasswordHash { get; set; }

        [Required(ErrorMessage = "Imię jest wymagane")]
        [Display(Name = "Imię")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [Display(Name = "Nazwisko")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu email")]
        [Display(Name = "Adres Email")]
        public string Email { get; set; }
    }
}