using BarberRezende.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarberRezende.Infrastructure.Configurations;

public class ServicoConfiguration : IEntityTypeConfiguration<Servico>
{
    public void Configure(EntityTypeBuilder<Servico> builder)
    {
        builder.ToTable("Servicos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
               .HasMaxLength(120)
               .IsRequired();

        builder.Property(x => x.Preco)
               .HasColumnType("decimal(10,2)");
    }
}
