using System.Security.Claims;
using System.Text.RegularExpressions;
using AutoMapper;
using BookManagementSystem.Application.Dtos.User;
using BookManagementSystem.Application.Exceptions;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Queries;
using BookManagementSystem.Application.Wrappers;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Helpers;
using BookManagementSystem.Infrastructure.Repositories.User;
using Microsoft.AspNetCore.Http.HttpResults;
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
            var createUser = await _userManager.CreateAsync(user, registerDto.Password);
            

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
                    Email = createdUser.Email ?? null,
                    Token = token,
                    Roles = new List<string>() { Roles.Customer.ToString() }
                };
            }
            else
            {
                return new UserDto
                {
                    Errors = createUser.Errors.Select(error => error.Description).ToList(),
                    Username = ""
                };
            }
        }


        public async Task<List<UserViewDto>?> GetAllUsers(UserQuery userQuery)
        {
            var users = await _userManager.Users.ToListAsync();

            if (userQuery.FullName != null)
            {
                var normalizedSearchTerm = userQuery.FullName.RemoveDiacritics().ToLower();
                Console.WriteLine(normalizedSearchTerm);
                users = users.Where(u => Regex.IsMatch(u.FullName.RemoveDiacritics().ToLower(), normalizedSearchTerm, RegexOptions.IgnoreCase)).ToList();
            }

            if(userQuery.Role != null)
            {
                for(int i = 0; i < users.Count; i++)
                {
                    var role = await _userManager.GetRolesAsync(users[i]);
                    if(!role.Contains(userQuery.Role))
                    {
                        users.RemoveAt(i);
                        --i;
                    }
                }
            }

            var userDtos = new List<UserViewDto>();

            foreach (var user in users)
            {
                IList<string>? role = await _userManager.GetRolesAsync(user);
                userDtos.Add(new UserViewDto
                {
                    Id = user.Id,
                    UserName = user.UserName ?? "",
                    FullName = user.FullName,
                    Email = user.Email,
                    Roles = role as List<string> ?? []
                });
            }
            
            return userDtos;
        }

        public async Task<bool> DoesUsernameExist(string username)
        {
            return await _userManager.FindByNameAsync(username) != null;
        }

        public async Task<UserDto> GetUserByAccessToken(string accessToken)
        {
            
            // var isTokenValid = _tokenService.ValidateToken(accessToken);
            // if(!isTokenValid)
            // {
            //     throw new InvalidTokenException(accessToken);
            // }

            var currentUserName = _httpContextAccessor?.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (currentUserName == null) return null;
            var user = await _userManager.FindByNameAsync(currentUserName);

            if (user == null) throw new BaseException("User không tồn tại", System.Net.HttpStatusCode.NotFound);

            var roles = await _userManager.GetRolesAsync(user);

            var userDto = new UserDto
            {
                Message = "Tìm thấy user",
                IsAuthenticated = false,
                Username = user.UserName,
                Email = user.Email ?? null,
                Roles = roles as List<string>
            };

            return userDto;
        }

        public async Task<UserDto> ChangeUserRole(string userId, ChangeRoleDto changeRoleDto)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new UserException($"Không tìm thấy user. Id không hợp lệ", System.Net.HttpStatusCode.NotFound);

            var oldRole = await _userManager.GetRolesAsync(user) as List<string> ?? throw new UserException("User này không có vai trò!!!");
            
            if (!oldRole.Contains(changeRoleDto.Role))
            {
                await _userManager.RemoveFromRolesAsync(user, oldRole);
                await _userManager.AddToRoleAsync(user, changeRoleDto.Role);
            }

            var newRole = new List<string>{changeRoleDto.Role};

            var userDto = new UserDto
            {
                Message = "Thay đổi role của user thành công",
                Username = user.UserName,
                Email = user.Email ?? null,
                Roles = newRole
            };

            return userDto;
        }

        public async Task<UserDto> DeleteUserById(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new UserException("Không tìm thấy user!!", System.Net.HttpStatusCode.NotFound);

            var result = await _userManager.DeleteAsync(user);

            if(result.Succeeded)
            {
                var userDto = new UserDto
                {
                    Message = "Xóa user thành công!",
                    Username = user.UserName
                };

                return userDto;
            }
            else 
            {
                throw new UserException($"{result.Errors.ToList()}", System.Net.HttpStatusCode.InternalServerError);
            }
        }
    }
}
