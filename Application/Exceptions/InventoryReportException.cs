using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Exceptions
{
    public class InventoryReportException : BaseException
    {
        public InventoryReportException(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message, statusCode)
        {
        }
    }
    public class InventoryReportNotFound : BaseException
    {
        public InventoryReportNotFound(int id) : base($"Không tìm thấy báo cáo với ID : {id}.", HttpStatusCode.NotFound)
        {
        }
        public InventoryReportNotFound(int month,int year) : base($"Không tìm thấy báo cáo với tháng {month} năm {year}.", HttpStatusCode.NotFound)
        {
        }
    }
}