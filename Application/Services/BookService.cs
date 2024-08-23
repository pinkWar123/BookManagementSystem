using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Exceptions;
using BookManagementSystem.Application.Dtos.Book;
using BookManagementSystem.Application.Dtos.InventoryReportDetail;
using BookManagementSystem.Application.Dtos.InventoryReport;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Queries;
using BookManagementSystem.Application.Validators;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.Book;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IInventoryReportDetailService _inventoryReportDetailService;
        private readonly IInventoryReportService _inventoryReportService;
        public BookService(
            IBookRepository bookRepository,
            IInventoryReportDetailService inventoryReportDetailService,
            IInventoryReportService inventoryReportService,
            IMapper mapper
)
        {
            _bookRepository = bookRepository;
            _inventoryReportDetailService = inventoryReportDetailService;
            _inventoryReportService = inventoryReportService;
            _mapper = mapper;
        }

        public async Task<bool> CheckBookExists(int bookId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            return book != null;
        }

        public async Task<BookDto> CreateBook(CreateBookDto createBookDto)
        {
            var booktemp = _mapper.Map<Book>(createBookDto);

            if (await _bookRepository.BookExistsAsync(createBookDto.Title, createBookDto.Genre, createBookDto.Author))
            {
                throw new BookExisted(createBookDto.Title, createBookDto.Author, createBookDto.Genre);
            }

            await _bookRepository.AddAsync(booktemp);
            var Reportid = await _inventoryReportService.GetReportIdByMonthYear(DateTime.Now.Month, DateTime.Now.Year);
            
            if(Reportid == -1)
            {
                var now = DateTime.Now;
                var createInventoryReportDto = new CreateInventoryReportDto
                {
                    ReportMonth = now.Month,
                    ReportYear = now.Year
                };
                var report = await _inventoryReportService.CreateInventoryReport(createInventoryReportDto);
                if(report != null)
                {
                    Reportid = report.ReportID;
                }
            }
            
            await _bookRepository.SaveChangesAsync();

            var book = _mapper.Map<BookDto>(booktemp);
            
            // create inventory report detail for this book
            var newInventoryReportDetail = new CreateInventoryReportDetailDto {
                ReportID = Reportid,
                BookID = booktemp.Id,
                InitialStock = 0,
                FinalStock = 0,
                AdditionalStock = 0,
            };
            await _inventoryReportDetailService.CreateInventoryReportDetail(newInventoryReportDetail);
            return book;
        }


        public async Task<bool> DeleteBook(int BookId)
        {
            var book = await _bookRepository.GetByIdAsync(BookId);

            if (book == null)
            {
                return false;
            }

            _bookRepository.Remove(book);
            await _bookRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BookDto>> GetallBook(BookQuery bookQuery)
        {
            var books = _bookRepository.GetValuesByQuery(bookQuery);

            if (books == null)
            {
                return Enumerable.Empty<BookDto>();
            }

            var temp = await books.ToListAsync();
            return _mapper.Map<IEnumerable<BookDto>>(temp);
        }

        public async Task<List<int>> GetAllBookId()
        {
            return await _bookRepository.GetAllBookId();
        }

        public async Task<BookDto> GetBookById(int BookId)
        {
            var book = await _bookRepository.GetByIdAsync(BookId);

            if (book == null)
            {
                throw new BookNotFound(BookId);
            }

            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> UpdateBook(int BookId, UpdateBookDto updateBookDto)
        {
            var book = await _bookRepository.GetByIdAsync(BookId);
            if (book == null)
            {
                throw new BookNotFound(BookId);
            }
            
            _mapper.Map(updateBookDto, book);
            await _bookRepository.UpdateAsync(BookId, book);
            await _bookRepository.SaveChangesAsync();
            return _mapper.Map<BookDto>(book);

        }


    }
}
