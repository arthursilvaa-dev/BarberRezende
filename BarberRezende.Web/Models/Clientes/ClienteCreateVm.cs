using System.ComponentModel.DataAnnotations;

namespace BarberRezende.Web.Models.Clientes
{
    public class ClienteCreateVm
    {
        [Required(ErrorMessage = "Nome é obrigatório.")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório.")]
        // ✅ Formatos aceitos (exemplos):
        // 11999999999
        // (11)99999-9999
        // (11) 9999-9999
        [RegularExpression(@"^\(?\d{2}\)?\s?\d{4,5}\-?\d{4}$", ErrorMessage = "Telefone inválido. Use DDD + número.")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string? Email { get; set; }
    }


}
