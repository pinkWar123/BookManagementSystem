using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using BookManagementSystem.Application.Dtos.InventoryReportDetail;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using BookManagementSystem.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
namespace BookManagementSystem.Api.Controllers
{
    [Authorize]
    [Route("api/inventory_report_detail")]
    [ApiController]
    public class InventoryReportDetailController : ControllerBase
    {
        private readonly IInventoryReportDetailService _inventoryReportDetailService;
        private readonly IValidator<CreateInventoryReportDetailDto> _createinventoryReportDetailValidator;
        private readonly IValidator<UpdateInventoryReportDetailDto> _updateinventoryReportDetailValidator;
        public InventoryReportDetailController(
            IInventoryReportDetailService _inventoryReportDetailService,
            IValidator<CreateInventoryReportDetailDto> _createinventoryReportDetailValidator,
            IValidator<UpdateInventoryReportDetailDto> _updateinventoryReportDetailValidator    
        )
        {
            this._inventoryReportDetailService = _inventoryReportDetailService;
            this._createinventoryReportDetailValidator = _createinventoryReportDetailValidator;
            this._updateinventoryReportDetailValidator = _updateinventoryReportDetailValidator;
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateNewDebtReportDetail(CreateInventoryReportDetailDto createinventoryReportDetailDto)
        {
            var validateResult = await _createinventoryReportDetailValidator.ValidateAsync(createinventoryReportDetailDto);

            if (!validateResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));
            }

            var createdinventoryReportDetail = await _inventoryReportDetailService.CreateInventoryReportDetail(createinventoryReportDetailDto);

            return Ok(new Application.Wrappers.Response<InventoryReportDetailDto>(createdinventoryReportDetail));
        }

        [HttpPut("{reportId, bookId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateinventoryReportDetail([FromRoute] int reportId, [FromRoute] int bookId, UpdateInventoryReportDetailDto updateinventoryReportDetailDto)
        {
            var validateResult = await _updateinventoryReportDetailValidator.ValidateAsync(updateinventoryReportDetailDto);

            if (!validateResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));
            }

            var existingInventoryReportDetail = await _inventoryReportDetailService.GetInventoryReportDetailById(reportId, bookId);
            if (existingInventoryReportDetail == null)
            {
                return NotFound($"Debt report detail with report ID {reportId} and customer ID {bookId} not found.");
            }

            try
            {
                var updatedDebtReportDetail = await _inventoryReportDetailService.UpdateInventoryReportDetail(reportId, bookId, updateinventoryReportDetailDto);
                return Ok(new Application.Wrappers.Response<InventoryReportDetailDto>(updatedDebtReportDetail));
            }
            
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the debt report detail: {ex.Message}");
            }
        }

        [HttpGet("{reportId, customerId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetinventoryReportDetailById([FromRoute] int reportId, [FromRoute] int bookId)
        {
            var inventoryReportDetail = await _inventoryReportDetailService.GetInventoryReportDetailById(reportId, bookId);

            if (inventoryReportDetail == null) return NotFound();

            return Ok(new Application.Wrappers.Response<InventoryReportDetailDto>(inventoryReportDetail));
        }

        [HttpDelete("{reportId, customerId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteinventoryReportDetail([FromRoute] int reportId, [FromRoute] int customerId)
        {
            var result = await _inventoryReportDetailService.DeleteInventoryReportDetail(reportId, customerId);

            if (!result) return NotFound();

            return Ok("delete Successfully");
        }
    }
}