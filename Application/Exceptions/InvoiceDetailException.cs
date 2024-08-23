using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Exceptions
{
    public class InvoiceDetailException : BaseException
    {
        public InvoiceDetailException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message, statusCode)
        {
        }
    }
    public class InvoiceDetailNotFound : BaseException
    {
        public InvoiceDetailNotFound(int InvoiceID,int BookID) : base($"Không tìm thấy chi tiết hóa đơn với ID {InvoiceID} và sách {BookID}", HttpStatusCode.NotFound)
        {
        }
        public InvoiceDetailNotFound(int InvoiceID) : base($"Không tìm thấy bất kì chi tiết hóa đơn với ID {InvoiceID}", HttpStatusCode.NotFound)
        {
        }
    } 
}