using Microsoft.AspNetCore.Mvc;
using BarberRezende.Web.Services;
using BarberRezende.Web.Models.Admin;

namespace BarberRezende.Web.Controllers
{
    [ServiceFilter(typeof(AuthFilter))]
    public class AdminController : Controller
    {
        private readonly ApiClient _apiClient;

        // Injeção de dependência do ApiClient para comunicação com a API REST
        public AdminController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<IActionResult> Index()
        {
            var taskClientes = _apiClient.GetClientesAsync();
            var taskBarbeiros = _apiClient.GetBarbeirosAsync();
            var taskServicos = _apiClient.GetServicosAsync();
            var taskAgendamentos = _apiClient.GetAgendamentosAsync();

            await Task.WhenAll(taskClientes, taskBarbeiros, taskServicos, taskAgendamentos);

            // Resgata a lista de agendamentos de forma segura
            var agendamentos = taskAgendamentos.Result.Success && taskAgendamentos.Result.Data != null
                ? taskAgendamentos.Result.Data
                : new List<Models.Agendamentos.AgendamentoListItemVm>();

            // Lógica de datas para agrupar o faturamento
            var hoje = DateTime.Now;
            var mesAnterior = hoje.AddMonths(-1);
            var doisMesesAtras = hoje.AddMonths(-2);

            var vm = new AdminDashboardVm {
                TotalClientes = taskClientes.Result.Success ? (taskClientes.Result.Data?.Count ?? 0) : 0,
                TotalBarbeiros = taskBarbeiros.Result.Success ? (taskBarbeiros.Result.Data?.Count ?? 0) : 0,
                TotalServicos = taskServicos.Result.Success ? (taskServicos.Result.Data?.Count ?? 0) : 0,
                TotalAgendamentos = agendamentos.Count,

                // Nomes dos meses formatados (ex: "janeiro")
                NomeMesAtual = hoje.ToString("MMMM"),
                NomeMesAnterior = mesAnterior.ToString("MMMM"),
                NomeDoisMesesAtras = doisMesesAtras.ToString("MMMM"),

                // LINQ: Filtra por Ano e Mês, depois soma o PrecoSnapshot
                FaturamentoMesAtual = agendamentos
                    .Where(a => a.DataHora.Year == hoje.Year && a.DataHora.Month == hoje.Month)
                    .Sum(a => a.PrecoSnapshot),

                FaturamentoMesAnterior = agendamentos
                    .Where(a => a.DataHora.Year == mesAnterior.Year && a.DataHora.Month == mesAnterior.Month)
                    .Sum(a => a.PrecoSnapshot),

                FaturamentoDoisMesesAtras = agendamentos
                    .Where(a => a.DataHora.Year == doisMesesAtras.Year && a.DataHora.Month == doisMesesAtras.Month)
                    .Sum(a => a.PrecoSnapshot)
            };

            return View(vm);
        }
    }
}