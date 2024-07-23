using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.BookEntryDetail;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.BookEntryDetail;
using BookManagementSystem.Application.Validators;
using FluentValidation;
using FluentValidation.Results;

namespace BookManagementSystem.Application.Services
{
    public class BookEntryDetailService : IBookEntryDetailService
    {
        private readonly IBookEntryDetailRepository _bookEntryDetailRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateBookEntryDetailDto> _createValidator;
        private readonly IValidator<UpdateBookEntryDetailDto> _updateValidator;

        public BookEntryDetailService(
            IBookEntryDetailRepository bookEntryDetailRepository, 
            IMapper mapper, 
            IValidator<CreateBookEntryDetailDto> createValidator,
            IValidator<UpdateBookEntryDetailDto> updateValidator)
        {
            _bookEntryDetailRepository = bookEntryDetailRepository ?? throw new ArgumentNullException(nameof(bookEntryDetailRepository));
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
        }

        public async Task<BookEntryDetailDto> CreateNewBookEntryDetail(CreateBookEntryDetailDto createBookEntryDetailDto)
        {
            var validationResult = await _createValidator.ValidateAsync(createBookEntryDetailDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            var bookEntryDetail = _mapper.Map<BookEntryDetail>(createBookEntryDetailDto);
            await _bookEntryDetailRepository.AddAsync(bookEntryDetail);
            return _mapper.Map<BookEntryDetailDto>(bookEntryDetail);
        }

        public async Task<BookEntryDetailDto> UpdateBookEntryDetail(string EntryID, string BookID, UpdateBookEntryDetailDto updateBookEntryDetailDto)
        {
            var validationResult = await _updateValidator.ValidateAsync(updateBookEntryDetailDto);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            // write again GetByIdAsync
            // var existingDetail = await _bookEntryDetailRepository.GetByIdAsync(EntryID, BookID);
            var existingDetail = await _bookEntryDetailRepository.GetByIdAsync(EntryID);

            if (existingDetail == null)
            {
                throw new KeyNotFoundException($"BookEntryDetail with EntryID {EntryID} and BookID {BookID} not found.");
            }

            _mapper.Map(updateBookEntryDetailDto, existingDetail);

            // write again UpdateAsync
            // var updatedDetail = await _bookEntryDetailRepository.UpdateAsync(EntryID, BookID, existingDetail);
            var updatedDetail = await _bookEntryDetailRepository.UpdateAsync(EntryID, existingDetail);

            return _mapper.Map<BookEntryDetailDto>(updatedDetail);
        }

        public async Task<BookEntryDetailDto> GetBookEntryDetailById(string EntryID, string BookID)
        {
            // var bookEntryDetail = await _bookEntryDetailRepository.GetByIdAsync(EntryID, BookID);
            var bookEntryDetail = await _bookEntryDetailRepository.GetByIdAsync(EntryID);
            if (bookEntryDetail == null)
                throw new KeyNotFoundException($"BookEntryDetail with EntryID {EntryID} and BookID {BookID} not found.");
            
            return _mapper.Map<BookEntryDetailDto>(bookEntryDetail);
        }



        public async Task<bool> DeleteBookEntryDetail(string EntryID, string BookID)
        {
            // var bookEntryDetail = await _bookEntryDetailRepository.GetByIdAsync(EntryID, BookID);
            var bookEntryDetail = await _bookEntryDetailRepository.GetByIdAsync(EntryID);
            if (bookEntryDetail == null)
                return false;
        
            _bookEntryDetailRepository.Remove(bookEntryDetail);
            return true;
        }
    }
}