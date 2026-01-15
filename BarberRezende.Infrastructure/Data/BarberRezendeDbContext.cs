using BarberRezende.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BarberRezende.Infrastructure.Data
{
    public class BarberRezendeDbContext : DbContext
    {
        public BarberRezendeDbContext(DbContextOptions<BarberRezendeDbContext> options)
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Barbeiro> Barbeiros => Set<Barbeiro>();
        public DbSet<Funcionario> Funcionarios => Set<Funcionario>();
        public DbSet<Servico> Servicos => Set<Servico>();
        public DbSet<Agendamento> Agendamentos => Set<Agendamento>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Se você usa configurações (Fluent API)
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(BarberRezendeDbContext).Assembly);
        }
    }
}
