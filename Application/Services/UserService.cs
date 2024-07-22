using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.User;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Services
{
    public class UserService : IUserService
    {
        public Task<User?> GetCurrentUser()
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> GetUserByToken(string token)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> Login(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<UserDto> Register(RegisterDto registerDto)
        {
            throw new NotImplementedException();
        }
    }
}