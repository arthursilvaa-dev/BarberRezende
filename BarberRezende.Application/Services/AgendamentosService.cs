using AutoMapper;
using BarberRezende.Application.DTOs.Agendamentos;
using BarberRezende.Application.Interfaces.Services;
using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;

namespace BarberRezende.Application.Services
{
    public class AgendamentosService : IAgendamentosService
    {
        private readonly IAgendamentoRepository _repo;
        private readonly IMapper _mapper;

        public AgendamentosService(IAgendamentoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public Task<IEnumerable<AgendamentosDTO>> GetAllAsync()
        {
            var list = _repo.GetAll();
            var dto = _mapper.Map<IEnumerable<AgendamentosDTO>>(list);
            return Task.FromResult(dto);
        }

        public Task<AgendamentosDTO?> GetByIdAsync(int id)
        {
            var entity = _repo.GetById(id);
            if (entity is null) return Task.FromResult<AgendamentosDTO?>(null);

            var dto = _mapper.Map<AgendamentosDTO>(entity);
            return Task.FromResult<AgendamentosDTO?>(dto);
        }

        public Task<AgendamentosDTO> CreateAsync(AgendamentosCreateDTO dto)
        {
            var entity = _mapper.Map<Agendamento>(dto);
            _repo.Create(entity);

            var result = _mapper.Map<AgendamentosDTO>(entity);
            return Task.FromResult(result);
        }

        public Task<bool> UpdateAsync(int id, AgendamentosUpdateDTO dto)
        {
            var existing = _repo.GetById(id);
            if (existing is null) return Task.FromResult(false);

            _mapper.Map(dto, existing);
            _repo.Update(existing);

            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var existing = _repo.GetById(id);
            if (existing is null) return Task.FromResult(false);

            _repo.Delete(id);
            return Task.FromResult(true);
        }
    }
}
