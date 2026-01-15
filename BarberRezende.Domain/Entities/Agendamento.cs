namespace BarberRezende.Domain.Entities;


    public class Agendamento
    {
        public int Id { get; set; }

        public DateTime DataHora { get; set; }

        // Chaves estrangeiras
        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
    
        public int BarbeiroId { get; set; }
        public Barbeiro? Barbeiro { get; set; }

        public int ServicoId { get; set; }
        public Servico? Servico { get; set; }
    }

