using AutoMapper;
using BookManagementSystem.Application.Dtos.PaymentReceipt;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class PaymentReceiptProfile : Profile
    {
        public PaymentReceiptProfile()
        {
            CreateMap<CreatePaymentReceiptDto, PaymentReceipt>();
            CreateMap<PaymentReceipt, PaymentReceiptDto>();
            CreateMap<UpdatePaymentReceiptDto, PaymentReceipt>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}