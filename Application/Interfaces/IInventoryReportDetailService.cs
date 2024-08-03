using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.InventoryReportDetail;
using BookManagementSystem.Application.Queries;

namespace BookManagementSystem.Application.Interfaces
{
    public interface IInventoryReportDetailService
    {
        Task<InventoryReportDetailDto> CreateInventoryReportDetail(CreateInventoryReportDetailDto _createInventoryReportDetailDto);
        Task<InventoryReportDetailDto> UpdateInventoryReportDetail(int reportId,int BookID, UpdateInventoryReportDetailDto _updateInventoryReportDetailDto);
        Task<InventoryReportDetailDto> GetInventoryReportDetailById(int reportId ,int BookID );
        Task<bool> DeleteInventoryReportDetail(int reportId ,int BookID);
        Task<bool> DeleteAllInventoryReportDetailWithReportId(int reportId);

        Task<IEnumerable<InventoryReportDetailDto>> GetAllInventoryReportDetails(InventoryReportDetailQuery inventoryReportDetailQuery);
    }
}
