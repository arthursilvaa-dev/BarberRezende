using System.ComponentModel.DataAnnotations;

namespace BarberRezende.Application.DTOs.Agendamentos
{
    /// <summary>
    /// DTO usado para atualizar um Agendamento.
    /// </summary>
    public class AgendamentosUpdateDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "ClienteId deve ser maior que zero.")]
        public int ClienteId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "BarbeiroId deve ser maior que zero.")]
        public int BarbeiroId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ServicoId deve ser maior que zero.")]
        public int ServicoId { get; set; }

        [Required(ErrorMessage = "DataHora é obrigatória.")]
        public DateTime DataHora { get; set; }
    }
}
