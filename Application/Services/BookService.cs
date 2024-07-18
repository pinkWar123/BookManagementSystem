using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.Book;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Infrastructure.Repositories.Book;

namespace BookManagementSystem.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;
        public BookService(IBookRepository bookRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
        }
        // public async Task CreateNewBook(CreateBookDto createBookDto)
        // {
        //     var book = _mapper.Map<Book>(createBookDto);
        //     await _bookRepository.AddAsync(book);
        // }
    }
}