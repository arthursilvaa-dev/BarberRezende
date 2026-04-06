using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarberRezende.Web.Models.Agendamentos;
using BarberRezende.Web.Services; // <-- ajuste pro namespace do seu ApiClient
using Microsoft.AspNetCore.Mvc;
using BarberRezende.Web.Models.Barbeiros;
using BarberRezende.Web.Models.Clientes;
using BarberRezende.Web.Models.Servicos;
using BarberRezende.Web.Services;

namespace BarberRezende.Web.Controllers
{
    [ServiceFilter(typeof(AuthFilter))]
    public class AgendamentosController : Controller
    {
        private readonly ApiClient _api;

        public AgendamentosController(ApiClient api)
        {
            _api = api;
        }

        // =========================
        // INDEX (Próximos + Histórico 4 meses) com ENRIQUECIMENTO
        // =========================
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var vm = new AgendamentosIndexVm();

            // 1) Buscar agendamentos
            // Substitua o bloco que trata falha de agRes por algo temporário para diagnóstico:
            var agRes = await _api.GetAgendamentosAsync();
            if (!agRes.Success) {
                // DEBUG: expõe status e detalhe retornado pela API (remova depois)
                vm.ErrorMessage = agRes.FriendlyMessage
                    ?? $"Não foi possível validar conflitos de agenda. HTTP {(int)agRes.StatusCode} - Sem detalhes";
                return View(vm); // Corrigido: retorna View(vm) ao invés de código inválido
            }

            // 2) Buscar listas completas (para preencher nomes + preço)
            var cliRes = await _api.GetClientesAsync();
            var barRes = await _api.GetBarbeirosAsync();
            var serRes = await _api.GetServicosAsync();

            if (!cliRes.Success || !barRes.Success || !serRes.Success) {
                vm.ErrorMessage = "Não foi possível carregar dados auxiliares (clientes/barbeiros/serviços).";
                return View(vm);
            }

            var clientes = cliRes.Data ?? new List<Models.Clientes.ClienteVm>();
            var barbeiros = barRes.Data ?? new List<Models.Barbeiros.BarbeiroVm>();
            var servicos = serRes.Data ?? new List<Models.Servicos.ServicoVm>();

            // 3) Dicionários rápidos
            var clienteById = clientes.ToDictionary(x => x.Id, x => x.Nome ?? $"Cliente #{x.Id}");
            var barbeiroById = barbeiros.ToDictionary(x => x.Id, x => x.Nome ?? $"Barbeiro #{x.Id}");
            var servicoById = servicos.ToDictionary(x => x.Id, x => x);

            // 4) Enriquecer a lista
            var now = DateTime.Now;
            var fourMonthsAgo = now.AddMonths(-4);

            var all = agRes.Data ?? new List<AgendamentoListItemVm>();

            foreach (var a in all) {
                // =========================
                // CLIENTE
                // =========================
                if (a.ClienteId.HasValue && clienteById.TryGetValue(a.ClienteId.Value, out var cn))
                    a.ClienteNome = cn;
                else
                    a.ClienteNome = a.ClienteNomeSnapshot ?? "Cliente removido";

                // =========================
                // BARBEIRO
                // =========================
                if (a.BarbeiroId.HasValue && barbeiroById.TryGetValue(a.BarbeiroId.Value, out var bn))
                    a.BarbeiroNome = bn;
                else
                    a.BarbeiroNome = a.BarbeiroNomeSnapshot ?? "Barbeiro removido";

                // =========================
                // SERVIÇO
                // =========================
                if (a.ServicoId.HasValue && servicoById.TryGetValue(a.ServicoId.Value, out var s)) {
                    a.ServicoNome = s.NomeServico ?? $"Serviço #{s.Id}";
                    a.Preco = s.Preco;
                }
                else {
                    a.ServicoNome = a.ServicoNomeSnapshot ?? "Serviço removido";
                    a.Preco = a.PrecoSnapshot;
                }
            }
            // 5) Próximos: agora pra frente, ORDEM ASC (mais próximo primeiro)
            vm.Proximos = all
                .Where(x => x.DataHora >= now)
                .OrderBy(x => x.DataHora)
                .ToList();

