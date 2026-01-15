using AutoMapper;
using BarberRezende.Application.DTOs.Agendamentos;
using BarberRezende.Application.DTOs.Barbeiros;
using BarberRezende.Application.DTOs.Clientes;
using BarberRezende.Application.DTOs.Funcionarios;
using BarberRezende.Application.DTOs.Servicos;
using BarberRezende.Domain.Entities;

namespace BarberRezende.Application.Mapping
{
    /// <summary>
    /// Profile do AutoMapper:
    /// Define como converter Entities (Domain) <-> DTOs (Application).
    /// Assim Controllers/Services não precisam ficar fazendo "new DTO()" manualmente.
    /// </summary>
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile()
        {
            // =========================
            // CLIENTES
            // =========================
            CreateMap<Cliente, ClientesDTO>();
            CreateMap<ClientesCreateDTO, Cliente>();
            CreateMap<ClientesUpdateDTO, Cliente>();

            // =========================
            // BARBEIROS
            // =========================
            CreateMap<Barbeiro, BarbeirosDTO>();
            CreateMap<BarbeirosCreateDTO, Barbeiro>();
            CreateMap<BarbeirosUpdateDTO, Barbeiro>();

            // =========================
            // SERVIÇOS
            // =========================
            // Exemplo: DTO chama "NomeServico", Entity chama "Nome"
            CreateMap<Servico, ServicosDTO>()
                .ForMember(dest => dest.NomeServico, opt => opt.MapFrom(src => src.Nome));

            CreateMap<ServicosCreateDTO, Servico>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NomeServico));

            CreateMap<ServicosUpdateDTO, Servico>()
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NomeServico));

            // =========================
            // FUNCIONÁRIOS
            // =========================
            CreateMap<Funcionario, FuncionariosDTO>();
            CreateMap<FuncionariosCreateDTO, Funcionario>();
            CreateMap<FuncionariosUpdateDTO, Funcionario>();

            // =========================
            // AGENDAMENTOS
            // =========================
            CreateMap<Agendamento, AgendamentosDTO>();
            CreateMap<AgendamentosCreateDTO, Agendamento>();
            CreateMap<AgendamentosUpdateDTO, Agendamento>();
        }
    }
}
