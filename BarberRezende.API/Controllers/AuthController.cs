using BarberRezende.Application.DTOs.Auth;
using BarberRezende.Application.Interfaces;
using BarberRezende.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberRezende.API.Controllers
{
    /// <summary>
    /// Controller responsável pela autenticação do administrador.
    /// Aqui ficará o endpoint de login que retorna o token JWT.
    /// </summary>
    [ApiController]

    // Define a rota base da API.
    // Exemplo final da rota: /api/auth/login
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        // Injeção do serviço de autenticação
        private readonly IAuthService _authService;

        /// <summary>
        /// Construtor do controller.
        /// Recebe o AuthService através da injeção de dependência.
        /// </summary>
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Endpoint responsável pelo login do administrador.
        /// Recebe email e senha e retorna um token JWT se as credenciais forem válidas.
        /// </summary>
        /// <param name="loginDto">Dados de login (email e senha)</param>
        /// <returns>Token JWT e data de expiração</returns>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequestDto loginDto)
        {
            // Chama o serviço de autenticação
            var response = await _authService.LoginAsync(loginDto);

            // Se a autenticação falhar, retorna Unauthorized (401)
            if (response == null) {
                return Unauthorized("Email ou senha inválidos.");
            }

            // Se autenticar corretamente, retorna o token
            return Ok(response);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
        {
            try {
                var success = await _authService.ChangePasswordAsync(request);
                if (!success)
                    return BadRequest(new { message = "Senha atual incorreta." });

                return Ok(new { message = "Senha alterada com sucesso!" });
            }
            catch (System.Exception ex) {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}