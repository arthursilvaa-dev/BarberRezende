using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;
using BarberRezende.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BarberRezende.Infrastructure.Repositories
{
    /// <summary>
    /// Implementação do repositório de Agendamentos usando EF Core.
    /// Aqui fica o "como faz" (consultas, Include, LINQ etc).
    /// </summary>
    public class AgendamentoRepository : GenericRepository<Agendamento>, IAgendamentoRepository
    {
        private readonly BarberRezendeDbContext _context;

        public AgendamentoRepository(BarberRezendeDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Agendamento>> GetByBarbeiroInRangeWithServicoAsync(int barbeiroId, DateTime start, DateTime end)
        {
            // AsNoTracking: melhor performance para leitura (não vamos editar esses objetos aqui).
            // Include(a => a.Servico): traz a entidade Servico junto, pra usar DuracaoMinutos.
            return await _context.Agendamentos
                .AsNoTracking()
                .Include(a => a.Servico)
                .Where(a => a.BarbeiroId == barbeiroId && a.DataHora >= start && a.DataHora < end)
                .ToListAsync();
        }
    }
}
