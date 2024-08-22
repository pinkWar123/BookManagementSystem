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
            RuleFor(x => x.CustomerID).NotEmpty().WithMessage("ID của khách hàng không được để trống");
            RuleFor(x => x.InvoiceDate)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Ngày tháng nhập hóa đơn không được để trống.")
                .Must(BeAValidDate).WithMessage("Ngày tháng nhập hóa đơn phải là giá trị ngày tháng năm hợp lệ.");
        } 
    }
}