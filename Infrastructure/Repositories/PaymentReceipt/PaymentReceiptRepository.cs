using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.PaymentReceipt
{
    public class PaymentReceiptRepository : GenericRepository<Domain.Entities.PaymentReceipt>, IPaymentReceiptRepository
    {
        public PaymentReceiptRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
        public async Task<List<int>> GetIdListByMonthYearAsync(int month, int year)
        {
            var receiptIds = await _context.PaymentReceipts
                .Where(r => r.Date.Month == month && r.Date.Year == year)
                .Select(r => r.Id)
                .ToListAsync();

            return receiptIds;
        }

        public async Task<List<int>> GetIdListByYearAsync(int year)
        {
            var receiptIds = await _context.PaymentReceipts
                .Where(r => r.Date.Year == year)
                .Select(r => r.Id)
                .ToListAsync();

            return receiptIds;
        }

        public async Task<int> GetTotalAmountByIdAsync(int id)
        {
            var totalAmount = await _context.PaymentReceipts
                .Where(r => r.Id == id)
                .SumAsync(r => r.Amount);

            return totalAmount;
        }
    }

}
