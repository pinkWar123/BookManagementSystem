using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.InvoiceDetail;
using FluentValidation;

namespace BookManagementSystem.Application.Validators.InvoiceDetail
{
    public class CreateInvoiceDetailValidator : AbstractValidator<CreateInvoiceDetailDto>
    {
        public CreateInvoiceDetailValidator()
        {
            RuleFor(x => x.BookID)
                .NotEmpty().WithMessage("BookID không được để trống");
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Quantity không được để trống")
                .GreaterThanOrEqualTo(0).WithMessage("Quantity không được nhỏ hơn 0");
            
            
        }
    }
}