using System.Text.Json.Serialization;

namespace BarberRezende.Web.Models.Servicos
{
    /// <summary>
    /// ViewModel para EXIBIR os dados de um serviço.
    /// Usado em:
    /// - GET /api/Servicos  (listagem)
    /// - GET /api/Servicos/{id} (detalhe/edição)
    /// 
    /// Importante:
    /// - Padronizamos para "NomeServico", porque as Views já usam isso
    ///   e sua API/DTO também (pelo que apareceu nos warnings do projeto).
    /// </summary>
    public class ServicoVm
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        // A API normalmente envia "nomeServico" (camelCase)
        [JsonPropertyName("nomeServico")] // JsonPropertyName para mapear "nomeServico" da API para "NomeServico" do C#
        public string? NomeServico { get; set; }

        [JsonPropertyName("preco")]
        public decimal Preco { get; set; }

        [JsonPropertyName("duracaoMinutos")]
        public int DuracaoMinutos { get; set; }
    }
}
