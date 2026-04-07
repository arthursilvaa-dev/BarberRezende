namespace BarberRezende.Application.DTOs.Auth
{
    public class ChangePasswordDto
    {
        public string Email { get; set; }
        public string SenhaAtual { get; set; }
        public string NovaSenha { get; set; }
    }
}