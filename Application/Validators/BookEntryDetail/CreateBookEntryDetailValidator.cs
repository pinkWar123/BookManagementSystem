using BookManagementSystem.Application.Dtos.BookEntryDetail;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class CreateBookEntryDetailValidator : AbstractValidator<CreateBookEntryDetailDto>
    {
        public CreateBookEntryDetailValidator()
        {
            RuleFor(x => x.Quantity)
                .NotEmpty()
                .GreaterThanOrEqualTo(0).WithMessage("Quantity không được nhỏ hơn 0")
                .WithMessage("Quantity không được để trống");
        } 
    }
}