using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarberRezende.Application.Exceptions
{
    /// <summary>
    /// Erros de validação/regra de negócio (400).
    /// </summary>
    public sealed class ValidationAppException : AppException
    {
        public ValidationAppException(string message)
            : base(message, statusCode: 400, errorCode: "validation_error")
        {
        }
    }
}
