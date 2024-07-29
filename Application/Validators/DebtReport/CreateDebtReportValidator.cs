using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.DebtReport;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class CreateDebtReportValidator : AbstractValidator<CreateDebtReportDto>
    {
        public CreateDebtReportValidator()
        {
            RuleFor(x => x.ReportMonth)
                .NotEmpty().WithMessage("Tháng không được để trống.")
                .InclusiveBetween(1, 12).WithMessage("Tháng phải nằm trong khoảng từ 1 đến 12.");

            RuleFor(x => x.ReportYear)
                .NotEmpty().WithMessage("Năm không được để trống.");
        }
    }
}
