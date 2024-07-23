using BookManagementSystem.Application.Dtos.User;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Queries;
using BookManagementSystem.Domain.Entities;
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
        private readonly IValidator<RegisterDto> _registerValidator;
        private readonly IValidator<LoginDto> _loginValidator;
        private readonly UserManager<User> _userManager;
        public UserController(IUserService userService, IValidator<RegisterDto> registerValidator, IValidator<LoginDto> loginValidator, UserManager<User> userManager)
        {
            _userService = userService;
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

            return Ok(userDto);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var validateResult = await _loginValidator.ValidateAsync(loginDto);

            if (!validateResult.IsValid) return BadRequest(Results.ValidationProblem(validateResult.ToDictionary()));

            var userDto = await _userService.Login(loginDto);

            if (!userDto.IsAuthenticated) return BadRequest(userDto);

            return Ok(userDto);
        }

        [HttpGet]
        // [Authorize(Roles = "Manager")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUsers([FromQuery] UserQuery userQuery)
        {
            var users = await _userService.GetAllUsers(userQuery);
            return Ok(users);
        }
    }
}
