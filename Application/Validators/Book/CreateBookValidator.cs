using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Book;
using FluentValidation;

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
                .MaximumLength(100).WithMessage("Thể loại không được vượt quá 100 ký tự");

            RuleFor(book => book.Quantity)
                .GreaterThan(0).WithMessage("Số lượng sách phải là một số dương");

            RuleFor(book => book.Price)
                .GreaterThan(0).WithMessage("Giá của sách phải là một số dương");
        }
    }
}
