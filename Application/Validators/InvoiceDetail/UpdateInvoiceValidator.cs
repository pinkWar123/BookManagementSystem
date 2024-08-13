using BookManagementSystem.Application.Dtos.InvoiceDetail;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class UpdateInvoiceDetailValidator : AbstractValidator<UpdateInvoiceDetailDto>
    {
    
        public UpdateInvoiceDetailValidator()
        {  
            RuleFor(x => x.Quantity)
                .NotEmpty()
                .GreaterThanOrEqualTo(0).WithMessage("Quantity không được nhỏ hơn 0")
                .WithMessage("Quantity không được để trống");
        } 
    }
}