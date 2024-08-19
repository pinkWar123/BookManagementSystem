using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Application.Dtos.DebtReportDetail;

namespace BookManagementSystem.Infrastructure.Repositories.DebtReport
{
    public class DebtReportRepository : GenericRepository<Domain.Entities.DebtReport>, IDebtReportRepository
    {
        public DebtReportRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
        public async Task<int> GetReportIdByMonthYearAsync(int month, int year)
        {
            return await _context.DebtReports
                .Where(dr => dr.ReportMonth == month && dr.ReportYear == year)
                .Select(dr => dr.Id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> DebtReportExists(int month, int year)
        {
            return await _context.DebtReports
                .AnyAsync(r => r.ReportMonth == month && r.ReportYear == year);
        }

        public async Task<IEnumerable<AllDebtReportDetailDto>> GetDebtReportDetailsByReportIdAsync(int reportId)
        {
            var query = from debtReport in _context.DebtReports
                        join debtReportDetail in _context.DebtReportDetails on debtReport.Id equals debtReportDetail.ReportID
                        join customer in _context.Customers on debtReportDetail.CustomerID equals customer.Id
                        where debtReport.Id == reportId
                        select new AllDebtReportDetailDto
                        {
                            ReportID = debtReport.Id,
                            CustomerID = customer.Id,
                            customerName = customer.CustomerName,
                            InitialDebt = debtReportDetail.InitialDebt,
                            FinalDebt = debtReportDetail.FinalDebt,
                            AdditionalDebt = debtReportDetail.AdditionalDebt
                        };

            return await query.ToListAsync();
        }
    }

}
