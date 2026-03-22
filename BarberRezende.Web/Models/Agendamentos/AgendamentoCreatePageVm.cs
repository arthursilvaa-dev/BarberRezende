using System.Collections.Generic;
using BarberRezende.Web.Models.Common;

namespace BarberRezende.Web.Models.Agendamentos
{
    /// <summary>
    /// ViewModel da página de criação de agendamento:
    /// - Form: dados do agendamento (o que o usuário preenche)
    /// - Clientes/Barbeiros/Servicos: listas para os dropdowns (Id/Text)
    /// - ErrorMessage: mensagem amigável caso a API falhe
    /// </summary>
    public class AgendamentoCreatePageVm
    {
        /// <summary>
        /// Dados do formulário (vamos usar UM modelo só: AgendamentoCreateVm).
        /// </summary>
        public AgendamentoCreateVm Form { get; set; } = new();

        /// <summary>
        /// Opções para dropdowns.
        /// </summary>
        public List<SimpleOptionVm> Clientes { get; set; } = new();
        public List<SimpleOptionVm> Barbeiros { get; set; } = new();
        public List<SimpleOptionVm> Servicos { get; set; } = new();

        /// <summary>
        /// Mensagem de erro amigável para exibir na tela.
        /// </summary>
        public string? ErrorMessage { get; set; }
    }
}
