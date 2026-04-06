namespace BarberRezende.Web.Models.Admin
{
    public class AdminDashboardVm
    {
        // Métricas de quantidade
        public int TotalClientes { get; set; }
        public int TotalBarbeiros { get; set; }
        public int TotalServicos { get; set; }
        public int TotalAgendamentos { get; set; }

        // Métricas financeiras (Faturamento)
        public decimal FaturamentoMesAtual { get; set; }
        public decimal FaturamentoMesAnterior { get; set; }
        public decimal FaturamentoDoisMesesAtras { get; set; }

        // Nomes dos meses para exibição na tela
        public string? NomeMesAtual { get; set; }
        public string? NomeMesAnterior { get; set; }
        public string? NomeDoisMesesAtras { get; set; }
    }
}