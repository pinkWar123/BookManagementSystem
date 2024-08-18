using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Book;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Application.Queries;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IBookService
    {
        Task<BookDto> CreateBook(CreateBookDto createBookDto);
        Task<BookDto> UpdateBook(int BookId, UpdateBookDto updateBookDto);
        Task<BookDto> GetBookById(int BookId);
        Task<bool> DeleteBook(int BookId);
        Task<bool> CheckBookExists(int bookId);

        Task<IEnumerable<BookDto>> GetallBook(BookQuery bookQuery);
        Task<List<int>> GetAllBookId();
    }
}
