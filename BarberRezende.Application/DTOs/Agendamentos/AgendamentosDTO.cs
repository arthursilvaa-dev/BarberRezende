namespace BarberRezende.Application.DTOs.Agendamentos
{
    public class AgendamentosDTO
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }

        public int? ClienteId { get; set; }
        public int? BarbeiroId { get; set; }
        public int? ServicoId { get; set; }

        // 🔥 SNAPSHOTS (fonte principal)
        public string? ClienteNomeSnapshot { get; set; }
        public string? BarbeiroNomeSnapshot { get; set; }
        public string? ServicoNomeSnapshot { get; set; }
        public decimal PrecoSnapshot { get; set; }

        // (opcional) fallback atual
        public string? ClienteNome { get; set; }
        public string? BarbeiroNome { get; set; }
        public string? ServicoNome { get; set; }
    }
}