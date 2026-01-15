using BarberRezende.Application.DTOs.Servicos;

namespace BarberRezende.Application.Interfaces.Services
{
    // Interface do serviço de Serviços (camada Application)
    public interface IServicosService
    {
        Task<IEnumerable<ServicosDTO>> GetAllAsync();
        Task<ServicosDTO?> GetByIdAsync(int id);
        Task<ServicosDTO> CreateAsync(ServicosCreateDTO dto);
        Task<bool> UpdateAsync(int id, ServicosUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
