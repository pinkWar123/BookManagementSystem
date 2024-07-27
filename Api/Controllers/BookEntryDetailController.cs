using BookManagementSystem.Application.Dtos.BookEntryDetail;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Wrappers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementSystem.Api.Controllers
{
    [Authorize]
    [Route("api/book-entry-details")]
    [ApiController]
    public class BookEntryDetailController : ControllerBase
    {
        private readonly IBookEntryDetailService _bookEntryDetailService;
        private readonly IValidator<CreateBookEntryDetailDto> _createBookEntryDetailValidator;
        private readonly IValidator<UpdateBookEntryDetailDto> _updateBookEntryDetailValidator;

        public BookEntryDetailController(
            IBookEntryDetailService bookEntryDetailService,
            IValidator<CreateBookEntryDetailDto> createBookEntryDetailValidator,
            IValidator<UpdateBookEntryDetailDto> updateBookEntryDetailValidator)
        {
            _bookEntryDetailService = bookEntryDetailService;
            _createBookEntryDetailValidator = createBookEntryDetailValidator;
            _updateBookEntryDetailValidator = updateBookEntryDetailValidator;
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateNewBookEntryDetailAsync(CreateBookEntryDetailDto createBookEntryDetailDto)
        {
            var validationResult = await _createBookEntryDetailValidator.ValidateAsync(createBookEntryDetailDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validationResult.ToDictionary()));
            }

            var createdBookEntryDetail = await _bookEntryDetailService.CreateNewBookEntryDetail(createBookEntryDetailDto);
            return Ok(new Response<BookEntryDetailDto>(createdBookEntryDetail));
        }

        [HttpPut("{entryID}/{bookID}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateBookEntryDetailAsync([FromRoute] int entryID, [FromRoute] int bookID, UpdateBookEntryDetailDto updateBookEntryDetailDto)
        {
            var validationResult = await _updateBookEntryDetailValidator.ValidateAsync(updateBookEntryDetailDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validationResult.ToDictionary()));
            }

            var existingBookEntryDetail = await _bookEntryDetailService.GetBookEntryDetailById(entryID, bookID);
            if (existingBookEntryDetail == null)
            {
                return NotFound($"Book entry detail with EntryID {entryID} and BookID {bookID} not found.");
            }

            try
            {
                var updatedBookEntryDetail = await _bookEntryDetailService.UpdateBookEntryDetail(entryID, bookID, updateBookEntryDetailDto);
                return Ok(new Response<BookEntryDetailDto>(updatedBookEntryDetail));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the book entry detail: {ex.Message}");
            }
        }

        [HttpGet("{entryID}/{bookID}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetBookEntryDetailByIdAsync([FromRoute] int entryID, [FromRoute] int bookID)
        {
            var bookEntryDetail = await _bookEntryDetailService.GetBookEntryDetailById(entryID, bookID);
            if (bookEntryDetail == null)
            {
                return NotFound();
            }

            return Ok(new Response<BookEntryDetailDto>(bookEntryDetail));
        }

        [HttpDelete("{entryID}/{bookID}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteBookEntryDetailAsync([FromRoute] int entryID, [FromRoute] int bookID)
        {
            var result = await _bookEntryDetailService.DeleteBookEntryDetail(entryID, bookID);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
