using AutoMapper;
using BarberRezende.Application.DTOs.Barbeiros;
using BarberRezende.Application.Interfaces.Services;
using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;

namespace BarberRezende.Application.Services
{
    /// <summary>
    /// Regras/orquestração de Barbeiros.
    /// Aqui fica a lógica de negócio e chamadas ao repositório.
    /// </summary>
    public class BarbeirosService : IBarbeirosService
    {
        private readonly IBarbeiroRepository _barbeiroRepository;
        private readonly IMapper _mapper;

        public BarbeirosService(IBarbeiroRepository barbeiroRepository, IMapper mapper)
        {
            _barbeiroRepository = barbeiroRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BarbeirosDTO>> GetAllAsync()
        {
            var entities = await _barbeiroRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<BarbeirosDTO>>(entities);
        }

        public async Task<BarbeirosDTO?> GetByIdAsync(int id)
        {
            var entity = await _barbeiroRepository.GetByIdAsync(id);

            if (entity is null)
                return null;

            return _mapper.Map<BarbeirosDTO>(entity);
        }

        public async Task<BarbeirosDTO> CreateAsync(BarbeirosCreateDTO dto)
        {
            // Exemplo de validação simples
            if (string.IsNullOrWhiteSpace(dto.Nome))
                throw new ArgumentException("Nome do barbeiro é obrigatório.");

            if (string.IsNullOrWhiteSpace(dto.Especialidade))
                throw new ArgumentException("Especialidade do barbeiro é obrigatória.");

            var entity = _mapper.Map<Barbeiro>(dto);

            await _barbeiroRepository.AddAsync(entity);
            await _barbeiroRepository.SaveChangesAsync();

            return _mapper.Map<BarbeirosDTO>(entity);
        }

        public async Task<bool> UpdateAsync(int id, BarbeirosUpdateDTO dto)
        {
            var existing = await _barbeiroRepository.GetByIdAsync(id);

            if (existing is null)
                return false;

            _mapper.Map(dto, existing);

            _barbeiroRepository.Update(existing);
            await _barbeiroRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _barbeiroRepository.GetByIdAsync(id);

            if (existing is null)
                return false;

            _barbeiroRepository.Delete(existing);
            await _barbeiroRepository.SaveChangesAsync();

            return true;
        }
    }
}
