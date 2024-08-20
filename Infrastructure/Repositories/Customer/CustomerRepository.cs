using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using BookManagementSystem.Application.Dtos.Customer;


namespace BookManagementSystem.Infrastructure.Repositories.Customer
{
    public class CustomerRepository : GenericRepository<Domain.Entities.Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
        public async Task<IEnumerable<int>> GetAllCustomerIdAsync()
        {
            return await _context.Customers
                .Select(c => c.Id).ToListAsync();
        }

        public async Task<IEnumerable<CustomerDtoWithAmount>> GetTopCustomersByMonthYearAsync(int month, int year)
        {
            var query = _context.PaymentReceipts
                .Where(receipt => receipt.Date.Month == month && receipt.Date.Year == year)
                .Join(_context.Customers,
                    receipt => receipt.CustomerID,
                    customer => customer.Id,
                    (receipt, customer) => new { receipt, customer })
                .GroupBy(x => new { x.customer.Id, x.customer.CustomerName, x.customer.Address, x.customer.PhoneNumber, x.customer.Email, x.customer.TotalDebt }) // Assume TotalDebt is a property of the Customer entity
                .Select(g => new CustomerDtoWithAmount
                {
                    Id = g.Key.Id,
                    CustomerName = g.Key.CustomerName,
                    Address = g.Key.Address,
                    PhoneNumber = g.Key.PhoneNumber,
                    Email = g.Key.Email,
                    TotalDebt = g.Key.TotalDebt,
                    TotalAmount = g.Sum(x => x.receipt.Amount)
                })
                .OrderByDescending(c => c.TotalAmount)
                .Take(5);

            return await query.ToListAsync();
        }


    }
}
