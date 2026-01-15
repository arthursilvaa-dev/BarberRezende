using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;
using BarberRezende.Infrastructure.Data;

using System;

namespace BarberRezende.Infrastructure.Repositories
{
    public class ServicoRepository : GenericRepository<Servico>, IServicoRepository
    {
        public ServicoRepository(BarberRezendeDbContext context) : base(context) { }
    }
}
