using Microsoft.AspNetCore.Mvc;
using BarberRezende.Web.Models.Configuracoes;
using BarberRezende.Web.Services;

namespace BarberRezende.Web.Controllers
{
    public class ConfiguracoesController : Controller
    {
        private readonly ApiClient _api;

        // Injetando o ApiClient
        public ConfiguracoesController(ApiClient api)
        {
            _api = api;
        }

        public IActionResult Index()
        {
            return View(new MudarSenhaVm());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MudarSenha(MudarSenhaVm vm)
        {
            if (!ModelState.IsValid) {
                TempData["Error"] = "Verifique os dados digitados.";
                return View("Index", vm);
            }

            // Pegamos o e-mail do usuário logado (Se você configurou Claims no login Web, pode usar User.Identity.Name)
            // Aqui usamos o e-mail padrão do Admin para fins de completude do MVP.
            string emailAdmin = "admin@barberrezende.com";

            var res = await _api.ChangePasswordAsync(emailAdmin, vm.SenhaAtual, vm.NovaSenha);

            if (res.Success) {
                TempData["Success"] = "Sua senha foi alterada com segurança!";
                return RedirectToAction("Index"); // Recarrega a página limpa
            }
            else {
                TempData["Error"] = res.FriendlyMessage;
                return View("Index", vm);
            }
        }
    }
}