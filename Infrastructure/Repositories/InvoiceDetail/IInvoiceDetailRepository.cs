using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.InvoiceDetail;
namespace BookManagementSystem.Infrastructure.Repositories.InvoiceDetail
{
    public interface IInvoiceDetailRepository : IGenericRepository<Domain.Entities.InvoiceDetail>
    {
        Task<Domain.Entities.InvoiceDetail?> GetByIdAsync(int InvoiceID, int BookID);
        Task<Domain.Entities.InvoiceDetail?> UpdateAsync<TUpdateDto>(int InvoiceID,int BookID, TUpdateDto entity) where TUpdateDto : class;
        Task<List<Domain.Entities.InvoiceDetail>?> FindInvoiceDetailsByInvoiceIdAsync(int InvoiceID);
    }
}