            // 6) Histórico: apenas últimos 4 meses (passados), ordem DESC (mais recente primeiro)
            vm.HistoricoUltimos4Meses = all
                .Where(x => x.DataHora < now && x.DataHora >= fourMonthsAgo)
                .OrderByDescending(x => x.DataHora)
                .ToList();

            return View(vm);
        }

        // =========================
        // CREATE GET
        // =========================
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var page = new AgendamentoCreatePageVm();

            // listas para dropdown (ordenadas alfabeticamente)
            var cliRes = await _api.GetClientesAsync();
            var barRes = await _api.GetBarbeirosAsync();
            var serRes = await _api.GetServicosAsync();

            if (!cliRes.Success || !barRes.Success || !serRes.Success) {
                page.ErrorMessage = "Não foi possível carregar listas (clientes/barbeiros/serviços).";
                return View(page);
            }

            var clientes = (cliRes.Data ?? new List<Models.Clientes.ClienteVm>())
                .OrderBy(c => c.Nome)
                .Select(c => new Models.Common.SimpleOptionVm { Id = c.Id, Text = c.Nome ?? $"Cliente #{c.Id}" })
                .ToList();

            var barbeiros = (barRes.Data ?? new List<Models.Barbeiros.BarbeiroVm>())
                .OrderBy(b => b.Nome)
                .Select(b => new Models.Common.SimpleOptionVm { Id = b.Id, Text = b.Nome ?? $"Barbeiro #{b.Id}" })
                .ToList();

            var servicos = (serRes.Data ?? new List<Models.Servicos.ServicoVm>())
                 .OrderBy(s => s.NomeServico)
                 .Select(s => new Models.Common.SimpleOptionVm {
                     Id = s.Id,
                     Text = s.NomeServico ?? $"Serviço #{s.Id}",
                     DuracaoMinutos = s.DuracaoMinutos // Passando o valor para a tela!
                       })
                 .ToList();

            page.Clientes = clientes;
            page.Barbeiros = barbeiros;
            page.Servicos = servicos;

            // default: agora arredondado pra próxima “grade” de 15 min
            page.Form.DataHora = RoundUpTo15Minutes(DateTime.Now.AddMinutes(15));

