using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberRezende.Application.DTOs.Auth
{
    /// <summary>
    /// DTO responsável por receber os dados de login.
    /// Utilizado pela API no endpoint de autenticação.
    /// </summary>
    public class LoginRequestDto
    {
        /// <summary>
        /// Email do administrador.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Senha digitada pelo usuário.
        /// Nunca será armazenada.
        /// Apenas validada.
        /// </summary>
        public string Password { get; set; }
    }
}