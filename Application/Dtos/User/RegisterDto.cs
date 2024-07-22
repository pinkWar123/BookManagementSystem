using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.User
{
    public class RegisterDto
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string PasswordConfirm { get; set; }
        public string? Email { get; set; }
        public required string FullName { get; set; }
    }
}