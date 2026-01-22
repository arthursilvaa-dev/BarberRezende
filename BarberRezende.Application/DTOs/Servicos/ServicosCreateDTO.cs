using System.ComponentModel.DataAnnotations;

namespace BarberRezende.Application.DTOs.Servicos
{
    /// <summary>
    /// DTO usado para criar um Serviço.
    /// Regras simples de validação ficam aqui (não no Controller),
    /// porque o ASP.NET valida automaticamente quando usamos [ApiController].
    /// </summary>
    public class ServicosCreateDTO
    {
        /// <summary>
        /// Nome do serviço (ex: "Corte Social", "Barba", etc).
        /// </summary>
        [Required(ErrorMessage = "O nome do serviço é obrigatório.")]
        [StringLength(120, MinimumLength = 2, ErrorMessage = "O nome do serviço deve ter entre 2 e 120 caracteres.")]
        public string NomeServico { get; set; } = string.Empty;

        /// <summary>
        /// Preço do serviço.
        /// </summary>
        [Range(0.01, 9999.99, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

        /// <summary>
        /// Duração do serviço em minutos.
        /// </summary>
        [Range(1, 600, ErrorMessage = "A duração deve estar entre 1 e 600 minutos.")]
        public int DuracaoMinutos { get; set; }
    }
}
