using BookManagementSystem.Application.Dtos.PaymentReceipt;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Wrappers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BookManagementSystem.Application.Filter;
using BookManagementSystem.Helpers;
using BookManagementSystem.Application.Queries;

namespace BookManagementSystem.Api.Controllers
{
    [Authorize]
    [Route("api/payment-receipt")]
    [ApiController]
    public class PaymentReceiptController : ControllerBase
    {
        private readonly IPaymentReceiptService _paymentReceiptService;
        private readonly IValidator<CreatePaymentReceiptDto> _createPaymentReceiptValidator;
        private readonly IValidator<UpdatePaymentReceiptDto> _updatePaymentReceiptValidator;
        private readonly IUriService _uriService;
        public PaymentReceiptController(
            IPaymentReceiptService paymentReceiptService,
            IValidator<CreatePaymentReceiptDto> createPaymentReceiptValidator,
            IValidator<UpdatePaymentReceiptDto> updatePaymentReceiptValidator,
            IUriService uriService)
        {
            _paymentReceiptService = paymentReceiptService;
            _createPaymentReceiptValidator = createPaymentReceiptValidator;
            _updatePaymentReceiptValidator = updatePaymentReceiptValidator;
            _uriService = uriService;
        }

        [HttpPost]
        // [Authorize(Roles = "Cashier")]
        [Authorize(Roles = "Manager, Cashier")]
        public async Task<IActionResult> CreateNewPaymentReceipt(CreatePaymentReceiptDto createPaymentReceiptDto)
        {
            var validateResult = await _createPaymentReceiptValidator.ValidateAsync(createPaymentReceiptDto);

            if (!validateResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));
            }

            var createdPaymentReceipt = await _paymentReceiptService.CreateNewPaymentReceipt(createPaymentReceiptDto);

            return Ok(new Response<PaymentReceiptDto>(createdPaymentReceipt));
        }

        [HttpPut("{receiptID}")]
        // [Authorize(Roles = "Cashier")]
        [Authorize(Roles = "Manager, Cashier")]
        public async Task<IActionResult> UpdatePaymentReceipt([FromRoute] int receiptID, UpdatePaymentReceiptDto updatePaymentReceiptDto)
        {
            var validateResult = await _updatePaymentReceiptValidator.ValidateAsync(updatePaymentReceiptDto);

            if (!validateResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));
            }

            var existingPaymentReceipt = await _paymentReceiptService.GetPaymentReceiptById(receiptID);
            if (existingPaymentReceipt == null)
            {
                return NotFound($"Payment receipt with ID {receiptID} not found.");
            }

            try
            {
                var updatedPaymentReceipt = await _paymentReceiptService.UpdatePaymentReceipt(receiptID, updatePaymentReceiptDto);
                return Ok(new Response<PaymentReceiptDto>(updatedPaymentReceipt));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the payment receipt: {ex.Message}");
            }
        }

        [HttpGet]
        // [Authorize(Roles = "Cashier")]
        [Authorize(Roles = "Manager, Cashier")]
        public async Task<IActionResult> GetAllPaymentReceipts([FromQuery] PaymentReceiptQuery paymentReceiptQuery)
        {
            var paymentReceipts = await _paymentReceiptService.GetAllPaymentReceipts(paymentReceiptQuery);
            var totalRecords = paymentReceipts != null ? paymentReceipts.Count() : 0;
            var validFilter = new PaginationFilter(paymentReceiptQuery.PageNumber, paymentReceiptQuery.PageSize);
            var pagedPaymentReceipts = paymentReceipts.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var pagedResponse = PaginationHelper.CreatePagedResponse(pagedPaymentReceipts, validFilter, totalRecords, _uriService, Request.Path.Value);
            return Ok(pagedResponse);
        }

        [HttpGet("{receiptID}")]
        // [Authorize(Roles = "Cashier")]
        [Authorize(Roles = "Manager, Cashier")]
        public async Task<IActionResult> GetPaymentReceiptById([FromRoute] int receiptID)
        {
            var paymentReceipt = await _paymentReceiptService.GetPaymentReceiptById(receiptID);

            if (paymentReceipt == null) return NotFound();

            return Ok(new Response<PaymentReceiptDto>(paymentReceipt));
        }

        [HttpDelete("{receiptID}")]
        // [Authorize(Roles = "Cashier")]
        [Authorize(Roles = "Manager, Cashier")]
        public async Task<IActionResult> DeletePaymentReceipt([FromRoute] int receiptID)
        {
            var result = await _paymentReceiptService.DeletePaymentReceipt(receiptID);

            if (!result) return NotFound();

            return NoContent();
        }
        
        [HttpGet("getAmountByMonthAndYear")]
        // [Authorize(Roles = "Cashier")]
        [Authorize(Roles = "Manager, Cashier")]
        public async Task<IActionResult> GetTotalAmountByMonthYear([FromQuery] int month, [FromQuery] int year)
        {
            int totalAmount = await _paymentReceiptService.GetTotalAmountByMonthYear(month, year);
            return Ok(totalAmount);
        }

        [HttpGet("getAmountByYear")]
        // [Authorize(Roles = "Cashier")]
        [Authorize(Roles = "Manager, Cashier")]
        public async Task<IActionResult> GetTotalAmountByYear([FromQuery] int year)
        {
            int totalAmount = await _paymentReceiptService.GetTotalAmountByOnlyYear(year);
            return Ok(totalAmount);
        }
    }
}