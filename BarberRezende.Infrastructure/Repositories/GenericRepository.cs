using BarberRezende.Domain.Interfaces;
using BarberRezende.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BarberRezende.Infrastructure.Repositories
{
    /// <summary>
    /// Repositório genérico: concentra CRUD básico para qualquer entidade.
    /// As classes específicas (ClienteRepository, etc.) podem herdar dele.
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

        public IEnumerable<T> GetAll() => _dbSet.ToList();

        public T? GetById(int id) => _dbSet.Find(id);

        public void Create(T entity)
        {
            _dbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var entity = GetById(id);
            if (entity is null) return;

            _dbSet.Remove(entity);
            _context.SaveChanges();
        }
    }
}
