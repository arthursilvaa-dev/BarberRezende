using AutoMapper;
using BarberRezende.Application.DTOs.Agendamentos;
using BarberRezende.Application.Exceptions;
using BarberRezende.Application.Interfaces.Services;
using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;

namespace BarberRezende.Application.Services
{
    /// <summary>
    /// Camada Application (Service):
    /// - Orquestra casos de uso (CRUD + filtros)
    /// - Aplica regras de aplicação e coordena repositórios
    /// - NÃO acessa DbContext diretamente (isso é Infra)
    /// - NÃO expõe entidades do Domain para fora (usa DTOs)
    /// </summary>
    public class AgendamentosService : IAgendamentosService
    {
        private readonly IAgendamentoRepository _agendamentoRepository;
        private readonly IServicoRepository _servicoRepository;
        private readonly IBarbeiroRepository _barbeiroRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public AgendamentosService(
            IAgendamentoRepository agendamentoRepository,
            IServicoRepository servicoRepository,
            IBarbeiroRepository barbeiroRepository,
            IClienteRepository clienteRepository,
            IMapper mapper)
        {
            _agendamentoRepository = agendamentoRepository;
            _servicoRepository = servicoRepository;
            _barbeiroRepository = barbeiroRepository;
            _clienteRepository = clienteRepository;
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
            if (agendamento is null) return null;

            return _mapper.Map<AgendamentosDTO>(agendamento);
        }

        // =========================
        // GET BY FILTER
        // =========================
        public async Task<IEnumerable<AgendamentosDTO>> GetByFilterAsync(
            int? barbeiroId,
            int? clienteId,
            int? servicoId,
            DateOnly? data)
        {
            var agendamentos = await _agendamentoRepository.GetAllAsync();

            if (clienteId.HasValue)
                agendamentos = agendamentos.Where(a => a.ClienteId == clienteId.Value);

            if (barbeiroId.HasValue)
                agendamentos = agendamentos.Where(a => a.BarbeiroId == barbeiroId.Value);

            if (servicoId.HasValue)
                agendamentos = agendamentos.Where(a => a.ServicoId == servicoId.Value);

            if (data.HasValue) {
                var dia = data.Value;
                agendamentos = agendamentos.Where(a => DateOnly.FromDateTime(a.DataHora) == dia);
            }

            return _mapper.Map<IEnumerable<AgendamentosDTO>>(agendamentos);
        }

        // =========================
        // CREATE
        // =========================
        public async Task<AgendamentosDTO> CreateAsync(AgendamentosCreateDTO dto)
        {
            var entity = _mapper.Map<Agendamento>(dto);

            if (!entity.ServicoId.HasValue || entity.ServicoId.Value <= 0)
                throw new BusinessRuleException("Serviço é obrigatório.");

            if (!entity.BarbeiroId.HasValue || entity.BarbeiroId.Value <= 0)
                throw new BusinessRuleException("Barbeiro é obrigatório.");

            if (!entity.ClienteId.HasValue || entity.ClienteId.Value <= 0)
                throw new BusinessRuleException("Cliente é obrigatório.");

            // 🔥 BUSCAR DADOS COMPLETOS
            var servico = await _servicoRepository.GetByIdAsync(entity.ServicoId.Value);
            if (servico is null)
                throw new BusinessRuleException("Serviço não encontrado.");

            var barbeiro = await _barbeiroRepository.GetByIdAsync(entity.BarbeiroId.Value);
            var cliente = await _clienteRepository.GetByIdAsync(entity.ClienteId.Value);

            if (barbeiro is null)
                throw new BusinessRuleException("Barbeiro não encontrado.");

            if (cliente is null)
                throw new BusinessRuleException("Cliente não encontrado.");

            // 🔥 SALVAR SNAPSHOTS (AQUI ESTÁ A MÁGICA)
            entity.ClienteNomeSnapshot = cliente.Nome;
            entity.BarbeiroNomeSnapshot = barbeiro.Nome;
            entity.ServicoNomeSnapshot = servico.Nome;
            entity.PrecoSnapshot = servico.Preco;
            entity.DuracaoMinutosSnapshot = servico.DuracaoMinutos;

            // =========================
            // VALIDAÇÃO DE CONFLITO (igual você já tinha)
            // =========================
            var start = entity.DataHora;
            var end = start.AddMinutes(servico.DuracaoMinutos);

            var existentes = await _agendamentoRepository
                .GetByBarbeiroInRangeWithServicoAsync(entity.BarbeiroId.Value, start.AddHours(-6), end.AddHours(6));

            foreach (var ag in existentes) {
                if (ag.Servico is null) continue;

                var existingStart = ag.DataHora;
                var existingEnd = ag.DataHora.AddMinutes(ag.Servico.DuracaoMinutos);

                if (start < existingEnd && existingStart < end)
                    throw new BusinessRuleException("Conflito de agenda.");
            }

            await _agendamentoRepository.AddAsync(entity);
            await _agendamentoRepository.SaveChangesAsync();

            return _mapper.Map<AgendamentosDTO>(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task<bool> UpdateAsync(int id, AgendamentosUpdateDTO dto)
        {
            // 1) Buscar o agendamento existente
            var entity = await _agendamentoRepository.GetByIdAsync(id);
            if (entity is null) return false;

            // 2) Aplicar alterações do DTO no entity
            _mapper.Map(dto, entity);

            // 3) Validar IDs obrigatórios (porque no seu entity eles são int?)
            //    Aqui é a correção do CS1503: garantir que não é null antes de usar.
            if (entity.ServicoId is null)
                throw new BusinessRuleException("Serviço é obrigatório para atualizar o agendamento.");

            if (entity.BarbeiroId is null)
                throw new BusinessRuleException("Barbeiro é obrigatório para atualizar o agendamento.");

            // ✅ agora viram int “de verdade”
            int servicoId = entity.ServicoId.Value;
            int barbeiroId = entity.BarbeiroId.Value;

            // 4) Buscar serviço para saber a duração
            var servico = await _servicoRepository.GetByIdAsync(servicoId);
            if (servico is null)
                throw new BusinessRuleException("Serviço não encontrado para o ServicoId informado.");

            // 5) Calcular janela do agendamento (start -> end)
            var start = entity.DataHora;
            var end = start.AddMinutes(servico.DuracaoMinutos);

            // 6) Buscar agendamentos do mesmo barbeiro em um range “seguro”
            var existentes = await _agendamentoRepository
                .GetByBarbeiroInRangeWithServicoAsync(
                    barbeiroId,
                    start.AddHours(-6),
                    end.AddHours(6)
                );

            // 7) Verificar conflito (overlap)
            foreach (var ag in existentes) {
                // ignora ele mesmo
                if (ag.Id == id) continue;

                // segurança: se não veio com serviço incluído, ignora
                if (ag.Servico is null) continue;

                var existingStart = ag.DataHora;
                var existingEnd = existingStart.AddMinutes(ag.Servico.DuracaoMinutos);

                // overlap: start < existingEnd && end > existingStart
                if (start < existingEnd && end > existingStart) {
                    throw new BusinessRuleException(
                        $"Conflito de horário: este barbeiro já possui um agendamento entre " +
                        $"{existingStart:dd/MM/yyyy HH:mm} e {existingEnd:HH:mm}."
                    );
                }
            }

            // 8) Persistir
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
            if (entity is null) return false;

            _agendamentoRepository.Delete(entity);
            await _agendamentoRepository.SaveChangesAsync();

            return true;
        }
    }
}
