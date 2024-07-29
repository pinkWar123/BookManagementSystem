using BookManagementSystem.Application.Dtos.InventoryReportDetail;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class UpdateInventoryReportDetailValidator : AbstractValidator<UpdateInventoryReportDetailDto>
    {
        public UpdateInventoryReportDetailValidator()
        {
            RuleFor(x => x.InitialStock)
                .GreaterThanOrEqualTo(0).When(x => x.InitialStock.HasValue)
                .WithMessage("Số lượng ban đầu phải là số không âm.");

            RuleFor(x => x.FinalStock)
                .GreaterThanOrEqualTo(0).When(x => x.FinalStock.HasValue)
                .WithMessage("Số lượng cuối phải là số không âm.");

            RuleFor(x => x)
                .Custom((dto, context) =>
                {
                    if (dto.InitialStock.HasValue && dto.FinalStock.HasValue && dto.AdditionalStock.HasValue)
                    {
                        if (dto.FinalStock.Value - dto.InitialStock.Value != dto.AdditionalStock.Value)
                        {
                            context.AddFailure("AdditionalStock", "Số lượng thay đổi phải bằng nợ cuối trừ nợ đầu.");
                        }
                    }
                });
        }
    }
}