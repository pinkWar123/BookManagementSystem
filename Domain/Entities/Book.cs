using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Book : Base
    {
        [StringLength(100)]
        public required string title { get; set; }

        [StringLength(100)]
        public required string genre { get; set; }

        [StringLength(100)]
        public required string author { get; set; }

        public required int stockQuantity { get; set; }
        public required int price { get; set; }

        public ICollection<BookEntryDetail>? BookEntryDetails { get; set; }

        public ICollection<InventoryReportDetail>? InventoryReportDetails { get; set; }
        public ICollection<InvoiceDetail>? invoiceDetails { get; set; }
    }


}