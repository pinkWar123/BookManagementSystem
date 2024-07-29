using BookManagementSystem.Application.Dtos.User;
using BookManagementSystem.Application.Filter;
using BookManagementSystem.Application.Filter;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Queries;
using BookManagementSystem.Application.Wrappers;
using BookManagementSystem.Application.Wrappers;
using BookManagementSystem.Domain.Entities;
using BookManagementSystem.Helpers;
using BookManagementSystem.Helpers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Api.Controllers
{
    [Authorize]
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUriService _uriService;
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly UserManager<User> _userManager;
        
        public UserController(
        IUserService userService,
        IUriService uriService,
        IValidator<RegisterDto> registerValidator,
        IValidator<LoginDto> loginValidator,
        UserManager<User> userManager)
        {
            _userService = userService;
            _uriService = uriService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
            _userManager = userManager;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var validateResult = await _registerValidator.ValidateAsync(registerDto);

            if (!validateResult.IsValid) return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));

            var userDto = await _userService.Register(registerDto);

            if (!userDto.IsAuthenticated) return BadRequest(userDto);

            return Ok(new Response<UserDto>(userDto));
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var validateResult = await _loginValidator.ValidateAsync(loginDto);

            if (!validateResult.IsValid) return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));

            var userDto = await _userService.Login(loginDto);

            if (!userDto.IsAuthenticated) return BadRequest(userDto);

            return Ok(new Response<UserDto>(userDto));
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        // [AllowAnonymous]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserQuery userQuery)
        {
            var users = await _userService.GetAllUsers(userQuery);
            var totalRecords = users?.Count ?? 0;
            var validFilter = new PaginationFilter(userQuery.PageNumber, userQuery.PageSize);
            var pagedUsers = users.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();
            var pagedResponse = PaginationHelper.CreatePagedResponse(pagedUsers, validFilter, totalRecords, _uriService, Request.Path.Value);
            return Ok(pagedResponse);
        }

        [HttpGet("check-username")]
        [AllowAnonymous]
        public async Task<IActionResult> DoesUserNameExist([FromQuery] string username)
        {
            var doesUserNameExist = await _userService.DoesUsernameExist(username);
            return Ok(new Response<CheckUserDto>(new CheckUserDto { HasExisted = doesUserNameExist }));
        }

        [HttpGet("get-user")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserByAccessToken(string accessToken)
        {
            var userDto = await _userService.GetUserByAccessToken(accessToken);
            return Ok(new Response<UserDto>(userDto));
        }
    }
}
