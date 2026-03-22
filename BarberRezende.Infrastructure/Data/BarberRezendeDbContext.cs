using BarberRezende.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BarberRezende.Infrastructure.Data
{
    public class BarberRezendeDbContext : DbContext // classe que representa o banco de dados, herda de DbContext do Entity Framework
    {
        public BarberRezendeDbContext(DbContextOptions<BarberRezendeDbContext> options) // construtor que recebe opções de configuração do DbContext, como string de conexão, e passa para a classe base
            : base(options)
        {
        }

        public DbSet<Cliente> Clientes => Set<Cliente>(); // DbSet representa uma tabela no banco de dados, aqui definimos as tabelas para cada entidade do domínio
        public DbSet<Barbeiro> Barbeiros => Set<Barbeiro>(); // DbSet para Barbeiro, necessário para o processo de agendamento e histórico
        public DbSet<Funcionario> Funcionarios => Set<Funcionario>(); // DbSet para Funcionario, necessário para o processo de login e gerenciamento de funcionários
        public DbSet<Servico> Servicos => Set<Servico>(); // DbSet para Servico, necessário para o processo de agendamento e histórico
        public DbSet<Agendamento> Agendamentos => Set<Agendamento>(); // DbSet para Agendamento, necessário para o processo de agendamento e histórico
        public DbSet<AdminUser> AdminUsers { get; set; } // DbSet para AdminUser, necessário para o processo de login e gerenciamento de administradores

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Agendamento>(entity =>
            {
                entity.HasOne(a => a.Cliente)
                    .WithMany() // ou .WithMany(c => c.Agendamentos) se existir
                    .HasForeignKey(a => a.ClienteId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(a => a.Barbeiro)
                    .WithMany()
                    .HasForeignKey(a => a.BarbeiroId)
                    .OnDelete(DeleteBehavior.SetNull);

                entity.HasOne(a => a.Servico)
                    .WithMany()
                    .HasForeignKey(a => a.ServicoId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            /// <summary>
            /// Configuração da entidade AdminUser.
            /// Mantemos configuração explícita para clareza e controle.
            /// </summary>
            modelBuilder.Entity<AdminUser>(entity =>
            {
                // Define chave primária
                entity.HasKey(a => a.Id);

                // Email é obrigatório e limitado
                entity.Property(a => a.Email)
                      .IsRequired()
                      .HasMaxLength(150);

                // Garante que não existam dois admins com mesmo email
                entity.HasIndex(a => a.Email)
                      .IsUnique();

                // PasswordHash é obrigatório
                entity.Property(a => a.PasswordHash)
                      .IsRequired();

                // CreatedAt obrigatório
                entity.Property(a => a.CreatedAt)
                      .IsRequired();
            });
        }

    }
}
