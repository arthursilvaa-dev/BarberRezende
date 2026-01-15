namespace BarberRezende.Application.DTOs.Agendamentos
{
    /// <summary>
    /// DTO usado ao criar um agendamento.
    /// </summary>
    public class AgendamentosCreateDTO
    {
        public int ClienteId { get; set; }
        public int BarbeiroId { get; set; }
        public int ServicoId { get; set; }
        public DateTime DataHora { get; set; }
    }
}
