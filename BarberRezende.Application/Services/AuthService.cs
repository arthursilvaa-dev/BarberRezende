using System.Threading.Tasks;
using BarberRezende.Application.DTOs.Auth;
using BarberRezende.Application.Interfaces.Services;
using BarberRezende.Domain.Interfaces;
using BCrypt.Net;

namespace BarberRezende.Application.Services
{
    /// <summary>
    /// Serviço responsável pela autenticação do administrador.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IAdminUserRepository _adminRepository;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Injeção de dependências.
        /// Recebemos:
        /// - Repositório de administradores
        /// - Serviço de geração de token
        /// </summary>
        public AuthService(
            IAdminUserRepository adminRepository,
            ITokenService tokenService)
        {
            _adminRepository = adminRepository;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Realiza o login do administrador.
        /// </summary>
        public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
        {
            // 1️⃣ Buscar usuário pelo email
            var admin = await _adminRepository.GetByEmailAsync(request.Email);

            if (admin == null) {
                throw new System.Exception("Credenciais inválidas");
            }

            // 2️⃣ Validar senha com BCrypt
            var passwordValid = BCrypt.Net.BCrypt.Verify(
                request.Password,
                admin.PasswordHash
            );

            if (!passwordValid) {
                throw new System.Exception("Credenciais inválidas");
            }

            // 3️⃣ Gerar token JWT
            var (token, expiresAt) = _tokenService.GenerateToken(admin);

            // 4️⃣ Retornar DTO
            return new LoginResponseDto {
                Token = token,
                ExpiresAt = expiresAt
            };
        }
    }
}