using System.Globalization;
using BookManagementSystem.Application.Dtos.Invoice;
using FluentValidation;

namespace BookManagementSystem.Application.Validators
{
    public class CreateInvoiceValidator : AbstractValidator<CreateInvoiceDto>
    {
        private bool BeAValidDate(string? date)
        {
            return DateTime.TryParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
        public CreateInvoiceValidator()
        {
            // write more for InvoiceDetails
            RuleFor(x => x.InvoiceDetails)
                .NotEmpty().WithMessage("Chi tiết hóa đơn không được để trống")
                .Must(x =>
                {
                    foreach (var item in x)
                    {
                        if (item.Quantity <= 0)
                        {
                            return false;
                        }
                    }
                    return true;
                }).WithMessage("số lượng không được nhỏ hơn 0")
                .Must(x =>
                {
                    for (int i = 0; i < x.Count; i++)
                    {
                        for (int j = i + 1; j < x.Count; j++)
                        {
                            if (x[i].BookID == x[j].BookID)
                            {
                                return false;
                            }
                        }
                    }
                    return true;
                }).WithMessage("Không được nhập 2 sách giống nhau trong hóa đơn");
            RuleFor(x => x.CustomerID).NotEmpty().WithMessage("ID của khách hàng không được để trống");
            RuleFor(x => x.InvoiceDate)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage("Ngày nhập hóa đơn không được để trống.")
                .Must(BeAValidDate).WithMessage("Ngày nhập hóa đơn phải là giá trị ngày tháng năm hợp lệ.");
        }
    }
}