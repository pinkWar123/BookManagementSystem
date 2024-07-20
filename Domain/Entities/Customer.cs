using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    public class Customer{
        // Properties: CustomerID, CustomerName, TotalDebt, Address, PhoneNumber, Email
        public string? CustomerID { get; set; }
        public string? CustomerName { get; set; }
        public double TotalDebt { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        
    }
}