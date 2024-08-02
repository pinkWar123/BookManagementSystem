using AutoMapper;
using BookManagementSystem.Application.Dtos.PaymentReceipt;
using BookManagementSystem.Domain.Entities;

public class PaymentReceiptProfile : Profile
{
    public PaymentReceiptProfile()
    {
        CreateMap<CreatePaymentReceiptDto, PaymentReceipt>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => 
                string.IsNullOrEmpty(src.ReceiptDate) ? default : DateOnly.Parse(src.ReceiptDate)));

        CreateMap<PaymentReceipt, PaymentReceiptDto>()
            .ForMember(dest => dest.ReceiptDate, opt => opt.MapFrom(src => src.Date));
        
        CreateMap<UpdatePaymentReceiptDto, PaymentReceipt>()
            .ForMember(dest => dest.Date, opt => opt.MapFrom(src => 
                string.IsNullOrEmpty(src.ReceiptDate) ? default : DateOnly.Parse(src.ReceiptDate)));
    }
}
