using FluentValidation;
using BookManagementSystem.Application.Dtos.DebtReportDetail;

namespace BookManagementSystem.Application.Validators
{
    public class CreateDebtReportDetailValidator : AbstractValidator<CreateDebtReportDetailDto>
    {
        public CreateDebtReportDetailValidator()
        {
            RuleFor(x => x.ReportID)
                .NotEmpty().WithMessage("ReportID is required.")
                .MaximumLength(5).WithMessage("ReportID must not exceed 5 characters.");

            RuleFor(x => x.CustomerID)
                .NotEmpty().WithMessage("CustomerID is required.")
                .MaximumLength(5).WithMessage("CustomerID must not exceed 5 characters.");

            RuleFor(x => x.InitialDebt)
                .NotEmpty().WithMessage("InitialDebt is required.")
                .GreaterThanOrEqualTo(0).WithMessage("InitialDebt must be non-negative.");

            RuleFor(x => x.FinalDebt)
                .GreaterThanOrEqualTo(0).WithMessage("FinalDebt must be non-negative.");

            RuleFor(x => x.DebtChange)
                .Must((dto, debtChange) => dto.FinalDebt - dto.InitialDebt == debtChange)
                .WithMessage("DebtChange must equal FinalDebt minus InitialDebt.");
        }
    }
}