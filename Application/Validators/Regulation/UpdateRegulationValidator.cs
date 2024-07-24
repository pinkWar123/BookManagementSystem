using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Regulation;
using FluentValidation;


namespace BookManagementSystem.Application.Validators
{
    public class UpdateRegulationValidator : AbstractValidator<UpdateRegulationDto>
    {
        public UpdateRegulationValidator()
        {
            RuleFor(x => x.Code)
                .Length(5).When(x => !string.IsNullOrEmpty(x.Code)).WithMessage("Code must be exactly 5 characters.");

            RuleFor(x => x.Content)
                .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Content)).WithMessage("Content cannot exceed 200 characters.");

            RuleFor(x => x.Value)
                .GreaterThan(0).When(x => x.Value.HasValue).WithMessage("Value must be greater than 0.");
        }
    }
}
