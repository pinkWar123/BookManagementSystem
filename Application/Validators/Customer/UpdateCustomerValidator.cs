using FluentValidation;
using BookManagementSystem.Application.Dtos.Customer;

namespace BookManagementSystem.Application.Validators
{
    public class UpdateCustomerValidator : AbstractValidator<UpdateCustomerDto>
    {
        public UpdateCustomerValidator()
        {
            RuleFor(x => x.CustomerName)
                .MaximumLength(45).WithMessage("Tên khách hàng không được vượt quá 45 kí tự.")
                .When(x => !string.IsNullOrEmpty(x.CustomerName));

            RuleFor(x => x.TotalDebt)
                .GreaterThanOrEqualTo(0).WithMessage("Tổng nợ của khách hàng phải là số không âm.")
                .When(x => x.TotalDebt.HasValue);

            RuleFor(x => x.Address)
                .MaximumLength(150).WithMessage("Địa chỉ không được vượt quá 150 kí tự.")
                .When(x => !string.IsNullOrEmpty(x.Address));

            RuleFor(x => x.PhoneNumber)
                .MaximumLength(10).WithMessage("Số điện thoại không được vượt quá 10 kí tự.")
                .Matches(@"^\d+$").WithMessage("Số điện thoại chỉ được chứa chữ số.")
                .When(x => !string.IsNullOrEmpty(x.PhoneNumber));

            RuleFor(x => x.Email)
                .MaximumLength(100).WithMessage("Email không được vượt quá 100 kí tự.")
                .EmailAddress().WithMessage("Định dạng email không hợp lệ.")
                .When(x => !string.IsNullOrEmpty(x.Email));
        }
    }
}