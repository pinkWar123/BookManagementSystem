using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.BookEntry;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.BookEntry;
using BookManagementSystem.Application.Validators;
using FluentValidation;
using FluentValidation.Results;

namespace BookManagementSystem.Application.Services
{

    public class BookEntryService : IBookEntryService
    {
         private readonly IBookEntryRepository _bookEntryRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBookEntryDto> _createValidator;
        private readonly IValidator<UpdateBookEntryDto> _updateValidator;

        public BookEntryService(
            IBookEntryRepository bookEntryRepository, 
            IMapper mapper, 
            IValidator<CreateBookEntryDto> createValidator,
            IValidator<UpdateBookEntryDto> updateValidator)
        {
            _bookEntryRepository = bookEntryRepository ?? throw new ArgumentNullException(nameof(bookEntryRepository));
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<BookEntryDto> CreateNewBookEntry(CreateBookEntryDto createBookEntryDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createBookEntryDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var bookEntry = _mapper.Map<Domain.Entities.BookEntry>(createBookEntryDto);
            await _bookEntryRepository.AddAsync(bookEntry);
            return _mapper.Map<BookEntryDto>(bookEntry);
        }

        public async Task<BookEntryDto> UpdateBookEntry(int EntryID, UpdateBookEntryDto updateBookEntryDto)
        {   
            
            var validationResult = await _updateValidator.ValidateAsync(updateBookEntryDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            var existingEntry = await _bookEntryRepository.GetByIdAsync(EntryID);
            if (existingEntry == null)
            {
                throw new KeyNotFoundException($"BookEntry with ID {EntryID} not found.");
            }

            _mapper.Map(updateBookEntryDto, existingEntry);
            var updatedEntry = await _bookEntryRepository.UpdateAsync(EntryID, existingEntry);
            return _mapper.Map<BookEntryDto>(updatedEntry);
        }

        public async Task<BookEntryDto> GetBookEntryById(int EntryID)
        {
            
            var bookEntry = await _bookEntryRepository.GetByIdAsync(EntryID);
            if (bookEntry == null)
            {
                throw new KeyNotFoundException($"BookEntry with ID {EntryID} not found.");
            }
            return _mapper.Map<BookEntryDto>(bookEntry);
        }

       

        public async Task<bool> DeleteBookEntry(int EntryID)
        {
            var bookEntry = await _bookEntryRepository.GetByIdAsync(EntryID);
            if (bookEntry == null)
            {
                return false;
            }
            _bookEntryRepository.Remove(bookEntry);
            return true;
        }
    }
}