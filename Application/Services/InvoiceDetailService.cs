using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.InvoiceDetail;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.InvoiceDetail;
using FluentValidation;
using BookManagementSystem.Application.Exceptions;
using BookManagementSystem.Application.Queries;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Application.Services
{
    public class InvoiceDetailService : IInvoiceDetailService
    {
        private readonly IInvoiceDetailRepository _invoiceDetailRepository;
        private readonly IMapper _mapper;
      

        public InvoiceDetailService(
            IInvoiceDetailRepository invoiceDetailRepository, 
            IMapper mapper)
        {
            _invoiceDetailRepository = invoiceDetailRepository ?? throw new ArgumentNullException(nameof(invoiceDetailRepository));
            _mapper = mapper;
            
        }

        public async Task<InvoiceDetailDto> CreateNewInvoiceDetail(CreateInvoiceDetailDto createInvoiceDetailDto)
        {
           

            var invoiceDetail = _mapper.Map<InvoiceDetail>(createInvoiceDetailDto);
            await _invoiceDetailRepository.AddAsync(invoiceDetail);
            await _invoiceDetailRepository.SaveChangesAsync();
            return _mapper.Map<InvoiceDetailDto>(invoiceDetail);
        }

        public async Task<InvoiceDetailDto> UpdateInvoiceDetail(int InvoiceID, int BookID, UpdateInvoiceDetailDto updateInvoiceDetailDto)
        {
            var existingDetail = await _invoiceDetailRepository.GetByIdAsync(InvoiceID, BookID);
            

            if (existingDetail == null)
            {
                throw new InvoiceDetailException($"Không tìm thấy chi tiết hóa đơn với InvoiceID {InvoiceID} và BookID {BookID}");
            }

            _mapper.Map(updateInvoiceDetailDto, existingDetail);

            var updatedDetail = await _invoiceDetailRepository.UpdateAsync(InvoiceID, BookID, existingDetail);
            await _invoiceDetailRepository.SaveChangesAsync();

            return _mapper.Map<InvoiceDetailDto>(updatedDetail);
        }

        public async Task<InvoiceDetailDto> GetInvoiceDetailById(int InvoiceID, int BookID)
        {
            var invoiceDetail = await _invoiceDetailRepository.GetByIdAsync(InvoiceID, BookID);
            if (invoiceDetail == null)
                throw new InvoiceDetailException($"Không tìm thấy chi tiết hóa đơn với InvoiceID {InvoiceID} và BookID {BookID}");
            
            return _mapper.Map<InvoiceDetailDto>(invoiceDetail);
        }

        public async Task<IEnumerable<InvoiceDetailDto>> GetAllInvoiceDetail(InvoiceDetailQuery invoiceDetailQuery)
        {
            var query = _invoiceDetailRepository.GetValuesByQuery(invoiceDetailQuery);
            if (query == null)
            {
                return Enumerable.Empty<InvoiceDetailDto>();
            }

            var invoiceDetails = await query.ToListAsync();

            return _mapper.Map<IEnumerable<InvoiceDetailDto>>(invoiceDetails);
        }
        public async Task<bool> DeleteInvoiceDetail(int InvoiceID, int BookID)
        {
            var invoiceDetail = await _invoiceDetailRepository.GetByIdAsync(InvoiceID, BookID);
            if (invoiceDetail == null)
                return false;
        
            _invoiceDetailRepository.Remove(invoiceDetail);
            await _invoiceDetailRepository.SaveChangesAsync();
            return true;
        }
    }
}
