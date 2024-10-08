using AutoMapper;
using BookManagementSystem.Application.Dtos.PaymentReceipt;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Validators;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.PaymentReceipt;
using BookManagementSystem.Infrastructure.Repositories.Regulation;
using FluentValidation;
using FluentValidation.Results;
using BookManagementSystem.Application.Exceptions;
using System.Net;
using BookManagementSystem.Application.Queries;
using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Data;
using BookManagementSystem.Application.Dtos.Customer;
using BookManagementSystem.Application.Dtos.DebtReportDetail;

namespace BookManagementSystem.Application.Services
{
    public class PaymentReceiptService : IPaymentReceiptService
    {
        private readonly IPaymentReceiptRepository _paymentReceiptRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _context;
        private readonly ICustomerService _customerService;
        private readonly IRegulationService _regulationService;
        private readonly IDebtReportService _debtReportService;

        private readonly IDebtReportDetailService _debtReportDetailService;
        public PaymentReceiptService(
            IPaymentReceiptRepository paymentReceiptRepository,
            IMapper mapper,
            ApplicationDBContext context,
            ICustomerService customerService,
            IRegulationService regulationService,
            IDebtReportDetailService debtReportDetailService,
            IDebtReportService debtReportService
        )
        {
            _paymentReceiptRepository = paymentReceiptRepository ?? throw new ArgumentNullException(nameof(paymentReceiptRepository));
            _mapper = mapper;
            _context = context;
            _customerService = customerService;
            _regulationService = regulationService;
            _debtReportDetailService = debtReportDetailService;
            _debtReportService = debtReportService;
        }

        public async Task<PaymentReceiptDto> CreateNewPaymentReceipt(CreatePaymentReceiptDto createPaymentReceiptDto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Map DTO to PaymentReceipt
                    var paymentReceipt = _mapper.Map<PaymentReceipt>(createPaymentReceiptDto);
                    await _paymentReceiptRepository.AddAsync(paymentReceipt);
                    var customer = await _customerService.GetCustomerById(paymentReceipt.CustomerID);
                    if (customer == null)
                    {
                        throw new CustomerNotFound(paymentReceipt.CustomerID);
                    }

                    // Update TotalDebt of Customer
                    var regulation = await _regulationService.GetPaymentNotExceedDebt();

                    if (regulation?.Status == true && createPaymentReceiptDto.Amount > customer.TotalDebt)
                    {
                        throw new PaymentReceiptConflictRegulation();
                    }
                    
                    var updateCustomerDto = new UpdateCustomerDto
                    {
                        TotalDebt = Math.Max(customer.TotalDebt - (createPaymentReceiptDto.Amount ?? 0), 0)
                    };

                    await _customerService.UpdateCustomer(paymentReceipt.CustomerID, updateCustomerDto);

                    // Update FinalDebt of DebtReportDetail
                    var paymentReceiptDto = _mapper.Map<PaymentReceiptDto>(paymentReceipt);
                    int reportId = await _debtReportService.GetReportIdByMonthYear(paymentReceiptDto.ReceiptDate.Month, paymentReceiptDto.ReceiptDate.Year);

                    var debtReportDetail = await _debtReportDetailService.GetDebtReportDetailById(reportId,paymentReceipt.CustomerID);
                    if (debtReportDetail == null)
                    {
                        throw new DebtReportDetailNotFound(reportId, paymentReceipt.CustomerID);
                    }
                    var updateDebtReportDetailDto = new UpdateDebtReportDetailDto
                    {
                        FinalDebt = debtReportDetail.FinalDebt - createPaymentReceiptDto.Amount
                    };

                    await _debtReportDetailService.UpdateDebtReportDetail(reportId, paymentReceipt.CustomerID, updateDebtReportDetailDto);

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return _mapper.Map<PaymentReceiptDto>(paymentReceipt);
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }
        }

        public async Task<PaymentReceiptDto> UpdatePaymentReceipt(int receiptId, UpdatePaymentReceiptDto updatePaymentReceiptDto)
        {
            var paymentReceipt = await _paymentReceiptRepository.GetByIdAsync(receiptId);
            if (paymentReceipt == null)
            {
                throw new PaymentReceiptNotFound(receiptId);
            }

            _mapper.Map(updatePaymentReceiptDto, paymentReceipt);

            await _paymentReceiptRepository.UpdateAsync(receiptId, paymentReceipt);
            await _paymentReceiptRepository.SaveChangesAsync();

            return _mapper.Map<PaymentReceiptDto>(paymentReceipt);
        }


        public async Task<PaymentReceiptDto> GetPaymentReceiptById(int receiptId)
        {
            var paymentReceipt = await _paymentReceiptRepository.GetByIdAsync(receiptId);
            if (paymentReceipt == null)
            {
                throw new PaymentReceiptNotFound(receiptId);
            }
            return _mapper.Map<PaymentReceiptDto>(paymentReceipt);
        }

        public async Task<IEnumerable<PaymentReceiptDto>> GetAllPaymentReceipts(PaymentReceiptQuery paymentReceiptQuery)
        {
            var query = _paymentReceiptRepository.GetValuesByQuery(paymentReceiptQuery);
            if (query == null)
            {
                return Enumerable.Empty<PaymentReceiptDto>();
            }
            
            var paymentReceipt = await query.ToListAsync();

            return _mapper.Map<IEnumerable<PaymentReceiptDto>>(paymentReceipt);
        }

        public async Task<bool> DeletePaymentReceipt(int receiptId)
        {
            var paymentReceipt = await _paymentReceiptRepository.GetByIdAsync(receiptId);
            if (paymentReceipt == null)
            {
                return false;
            }
            _paymentReceiptRepository.Remove(paymentReceipt);
            await _paymentReceiptRepository.SaveChangesAsync();
            return true;
        }

        public async Task<int> GetTotalAmountByMonthYear(int month, int year)
        {
            var paymentReceiptIds = await _paymentReceiptRepository.GetIdListByMonthYearAsync(month, year);

            if (paymentReceiptIds == null || !paymentReceiptIds.Any()) // If no receipts were found, return 0
            {
                return 0;
            }

            int totalAmount = 0;
            foreach (var id in paymentReceiptIds)
            {
                totalAmount += await _paymentReceiptRepository.GetTotalAmountByIdAsync(id);
            }

            return totalAmount;
        }

        public async Task<int> GetTotalAmountByOnlyYear(int year)
        {
            var paymentReceiptIds = await _paymentReceiptRepository.GetIdListByYearAsync(year);

            if (paymentReceiptIds == null || !paymentReceiptIds.Any()) // If no receipts were found, return 0
            {
                return 0;
            }

            int totalAmount = 0;
            foreach (var id in paymentReceiptIds)
            {
                totalAmount += await _paymentReceiptRepository.GetTotalAmountByIdAsync(id);
            }

            return totalAmount;
        }

        public async Task<List<IncomeByMonthDto>> GetIncomeFromJanuary()
        {
            var today = DateTime.UtcNow;
            var incomeList = new List<IncomeByMonthDto>();
            for(var i = 1; i <= today.Month; i++)
            {
                var income = await GetTotalAmountByMonthYear(i, today.Year);
                var incomeByMonthDto = new IncomeByMonthDto
                {
                    Month = i,
                    Year = today.Year,
                    Income = income
                };
                incomeList.Add(incomeByMonthDto);
            }

            return incomeList;
        }
    }
}
