using System;
using System.Globalization;
using BookManagementSystem.Application.Dtos.BookEntryDetail;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class UpdateBookEntryDetailValidator : AbstractValidator<UpdateBookEntryDetailDto>
    {
    
        public UpdateBookEntryDetailValidator()
        {  
            RuleFor(x => x.Quantity)
                .NotEmpty()
                .GreaterThanOrEqualTo(0).WithMessage("Quantity không được nhỏ hơn 0")
                .WithMessage("Quantity không được để trống");
        } 
    }
}