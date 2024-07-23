using BookManagementSystem.Application.Dtos.PaymentReceipt;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class UpdatePaymentReceiptValidator : AbstractValidator<UpdatePaymentReceiptDto>
    {
        public UpdatePaymentReceiptValidator()
        {
            RuleFor(x => x.ReceiptDate).NotEmpty();
            RuleFor(x => x.Amount).NotEmpty().GreaterThan(0);
            RuleFor(x => x.CustomerID).NotEmpty();
        }
    }
}
