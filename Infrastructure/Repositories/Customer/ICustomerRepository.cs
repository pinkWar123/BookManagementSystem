using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Application.Dtos.Customer;


namespace BookManagementSystem.Infrastructure.Repositories.Customer
{
    public interface ICustomerRepository : IGenericRepository<Domain.Entities.Customer>
    {
        Task<IEnumerable<int>> GetAllCustomerIdAsync();
        Task<IEnumerable<CustomerDtoWithAmount>> GetTopCustomersByMonthYearAsync(int month, int year);
    }
}
