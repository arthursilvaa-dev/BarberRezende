using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberRezende.Web.Models.Agendamentos
{
    public class AgendamentoIndexRowVm
    {
        public DateTime DataHora { get; set; }

        public string Cliente { get; set; } = "";
        public string Barbeiro { get; set; } = "";
        public string Servico { get; set; } = "";

        public decimal Preco { get; set; }
    }
}

