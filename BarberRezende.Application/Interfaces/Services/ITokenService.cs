using System;
using BarberRezende.Domain.Entities;

namespace BarberRezende.Application.Interfaces.Services
{
    /// <summary>
    /// Contrato responsável pela geração de tokens JWT.
    /// 
    /// Fica na camada Application porque:
    /// - Token é regra de aplicação
    /// - Não pertence ao domínio
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Gera token JWT para o administrador autenticado.
        /// Retorna:
        /// - string Token
        /// - DateTime Data de expiração
        /// </summary>
        (string token, DateTime expiresAt) GenerateToken(AdminUser admin);
    }
}