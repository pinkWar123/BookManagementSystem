using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Interfaces
{
    public interface ITokenService
    {
        string? GetUserIdFromToken(string token);
        Task<string> GenerateJwtToken(User appUser);
        bool ValidateToken(string token);
    }
}
