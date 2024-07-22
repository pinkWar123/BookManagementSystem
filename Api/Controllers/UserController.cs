using BookManagementSystem.Application.Dtos.User;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Queries;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public UserController(IUserService userService, IValidator<RegisterDto> registerValidator, IValidator<LoginDto> loginValidator)
        {
            _userService = userService;
            _registerValidator = registerValidator;
            _loginValidator = loginValidator;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var validateResult = await _registerValidator.ValidateAsync(registerDto);

            if(!validateResult.IsValid) return BadRequest(validateResult);

            var userDto = await _userService.Register(registerDto);

            if(!userDto.IsAuthenticated) return BadRequest(userDto);

            return Ok(userDto);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var validateResult = await _loginValidator.ValidateAsync(loginDto);

            if(!validateResult.IsValid) return BadRequest(validateResult);

            var userDto = await _userService.Login(loginDto);

            if(!userDto.IsAuthenticated) return BadRequest(userDto);

            return Ok(userDto);
        }

        [HttpGet]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> GetAllUsers(UserQuery userQuery)
        {
            var users = await _userService.GetAllUsers(userQuery);

            return Ok(users);
        }
    }
}