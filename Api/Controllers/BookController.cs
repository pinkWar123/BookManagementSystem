using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using BookManagementSystem.Application.Dtos.Book;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using BookManagementSystem.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;

namespace BookManagementSystem.Api.Controllers
{
    [Authorize]
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IValidator<CreateBookDto> _createvalidator;
        private readonly IValidator<UpdateBookDto> _updatevalidator;
        public BookController(
            IBookService bookService,
            IValidator<CreateBookDto> _createbookvalidator,
            IValidator<UpdateBookDto> _updatebookvalidator
            )
        {
            this._createvalidator = _createbookvalidator;
            this._updatevalidator = _updatebookvalidator;
            _bookService = bookService;
        }

        [HttpPost]
        [Authorize(Roles = "Manager,StoreKeeper")]
        public async Task<IActionResult> CreateBook(CreateBookDto createBookDto)
        {
            var validationResult = await _createvalidator.ValidateAsync(createBookDto);
            if (!validationResult.IsValid)
            {
                BadRequest(Results.ValidationProblem(validationResult.ToDictionary()));
            }

            var book = await _bookService.CreateBook(createBookDto);
            return Ok(new Application.Wrappers.Response<BookDto>(book));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook([FromRoute] int id, UpdateBookDto updateBookDto)
        {
            var validateResult = await _updatevalidator.ValidateAsync(updateBookDto);

            if (!validateResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));
            }

            var book = await _bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }
            var updatedbook = await _bookService.UpdateBook(id, updateBookDto);
            return Ok(new Application.Wrappers.Response<BookDto>(updatedbook));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById([FromRoute] int id)
        {
            var book = await _bookService.GetBookById(id);

            if (book == null) return NotFound();

            return Ok(new Application.Wrappers.Response<BookDto>(book));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            var result = await _bookService.DeleteBook(id);

            if (!result) return NotFound();

            return Ok("delete Successfully");
        }
    }
}
