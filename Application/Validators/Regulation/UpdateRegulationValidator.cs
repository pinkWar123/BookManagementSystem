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
                .Length(5).When(x => !string.IsNullOrEmpty(x.Code)).WithMessage("Code phải có 5 ký tự");

            RuleFor(x => x.Content)
                .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Content)).WithMessage("Nội dung quy định không được vượt quá 200 ký tự");

            RuleFor(x => x.Value)
                .GreaterThan(0).When(x => x.Value.HasValue).WithMessage("giá trị phải lớn hơn 0");
        }
    }
}
