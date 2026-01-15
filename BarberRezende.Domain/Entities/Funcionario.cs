namespace BarberRezende.Domain.Entities
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public DateTime DataContratacao { get; set; }
        public bool Ativo { get; set; } = true;
    }
}
