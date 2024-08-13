using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Exceptions
{
    public class PaymentReceiptException : BaseException
    {
        public PaymentReceiptException(string message, HttpStatusCode statusCode = HttpStatusCode.NotFound) : base(message, statusCode)
        {
        }
    }
    public class PaymentReceiptNotFound : BaseException
    {
        public PaymentReceiptNotFound(int id) : base($"Không tìm thấy hóa đơn với ID {id}.", HttpStatusCode.NotFound)
        {
        }
    }

    public class PaymentReceiptConflictRegulation : BaseException
    {
        public PaymentReceiptConflictRegulation(): base("Số tiền thu không vượt quá số tiền khách hàng đang nợ.", HttpStatusCode.Conflict)
        {
        }
    }
}