namespace BarberRezende.Application.DTOs.Barbeiros
{
    /// <summary>
    /// DTO de retorno para barbeiros.
    /// </summary>
    public class BarbeirosDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        /// <summary>Especialidade (degradê, navalhado, barba, etc).</summary>
        public string Especialidade { get; set; }
    }
}
