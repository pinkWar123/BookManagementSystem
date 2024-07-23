using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.User;
using FluentValidation;

namespace BookManagementSystem.Application.Validators.Users
{
    public class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("{PropertyName} không được để trống");
            RuleFor(x => x.Password).Password();
        }
    }
}
