using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace BookManagementSystem.Application.Validators.Users
{
    public static class PasswordExtension
    {
        public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
                .NotEmpty()
                .MinimumLength(12)
                .Matches("[A-Z]").WithMessage("{PropertyName} phải chứa ít nhất một chữ cái viết hoa")
                .Matches("[a-z]").WithMessage("{PropertyName} phải chứa ít nhất một chữ cái viết thường")
                .Matches("[0-9]").WithMessage("{PropertyName} phải chứa ít nhất một chữ số")
                .Matches("[^a-zA-Z0-9]").WithMessage("{PropertyName} phải chứa ít nhất một kí tự không phải số hoặc chữ cái");
        }
    }
}