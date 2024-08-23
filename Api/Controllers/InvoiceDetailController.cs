using BookManagementSystem.Application.Dtos.InvoiceDetail;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Wrappers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookManagementSystem.Application.Queries;
using BookManagementSystem.Application.Filter;
using BookManagementSystem.Helpers;

namespace BookManagementSystem.Api.Controllers
{
    [Authorize]
    [Route("api/invoice-details")]
    [ApiController]
    public class InvoiceDetailController : ControllerBase
    {
        private readonly IInvoiceDetailService _invoiceDetailService;
        private readonly IValidator<CreateInvoiceDetailDto> _createInvoiceDetailValidator;
        private readonly IValidator<UpdateInvoiceDetailDto> _updateInvoiceDetailValidator;
        private readonly IUriService _uriService;
        public InvoiceDetailController(
            IInvoiceDetailService invoiceDetailService,
            IValidator<CreateInvoiceDetailDto> createInvoiceDetailValidator,
            IValidator<UpdateInvoiceDetailDto> updateInvoiceDetailValidator,
            IUriService uriService)
        {
            _invoiceDetailService = invoiceDetailService;
            _createInvoiceDetailValidator = createInvoiceDetailValidator;
            _updateInvoiceDetailValidator = updateInvoiceDetailValidator;
            _uriService = uriService;
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Cashier")]
        public async Task<IActionResult> CreateNewInvoiceDetailAsync(CreateInvoiceDetailDto createInvoiceDetailDto)
        {
            var validationResult = await _createInvoiceDetailValidator.ValidateAsync(createInvoiceDetailDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validationResult.ToDictionary()));
            }

            var createdInvoiceDetail = await _invoiceDetailService.CreateNewInvoiceDetail(createInvoiceDetailDto);
            return Ok(new Response<InvoiceDetailDto>(createdInvoiceDetail));
        }

        [HttpPut("{entryID}/{bookID}")]
        [Authorize(Roles = "Manager, Cashier")]
        public async Task<IActionResult> UpdateInvoiceDetailAsync([FromRoute] int entryID, [FromRoute] int bookID, UpdateInvoiceDetailDto updateInvoiceDetailDto)
        {
            var validationResult = await _updateInvoiceDetailValidator.ValidateAsync(updateInvoiceDetailDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validationResult.ToDictionary()));
            }

            var existingInvoiceDetail = await _invoiceDetailService.GetInvoiceDetailById(entryID, bookID);
            if (existingInvoiceDetail == null)
            {
                return NotFound($"Invoice detail with EntryID {entryID} and BookID {bookID} not found.");
            }

            try
            {
                var updatedInvoiceDetail = await _invoiceDetailService.UpdateInvoiceDetail(entryID, bookID, updateInvoiceDetailDto);
                return Ok(new Response<InvoiceDetailDto>(updatedInvoiceDetail));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the invoice detail: {ex.Message}");
            }
        }

        [HttpGet("{entryID}/{bookID}")]
        [Authorize(Roles = "Manager, Cashier")]
        public async Task<IActionResult> GetInvoiceDetailByIdAsync([FromRoute] int entryID, [FromRoute] int bookID)
        {
            var invoiceDetail = await _invoiceDetailService.GetInvoiceDetailById(entryID, bookID);
            if (invoiceDetail == null)
            {
                return NotFound();
            }

            return Ok(new Response<InvoiceDetailDto>(invoiceDetail));
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Cashier")]
        public async Task<IActionResult> GetAllInvoiceDetailsAsync([FromQuery] InvoiceDetailQuery query)
        {
            var invoiceDetails = await _invoiceDetailService.GetAllInvoiceDetail(query);
            var totalRecords = invoiceDetails != null ? invoiceDetails.Count() : 0;
            
            var validFilter = new PaginationFilter(query.PageNumber, query.PageSize);
            var pagedInvoiceDetails = invoiceDetails.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var pagedResponse = PaginationHelper.CreatePagedResponse(pagedInvoiceDetails, validFilter, totalRecords, _uriService, Request.Path.Value);
            return Ok(pagedResponse); 
        }


        [HttpDelete("{entryID}/{bookID}")]
        [Authorize(Roles = "Manager, Cashier")]
        public async Task<IActionResult> DeleteInvoiceDetailAsync([FromRoute] int entryID, [FromRoute] int bookID)
        {
            var result = await _invoiceDetailService.DeleteInvoiceDetail(entryID, bookID);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
