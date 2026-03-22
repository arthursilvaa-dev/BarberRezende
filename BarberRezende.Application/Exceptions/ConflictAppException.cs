using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberRezende.Application.Exceptions
{
    /// <summary>
    /// Conflito de estado (409).
    /// Ex: barbeiro já possui agendamento naquele horário.
    /// </summary>
    public sealed class ConflictAppException : AppException
    {
        public ConflictAppException(string message)
            : base(message, statusCode: 409, errorCode: "conflict")
        {
        }
    }
}
