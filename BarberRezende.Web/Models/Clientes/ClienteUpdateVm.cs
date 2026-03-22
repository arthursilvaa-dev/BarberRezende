using System.ComponentModel.DataAnnotations;

namespace BarberRezende.Web.Models.Clientes
{
    public class ClienteUpdateVm
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o nome.")]
        [StringLength(100, ErrorMessage = "Nome muito longo.")]
        public string Nome { get; set; } = "";

        [Required(ErrorMessage = "Informe o telefone.")]
        [StringLength(20)]
        public string Telefone { get; set; } = "";

        [Required(ErrorMessage = "Informe o e-mail.")]
        [EmailAddress(ErrorMessage = "E-mail inválido.")]
        public string Email { get; set; } = "";
    }
}
