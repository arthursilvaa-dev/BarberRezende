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

            // ⚠️ Troque DataHora pelo nome REAL na sua entidade (ex: DataAgendamento, DataMarcada...)
            builder.Property(x => x.DataHora)
                .IsRequired();
        }
    }
}
