using AutoMapper;
using BookManagementSystem.Application.Dtos.Invoice;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.Invoice;
using BookManagementSystem.Infrastructure.Repositories.InvoiceDetail;
using BookManagementSystem.Application.Exceptions;
using BookManagementSystem.Application.Queries;
using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Application.Dtos.Customer;
using BookManagementSystem.Application.Dtos.DebtReportDetail;
using BookManagementSystem.Application.Dtos.Book;
using BookManagementSystem.Application.Dtos.InventoryReportDetail;




namespace BookManagementSystem.Application.Services
{

    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IInvoiceDetailRepository _invoiceDetailRepository;
        private readonly ICustomerService _customerService;
        private readonly IRegulationService _regulationService;
        private readonly IBookService _bookService;
        private readonly IDebtReportService _debtReportService;
        private readonly IDebtReportDetailService _debtReportDetailService;
        private readonly IInventoryReportService _inventoryReportService;
        private readonly IInventoryReportDetailService _inventoryReportDetail;
        private readonly IMapper _mapper;
        public InvoiceService(
            IInvoiceRepository invoiceRepository, 
            IInvoiceDetailRepository invoiceDetailRepository,
            ICustomerService customerService,
            IRegulationService regulationService,
            IBookService bookService,
            IDebtReportService debtReportService,
            IDebtReportDetailService debtReportDetailService,
            IInventoryReportService inventoryReportService,
            IInventoryReportDetailService inventoryReportDetail,
            IMapper mapper
  )
        {
            _customerService = customerService;
            _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
            _invoiceDetailRepository = invoiceDetailRepository ?? throw new ArgumentNullException(nameof(invoiceDetailRepository));
            _regulationService = regulationService;
            _bookService = bookService;
            _debtReportService = debtReportService;
            _debtReportDetailService = debtReportDetailService;
            _inventoryReportService = inventoryReportService;
            _inventoryReportDetail = inventoryReportDetail;
            _mapper = mapper;
        }

