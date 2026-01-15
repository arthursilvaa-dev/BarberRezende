namespace BarberRezende.Application.DTOs.Servicos
{
    /// <summary>
    /// DTO para criação de novos serviços.
    /// </summary>
    public class ServicosCreateDTO
    {
        public string NomeServico { get; set; }
        public decimal Preco { get; set; }
        public int DuracaoMinutos { get; set; }
    }
}
