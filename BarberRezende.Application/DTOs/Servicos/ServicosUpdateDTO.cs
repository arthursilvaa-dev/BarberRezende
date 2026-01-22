using System.ComponentModel.DataAnnotations;

namespace BarberRezende.Application.DTOs.Servicos
{
    /// <summary>
    /// DTO usado para atualizar um Serviço.
    /// A validação segue o mesmo padrão do Create.
    /// </summary>
    public class ServicosUpdateDTO
    {
        [Required(ErrorMessage = "O nome do serviço é obrigatório.")]
        [StringLength(120, MinimumLength = 2, ErrorMessage = "O nome do serviço deve ter entre 2 e 120 caracteres.")]
        public string NomeServico { get; set; } = string.Empty;

        [Range(0.01, 9999.99, ErrorMessage = "O preço deve ser maior que zero.")]
        public decimal Preco { get; set; }

        [Range(1, 600, ErrorMessage = "A duração deve estar entre 1 e 600 minutos.")]
        public int DuracaoMinutos { get; set; }
    }
}
