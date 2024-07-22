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
        Task<DebtReportDto> UpdateDebtReport(string reportId, UpdateDebtReportDto updateDebtReportDto);
        Task<DebtReportDto> GetDebtReportById(string reportId);
        // Task<IEnumerable<DebtReportDto>> GetAllDebtReports();
        Task<bool> DeleteDebtReport(string reportId);
    }
}