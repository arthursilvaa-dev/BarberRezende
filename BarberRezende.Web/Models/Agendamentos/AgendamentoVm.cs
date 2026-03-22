using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberRezende.Web.Models.Agendamentos
{
    /// <summary>
    /// Representa um agendamento retornado da API.
    /// Ajuste os nomes se o seu DTO tiver campos diferentes.
    /// </summary>
    public class AgendamentoVm
    {
        public int Id { get; set; }
        public DateTime DataHora { get; set; }

        public int ClienteId { get; set; }
        public string ClienteNome { get; set; } = string.Empty;

        public int BarbeiroId { get; set; }
        public string BarbeiroNome { get; set; } = string.Empty;

        public int ServicoId { get; set; }
        public string ServicoNome { get; set; } = string.Empty;

        public decimal ServicoPreco { get; set; }
    }
}

