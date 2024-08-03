using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.User;
using FluentValidation;

namespace BookManagementSystem.Application.Validators.Users
{
    public class ChangeRoleValidator : AbstractValidator<ChangeRoleDto>
    {
        public ChangeRoleValidator()
        {
            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role không được để trống")
                .Must(IsValidRole).WithMessage("Role không tồn tại");
        }

        public bool IsValidRole(string role)
        {
            var validRoles = new List<string>(){"Manager", "Cashier", "Customer", "StoreKeeper"};
            return validRoles.Contains(role);
        }
    }
}