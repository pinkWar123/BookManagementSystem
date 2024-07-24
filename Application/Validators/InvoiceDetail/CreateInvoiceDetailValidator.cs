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
        RuleFor(x => x.InvoiceID).NotEmpty().WithMessage("InvoiceID không được để trống");
            RuleFor(x => x.CustomerID).NotEmpty().WithMessage("BookID không được để trống");
            RuleFor(x => x.InvoiceDate)
                .NotEmpty()
                .WithMessage("InvoiceDate không được để trống");
            
        }
    }
}