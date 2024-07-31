using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Application.Queries
{
    public class CustomerQuery : QueryObject
    {
        public int? Id { get; set; }
        public string? CustomerName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
    }
}
