using AutoMapper;
using BookManagementSystem.Application.Dtos.Regulation;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class RegulationProfile : Profile
    {
        public RegulationProfile()
        {
            CreateMap<CreateRegulationDto, Regulation>().ReverseMap();
            CreateMap<CreateRegulationDto, RegulationDto>();
            CreateMap<Regulation, RegulationDto>()
                .ForMember(dest => dest.RegulationId, opt => opt.MapFrom(src => src.Id));
            CreateMap<UpdateRegulationDto, Regulation>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<Regulation, UpdateRegulationDto>();
        }
    }
}