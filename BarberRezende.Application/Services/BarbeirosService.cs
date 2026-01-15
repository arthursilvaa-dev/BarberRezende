using AutoMapper;
using BarberRezende.Application.DTOs.Barbeiros;
using BarberRezende.Application.Interfaces.Services;
using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;

namespace BarberRezende.Application.Services
{
    public class BarbeirosService : IBarbeirosService
    {
        private readonly IBarbeiroRepository _repo;
        private readonly IMapper _mapper;

        public BarbeirosService(IBarbeiroRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public Task<IEnumerable<BarbeirosDTO>> GetAllAsync()
        {
            var list = _repo.GetAll();
            var dto = _mapper.Map<IEnumerable<BarbeirosDTO>>(list);
            return Task.FromResult(dto);
        }

        public Task<BarbeirosDTO?> GetByIdAsync(int id)
        {
            var entity = _repo.GetById(id);
            if (entity is null) return Task.FromResult<BarbeirosDTO?>(null);

            var dto = _mapper.Map<BarbeirosDTO>(entity);
            return Task.FromResult<BarbeirosDTO?>(dto);
        }

        public Task<BarbeirosDTO> CreateAsync(BarbeirosCreateDTO dto)
        {
            var entity = _mapper.Map<Barbeiro>(dto);
            _repo.Create(entity);

            var result = _mapper.Map<BarbeirosDTO>(entity);
            return Task.FromResult(result);
        }

        public Task<bool> UpdateAsync(int id, BarbeirosUpdateDTO dto)
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
