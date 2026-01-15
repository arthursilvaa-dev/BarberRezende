using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;
using BarberRezende.Infrastructure.Data;

using System;

namespace BarberRezende.Infrastructure.Repositories
{
    public class ClienteRepository : GenericRepository<Cliente>, IClienteRepository
    {
        public ClienteRepository(BarberRezendeDbContext context) : base(context) { }
    }
}
