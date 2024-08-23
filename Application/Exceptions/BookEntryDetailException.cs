using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Exceptions
{
    public class BookEntryDetailException : BaseException
    {
        public BookEntryDetailException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message, statusCode)
        {
        }
    }
    public class BookEntryDetailNotFound : BaseException
    {
        public BookEntryDetailNotFound(int EntryID,int BookID) : base($"Không tìm thấy chi tiết phiếu lập sách với ID phiếu là {EntryID} và ID sách là {BookID}.", HttpStatusCode.NotFound)
        {
        }
    } 
}