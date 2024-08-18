using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using BookManagementSystem.Application.Dtos.InventoryReportDetail;
using BookManagementSystem.Application.Dtos.BookEntryDetail;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using BookManagementSystem.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using BookManagementSystem.Application.Queries;
using BookManagementSystem.Application.Filter;
using BookManagementSystem.Helpers;
using BookManagementSystem.Application.Exceptions;


namespace BookManagementSystem.Api.Controllers
{
    [Authorize]
    [Route("api/inventory_report_detail")]
    [ApiController]
    public class InventoryReportDetailController : ControllerBase
    {
        private readonly IInventoryReportDetailService _inventoryReportDetailService;
        private readonly IUriService _uriService;
        public InventoryReportDetailController(
            IInventoryReportDetailService _inventoryReportDetailService,
            IUriService _uriService
        )
        {
            this._inventoryReportDetailService = _inventoryReportDetailService;
            this._uriService = _uriService;
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateNewInventoryReportDetail(CreateInventoryReportDetailDto createinventoryReportDetailDto)
        {


            var createdinventoryReportDetail = await _inventoryReportDetailService.CreateInventoryReportDetail(createinventoryReportDetailDto);

            return Ok(new Application.Wrappers.Response<InventoryReportDetailDto>(createdinventoryReportDetail));
        }

        [HttpPut("{reportId}/{bookId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateinventoryReportDetail([FromRoute] int reportId, [FromRoute] int bookId, UpdateInventoryReportDetailDto updateinventoryReportDetailDto)
        {

            Console.WriteLine("Hello World");
            var existingInventoryReportDetail = await _inventoryReportDetailService.GetInventoryReportDetailById(reportId, bookId);
            if (existingInventoryReportDetail == null)
            {
                return BadRequest("Unable to update");
            }
            Console.WriteLine("hehehehhe");
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

        [HttpGet("{reportId}/{BookId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetinventoryReportDetailById([FromRoute] int reportId, [FromRoute] int BookId)
        {
            var inventoryReportDetail = await _inventoryReportDetailService.GetInventoryReportDetailById(reportId, BookId);

            if (inventoryReportDetail == null) return NotFound();

            return Ok(new Application.Wrappers.Response<InventoryReportDetailDto>(inventoryReportDetail));
        }

        [HttpDelete("{reportId}/{BookId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteinventoryReportDetail([FromRoute] int reportId, [FromRoute] int BookId)
        {
            var result = await _inventoryReportDetailService.DeleteInventoryReportDetail(reportId, BookId);

            if (!result) return NotFound();

            return Ok("delete Successfully");
        }

        [HttpGet("All")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllInventoryReportDetails([FromQuery] InventoryReportDetailQuery InventoryReportDetailQuery)
        {
            var InventoryReportDetails = await _inventoryReportDetailService.GetAllInventoryReportDetails(InventoryReportDetailQuery);
            var totalRecords = InventoryReportDetails != null ? InventoryReportDetails.Count() : 0;
            var validFilter = new PaginationFilter(InventoryReportDetailQuery.PageNumber, InventoryReportDetailQuery.PageSize);
            var pagedInventoryReportDetails = InventoryReportDetails.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var pagedResponse = PaginationHelper.CreatePagedResponse(pagedInventoryReportDetails, validFilter, totalRecords, _uriService, Request.Path.Value);
            return Ok(pagedResponse);
        }

        [HttpPut("Book")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateinventoryReportDetail(BookEntryDetailDto bookentrydetail)
        {
            try
            {
                var temp = _inventoryReportDetailService.CreateInventoryFromBookEntry(bookentrydetail);
                Console.WriteLine("++++++++++++ddmdmmdmdmmd++++++++++++++++++++++++++++++++++++++++++++++++++");
                return Ok(temp);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the debt report detail: {ex.Message}");
            }
        }
    }
}