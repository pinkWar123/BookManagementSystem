using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.User;
using BookManagementSystem.Application.Queries;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> Login(LoginDto loginDto);
        Task<UserDto> Register(RegisterDto registerDto);
        Task<UserDto> GetUserByToken(string token);
        Task<User?> GetCurrentUser();
        Task<List<UserViewDto>?> GetAllUsers(UserQuery userQuery);
        Task<bool> DoesUsernameExist(string username);
        Task<UserDto> GetUserByAccessToken(string accessToken);
        Task<UserDto> ChangeUserRole(string userId, ChangeRoleDto changeRoleDto);
        Task<UserDto> DeleteUserById(string userId);
    }
}
