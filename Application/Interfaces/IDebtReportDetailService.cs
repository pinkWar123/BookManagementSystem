using System.Collections.Generic;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.DebtReportDetail;
using BookManagementSystem.Application.Queries;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IDebtReportDetailService
    {
        Task<DebtReportDetailDto> CreateNewDebtReportDetail(CreateDebtReportDetailDto createDebtReportDetailDto);
        Task<DebtReportDetailDto> UpdateDebtReportDetail(int reportId, int customerId, UpdateDebtReportDetailDto updateDebtReportDetailDto);
        Task<DebtReportDetailDto> GetDebtReportDetailById(int reportId, int customerId);
        Task<IEnumerable<DebtReportDetailDto>> GetAllDebtReportDetails(DebtReportDetailQuery debtReportDetailQuery);
        Task<bool> DeleteDebtReportDetail(int reportId, int customerId);
    }
}
