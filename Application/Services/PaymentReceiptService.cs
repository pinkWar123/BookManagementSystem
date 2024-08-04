using AutoMapper;
using BookManagementSystem.Application.Dtos.PaymentReceipt;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Validators;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.PaymentReceipt;
using BookManagementSystem.Infrastructure.Repositories.Customer;
using FluentValidation;
using FluentValidation.Results;
using BookManagementSystem.Application.Exceptions;
using System.Net;
using BookManagementSystem.Application.Queries;
using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Data;
using BookManagementSystem.Application.Dtos.Customer;

namespace BookManagementSystem.Application.Services
{
    public class PaymentReceiptService : IPaymentReceiptService
    {
        private readonly IPaymentReceiptRepository _paymentReceiptRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _context;
        private readonly ICustomerService _customerService;

        public PaymentReceiptService(
            IPaymentReceiptRepository paymentReceiptRepository,
            IMapper mapper,
            ApplicationDBContext context,
            ICustomerService customerService
        )
        {
            _paymentReceiptRepository = paymentReceiptRepository ?? throw new ArgumentNullException(nameof(paymentReceiptRepository));
            _mapper = mapper;
            _context = context;
            _customerService = customerService;
        }

        // public async Task<PaymentReceiptDto> CreateNewPaymentReceipt(CreatePaymentReceiptDto createPaymentReceiptDto)
        // {
        //     var paymentReceipt = _mapper.Map<PaymentReceipt>(createPaymentReceiptDto);
        //     await _paymentReceiptRepository.AddAsync(paymentReceipt);
        //     await _paymentReceiptRepository.SaveChangesAsync();
        //     return _mapper.Map<PaymentReceiptDto>(paymentReceipt);
        // }

        // public async Task<PaymentReceiptDto> CreateNewPaymentReceipt(CreatePaymentReceiptDto createPaymentReceiptDto)
        // {
        //     using (var transaction = await _context.Database.BeginTransactionAsync())
        //     {
        //         try
        //         {
        //             // Map DTO to PaymentReceipt
        //             var paymentReceipt = _mapper.Map<PaymentReceipt>(createPaymentReceiptDto);

        //             await _paymentReceiptRepository.AddAsync(paymentReceipt);

        //             // Update TotalDebt of Customer
        //             var customer = await _customerRepository.GetByIdAsync(paymentReceipt.CustomerID);
        //             if (customer == null)
        //             {
        //                 throw new Exception("Customer not found");
        //             }

        //             customer.TotalDebt -= paymentReceipt.Amount;
        //             await _customerRepository.UpdateAsync(paymentReceipt.CustomerID, customer);

        //             await _context.SaveChangesAsync();
        //             await transaction.CommitAsync();

        //             // Map PaymentReceipt to PaymentReceiptDto
        //             return _mapper.Map<PaymentReceiptDto>(paymentReceipt);
        //         }
        //         catch
        //         {
        //             await transaction.RollbackAsync();
        //             throw;
        //         }
        //     }
        // }

        public async Task<PaymentReceiptDto> CreateNewPaymentReceipt(CreatePaymentReceiptDto createPaymentReceiptDto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    // Map DTO to PaymentReceipt
                    var paymentReceipt = _mapper.Map<PaymentReceipt>(createPaymentReceiptDto);
                    await _paymentReceiptRepository.AddAsync(paymentReceipt);
                    await _paymentReceiptRepository.SaveChangesAsync();
           
                    var customer = await _customerService.GetCustomerById(paymentReceipt.CustomerID);
                    if (customer == null)
                    {
                        throw new CustomerException($"Không tìm thấy khách hàng với ID {paymentReceipt.CustomerID}.", HttpStatusCode.NotFound);
                    }

                    if (createPaymentReceiptDto.Amount > customer.TotalDebt)
                    {
                        throw new PaymentReceiptException("Số nợ thanh toán phải nhỏ hơn hoặc bằng tổng nợ.", HttpStatusCode.BadRequest);
                    }

                    var updateCustomerDto = new UpdateCustomerDto
                    {
                        TotalDebt = customer.TotalDebt - createPaymentReceiptDto.Amount
                    };

                    // Update TotalDebt of Customer
                    await _customerService.UpdateCustomer(paymentReceipt.CustomerID, updateCustomerDto);

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
                throw new KeyNotFoundException("PaymentReceipt not found");
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
                throw new PaymentReceiptException($"Không tìm thấy hóa đơn với ID {receiptId}.", HttpStatusCode.NotFound);
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
    }
}
