using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Application.Queries
{
    public class InvoiceDetailQuery : QueryObject
    {
        public int? InvoiceID { get; set; }
        public int? BookID { get; set; }
        public int? Quantity { get; set; }
        public int? Price { get; set; }
    }
}
