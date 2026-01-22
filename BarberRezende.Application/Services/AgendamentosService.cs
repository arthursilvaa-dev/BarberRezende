using AutoMapper;
using BarberRezende.Application.DTOs.Agendamentos;
using BarberRezende.Application.Interfaces.Services;
using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;

namespace BarberRezende.Application.Services
{
    /// <summary>
    /// Camada Application (Service):
    /// - Orquestra casos de uso (CRUD + filtros)
    /// - Aplica regras de aplicação (ex: validações de IDs existentes, etc.)
    /// - NÃO faz acesso direto ao banco (isso é do repository/infra)
    /// - NÃO deve expor entidades do Domain direto (usa DTOs)
    /// </summary>
    public class AgendamentosService : IAgendamentosService
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IMapper _mapper;

        public AgendamentosService(IAgendamentoRepository agendamentoRepository, IMapper mapper)
        {
            _agendamentoRepository = agendamentoRepository;
            _mapper = mapper;
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<IEnumerable<AgendamentosDTO>> GetAllAsync()
        {
            var agendamentos = await _agendamentoRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AgendamentosDTO>>(agendamentos);
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<AgendamentosDTO?> GetByIdAsync(int id)
        {
            var agendamento = await _agendamentoRepository.GetByIdAsync(id);

            if (agendamento is null)
                return null;

            return _mapper.Map<AgendamentosDTO>(agendamento);
        }

        // =========================
        // GET BY FILTER (NOVO - corrige o erro do build)
        // =========================
        /// <summary>
        /// Filtra agendamentos por ClienteId, BarbeiroId, ServicoId e Data (somente o dia).
        /// 
        /// Importante:
        /// - Os parâmetros são opcionais (nullable).
        /// - Se vier tudo null, retorna tudo (ou você pode decidir retornar vazio, mas aqui retorna tudo).
        /// - DateOnly filtra pelo DIA (independente do horário).
        /// </summary>
        public async Task<IEnumerable<AgendamentosDTO>> GetByFilterAsync(
            int? clienteId,
            int? barbeiroId,
            int? servicoId,
            DateOnly? data)
        {
            // 1) Busca todos (simples e funciona sempre).
            //    Depois, filtramos em memória.
            //    (Mais pra frente, a gente pode otimizar criando um método específico no repository para filtrar no banco.)
            var agendamentos = await _agendamentoRepository.GetAllAsync();

            // 2) Filtra por ClienteId
            if (clienteId.HasValue)
                agendamentos = agendamentos.Where(a => a.ClienteId == clienteId.Value);

            // 3) Filtra por BarbeiroId
            if (barbeiroId.HasValue)
                agendamentos = agendamentos.Where(a => a.BarbeiroId == barbeiroId.Value);

            // 4) Filtra por ServicoId
            if (servicoId.HasValue)
                agendamentos = agendamentos.Where(a => a.ServicoId == servicoId.Value);

            // 5) Filtra pela DATA (ignorando horário)
            if (data.HasValue) {
                var dia = data.Value;
                agendamentos = agendamentos.Where(a =>
                    DateOnly.FromDateTime(a.DataHora) == dia
                );
            }

            // 6) Retorna como DTO
            return _mapper.Map<IEnumerable<AgendamentosDTO>>(agendamentos);
        }

        // =========================
        // CREATE
        // =========================
        public async Task<AgendamentosDTO> CreateAsync(AgendamentosCreateDTO dto)
        {
            // Mapeia DTO -> Entidade
            var entity = _mapper.Map<Agendamento>(dto);

            // Regra de negócio: não permitir dois agendamentos no mesmo horário pro mesmo barbeiro
            // (Você já tinha feito essa regra, aqui deixo de forma clara)
            var conflito = await _agendamentoRepository.ExistsAsync(a =>
                a.BarbeiroId == entity.BarbeiroId &&
                a.DataHora == entity.DataHora
            );

            if (conflito)
                throw new InvalidOperationException("Já existe um agendamento para este barbeiro neste mesmo horário.");

            await _agendamentoRepository.AddAsync(entity);
            await _agendamentoRepository.SaveChangesAsync();

            return _mapper.Map<AgendamentosDTO>(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task<bool> UpdateAsync(int id, AgendamentosUpdateDTO dto)
        {
            var entity = await _agendamentoRepository.GetByIdAsync(id);

            if (entity is null)
                return false;

            // Atualiza entidade com os dados do DTO
            _mapper.Map(dto, entity);

            // Regra de negócio: não permitir conflito
            var conflito = await _agendamentoRepository.ExistsAsync(a =>
                a.Id != id &&
                a.BarbeiroId == entity.BarbeiroId &&
                a.DataHora == entity.DataHora
            );

            if (conflito)
                throw new InvalidOperationException("Já existe um agendamento para este barbeiro neste mesmo horário.");

            _agendamentoRepository.Update(entity);
            await _agendamentoRepository.SaveChangesAsync();

            return true;
        }

        // =========================
        // DELETE
        // =========================
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _agendamentoRepository.GetByIdAsync(id);

            if (entity is null)
                return false;

            _agendamentoRepository.Delete(entity);
            await _agendamentoRepository.SaveChangesAsync();

            return true;
        }
    }
}
