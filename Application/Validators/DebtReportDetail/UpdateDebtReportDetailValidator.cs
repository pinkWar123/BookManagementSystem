using FluentValidation;
using BookManagementSystem.Application.Dtos.DebtReportDetail;

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

            RuleFor(x => x)
                .Custom((dto, context) =>
                {
                    if (dto.InitialDebt.HasValue && dto.FinalDebt.HasValue && dto.AdditionalDebt.HasValue)
                    {
                        if (dto.FinalDebt.Value - dto.InitialDebt.Value != dto.AdditionalDebt.Value)
                        {
                            context.AddFailure("AdditionalDebt", "Số nợ thay đổi phải bằng nợ cuối trừ nợ đầu.");
                        }
                    }
                });
        }
    }
}