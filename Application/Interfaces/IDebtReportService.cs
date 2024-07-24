using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.DebtReport;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IDebtReportService
    {
        Task<DebtReportDto> CreateNewDebtReport(CreateDebtReportDto createDebtReportDto);
        Task<DebtReportDto> UpdateDebtReport(int reportId, UpdateDebtReportDto updateDebtReportDto);
        Task<DebtReportDto> GetDebtReportById(int reportId);
        // Task<IEnumerable<DebtReportDto>> GetAllDebtReports();
        Task<bool> DeleteDebtReport(int reportId);
    }
}
