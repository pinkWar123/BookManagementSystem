using System.Globalization;
using BookManagementSystem.Application.Dtos.BookEntry;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class UpdateBookEntryValidator : AbstractValidator<UpdateBookEntryDto>
    {
        private bool BeAValidDate(string? date)
        {
            return DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
        public UpdateBookEntryValidator()
        {  
            RuleFor(x => x.EntryDate)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Ngày lập phiếu không được để trống.")
                .Must(BeAValidDate).WithMessage("Ngày lập phiếu phải là giá trị ngày tháng năm hợp lệ.");
        } 
    }
}