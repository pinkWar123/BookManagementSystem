
using BookManagementSystem.Application.Dtos.BookEntry;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IBookEntryService
    {
        Task<BookEntryDto> CreateNewBookEntry(CreateBookEntryDto createBookEntryDto);
        Task<BookEntryDto> UpdateBookEntry(int EntryID, UpdateBookEntryDto updateBookEntryDto);
        Task<BookEntryDto> GetBookEntryById(int EntryID);
        // Task<IEnumerable<BookEntryDto>> GetAllBookEntry();
        Task<bool> DeleteBookEntry(int EntryID);
    }
}