using BookManagementSystem.Application.Dtos.BookEntryDetail;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class CreateBookEntryDetailValidator : AbstractValidator<CreateBookEntryDetailDto>
    {
        public CreateBookEntryDetailValidator()
        {
            RuleFor(x => x.BookID)
                .NotEmpty().WithMessage($"ID Sách không được để trống");
            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage("Số lượng không được để trống")
                .GreaterThan(0).WithMessage("Số lượng không được nhỏ hơn hoặc bằng 0");
                
        } 
    }
}