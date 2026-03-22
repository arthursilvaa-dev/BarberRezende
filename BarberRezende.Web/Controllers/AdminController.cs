using Microsoft.AspNetCore.Mvc;
using BarberRezende.Web.Services;

namespace BarberRezende.Web.Controllers
{
    /// <summary>
    /// Dashboard inicial do Admin.
    /// Só navegação para os módulos (Clientes, Barbeiros, Serviços, Agendamentos).
    /// </summary>


    [ServiceFilter(typeof(AuthFilter))]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
