using System.Collections.Generic;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Customer;
using BookManagementSystem.Application.Queries;

namespace BookManagementSystem.Application.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerDto> CreateCustomer(CreateCustomerDto createCustomerDto);
        Task<CustomerDto> UpdateCustomer(int customerId, UpdateCustomerDto updateCustomerDto);
        Task<CustomerDto> GetCustomerById(int customerId);
        Task<IEnumerable<CustomerDto>> GetAllCustomers(CustomerQuery customerQuery);
        Task<bool> DeleteCustomer(int customerId);
        Task<IEnumerable<int>> GetAllCustomerId();
        Task<int> GetCustomerCount();
    }
}
