using Microsoft.AspNetCore.Mvc;
using BarberRezende.Web.Models.Clientes;
using BarberRezende.Web.Services;

namespace BarberRezende.Web.Controllers
{
    [ServiceFilter(typeof(AuthFilter))]
    public class ClientesController : Controller
    {
        private readonly ApiClient _api;

        public ClientesController(ApiClient api)
        {
            _api = api;
        }

        // GET: /Clientes?q=joao
        public async Task<IActionResult> Index(string? q)
        {
            var res = await _api.GetClientesAsync();
            if (!res.Success) {
                ViewBag.ErrorMessage = res.FriendlyMessage;
                return View(new List<ClienteVm>());
            }

            var data = res.Data ?? new List<ClienteVm>();

            // Busca simples (nome/email/telefone)
            if (!string.IsNullOrWhiteSpace(q)) {
                q = q.Trim();
                data = data.Where(c =>
                        (c.Nome ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) ||
                        (c.Email ?? "").Contains(q, StringComparison.OrdinalIgnoreCase) ||
                        (c.Telefone ?? "").Contains(q, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            // Ordem alfabética por Nome
            data = data
                .OrderBy(c => c.Nome ?? "")
                .ToList();

            ViewBag.Query = q;
            return View(data);
        }

        // GET: /Clientes/Create
        public IActionResult Create()
        {
            return View(new ClienteCreateVm());
        }

        // POST: /Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteCreateVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var res = await _api.CreateClienteAsync(vm);
            if (!res.Success) {
                ModelState.AddModelError("", res.FriendlyMessage ?? "Erro ao criar cliente.");
                return View(vm);
            }

            TempData["Success"] = "Cliente criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Clientes/Edit/1
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _api.GetClienteByIdAsync(id);
            if (!res.Success || res.Data == null) {
                TempData["Error"] = res.FriendlyMessage ?? "Cliente não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            // Importante: o Id vai no MODEL (nada de ViewBag)
            var vm = new ClienteUpdateVm {
                Id = res.Data.Id,
                Nome = res.Data.Nome,
                Telefone = res.Data.Telefone,
                Email = res.Data.Email
            };

            return View(vm);
        }

        // POST: /Clientes/Edit/1
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClienteUpdateVm vm)
        {
            if (id != vm.Id)
                return BadRequest("Id inconsistente.");

            if (!ModelState.IsValid)
                return View(vm);

            var res = await _api.UpdateClienteAsync(id, vm);
            if (!res.Success) {
                ModelState.AddModelError("", res.FriendlyMessage ?? "Erro ao atualizar cliente.");
                return View(vm);
            }

            TempData["Success"] = "Cliente atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // GET: /Clientes/Delete/3  (tela de confirmação)
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _api.GetClienteByIdAsync(id);
            if (!res.Success || res.Data == null) {
                TempData["Error"] = res.FriendlyMessage ?? "Cliente não encontrado.";
                return RedirectToAction(nameof(Index));
            }

            return View(res.Data);
        }

        // POST: /Clientes/Delete/3  (executa)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, string? confirm = null)
        {
            // confirm existe só pra evitar “POST acidental”
            var res = await _api.DeleteClienteAsync(id);
            if (!res.Success) {
                TempData["Error"] = res.FriendlyMessage ?? "Erro ao excluir cliente.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Success"] =
                "Cliente excluído com sucesso. Agendamentos vinculados a este cliente também podem ter sido removidos pela API.";

            return RedirectToAction(nameof(Index));
        }
    }
}
