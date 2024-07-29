using BookManagementSystem.Application.Dtos.Invoice;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Wrappers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BookManagementSystem.Api.Controllers.Invoice
{
    [Authorize]
    [Route("api/invoices")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IValidator<CreateInvoiceDto> _createInvoiceValidator;
        private readonly IValidator<UpdateInvoiceDto> _updateInvoiceValidator;

        public InvoiceController(
            IInvoiceService invoiceService,
            IValidator<CreateInvoiceDto> createInvoiceValidator,
            IValidator<UpdateInvoiceDto> updateInvoiceValidator)
        {
            _invoiceService = invoiceService;
            _createInvoiceValidator = createInvoiceValidator;
            _updateInvoiceValidator = updateInvoiceValidator;
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateNewInvoiceAsync(CreateInvoiceDto createInvoiceDto)
        {
            var validateResult = await _createInvoiceValidator.ValidateAsync(createInvoiceDto);

            if (!validateResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));
            }

            var createdInvoice = await _invoiceService.CreateNewInvoice(createInvoiceDto);

            return Ok(new Response<InvoiceDto>(createdInvoice));
        }

        [HttpPut("{invoiceId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateInvoiceAsync([FromRoute] int invoiceId, UpdateInvoiceDto updateInvoiceDto)
        {
            var validateResult = await _updateInvoiceValidator.ValidateAsync(updateInvoiceDto);

            if (!validateResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));
            }

            var existingInvoice = await _invoiceService.GetInvoiceById(invoiceId);
            if (existingInvoice == null)
            {
                return NotFound($"Invoice with ID {invoiceId} not found.");
            }

            try
            {
                var updatedInvoice = await _invoiceService.UpdateInvoice(invoiceId, updateInvoiceDto);
                return Ok(new Response<InvoiceDto>(updatedInvoice));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the invoice: {ex.Message}");
            }
        }

        [HttpGet("{invoiceId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetInvoiceByIdAsync([FromRoute] int invoiceId)
        {
            var invoice = await _invoiceService.GetInvoiceById(invoiceId);

            if (invoice == null) return NotFound();

            return Ok(new Response<InvoiceDto>(invoice));
        }

        [HttpDelete("{invoiceId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteInvoiceAsync([FromRoute] int invoiceId)
        {
            var result = await _invoiceService.DeleteInvoice(invoiceId);

            if (!result) return NotFound();

            return NoContent();
        }
    }
}
