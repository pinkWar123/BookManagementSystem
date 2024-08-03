using AutoMapper;
using BookManagementSystem.Application.Dtos.BookEntry;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class BookEntryProfile : Profile
    {
        public BookEntryProfile()
        {
            CreateMap<CreateBookEntryDto, BookEntry>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => 
                    string.IsNullOrEmpty(src.EntryDate) ? default : DateOnly.Parse(src.EntryDate)));

            CreateMap<BookEntry, BookEntryDto>()
                .ForMember(dest => dest.EntryDate, opt => opt.MapFrom(src => src.Date));
            
            CreateMap<UpdateBookEntryDto, BookEntry>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => 
                    string.IsNullOrEmpty(src.EntryDate) ? default : DateOnly.Parse(src.EntryDate)))
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}