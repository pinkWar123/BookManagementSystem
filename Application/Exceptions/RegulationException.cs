using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Exceptions
{
    public class DebtExceedException : BaseException
    {
        public DebtExceedException() : base($"Số tiền nợ vượt quá cho phép.", HttpStatusCode.BadRequest)
        {
        }
    }
    public class ExceedMinimumInventoryAfterSelling : BaseException
    {
        public ExceedMinimumInventoryAfterSelling() : base($"Số lượng tồn kho còn lại quá ít.", HttpStatusCode.BadRequest)
        {
        }
    }

    public class ExceedMinimumBookEntry : BaseException
    {
        public ExceedMinimumBookEntry() : base($"Số lượng sách nhập vào không được nhỏ hơn số lượng quy định.", HttpStatusCode.BadRequest)
        {
        }
    }

    public class PaymentReceiptConflictRegulation : BaseException
    {
        public PaymentReceiptConflictRegulation(): base("Số tiền thu không vượt quá số tiền khách hàng đang nợ.", HttpStatusCode.BadRequest)
        {
        }
    }
}