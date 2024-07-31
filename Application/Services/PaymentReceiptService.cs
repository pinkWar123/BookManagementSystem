using AutoMapper;
using BookManagementSystem.Application.Dtos.PaymentReceipt;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Validators;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.PaymentReceipt;
using FluentValidation;
using FluentValidation.Results;
using BookManagementSystem.Application.Exceptions;
using System.Net;
using BookManagementSystem.Application.Queries;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Application.Services
{
    public class PaymentReceiptService : IPaymentReceiptService
    {
        private readonly IPaymentReceiptRepository _paymentReceiptRepository;
        private readonly IMapper _mapper;

        public PaymentReceiptService(
            IPaymentReceiptRepository paymentReceiptRepository,
            IMapper mapper)
        {
            _paymentReceiptRepository = paymentReceiptRepository ?? throw new ArgumentNullException(nameof(paymentReceiptRepository));
            _mapper = mapper;
        }

        public async Task<PaymentReceiptDto> CreateNewPaymentReceipt(CreatePaymentReceiptDto createPaymentReceiptDto)
        {
            var paymentReceipt = _mapper.Map<PaymentReceipt>(createPaymentReceiptDto);
            await _paymentReceiptRepository.AddAsync(paymentReceipt);
            await _paymentReceiptRepository.SaveChangesAsync();
            return _mapper.Map<PaymentReceiptDto>(paymentReceipt);
        }

        public async Task<PaymentReceiptDto> UpdatePaymentReceipt(int receiptId, UpdatePaymentReceiptDto updatePaymentReceiptDto)
        {
            var updatedReceipt = await _paymentReceiptRepository.UpdateAsync(receiptId, updatePaymentReceiptDto);
            if (updatedReceipt == null)
            {
                throw new PaymentReceiptException($"Không tìm thấy hóa đơn với ID {receiptId}.", HttpStatusCode.NotFound);
            }
            await _paymentReceiptRepository.SaveChangesAsync();
            return _mapper.Map<PaymentReceiptDto>(updatedReceipt);
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
