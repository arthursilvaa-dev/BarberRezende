using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;
using BarberRezende.Infrastructure.Data;

using System;

namespace BarberRezende.Infrastructure.Repositories
{
    public class AgendamentoRepository : GenericRepository<Agendamento>, IAgendamentoRepository
    {
        public AgendamentoRepository(BarberRezendeDbContext context) : base(context) { }
    }
}
