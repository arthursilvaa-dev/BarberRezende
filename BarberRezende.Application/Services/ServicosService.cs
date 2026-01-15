using AutoMapper;
using BarberRezende.Application.DTOs.Servicos;
using BarberRezende.Application.Interfaces.Services;
using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;

namespace BarberRezende.Application.Services
{
    public class ServicosService : IServicosService
    {
        private readonly IServicoRepository _repo;
        private readonly IMapper _mapper;

        public ServicosService(IServicoRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public Task<IEnumerable<ServicosDTO>> GetAllAsync()
        {
            var list = _repo.GetAll();
            var dto = _mapper.Map<IEnumerable<ServicosDTO>>(list);
            return Task.FromResult(dto);
        }

        public Task<ServicosDTO?> GetByIdAsync(int id)
        {
            var entity = _repo.GetById(id);
            if (entity is null) return Task.FromResult<ServicosDTO?>(null);

            var dto = _mapper.Map<ServicosDTO>(entity);
            return Task.FromResult<ServicosDTO?>(dto);
        }

        public Task<ServicosDTO> CreateAsync(ServicosCreateDTO dto)
        {
            var entity = _mapper.Map<Servico>(dto);
            _repo.Create(entity);

            var result = _mapper.Map<ServicosDTO>(entity);
            return Task.FromResult(result);
        }

        public Task<bool> UpdateAsync(int id, ServicosUpdateDTO dto)
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
