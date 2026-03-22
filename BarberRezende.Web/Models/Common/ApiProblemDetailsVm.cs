// BarberRezende.Web/Models/Common/ApiProblemDetailsVm.cs
namespace BarberRezende.Web.Models.Common
{
    // Modelo simplificado de ProblemDetails (RFC 7807).
    // A API pode devolver: title, status, detail, instance, etc.
    public class ApiProblemDetailsVm
    {
        public string? Title { get; set; }
        public int? Status { get; set; }
        public string? Detail { get; set; }
        public string? Instance { get; set; }
        public string? TraceId { get; set; }
        public string? ExceptionType { get; set; }
    }
}
