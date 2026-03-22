using AutoMapper;

// ===== DTOs =====
using BarberRezende.Application.DTOs.Clientes;
using BarberRezende.Application.DTOs.Barbeiros;
using BarberRezende.Application.DTOs.Funcionarios;
using BarberRezende.Application.DTOs.Servicos;
using BarberRezende.Application.DTOs.Agendamentos;

// ===== Entities =====
using BarberRezende.Domain.Entities;

namespace BarberRezende.Application.Mapping
{
    /// <summary>
    /// Perfil central de mapeamento da aplicação.
    /// 
    /// Responsável por ensinar o AutoMapper
    /// a converter ENTITIES (Domain)
    /// para DTOs (Application) e vice-versa.
    /// 
    /// 🔑 Regra importante:
    /// - Se o nome da propriedade for IGUAL → AutoMapper resolve sozinho
    /// - Se o nome for DIFERENTE → precisamos mapear manualmente
    /// </summary>
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            // ==========================================================
            // CLIENTES
            // ==========================================================

            CreateMap<Cliente, ClientesDTO>();
            CreateMap<ClientesCreateDTO, Cliente>();
            CreateMap<ClientesUpdateDTO, Cliente>();


            // ==========================================================
            // BARBEIROS
            // ==========================================================

            CreateMap<Barbeiro, BarbeirosDTO>();
            CreateMap<BarbeirosCreateDTO, Barbeiro>();
            CreateMap<BarbeirosUpdateDTO, Barbeiro>();


            // ==========================================================
            // FUNCIONÁRIOS
            // ==========================================================

            CreateMap<Funcionario, FuncionariosDTO>();
            CreateMap<FuncionariosCreateDTO, Funcionario>();
            CreateMap<FuncionariosUpdateDTO, Funcionario>();


            // ==========================================================
            // SERVIÇOS  ⚠️ AQUI ESTAVA O PROBLEMA
            // ==========================================================

            /*
             * Entity: Servico
             * Propriedade: Nome
             * 
             * DTO: ServicosDTO / CreateDTO / UpdateDTO
             * Propriedade: NomeServico
             * 
             * Como os nomes são DIFERENTES,
             * o AutoMapper NÃO mapeia sozinho.
             */

            // Entity -> DTO (GET)
            CreateMap<Servico, ServicosDTO>()
                .ForMember(
                    dest => dest.NomeServico,
                    opt => opt.MapFrom(src => src.Nome)
                );

            // DTO -> Entity (POST)
            CreateMap<ServicosCreateDTO, Servico>()
                .ForMember(
                    dest => dest.Nome,
                    opt => opt.MapFrom(src => src.NomeServico)
                );

            // DTO -> Entity (PUT)
            CreateMap<ServicosUpdateDTO, Servico>()
                .ForMember(
                    dest => dest.Nome,
                    opt => opt.MapFrom(src => src.NomeServico)
                );


            // ==========================================================
            // AGENDAMENTOS
            // ==========================================================

            CreateMap<AgendamentosCreateDTO, Agendamento>()
                .ForMember(dest => dest.DataHora,
                    opt => opt.MapFrom(src => src.DataHora))
                .ForMember(dest => dest.ClienteId,
                    opt => opt.MapFrom(src => src.ClienteId))
                .ForMember(dest => dest.BarbeiroId,
                    opt => opt.MapFrom(src => src.BarbeiroId))
                .ForMember(dest => dest.ServicoId,
                    opt => opt.MapFrom(src => src.ServicoId))

                // 🔥 IGNORA SNAPSHOT (será preenchido no Service)
                .ForMember(dest => dest.ClienteNomeSnapshot, opt => opt.Ignore())
                .ForMember(dest => dest.BarbeiroNomeSnapshot, opt => opt.Ignore())
                .ForMember(dest => dest.ServicoNomeSnapshot, opt => opt.Ignore())
                .ForMember(dest => dest.PrecoSnapshot, opt => opt.Ignore())
                .ForMember(dest => dest.DuracaoMinutosSnapshot, opt => opt.Ignore())

                // navegações
                .ForMember(dest => dest.Cliente, opt => opt.Ignore())
                .ForMember(dest => dest.Barbeiro, opt => opt.Ignore())
                .ForMember(dest => dest.Servico, opt => opt.Ignore());
        }
    }
}
