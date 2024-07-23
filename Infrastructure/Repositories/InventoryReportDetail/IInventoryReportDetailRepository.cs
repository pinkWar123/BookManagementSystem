using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.InventoryReportDetail;
using BookManagementSystem.Data.Repositories;
using BookManagementSystem.Domain.Entities;


namespace BookManagementSystem.Infrastructure.Repositories.InventoryReportDetail
{
    public interface IInventoryReportDetailRepository : IGenericRepository<Domain.Entities.InventoryReportDetail>
    {
        Task<Domain.Entities.InventoryReportDetail> UpdateAsync(string id, string id2, UpdateInventoryReportDetailDto entity);
        Task<Domain.Entities.InventoryReportDetail?> GetByIdAsync(string id, string id2);
    }
}
