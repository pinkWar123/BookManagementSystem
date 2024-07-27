using BookManagementSystem.Application.Dtos.DebtReport;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Wrappers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookManagementSystem.Api.Controllers
{
    [Authorize]
    [Route("api/debt-report")]
    [ApiController]
    public class DebtReportController : ControllerBase
    {
        private readonly IDebtReportService _debtReportService;
        private readonly IValidator<CreateDebtReportDto> _createDebtReportValidator;
        private readonly IValidator<UpdateDebtReportDto> _updateDebtReportValidator;
        public DebtReportController(
            IDebtReportService debtReportService,
            IValidator<CreateDebtReportDto> createDebtReportValidator,
            IValidator<UpdateDebtReportDto> updateDebtReportValidator    
        )
        {
            _debtReportService = debtReportService;
            _createDebtReportValidator = createDebtReportValidator;
            _updateDebtReportValidator = updateDebtReportValidator;
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateNewDebtReport(CreateDebtReportDto createDebtReportDto)
        {
            var validateResult = await _createDebtReportValidator.ValidateAsync(createDebtReportDto);

            if (!validateResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));
            }

            var createdDebtReport = await _debtReportService.CreateNewDebtReport(createDebtReportDto);

            return Ok(new Response<DebtReportDto>(createdDebtReport));
        }

        [HttpPut("{reportId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateDebtReport([FromRoute] int reportId, UpdateDebtReportDto updateDebtReportDto)
        {
            var validateResult = await _updateDebtReportValidator.ValidateAsync(updateDebtReportDto);

            if (!validateResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));
            }

            var existingDebtReport = await _debtReportService.GetDebtReportById(reportId);
            if (existingDebtReport == null)
            {
                return NotFound($"Debt report with ID {reportId} not found.");
            }

            try
            {
                var updatedDebtReport = await _debtReportService.UpdateDebtReport(reportId, updateDebtReportDto);
                return Ok(new Response<DebtReportDto>(updatedDebtReport));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the debt report: {ex.Message}");
            }
        }

        // [HttpGet]
        // [Authorize(Roles = "Manager")]
        // public async Task<IActionResult> GetAllDebtReports([FromQuery] DebtReportQuery debtReportQuery)
        // {
        //     var debtReports = await _debtReportService.GetAllDebtReports(debtReportQuery);
        //     var totalRecords = debtReports?.Count ?? 0;
        //     var validFilter = new PaginationFilter(debtReportQuery.PageNumber, debtReportQuery.PageSize);
        //     var pagedDebtReports = debtReports.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
        //     var pagedResponse = PaginationHelper.CreatePagedResponse(pagedDebtReports, validFilter, totalRecords, _uriService, Request.Path.Value);
        //     return Ok(pagedResponse);
        // }

        [HttpGet("{reportId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetDebtReportById([FromRoute] int reportId)
        {
            var debtReport = await _debtReportService.GetDebtReportById(reportId);

            if (debtReport == null) return NotFound();

            return Ok(new Response<DebtReportDto>(debtReport));
        }

        [HttpDelete("{reportId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteDebtReport([FromRoute] int reportId)
        {
            var result = await _debtReportService.DeleteDebtReport(reportId);

            if (!result) return NotFound();

            return NoContent();
        }
    }
}