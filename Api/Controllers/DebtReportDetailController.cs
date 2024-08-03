using BookManagementSystem.Application.Dtos.DebtReportDetail;
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
    [Route("api/debt-report-detail")]
    [ApiController]
    public class DebtReportDetailDetailController : ControllerBase
    {
        private readonly IDebtReportDetailService _debtReportDetailService;
        private readonly IValidator<CreateDebtReportDetailDto> _createDebtReportDetailValidator;
        private readonly IValidator<UpdateDebtReportDetailDto> _updateDebtReportDetailValidator;
        private readonly IUriService _uriService;

        public DebtReportDetailDetailController(
            IDebtReportDetailService debtReportDetailService,
            IValidator<CreateDebtReportDetailDto> createDebtReportDetailValidator,
            IValidator<UpdateDebtReportDetailDto> updateDebtReportDetailValidator,
            IUriService uriService
        )
        {
            _debtReportDetailService = debtReportDetailService;
            _createDebtReportDetailValidator = createDebtReportDetailValidator;
            _updateDebtReportDetailValidator = updateDebtReportDetailValidator;
            _uriService = uriService;
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateNewDebtReportDetail(CreateDebtReportDetailDto createDebtReportDetailDto)
        {
            var validateResult = await _createDebtReportDetailValidator.ValidateAsync(createDebtReportDetailDto);

            if (!validateResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));
            }

            var createdDebtReportDetail = await _debtReportDetailService.CreateNewDebtReportDetail(createDebtReportDetailDto);

            return Ok(new Response<DebtReportDetailDto>(createdDebtReportDetail));
        }

        [HttpPut("{reportId}/{customerId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateDebtReportDetail([FromRoute] int reportId, [FromRoute] int customerId, UpdateDebtReportDetailDto updateDebtReportDetailDto)
        {
            var validateResult = await _updateDebtReportDetailValidator.ValidateAsync(updateDebtReportDetailDto);

            if (!validateResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));
            }

            var existingDebtReportDetail = await _debtReportDetailService.GetDebtReportDetailById(reportId, customerId);
            if (existingDebtReportDetail == null)
            {
                return NotFound($"Debt report detail with report ID {reportId} and customer ID {customerId} not found.");
            }

            try
            {
                var updatedDebtReportDetail = await _debtReportDetailService.UpdateDebtReportDetail(reportId, customerId, updateDebtReportDetailDto);
                return Ok(new Response<DebtReportDetailDto>(updatedDebtReportDetail));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the debt report detail: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllDebtReportDetails([FromQuery] DebtReportDetailQuery debtReportDetailQuery)
        {
            var debtReportDetails = await _debtReportDetailService.GetAllDebtReportDetails(debtReportDetailQuery);
            var totalRecords = debtReportDetails != null ? debtReportDetails.Count() : 0;
            var validFilter = new PaginationFilter(debtReportDetailQuery.PageNumber, debtReportDetailQuery.PageSize);
            var pagedDebtReportDetails = debtReportDetails.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var pagedResponse = PaginationHelper.CreatePagedResponse(pagedDebtReportDetails, validFilter, totalRecords, _uriService, Request.Path.Value);
            return Ok(pagedResponse);
        }

        [HttpGet("{reportId}/{customerId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetDebtReportDetailById([FromRoute] int reportId, [FromRoute] int customerId)
        {
            var debtReportDetail = await _debtReportDetailService.GetDebtReportDetailById(reportId, customerId);

            if (debtReportDetail == null) return NotFound();

            return Ok(new Response<DebtReportDetailDto>(debtReportDetail));
        }

        [HttpDelete("{reportId}/{customerId}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteDebtReportDetail([FromRoute] int reportId, [FromRoute] int customerId)
        {
            var result = await _debtReportDetailService.DeleteDebtReportDetail(reportId, customerId);

            if (!result) return NotFound();

            return NoContent();
        }
    }
}