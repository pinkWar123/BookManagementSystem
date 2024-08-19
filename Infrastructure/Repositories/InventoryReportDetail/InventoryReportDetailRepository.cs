using BookManagementSystem.Application.Dtos.InventoryReportDetail;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Infrastructure.Repositories.InventoryReportDetail
{
    public class InventoryReportDetailRepository : GenericRepository<Domain.Entities.InventoryReportDetail>, IInventoryReportDetailRepository
    {
        public InventoryReportDetailRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        { }
        public async Task<Domain.Entities.InventoryReportDetail?> UpdateAsync<TUpdateDto>(int reportId, int bookId, TUpdateDto entity) where TUpdateDto : class
        {
            var existingEntity = await GetByIdAsync(reportId, bookId);
            if (existingEntity == null)
                return null;

            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            return existingEntity;
        }

        public async Task<Domain.Entities.InventoryReportDetail?> GetByIdAsync(int id, int id2)
        {
            return await _context.Set<Domain.Entities.InventoryReportDetail>()
                .FindAsync(id, id2);
        }

        public async Task<List<Domain.Entities.InventoryReportDetail>> GetListInventoryReportDetailsByIdAsync(int id)
        {
            return await _context.InventoryReportDetails
                                 .Where(o => o.ReportID == id)
                                 .ToListAsync();
        }
    }

}