using BarberRezende.Application.DTOs.Barbeiros;
using BarberRezende.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberRezende.API.Controllers
{
    /// <summary>
    /// Endpoints HTTP (REST) de Barbeiros.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class BarbeirosController : ControllerBase
    {
        private readonly IBarbeirosService _barbeirosService;

        public BarbeirosController(IBarbeirosService barbeirosService)
        {
            _barbeirosService = barbeirosService;
        }

        /// <summary>
        /// GET: api/barbeiros
        /// Lista todos os barbeiros.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BarbeirosDTO>>> GetAll()
        {
            var result = await _barbeirosService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// GET: api/barbeiros/5
        /// Busca barbeiro por Id.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BarbeirosDTO>> GetById(int id)
        {
            var result = await _barbeirosService.GetByIdAsync(id);

            if (result is null)
                return NotFound(new { message = $"Barbeiro com Id={id} não encontrado." });

            return Ok(result);
        }

        /// <summary>
        /// POST: api/barbeiros
        /// Cria um novo barbeiro.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<BarbeirosDTO>> Create([FromBody] BarbeirosCreateDTO dto)
        {
            var created = await _barbeirosService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// PUT: api/barbeiros/5
        /// Atualiza um barbeiro existente.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] BarbeirosUpdateDTO dto)
        {
            var updated = await _barbeirosService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(new { message = $"Barbeiro com Id={id} não encontrado." });

            return NoContent();
        }

        /// <summary>
        /// DELETE: api/barbeiros/5
        /// Remove um barbeiro por Id.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _barbeirosService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = $"Barbeiro com Id={id} não encontrado." });

            return NoContent();
        }
    }
}
