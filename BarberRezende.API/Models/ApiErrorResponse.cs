namespace BarberRezende.API.Models
{
    /// <summary>
    /// Resposta padrão de erro da API.
    /// Mantém um formato consistente para o front, Postman, Swagger e logs.
    /// </summary>
    public class ApiErrorResponse
    {
        /// <summary>
        /// Um título curto do erro (ex: "Erro de validação").
        /// </summary>
        public string Title { get; set; } = "Erro";

        /// <summary>
        /// Status HTTP (400, 404, 409, 500...).
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Mensagem mais detalhada do erro (explicação para o dev/front).
        /// </summary>
        public string Detail { get; set; } = string.Empty;

        /// <summary>
        /// (Opcional) Nome do endpoint / caminho que deu erro.
        /// Ajuda muito na depuração.
        /// </summary>
        public string? Path { get; set; }
    }
}
