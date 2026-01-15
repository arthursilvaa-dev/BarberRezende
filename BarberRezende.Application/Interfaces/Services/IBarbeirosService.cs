using BarberRezende.Application.DTOs.Barbeiros;

namespace BarberRezende.Application.Interfaces.Services
{
    public interface IBarbeirosService
    {
        Task<IEnumerable<BarbeirosDTO>> GetAllAsync();
        Task<BarbeirosDTO?> GetByIdAsync(int id);
        Task<BarbeirosDTO> CreateAsync(BarbeirosCreateDTO dto);
        Task<bool> UpdateAsync(int id, BarbeirosUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
