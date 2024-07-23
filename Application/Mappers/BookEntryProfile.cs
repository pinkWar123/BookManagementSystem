using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.BookEntry;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class BookEntryProfile : Profile
    {
        public BookEntryProfile()
        {
            CreateMap<CreateBookEntryDto, BookEntry>();

            CreateMap<BookEntry, BookEntryDto>();
            CreateMap<UpdateBookEntryDto, BookEntry>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}