using System.ComponentModel.DataAnnotations;

namespace BarberRezende.Web.Models.Servicos // ViewModels para Servicos
{
    public class ServicoCreateVm // ViewModel para CRIAR um serviço
    {
        [Required(ErrorMessage = "O nome do serviço é obrigatório.")] // Validação de campo obrigatório
        public string? NomeServico { get; set; }

        [Range(10, 100000, ErrorMessage = "O preço mínimo é R$ 10,00.")] // Validação de valor mínimo
        public decimal Preco { get; set; }

        [Range(1, 600, ErrorMessage = "A duração deve ser entre 1 e 600 minutos.")] // Validação de intervalo
        public int DuracaoMinutos { get; set; }
    }


}
