using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Application.Dtos.DebtReportDetail;
using BookManagementSystem.Application.Queries;


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

        // public async Task<IEnumerable<AllDebtReportDetailDto>> GetDebtReportDetailsByReportIdAsync(int reportId)
        // {
        //     var query = from debtReport in _context.DebtReports
        //                 join debtReportDetail in _context.DebtReportDetails on debtReport.Id equals debtReportDetail.ReportID
        //                 join customer in _context.Customers on debtReportDetail.CustomerID equals customer.Id
        //                 where debtReport.Id == reportId
        //                 select new AllDebtReportDetailDto
        //                 {
        //                     ReportID = debtReport.Id,
        //                     CustomerID = customer.Id,
        //                     customerName = customer.CustomerName,
        //                     InitialDebt = debtReportDetail.InitialDebt,
        //                     FinalDebt = debtReportDetail.FinalDebt,
        //                     AdditionalDebt = debtReportDetail.AdditionalDebt
        //                 };

        //     return await query.ToListAsync();
        // }

        public async Task<IEnumerable<AllDebtReportDetailDto>> GetDebtReportDetailsByReportIdAsync(int reportId, DebtReportQuery debtReportQuery)
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

            // Apply sorting
            if (!string.IsNullOrEmpty(debtReportQuery.SortBy))
            {
                query = debtReportQuery.IsDescending
                    ? query.OrderByDescending(r => EF.Property<object>(r, debtReportQuery.SortBy))
                    : query.OrderBy(r => EF.Property<object>(r, debtReportQuery.SortBy));
            }

            // Apply pagination
            query = query
                .Skip((debtReportQuery.PageNumber - 1) * debtReportQuery.PageSize)
                .Take(debtReportQuery.PageSize);

            return await query.ToListAsync();
        }


        public override IQueryable<Domain.Entities.DebtReport>? GetValuesByQuery(QueryObject queryObject)
        {
            var items = _context.Set<Domain.Entities.DebtReport>().AsQueryable();

            // Lặp qua từng thuộc tính trong QueryObject
            foreach (var property in queryObject.GetType().GetProperties())
            {
                var value = property.GetValue(queryObject);

                // Kiểm tra nếu thuộc tính có giá trị và không nằm trong danh sách loại trừ
                if (value != null && !QueryObject.GetFilterExcludes().Contains(property.Name))
                {
                    var propertyName = property.Name;
                    var propertyType = property.PropertyType;

                    if (propertyType == typeof(int) || propertyType == typeof(int?))
                    {
                        items = items.Where(e => EF.Property<int?>(e, propertyName) == (int?)value);
                    }
                    else if (propertyType == typeof(string))
                    {
                        // Lọc cho các thuộc tính kiểu string
                        var stringValue = value as string;
                        items = items.Where(e => EF.Property<string>(e, propertyName) == stringValue);
                    }
                    else if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                    {
                        items = items.Where(e => EF.Property<DateTime?>(e, propertyName) == (DateTime?)value);
                    }
                }
            }
            if (!string.IsNullOrEmpty(queryObject.SortBy))
            {
                var sortByFields = queryObject.SortBy.Split(',');
                var isDescending = queryObject.IsDescending;

                IOrderedQueryable<Domain.Entities.DebtReport> orderedItems = null;

                for (int i = 0; i < sortByFields.Length; i++)
                {
                    var field = sortByFields[i];
                    var sortByProperty = typeof(Domain.Entities.DebtReport).GetProperty(field);

                    if (sortByProperty == null) continue;

                    if (i == 0)
                    {
                        orderedItems = isDescending
                        ? items.OrderByDescending(e => EF.Property<object>(e, field))
                        : items.OrderBy(e => EF.Property<object>(e, field));
                    }
                    else
                    {
                        orderedItems = isDescending
                        ? orderedItems.ThenByDescending(e => EF.Property<object>(e, field))
                        : orderedItems.ThenBy(e => EF.Property<object>(e, field));
                    }
                }

                if (orderedItems != null)
                    items = orderedItems;
            }

            return items;
        }


    }

}
