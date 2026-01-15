using BarberRezende.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BarberRezende.Infrastructure.Configurations;

public class BarbeiroConfiguration : IEntityTypeConfiguration<Barbeiro>
{
    public void Configure(EntityTypeBuilder<Barbeiro> builder)
    {
        builder.ToTable("Barbeiros");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Nome)
               .HasMaxLength(120)
               .IsRequired();
    }
}
