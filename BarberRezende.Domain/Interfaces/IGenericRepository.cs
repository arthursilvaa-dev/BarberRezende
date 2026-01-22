using System.Linq.Expressions;

namespace BarberRezende.Domain.Interfaces
{
    /// <summary>
    /// Contrato genérico de acesso a dados (CRUD).
    /// Fica no Domain porque o Domain define o "o que precisa existir",
    /// e a Infrastructure implementa "como faz".
    /// </summary>
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);

        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(T entity);

        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);

        Task<int> SaveChangesAsync();
    }
}
