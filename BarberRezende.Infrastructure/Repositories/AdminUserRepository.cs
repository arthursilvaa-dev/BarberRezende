using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;
using BarberRezende.Infrastructure.Data;

namespace BarberRezende.Infrastructure.Repositories
{
    /// <summary>
    /// Implementação concreta do repositório de AdminUser.
    /// 
    /// Decisão arquitetural:
    /// - Implementa interface do Domain.
    /// - Depende do DbContext real.
    /// - Não contém regra de negócio.
    /// - Apenas acesso a dados.
    /// </summary>
    public class AdminUserRepository : IAdminUserRepository
    {
        private readonly BarberRezendeDbContext _context;

        /// <summary>
        /// Injeção de dependência do DbContext.
        /// </summary>
        public AdminUserRepository(BarberRezendeDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Busca administrador pelo email.
        /// Utilizado no login.
        /// </summary>
        public async Task<AdminUser?> GetByEmailAsync(string email)
        {
            return await _context.AdminUsers
                                 .FirstOrDefaultAsync(a => a.Email == email);
        }

        /// <summary>
        /// Busca administrador pelo Id.
        /// </summary>
        public async Task<AdminUser?> GetByIdAsync(Guid id)
        {
            return await _context.AdminUsers
                                 .FirstOrDefaultAsync(a => a.Id == id);
        }

        /// <summary>
        /// Adiciona novo administrador ao banco.
        /// </summary>
        public async Task AddAsync(AdminUser admin)
        {
            await _context.AdminUsers.AddAsync(admin);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AdminUser user)
        {
            _context.AdminUsers.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}