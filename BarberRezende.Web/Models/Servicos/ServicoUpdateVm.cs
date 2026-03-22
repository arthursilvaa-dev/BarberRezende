using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BarberRezende.Web.Models.Servicos
{
    /// <summary>
    /// ViewModel para ATUALIZAR um serviço existente.
    /// Usado em:
    /// - PUT /api/Servicos/{id}
    /// 
    /// Importante:
    /// - Inclui Id porque a tela de edição geralmente precisa dele.
    /// - NomeServico padronizado.
    /// </summary>
    public class ServicoUpdateVm // ViewModel para atualizar um serviço existente
    {
        [Required]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Informe o nome do serviço.")]
        [StringLength(80, ErrorMessage = "O nome deve ter no máximo 80 caracteres.")]
        [JsonPropertyName("nomeServico")]
        public string? NomeServico { get; set; }

        [Range(0.01, 9999.99, ErrorMessage = "O preço deve ser maior que 0.")]
        [JsonPropertyName("preco")]
        public decimal Preco { get; set; }

        [Range(1, 600, ErrorMessage = "A duração deve ser maior que 0.")]
        [JsonPropertyName("duracaoMinutos")]
        public int DuracaoMinutos { get; set; }
    }
}
