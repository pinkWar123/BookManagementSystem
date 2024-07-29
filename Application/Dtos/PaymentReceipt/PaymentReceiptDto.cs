using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.PaymentReceipt
{
    public class PaymentReceiptDto
    {
        public required string ReceiptID { get; set; }
        public required DateTime ReceiptDate { get; set; }
    public required int Amount { get; set; }
    public required string CustomerID { get; set; }
}
}
