using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;
using BookManagementSystem.Application.Dtos.DebtReportDetail;
using BookManagementSystem.Application.Queries;

namespace BookManagementSystem.Infrastructure.Repositories.DebtReport
{
    public interface IDebtReportRepository : IGenericRepository<Domain.Entities.DebtReport>
    {
        Task<int> GetReportIdByMonthYearAsync(int month, int year);
        Task<bool> DebtReportExists(int month, int year);
        Task<IEnumerable<AllDebtReportDetailDto>> GetDebtReportDetailsByReportIdAsync(int reportId, DebtReportQuery debtReportQuery);
        IQueryable<Domain.Entities.DebtReport>? GetValuesByQuery(QueryObject queryObject);
    }
}
