using System.Collections.Generic;

namespace BarberRezende.Web.Models.Agendamentos
{
    /// <summary>
    /// ViewModel do Index:
    /// - Próximos (ordem crescente)
    /// - Histórico (últimos 4 meses, ordem decrescente)
    /// </summary>
    public class AgendamentosIndexVm // ViewModel para a página de listagem de agendamentos
    {
        public List<AgendamentoListItemVm> Proximos { get; set; } = new(); // Próximos agendamentos (ordem crescente)
        public List<AgendamentoListItemVm> HistoricoUltimos4Meses { get; set; } = new(); // Histórico dos últimos 4 meses (ordem decrescente)

        public string? ErrorMessage { get; set; } // Mensagem de erro amigável para exibir na tela, caso a API falhe
    }
}
