using BarberRezende.Application.Exceptions;
using BarberRezende.Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace BarberRezende.API.Middlewares
{
    /// <summary>
    /// Middleware global de exceções:
    /// - Captura exceções que acontecerem em qualquer Controller/Service/Repository.
    /// - Retorna um ProblemDetails padronizado.
    /// - Converte regras de negócio em 409 (Conflict), validações em 400, não encontrado em 404.
    /// - Qualquer outra exceção vira 500 (com detalhe apenas em Development).
    /// </summary>
    public class ApiExceptionMiddleware : IMiddleware
    {
        private readonly ILogger<ApiExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ApiExceptionMiddleware(ILogger<ApiExceptionMiddleware> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                // Se não acontecer exceção, só segue o fluxo normal.
                await next(context);
            }
            catch (Exception ex)
            {
                // Loga o erro real (pra você ver no console e/ou arquivo se configurar)
                _logger.LogError(ex, "Unhandled exception: {Message}", ex.Message);

                // Usa application/problem+json
                context.Response.ContentType = "application/problem+json";

                // Identifica o tipo de erro e escolhe status + título + detail básico
                var (status, title, basicDetail, exceptionType) = ex switch
                {
                    // Regra de negócio (conflito de horário etc.)
                    BusinessRuleException bre => (
                        StatusCodes.Status409Conflict,
                        "business_error",
                        bre.Message,
                        nameof(BusinessRuleException)
                    ),

                    // Validação (se você usar exceção própria de validação)
                    ValidationException vex => (
                        StatusCodes.Status400BadRequest,
                        "validation_error",
                        vex.Message,
                        nameof(ValidationException)
                    ),

                    // Não encontrado (se você usar exceção própria)
                    NotFoundException nf => (
                        StatusCodes.Status404NotFound,
                        "not_found",
                        nf.Message,
                        nameof(NotFoundException)
                    ),

                    // Qualquer outro erro vira 500
                    _ => (
                        StatusCodes.Status500InternalServerError,
                        "unexpected_error",
                        "Erro inesperado. Tente novamente ou contate o suporte.",
                        ex.GetType().Name
                    )
                };

                context.Response.StatusCode = status;

                // Em Development expor detalhes completos para facilitar debug; em produção manter mensagem genérica.
                var detail = _env.IsDevelopment() ? ex.ToString() : basicDetail;

                var problem = new ProblemDetails
                {
                    Title = title,
                    Status = status,
                    Detail = detail,
                    Instance = context.Request.Path
                };

                // Extensões úteis para rastreio
                problem.Extensions["traceId"] = context.TraceIdentifier;
                problem.Extensions["exceptionType"] = exceptionType;

                if (_env.IsDevelopment())
                {
                    // Adiciona stack trace separadamente também
                    problem.Extensions["stackTrace"] = ex.StackTrace;
                }

                var json = JsonSerializer.Serialize(problem, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                await context.Response.WriteAsync(json);
            }
        }
    }
}
