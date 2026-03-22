using System;
using System.ComponentModel.DataAnnotations;

namespace BarberRezende.Web.Models.Agendamentos
{
    public class AgendamentoCreateVm // ViewModel para representar os dados do formulário de criação de agendamento
    {
        [Required(ErrorMessage = "Informe a data e hora do agendamento.")] // Validação de campo obrigatório
        public DateTime DataHora { get; set; } // Data e hora do agendamento

        [Required(ErrorMessage = "É necessário selecionar um cliente.")]
        public int? ClienteId { get; set; }

        [Required(ErrorMessage = "É necessário selecionar um barbeiro.")]
        public int? BarbeiroId { get; set; }

        [Required(ErrorMessage = "É necessário selecionar um serviço.")]
        public int? ServicoId { get; set; }
    }
}
