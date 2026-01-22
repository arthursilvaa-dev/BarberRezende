using System;

namespace BarberRezende.Domain.Exceptions
{
    /// <summary>
    /// Exceção usada quando um recurso não é encontrado.
    /// Ex: ClienteId não existe, BarbeiroId não existe, etc.
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
        }
    }
}
