using System;
using System.Threading.Tasks;
using BarberRezende.Domain.Entities;

namespace BarberRezende.Domain.Interfaces
{
    /// <summary>
    /// Contrato para acesso aos dados do AdminUser.
    /// 
    /// Decisão arquitetural:
    /// - Interface fica no Domain.
    /// - Implementação ficará na Infrastructure.
    /// - Mantemos inversão de dependência.
    /// </summary>
    public interface IAdminUserRepository
    {
        /// <summary>
        /// Busca um administrador pelo email.
        /// Utilizado no processo de login.
        /// </summary>
        Task<AdminUser?> GetByEmailAsync(string email);

        /// <summary>
        /// Busca administrador pelo Id.
        /// Pode ser usado futuramente para validações.
        /// </summary>
        Task<AdminUser?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adiciona um novo administrador.
        /// </summary>
        Task AddAsync(AdminUser admin);
    }
}