using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.User
{
    public class UserViewDto
    {
        public required string UserName { get; set; }
        public string? Email { get; set; }
        public required List<string> Roles { get; set; }
    }
}