using AutoMapper;
using BarberRezende.Application.DTOs.Servicos;
using BarberRezende.Application.Interfaces.Services;
using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;

namespace BarberRezende.Application.Services
{
    /// <summary>
    /// Regras/orquestração de Serviços.
    /// Aqui fazemos validações e chamamos o repositório.
    /// </summary>
    public class ServicosService : IServicosService
    {
        private readonly IServicoRepository _servicoRepository;
        private readonly IMapper _mapper;

        public ServicosService(IServicoRepository servicoRepository, IMapper mapper)
        {
            _servicoRepository = servicoRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServicosDTO>> GetAllAsync()
        {
            var entities = await _servicoRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ServicosDTO>>(entities);
        }

        public async Task<ServicosDTO?> GetByIdAsync(int id)
        {
            var entity = await _servicoRepository.GetByIdAsync(id);

            if (entity is null)
                return null;

            return _mapper.Map<ServicosDTO>(entity);
        }

        public async Task<ServicosDTO> CreateAsync(ServicosCreateDTO dto)
        {
            // Validações mínimas (profissionais) -> evita lixo no banco
            if (string.IsNullOrWhiteSpace(dto.NomeServico))
                throw new ArgumentException("Nome do serviço é obrigatório.");

            if (dto.Preco <= 0)
                throw new ArgumentException("Preço precisa ser maior que zero.");

            if (dto.DuracaoMinutos <= 0)
                throw new ArgumentException("Duração precisa ser maior que zero.");

            var entity = _mapper.Map<Servico>(dto);

            await _servicoRepository.AddAsync(entity);
            await _servicoRepository.SaveChangesAsync();

            return _mapper.Map<ServicosDTO>(entity);
        }

        public async Task<bool> UpdateAsync(int id, ServicosUpdateDTO dto)
        {
            var existing = await _servicoRepository.GetByIdAsync(id);

            if (existing is null)
                return false;

            // Se você quiser permitir "atualização parcial", aqui é o lugar.
            // Por enquanto: exige preencher corretamente.
            if (string.IsNullOrWhiteSpace(dto.NomeServico))
                throw new ArgumentException("Nome do serviço é obrigatório.");

            if (dto.Preco <= 0)
                throw new ArgumentException("Preço precisa ser maior que zero.");

            if (dto.DuracaoMinutos <= 0)
                throw new ArgumentException("Duração precisa ser maior que zero.");

            _mapper.Map(dto, existing);

            _servicoRepository.Update(existing);
            await _servicoRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _servicoRepository.GetByIdAsync(id);

            if (existing is null)
                return false;

            _servicoRepository.Delete(existing);
            await _servicoRepository.SaveChangesAsync();

            return true;
        }
    }
}
