using BarberRezende.Web.Models.Servicos;
using BarberRezende.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberRezende.Web.Controllers
{
    [ServiceFilter(typeof(AuthFilter))]
    /// <summary>
    /// Controller MVC responsável pela UI de Serviços.
    /// Ele consome a API via ApiClient.
    /// </summary>
    public class ServicosController : Controller
    {
        private readonly ApiClient _api;

        public ServicosController(ApiClient api)
        {
            _api = api;
        }

        /// <summary>
        /// Lista de serviços (GET /Servicos)
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var result = await _api.GetServicosAsync();

            // Se der erro, exibe uma lista vazia e mostra o alerta.
            if (!result.Success || result.Data is null) {
                ViewBag.Error = result.FriendlyMessage ?? "Falha ao carregar serviços.";
                return View(new List<ServicoVm>());
            }

            return View(result.Data);
        }

        /// <summary>
        /// Tela de criação (GET /Servicos/Create)
        /// </summary>
        public IActionResult Create()
            => View(new ServicoCreateVm());

        /// <summary>
        /// Envia criação para API (POST /Servicos/Create)
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ServicoCreateVm vm)
        {
            // Validação do MVC (DataAnnotations)
            if (!ModelState.IsValid)
                return View(vm);

            var result = await _api.CreateServicoAsync(vm);

            // Se a API rejeitar, mostra mensagem e mantém o formulário.
            if (!result.Success) {
                ViewBag.Error = result.FriendlyMessage ?? "Falha ao criar serviço.";
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Tela de edição (GET /Servicos/Edit/{id})
        /// </summary>
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _api.GetServicoByIdAsync(id);

            if (!result.Success || result.Data is null)
                return NotFound();

            // Mapeia da VM "de leitura" para a VM de update.
            // IMPORTANTE: agora é NomeServico (padronizado com Views e API).
            var vm = new ServicoUpdateVm {
                Id = result.Data.Id,
                NomeServico = result.Data.NomeServico,
                Preco = result.Data.Preco,
                DuracaoMinutos = result.Data.DuracaoMinutos
            };

            return View(vm);
        }

        /// <summary>
        /// Envia edição para API (POST /Servicos/Edit/{id})
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ServicoUpdateVm vm)
        {
            // Garante que a rota bate com o model (anti-bug)
            if (id != vm.Id)
                return BadRequest();

            if (!ModelState.IsValid)
                return View(vm);

            var result = await _api.UpdateServicoAsync(id, vm);

            if (!result.Success) {
                ViewBag.Error = result.FriendlyMessage ?? "Falha ao atualizar serviço.";
                return View(vm);
            }

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Tela de confirmação de exclusão (GET /Servicos/Delete/{id})
        /// </summary>
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _api.GetServicoByIdAsync(id);

            if (!result.Success || result.Data is null)
                return NotFound();

            return View(result.Data);
        }

        /// <summary>
        /// Confirma exclusão (POST /Servicos/DeleteConfirmed/{id})
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _api.DeleteServicoAsync(id);

            // Se falhar, volta pra tela de Delete com mensagem de erro.
            if (!result.Success) {
                TempData["Error"] = result.FriendlyMessage ?? "Falha ao remover serviço.";
                return RedirectToAction(nameof(Delete), new { id });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
