using BarberRezende.Application.DTOs.Servicos;

namespace BarberRezende.Application.Interfaces.Services
{
    /// <summary>
    /// Contrato do service de Serviços (camada Application).
    /// Controller conhece só essa interface.
    /// </summary>
    public interface IServicosService
    {
        Task<IEnumerable<ServicosDTO>> GetAllAsync();
        Task<ServicosDTO?> GetByIdAsync(int id);
        Task<ServicosDTO> CreateAsync(ServicosCreateDTO dto);
        Task<bool> UpdateAsync(int id, ServicosUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
