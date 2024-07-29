using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Application.Queries
{
    public class UserQuery : QueryObject
    {
        public string? FullName { get; set; }
        public string? Role { get; set; }
    }
}
