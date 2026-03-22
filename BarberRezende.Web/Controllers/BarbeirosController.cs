using Microsoft.AspNetCore.Mvc;
using BarberRezende.Web.Services;


namespace BarberRezende.Web.Controllers
{
    [ServiceFilter(typeof(AuthFilter))]
    public class BarbeirosController : Controller
    {
        public IActionResult Index() => Content("Em construção: Barbeiros (Admin).");
    }
}
