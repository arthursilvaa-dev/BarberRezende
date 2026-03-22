using BarberRezende.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace BarberRezende.Web.Controllers
{
    /// <summary>
    /// Controller inicial do site.
    /// Aqui fica o "Dashboard" e a rota padrão (/).
    /// </summary>
    [ServiceFilter(typeof(AuthFilter))]
    public class HomeController : Controller
    {
        /// <summary>
        /// Página inicial do sistema.
        /// GET /
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }
    }
}
