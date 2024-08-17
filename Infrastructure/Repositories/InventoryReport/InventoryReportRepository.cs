
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Infrastructure.Repositories.InventoryReport
{
    public class InventoryReportRepository : GenericRepository<Domain.Entities.InventoryReport>, IInventoryReportRepository
    {
        public InventoryReportRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
        public async Task<int> GetReportIdByMonthYearAsync(int month, int year)
        {
            return await _context.InventoryReports
                .Where(dr => dr.ReportMonth == month && dr.ReportYear == year)
                .Select(dr => dr.Id)
                .FirstOrDefaultAsync();
        }
        
    }

}
