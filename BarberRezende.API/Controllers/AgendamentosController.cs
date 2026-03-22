using BarberRezende.Application.DTOs.Agendamentos;
using BarberRezende.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberRezende.API.Controllers
{
    /// <summary>
    /// Endpoints HTTP (REST) de Agendamentos.
    /// 
    /// Responsabilidades:
    /// - Receber requisições
    /// - Delegar para o Service
    /// - Traduzir exceções de negócio em respostas HTTP corretas
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

        // =========================================================
        // GET ALL
        // =========================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AgendamentosDTO>>> GetAll()
        {
            var result = await _agendamentosService.GetAllAsync();
            return Ok(result);
        }

        // =========================================================
        // GET BY FILTER
        // =========================================================
        [HttpGet("filter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<AgendamentosDTO>>> GetByFilter(
            [FromQuery] int? clienteId,
            [FromQuery] int? barbeiroId,
            [FromQuery] int? servicoId,
            [FromQuery] DateOnly? data)
        {
            var result = await _agendamentosService
                .GetByFilterAsync(clienteId, barbeiroId, servicoId, data);

            return Ok(result);
        }

        // =========================================================
        // GET BY ID
        // =========================================================
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

        // =========================================================
        // CREATE
        // =========================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AgendamentosDTO>> Create(
            [FromBody] AgendamentosCreateDTO dto)
        {
            try {
                var created = await _agendamentosService.CreateAsync(dto);

                return CreatedAtAction(
                    nameof(GetById),
                    new { id = created.Id },
                    created
                );
            }
            catch (InvalidOperationException ex) {
                // ⚠️ REGRA DE NEGÓCIO VIOLADA
                // Ex: conflito de horário
                return Conflict(new { message = ex.Message });
            }
        }

        // =========================================================
        // UPDATE
        // =========================================================
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(
            int id,
            [FromBody] AgendamentosUpdateDTO dto)
        {
            try {
                var updated = await _agendamentosService.UpdateAsync(id, dto);

                if (!updated)
                    return NotFound(new { message = $"Agendamento com Id={id} não encontrado." });

                return NoContent();
            }
            catch (InvalidOperationException ex) {
                // Regra de negócio violada
                return Conflict(new { message = ex.Message });
            }
        }

        // =========================================================
        // DELETE
        // =========================================================
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
