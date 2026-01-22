using System.Text.Json.Serialization;

namespace BarberRezende.API.Errors
{
    /// <summary>
    /// Modelo padrão de erro devolvido pela API.
    /// Fica na camada API porque isso é "contrato HTTP/JSON".
    /// </summary>
    public class ApiErrorResponse
    {
        /// <summary>
        /// Código HTTP (400, 404, 500...)
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Mensagem amigável para o cliente/consumidor da API.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Identificador da requisição (ajuda a debugar logs).
        /// </summary>
        public string TraceId { get; set; } = string.Empty;

        /// <summary>
        /// Em desenvolvimento, podemos devolver detalhes extras.
        /// Em produção, normalmente fica nulo.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? Details { get; set; }
    }
}
