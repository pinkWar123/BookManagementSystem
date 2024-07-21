

namespace BookManagementSystem.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Book : Base
    {
        [StringLength(100)]
        public required string Title { get; set; }

        [StringLength(100)]
        public required string Genre { get; set; }

        [StringLength(100)]
        public required string Author { get; set; }

        public required int StockQuantity { get; set; }
        public required int Price { get; set; }

        public ICollection<BookEntryDetail>? BookEntryDetails { get; set; }

        public ICollection<InventoryReportDetail>? InventoryReportDetails { get; set; }
        public ICollection<InvoiceDetail>? invoiceDetails { get; set; }
    }
}
