namespace BarberRezende.Application.DTOs.Agendamentos
{
    /// <summary>
    /// DTO usado ao atualizar um agendamento já existente.
    /// </summary>
    public class AgendamentosUpdateDTO
    {
        public int ClienteId { get; set; }
        public int BarbeiroId { get; set; }
        public int ServicoId { get; set; }
        public DateTime DataHora { get; set; }
    }
}
