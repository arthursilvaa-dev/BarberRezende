using BarberRezende.Application.DTOs.Agendamentos;
using BarberRezende.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberRezende.API.Controllers
{
    /// <summary>
    /// Endpoints HTTP (REST) de Agendamentos.
    /// O Controller recebe a requisição e delega a execução para o Service.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AgendamentosController : ControllerBase
    {
        private readonly IAgendamentosService _agendamentosService;

        public AgendamentosController(IAgendamentosService agendamentosService)
        {
            _agendamentosService = agendamentosService;
        }

        /// <summary>
        /// Lista todos os agendamentos.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AgendamentosDTO>>> GetAll()
        {
            var result = await _agendamentosService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// Lista agendamentos por filtros opcionais:
        /// clienteId, barbeiroId, servicoId e data (apenas o dia).
        /// Ex: /api/agendamentos/filter?clienteId=1&barbeiroId=2&data=2026-01-22
        /// </summary>
        [HttpGet("filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AgendamentosDTO>>> GetByFilter(
            [FromQuery] int? clienteId,
            [FromQuery] int? barbeiroId,
            [FromQuery] int? servicoId,
            [FromQuery] DateOnly? data)
        {
            var result = await _agendamentosService.GetByFilterAsync(clienteId, barbeiroId, servicoId, data);
            return Ok(result);
        }

        /// <summary>
        /// Busca um agendamento por Id.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<AgendamentosDTO>> GetById(int id)
        {
            var result = await _agendamentosService.GetByIdAsync(id);

            if (result is null)
                return NotFound(new { message = $"Agendamento com Id={id} não encontrado." });

            return Ok(result);
        }

        /// <summary>
        /// Cria um novo agendamento.
        /// Validações básicas (Required/Range) ficam no DTO com DataAnnotations.
        /// Regras de negócio (ex: não duplicar horário pro mesmo barbeiro) ficam no Domain/Service.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AgendamentosDTO>> Create([FromBody] AgendamentosCreateDTO dto)
        {
            var created = await _agendamentosService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Atualiza um agendamento.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] AgendamentosUpdateDTO dto)
        {
            var updated = await _agendamentosService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(new { message = $"Agendamento com Id={id} não encontrado." });

            return NoContent();
        }

        /// <summary>
        /// Remove um agendamento.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _agendamentosService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = $"Agendamento com Id={id} não encontrado." });

            return NoContent();
        }
    }
}
