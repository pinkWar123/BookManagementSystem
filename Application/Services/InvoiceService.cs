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
using BookManagementSystem.Application.Queries;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Application.Services
{

    public class InvoiceService : IInvoiceService
    {
         private readonly IInvoiceRepository _invoiceRepository;
        private readonly IMapper _mapper;
        public InvoiceService(
            IInvoiceRepository invoiceRepository, 
            IMapper mapper, 
            IValidator<CreateInvoiceDto> createValidator,
            IValidator<UpdateInvoiceDto> updateValidator)
        {
            _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
            _mapper = mapper;
        }

        public async Task<InvoiceDto> CreateNewInvoice(CreateInvoiceDto createInvoiceDto)
        {
            var invoice = _mapper.Map<Domain.Entities.Invoice>(createInvoiceDto);
            await _invoiceRepository.AddAsync(invoice);
            await _invoiceRepository.SaveChangesAsync();
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