using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.PaymentReceipt
{
    public class CreatePaymentReceiptDto
    {
        public string? ReceiptDate { get; set; }
        public int? Amount { get; set; }
        public int? CustomerID { get; set; }
    }
}
