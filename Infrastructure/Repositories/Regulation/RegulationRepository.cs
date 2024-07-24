using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.Regulation
{
    public class RegulationRepository : GenericRepository<Domain.Entities.Regulation>, IRegulationRepository
    {
        public RegulationRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
