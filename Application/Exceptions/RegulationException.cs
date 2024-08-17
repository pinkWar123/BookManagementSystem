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
}