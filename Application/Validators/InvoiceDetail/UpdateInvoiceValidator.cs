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
                .GreaterThan(0).WithMessage("Số lượng không được nhỏ hơn hoặc bằng 0")
                .WithMessage("Số lượng không được để trống");
        } 
    }
}