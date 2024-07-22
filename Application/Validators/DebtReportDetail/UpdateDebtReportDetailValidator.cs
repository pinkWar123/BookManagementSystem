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
                .WithMessage("InitialDebt must be non-negative.");

            RuleFor(x => x.FinalDebt)
                .GreaterThanOrEqualTo(0).When(x => x.FinalDebt.HasValue)
                .WithMessage("FinalDebt must be non-negative.");

            RuleFor(x => x)
                .Custom((dto, context) =>
                {
                    if (dto.InitialDebt.HasValue && dto.FinalDebt.HasValue && dto.DebtChange.HasValue)
                    {
                        if (dto.FinalDebt.Value - dto.InitialDebt.Value != dto.DebtChange.Value)
                        {
                            context.AddFailure("DebtChange", "When all values are provided, DebtChange must equal FinalDebt minus InitialDebt.");
                        }
                    }
                });
        }
    }
}