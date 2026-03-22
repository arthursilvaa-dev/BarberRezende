using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberRezende.Web.Models.Barbeiros
{
    /// <summary>
    /// ViewModel usado nas telas para representar um Barbeiro.
    /// </summary>
    public class BarbeiroVm
    {
        public int Id { get; set; }

        // Nome do barbeiro
        public string? Nome { get; set; } = ""; 

        // Especialidade (ex: degradê, barba, etc.)
        public string? Especialidade { get; set; }
    }
}
