using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BarberRezende.API.Middlewares
{
    /// <summary>
    /// Middleware global para capturar exceptions e devolver uma resposta padronizada
    /// no formato ProblemDetails (padrão usado em APIs profissionais).
    /// 
    /// Vantagens:
    /// - Controllers ficam limpos (sem try/catch para tudo)
    /// - Respostas de erro consistentes (JSON padronizado)
    /// - Inclui TraceId para facilitar debug em logs
    /// </summary>
    public sealed class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try {
                // Continua o pipeline normal da aplicação
                await _next(context);
            }
            catch (Exception ex) {
                // Se algo explodir em qualquer lugar (Controller, Service, Repository, EF...)
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            // TraceId é essencial para "investigação" (você bate no log e acha o erro real)
            var traceId = context.TraceIdentifier;

            // Log sempre: em produção você quer isso no console/arquivo/AppInsights etc.
            _logger.LogError(ex,
                "Erro não tratado. TraceId: {TraceId}. Path: {Path}",
                traceId,
                context.Request.Path);

            // Define status code + título + detalhe de acordo com o tipo de exceção
            var (statusCode, title, type) = MapException(ex);

            // Monta o ProblemDetails (padrão)
            var problem = new ProblemDetails {
                Status = statusCode,
                Title = title,
                Type = type,
                Instance = context.Request.Path
            };

            // Em Development: você quer detalhe completo
            // Em Produção: você não quer vazar stack trace/infos internas
            if (_env.IsDevelopment()) {
                problem.Detail = ex.Message;
            }
            else {
                // Mensagem genérica em produção (segurança)
                problem.Detail = "Ocorreu um erro ao processar sua requisição.";
            }

            // Campo extra MUITO útil (você pode adicionar mais coisas depois)
            problem.Extensions["traceId"] = traceId;

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = statusCode;

            var jsonOptions = new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(problem, jsonOptions));
        }

        private static (int statusCode, string title, string type) MapException(Exception ex)
        {
            // 1) Erros de validação (ex: DataAnnotations / validação manual)
            if (ex is ValidationException) {
                return (
                    StatusCodes.Status400BadRequest,
                    "Dados inválidos.",
                    "https://httpstatuses.com/400"
                );
            }

            // 2) Argumentos inválidos (ex: id <= 0, dados faltando)
            if (ex is ArgumentException) {
                return (
                    StatusCodes.Status400BadRequest,
                    "Requisição inválida.",
                    "https://httpstatuses.com/400"
                );
            }

            // 3) Não encontrado
            // Use KeyNotFoundException quando quiser forçar um 404 no Service
            if (ex is KeyNotFoundException) {
                return (
                    StatusCodes.Status404NotFound,
                    "Recurso não encontrado.",
                    "https://httpstatuses.com/404"
                );
            }

            // 4) Conflito (regras de negócio / duplicidade)
            // Ex: tentativa de agendar 2 horários iguais pro mesmo barbeiro
            // Você pode lançar InvalidOperationException para "conflito" quando fizer sentido
            if (ex is InvalidOperationException) {
                return (
                    StatusCodes.Status409Conflict,
                    "Conflito de regra de negócio.",
                    "https://httpstatuses.com/409"
                );
            }

            // 5) Erros de banco (SQL) – pode ser conflito, constraint, etc.
            if (ex is DbUpdateException || ex is SqlException) {
                return (
                    StatusCodes.Status409Conflict,
                    "Erro ao persistir dados (conflito/constraint).",
                    "https://httpstatuses.com/409"
                );
            }

            // 6) Qualquer outra coisa vira 500
            return (
                StatusCodes.Status500InternalServerError,
                "Erro interno no servidor.",
                "https://httpstatuses.com/500"
            );
        }
    }
}
