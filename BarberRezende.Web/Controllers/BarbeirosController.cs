using Microsoft.AspNetCore.Mvc;
using BarberRezende.Web.Services;
using BarberRezende.Web.Models.Barbeiros;

namespace BarberRezende.Web.Controllers
{
    [ServiceFilter(typeof(AuthFilter))]
    public class BarbeirosController : Controller
    {
        private readonly ApiClient _api;

        public BarbeirosController(ApiClient api)
        {
            _api = api;
        }

        // 1. LISTAR
        public async Task<IActionResult> Index()
        {
            var res = await _api.GetBarbeirosAsync();
            if (!res.Success) {
                TempData["Error"] = res.FriendlyMessage;
                return View(new List<BarbeiroVm>());
            }
            return View(res.Data);
        }

        // 2. CRIAR (GET e POST)
        [HttpGet]
        public IActionResult Create() => View(new BarbeiroVm());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BarbeiroVm model)
        {
            if (!ModelState.IsValid) return View(model);

            var res = await _api.CreateBarbeiroAsync(model);
            if (!res.Success) {
                ModelState.AddModelError(string.Empty, res.FriendlyMessage ?? "Erro ao cadastrar barbeiro.");
                return View(model);
            }

            TempData["Success"] = "Barbeiro cadastrado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // 3. EDITAR (GET e POST)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var res = await _api.GetBarbeiroByIdAsync(id);
            if (!res.Success || res.Data == null) {
                TempData["Error"] = "Barbeiro não encontrado.";
                return RedirectToAction(nameof(Index));
            }
            return View(res.Data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BarbeiroVm model)
        {
            if (!ModelState.IsValid) return View(model);

            var res = await _api.UpdateBarbeiroAsync(id, model);
            if (!res.Success) {
                ModelState.AddModelError(string.Empty, res.FriendlyMessage ?? "Erro ao atualizar barbeiro.");
                return View(model);
            }

            TempData["Success"] = "Barbeiro atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        // 4. DELETAR (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var res = await _api.DeleteBarbeiroAsync(id);
            if (res.Success) {
                TempData["Success"] = "Barbeiro removido com sucesso!";
            }
            else {
                TempData["Error"] = res.FriendlyMessage ?? "Erro ao remover. Verifique se ele possui agendamentos.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}