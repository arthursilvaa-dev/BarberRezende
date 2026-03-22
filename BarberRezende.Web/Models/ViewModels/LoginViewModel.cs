using System.ComponentModel.DataAnnotations;

namespace BarberRezende.Web.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        // ❗ NÃO pode ser obrigatório
        public string? ErrorMessage { get; set; }
    }
}