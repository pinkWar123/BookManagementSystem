using System;
using System.Globalization;
using BookManagementSystem.Application.Dtos.Invoice;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class UpdateInvoiceValidator : AbstractValidator<UpdateInvoiceDto>
    {
        private bool BeAValidDate(string? date)
        {
            return DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
        public UpdateInvoiceValidator()
        {  
            RuleFor(x => x.CustomerID).NotEmpty().WithMessage("CustomerID không được để trống");
            RuleFor(x => x.InvoiceDate)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Date không được để trống.")
                .Must(BeAValidDate).WithMessage("InvoiceDate phải là giá trị ngày tháng năm hợp lệ.");
        } 
    }
}