        public async Task<InvoiceDto> CreateNewInvoice(CreateInvoiceDto createInvoiceDto)
        {
            var invoiceDetails = createInvoiceDto.InvoiceDetails;
            
            var invoice = _mapper.Map<Invoice>(createInvoiceDto);
            var customer = await _customerService.GetCustomerById(invoice.CustomerID);

            if (customer == null)
                throw new CustomerException($"Customer not found with ID {invoice.CustomerID}");
            // Check if Customer is blocked
            var regulation = await _regulationService.GetMaximumCustomerDebt();
            if(customer.TotalDebt > regulation?.Value)
            {
                throw new DebtExceedException();
            }
            // check book is available
            foreach (var detail in invoiceDetails)
            {
                int bookID = detail.BookID;
                var book = await _bookService.GetBookById(bookID);
                if (book == null)
                    throw new BaseException($"Sách {book.Title} không tồn tại");
                if(book.StockQuantity < detail.Quantity) 
                    throw new ExceedMinimumInventoryAfterSelling();
            }
            // check two books are the same
            for (int i = 0; i < invoiceDetails.Count; i++)
            {
                for (int j = i + 1; j < invoiceDetails.Count; j++)
                {
                    if (invoiceDetails[i].BookID == invoiceDetails[j].BookID)
                        throw new BaseException($"Có 2 cuốn sách {invoiceDetails[i].BookID} trong hóa đơn");
                }
            }
            
            var invoiceDto = _mapper.Map<InvoiceDto>(invoice);
            int inventoryReportID = await _inventoryReportService.GetReportIdByMonthYear(invoiceDto.InvoiceDate.Month, invoiceDto.InvoiceDate.Year);
            
            if (inventoryReportID == null)
            {
                throw new BaseException($"Không tìm thấy báo cáo tồn kho tháng {invoiceDto.InvoiceDate.Month} năm {invoiceDto.InvoiceDate.Year}");
            }




            int debtReportId = await _debtReportService.GetReportIdByMonthYear(invoiceDto.InvoiceDate.Month, invoiceDto.InvoiceDate.Year);
            var debtReportDetail = await _debtReportDetailService.GetDebtReportDetailById(debtReportId,invoice.CustomerID);
            if (debtReportDetail == null)
            {
                throw new DebtReportDetailNotFound(debtReportId, invoice.CustomerID);
            }
            // Update TotalDebt of Customer
            var totalDebt = 0;
            var inventoryRegulation = await _regulationService.GetMinimumInventoryAfterSelling();
            foreach (var detail in invoiceDetails)
            {
                int bookID = detail.BookID;
                var book = await _bookService.GetBookById(bookID);
                if(inventoryRegulation?.Status == true && book.StockQuantity - detail.Quantity < inventoryRegulation?.Value)
                    throw new ExceedMinimumInventoryAfterSelling();
                totalDebt += detail.Quantity * book.Price;
            }

            // Update StockQuantity of Book
            foreach (var detail in invoiceDetails)
            {
                int bookID = detail.BookID;
                var book = await _bookService.GetBookById(bookID);
                var updateBookDto = new UpdateBookDto
                {
                    StockQuantity = book.StockQuantity - detail.Quantity
                };
                // update inventory report detail
                var inventoryReportDetail = await _inventoryReportDetail.GetInventoryReportDetailById(inventoryReportID, bookID);
                var updateInventoryReportDetailDto = new UpdateInventoryReportDetailDto
                {
                    FinalStock = inventoryReportDetail.FinalStock - detail.Quantity,
                    AdditionalStock = inventoryReportDetail.FinalStock - inventoryReportDetail.InitialStock
                };
                await _inventoryReportDetail.UpdateInventoryReportDetail(inventoryReportID, bookID, updateInventoryReportDetailDto);
                await _bookService.UpdateBook(bookID, updateBookDto);
            }
            
            var updateCustomerDto = new UpdateCustomerDto
            {
                TotalDebt = totalDebt + customer.TotalDebt
            };
            await _customerService.UpdateCustomer(invoice.CustomerID, updateCustomerDto);

            /// Update FinalDebt of DebtReportDetail
            
            var updateDebtReportDetailDto = new UpdateDebtReportDetailDto
            {
                FinalDebt = debtReportDetail.FinalDebt + totalDebt
            };
            await _debtReportDetailService.UpdateDebtReportDetail(debtReportId, invoice.CustomerID, updateDebtReportDetailDto);
            

            await _invoiceRepository.AddAsync(invoice);
            await _invoiceRepository.SaveChangesAsync();
            if (createInvoiceDto.InvoiceDetails == null)
                throw new InvoiceException("Chi tiết hóa đơn không được để trống");
            
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
        public async Task<List<int>> getInvoiceByMonth(int month, int year)
        {
            return await _invoiceRepository.GetInvoiceIdByMonthYearAsync(month, year);
        }
        public async Task<IncomeViewDto?> getPriceByMonth(int month, int year)
        {
            var invoiceIds = await _invoiceRepository.GetInvoiceIdByMonthYearAsync(month, year);
            var totalPrices = 0;
            foreach (int invoiceId in invoiceIds)
            {
                var invoice = await _invoiceRepository.GetByIdAsync(invoiceId);
                if (invoice == null)
                    throw new InvoiceException($"Không tìm thấy hóa đơn với ID {invoiceId}");
                var invoiceDetail = await _invoiceDetailRepository.FindInvoiceDetailsByInvoiceIdAsync(invoiceId);
                if (invoiceDetail == null)
                    throw new InvoiceException($"Không tìm thấy chi tiết hóa đơn với ID {invoiceId}");
                foreach (var detail in invoiceDetail)
                {
                    int bookID = detail.BookID;
                    var book = await _bookService.GetBookById(bookID);
                    totalPrices += detail.Quantity * book.Price;
                }
            }
            var incomeViewDto = new IncomeViewDto
            {
                Month = month,
                Year = year,
                Income = totalPrices
            };
            return incomeViewDto;
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

        public async Task<int> GetInvoiceCountByMonthYear(int month, int year)
        {
            return await _invoiceRepository.GetInvoiceCountByMonthYear(month, year);
        }

    }
}