using BookManagementSystem.Application.Dtos.DebtReportDetail;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class UpdateDebtReportDetailValidator : AbstractValidator<UpdateDebtReportDetailDto>
    {
        public UpdateDebtReportDetailValidator()
        {
            RuleFor(x => x.InitialDebt)
                .GreaterThanOrEqualTo(0).When(x => x.InitialDebt.HasValue)
                .WithMessage("Số nợ ban đầu phải là số không âm.");

            RuleFor(x => x.FinalDebt)
                .GreaterThanOrEqualTo(0).When(x => x.FinalDebt.HasValue)
                .WithMessage("Số nợ cuối phải là số không âm.");
        }
    }
}
