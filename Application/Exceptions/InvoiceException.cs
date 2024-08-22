using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Exceptions
{
    public class InvoiceException : BaseException
    {
        public InvoiceException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message, statusCode)
        {
        }
    }
    public class InvoiceNotFound: BaseException
    {
        public InvoiceNotFound(int id) : base($"Không tìm thấy hóa đơn với ID : {id}.", HttpStatusCode.NotFound)
        {
        }
    }
}