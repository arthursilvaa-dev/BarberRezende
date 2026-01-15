namespace BarberRezende.Application.DTOs.Clientes
{
    // DTO usado ao atualizar um cliente existente
    public class ClientesUpdateDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
