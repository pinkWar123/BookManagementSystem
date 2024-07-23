using System.Collections.Generic;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.DebtReportDetail;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IDebtReportDetailService
    {
        Task<DebtReportDetailDto> CreateNewDebtReportDetail(CreateDebtReportDetailDto createDebtReportDetailDto);
        Task<DebtReportDetailDto> UpdateDebtReportDetail(string reportId, string customerId, UpdateDebtReportDetailDto updateDebtReportDetailDto);
        Task<DebtReportDetailDto> GetDebtReportDetailById(string reportId, string customerId);
        // Task<IEnumerable<DebtReportDetailDto>> GetAllDebtReportDetails();
        Task<bool> DeleteDebtReportDetail(string reportId, string customerId);
    }
}
