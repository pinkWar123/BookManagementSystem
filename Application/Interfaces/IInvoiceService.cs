
using BookManagementSystem.Application.Dtos.Invoice;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IInvoiceService
    {
        Task<InvoiceDto> CreateNewInvoice(CreateInvoiceDto createInvoiceDto);
        Task<InvoiceDto> UpdateInvoice(string InvoiceID, UpdateInvoiceDto updateInvoiceDto);
        Task<InvoiceDto> GetInvoiceById(string InvoiceID);
        // Task<IEnumerable<InvoiceDto>> GetAllInvoice();
        Task<bool> DeleteInvoice(string InvoiceID);
    }
}