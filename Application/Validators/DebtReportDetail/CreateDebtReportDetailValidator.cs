using System.Data;
using BookManagementSystem.Application.Dtos.DebtReportDetail;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class CreateDebtReportDetailValidator : AbstractValidator<CreateDebtReportDetailDto>
    {
        public CreateDebtReportDetailValidator()
        {
            RuleFor(x => x.ReportID)
                .NotEmpty().WithMessage("ID của báo cáo không được để trống.")
                .GreaterThan(0).WithMessage("ID của báo cáo phải là số dương.");

            RuleFor(x => x.CustomerID)
                .NotEmpty().WithMessage("ID của khách hàng không được để trống.")
                .GreaterThan(0).WithMessage("ID của khách hàng phải là số dương.");

            RuleFor(x => x.InitialDebt)
                .NotEmpty().WithMessage("Số nợ ban đầu không được để trống.")
                .GreaterThanOrEqualTo(0).WithMessage("Số nợ ban đầu phải là số không âm.");

            RuleFor(x => x.FinalDebt)
                .NotEmpty().WithMessage("Số nợ cuối không được để trống.")
                .GreaterThanOrEqualTo(0).WithMessage("Số nợ cuối phải là số không âm.");
        }
    }
}
