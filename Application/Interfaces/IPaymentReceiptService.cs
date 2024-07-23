using BookManagementSystem.Application.Dtos.PaymentReceipt;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IPaymentReceiptService
    {
        Task<PaymentReceiptDto> CreateNewPaymentReceipt(CreatePaymentReceiptDto createPaymentReceiptDto);
        Task<PaymentReceiptDto> UpdatePaymentReceipt(string receiptId, UpdatePaymentReceiptDto updatePaymentReceiptDto);
        Task<PaymentReceiptDto> GetPaymentReceiptById(string receiptId);
        // Task<IEnumerable<PaymentReceiptDto>> GetAllPaymentReceipts();
        Task<bool> DeletePaymentReceipt(string receiptId);
    }
}
