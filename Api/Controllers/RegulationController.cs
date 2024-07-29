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
        public RegulationController(
             IRegulationService _regulationService,
            IValidator<CreateRegulationDto> _createvalidator,
            IValidator<UpdateRegulationDto> _updatevalidator
        )
        {
            this._createvalidator = _createvalidator;
            this._updatevalidator = _updatevalidator;
            this._regulationService = _regulationService;
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
    }
}
