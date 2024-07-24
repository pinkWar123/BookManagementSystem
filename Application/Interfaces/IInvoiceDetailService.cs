using BookManagementSystem.Application.Dtos.InvoiceDetail;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IInvoiceDetailService
    {
        Task<InvoiceDetailDto> CreateNewInvoiceDetail(CreateInvoiceDetailDto createInvoiceDetailDto);
        Task<InvoiceDetailDto> UpdateInvoiceDetail(string InvoiceID, string BookID, UpdateInvoiceDetailDto updateInvoiceDetailDto);
        Task<InvoiceDetailDto> GetInvoiceDetailById(string InvoiceID, string BookID);
        // Task<IEnumerable<InvoiceDetailDto>> GetAllInvoiceDetail();
        Task<bool> DeleteInvoiceDetail(string InvoiceID, string BookID);
    }
}