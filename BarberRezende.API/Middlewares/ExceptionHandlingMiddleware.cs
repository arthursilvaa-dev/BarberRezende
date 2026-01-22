using System.Net;
using System.Text.Json;
using BarberRezende.API.Errors;
using BarberRezende.Domain.Exceptions;

namespace BarberRezende.API.Middlewares
{
    /// <summary>
    /// Middleware global de tratamento de exceções.
    /// Ele intercepta erros não tratados e retorna um JSON padronizado.
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger,
            IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try {
                // Executa o próximo middleware (e a pipeline toda).
                await _next(context);
            }
            catch (Exception ex) {
                // Se algo explodir em qualquer lugar (controller/service/repository),
                // caímos aqui e devolvemos um erro padronizado.
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // Logamos sempre o erro (no mínimo no console).
            // Em empresas, isso iria para Application Insights, ELK, Datadog, etc.
            _logger.LogError(ex, "Erro não tratado: {Message}", ex.Message);

            // Definimos o status code conforme o tipo do erro.
            int statusCode = ex switch {
                DomainException => (int)HttpStatusCode.BadRequest,   // 400
                NotFoundException => (int)HttpStatusCode.NotFound,   // 404
                _ => (int)HttpStatusCode.InternalServerError         // 500
            };

            // Mensagem: em produção, evite expor detalhes internos.
            string message = ex switch {
                DomainException => ex.Message,
                NotFoundException => ex.Message,
                _ => "Ocorreu um erro inesperado. Tente novamente mais tarde."
            };

            var errorResponse = new ApiErrorResponse {
                StatusCode = statusCode,
                Message = message,
                TraceId = context.TraceIdentifier,

                // Só em Development devolvemos detalhes.
                Details = _env.IsDevelopment()
                    ? new
                    {
                        exception = ex.GetType().Name,
                        ex.Message,
                        ex.StackTrace
                    }
                    : null
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            // Serialização padronizada (camelCase fica mais “padrão REST”)
            var jsonOptions = new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(errorResponse, jsonOptions);
            await context.Response.WriteAsync(json);
        }
    }
}
