using AutoMapper;
using BarberRezende.Application.DTOs.Clientes;
using BarberRezende.Application.Interfaces.Services;
using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;

namespace BarberRezende.Application.Services
{
    /// <summary>
    /// Orquestra as regras de Cliente.
    /// Aqui você valida, aplica regras e chama o repositório (Infra).
    /// </summary>
    public class ClientesService : IClientesService
    {
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;

        public ClientesService(IClienteRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ClientesDTO>> GetAllAsync()
        {
            var entities = await _clienteRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ClientesDTO>>(entities);
        }

        public async Task<ClientesDTO?> GetByIdAsync(int id)
        {
            var entity = await _clienteRepository.GetByIdAsync(id);

            if (entity is null)
                return null;

            return _mapper.Map<ClientesDTO>(entity);
        }

        public async Task<ClientesDTO> CreateAsync(ClientesCreateDTO dto)
        {
            // Regra simples (exemplo): não deixar telefone vazio/space
            // (Validações mais profissionais faremos depois com FluentValidation)
            if (string.IsNullOrWhiteSpace(dto.Telefone))
                throw new ArgumentException("Telefone é obrigatório.");

            var entity = _mapper.Map<Cliente>(dto);

            await _clienteRepository.AddAsync(entity);
            await _clienteRepository.SaveChangesAsync();

            return _mapper.Map<ClientesDTO>(entity);
        }

        public async Task<bool> UpdateAsync(int id, ClientesUpdateDTO dto)
        {
            var existing = await _clienteRepository.GetByIdAsync(id);

            if (existing is null)
                return false;

            // Atualiza campos do existing com base no DTO
            _mapper.Map(dto, existing);

            _clienteRepository.Update(existing);
            await _clienteRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _clienteRepository.GetByIdAsync(id);

            if (existing is null)
                return false;

            _clienteRepository.Delete(existing);
            await _clienteRepository.SaveChangesAsync();

            return true;
        }
    }
}
