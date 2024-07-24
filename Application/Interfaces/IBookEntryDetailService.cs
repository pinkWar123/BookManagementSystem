using BookManagementSystem.Application.Dtos.BookEntryDetail;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IBookEntryDetailService
    {
        Task<BookEntryDetailDto> CreateNewBookEntryDetail(CreateBookEntryDetailDto createBookEntryDetailDto);
        Task<BookEntryDetailDto> UpdateBookEntryDetail(string EntryID, string BookID, UpdateBookEntryDetailDto updateBookEntryDetailDto);
        Task<BookEntryDetailDto> GetBookEntryDetailById(string EntryID, string BookID);
        // Task<IEnumerable<BookEntryDetailDto>> GetAllBookEntryDetail();
        Task<bool> DeleteBookEntryDetail(string EntryID, string BookID);
    }
}