            return View(page);
        }

        // =========================
        // CREATE POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AgendamentoCreatePageVm page)
        {
            // Recarregar dropdowns sempre que voltar com erro
            await FillDropdownsAsync(page);

            // 1) validação normal do MVC
            if (!ModelState.IsValid)
                return View(page);

            // 2) validar sem segundos
            var dt = page.Form.DataHora;

            if (dt.Second != 0 || dt.Millisecond != 0) {
                ModelState.AddModelError("Form.DataHora", "A hora enviada é inválida.");
                return View(page);
            }
            // 3) validar limite de data: de agora até 365 dias
            var now = DateTime.Now;
            if (dt < now) {
                ModelState.AddModelError("Form.DataHora", "A data/hora não pode ser no passado.");
                return View(page);
            }

            if (dt > now.AddDays(365)) {
                ModelState.AddModelError("Form.DataHora", "A data/hora não pode ultrapassar 365 dias à frente.");
                return View(page);
            }

            // 4) validar conflito por barbeiro (não sobrepor horários)
            // - precisamos da duração do serviço escolhido
            var servicosRes = await _api.GetServicosAsync();
            if (!servicosRes.Success) {
                page.ErrorMessage = "Não foi possível validar o serviço selecionado.";
                return View(page);
            }

            var servicos = servicosRes.Data ?? new List<Models.Servicos.ServicoVm>();
            var servico = servicos.FirstOrDefault(s => s.Id == page.Form.ServicoId!.Value);
            if (servico == null) {
                ModelState.AddModelError("Form.ServicoId", "Serviço inválido.");
                return View(page);
            }

            var duracaoMin = servico.DuracaoMinutos;
            if (duracaoMin <= 0) duracaoMin = 30; // fallback seguro

            // Buscar agendamentos e verificar conflito no mesmo barbeiro
            var agRes = await _api.GetAgendamentosAsync();
            if (!agRes.Success) {
                // mostrar mensagem amigável retornada pelo ApiClient (mais diagnósticos)
                page.ErrorMessage = agRes.FriendlyMessage ?? "Não foi possível validar conflitos de agenda.";
                return View(page);
            }

            var allAg = agRes.Data ?? new List<AgendamentoListItemVm>();

            // Para checar conflito, precisamos da duração dos agendamentos existentes também:
            // - vamos usar o serviço de cada agendamento existente
            var duracaoByServicoId = servicos.ToDictionary(s => s.Id, s => s.DuracaoMinutos);

            DateTime novoInicio = dt;
            DateTime novoFim = dt.AddMinutes(duracaoMin);

            var sameBarber = allAg
                .Where(a => a.BarbeiroId == page.Form.BarbeiroId!.Value)
                .ToList();

            foreach (var existing in sameBarber) {
                // duração do serviço do agendamento existente
                int durEx = 30;
                if (existing.ServicoId.HasValue && duracaoByServicoId.TryGetValue(existing.ServicoId.Value, out var d) && d > 0)
                    durEx = d;

                var exInicio = existing.DataHora;
                var exFim = existing.DataHora.AddMinutes(durEx);

                // overlap: novoInicio < exFim && exInicio < novoFim
                if (novoInicio < exFim && exInicio < novoFim) {
                    ModelState.AddModelError("Form.DataHora",
                        $"Conflito de agenda: o barbeiro já possui um atendimento entre {exInicio:dd/MM/yyyy HH:mm} e {exFim:HH:mm}.");
                    return View(page);
                }
            }

            // 5) Enviar para API (agora usando .Value porque são int?)
            var createVm = new AgendamentoCreateVm {
                DataHora = page.Form.DataHora,
                ClienteId = page.Form.ClienteId!.Value,
                BarbeiroId = page.Form.BarbeiroId!.Value,
                ServicoId = page.Form.ServicoId!.Value
            };


            var createRes = await _api.CreateAgendamentoAsync(createVm);
            if (!createRes.Success) {
                page.ErrorMessage = createRes.FriendlyMessage ?? "Não foi possível criar o agendamento.";
                return View(page);
            }

            TempData["Success"] = "Agendamento criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EDIT GET
        // =========================
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _api.GetAgendamentoByIdAsync(id);
            if (!res.Success || res.Data == null) {
                TempData["Error"] = "Agendamento não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            var page = new AgendamentoUpdatePageVm {
                Form = res.Data // Popula o formulário com os dados da API
            };

            // Reaproveita seu helper para preencher os dropdowns!
            await FillDropdownsAsync(page);
            return View(page);
        }

        // =========================
        // EDIT POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AgendamentoUpdatePageVm page)
        {
            await FillDropdownsAsync(page);

            if (!ModelState.IsValid) return View(page);

            var dt = page.Form.DataHora;

            if (dt.Second != 0 || dt.Millisecond != 0) {
                ModelState.AddModelError("Form.DataHora", "A hora enviada é inválida.");
                return View(page);
            }
            // Idealmente, a validação de conflito de agenda deve ficar na API (Domain/Application layer).
            // Como a API retornará um erro 409 Conflict se der problema, nós apenas repassamos a mensagem amigável:
            var updateRes = await _api.UpdateAgendamentoAsync(id, page.Form);

            if (!updateRes.Success) {
                page.ErrorMessage = updateRes.FriendlyMessage ?? "Conflito de horário ou erro ao atualizar.";
                return View(page);
            }

            TempData["Success"] = "Agendamento atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // CANCELAR (DELETE) POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _api.DeleteAgendamentoAsync(id);

            if (res.Success) {
                TempData["Success"] = "Agendamento cancelado e removido com sucesso!";
            }
            else {
                TempData["Error"] = res.FriendlyMessage ?? "Erro ao cancelar o agendamento.";
            }

            return RedirectToAction(nameof(Index));
        }



        // =========================
        // AJAX: CONSULTA DE HORÁRIOS OCUPADOS
        // =========================
        [HttpGet]
        public async Task<IActionResult> GetHorariosOcupados(int barbeiroId, string dataSelecionada)
        {
            if (!DateTime.TryParse(dataSelecionada, out var data))
                return Json(new List<object>()); // Retorna lista vazia se a data for inválida

            var agRes = await _api.GetAgendamentosAsync();
            var servRes = await _api.GetServicosAsync();

            if (!agRes.Success || !servRes.Success)
                return Json(new List<object>());

            var servicos = servRes.Data ?? new List<Models.Servicos.ServicoVm>();
            var agendamentos = agRes.Data ?? new List<Models.Agendamentos.AgendamentoListItemVm>();

            var duracaoByServicoId = servicos.ToDictionary(s => s.Id, s => s.DuracaoMinutos);

            // Filtra agendamentos apenas do barbeiro específico, no dia específico
            var ocupados = agendamentos
                .Where(a => a.BarbeiroId == barbeiroId && a.DataHora.Date == data.Date)
                .Select(a => {
                    // Descobre a duração daquele atendimento específico
                    int duracao = 30;
                    if (a.ServicoId.HasValue && duracaoByServicoId.TryGetValue(a.ServicoId.Value, out var d) && d > 0)
                        duracao = d;

                    return new
                    {
                        inicio = a.DataHora,
                        fim = a.DataHora.AddMinutes(duracao) // Fim exato daquele corte/barba
                    };
                })
                .ToList();

            return Json(ocupados); // Devolve para o JavaScript
        }




        // Helper adaptado para suportar tanto CreatePageVm quanto UpdatePageVm
        private async Task FillDropdownsAsync(dynamic page)
        {
            var cliRes = await _api.GetClientesAsync();
            var barRes = await _api.GetBarbeirosAsync();
            var serRes = await _api.GetServicosAsync();

            page.Clientes = (cliRes.Data ?? new List<Models.Clientes.ClienteVm>())
                .OrderBy(c => c.Nome)
                .Select(c => new Models.Common.SimpleOptionVm { Id = c.Id, Text = c.Nome ?? $"Cliente #{c.Id}" }).ToList();

            page.Barbeiros = (barRes.Data ?? new List<Models.Barbeiros.BarbeiroVm>())
                .OrderBy(b => b.Nome)
                .Select(b => new Models.Common.SimpleOptionVm { Id = b.Id, Text = b.Nome ?? $"Barbeiro #{b.Id}" }).ToList();

            page.Servicos = (serRes.Data ?? new List<Models.Servicos.ServicoVm>())
                .OrderBy(s => s.NomeServico)
                .Select(s => new Models.Common.SimpleOptionVm { Id = s.Id, Text = s.NomeServico ?? $"Serviço #{s.Id}" }).ToList();
        }


        // =========================
        // helpers
        // =========================
        private static DateTime RoundUpTo15Minutes(DateTime dt)
        {
            dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);
            int mod = dt.Minute % 15;
            if (mod == 0) return dt;
            return dt.AddMinutes(15 - mod);
        }

        private async Task FillDropdownsAsync(AgendamentoCreatePageVm page)
        {
            var cliRes = await _api.GetClientesAsync();
            var barRes = await _api.GetBarbeirosAsync();
            var serRes = await _api.GetServicosAsync();

            page.Clientes = (cliRes.Data ?? new List<Models.Clientes.ClienteVm>())
                .OrderBy(c => c.Nome)
                .Select(c => new Models.Common.SimpleOptionVm { Id = c.Id, Text = c.Nome ?? $"Cliente #{c.Id}" })
                .ToList();

            page.Barbeiros = (barRes.Data ?? new List<Models.Barbeiros.BarbeiroVm>())
                .OrderBy(b => b.Nome)
                .Select(b => new Models.Common.SimpleOptionVm { Id = b.Id, Text = b.Nome ?? $"Barbeiro #{b.Id}" })
                .ToList();

            page.Servicos = (serRes.Data ?? new List<Models.Servicos.ServicoVm>())
                .OrderBy(s => s.NomeServico)
                .Select(s => new Models.Common.SimpleOptionVm { Id = s.Id, Text = s.NomeServico ?? $"Serviço #{s.Id}" })
                .ToList();
        }
    }
}
