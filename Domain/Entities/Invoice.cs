using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    public class Invoice
    {
        // Properties: InvoiceID, InvoiceDate, CustomerID
        public string? InvoiceID { get; set; }
        public string? InvoiceDate { get; set; }
        public string? CustomerID { get; set; }
    }
}