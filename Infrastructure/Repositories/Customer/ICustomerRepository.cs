using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Infrastructure.Repositories.Customer
{
    public interface ICustomerRepository : IGenericRepository<Domain.Entities.Customer>
    {
        Task<IEnumerable<int>> GetAllCustomerIdAsync();
    }
}
