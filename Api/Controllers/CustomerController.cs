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

namespace BookManagementSystem.Api.Controllers
{
    // [Authorize]
    [Route("api/customers")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IValidator<CreateCustomerDto> _createCustomerValidator;
        public CustomerController(
            ICustomerService customerService,
            IValidator<CreateCustomerDto> createCustomerValidator
            )
        {
            _customerService = customerService;
            _createCustomerValidator = createCustomerValidator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            var validateResult = await _createCustomerValidator.ValidateAsync(createCustomerDto);

            if (!validateResult.IsValid) return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));
        
            var customer = await _customerService.CreateCustomer(createCustomerDto);
            
            return Ok(new Response<CustomerDto>(customer));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomerById([FromRoute] int id)
        {
            var customer = await _customerService.GetCustomerById(id);
            return Ok(new Response<CustomerDto>(customer));
        }
    }
}