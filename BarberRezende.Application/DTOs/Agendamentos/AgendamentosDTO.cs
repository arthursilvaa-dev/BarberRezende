namespace BarberRezende.Application.DTOs.Agendamentos
{
    // DTO de retorno (READ) para listar/consultar agendamentos
    public class AgendamentosDTO
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }

        public int ClienteId { get; set; }
        public string ClienteNome { get; set; } = string.Empty;

        public int BarbeiroId { get; set; }
        public string BarbeiroNome { get; set; } = string.Empty;

        public int ServicoId { get; set; }
        public string ServicoNome { get; set; } = string.Empty;
        public decimal ServicoPreco { get; set; }
    }
}
