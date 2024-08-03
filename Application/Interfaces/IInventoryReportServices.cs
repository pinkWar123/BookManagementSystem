using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.InventoryReport;
using BookManagementSystem.Application.Queries;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IInventoryReportService
    {
        Task<InventoryReportDto> CreateInventoryReport(CreateInventoryReportDto _createInventoryReportDto);
        Task<InventoryReportDto> UpdateInventoryReport(int reportId, UpdateInventoryReportDto _updateInventoryReportDto);
        Task<InventoryReportDto> GetInventoryReportById(int reportId);
        Task<bool> DeleteInventoryReport(int reportId);

        Task<IEnumerable<InventoryReportDto>> GetAllDebtReports(InventoryReportQuery debtReportQuery);
    }
}
