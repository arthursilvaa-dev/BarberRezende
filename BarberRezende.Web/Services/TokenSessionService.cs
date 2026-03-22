using Microsoft.AspNetCore.Http;

namespace BarberRezende.Web.Services
{
    public class TokenSessionService
    {
        private const string SESSION_TOKEN = "JWT_TOKEN";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenSessionService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext!.Session.SetString(SESSION_TOKEN, token);
        }

        public string? GetToken()
        {
            return _httpContextAccessor.HttpContext!.Session.GetString(SESSION_TOKEN);
        }

        public void ClearToken()
        {
            _httpContextAccessor.HttpContext!.Session.Remove(SESSION_TOKEN);
        }

        public bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(GetToken());
        }
    }
}