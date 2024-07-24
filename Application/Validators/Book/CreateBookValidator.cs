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
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(book => book.Genre)
                .NotEmpty().WithMessage("Genre is required.")
                .MaximumLength(100).WithMessage("Genre must not exceed 100 characters.");

            RuleFor(book => book.Author)
                .NotEmpty().WithMessage("Author is required.")
                .MaximumLength(100).WithMessage("Author must not exceed 100 characters.");

            RuleFor(book => book.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be a non-negative integer.");

            RuleFor(book => book.Price)
                .GreaterThan(0).WithMessage("Price must be a non-negative integer.");
        }
    }
}
