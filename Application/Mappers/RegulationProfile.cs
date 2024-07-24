using AutoMapper;
using BookManagementSystem.Application.Dtos.Regulation;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class RegulationProfile : Profile
    {
        public RegulationProfile()
        {
            CreateMap<CreateRegulationDto, Regulation>();
            CreateMap<Regulation, RegulationDto>();
            CreateMap<UpdateRegulationDto, Regulation>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Regulation, UpdateRegulationDto>();
        }
    }
}