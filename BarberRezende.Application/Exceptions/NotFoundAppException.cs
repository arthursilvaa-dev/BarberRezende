using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberRezende.Application.Exceptions
{
    /// <summary>
    /// Recurso não encontrado (404).
    /// </summary>
    public sealed class NotFoundAppException : AppException
    {
        public NotFoundAppException(string message)
            : base(message, statusCode: 404, errorCode: "not_found")
        {
        }
    }
}
