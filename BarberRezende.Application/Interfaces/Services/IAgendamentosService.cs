using BarberRezende.Application.DTOs.Agendamentos;

namespace BarberRezende.Application.Interfaces.Services
{
    public interface IAgendamentosService
    {
        Task<IEnumerable<AgendamentosDTO>> GetAllAsync();
        Task<AgendamentosDTO?> GetByIdAsync(int id);
        Task<AgendamentosDTO> CreateAsync(AgendamentosCreateDTO dto);
        Task<bool> UpdateAsync(int id, AgendamentosUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
