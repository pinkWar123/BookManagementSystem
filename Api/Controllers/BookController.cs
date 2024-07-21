using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Book;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Validators;
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
        [HttpPost]
        public async Task<IActionResult> CreateNewBook(CreateBookDto createBookDto)
        {
            var validator = new CreateBookValidator();
            var validateResult = await validator.ValidateAsync(createBookDto);
            if (!validateResult.IsValid)
            {
                return BadRequest(validateResult);
            }

            await _bookService.CreateNewBook(createBookDto);
            return Ok(createBookDto);
        }

        // [HttpPatch]
        // public async Task<IActionResult> UpdateBookById(UpdateBookDto createBookDto)
        // {
        //     var validator = new CreateBookValidator();
        //     var validateResult = await validator.ValidateAsync(createBookDto);
        //     if(!validateResult.IsValid)
        //     {
        //         return BadRequest(validateResult);
        //     }

        //     await _bookService.CreateNewBook(createBookDto);
        //     return Ok(createBookDto);
        // }
    }
}
