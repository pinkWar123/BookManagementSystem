using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Customer;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Wrappers;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using BookManagementSystem.Application.Queries;
using Microsoft.AspNetCore.Authorization;
using BookManagementSystem.Application.Filter;
using BookManagementSystem.Helpers;

namespace BookManagementSystem.Api.Controllers
{
    // [Authorize]
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IValidator<CreateCustomerDto> _createCustomerValidator;
        private readonly IValidator<UpdateCustomerDto> _updateCustomerValidator;
        private readonly IUriService _uriService;

        public CustomerController(
            ICustomerService customerService,
            IValidator<CreateCustomerDto> createCustomerValidator,
            IValidator<UpdateCustomerDto> updateCustomerValidator,
            IUriService uriService)
        {
            _customerService = customerService;
            _createCustomerValidator = createCustomerValidator;
            _updateCustomerValidator = updateCustomerValidator;
            _uriService = uriService;
        }

        [HttpPost]
        [Authorize(Roles = "Manager, Cashier")]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            var validateResult = await _createCustomerValidator.ValidateAsync(createCustomerDto);

            if (!validateResult.IsValid) return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));
        
            var customer = await _customerService.CreateCustomer(createCustomerDto);
            
            return Ok(new Response<CustomerDto>(customer));
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> UpdateCustomer([FromRoute] int id, UpdateCustomerDto updateCustomerDto)
        {
            var validateResult = await _updateCustomerValidator.ValidateAsync(updateCustomerDto);

            if (!validateResult.IsValid)
            {
                return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));
            }

            var existingCustomer = await _customerService.GetCustomerById(id);
            if (existingCustomer == null)
            {
                return NotFound($"Customer with ID {id} not found.");
            }

            try
            {
                var updatedCustomer = await _customerService.UpdateCustomer(id, updateCustomerDto);
                return Ok(new Response<CustomerDto>(updatedCustomer));
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating customer: {ex.Message}");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Manager, Cashier")]
        public async Task<IActionResult> GetAllCustomers([FromQuery] CustomerQuery customerQuery)
        {
            var customers = await _customerService.GetAllCustomers(customerQuery);
            var totalRecords = customers != null ? customers.Count() : 0;
            var validFilter = new PaginationFilter(customerQuery.PageNumber, customerQuery.PageSize);
            var pagedCustomers = customers.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var pagedResponse = PaginationHelper.CreatePagedResponse(pagedCustomers, validFilter, totalRecords, _uriService, Request.Path.Value);
            return Ok(pagedResponse);
        }


        [HttpGet("{id}")]
        [Authorize(Roles = "Manager, Cashier")]
        public async Task<IActionResult> GetCustomerById([FromRoute] int id)
        {
            var customer = await _customerService.GetCustomerById(id);

            if (customer == null) return NotFound();

            return Ok(new Response<CustomerDto>(customer));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> DeleteCustomer([FromRoute] int id)
        {
            var result = await _customerService.DeleteCustomer(id);

            if (!result) return NotFound();

            return NoContent();
        }

        [HttpGet("getTop5CustomerPerMonth")]
        
        public async Task<IActionResult> GetTopCustomersByMonthYear(int month, int year)
        {
            var topCustomers = await _customerService.GetTopCustomersByMonthYear(month, year);

            // if (!topCustomers.Any())
            //     return NotFound();

            return Ok(new Response<List<CustomerDtoWithAmount>>(topCustomers as List<CustomerDtoWithAmount>));
        }

    }
}