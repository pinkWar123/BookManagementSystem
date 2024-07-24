using AutoMapper;
using BookManagementSystem.Application.Dtos.InventoryReportDetail;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class InventoryReportDetailProfile : Profile
    {
        public InventoryReportDetailProfile()
        {
            CreateMap<CreateInventoryReportDetailDto, InventoryReportDetail>();
            CreateMap<InventoryReportDetail, InventoryReportDetailDto>();
            CreateMap<UpdateInventoryReportDetailDto, InventoryReportDetail>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<InventoryReportDetail, UpdateInventoryReportDetailDto>();
        }
    }
}