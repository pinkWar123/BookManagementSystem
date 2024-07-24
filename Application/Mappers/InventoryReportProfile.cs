using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.InventoryReport;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class DebtReportProfile : Profile
    {
        public DebtReportProfile()
        {
            // Map from CreateDebtReportDto to DebtReport entity
            CreateMap<CreateInventoryReportDto, InventoryReport>();

            // Map from DebtReport entity to DebtReportDto
            CreateMap<InventoryReport, InventoryReportDto>();

            // Map from UpdateDebtReportDto to DebtReport entity
            CreateMap<UpdateInventoryReportDto, DebtReport>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}