using BookManagementSystem.Application.Dtos.InvoiceDetail;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IInvoiceDetailService
    {
        Task<InvoiceDetailDto> CreateNewInvoiceDetail(CreateInvoiceDetailDto createInvoiceDetailDto);
        Task<InvoiceDetailDto> UpdateInvoiceDetail(int InvoiceID, int BookID, UpdateInvoiceDetailDto updateInvoiceDetailDto);
        Task<InvoiceDetailDto> GetInvoiceDetailById(int InvoiceID, int BookID);
        // Task<IEnumerable<InvoiceDetailDto>> GetAllInvoiceDetail();
        Task<bool> DeleteInvoiceDetail(int InvoiceID, int BookID);
    }
}