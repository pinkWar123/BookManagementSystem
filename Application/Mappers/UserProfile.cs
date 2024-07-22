using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.User;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDto, User>().ReverseMap();
        }
    }
}
