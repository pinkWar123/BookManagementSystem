using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Infrastructure.Repositories.InvoiceDetail
{
    public class InvoiceDetailRepository : GenericRepository<Domain.Entities.InvoiceDetail>, IInvoiceDetailRepository
    {
        private readonly ApplicationDBContext _context;

        public InvoiceDetailRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public void Detach(Domain.Entities.InvoiceDetail entity)
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public async Task<Domain.Entities.InvoiceDetail?> GetByIdAsync(int InvoiceID, int BookID)
        {
            var entity = await _context.Set<Domain.Entities.InvoiceDetail>().FindAsync(InvoiceID, BookID);
            if (entity == null)
            {
                Console.WriteLine("deo tim ra gi ca");
                return null;
            }
            Console.WriteLine("ghost ghost ghost ghost");
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