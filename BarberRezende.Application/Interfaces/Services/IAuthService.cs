using System.Threading.Tasks;
using BarberRezende.Application.DTOs.Auth;

namespace BarberRezende.Application.Interfaces.Services
{
    /// <summary>
    /// Contrato responsável pela autenticação do sistema.
    /// 
    /// Decisão arquitetural:
    /// - Interface fica na Application.
    /// - Implementação também na Application.
    /// - Dependerá do repositório do Domain.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Realiza login do administrador.
        /// Retorna token JWT caso sucesso.
        /// </summary>
        Task<LoginResponseDto> LoginAsync(LoginRequestDto request);
    }
}