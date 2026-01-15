namespace BarberRezende.Domain.Entities;


    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string? Email { get; set; }

        // Relacionamento
        public ICollection<Agendamento> Agendamentos { get; set; } = new List<Agendamento>();
    }

