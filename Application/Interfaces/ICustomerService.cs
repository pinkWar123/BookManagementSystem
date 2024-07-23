using System.Collections.Generic;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Customer;

namespace BookManagementSystem.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDto> CreateCustomer(CreateCustomerDto createCustomerDto);
        Task<CustomerDto> UpdateCustomer(string customerId, UpdateCustomerDto updateCustomerDto);
        Task<CustomerDto> GetCustomerById(string customerId);
        // Task<IEnumerable<CustomerDto>> GetAllCustomers();
        Task<bool> DeleteCustomer(string customerId);
    }
}
