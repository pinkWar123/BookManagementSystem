using AutoMapper;
using BookManagementSystem.Application.Dtos.Invoice;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.Invoice;
using BookManagementSystem.Application.Exceptions;
using BookManagementSystem.Application.Queries;
using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Application.Dtos.Customer;
using BookManagementSystem.Application.Dtos.DebtReportDetail;
using BookManagementSystem.Data;
using BookManagementSystem.Application.Dtos.Book;

namespace BookManagementSystem.Application.Services
{

    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ICustomerService _customerService;
        private readonly IRegulationService _regulationService;
        private readonly IBookService _bookService;
        private readonly IDebtReportService _debtReportService;
        private readonly IDebtReportDetailService _debtReportDetailService;
        private readonly IMapper _mapper;
        public InvoiceService(
            IInvoiceRepository invoiceRepository, 
            ICustomerService customerService,
            IRegulationService regulationService,
            IBookService bookService,
            IDebtReportService debtReportService,
            IDebtReportDetailService debtReportDetailService,
            IMapper mapper
  )
        {
            _customerService = customerService;
            _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
            _regulationService = regulationService;
            _bookService = bookService;
            _debtReportService = debtReportService;
            _debtReportDetailService = debtReportDetailService;
            _mapper = mapper;
        }

        public async Task<InvoiceDto> CreateNewInvoice(CreateInvoiceDto createInvoiceDto)
        {
            if (createInvoiceDto == null)
                throw new InvoiceException("CreateInvoiceDto is required");
            var invoiceDetails = createInvoiceDto.InvoiceDetails;
            if (invoiceDetails == null)
                throw new InvoiceException("InvoiceDetails is required");
            var invoice = _mapper.Map<Invoice>(createInvoiceDto);
            var customer = await _customerService.GetCustomerById(invoice.CustomerID);
            if (customer == null)
                throw new CustomerException($"Customer not found with ID {invoice.CustomerID}");
            // Check if Customer is blocked
            var regulation = await _regulationService.GetRegulationById(1);

            // Update TotalDebt of Customer
            var totalDebt = 0;
            foreach (var detail in invoiceDetails)
            {
                int bookID = detail.BookID;
                var book = await _bookService.GetBookById(bookID);
                totalDebt += detail.Quantity * book.Price;
            }
            var updateCustomerDto = new UpdateCustomerDto
            {
                TotalDebt = totalDebt + customer.TotalDebt
            };
            await _customerService.UpdateCustomer(invoice.CustomerID, updateCustomerDto);

            /// Update FinalDebt of DebtReportDetail
            var invoiceDto = _mapper.Map<InvoiceDto>(invoice);
            int reportId = await _debtReportService.GetReportIdByMonthYear(invoiceDto.InvoiceDate.Month, invoiceDto.InvoiceDate.Year);

            var debtReportDetail = await _debtReportDetailService.GetDebtReportDetailById(reportId,invoice.CustomerID);
            if (debtReportDetail == null)
            {
                throw new DebtReportDetailNotFound(reportId, invoice.CustomerID);
            }
            var updateDebtReportDetailDto = new UpdateDebtReportDetailDto
            {
                FinalDebt = debtReportDetail.FinalDebt + totalDebt
            };
            await _debtReportDetailService.UpdateDebtReportDetail(reportId, invoice.CustomerID, updateDebtReportDetailDto);






            await _invoiceRepository.AddAsync(invoice);
            await _invoiceRepository.SaveChangesAsync();
            if (createInvoiceDto.InvoiceDetails == null)
                throw new InvoiceException("InvoiceDetails is required");
            
            return _mapper.Map<InvoiceDto>(invoice);
        }

        public async Task<InvoiceDto> UpdateInvoice(int  InvoiceID, UpdateInvoiceDto updateInvoiceDto)
        {   
            
            var existingEntry = await _invoiceRepository.GetByIdAsync(InvoiceID);
            if (existingEntry == null)
            {
                throw new InvoiceException($"Không tìm thấy hóa đơn với ID {InvoiceID}");
            }

            _mapper.Map(updateInvoiceDto, existingEntry);
            var updatedEntry = await _invoiceRepository.UpdateAsync(InvoiceID, existingEntry);
            await _invoiceRepository.SaveChangesAsync();
            return _mapper.Map<InvoiceDto>(updatedEntry);
        }

        public async Task<InvoiceDto> GetInvoiceById(int InvoiceID)
        {
            
            var invoice = await _invoiceRepository.GetByIdAsync(InvoiceID);
            if (invoice == null)
            {
                throw new InvoiceException($"Không tìm thấy hóa đơn với ID {InvoiceID}");
            }
            return _mapper.Map<InvoiceDto>(invoice);
        }

        public async Task<IEnumerable<InvoiceDto>> GetAllInvoice(InvoiceQuery invoiceQuery)
        {
            var query = _invoiceRepository.GetValuesByQuery(invoiceQuery);
            if (query == null)
            {
                return Enumerable.Empty<InvoiceDto>();
            }

            var invoices = await query.ToListAsync();

            return _mapper.Map<IEnumerable<InvoiceDto>>(invoices);
        }

        public async Task<bool> DeleteInvoice(int InvoiceID)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(InvoiceID);
            if (invoice == null)
            {
                throw new InvoiceException($"Không tìm thấy hóa đơn với ID {InvoiceID}");
            }
            _invoiceRepository.Remove(invoice);
            await _invoiceRepository.SaveChangesAsync();
            return true;
        }
    }
}