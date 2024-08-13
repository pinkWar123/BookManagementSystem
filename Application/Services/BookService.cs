using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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


        public BookService(
            IBookRepository bookRepository,
            IMapper mapper
)
        {
            _bookRepository = bookRepository;
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
            await _bookRepository.AddAsync(book);
            await _bookRepository.SaveChangesAsync();
            return _mapper.Map<BookDto>(book);
        }

        public async Task<bool> DeleteBook(int BookId)
        {
            var book = await _bookRepository.GetByIdAsync(BookId);

            if(book == null)
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

            if(books == null)
            {
                return Enumerable.Empty<BookDto>();
            }

            var temp = await books.ToListAsync();
            return _mapper.Map<IEnumerable<BookDto>>(temp);
        }

        public async Task<BookDto> GetBookById(int BookId)
        {
            var book = await _bookRepository.GetByIdAsync(BookId);

            if(book == null)
            {
                 throw new KeyNotFoundException($"Không tìm thấy BookId");
            }

            return _mapper.Map<BookDto>(book);
        }

        public async Task<BookDto> UpdateBook(int BookId, UpdateBookDto updateBookDto)
        {
            var book = await _bookRepository.GetByIdAsync(BookId);
            if(book == null)
            {
                 throw new KeyNotFoundException($"Không tìm thấy BookId, không thể cập nhật");
            }
            _mapper.Map(updateBookDto, book);
            await _bookRepository.UpdateAsync(BookId, book);
            await _bookRepository.SaveChangesAsync();
            return _mapper.Map<BookDto>(book);

        }

        
    }
}
