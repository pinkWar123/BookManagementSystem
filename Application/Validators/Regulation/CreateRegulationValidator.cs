using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Regulation;
using FluentValidation;


namespace BookManagementSystem.Application.Validators
{
    public class CreateRegulationValidator : AbstractValidator<CreateRegulationDto>
    {
        public CreateRegulationValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.")
                .Length(5).WithMessage("Code must be exactly 5 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(200).WithMessage("Content cannot exceed 200 characters.");

            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Value is required.")
                .GreaterThan(0).WithMessage("Value must be greater than 0.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Status is required.");
        }
    }
}
