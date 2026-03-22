using System;
using System.Text.Json.Serialization;

namespace BarberRezende.Web.Models.Agendamentos
{
    public class AgendamentoListItemVm
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("dataHora")]
        public DateTime DataHora { get; set; }

        [JsonPropertyName("clienteId")]
        public int? ClienteId { get; set; }

        [JsonPropertyName("barbeiroId")]
        public int? BarbeiroId { get; set; }

        [JsonPropertyName("servicoId")]
        public int? ServicoId { get; set; }

        // =========================
        // NOMES (usados no MVC)
        // =========================
        public string? ClienteNome { get; set; }
        public string? BarbeiroNome { get; set; }
        public string? ServicoNome { get; set; }

        public decimal Preco { get; set; }

        // =========================
        // 🔥 SNAPSHOTS (ESSENCIAL)
        // =========================
        [JsonPropertyName("clienteNomeSnapshot")]
        public string? ClienteNomeSnapshot { get; set; }

        [JsonPropertyName("barbeiroNomeSnapshot")]
        public string? BarbeiroNomeSnapshot { get; set; }

        [JsonPropertyName("servicoNomeSnapshot")]
        public string? ServicoNomeSnapshot { get; set; }

        [JsonPropertyName("precoSnapshot")]
        public decimal PrecoSnapshot { get; set; }
    }
}