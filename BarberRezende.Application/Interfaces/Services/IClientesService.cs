using BarberRezende.Application.DTOs.Clientes;

namespace BarberRezende.Application.Interfaces.Services
{
    public interface IClientesService
    {
        Task<IEnumerable<ClientesDTO>> GetAllAsync();
        Task<ClientesDTO?> GetByIdAsync(int id);
        Task<ClientesDTO> CreateAsync(ClientesCreateDTO dto);
        Task<bool> UpdateAsync(int id, ClientesUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
