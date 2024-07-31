using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.Customer;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Validators;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.Customer;
using FluentValidation;
using FluentValidation.Results;
using BookManagementSystem.Application.Exceptions;
using System.Net;

namespace BookManagementSystem.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomerService(
            ICustomerRepository customerRepository,
            IMapper mapper)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _mapper = mapper;
        }

        public async Task<CustomerDto> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            var customer = _mapper.Map<Customer>(createCustomerDto);
            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<CustomerDto> UpdateCustomer(int customerId, UpdateCustomerDto updateCustomerDto)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(customerId);
            if (existingCustomer == null)
            {
                throw new CustomerException($"Không tìm thấy khách hàng với ID {customerId}.", HttpStatusCode.NotFound);
            }

            _mapper.Map(updateCustomerDto, existingCustomer);
            await _customerRepository.UpdateAsync(customerId, existingCustomer);
            await _customerRepository.SaveChangesAsync();
            return _mapper.Map<CustomerDto>(existingCustomer);
        }

        public async Task<CustomerDto> GetCustomerById(int customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
            {
                throw new CustomerException($"Không tìm thấy khách hàng với ID {customerId}.", HttpStatusCode.NotFound);
            }
            return _mapper.Map<CustomerDto>(customer);
        }

        // public async Task<IEnumerable<CustomerDto>> GetAllCustomers()
        // {
        //     var customers = await _customerRepository.GetAllAsync();
        //     return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        // }

        public async Task<bool> DeleteCustomer(int customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
            {
                return false;
            }
            _customerRepository.Remove(customer);
            await _customerRepository.SaveChangesAsync();
            return true;
        }
    }
}
