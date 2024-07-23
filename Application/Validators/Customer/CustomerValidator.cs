using BookManagementSystem.Application.Dtos.Customer;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerDto>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(45).WithMessage("Customer name must not exceed 45 characters.");

            RuleFor(x => x.TotalDebt)
                .GreaterThanOrEqualTo(0).WithMessage("Total debt must be non-negative.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(150).WithMessage("Address must not exceed 150 characters.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(10).WithMessage("Phone number must not exceed 10 characters.")
                .Matches(@"^\d+$").WithMessage("Phone number must contain only digits.");

            RuleFor(x => x.Email)
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("Invalid email format.");
        }
    }

    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerDto>
    {
        public UpdateCustomerValidator()
        {
            RuleFor(x => x.CustomerName)
                .MaximumLength(45).WithMessage("Customer name must not exceed 45 characters.")
                .When(x => !string.IsNullOrEmpty(x.CustomerName));

            RuleFor(x => x.TotalDebt)
                .GreaterThanOrEqualTo(0).WithMessage("Total debt must be non-negative.")
                .When(x => x.TotalDebt.HasValue);

            RuleFor(x => x.Address)
                .MaximumLength(150).WithMessage("Address must not exceed 150 characters.")
                .When(x => !string.IsNullOrEmpty(x.Address));

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(10).WithMessage("Phone number must not exceed 10 characters.")
                .Matches(@"^\d+$").WithMessage("Phone number must contain only digits.")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            RuleFor(x => x.Email)
                .MaximumLength(100).WithMessage("Email must not exceed 100 characters.")
                .EmailAddress().WithMessage("Invalid email format.")
                .When(x => !string.IsNullOrEmpty(x.Email));
        }
    }
}
