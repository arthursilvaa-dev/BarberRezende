using BarberRezende.Domain.Entities;

namespace BarberRezende.Domain.Interfaces
{
    /// <summary>
    /// Repositório específico de Agendamentos.
    /// Herda o CRUD genérico (Add/Update/Delete/GetById/GetAll etc.)
    /// e adiciona consultas específicas de agenda.
    /// </summary>
    public interface IAgendamentoRepository : IGenericRepository<Agendamento>
    {
        /// <summary>
        /// Busca agendamentos de um barbeiro dentro de um intervalo de tempo,
        /// já trazendo o Serviço junto para obter DuracaoMinutos.
        /// </summary>
        Task<List<Agendamento>> GetByBarbeiroInRangeWithServicoAsync(int barbeiroId, DateTime start, DateTime end);

        /// <summary>
        /// REGRA FORTE (técnica pra entrevista):
        /// Retorna true se existir QUALQUER agendamento do barbeiro que SOBREPOE
        /// o intervalo [start, end).
        ///
        /// Overlap padrão:
        /// existenteStart < novoEnd  &&  existenteEnd > novoStart
        ///
        /// ignoreAgendamentoId:
        /// usado no Update para ignorar o próprio registro.
        /// </summary>
        Task<bool> ExistsOverlappingAsync(int barbeiroId, DateTime start, DateTime end, int? ignoreAgendamentoId = null);
    }
}
