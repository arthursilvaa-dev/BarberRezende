using BarberRezende.Domain.Entities;

namespace BarberRezende.Domain.Interfaces
{
    /// <summary>
    /// Repositório específico de Serviços.
    /// Herda o CRUD genérico e adiciona consultas específicas quando necessário.
    /// </summary>
    public interface IServicoRepository : IGenericRepository<Servico>
    {
        // Por enquanto, não precisamos de métodos extras.
        // O GetByIdAsync do genérico já resolve para buscar DuracaoMinutos.
    }
}
