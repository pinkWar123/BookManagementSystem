using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Exceptions
{
    public class DebtReportException : BaseException
    {
        public DebtReportException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message, statusCode)
        {
        }
    }
    public class DebtReportNotFound : BaseException
    {
        public DebtReportNotFound(int id) : base($"Không tìm thấy báo cáo với ID {id}.", HttpStatusCode.NotFound)
        {
        }
    }
}