using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;
using BookManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BookManagementSystem.Infrastructure.Repositories.DebtReportDetail
{
    public class DebtReportDetailRepository : GenericRepository<Domain.Entities.DebtReportDetail>, IDebtReportDetailRepository
    {
        public DebtReportDetailRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }

        public async Task<Domain.Entities.DebtReportDetail?> GetByIdAsync(int reportId, int customerId)
        {
            return await _context.Set<Domain.Entities.DebtReportDetail>()
                // .FirstOrDefaultAsync(d => d.ReportID == reportId && d.CustomerID == customerId);
                .FindAsync(reportId, customerId);
        }

        public async Task<Domain.Entities.DebtReportDetail?> UpdateAsync<TUpdateDto>(int reportId, int customerId, TUpdateDto entity) where TUpdateDto : class
        {
            var existingEntity = await GetByIdAsync(reportId, customerId);
            if (existingEntity == null)
                return null;

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            return existingEntity;
        }
        public override IQueryable<Domain.Entities.DebtReportDetail>? GetValuesByQuery(QueryObject queryObject)
        {
            var items = _context.Set<Domain.Entities.DebtReportDetail>().AsQueryable();

            var reportIdProperty = queryObject.GetType().GetProperty("ReportId");
            if (reportIdProperty != null)
            {
                var reportIdValue = reportIdProperty.GetValue(queryObject);
                if (reportIdValue != null && reportIdValue is int reportId)
                {
                    items = items.Where(d => d.ReportID == reportId);
                }
            }


            if (!string.IsNullOrEmpty(queryObject.SortBy))
            {
                // Include the related DebtReport entity for sorting
                items = items.Include(d => d.DebtReport);

                var sortByFields = queryObject.SortBy.Split(',');
                var isDescending = queryObject.IsDescending;

                IOrderedQueryable<Domain.Entities.DebtReportDetail> orderedItems = null;

                for (int i = 0; i < sortByFields.Length; i++)
                {
                    var field = sortByFields[i];

                    // Try getting the property from DebtReport first
                    var sortByProperty = typeof(Domain.Entities.DebtReport).GetProperty(field); 

                    if (sortByProperty != null) 
                    { 
                        // Property belongs to DebtReport
                        if (i == 0)
                        {
                            orderedItems = isDescending
                                ? items.OrderByDescending(e => EF.Property<object>(e.DebtReport, field))
                                : items.OrderBy(e => EF.Property<object>(e.DebtReport, field));
                        }
                        else
                        {
                            orderedItems = isDescending
                                ? orderedItems.ThenByDescending(e => EF.Property<object>(e.DebtReport, field))
                                : orderedItems.ThenBy(e => EF.Property<object>(e.DebtReport, field));
                        }
                    }
                    else 
                    {
                        // If not found in DebtReport, try DebtReportDetail 
                        sortByProperty = typeof(Domain.Entities.DebtReportDetail).GetProperty(field);

                        if (sortByProperty != null)
                        {
                            // Property belongs to DebtReportDetail
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
                    }
                }

                if (orderedItems != null)
                    items = orderedItems;
            }

            return items;
        }
        
    }
}