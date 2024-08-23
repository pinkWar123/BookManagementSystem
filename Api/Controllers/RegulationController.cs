using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure;
using BookManagementSystem.Application.Dtos.Regulation;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using BookManagementSystem.Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using BookManagementSystem.Application.Queries;
using BookManagementSystem.Application.Filter;
using BookManagementSystem.Helpers;

namespace BookManagementSystem.Api.Controllers
{
    [Authorize]
    [Route("api/regulation")]
    [ApiController]
    public class RegulationController : ControllerBase
    {
        private readonly IRegulationService _regulationService;
        private readonly IValidator<CreateRegulationDto> _createvalidator;
        private readonly IValidator<UpdateRegulationDto> _updatevalidator;
        private readonly IUriService _uriService;
        public RegulationController(
             IRegulationService _regulationService,
            IValidator<CreateRegulationDto> _createvalidator,
            IValidator<UpdateRegulationDto> _updatevalidator,
            IUriService _uriService
        )
        {
            this._createvalidator = _createvalidator;
            this._updatevalidator = _updatevalidator;
            this._regulationService = _regulationService;
            this._uriService = _uriService;
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> CreateRegulation(CreateRegulationDto createregulationDto)
        {
            var validationResult = await _createvalidator.ValidateAsync(createregulationDto);
            if (!validationResult.IsValid)
            {
                BadRequest(Results.ValidationProblem(validationResult.ToDictionary()));
            }

            var regualtion = await _regulationService.CreateRegulation(createregulationDto);
            return Ok(new Application.Wrappers.Response<RegulationDto>(regualtion));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateRegulation([FromRoute] int id, UpdateRegulationDto updateRegulationDto)
        {
            var validateResult = await _updatevalidator.ValidateAsync(updateRegulationDto);

            if (!validateResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));
            }

            var regualtion = await _regulationService.GetRegulationById(id);
            if (regualtion == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }
            var updatedregulation = await _regulationService.UpdateRegulation(id, updateRegulationDto);
            return Ok(new Application.Wrappers.Response<RegulationDto>(updatedregulation));
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetRegulationById([FromRoute] int id)
        {
            var regulation = await _regulationService.GetRegulationById(id);

            if (regulation == null) return NotFound();

            return Ok(new Application.Wrappers.Response<RegulationDto>(regulation));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook([FromRoute] int id)
        {
            var result = await _regulationService.DeleteRegulation(id);

            if (!result) return NotFound();

            return Ok("delete Successfully");
        }

        [HttpGet()]
        [Authorize(Roles = "Manager, Cashier")]
        public async Task<IActionResult> GetAllRegulations([FromQuery] RegulationQuery regulationQuery)
        {
            var books = await _regulationService.GetAllRegulations(regulationQuery);
            var totalRecords = books != null ? books.Count() : 0;
            var validFilter = new PaginationFilter(regulationQuery.PageNumber, regulationQuery.PageSize);
            var pagedregulations = books.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var pagedResponse = PaginationHelper.CreatePagedResponse(pagedregulations, validFilter, totalRecords, _uriService, Request.Path.Value);
            return Ok(pagedResponse);
        }
    }
}
