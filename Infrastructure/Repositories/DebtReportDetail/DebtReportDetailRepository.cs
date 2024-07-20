using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.DeptReportDetail
{
    public class DeptReportDetailRepository : GenericRepository<Domain.Entities.DeptReportDetail>, IDeptReportDetailRepository
    {
        public DeptReportDetailRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
    
}