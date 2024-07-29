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
           .MaximumLength(100).WithMessage("Title must be less than or equal to 100 characters.")
           .When(book => book.Title != null);

            RuleFor(book => book.Genre)
                .MaximumLength(100).WithMessage("Genre must be less than or equal to 100 characters.")
                .When(book => book.Genre != null);

            RuleFor(book => book.Author)
                .MaximumLength(100).WithMessage("Author must be less than or equal to 100 characters.")
                .When(book => book.Author != null);

            RuleFor(book => book.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("StockQuantity must be greater than or equal to 0.")
                .When(book => book.StockQuantity.HasValue);

            RuleFor(book => book.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0.")
                .When(book => book.Price.HasValue);
        }
    }
}
