using BarberRezende.Domain.Interfaces;
using BarberRezende.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BarberRezende.Infrastructure.Config
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            // Genérico
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Específicos (SINGULAR!)
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IBarbeiroRepository, BarbeiroRepository>();
            services.AddScoped<IServicoRepository, ServicoRepository>();
            services.AddScoped<IFuncionarioRepository, FuncionarioRepository>();
            services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();

            return services;
        }
    }
}
