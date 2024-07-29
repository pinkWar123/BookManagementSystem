using BookManagementSystem.Application.Dtos.BookEntryDetail;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IBookEntryDetailService
    {
        Task<BookEntryDetailDto> CreateNewBookEntryDetail(CreateBookEntryDetailDto createBookEntryDetailDto);
        Task<BookEntryDetailDto> UpdateBookEntryDetail(int EntryID, int BookID, UpdateBookEntryDetailDto updateBookEntryDetailDto);
        Task<BookEntryDetailDto> GetBookEntryDetailById(int EntryID, int BookID);
        // Task<IEnumerable<BookEntryDetailDto>> GetAllBookEntryDetail();
        Task<bool> DeleteBookEntryDetail(int EntryID, int BookID);
    }
}