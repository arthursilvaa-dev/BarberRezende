using BarberRezende.API.Helpers;
using BarberRezende.Application.DTOs.Servicos;
using BarberRezende.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberRezende.API.Controllers
{
    /// <summary>
    /// Endpoints HTTP (REST) de Serviços.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ServicosController : ControllerBase
    {
        private readonly IServicosService _servicosService;

        public ServicosController(IServicosService servicosService)
        {
            _servicosService = servicosService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ServicosDTO>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ServicosDTO>>> GetAll()
        {
            var result = await _servicosService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(ServicosDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServicosDTO>> GetById(int id)
        {
            var result = await _servicosService.GetByIdAsync(id);

            if (result is null)
                return NotFound(ApiProblemFactory.NotFound(HttpContext, $"Serviço com Id {id} não encontrado."));

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ServicosDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ServicosDTO>> Create([FromBody] ServicosCreateDTO dto)
        {
            var created = await _servicosService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update(int id, [FromBody] ServicosUpdateDTO dto)
        {
            var updated = await _servicosService.UpdateAsync(id, dto);

            if (!updated)
                return NotFound(ApiProblemFactory.NotFound(HttpContext, $"Serviço com Id {id} não encontrado."));

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _servicosService.DeleteAsync(id);

            if (!deleted)
                return NotFound(ApiProblemFactory.NotFound(HttpContext, $"Serviço com Id {id} não encontrado."));

            return NoContent();
        }
    }
}
