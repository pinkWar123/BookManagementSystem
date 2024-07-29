using BookManagementSystem.Application.Dtos.PaymentReceipt;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IPaymentReceiptService
    {
        Task<PaymentReceiptDto> CreateNewPaymentReceipt(CreatePaymentReceiptDto createPaymentReceiptDto);
        Task<PaymentReceiptDto> UpdatePaymentReceipt(int receiptId, UpdatePaymentReceiptDto updatePaymentReceiptDto);
        Task<PaymentReceiptDto> GetPaymentReceiptById(int receiptId);
        // Task<IEnumerable<PaymentReceiptDto>> GetAllPaymentReceipts();
        Task<bool> DeletePaymentReceipt(int receiptId);
    }
}
