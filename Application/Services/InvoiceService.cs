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
using BookManagementSystem.Infrastructure.Repositories.Customer;
using BookManagementSystem.Infrastructure.Repositories.InvoiceDetail;
using System.Runtime.InteropServices;

namespace BookManagementSystem.Application.Services
{

    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IInvoiceDetailRepository _invoiceDetailRepository;
        private readonly IMapper _mapper;
        public InvoiceService(
            IInvoiceRepository invoiceRepository, 
            ICustomerRepository customerRepository,
            IInvoiceDetailRepository invoiceDetailRepository,
            IMapper mapper, 
            IValidator<CreateInvoiceDto> createValidator,
            IValidator<UpdateInvoiceDto> updateValidator)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
            _invoiceDetailRepository = invoiceDetailRepository ?? throw new ArgumentNullException(nameof(invoiceDetailRepository));
            _mapper = mapper;
        }

        public async Task<InvoiceDto> CreateNewInvoice(CreateInvoiceDto createInvoiceDto)
        {
            if (createInvoiceDto == null)
                throw new ArgumentNullException(nameof(createInvoiceDto));
            var invoice = _mapper.Map<Invoice>(createInvoiceDto);
            Console.WriteLine("InvoiceID: " + invoice.Id);
            var customer = await _customerRepository.GetByIdAsync(invoice.CustomerID);
            if (customer == null)
                throw new CustomerException($"Customer not found with ID {invoice.CustomerID}");
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