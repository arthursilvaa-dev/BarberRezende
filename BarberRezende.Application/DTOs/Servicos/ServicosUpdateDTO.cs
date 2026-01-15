namespace BarberRezende.Application.DTOs.Servicos
{
    /// <summary>
    /// DTO para atualizar serviços existentes.
    /// </summary>
    public class ServicosUpdateDTO
    {
        public string NomeServico { get; set; }
        public decimal Preco { get; set; }
        public int DuracaoMinutos { get; set; }
    }
}
