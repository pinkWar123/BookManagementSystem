using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.DeptReport
{
    public class DeptReportRepository : GenericRepository<Domain.Entities.DeptReport>, IDeptReportRepository
    {
        public DeptReportRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }

}
