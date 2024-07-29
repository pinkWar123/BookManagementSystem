using AutoMapper;
using BookManagementSystem.Application.Dtos.Invoice;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<CreateInvoiceDto, Invoice>();

            CreateMap<Invoice, InvoiceDto>();
            CreateMap<UpdateInvoiceDto, Invoice>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}