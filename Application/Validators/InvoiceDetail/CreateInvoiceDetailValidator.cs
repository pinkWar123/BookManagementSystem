using BookManagementSystem.Application.Dtos.InvoiceDetail;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class CreateInvoiceDetailValidator : AbstractValidator<CreateInvoiceDetailDto>
    {
        public CreateInvoiceDetailValidator()
        {
            RuleFor(x => x.InvoiceID).NotEmpty().WithMessage("InvoiceID không được để trống");
            RuleFor(x => x.BookID).NotEmpty().WithMessage("BookID không được để trống");
            RuleFor(x => x.Quantity)
                .NotEmpty()
                .GreaterThanOrEqualTo(0).WithMessage("Quantity không được nhỏ hơn 0")
                .WithMessage("Quantity không được để trống");
            RuleFor(x => x.Price)
                .NotEmpty()
                .GreaterThanOrEqualTo(0).WithMessage("Price không được nhỏ hơn 0")
                .WithMessage("Price không được để trống");
        } 
    }
}