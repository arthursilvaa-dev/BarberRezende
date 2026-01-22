using System;

namespace BarberRezende.Domain.Exceptions
{
    /// <summary>
    /// Exceção base para regras de negócio do Domain.
    /// Tudo que for "o sistema não permite" por regra, pode virar DomainException.
    /// Ex: agendar 2 vezes no mesmo horário, serviço com preço inválido, etc.
    /// </summary>
    public class DomainException : Exception
    {
        public DomainException(string message) : base(message)
        {
        }
    }
}
