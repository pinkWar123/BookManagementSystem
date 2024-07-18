using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Book;
using BookManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementSystem.Api.Controllers
{
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        // [HttpPost]
        // public async Task<IActionResult> CreateNewBook(CreateBookDto createBookDto)
        // {
        //     // Validate data
        //     await _bookService.CreateNewBook(createBookDto);
        //     return Ok(createBookDto);
        // }

        
    }
}