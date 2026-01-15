namespace BarberRezende.Application.DTOs.Agendamentos
{
    /// <summary>
    /// DTO completo de agendamentos.
    /// </summary>
    public class AgendamentosDTO
    {
        public int Id { get; set; }

        /// <summary>ID do cliente que fez o agendamento.</summary>
        public int ClienteId { get; set; }

        /// <summary>ID do barbeiro responsável.</summary>
        public int BarbeiroId { get; set; }

        /// <summary>ID do serviço solicitado.</summary>
        public int ServicoId { get; set; }

        /// <summary>Data e hora do agendamento.</summary>
        public DateTime DataHora { get; set; }
    }
}
