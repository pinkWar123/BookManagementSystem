using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Exceptions
{
    public class CustomerException : BaseException
    {
        public CustomerException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message, statusCode)
        {
        }
    }

    public class CustomerNotFound : BaseException
    {
        public CustomerNotFound(int id) : base($"Không tìm thấy khách hàng với ID {id}.", HttpStatusCode.NotFound)
        {
        }
    }

    public class CustomerAlreadyExists : BaseException
    {
        public CustomerAlreadyExists(string CustomerName, string PhoneNumber) : base($"Khách hàng {CustomerName} với số điện thoại {PhoneNumber} đã tồn tại.", HttpStatusCode.Ambiguous)
        {
        }
    }
}