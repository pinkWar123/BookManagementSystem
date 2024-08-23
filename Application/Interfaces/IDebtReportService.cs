using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.DebtReport;
using BookManagementSystem.Application.Dtos.DebtReportDetail;
using BookManagementSystem.Application.Queries;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IDebtReportService
    {
        Task<DebtReportDto> CreateNewDebtReport(CreateDebtReportDto createDebtReportDto);
        Task<DebtReportDto> UpdateDebtReport(int reportId, UpdateDebtReportDto updateDebtReportDto);
        Task<DebtReportDto> GetDebtReportById(int reportId);
        Task<IEnumerable<DebtReportDto>> GetAllDebtReports(DebtReportQuery debtReportQuery);
        Task<bool> DeleteDebtReport(int reportId);
        Task<int> GetReportIdByMonthYear(int month, int year);
        Task<bool> DebtReportExists(int month, int year);
        // Task<IEnumerable<AllDebtReportDetailDto>> GetAllDebtReportDetailsByMonth(int month, int year);
        Task<IEnumerable<GetAllDebtReportDto>> GetAllDebtReportDetails(DebtReportQuery debtReportQuery);
    }
}
