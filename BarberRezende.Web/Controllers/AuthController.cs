using BarberRezende.Web.Models.ViewModels;
using BarberRezende.Web.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace BarberRezende.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly TokenSessionService _tokenSession;

        public AuthController(
            IHttpClientFactory httpClientFactory,
            TokenSessionService tokenSession)
        {
            _httpClient = httpClientFactory.CreateClient("Api");
            _tokenSession = tokenSession;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            Console.WriteLine("POST LOGIN FOI CHAMADO");

            if (!ModelState.IsValid)
                return View(model);

            var loginData = new
            {
                email = model.Email,
                password = model.Password
            };

            Console.WriteLine("EMAIL: " + model.Email);
            Console.WriteLine("PASSWORD: " + model.Password);

            // ✅ SEM BARRA NA FRENTE
            var response = await _httpClient.PostAsJsonAsync(
                "api/auth/login",
                loginData
            );

            Console.WriteLine("STATUS API: " + response.StatusCode);

            var raw = await response.Content.ReadAsStringAsync();
            Console.WriteLine("RESPOSTA API: " + raw);

            if (!response.IsSuccessStatusCode) {
                model.ErrorMessage = "Email ou senha inválidos.";
                return View(model);
            }

            var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

            if (result == null || string.IsNullOrEmpty(result.Token)) {
                model.ErrorMessage = "Erro ao autenticar.";
                return View(model);
            }

            _tokenSession.SetToken(result.Token);

            Console.WriteLine("TOKEN SALVO COM SUCESSO");

            return RedirectToAction("Index", "Admin");
        }

        public IActionResult Logout()
        {
            _tokenSession.ClearToken();
            return RedirectToAction("Login");
        }
    }

    public class LoginResponse
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}