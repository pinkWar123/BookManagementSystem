using BookManagementSystem.Application.Dtos.InventoryReportDetail;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.InventoryReportDetail
{
    public class InventoryReportDetailRepository : GenericRepository<Domain.Entities.InventoryReportDetail>, IInventoryReportDetailRepository
    {
        public InventoryReportDetailRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {}
        public async Task<Domain.Entities.InventoryReportDetail> UpdateAsync(string id, string id2, UpdateInventoryReportDetailDto entity)
        {
            var existingEntity = await GetByIdAsync(id, id2);
            if (existingEntity == null)
                return null;

            var updateDtoProperties = typeof(InventoryReportDetailDto).GetProperties();
            var entityProperties = typeof(InventoryReportDetailDto).GetProperties().Select(p => p.Name).ToHashSet();

            // Ensure all properties in updateDto exist in the entity class
            foreach (var property in updateDtoProperties)
            {
                if (!entityProperties.Contains(property.Name))
                {
                    throw new Exception($"Property {property.Name} does not exist in the entity class.");
                }
            }

            foreach (var property in updateDtoProperties)
            {
                var newValue = property.GetValue(entity);
                var existingProperty = existingEntity.GetType().GetProperty(property.Name);
                if (existingProperty != null && existingProperty.CanWrite)
                {
                    existingProperty.SetValue(existingEntity, newValue);
                }
            }

            return existingEntity;
        }

        public async Task<Domain.Entities.InventoryReportDetail?> GetByIdAsync(string id, string id2)
        {

            return await GetContext().FindAsync(id, id2);
        }
    }

}