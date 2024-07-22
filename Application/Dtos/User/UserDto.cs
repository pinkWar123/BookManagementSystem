using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.User
{
    public class UserDto
    {
        public string? Message { get; set; }
        public bool IsAuthenticated { get; set; }
        public required string Username { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public List<string> Roles { get; set; } = new List<string>();
    }
}
