
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.InventoryReport
{
    public class InventoryReportRepository : GenericRepository<Domain.Entities.InventoryReport>, IInventoryReportRepository
    {
        public InventoryReportRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }

}
