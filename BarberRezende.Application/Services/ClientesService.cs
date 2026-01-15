using AutoMapper;
using BarberRezende.Application.DTOs.Clientes;
using BarberRezende.Application.Interfaces.Services;
using BarberRezende.Domain.Entities;
using BarberRezende.Domain.Interfaces;

namespace BarberRezende.Application.Services
{
    /// <summary>
    /// Camada de aplicação: orquestra regras simples e integra DTO <-> Entidade.
    /// Aqui não tem EF diretamente; quem conversa com o banco é o Repository.
    /// </summary>
    public class ClientesService : IClientesService
    {
        private readonly IClienteRepository _repo;
        private readonly IMapper _mapper;

        public ClientesService(IClienteRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public Task<IEnumerable<ClientesDTO>> GetAllAsync()
        {
            // Repo retorna entidades
            var list = _repo.GetAll();

            // Service retorna DTOs
            var dto = _mapper.Map<IEnumerable<ClientesDTO>>(list);

            return Task.FromResult(dto);
        }

        public Task<ClientesDTO?> GetByIdAsync(int id)
        {
            var entity = _repo.GetById(id);
            if (entity is null) return Task.FromResult<ClientesDTO?>(null);

            var dto = _mapper.Map<ClientesDTO>(entity);
            return Task.FromResult<ClientesDTO?>(dto);
        }

        public Task<ClientesDTO> CreateAsync(ClientesCreateDTO dto)
        {
            // DTO -> Entidade
            var entity = _mapper.Map<Cliente>(dto);

            // Persiste
            _repo.Create(entity);

            // Entidade (já com Id) -> DTO
            var result = _mapper.Map<ClientesDTO>(entity);
            return Task.FromResult(result);
        }

        public Task<bool> UpdateAsync(int id, ClientesUpdateDTO dto)
        {
            var existing = _repo.GetById(id);
            if (existing is null) return Task.FromResult(false);

            // Atualiza campos do existing a partir do dto
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
