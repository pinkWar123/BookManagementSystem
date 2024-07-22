using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookManagementSystem.Application.Dtos.DebtReport;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class DebtReportProfile : Profile
    {
        public DebtReportProfile()
        {
            // Map from CreateDebtReportDto to DebtReport entity
            CreateMap<CreateDebtReportDto, DebtReport>();

            // Map from DebtReport entity to DebtReportDto
            CreateMap<DebtReport, DebtReportDto>();

            // Map from UpdateDebtReportDto to DebtReport entity
            CreateMap<UpdateDebtReportDto, DebtReport>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}