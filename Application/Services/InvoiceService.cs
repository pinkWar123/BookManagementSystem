using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.Invoice;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.Invoice;
using BookManagementSystem.Application.Validators;
using FluentValidation;
using FluentValidation.Results;
using BookManagementSystem.Application.Exceptions;

namespace BookManagementSystem.Application.Services
{

    public class InvoiceService : IInvoiceService
    {
         private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateInvoiceDto> _createValidator;
        private readonly IValidator<UpdateInvoiceDto> _updateValidator;

        public InvoiceService(
            IInvoiceRepository invoiceRepository, 
            IMapper mapper, 
            IValidator<CreateInvoiceDto> createValidator,
            IValidator<UpdateInvoiceDto> updateValidator)
        {
            _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<InvoiceDto> CreateNewInvoice(CreateInvoiceDto createInvoiceDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createInvoiceDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var invoice = _mapper.Map<Domain.Entities.Invoice>(createInvoiceDto);
            await _invoiceRepository.AddAsync(invoice);
            await _invoiceRepository.SaveChangesAsync();
            return _mapper.Map<InvoiceDto>(invoice);
        }

        public async Task<InvoiceDto> UpdateInvoice(int  InvoiceID, UpdateInvoiceDto updateInvoiceDto)
        {   
            
            var validationResult = await _updateValidator.ValidateAsync(updateInvoiceDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
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

       

        public async Task<bool> DeleteInvoice(int InvoiceID)
        {
            var invoice = await _invoiceRepository.GetByIdAsync(InvoiceID);
            if (invoice == null)
            {
                return false;
            }
            _invoiceRepository.Remove(invoice);
            await _invoiceRepository.SaveChangesAsync();
            return true;
        }
    }
}