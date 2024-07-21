
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;
using BookManagementSystem.Infrastructure.Repositories.Invoice;

namespace BookManagementSystem.Infrastructure.Repositories.Book
{
    public class InvoiceRepository : GenericRepository<Domain.Entities.Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
