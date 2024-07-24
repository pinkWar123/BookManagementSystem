using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.InventoryReport;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IInventoryReportService
    {
        Task<InventoryReportDto> CreateInventoryReport(CreateInventoryReportDto _createInventoryReportDto);
        Task<InventoryReportDto> UpdateInventoryReport(string reportId, UpdateInventoryReportDto _updateInventoryReportDto);
        Task<InventoryReportDto> GetInventoryReportById(string reportId);
        Task<bool> DeleteInventoryReport(string reportId);
    }
}
