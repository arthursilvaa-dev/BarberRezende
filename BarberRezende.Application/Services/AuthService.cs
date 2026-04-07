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

        /// <summary>
        /// Troca a senha do administrador validando a senha atual.
        /// </summary>
        public async Task<bool> ChangePasswordAsync(ChangePasswordDto request)
        {
            // 1️⃣ Buscar o usuário (Admin)
            var admin = await _adminRepository.GetByEmailAsync(request.Email);
            if (admin == null)
                throw new System.Exception("Usuário não encontrado.");

            // 2️⃣ Validar se a senha atual digitada está correta
            var passwordValid = BCrypt.Net.BCrypt.Verify(request.SenhaAtual, admin.PasswordHash);
            if (!passwordValid)
                return false; // Retorna falso se ele errar a senha atual

            // 3️⃣ Criptografar a NOVA senha
            admin.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NovaSenha);

            // 4️⃣ Salvar no banco (Estou assumindo que seu repositório tem um método Update)
            await _adminRepository.UpdateAsync(admin);

            return true;
        }


    }
}