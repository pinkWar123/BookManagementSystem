using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.DebtReport
{
    public class DebtReportRepository : GenericRepository<Domain.Entities.DebtReport>, IDebtReportRepository
    {
        public DebtReportRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }

}
