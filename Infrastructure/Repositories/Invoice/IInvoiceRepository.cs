using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Infrastructure.Repositories.Invoice
{
    public interface IInvoiceRepository : IGenericRepository<Domain.Entities.Invoice>
    {
        Task<List<int>> GetInvoiceIdByMonthYearAsync(int month, int year);

    }
}
