using BookManagementSystem.Application.Dtos.InventoryReportDetail;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class CreateInventoryReportDetailValidator : AbstractValidator<CreateInventoryReportDetailDto>
    {
        public CreateInventoryReportDetailValidator()
        {

            RuleFor(x => x.InitialStock)
                .NotEmpty().WithMessage("Số lượng ban đầu không được để trống.")
                .GreaterThanOrEqualTo(0).WithMessage("Số lượng ban đầu phải là số không âm.");

            RuleFor(x => x.FinalStock)
                .NotEmpty().WithMessage("Số lượng cuối không được để trống.")
                .GreaterThanOrEqualTo(0).WithMessage("Số lượng cuối phải là số không âm.");

            RuleFor(x => x.AdditionalStock)
                .NotEmpty().WithMessage("Số lượng thay đổi không được để trống.")
                .Must((dto, AdditionalStock) => dto.FinalStock - dto.InitialStock == AdditionalStock)
                .WithMessage("Số lượng thay đổi phải bằng nợ cuối trừ nợ đầu.");
        }
    }
}