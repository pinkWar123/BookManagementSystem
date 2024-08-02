
using BookManagementSystem.Application.Dtos.Invoice;
using BookManagementSystem.Application.Queries;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IInvoiceService
    {
        Task<InvoiceDto> CreateNewInvoice(CreateInvoiceDto createInvoiceDto);
        Task<InvoiceDto> UpdateInvoice(int InvoiceID, UpdateInvoiceDto updateInvoiceDto);
        Task<InvoiceDto> GetInvoiceById(int InvoiceID);
        Task<IEnumerable<InvoiceDto>> GetAllInvoice(InvoiceQuery query);
        Task<bool> DeleteInvoice(int InvoiceID);
    }
}