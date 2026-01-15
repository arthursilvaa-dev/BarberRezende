using AutoMapper;
using BarberRezende.Application.DTOs.Funcionarios;
using BarberRezende.Application.Interfaces.Services;
using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;

namespace BarberRezende.Application.Services
{
    public class FuncionariosService : IFuncionariosService
    {
        private readonly IFuncionarioRepository _repo;
        private readonly IMapper _mapper;

        public FuncionariosService(IFuncionarioRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public Task<IEnumerable<FuncionariosDTO>> GetAllAsync()
        {
            var list = _repo.GetAll();
            var dto = _mapper.Map<IEnumerable<FuncionariosDTO>>(list);
            return Task.FromResult(dto);
        }

        public Task<FuncionariosDTO?> GetByIdAsync(int id)
        {
            var entity = _repo.GetById(id);
            if (entity is null) return Task.FromResult<FuncionariosDTO?>(null);

            var dto = _mapper.Map<FuncionariosDTO>(entity);
            return Task.FromResult<FuncionariosDTO?>(dto);
        }

        public Task<FuncionariosDTO> CreateAsync(FuncionariosCreateDTO dto)
        {
            var entity = _mapper.Map<Funcionario>(dto);
            _repo.Create(entity);

            var result = _mapper.Map<FuncionariosDTO>(entity);
            return Task.FromResult(result);
        }

        public Task<bool> UpdateAsync(int id, FuncionariosUpdateDTO dto)
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
