using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Infrastructure.Repositories.InventoryReport
{
    public interface IInventoryReportRepository : IGenericRepository<Domain.Entities.InventoryReport>
    {
        Task<int> GetReportIdByMonthYearAsync(int month, int year);
        //object GetReportIdByMonthYearAsync(int? reportMonth, int? reportYear);
        IQueryable<Domain.Entities.InventoryReport>? GetValuesByQuery(QueryObject queryObject);
    }
}
