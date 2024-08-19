



using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.InventoryReport;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class InventoryReportProfile : Profile
    {
        public InventoryReportProfile()
        {
            // Map from CreateInventoryReportDto to InventoryReport entity
            CreateMap<CreateInventoryReportDto, InventoryReport>();

            // Map from InventoryReport entity to InventoryReportDto
            CreateMap<InventoryReport, InventoryReportDto>()
                .ForMember(dest => dest.ReportID, opt => opt.MapFrom(src => src.Id));

            // Map from UpdateInventoryReportDto to InventoryReport entity
            CreateMap<UpdateInventoryReportDto, InventoryReport>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<InventoryReport, GetAllInventoryReportDto>()
            .ForMember(dest => dest.InventoryReportDetails, opt => opt.MapFrom(src => src.InventoryReportDetails))
            .ForMember(dest => dest.ReportID, opt => opt.MapFrom(src => src.Id));
        }
    }
}