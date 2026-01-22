using BarberRezende.Domain.Entities;

namespace BarberRezende.Domain.Interfaces
{
    /// <summary>
    /// Repositório específico de Agendamentos.
    /// Ele herda o CRUD genérico e adiciona consultas que só fazem sentido para Agendamento.
    /// </summary>
    public interface IAgendamentoRepository : IGenericRepository<Agendamento>
    {
        /// <summary>
        /// Busca agendamentos de um barbeiro dentro de um intervalo de tempo,
        /// já trazendo o Serviço junto (Include) para conseguirmos usar DuracaoMinutos.
        /// </summary>
        Task<List<Agendamento>> GetByBarbeiroInRangeWithServicoAsync(int barbeiroId, DateTime start, DateTime end);
    }
}
