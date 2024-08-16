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
using BookManagementSystem.Application.Queries;
using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Data;
using BookManagementSystem.Application.Dtos.DebtReportDetail;

namespace BookManagementSystem.Application.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDBContext _context;
        private readonly IDebtReportService _debtReportService;
        private readonly IDebtReportDetailService _debtReportDetailService;

        public CustomerService(
            ICustomerRepository customerRepository,
            IMapper mapper,
            ApplicationDBContext context,
            IDebtReportService debtReportService,
            IDebtReportDetailService debtReportDetailService
        )
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _mapper = mapper;
            _context = context;
            _debtReportService = debtReportService;
            _debtReportDetailService = debtReportDetailService;
        }

        public async Task<CustomerDto> CreateCustomer(CreateCustomerDto createCustomerDto)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var customer = _mapper.Map<Customer>(createCustomerDto);
                    await _customerRepository.AddAsync(customer);
                    await _context.SaveChangesAsync();
                    
                    int reportId = await _debtReportService.GetReportIdByMonthYear(DateTime.Today.Month, DateTime.Today.Year);
                    var createDebtReportDetailDto = new CreateDebtReportDetailDto
                    {
                        ReportID = reportId,
                        CustomerID = customer.Id,
                        InitialDebt = customer.TotalDebt,
                        FinalDebt = customer.TotalDebt
                    };
                    
                    await _debtReportDetailService.CreateNewDebtReportDetail(createDebtReportDetailDto);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return _mapper.Map<CustomerDto>(customer);
                }
                catch
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            } 
        }

        public async Task<CustomerDto> UpdateCustomer(int customerId, UpdateCustomerDto updateCustomerDto)
        {
            var existingCustomer = await _customerRepository.GetByIdAsync(customerId);
            if (existingCustomer == null)
            {
                throw new CustomerNotFound(customerId);
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
                throw new CustomerNotFound(customerId);
            }
            return _mapper.Map<CustomerDto>(customer);
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomers(CustomerQuery customerQuery)
        {
            var query = _customerRepository.GetValuesByQuery(customerQuery);
            if (query == null)
            {
                return Enumerable.Empty<CustomerDto>();
            }
            
            var customers = await query.ToListAsync();

            return _mapper.Map<IEnumerable<CustomerDto>>(customers);
        }

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

        public async Task<IEnumerable<int>> GetAllCustomerId()
        {
            return await _customerRepository.GetAllCustomerIdAsync();
        }
    }
}
