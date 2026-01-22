using Microsoft.AspNetCore.Mvc;

namespace BarberRezende.API.Helpers
{
    /// <summary>
    /// Centraliza a criação de respostas de erro no padrão RFC 7807 (ProblemDetails).
    /// Benefícios:
    /// - formato único de erro na API toda
    /// - facilita debug (traceId)
    /// - fica mais profissional no Swagger e em logs
    /// </summary>
    public static class ApiProblemFactory
    {
        public static ProblemDetails NotFound(HttpContext http, string detail)
        {
            var problem = new ProblemDetails {
                Status = StatusCodes.Status404NotFound,
                Title = "Recurso não encontrado.",
                Type = "https://httpstatuses.com/404",
                Detail = detail,
                Instance = http.Request.Path
            };

            // Ajuda muito a rastrear erros em logs/telemetria
            problem.Extensions["traceId"] = http.TraceIdentifier;
            return problem;
        }

        public static ProblemDetails Conflict(HttpContext http, string detail)
        {
            var problem = new ProblemDetails {
                Status = StatusCodes.Status409Conflict,
                Title = "Conflito de regra de negócio.",
                Type = "https://httpstatuses.com/409",
                Detail = detail,
                Instance = http.Request.Path
            };

            problem.Extensions["traceId"] = http.TraceIdentifier;
            return problem;
        }

        public static ProblemDetails BadRequest(HttpContext http, string detail)
        {
            var problem = new ProblemDetails {
                Status = StatusCodes.Status400BadRequest,
                Title = "Requisição inválida.",
                Type = "https://httpstatuses.com/400",
                Detail = detail,
                Instance = http.Request.Path
            };

            problem.Extensions["traceId"] = http.TraceIdentifier;
            return problem;
        }
    }
}
