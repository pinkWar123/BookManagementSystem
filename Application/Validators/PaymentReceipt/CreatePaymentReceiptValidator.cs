using BookManagementSystem.Application.Dtos.PaymentReceipt;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class CreatePaymentReceiptValidator : AbstractValidator<CreatePaymentReceiptDto>
    {
        public CreatePaymentReceiptValidator()
        {
            RuleFor(x => x.ReceiptDate)
                .NotEmpty().WithMessage("Ngày lập phiếu thu không được để trống.");

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Số tiền thu vào không được để trống.")
                .GreaterThan(0).WithMessage("Số tiền thu vào phải lớn hơn 0.");

            RuleFor(x => x.CustomerID)
                .NotEmpty().WithMessage("Mã khách hàng không được để trống.")
                .Length(5).WithMessage("Mã khách hàng phải có độ dài là 5 ký tự.");
        }
    }
}