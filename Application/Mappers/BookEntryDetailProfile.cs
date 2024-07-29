using AutoMapper;
using BookManagementSystem.Application.Dtos.BookEntryDetail;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class BookEntryDetailProfile : Profile
    {
        public BookEntryDetailProfile()
        {
            CreateMap<CreateBookEntryDetailDto, BookEntryDetail>();

            CreateMap<BookEntryDetail, BookEntryDetailDto>();
            CreateMap<UpdateBookEntryDetailDto, BookEntryDetail>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}