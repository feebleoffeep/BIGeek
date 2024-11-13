using System.ComponentModel.DataAnnotations;

namespace _2.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ім'я є обов'язковим")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Прізвище є обов'язковим")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email є обов'язковим")]
        [EmailAddress(ErrorMessage = "Невірний формат email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Пароль є обов'язковим")]
        [MinLength(8, ErrorMessage = "Пароль має містити не менше 8 символів")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Підтвердження пароля є обов'язковим")]
        [Compare("Password", ErrorMessage = "Паролі не збігаються")]
        public string? ConfirmPassword { get; set; }
    }
}





