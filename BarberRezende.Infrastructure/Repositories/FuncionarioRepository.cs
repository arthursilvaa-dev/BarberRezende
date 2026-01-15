using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;
using BarberRezende.Infrastructure.Data;

using System;

namespace BarberRezende.Infrastructure.Repositories
{
    public class FuncionarioRepository : GenericRepository<Funcionario>, IFuncionarioRepository
    {
        public FuncionarioRepository(BarberRezendeDbContext context) : base(context) { }
    }
}
