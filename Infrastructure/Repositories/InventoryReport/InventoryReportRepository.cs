
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
        var report = await _context.InventoryReports
            .Where(r => r.ReportMonth == month && r.ReportYear == year)
            .FirstOrDefaultAsync();

        if (report == null)
        {
            throw new Exception("Không tìm thấy InventoryReport cho tháng và năm này.");
        }

        return report.Id;
    }
    }

}
