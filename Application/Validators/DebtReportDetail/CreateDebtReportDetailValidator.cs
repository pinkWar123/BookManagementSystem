using BookManagementSystem.Application.Dtos.DebtReportDetail;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class CreateDebtReportDetailValidator : AbstractValidator<CreateDebtReportDetailDto>
    {
        public CreateDebtReportDetailValidator()
        {
            RuleFor(x => x.InitialDebt)
                .NotEmpty().WithMessage("Số nợ ban đầu không được để trống.")
                .GreaterThanOrEqualTo(0).WithMessage("Số nợ ban đầu phải là số không âm.");

            RuleFor(x => x.FinalDebt)
                .NotEmpty().WithMessage("Số nợ cuối không được để trống.")
                .GreaterThanOrEqualTo(0).WithMessage("Số nợ cuối phải là số không âm.");

            RuleFor(x => x.AdditionalDebt)
                .NotEmpty().WithMessage("Số nợ thay đổi không được để trống.")
                .Must((dto, AdditionalDebt) => dto.FinalDebt - dto.InitialDebt == AdditionalDebt)
                .WithMessage("Số nợ thay đổi phải bằng nợ cuối trừ nợ đầu.");
        }
    }
}
