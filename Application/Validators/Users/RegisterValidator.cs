using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.User;
using BookManagementSystem.Infrastructure.Repositories.User;
using FluentValidation;
using Microsoft.Identity.Client;

namespace BookManagementSystem.Application.Validators.Users
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        private readonly IUserRepository _userRepository;
        public RegisterValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            RuleFor(x => x.FullName).
                NotEmpty().WithMessage("{ProperyName} không được để trống")
                .MaximumLength(100).WithMessage("{ProperyName} không được vượt quá 100 kí tự");

            RuleFor(x => x.UserName)
                .MustAsync((userName, cancellation) => IsUsernameUniqueAsync(userName))
                .WithMessage("{PropertyName} đã tồn tại");

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("{PropertyName} không hợp lệ")
                .When(x => !string.IsNullOrEmpty(x.Email))
                .WithMessage("{PropertyName} không hợp lệ");

            RuleFor(x => x.Password).Password();

            RuleFor(x => x.PasswordConfirm).Equal(x => x.Password).WithMessage("Xác nhận không khớp với mật khẩu");
        }
        public async Task<bool> IsUsernameUniqueAsync(string username)
        {
            var users = await _userRepository.FindAllAsync(x => x.UserName == username);
            return users == null || users.Count == 0;
        }
    }
}
