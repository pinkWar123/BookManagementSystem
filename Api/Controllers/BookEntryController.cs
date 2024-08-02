using BookManagementSystem.Application.Dtos.BookEntry;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Wrappers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BookManagementSystem.Application.Queries;
using BookManagementSystem.Application.Filter;
using BookManagementSystem.Helpers;

namespace BookManagementSystem.Api.Controllers
{
    [Authorize]
    [Route("api/book-entries")]
    [ApiController]
    public class BookEntryController : ControllerBase
    {
        private readonly IBookEntryService _bookEntryService;
        private readonly IValidator<CreateBookEntryDto> _createBookEntryValidator;
        private readonly IValidator<UpdateBookEntryDto> _updateBookEntryValidator;
        private readonly IUriService _uriService;
        public BookEntryController(
            IBookEntryService bookEntryService,
            IValidator<CreateBookEntryDto> createBookEntryValidator,
            IValidator<UpdateBookEntryDto> updateBookEntryValidator,
            IUriService uriService)
        {
            _bookEntryService = bookEntryService;
            _createBookEntryValidator = createBookEntryValidator;
            _updateBookEntryValidator = updateBookEntryValidator;
            _uriService = uriService;
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateNewBookEntryAsync(CreateBookEntryDto createBookEntryDto)
        {
            var validateResult = await _createBookEntryValidator.ValidateAsync(createBookEntryDto);

            if (!validateResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));
            }

            var createdBookEntry = await _bookEntryService.CreateNewBookEntry(createBookEntryDto);

            return Ok(new Response<BookEntryDto>(createdBookEntry));
        }

        [HttpPut("{entryId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateBookEntryAsync([FromRoute] int entryId, UpdateBookEntryDto updateBookEntryDto)
        {
            var validateResult = await _updateBookEntryValidator.ValidateAsync(updateBookEntryDto);

            if (!validateResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));
            }

            var existingBookEntry = await _bookEntryService.GetBookEntryById(entryId);
            if (existingBookEntry == null)
            {
                return NotFound($"Book entry with ID {entryId} not found.");
            }

            try
            {
                var updatedBookEntry = await _bookEntryService.UpdateBookEntry(entryId, updateBookEntryDto);
                return Ok(new Response<BookEntryDto>(updatedBookEntry));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the book entry: {ex.Message}");
            }
        }

        [HttpGet("{entryId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetBookEntryByIdAsync([FromRoute] int entryId)
        {
            var bookEntry = await _bookEntryService.GetBookEntryById(entryId);

            if (bookEntry == null) return NotFound();

            return Ok(new Response<BookEntryDto>(bookEntry));
        }

        [HttpDelete("{entryId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteBookEntryAsync([FromRoute] int entryId)
        {
            var result = await _bookEntryService.DeleteBookEntry(entryId);

            if (!result) return NotFound();

            return NoContent();
        }
        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllBookEntriesAsync([FromQuery] BookEntryQuery query)
        {
            var bookEntries = await _bookEntryService.GetAllBookEntries(query);
            var totalRecords = bookEntries != null ? bookEntries.Count() : 0;
            
            var validFilter = new PaginationFilter(query.PageNumber, query.PageSize);
            var pagedBookEntry = bookEntries.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var pagedResponse = PaginationHelper.CreatePagedResponse(pagedBookEntry, validFilter, totalRecords, _uriService, Request.Path.Value);
            return Ok(pagedResponse); 
        }
    }
}
