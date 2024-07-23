using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.InventoryReportDetail;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IInventoryReportDetailService
    {
        Task<InventoryReportDetailDto> CreateInventoryReportDetail(CreateInventoryReportDetailDto _createInventoryReportDetailDto);
        Task<InventoryReportDetailDto> UpdateInventoryReportDetail(string reportId,string BookID, UpdateInventoryReportDetailDto _updateInventoryReportDetailDto);
        Task<InventoryReportDetailDto> GetInventoryReportDetailById(string reportId ,string BookID );
        Task<bool> DeleteInventoryReportDetail(string reportId ,string BookID);
    }
}
