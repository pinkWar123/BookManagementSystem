using System.Globalization;
using BookManagementSystem.Application.Dtos.PaymentReceipt;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class UpdatePaymentReceiptValidator : AbstractValidator<UpdatePaymentReceiptDto>
    {
        public UpdatePaymentReceiptValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0).WithMessage("Số tiền thu vào phải là số dương.");
        }
    }
}