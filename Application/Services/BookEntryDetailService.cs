using AutoMapper;
using BookManagementSystem.Application.Dtos.BookEntryDetail;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.BookEntryDetail;
using BookManagementSystem.Application.Exceptions;
using BookManagementSystem.Application.Queries;
using Microsoft.EntityFrameworkCore;



namespace BookManagementSystem.Application.Services
{
    public class BookEntryDetailService : IBookEntryDetailService
    {
        private readonly IBookEntryDetailRepository _bookEntryDetailRepository;
        private readonly IMapper _mapper;


        public BookEntryDetailService(
            IBookEntryDetailRepository bookEntryDetailRepository, 
            IMapper mapper)
        {
            _bookEntryDetailRepository = bookEntryDetailRepository ?? throw new ArgumentNullException(nameof(bookEntryDetailRepository));
            _mapper = mapper;
           
        }

        public async Task<BookEntryDetailDto> CreateNewBookEntryDetail(CreateBookEntryDetailDto createBookEntryDetailDto)
        {
            

            var bookEntryDetail = _mapper.Map<BookEntryDetail>(createBookEntryDetailDto);
            await _bookEntryDetailRepository.AddAsync(bookEntryDetail);
            await _bookEntryDetailRepository.SaveChangesAsync();
            return _mapper.Map<BookEntryDetailDto>(bookEntryDetail);
        }

        public async Task<BookEntryDetailDto> UpdateBookEntryDetail(int EntryID, int BookID, UpdateBookEntryDetailDto updateBookEntryDetailDto)
        {
            
            var existingDetail = await _bookEntryDetailRepository.GetByIdAsync(EntryID, BookID);
            if (existingDetail == null)
            {
                throw new BookEntryDetailException($"Không tìm thấy BookEntryDetail với EntryID {EntryID} và BookID {BookID}.");
            }
            _mapper.Map(updateBookEntryDetailDto, existingDetail);

            var updatedDetail = await _bookEntryDetailRepository.UpdateAsync(EntryID, BookID, existingDetail);
            await _bookEntryDetailRepository.SaveChangesAsync();
            return _mapper.Map<BookEntryDetailDto>(updatedDetail);
        }

        public async Task<BookEntryDetailDto> GetBookEntryDetailById(int EntryID, int BookID)
        {
            var bookEntryDetail = await _bookEntryDetailRepository.GetByIdAsync(EntryID, BookID);
            if (bookEntryDetail == null)
                throw new BookEntryDetailException($"Không tìm thấy BookEntryDetail với EntryID {EntryID} và BookID {BookID}.");
            
            return _mapper.Map<BookEntryDetailDto>(bookEntryDetail);
        }



        public async Task<bool> DeleteBookEntryDetail(int EntryID, int BookID)
        {
            var bookEntryDetail = await _bookEntryDetailRepository.GetByIdAsync(EntryID, BookID);
            if (bookEntryDetail == null)
                return false;
        
            _bookEntryDetailRepository.Remove(bookEntryDetail);
            await _bookEntryDetailRepository.SaveChangesAsync();
            return true;
        }
    }
}