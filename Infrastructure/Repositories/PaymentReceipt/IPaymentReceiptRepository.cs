
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.PaymentReceipt
{
    public interface IPaymentReceiptRepository : IGenericRepository<Domain.Entities.PaymentReceipt>
    {
        Task<List<int>> GetIdListByMonthYearAsync(int month, int year);
        Task<List<int>> GetIdListByYearAsync(int year);
        Task<int> GetTotalAmountByIdAsync(int id);
    }
}
