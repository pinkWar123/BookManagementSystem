using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.Book;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {

            CreateMap<CreateBookDto, Book>()
                .ForMember(dest => dest.StockQuantity, opt => opt.MapFrom(src => src.Quantity));

            CreateMap<UpdateBookDto, Book>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); 

            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Id));
        }
    }
}
