using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using BookManagementSystem.Application.Dtos.InventoryReport;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using BookManagementSystem.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;

namespace BookManagementSystem.Api.Controllers
{
    [Authorize]
    [Route("api/inventory_report")]
    [ApiController]
    
    public class InventoryReportController : ControllerBase
    {
        private readonly IInventoryReportService _inventoryreportservice;
        private readonly IValidator<CreateInventoryReportDto> _createvalidator;
        private readonly IValidator<UpdateInventoryReportDto> _updatevalidator;
        public InventoryReportController(
            IInventoryReportService bookService,
            IValidator<CreateInventoryReportDto> _createbookvalidator,
            IValidator<UpdateInventoryReportDto> _updatebookvalidator
            )
        {
            this._createvalidator = _createbookvalidator;
            this._updatevalidator = _updatebookvalidator;
            _inventoryreportservice = bookService;
        }

        [HttpPost]
        [Authorize(Roles = "Manager,StoreKeeper,Cashier")]
        public async Task<IActionResult> CreateInventoryReport(CreateInventoryReportDto createinventoryreport)
        {
            var validationResult = await _createvalidator.ValidateAsync(createinventoryreport);
            if (!validationResult.IsValid)
            {
                BadRequest(Results.ValidationProblem(validationResult.ToDictionary()));
            }

            var inventory = await _inventoryreportservice.CreateInventoryReport(createinventoryreport);
            return Ok(new Application.Wrappers.Response<InventoryReportDto>(inventory));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager,StoreKeeper,Cashier")]
        public async Task<IActionResult> UpdateInventoryReport([FromRoute] int id, UpdateInventoryReportDto updateinventoryreport)
        {
            var validateResult = await _updatevalidator.ValidateAsync(updateinventoryreport);

            if (!validateResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));
            }

            var temp = await _inventoryreportservice.GetInventoryReportById(id);
            if (temp == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }
            var updatedinventory = await _inventoryreportservice.UpdateInventoryReport(id, updateinventoryreport);
            return Ok(new Application.Wrappers.Response<InventoryReportDto>(updatedinventory));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager,StoreKeeper,Cashier")]
        public async Task<IActionResult> GetInventoryReportById([FromRoute] int id)
        {
            var inventory = await _inventoryreportservice.GetInventoryReportById(id);

            if (inventory == null) return NotFound();

            return Ok(new Application.Wrappers.Response<InventoryReportDto>(inventory));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager,StoreKeeper,Cashier")]
        public async Task<IActionResult> DeleteInventoryReport([FromRoute] int id)
        {
            var result = await _inventoryreportservice.DeleteInventoryReport(id);

            if (!result) return NotFound();

            return Ok("delete Successfully");
        }
    }
}
