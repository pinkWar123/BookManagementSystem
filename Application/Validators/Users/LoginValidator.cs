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
            RuleFor(x => x.Username).NotEmpty().WithMessage("{PropertyName} must not be empty");
            RuleFor(x => x.Password).Password();
        }
    }
}