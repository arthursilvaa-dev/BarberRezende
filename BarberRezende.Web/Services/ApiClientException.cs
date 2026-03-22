using System.Net;

namespace BarberRezende.Web.Services
{
    /// <summary>
    /// Exceção do ApiClient:
    /// - Carrega StatusCode + Body (texto) retornado pela API
    /// - Controllers capturam isso e mostram mensagem no front
    /// </summary>
    public class ApiClientException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string? Body { get; }

        public ApiClientException(HttpStatusCode statusCode, string? body)
            : base(BuildMessage(statusCode, body))
        {
            StatusCode = statusCode;
            Body = body;
        }

        private static string BuildMessage(HttpStatusCode statusCode, string? body)
        {
            // Se a API já retorna JSON com "message", você pode melhorar depois.
            // Por agora, isso garante que SEMPRE terá uma mensagem.
            if (!string.IsNullOrWhiteSpace(body))
                return $"HTTP {(int)statusCode} ({statusCode}): {body}";

            return $"HTTP {(int)statusCode} ({statusCode}).";
        }
    }
}
