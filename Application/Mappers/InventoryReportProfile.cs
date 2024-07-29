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
            CreateMap<InventoryReport, InventoryReportDto>();

            // Map from UpdateInventoryReportDto to InventoryReport entity
            CreateMap<UpdateInventoryReportDto, DebtReport>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}