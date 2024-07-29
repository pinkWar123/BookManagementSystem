using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.DebtReportDetail
{
    public class DebtReportDetailRepository : GenericRepository<Domain.Entities.DebtReportDetail>, IDebtReportDetailRepository
    {
        public DebtReportDetailRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
