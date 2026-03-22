namespace BarberRezende.Domain.Entities;


public class Agendamento
{
    public int Id { get; set; }

    public DateTime DataHora { get; set; }

    // FKs ficam anuláveis para NÃO sumir histórico
    public int? ClienteId { get; set; }
    public int? BarbeiroId { get; set; }
    public int? ServicoId { get; set; }

    // Snapshots (salvos no momento do agendamento)
    public string? ClienteNomeSnapshot { get; set; }
    public string? BarbeiroNomeSnapshot { get; set; }
    public string? ServicoNomeSnapshot { get; set; }

    public decimal PrecoSnapshot { get; set; }
    public int DuracaoMinutosSnapshot { get; set; }

    // Navegações (se você usa)
    public Cliente? Cliente { get; set; }
    public Barbeiro? Barbeiro { get; set; }
    public Servico? Servico { get; set; }

    // Status (já adianta para Cancelar/Reagendar)
    public string Status { get; set; } = "Ativo"; // Ativo | Cancelado | Concluido
}

