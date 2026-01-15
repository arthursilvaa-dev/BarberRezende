namespace BarberRezende.Application.DTOs.Servicos
{
    /// <summary>
    /// DTO para retornar serviços oferecidos (corte, barba, etc).
    /// </summary>
    public class ServicosDTO
    {
        public int Id { get; set; }
        public string NomeServico { get; set; }
        public decimal Preco { get; set; }

        /// <summary>Tempo médio do serviço em minutos.</summary>
        public int DuracaoMinutos { get; set; }
    }
}
