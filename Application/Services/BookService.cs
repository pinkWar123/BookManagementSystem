using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Exceptions;
using BookManagementSystem.Application.Dtos.Book;
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
            var book = _mapper.Map<Book>(createBookDto);

            if (await _bookRepository.BookExistsAsync(createBookDto.Title, createBookDto.Genre, createBookDto.Author))
            {
                throw new BookExisted(createBookDto.Title, createBookDto.Author, createBookDto.Genre);
            }

            await _bookRepository.AddAsync(book);
            
            await _bookRepository.SaveChangesAsync();

            return _mapper.Map<BookDto>(book);
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
