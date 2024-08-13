using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Book;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class UpdateBookValidator : AbstractValidator<UpdateBookDto>
    {
        public UpdateBookValidator()
        {
            RuleFor(book => book.Title)
           .MaximumLength(100).WithMessage("Tên sách không được vượt quá 100 ký tự")
           .When(book => book.Title != null);

            RuleFor(book => book.Genre)
                .MaximumLength(100).WithMessage("Thể loại không được vượt quá 100 ký tự")
                .When(book => book.Genre != null);

            RuleFor(book => book.Author)
                .MaximumLength(100).WithMessage("Tác giả không được vượt quá 100 ký tự")
                .When(book => book.Author != null);

            RuleFor(book => book.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Số lượng sách phải là một số dương")
                .When(book => book.StockQuantity.HasValue);

            RuleFor(book => book.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Giá của sách phải là một số dương")
                .When(book => book.Price.HasValue);
        }
    }
}
