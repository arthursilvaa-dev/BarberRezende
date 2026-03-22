using BarberRezende.Application.DTOs.Agendamentos;

namespace BarberRezende.Application.Interfaces.Services
{
    /// <summary>
    /// Contrato do Service de Agendamentos.
    /// Controller só conhece isso (abstração).
    /// </summary>
    public interface IAgendamentosService
    {
        Task<IEnumerable<AgendamentosDTO>> GetAllAsync();
        Task<AgendamentosDTO?> GetByIdAsync(int id);

        /// <summary>
        /// Retorna lista filtrada por parâmetros opcionais.
        /// Ex: barbeiroId e data.
        /// </summary>
        Task<IEnumerable<AgendamentosDTO>> GetByFilterAsync(
            int? barbeiroId,
            int? clienteId,
            int? servicoId,
            DateOnly? data
        );

        Task<AgendamentosDTO> CreateAsync(AgendamentosCreateDTO dto);

        /// <summary>
        /// Atualiza e retorna true se atualizou; false se não achou o registro.
        /// </summary>
        Task<bool> UpdateAsync(int id, AgendamentosUpdateDTO dto);

        /// <summary>
        /// Remove e retorna true se removeu; false se não achou.
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
}
