using BookManagementSystem.Application.Dtos.BookEntryDetail;
using BookManagementSystem.Application.Queries;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IBookEntryDetailService
    {
        Task<BookEntryDetailDto> CreateNewBookEntryDetail(CreateBookEntryDetailDto createBookEntryDetailDto);
        Task<BookEntryDetailDto> UpdateBookEntryDetail(int EntryID, int BookID, UpdateBookEntryDetailDto updateBookEntryDetailDto);
        Task<BookEntryDetailDto> GetBookEntryDetailById(int EntryID, int BookID);
        Task<IEnumerable<BookEntryDetailDto>> GetAllBookEntryDetail(BookEntryDetailQuery query);
        Task<bool> DeleteBookEntryDetail(int EntryID, int BookID);
    }
}