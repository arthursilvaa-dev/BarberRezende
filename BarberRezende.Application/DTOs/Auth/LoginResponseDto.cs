namespace BarberRezende.Application.DTOs.Auth
{
    /// <summary>
    /// DTO retornado após autenticação bem-sucedida.
    /// Contém o token JWT.
    /// </summary>
    public class LoginResponseDto
    {
        /// <summary>
        /// Token JWT gerado para o usuário autenticado.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Data de expiração do token.
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
}