using BarberRezende.Application.DTOs.Funcionarios;
using BarberRezende.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberRezende.API.Controllers
{
    /// <summary>
    /// Endpoints HTTP (REST) de Funcionários.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FuncionariosController : ControllerBase
    {
        private readonly IFuncionariosService _funcionariosService;

        public FuncionariosController(IFuncionariosService funcionariosService)
        {
            _funcionariosService = funcionariosService;
        }

        /// <summary>
        /// GET: api/funcionarios
        /// Lista todos os funcionários.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<FuncionariosDTO>>> GetAll()
        {
            var result = await _funcionariosService.GetAllAsync();
            return Ok(result);
        }

        /// <summary>
        /// GET: api/funcionarios/5
        /// Busca funcionário por Id.
        /// </summary>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FuncionariosDTO>> GetById(int id)
        {
            var result = await _funcionariosService.GetByIdAsync(id);

            if (result is null)
                return NotFound(new { message = $"Funcionário com Id={id} não encontrado." });

            return Ok(result);
        }

        /// <summary>
        /// POST: api/funcionarios
        /// Cria um novo funcionário.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FuncionariosDTO>> Create([FromBody] FuncionariosCreateDTO dto)
        {
            var created = await _funcionariosService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// PUT: api/funcionarios/5
        /// Atualiza um funcionário existente.
        /// </summary>
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] FuncionariosUpdateDTO dto)
        {
            var updated = await _funcionariosService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(new { message = $"Funcionário com Id={id} não encontrado." });

            return NoContent();
        }

        /// <summary>
        /// DELETE: api/funcionarios/5
        /// Remove um funcionário por Id.
        /// </summary>
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _funcionariosService.DeleteAsync(id);

            if (!deleted)
                return NotFound(new { message = $"Funcionário com Id={id} não encontrado." });

            return NoContent();
        }
    }
}
