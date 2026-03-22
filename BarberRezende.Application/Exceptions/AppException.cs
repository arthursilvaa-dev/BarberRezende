using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberRezende.Application.Exceptions
{
    /// <summary>
    /// Exceção base da camada Application.
    /// A ideia é: regras/fluxos lançam isso e a API traduz para HTTP.
    /// </summary>
    public abstract class AppException : Exception
    {
        /// <summary>
        /// Status HTTP sugerido para essa falha (400, 404, 409, etc.)
        /// </summary>
        public int StatusCode { get; }

        /// <summary>
        /// Código interno para você identificar rapidamente o tipo do erro.
        /// Ex: "validation_error", "not_found", "conflict"
        /// </summary>
        public string ErrorCode { get; }

        protected AppException(string message, int statusCode, string errorCode)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
        }
    }
}

