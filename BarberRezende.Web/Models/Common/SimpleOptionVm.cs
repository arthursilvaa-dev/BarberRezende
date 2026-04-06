using System.Text.Json.Serialization;

namespace BarberRezende.Web.Models.Common
{
    /// <summary>
    /// Item simples para dropdowns (Id + Texto).
    /// 
    /// Padrão do front:
    /// - Id: valor real (Value do option)
    /// - Text: o texto exibido (Text do option)
    /// 
    /// Observação:
    /// - JsonPropertyName ajuda caso a API envie "text" (camelCase).
    /// </summary>
    public class SimpleOptionVm
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;

        // Nova propriedade para o JavaScript saber o tempo do serviço
        public int DuracaoMinutos { get; set; }
    }
}
