using AutoMapper;
using BookManagementSystem.Application.Dtos.Invoice;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class InvoiceProfile : Profile
    {
        public InvoiceProfile()
        {
            CreateMap<CreateInvoiceDto, Invoice>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => 
                    string.IsNullOrEmpty(src.InvoiceDate) ? default : DateOnly.Parse(src.InvoiceDate)));

            CreateMap<Invoice, InvoiceDto>()
                .ForMember(dest => dest.InvoiceDate, opt => opt.MapFrom(src => src.Date));
            
            CreateMap<UpdateInvoiceDto, Invoice>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => 
                    string.IsNullOrEmpty(src.InvoiceDate) ? default : DateOnly.Parse(src.InvoiceDate)))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}