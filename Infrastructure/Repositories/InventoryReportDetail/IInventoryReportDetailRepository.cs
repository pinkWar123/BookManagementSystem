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
        Task<Domain.Entities.InventoryReportDetail?> UpdateAsync<TUpdateDto>(int reportId, int bookId, TUpdateDto entity) where TUpdateDto : class;
        Task<Domain.Entities.InventoryReportDetail?> GetByIdAsync(int id, int id2);

        Task<List<Domain.Entities.InventoryReportDetail>> GetListInventoryReportDetailsByIdAsync(int id);
        IQueryable<Domain.Entities.InventoryReportDetail>? GetValuesByQuery(QueryObject queryObject);
    }
}
