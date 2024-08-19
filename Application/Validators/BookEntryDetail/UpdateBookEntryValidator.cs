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
                .GreaterThan(0).WithMessage("Số lượng không được nhỏ hơn 0")
                .WithMessage("Số lượng không được để trống");
        } 
    }
}