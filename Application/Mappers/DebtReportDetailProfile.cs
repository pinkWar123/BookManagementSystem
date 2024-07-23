using AutoMapper;
using BookManagementSystem.Application.Dtos.DebtReportDetail;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Mappers
{
    public class DebtReportDetailProfile : Profile
    {
        public DebtReportDetailProfile()
        {
            CreateMap<CreateDebtReportDetailDto, DebtReportDetail>();
            CreateMap<DebtReportDetail, DebtReportDetailDto>();
            CreateMap<UpdateDebtReportDetailDto, DebtReportDetail>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}