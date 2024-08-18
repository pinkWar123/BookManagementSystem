using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Exceptions
{
    public class BookException : BaseException
    {
        public BookException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message, statusCode)
        {}
    }

    public class BookNotFound : BaseException
    {
        public BookNotFound(int id) : base($"Không tìm thấy sách với ID {id}.", HttpStatusCode.NotFound)
        { }
    }    

    public class BookExisted : BaseException
    {
        public BookExisted(string? title, string? author, string? genre) : base($"Cuốn sách với tên : {title}, của tác giả : {author}, thể loại : {genre} đã tồn tại", HttpStatusCode.Ambiguous){}
    }
}