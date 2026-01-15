namespace BarberRezende.Application.DTOs.Clientes
{
    // DTO usado ao criar um cliente
    public class ClientesCreateDTO
    {
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
