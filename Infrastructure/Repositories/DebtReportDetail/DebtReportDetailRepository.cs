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
                .FirstOrDefaultAsync(d => d.ReportID == reportId && d.CustomerID == customerId);
        }

        public async Task<Domain.Entities.DebtReportDetail?> UpdateAsync<TUpdateDto>(int reportId, int customerId, TUpdateDto entity) where TUpdateDto : class
        {
            var existingEntity = await GetByIdAsync(reportId, customerId);
            if (existingEntity == null)
                return null;

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            return existingEntity;
        }
    }
}