namespace BarberRezende.Application.DTOs.Funcionarios
{
    /// <summary>
    /// DTO para retornar dados de funcionários administrativos.
    /// </summary>
    public class FuncionariosDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }

        /// <summary>Cargo (atendente, gerente, auxiliar, etc).</summary>
        public string Cargo { get; set; }
    }
}
