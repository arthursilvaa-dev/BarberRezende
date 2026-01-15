namespace BarberRezende.Application.DTOs.Clientes
{
    // DTO usado para leitura (retorno nas APIs)
    public class ClientesDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
