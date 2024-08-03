using System.Globalization;
using BookManagementSystem.Application.Dtos.PaymentReceipt;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class CreatePaymentReceiptValidator : AbstractValidator<CreatePaymentReceiptDto>
    {
        private bool BeAValidDate(string? date)
        {
            return DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
        public CreatePaymentReceiptValidator()
        {
            RuleFor(x => x.ReceiptDate)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Ngày lập phiếu thu không được để trống.")
                .Must(BeAValidDate).WithMessage("Ngày lập phiếu thu phải là giá trị ngày tháng năm hợp lệ.");

            RuleFor(x => x.Amount)
                .NotEmpty().WithMessage("Số tiền thu vào không được để trống.")
                .GreaterThan(0).WithMessage("Số tiền thu vào phải lớn hơn 0.");

            RuleFor(x => x.CustomerID)
                .NotEmpty().WithMessage("ID khách hàng không được để trống.")
                .GreaterThan(0).WithMessage("ID khách hàng phải là số dương.");
        }
    }
}