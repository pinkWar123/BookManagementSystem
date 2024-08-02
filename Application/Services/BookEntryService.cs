using AutoMapper;
using BookManagementSystem.Application.Dtos.BookEntry;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Infrastructure.Repositories.BookEntry;
using FluentValidation;
using BookManagementSystem.Application.Exceptions;
using BookManagementSystem.Application.Queries;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Application.Services
{

    public class BookEntryService : IBookEntryService
    {
        private readonly IBookEntryRepository _bookEntryRepository;
        private readonly IMapper _mapper;
        

        public BookEntryService(
            IBookEntryRepository bookEntryRepository, 
            IMapper mapper)
        {
            _bookEntryRepository = bookEntryRepository ?? throw new ArgumentNullException(nameof(bookEntryRepository));
            _mapper = mapper;
        
        }

        public async Task<BookEntryDto> CreateNewBookEntry(CreateBookEntryDto createBookEntryDto)
        {
            var bookEntry = _mapper.Map<Domain.Entities.BookEntry>(createBookEntryDto);
            await _bookEntryRepository.AddAsync(bookEntry);
            await _bookEntryRepository.SaveChangesAsync();
            return _mapper.Map<BookEntryDto>(bookEntry);
        }

        public async Task<BookEntryDto> UpdateBookEntry(int EntryID, UpdateBookEntryDto updateBookEntryDto)
        {   
            
            var existingEntry = await _bookEntryRepository.GetByIdAsync(EntryID);
            if (existingEntry == null)
            {
                throw new BookEntryException($"Không tìm thấy BookEntry với EntryID {EntryID}.");
            }

            _mapper.Map(updateBookEntryDto, existingEntry);
            var updatedEntry = await _bookEntryRepository.UpdateAsync(EntryID, existingEntry);
            await _bookEntryRepository.SaveChangesAsync();
            return _mapper.Map<BookEntryDto>(updatedEntry);
        }

        public async Task<BookEntryDto> GetBookEntryById(int EntryID)
        {
            
            var bookEntry = await _bookEntryRepository.GetByIdAsync(EntryID);
            if (bookEntry == null)
            {
                throw new BookEntryException($"Không tìm thấy BookEntry với EntryID {EntryID}.");
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
            await _bookEntryRepository.SaveChangesAsync();
            
            return true;
        }
    }
}