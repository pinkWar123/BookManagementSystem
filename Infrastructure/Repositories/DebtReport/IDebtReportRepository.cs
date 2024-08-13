using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.DebtReport
{
    public interface IDebtReportRepository : IGenericRepository<Domain.Entities.DebtReport>
    {
        Task<int> GetReportIdByMonthYearAsync(int month, int year);
    }
}
