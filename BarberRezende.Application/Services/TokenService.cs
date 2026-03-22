using BarberRezende.Application.Interfaces.Services;
using BarberRezende.Application.Settings;
using BarberRezende.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

//JWT Aqui se gera o token JWT para o administrador autenticado.
//O token inclui informações como o ID do usuário e o email, e é assinado
//usando uma chave secreta definida nas configurações. O token tem uma
//data de expiração para garantir segurança.
namespace BarberRezende.Application.Services
{
    public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;

        public TokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public (string token, DateTime expiresAt) GenerateToken(AdminUser admin)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_jwtSettings.SecretKey)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var expiresAt = DateTime.UtcNow
                .AddMinutes(_jwtSettings.ExpirationInMinutes);

            var claims = new List<Claim>
            {
        new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, admin.Id.ToString()),
        new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, admin.Email),
        new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return (tokenString, expiresAt);
        }
    }
}