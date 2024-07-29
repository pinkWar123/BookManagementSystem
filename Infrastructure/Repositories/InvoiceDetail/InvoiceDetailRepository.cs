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
        public async Task<Domain.Entities.InvoiceDetail?> GetByIdAsync(int InvoiceID, int BookID)
        {
            return await _context.Set<Domain.Entities.InvoiceDetail>().FindAsync(InvoiceID, BookID);
        }
        public async Task<Domain.Entities.InvoiceDetail?> UpdateAsync<TUpdateDto>(int InvoiceID, int BookID, TUpdateDto entity) where TUpdateDto : class
        {
            var existingEntity = await GetByIdAsync(InvoiceID, BookID);
            if (existingEntity == null)
            {
            return null;
            }
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            return existingEntity;
        }
    }

}
