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
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Price).NotEmpty().GreaterThan(0);
        }
    }
}