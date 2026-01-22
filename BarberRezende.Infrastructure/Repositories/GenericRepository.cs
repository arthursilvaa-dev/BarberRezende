using System.Linq.Expressions;
using BarberRezende.Domain.Interfaces;
using BarberRezende.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BarberRezende.Infrastructure.Repositories
{
    /// <summary>
    /// Implementação do repositório genérico usando EF Core.
    /// </summary>
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly BarberRezendeDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(BarberRezendeDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.AsNoTracking().ToListAsync();

        public async Task<T?> GetByIdAsync(int id)
            => await _dbSet.FindAsync(id);

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public void Update(T entity)
            => _dbSet.Update(entity);

        public void Delete(T entity)
            => _dbSet.Remove(entity);

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.AnyAsync(predicate);

        public async Task<int> SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
