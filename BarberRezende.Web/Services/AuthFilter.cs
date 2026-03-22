using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BarberRezende.Web.Services
{
    public class AuthFilter : IActionFilter
    {
        private readonly TokenSessionService _tokenSession;

        public AuthFilter(TokenSessionService tokenSession)
        {
            _tokenSession = tokenSession;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var token = _tokenSession.GetToken();

            Console.WriteLine("TOKEN NO FILTER: " + token);

            if (string.IsNullOrEmpty(token)) {
                Console.WriteLine("TOKEN NÃO ENCONTRADO → REDIRECT");

                context.Result = new RedirectToActionResult("Login", "Auth", null);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}