using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Exceptions
{
    public class PaymentReceiptException : BaseException
    {
        public PaymentReceiptException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message, statusCode)
        {
        }
    }
}