using System.Net;
using System.Text.Json;
using BarberRezende.API.Models;

namespace BarberRezende.API.Middlewares
{
    /// <summary>
    /// Middleware global para capturar exceções e devolver JSON padronizado.
    /// Assim a API nunca "quebra" em HTML feio, e o client sempre recebe um padrão.
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try {
                // Continua o pipeline normal (Controllers, etc.)
                await _next(context);
            }
            catch (InvalidOperationException ex) {
                // Ex.: conflito de horário, ids inválidos, regra quebrada -> 409/400
                await WriteErrorAsync(context, ex, HttpStatusCode.Conflict, "Regra de negócio violada");
            }
            catch (ArgumentException ex) {
                // Ex.: argumento inválido -> 400
                await WriteErrorAsync(context, ex, HttpStatusCode.BadRequest, "Erro de validação");
            }
            catch (Exception ex) {
                // Qualquer erro inesperado -> 500
                await WriteErrorAsync(context, ex, HttpStatusCode.InternalServerError, "Erro inesperado");
            }
        }

        private async Task WriteErrorAsync(HttpContext context, Exception ex, HttpStatusCode statusCode, string title)
        {
            // Loga no console/servidor (bom pra depuração)
            _logger.LogError(ex, "Erro capturado pelo middleware: {Message}", ex.Message);

            // Monta o JSON padrão
            var response = new ApiErrorResponse {
                Title = title,
                Status = (int)statusCode,
                Detail = ex.Message,
                Path = context.Request.Path
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            // Serializa com JSON legível
            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions {
                WriteIndented = true
            });

            await context.Response.WriteAsync(json);
        }
    }
}
