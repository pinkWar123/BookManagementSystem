using AutoMapper;
using BookManagementSystem.Application.Dtos.InventoryReportDetail;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class DebtReportDetailProfile : Profile
    {
        public DebtReportDetailProfile()
        {
            CreateMap<CreateInventoryReportDetailDto, InventoryReportDetail>();
            CreateMap<InventoryReportDetail, InventoryReportDetailDto>();
            CreateMap<UpdateInventoryReportDetailDto, InventoryReportDetail>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<InventoryReportDetail, UpdateInventoryReportDetailDto>();
        }
    }
}