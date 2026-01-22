using System.ComponentModel.DataAnnotations;

namespace BarberRezende.Application.DTOs.Agendamentos
{
    /// <summary>
    /// DTO usado ao criar um Agendamento.
    /// Note: a regra de NEGÓCIO (não permitir dois agendamentos no mesmo horário para o mesmo barbeiro)
    /// não fica aqui — ela fica no Domain/Service de domínio.
    /// Aqui fica apenas validação básica de entrada.
    /// </summary>
    public class AgendamentosCreateDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "ClienteId deve ser maior que zero.")]
        public int ClienteId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "BarbeiroId deve ser maior que zero.")]
        public int BarbeiroId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ServicoId deve ser maior que zero.")]
        public int ServicoId { get; set; }

        /// <summary>
        /// Data e hora do agendamento.
        /// O Swagger mostra como string ISO (é normal), mas vira DateTime no backend.
        /// </summary>
        [Required(ErrorMessage = "DataHora é obrigatória.")]
        public DateTime DataHora { get; set; }
    }
}
