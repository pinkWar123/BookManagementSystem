using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.InvoiceDetail;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.InvoiceDetail;
using BookManagementSystem.Infrastructure.Repositories.Book;
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
        private readonly IBookRepository _bookRepository;
      

        public InvoiceDetailService(
            IInvoiceDetailRepository invoiceDetailRepository, 
            IBookRepository bookRepository,
            IMapper mapper)
        {
            _invoiceDetailRepository = invoiceDetailRepository ?? throw new ArgumentNullException(nameof(invoiceDetailRepository));
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            _mapper = mapper;
            
        }

        public async Task<InvoiceDetailDto> CreateNewInvoiceDetail(CreateInvoiceDetailDto createInvoiceDetailDto)
        {
           
            var invoiceDetail = _mapper.Map<InvoiceDetail>(createInvoiceDetailDto);
            int BookID = createInvoiceDetailDto.BookID;
            var book = await _bookRepository.GetByIdAsync(BookID);
            if (book == null)
            {
                // throw new ($"Không tìm thấy sách với ID {BookID}"); wtf book Exception dau
                throw new Exception($"Không tìm thấy sách với ID {BookID}");
            }
            invoiceDetail.Price = book.Price * invoiceDetail.Quantity;
            await _invoiceDetailRepository.AddAsync(invoiceDetail);
            await _invoiceDetailRepository.SaveChangesAsync();
            return _mapper.Map<InvoiceDetailDto>(invoiceDetail);
        }

        public async Task<InvoiceDetailDto> UpdateInvoiceDetail(int InvoiceID, int BookID, UpdateInvoiceDetailDto updateInvoiceDetailDto)
        {
            var book = await _bookRepository.GetByIdAsync(BookID);
            if (book == null)
            {
                throw new InvoiceDetailException($"Không tìm thấy sách với ID {BookID}");
            }

            var existingDetail = await _invoiceDetailRepository.GetByIdAsync(InvoiceID, BookID);
            if (existingDetail == null)
            {
                throw new InvoiceDetailException($"Không tìm thấy chi tiết hóa đơn với InvoiceID {InvoiceID} và BookID {BookID}");
            }
            existingDetail.Price = book.Price * updateInvoiceDetailDto.Quantity;

            
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
                throw new InvoiceDetailException($"Không tìm thấy chi tiết hóa đơn với InvoiceID {InvoiceID} và BookID {BookID}");
        
            _invoiceDetailRepository.Remove(invoiceDetail);
            await _invoiceDetailRepository.SaveChangesAsync();
            return true;
        }
    }
}
