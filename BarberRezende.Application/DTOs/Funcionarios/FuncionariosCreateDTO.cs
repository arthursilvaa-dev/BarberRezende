namespace BarberRezende.Application.DTOs.Funcionarios
{
    /// <summary>
    /// DTO usado ao registrar um funcionário no sistema.
    /// </summary>
    public class FuncionariosCreateDTO
    {
        public string Nome { get; set; }
        public string Cargo { get; set; }
    }
}
