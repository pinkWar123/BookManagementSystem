
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
                return -1;
            }

            return report.Id;
        }

        public override IQueryable<Domain.Entities.InventoryReport>? GetValuesByQuery(QueryObject queryObject)
        {
            var items = _context.Set<Domain.Entities.InventoryReport>().AsQueryable();

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

                IOrderedQueryable<Domain.Entities.InventoryReport> orderedItems = null;

                for (int i = 0; i < sortByFields.Length; i++)
                {
                    var field = sortByFields[i];
                    var sortByProperty = typeof(Domain.Entities.InventoryReport).GetProperty(field);

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
