using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Application.Queries
{
    public class InvoiceQuery : QueryObject
    {
        public int? Id { get; set; }
        public required DateOnly InvoiceDate { get; set; }
        public int? CustomerId { get; set; }
    }
}
