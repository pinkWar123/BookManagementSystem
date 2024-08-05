using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Infrastructure.Repositories.DebtReport
{
    public class DebtReportRepository : GenericRepository<Domain.Entities.DebtReport>, IDebtReportRepository
    {
        public DebtReportRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
        public async Task<int > GetReportIdByMonthYearAsync(int month, int year)
        {
            return await _context.DebtReports
                .Where(dr => dr.ReportMonth == month && dr.ReportYear == year)
                .Select(dr => dr.Id)
                .FirstOrDefaultAsync();
        }
    }

}
