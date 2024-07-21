using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Book;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IBookService
    {
        Task CreateNewBook(CreateBookDto createBookDto);
        // Task UpdateBookById(UpdateBookDto updateBookDto, string bookId);
    }
}
