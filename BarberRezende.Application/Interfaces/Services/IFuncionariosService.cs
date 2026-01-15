using BarberRezende.Application.DTOs.Funcionarios;

namespace BarberRezende.Application.Interfaces.Services
{
    public interface IFuncionariosService
    {
        Task<IEnumerable<FuncionariosDTO>> GetAllAsync();
        Task<FuncionariosDTO?> GetByIdAsync(int id);
        Task<FuncionariosDTO> CreateAsync(FuncionariosCreateDTO dto);
        Task<bool> UpdateAsync(int id, FuncionariosUpdateDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
