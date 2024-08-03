using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.PaymentReceipt
{
    public class PaymentReceiptDto
    {
        public required int Id { get; set; }
        public required DateOnly ReceiptDate { get; set; }
        public required int Amount { get; set; }
        public required int CustomerID { get; set; }
    }
}
