using AutoMapper;
using BookManagementSystem.Application.Dtos.PaymentReceipt;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Validators;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.PaymentReceipt;
using FluentValidation;
using FluentValidation.Results;

namespace BookManagementSystem.Application.Services
{
    public class PaymentReceiptService : IPaymentReceiptService
    {
        private readonly IPaymentReceiptRepository _paymentReceiptRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreatePaymentReceiptDto> _createValidator;
        private readonly IValidator<UpdatePaymentReceiptDto> _updateValidator;

        public PaymentReceiptService(
            IPaymentReceiptRepository paymentReceiptRepository,
            IMapper mapper,
            IValidator<CreatePaymentReceiptDto> createValidator,
            IValidator<UpdatePaymentReceiptDto> updateValidator)
        {
            _paymentReceiptRepository = paymentReceiptRepository ?? throw new ArgumentNullException(nameof(paymentReceiptRepository));
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<PaymentReceiptDto> CreateNewPaymentReceipt(CreatePaymentReceiptDto createPaymentReceiptDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createPaymentReceiptDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var paymentReceipt = _mapper.Map<PaymentReceipt>(createPaymentReceiptDto);
            await _paymentReceiptRepository.AddAsync(paymentReceipt);
            return _mapper.Map<PaymentReceiptDto>(paymentReceipt);
        }

        public async Task<PaymentReceiptDto> UpdatePaymentReceipt(string receiptId, UpdatePaymentReceiptDto updatePaymentReceiptDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updatePaymentReceiptDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var updatedReceipt = await _paymentReceiptRepository.UpdateAsync(receiptId, updatePaymentReceiptDto);
            if (updatedReceipt == null)
            {
                throw new KeyNotFoundException($"PaymentReceipt with ID {receiptId} not found.");
            }

            return _mapper.Map<PaymentReceiptDto>(updatedReceipt);
        }

        public async Task<PaymentReceiptDto> GetPaymentReceiptById(string receiptId)
        {
            var paymentReceipt = await _paymentReceiptRepository.GetByIdAsync(receiptId);
            if (paymentReceipt == null)
            {
                throw new KeyNotFoundException($"PaymentReceipt with ID {receiptId} not found.");
            }
            return _mapper.Map<PaymentReceiptDto>(paymentReceipt);
        }

        // public async Task<IEnumerable<PaymentReceiptDto>> GetAllPaymentReceipts()
        // {
        //     var paymentReceipts = await _paymentReceiptRepository.GetAllAsync();
        //     return _mapper.Map<IEnumerable<PaymentReceiptDto>>(paymentReceipts);
        // }

        public async Task<bool> DeletePaymentReceipt(string receiptId)
        {
            var paymentReceipt = await _paymentReceiptRepository.GetByIdAsync(receiptId);
            if (paymentReceipt == null)
            {
                return false;
            }
            _paymentReceiptRepository.Remove(paymentReceipt);
            return true;
        }
    }
}
