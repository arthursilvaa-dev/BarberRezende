using System.ComponentModel.DataAnnotations;

namespace BarberRezende.Web.Models.Configuracoes
{
    public class MudarSenhaVm
    {
        [Required(ErrorMessage = "A senha atual é obrigatória.")]
        public string SenhaAtual { get; set; }

        [Required(ErrorMessage = "A nova senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A nova senha precisa ter pelo menos 6 caracteres.")]
        public string NovaSenha { get; set; }

        [Required(ErrorMessage = "Confirme a nova senha.")]
        [Compare("NovaSenha", ErrorMessage = "As senhas não coincidem. Digite novamente.")]
        public string ConfirmarSenha { get; set; }
    }
}