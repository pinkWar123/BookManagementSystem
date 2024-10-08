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
using BookManagementSystem.Application.Queries;
using BookManagementSystem.Application.Filter;
using BookManagementSystem.Helpers;

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
        private readonly IUriService _uriService;

        public BookController(
            IBookService bookService,
            IValidator<CreateBookDto> _createbookvalidator,
            IValidator<UpdateBookDto> _updatebookvalidator,
            IUriService _uriService
            )
        {
            this._createvalidator = _createbookvalidator;
            this._updatevalidator = _updatebookvalidator;
            _bookService = bookService;
            this._uriService = _uriService;
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
        [Authorize(Roles = "Manager,StoreKeeper")]
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
        [AllowAnonymous]
        public async Task<IActionResult> GetBookById([FromRoute] int id)
        {
            var book = await _bookService.GetBookById(id);

            if (book == null) return NotFound();

            return Ok(new Application.Wrappers.Response<BookDto>(book));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager,StoreKeeper")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            var result = await _bookService.DeleteBook(id);

            if (!result) return NotFound();

            return Ok("delete Successfully");
        }

        [HttpGet()]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllBooks([FromQuery] BookQuery bookQuery)
        {
            var books = await _bookService.GetallBook(bookQuery);
            var totalRecords = books != null ? books.Count() : 0;
            var validFilter = new PaginationFilter(bookQuery.PageNumber, bookQuery.PageSize);
            var pagedbooks = books.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var pagedResponse = PaginationHelper.CreatePagedResponse(pagedbooks, validFilter, totalRecords, _uriService, Request.Path.Value);
            return Ok(pagedResponse);
        }
    }
}
