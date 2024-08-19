using AutoMapper;
using BookManagementSystem.Application.Dtos.InventoryReportDetail;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class InventoryReportDetailProfile : Profile
    {
        public InventoryReportDetailProfile()
        {
            // Map từ CreateInventoryReportDetailDto đến InventoryReportDetail
            CreateMap<CreateInventoryReportDetailDto, InventoryReportDetail>()
                .ForMember(dest => dest.InitalStock, opt => opt.MapFrom(src => src.InitialStock)); // Ánh xạ thuộc tính với tên khác

            // Map từ InventoryReportDetail đến InventoryReportDetailDto
            CreateMap<InventoryReportDetail, InventoryReportDetailDto>()
                .ForMember(dest => dest.InitialStock, opt => opt.MapFrom(src => src.InitalStock)); // Ánh xạ thuộc tính với tên khác

            // Map từ UpdateInventoryReportDetailDto đến InventoryReportDetail
            CreateMap<UpdateInventoryReportDetailDto, InventoryReportDetail>()
                .ForMember(dest => dest.InitalStock, opt => opt.MapFrom(src => src.InitialStock)) // Ánh xạ thuộc tính với tên khác
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)); // Điều kiện ánh xạ

            // Map từ InventoryReportDetail đến UpdateInventoryReportDetailDto
            CreateMap<InventoryReportDetail, UpdateInventoryReportDetailDto>()
                .ForMember(dest => dest.InitialStock, opt => opt.MapFrom(src => src.InitalStock)); // Ánh xạ thuộc tính với tên khác
            

            CreateMap<UpdateInventoryReportDetailDto, InventoryReportDetailDto>();
            CreateMap<InventoryReportDetailDto, UpdateInventoryReportDetailDto>();
        }
    }
}