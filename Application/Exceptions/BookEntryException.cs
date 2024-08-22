using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Exceptions
{
    public class BookEntryException : BaseException
    {
        public BookEntryException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message, statusCode)
        {
        }
    }
    public class BookEntryNotFound : BaseException
    {
        public BookEntryNotFound(int EntryID) : base($"Không tìm thấy phiếu lập sách với ID {EntryID}.", HttpStatusCode.NotFound)
        {
        }
    }   
}