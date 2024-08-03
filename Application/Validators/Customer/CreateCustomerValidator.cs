using FluentValidation;
using BookManagementSystem.Application.Dtos.Customer;

namespace BookManagementSystem.Application.Validators
{
    public class CreateCustomerValidator : AbstractValidator<CreateCustomerDto>
    {
        public CreateCustomerValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Tên khách hàng là bắt buộc.")
                .MaximumLength(45).WithMessage("Tên khách hàng không được vượt quá 45 kí tự.");

            RuleFor(x => x.TotalDebt)
                .GreaterThanOrEqualTo(0).WithMessage("Tổng nợ của khách hàng phải là số không âm.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Địa chỉ là bắt buộc.")
                .MaximumLength(150).WithMessage("Địa chỉ phải không được vượt quá 150 kí tự.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Số điện thoại là bắt buộc.")
                .Length(10).WithMessage("Số điện thoại phải có đúng 10 kí tự.")
                .Matches(@"^\d+$").WithMessage("Số điện thoại chỉ được chứa chữ số.");

            RuleFor(x => x.Email)
                .MaximumLength(100).WithMessage("Email không được vượt quá 100 kí tự.")
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("Định dạng email không hợp lệ.");
        }
    }
}