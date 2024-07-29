using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Infrastructure.Repositories.InvoiceDetail
{
    public interface IInvoiceDetailRepository : IGenericRepository<Domain.Entities.InvoiceDetail>
    {
        Task<Domain.Entities.InvoiceDetail?> GetByIdAsync(int InvoiceID, int BookID);
        Task<Domain.Entities.InvoiceDetail?> UpdateAsync<TUpdateDto>(int InvoiceID,int BookID, TUpdateDto entity) where TUpdateDto : class;
    }
}
