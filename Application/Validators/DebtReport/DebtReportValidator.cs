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
            RuleFor(x => x.ReportMonth).NotEmpty().GreaterThan(0).LessThan(13);
            RuleFor(x => x.ReportYear).NotEmpty().GreaterThan(2023);
        }
    }

    public class UpdateDebtReportValidator : AbstractValidator<UpdateDebtReportDto>
    {
        public UpdateDebtReportValidator()
        {
            RuleFor(x => x.ReportMonth).NotEmpty().GreaterThan(0).LessThan(13);
            RuleFor(x => x.ReportYear).NotEmpty().GreaterThan(2023);
        }
    }
}
