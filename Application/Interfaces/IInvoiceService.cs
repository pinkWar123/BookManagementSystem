
using BookManagementSystem.Application.Dtos.Invoice;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IInvoiceService
    {
        Task<InvoiceDto> CreateNewInvoice(CreateInvoiceDto createInvoiceDto);
        Task<InvoiceDto> UpdateInvoice(int InvoiceID, UpdateInvoiceDto updateInvoiceDto);
        Task<InvoiceDto> GetInvoiceById(int InvoiceID);
        // Task<IEnumerable<InvoiceDto>> GetAllInvoice();
        Task<bool> DeleteInvoice(int InvoiceID);
    }
}