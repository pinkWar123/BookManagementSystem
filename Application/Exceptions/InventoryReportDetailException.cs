using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Exceptions
{
    public class InventoryReportDetailException : BaseException
    {
        public InventoryReportDetailException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message, statusCode)
        {
        }
    }
    public class InventoryReportDetailNotFound : BaseException
    {
        public InventoryReportDetailNotFound(int reportId, int customerId) : base($"Không tìm thấy báo cáo chi tiết với ID : {reportId} và ID khách hàng là : {customerId}.", HttpStatusCode.NotFound)
        {
        }
    }
}