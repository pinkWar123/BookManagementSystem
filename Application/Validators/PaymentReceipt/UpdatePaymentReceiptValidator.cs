using System.Globalization;
using BookManagementSystem.Application.Dtos.PaymentReceipt;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class UpdatePaymentReceiptValidator : AbstractValidator<UpdatePaymentReceiptDto>
    {
        private bool BeAValidDate(string? date)
        {
            return DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
        public UpdatePaymentReceiptValidator()
        {
            RuleFor(x => x.ReceiptDate)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Ngày lập phiếu thu không được để trống.")
                .Must(BeAValidDate).WithMessage("Ngày lập phiếu thu phải là giá trị ngày tháng năm hợp lệ.");

            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Số tiền thu vào phải là số dương.");
        }
    }
}