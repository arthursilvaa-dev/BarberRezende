using BarberRezende.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarberRezende.Infrastructure.Configurations
{
    public class AgendamentoConfiguration : IEntityTypeConfiguration<Agendamento>
    {
        public void Configure(EntityTypeBuilder<Agendamento> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.DataHora)
                .IsRequired();

            // Mapeamento explícito das chaves estrangeiras:
            // "O Agendamento tem um Cliente (opcional), com chave estrangeira ClienteId"
            builder.HasOne(a => a.Cliente)
                .WithMany() // Se Cliente não tem uma lista de Agendamentos (List<Agendamento>), deixe vazio.
                .HasForeignKey(a => a.ClienteId)
                .OnDelete(DeleteBehavior.SetNull); // Importante: Se o cliente for deletado, o ID aqui fica nulo, mantendo o histórico

            builder.HasOne(a => a.Barbeiro)
                .WithMany()
                .HasForeignKey(a => a.BarbeiroId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(a => a.Servico)
                .WithMany()
                .HasForeignKey(a => a.ServicoId)
                .OnDelete(DeleteBehavior.SetNull);

            // Resolvendo o Aviso 2 (veja abaixo)
            builder.Property(x => x.PrecoSnapshot)
                   .HasColumnType("decimal(10,2)");
        }
    }
}