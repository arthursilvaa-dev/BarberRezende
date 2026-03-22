using System;

namespace BarberRezende.Application.Exceptions
{
    /// <summary>
    /// Exceção para regras de negócio (não é "erro do sistema").
    /// Ex: "horário conflita", "fora do horário comercial", "limite excedido".
    /// 
    /// Por padrão, vamos mapear isso para HTTP 409 (Conflict).
    /// </summary>
    public sealed class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message) { }
    }
}
