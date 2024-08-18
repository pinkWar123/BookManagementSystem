using FluentValidation;
using BookManagementSystem.Application.Dtos.Book;

namespace BookManagementSystem.Application.Validators
{
    public class CreateBookValidator : AbstractValidator<CreateBookDto>
    {
        public CreateBookValidator()
        {
            RuleFor(book => book.Title)
                .NotEmpty().WithMessage("Tên sách là bắt buộc.")
                .MaximumLength(100).WithMessage("Tên sách không được vượt quá 100 ký tự");

            RuleFor(book => book.Genre)
                .NotEmpty().WithMessage("Thể loại là bắt buộc.")
                .MaximumLength(100).WithMessage("Thể loại không được vượt quá 100 ký tự");

            RuleFor(book => book.Author)
                .NotEmpty().WithMessage("Tác giả là bắt buộc.")
                .MaximumLength(100).WithMessage("Tác giả không được vượt quá 100 ký tự");

            RuleFor(book => book)
                .Must(BeUnique).WithMessage("Tên sách, thể loại, và tác giả phải khác nhau.");
        }

        private bool BeUnique(CreateBookDto book)
        {
            return book.Title != book.Genre && book.Title != book.Author && book.Genre != book.Author;
        }
    }
}
