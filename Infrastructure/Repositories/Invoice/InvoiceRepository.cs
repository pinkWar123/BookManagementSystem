
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;
using BookManagementSystem.Infrastructure.Repositories.Invoice;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Infrastructure.Repositories.Book
{
    public class InvoiceRepository : GenericRepository<Domain.Entities.Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
            
        }
        public async Task<List<int>> GetInvoiceIdByMonthYearAsync(int month, int year)
      
        {
            return await _context.Invoices
                .Where(i => i.Date.Month == month && i.Date.Year == year)
                .Select(i => i.Id)
                .ToListAsync();
        }
    }
}
