
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.InventoryReportDetail
{
    public class InventoryReportDetailRepository : GenericRepository<Domain.Entities.InventoryReportDetail>, IInventoryReportDetailRepository
    {
        public InventoryReportDetailRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }

}
