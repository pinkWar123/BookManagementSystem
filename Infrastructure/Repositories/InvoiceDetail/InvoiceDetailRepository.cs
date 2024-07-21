using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.InvoiceDetail
{
    public class InvoiceDetailRepository : GenericRepository<Domain.Entities.InvoiceDetail>, IInvoiceDetailRepository
    {
        public InvoiceDetailRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }

}
