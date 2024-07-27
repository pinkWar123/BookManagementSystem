using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.InvoiceDetail;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.InvoiceDetail;
using BookManagementSystem.Application.Validators;
using FluentValidation;
using FluentValidation.Results;

namespace BookManagementSystem.Application.Services
{
    public class InvoiceDetailService : IInvoiceDetailService
    {
        private readonly IInvoiceDetailRepository _invoiceDetailRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateInvoiceDetailDto> _createValidator;
        private readonly IValidator<UpdateInvoiceDetailDto> _updateValidator;

        public InvoiceDetailService(
            IInvoiceDetailRepository invoiceDetailRepository, 
            IMapper mapper, 
            IValidator<CreateInvoiceDetailDto> createValidator,
            IValidator<UpdateInvoiceDetailDto> updateValidator)
        {
            _invoiceDetailRepository = invoiceDetailRepository ?? throw new ArgumentNullException(nameof(invoiceDetailRepository));
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<InvoiceDetailDto> CreateNewInvoiceDetail(CreateInvoiceDetailDto createInvoiceDetailDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createInvoiceDetailDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var invoiceDetail = _mapper.Map<InvoiceDetail>(createInvoiceDetailDto);
            await _invoiceDetailRepository.AddAsync(invoiceDetail);
            return _mapper.Map<InvoiceDetailDto>(invoiceDetail);
        }

        public async Task<InvoiceDetailDto> UpdateInvoiceDetail(int InvoiceID, int BookID, UpdateInvoiceDetailDto updateInvoiceDetailDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateInvoiceDetailDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // write again GetByIdAsync
            // var existingDetail = await _invoiceDetailRepository.GetByIdAsync(InvoiceID, BookID);
            var existingDetail = await _invoiceDetailRepository.GetByIdAsync(InvoiceID);

            if (existingDetail == null)
            {
                throw new InvoiceDetailException($"Không tìm thấy chi tiết hóa đơn với InvoiceID {InvoiceID} và BookID {BookID}");
            }

            _mapper.Map(updateInvoiceDetailDto, existingDetail);

            // write again UpdateAsync
            // var updatedDetail = await _invoiceDetailRepository.UpdateAsync(InvoiceID, BookID, existingDetail);
            var updatedDetail = await _invoiceDetailRepository.UpdateAsync(InvoiceID, existingDetail);

            return _mapper.Map<InvoiceDetailDto>(updatedDetail);
        }

        public async Task<InvoiceDetailDto> GetInvoiceDetailById(int InvoiceID, int BookID)
        {
            // var invoiceDetail = await _invoiceDetailRepository.GetByIdAsync(InvoiceID, BookID);
            var invoiceDetail = await _invoiceDetailRepository.GetByIdAsync(InvoiceID);
            if (invoiceDetail == null)
                throw new InvoiceDetailException($"Không tìm thấy chi tiết hóa đơn với InvoiceID {InvoiceID} và BookID {BookID}");
            
            return _mapper.Map<InvoiceDetailDto>(invoiceDetail);
        }

        public async Task<bool> DeleteInvoiceDetail(int InvoiceID, int BookID)
        {
            // var invoiceDetail = await _invoiceDetailRepository.GetByIdAsync(InvoiceID, BookID);
            var invoiceDetail = await _invoiceDetailRepository.GetByIdAsync(InvoiceID);
            if (invoiceDetail == null)
                return false;
        
            _invoiceDetailRepository.Remove(invoiceDetail);
            return true;
        }
    }
}
