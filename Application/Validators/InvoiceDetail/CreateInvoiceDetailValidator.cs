using BookManagementSystem.Application.Dtos.InvoiceDetail;
using FluentValidation;

namespace BookManagementSystem.Application.Validators.InvoiceDetail
{
    public class CreateInvoiceDetailValidator : AbstractValidator<CreateInvoiceDetailDto>
    {
        public CreateInvoiceDetailValidator()
        {
            RuleFor(x => x.BookID)
                .NotEmpty().WithMessage("ID của sách không được để trống");
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Số lượng không được để trống")
                .GreaterThan(0).WithMessage("Số lượng không được nhỏ hơn hoặc bằng 0");
            
            
        }
    }
}