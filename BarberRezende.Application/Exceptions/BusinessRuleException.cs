using System;

namespace BarberRezende.Application.Exceptions
{
    /// <summary>
    /// Exceção para representar violações de regra de negócio.
    /// Exemplo: "Não pode dois agendamentos no mesmo horário para o mesmo barbeiro".
    /// 
    /// Por que na Application?
    /// - Domain é o coração (regras), mas o "como comunicar isso para a API" é papel da Application.
    /// - O Service (Application) detecta / valida e lança a exceção.
    /// - A API traduz isso para HTTP 409 (Conflict).
    /// </summary>
    public class BusinessRuleException : Exception
    {
        public BusinessRuleException(string message) : base(message) { }
    }
}
