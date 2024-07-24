using System;
using System.Globalization;
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
            RuleFor(x => x.Price)
                .NotEmpty()
                .GreaterThanOrEqualTo(0).WithMessage("Price không được nhỏ hơn 0")
                .WithMessage("Price không được để trống");
        } 
    }
}