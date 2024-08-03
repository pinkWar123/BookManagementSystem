using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.User
{
    public class ChangeRoleDto
    {
        public required string Role { get; set; }
    }
}