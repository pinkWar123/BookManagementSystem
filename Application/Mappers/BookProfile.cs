using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.Book;

namespace BookManagementSystem.Application.Mappers
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<Book, CreateBookDto>()
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title));
        }
    }
}
