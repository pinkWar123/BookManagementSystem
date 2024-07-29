using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.Book;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Validators;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.Book;
using FluentValidation;

namespace BookManagementSystem.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBookValidator> _createValidator;
        private readonly IValidator<UpdateBookValidator> _updateValidator;


        public BookService(
            IBookRepository bookRepository,
            IMapper mapper,
            IValidator<CreateBookValidator> _createValidator,
            IValidator<UpdateBookValidator> _updateValidator)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            this._createValidator = _createValidator;
            this._updateValidator = _updateValidator;
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
            var validationResult = await _createValidator.ValidateAsync((IValidationContext)updateBookDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

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
