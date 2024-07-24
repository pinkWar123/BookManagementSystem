using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Book;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IBookService
    {
        Task<BookDto> CreateBook(CreateBookDto createBookDto);
        Task<BookDto> UpdateBook(string BookId, UpdateBookDto updateBookDto);
        Task<BookDto> GetBookById(string BookId);
        Task<bool> DeleteBook(string BookId);

        Task<bool> CheckBookExists(string bookId);
    }
}
