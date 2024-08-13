using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Exceptions
{
    public class DebtReportDetailException : BaseException
    {
        public DebtReportDetailException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message, statusCode)
        {
        }
    }
    public class DebtReportDetailNotFound : BaseException
    {
        public DebtReportDetailNotFound(int reportId, int customerId) : base($"Không tìm thấy báo cáo chi tiết với ID báo cáo là {reportId} và ID khách hàng là {customerId}.", HttpStatusCode.NotFound)
        {
        }
    }
}