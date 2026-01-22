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

            CreateMap<Agendamento, AgendamentosDTO>()
                .ForMember(dest => dest.ClienteNome,
                    opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nome : string.Empty))
                .ForMember(dest => dest.BarbeiroNome,
                    opt => opt.MapFrom(src => src.Barbeiro != null ? src.Barbeiro.Nome : string.Empty))
                .ForMember(dest => dest.ServicoNome,
                    opt => opt.MapFrom(src => src.Servico != null ? src.Servico.Nome : string.Empty));

            CreateMap<AgendamentosCreateDTO, Agendamento>();
        }
    }
}
