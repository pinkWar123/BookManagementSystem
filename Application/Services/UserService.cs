using System.Security.Claims;
using AutoMapper;
using BookManagementSystem.Application.Dtos.User;
using BookManagementSystem.Application.Filter;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Queries;
using BookManagementSystem.Data.Repositories;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Infrastructure.Repositories.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserRepository _userRepo;

        public UserService(UserManager<User> userManager, ITokenService tokenService, RoleManager<IdentityRole> roleManager, IMapper mapper, IHttpContextAccessor httpContextAccessor, IUserRepository userRepo)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _roleManager = roleManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _userRepo = userRepo;
        }
        public async Task<User?> GetCurrentUser()
        {
            var currentUserName = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserName == null) return null;
            return await _userManager.FindByNameAsync(currentUserName);
        }

        public async Task<UserDto> GetUserByToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("Token cannot be null or empty", nameof(token));
            }

            var userId = _tokenService.GetUserIdFromToken(token);
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("Invalid token");
            }

            var user = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            var roles = await _userManager.GetRolesAsync(user);
            if (user != null)
            {
                return new UserDto
                {
                    Message = "Find user by token successfully",
                    IsAuthenticated = true,
                    Username = user.UserName ?? "",
                    Email = user.Email,
                    Token = token,
                    Roles = roles as List<string> ?? []
                };
            }
            return new UserDto
            {
                Message = "User not found",
                IsAuthenticated = false,
                Username = ""
            };
        }

        public async Task<UserDto> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.Username);

            if (user == null)
            {
                return new UserDto
                {
                    Username = "",
                    IsAuthenticated = false,
                    Message = "Username không tồn tại",
                };
            }

            if (await _userManager.CheckPasswordAsync(user, loginDto.Password) == false)
            {
                return new UserDto
                {
                    Username = "",
                    IsAuthenticated = false,
                    Message = "Password không đúng",
                };
            }

            var token = await _tokenService.GenerateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new UserDto
            {
                Message = "Đăng nhập thành công",
                IsAuthenticated = true,
                Username = user.UserName ?? "",
                Email = user.Email,
                Token = token,
                Roles = roles as List<string> ?? []
            };
        }

        public async Task<UserDto> Register(RegisterDto registerDto)
        {
            var user = _mapper.Map<User>(registerDto);
            var createUser = await _userManager.CreateAsync(user);

            if (createUser.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.Customer.ToString());
                var createdUser = await _userManager.FindByNameAsync(user.UserName);
                var token = await _tokenService.GenerateJwtToken(createdUser);
                return new UserDto
                {
                    Message = "Đăng ký thành công!",
                    IsAuthenticated = true,
                    Username = createdUser.UserName ?? "",
                    Email = createdUser.Email,
                    Token = token,
                    Roles = new List<string>() { Roles.Customer.ToString() }
                };
            }

            return new UserDto
            {
                Username = "",
                IsAuthenticated = false
            };
        }


        public async Task<List<UserViewDto>?> GetAllUsers(UserQuery userQuery)
        {
            List<User>? users = await _userManager.Users
                .ToListAsync();
            var userDtos = new List<UserViewDto>();
            foreach (User user in users)
            {
                IList<string>? role = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserViewDto
                {
                    UserName = user.UserName ?? "",
                    Email = user.Email,
                    Roles = role as List<string> ?? []
                });
            }
            return userDtos;
        }

    }
}
