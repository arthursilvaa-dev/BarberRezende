using AutoMapper;
using BarberRezende.Application.DTOs.Funcionarios;
using BarberRezende.Application.Interfaces.Services;
using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;

namespace BarberRezende.Application.Services
{
    public class FuncionariosService : IFuncionariosService
    {
        private readonly IFuncionarioRepository _funcionarioRepository;
        private readonly IMapper _mapper;

        public FuncionariosService(IFuncionarioRepository funcionarioRepository, IMapper mapper)
        {
            _funcionarioRepository = funcionarioRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FuncionariosDTO>> GetAllAsync()
        {
            var funcionarios = await _funcionarioRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<FuncionariosDTO>>(funcionarios);
        }

        public async Task<FuncionariosDTO?> GetByIdAsync(int id)
        {
            var funcionario = await _funcionarioRepository.GetByIdAsync(id);
            return funcionario is null ? null : _mapper.Map<FuncionariosDTO>(funcionario);
        }

        public async Task<FuncionariosDTO> CreateAsync(FuncionariosCreateDTO dto)
        {
            var entity = _mapper.Map<Funcionario>(dto);

            await _funcionarioRepository.AddAsync(entity);
            await _funcionarioRepository.SaveChangesAsync();

            return _mapper.Map<FuncionariosDTO>(entity);
        }

        public async Task<bool> UpdateAsync(int id, FuncionariosUpdateDTO dto)
        {
            var existing = await _funcionarioRepository.GetByIdAsync(id);
            if (existing is null) return false;

            _mapper.Map(dto, existing);

            _funcionarioRepository.Update(existing);
            await _funcionarioRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _funcionarioRepository.GetByIdAsync(id);
            if (existing is null) return false;

            _funcionarioRepository.Delete(existing);
            await _funcionarioRepository.SaveChangesAsync();

            return true;
        }
    }
}
