using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Application.Queries
{
    public class PaymentReceiptQuery : QueryObject
    {
        public int? Id { get; set; }
        public string? ReceiptDate { get; set; }
        public string? Amount { get; set; }
        public string? CustomerId { get; set; }
    }
}
