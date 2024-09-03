
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.DebtReportDetail
{
    public interface IDebtReportDetailRepository : IGenericRepository<Domain.Entities.DebtReportDetail>
    {
        Task<Domain.Entities.DebtReportDetail?> GetByIdAsync(int reportId, int customerId);
        Task<Domain.Entities.DebtReportDetail?> UpdateAsync<TUpdateDto>(int reportId, int customerId, TUpdateDto entity) where TUpdateDto : class;
        IQueryable<Domain.Entities.DebtReportDetail>? GetValuesByQuery(QueryObject queryObject);
    }
}
