using AutoMapper;
using BookManagementSystem.Application.Dtos.InvoiceDetail;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class InvoiceDetailProfile : Profile
    {
        public InvoiceDetailProfile()
        {
            CreateMap<CreateInvoiceDetailDto, InvoiceDetail>();

            CreateMap<InvoiceDetail, InvoiceDetailDto>();
            CreateMap<UpdateInvoiceDetailDto, InvoiceDetail>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}