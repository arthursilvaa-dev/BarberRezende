using System.ComponentModel.DataAnnotations;
using BarberRezende.Web.Models.Common;

namespace BarberRezende.Web.Models.Agendamentos
{
    // O formulário de envio
    public class AgendamentoUpdateVm
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A data/hora é obrigatória.")]
        public DateTime DataHora { get; set; }

        [Required(ErrorMessage = "O cliente é obrigatório.")]
        public int? ClienteId { get; set; }

        [Required(ErrorMessage = "O barbeiro é obrigatório.")]
        public int? BarbeiroId { get; set; }

        [Required(ErrorMessage = "O serviço é obrigatório.")]
        public int? ServicoId { get; set; }
    }

    // A página que contém o formulário + as listas do dropdown
    public class AgendamentoUpdatePageVm
    {
        public AgendamentoUpdateVm Form { get; set; } = new();
        public string? ErrorMessage { get; set; }
        public List<SimpleOptionVm> Clientes { get; set; } = new();
        public List<SimpleOptionVm> Barbeiros { get; set; } = new();
        public List<SimpleOptionVm> Servicos { get; set; } = new();
    }
}