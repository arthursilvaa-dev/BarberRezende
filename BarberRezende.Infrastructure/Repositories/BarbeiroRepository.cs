using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;
using BarberRezende.Infrastructure.Data;

using System;

namespace BarberRezende.Infrastructure.Repositories
{
    public class BarbeiroRepository : GenericRepository<Barbeiro>, IBarbeiroRepository
    {
        public BarbeiroRepository(BarberRezendeDbContext context) : base(context) { }
    }
}
