using System.Net;
using System.Text.Json;
using BarberRezende.Application.Exceptions;

namespace BarberRezende.API.Middlewares
{
    /// <summary>
    /// Middleware global:
    /// - Captura exceções
    /// - Converte em JSON padronizado (ProblemDetails-like)
    /// - Diferencia erro de negócio (409) de erro inesperado (500)
    /// </summary>
    public sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try {
                await next(context);
            }
            catch (BusinessRuleException ex) {
                // Regra de negócio: não é bug do sistema, é conflito/restrição do domínio.
                _logger.LogWarning(ex, "Business rule violation. TraceId: {TraceId}", context.TraceIdentifier);

                await WriteProblemDetailsAsync(
                    context,
                    statusCode: HttpStatusCode.Conflict,
                    title: "business_error",
                    detail: ex.Message,
                    exceptionType: nameof(BusinessRuleException)
                );
            }
            catch (Exception ex) {
                // Erro inesperado: isso é bug, ou infra, ou API caiu, etc.
                _logger.LogError(ex, "Unexpected error. TraceId: {TraceId}", context.TraceIdentifier);

                await WriteProblemDetailsAsync(
                    context,
                    statusCode: HttpStatusCode.InternalServerError,
                    title: "unexpected_error",
                    detail: "Erro inesperado. Tente novamente ou contate o suporte.",
                    exceptionType: ex.GetType().Name
                );
            }
        }

        private static async Task WriteProblemDetailsAsync(
            HttpContext context,
            HttpStatusCode statusCode,
            string title,
            string detail,
            string exceptionType)
        {
            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;

            // Objeto "ProblemDetails-like"
            var payload = new
            {
                title,
                status = (int)statusCode,
                detail,
                instance = context.Request.Path.ToString(),
                traceId = context.TraceIdentifier,
                exceptionType // em produção você pode remover para não expor detalhes
            };

            var json = JsonSerializer.Serialize(payload, new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            await context.Response.WriteAsync(json);
        }
    }
}
