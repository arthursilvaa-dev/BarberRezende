using BarberRezende.Application.DTOs.Clientes;
using BarberRezende.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberRezende.API.Controllers
{
    /// <summary>
    /// Endpoints HTTP (REST) de Clientes.
    /// Responsabilidade do Controller: receber request e devolver response (status code + payload).
    /// Responsabilidade do Service: lógica de aplicação (CRUD + regras do fluxo).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ClientesController : ControllerBase
    {
        private readonly IClientesService _clientesService;

        public ClientesController(IClientesService clientesService)
        {
            _clientesService = clientesService;
        }

        /// <summary>
        /// GET: api/clientes
        /// Retorna todos os clientes.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ClientesDTO>>> GetAll()
        {
            var result = await _clientesService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// GET: api/clientes/5
        /// Retorna um cliente por Id.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClientesDTO>> GetById(int id)
        {
            var result = await _clientesService.GetByIdAsync(id);

            if (result is null)
                return NotFound(new { message = $"Cliente com Id={id} não encontrado." });

            return Ok(result);
        }

        /// <summary>
        /// POST: api/clientes
        /// Cria um novo cliente.
        /// Observação: validações básicas ficam no DTO (DataAnnotations) e o ASP.NET retorna 400 automaticamente.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ClientesDTO>> Create([FromBody] ClientesCreateDTO dto)
        {
            var created = await _clientesService.CreateAsync(dto);

            // 201 Created com header Location apontando para o recurso recém-criado
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// PUT: api/clientes/5
        /// Atualiza um cliente existente.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] ClientesUpdateDTO dto)
        {
            var updated = await _clientesService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(new { message = $"Cliente com Id={id} não encontrado." });

            // 204 NoContent: atualização concluída e não precisamos retornar corpo
            return NoContent();
        }

        /// <summary>
        /// DELETE: api/clientes/5
        /// Remove um cliente por Id.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _clientesService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = $"Cliente com Id={id} não encontrado." });

            return NoContent();
        }
    }
}
