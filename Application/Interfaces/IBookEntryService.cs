using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Book;
using BookManagementSystem.Application.Dtos.BookEntry;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IBookEntryService
    {
        Task<BookEntryDto> CreateNewBookEntry(CreateBookEntryDto createBookEntryDto);
        Task<BookEntryDto> UpdateBookEntry(string EntryID, UpdateBookEntryDto updateBookEntryDto);
        Task<BookEntryDto> GetBookEntryById(string EntryID);
        // Task<IEnumerable<BookEntryDto>> GetAllBookEntry();
        Task<bool> DeleteBookEntryDto(string EntryID);
    }
}